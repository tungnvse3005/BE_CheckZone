using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CheckZone.Api.Data;
using CheckZone.Api.Entities;

namespace CheckZone.Api.Services
{
    public class DiscordNotificationService : IDiscordNotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DiscordNotificationService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DiscordNotificationService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<DiscordNotificationService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task SendScamReportNotificationAsync(ScamReport report)
        {
            string? webhookUrl = null;

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var settings = await context.SystemConfigurations.FindAsync(1);
                    if (settings != null && !string.IsNullOrWhiteSpace(settings.DiscordWebhookUrl))
                    {
                        webhookUrl = settings.DiscordWebhookUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading Discord Webhook URL from database. Falling back to environment variables.");
            }

            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                webhookUrl = _configuration["Discord:WebhookUrl"]
                                 ?? _configuration["DiscordWebhookUrl"]
                                 ?? _configuration["DISCORD__WEBHOOKURL"]
                                 ?? Environment.GetEnvironmentVariable("Discord__WebhookUrl")
                                 ?? Environment.GetEnvironmentVariable("DISCORD_WEBHOOK_URL")
                                 ?? Environment.GetEnvironmentVariable("Discord_WebhookUrl")
                                 ?? Environment.GetEnvironmentVariable("DiscordWebhookUrl");
            }

            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                _logger.LogWarning("[Discord Webhook Warning] Discord Webhook URL is empty or null. Checked database and environment keys.");
                return;
            }

            // Clean any trailing bracket if user copy-pasted it
            webhookUrl = webhookUrl.Trim();
            if (webhookUrl.EndsWith("]"))
            {
                webhookUrl = webhookUrl.Substring(0, webhookUrl.Length - 1).Trim();
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                
                // Format amount to VNĐ
                var formattedAmount = report.Amount.ToString("N0", new System.Globalization.CultureInfo("vi-VN")) + " VNĐ";

                // --- Build evidence image embeds (Discord allows max 10 embeds per message) ---
                // The first image is attached to the main embed via the "image" field.
                // Remaining images (up to 9 more) each get a minimal embed with only an image URL,
                // which Discord renders as a grouped gallery when they share the same URL value.
                var images = report.Images ?? new List<string>();

                // Main embed — contains all report fields
                var mainEmbed = new
                {
                    title = "🚨 PHÁT HIỆN TỐ CÁO LỪA ĐẢO MỚI",
                    description = "Một đơn tố cáo mới vừa được gửi lên hệ thống và đang chờ ban quản trị phê duyệt.",
                    color = 15158332, // Red color hex #e74c3c
                    fields = new[]
                    {
                        new { name = "Mã Đơn", value = report.Id ?? "N/A", inline = true },
                        new { name = "Tên Đối Tượng", value = report.Name ?? "N/A", inline = true },
                        new { name = "Số Điện Thoại", value = !string.IsNullOrEmpty(report.Phone) ? report.Phone : "Không có", inline = true },
                        new { name = "Ngân Hàng", value = report.BankName ?? "N/A", inline = true },
                        new { name = "Số Tài Khoản", value = report.AccountNumber ?? "N/A", inline = true },
                        new { name = "Số Tiền Thiệt Hại", value = formattedAmount, inline = true },
                        new { name = "Nạn Nhân", value = report.Victim ?? "Ẩn danh", inline = true },
                        new { name = "Liên Kết Facebook", value = !string.IsNullOrEmpty(report.Facebook) ? report.Facebook : "Không có", inline = true },
                        new { name = "Chi Tiết Hành Vi", value = report.Desc?.Length > 1000 ? report.Desc.Substring(0, 997) + "..." : (report.Desc ?? "N/A"), inline = false }
                    },
                    // Attach the first evidence image directly to the main embed (null-safe)
                    image = images.Count > 0 ? new { url = images[0] } : null,
                    timestamp = DateTime.UtcNow.ToString("o")
                };

                // Build the embeds list, starting with the main embed
                var embedsList = new List<object> { mainEmbed };

                // Add one extra embed per additional image (indices 1..N), capped so total ≤ 10
                // Discord groups consecutive image-only embeds into a gallery automatically
                for (int i = 1; i < images.Count && embedsList.Count < 10; i++)
                {
                    if (!string.IsNullOrWhiteSpace(images[i]))
                    {
                        embedsList.Add(new { url = images[i], image = new { url = images[i] } });
                    }
                }

                var payload = new
                {
                    username = "CheckZone Monitor",
                    avatar_url = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?auto=format&fit=crop&w=300&q=80",
                    embeds = embedsList
                };

                var content = JsonContent.Create(payload);
                var response = await httpClient.PostAsync(webhookUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully sent Discord notification for scam report {ReportId}", report.Id);
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("[Discord Webhook Error Detail] Status: {StatusCode}, Error: {Error}", response.StatusCode, responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending Discord notification for scam report {ReportId}", report.Id);
            }
        }
    }
}
