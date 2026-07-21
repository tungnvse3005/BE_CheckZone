using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CheckZone.Api.Entities;

namespace CheckZone.Api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Apply migrations automatically
            context.Database.Migrate();

            // Seed ScamReports if not already present (idempotent seeding)
            var reports = new List<ScamReport>
            {
                new ScamReport
                {
                    Id = "SR-2026-001",
                    Name = "Nguyễn Văn T*** (Giả danh Công an)",
                    Phone = "098xxxx567",
                    BankName = "Vietcombank",
                    AccountNumber = "101xxxxxx890",
                    Desc = "Đối tượng gọi điện xưng là Công an phường, yêu cầu cài đặt ứng dụng Dịch vụ công giả mạo đuôi .apk. Sau khi cài, ứng dụng chiếm quyền điều khiển màn hình điện thoại (Accessibility Service) và tự động thực hiện lệnh chuyển tiền lúc nửa đêm.",
                    Type = "Cài app giả mạo / Chiếm quyền điều khiển",
                    Amount = 150000000.00m,
                    Status = "Đã phê duyệt",
                    Victim = "Trần Thị H.",
                    Tags = new List<string> { "Dịch vụ công", "App giả mạo", "Chiếm quyền" },
                    Facebook = "",
                    Images = new List<string> { "https://i.ibb.co/placeholder1.jpg" },
                    CreatedAt = DateTime.Parse("2026-06-15T08:30:00Z").ToUniversalTime(),
                    Category = ScamCategory.FinancialScam
                },
                new ScamReport
                {
                    Id = "SR-2026-002",
                    Name = "Nhóm Telegram (Việc làm online)",
                    Phone = "093xxxx111",
                    BankName = "Techcombank",
                    AccountNumber = "190xxxxxx333",
                    Desc = "Tuyển cộng tác viên làm nhiệm vụ thanh toán đơn hàng Shopee/Tiktok để nhận hoa hồng. Vài đơn đầu (vài trăm ngàn) trả hoa hồng đầy đủ để tạo niềm tin. Đến nhiệm vụ lớn (vài chục triệu), hệ thống báo 'sai cú pháp' và yêu cầu nạp thêm tiền để rút. Nạn nhân mất trắng.",
                    Type = "Lừa đảo việc làm / Làm nhiệm vụ",
                    Amount = 85000000.00m,
                    Status = "Đã phê duyệt",
                    Victim = "Ẩn danh",
                    Tags = new List<string> { "Việc làm online", "Shopee", "Nhiệm vụ" },
                    Facebook = "https://facebook.com/fake.tuyencongnhan",
                    Images = new List<string> { "https://i.ibb.co/placeholder2.jpg" },
                    CreatedAt = DateTime.Parse("2026-06-18T14:15:00Z").ToUniversalTime(),
                    Category = ScamCategory.FinancialScam
                },
                new ScamReport
                {
                    Id = "SR-2026-003",
                    Name = "Lê Thị C*** (Bán vé máy bay rẻ)",
                    Phone = "091xxxx999",
                    BankName = "MB Bank",
                    AccountNumber = "091xxxxxx999",
                    Desc = "Lập Fanpage giả mạo đại lý du lịch, chạy quảng cáo bán combo du lịch / vé máy bay giá siêu rẻ mùa cao điểm. Yêu cầu chuyển khoản cọc 50%. Sau khi nhận tiền liền chặn Facebook, khóa Zalo.",
                    Type = "Bán hàng online / Lừa đảo tiền cọc",
                    Amount = 12000000.00m,
                    Status = "Đã phê duyệt",
                    Victim = "Nguyễn Minh T.",
                    Tags = new List<string> { "Vé máy bay", "Combo du lịch", "Lừa cọc" },
                    Facebook = "https://facebook.com/fake.phongvedulich",
                    Images = new List<string>(),
                    CreatedAt = DateTime.Parse("2026-06-19T09:45:00Z").ToUniversalTime(),
                    Category = ScamCategory.FinancialScam
                },
                new ScamReport
                {
                    Id = "SR-2026-004",
                    Name = "Sàn chứng khoán giả mạo (FX/Crypto)",
                    Phone = "",
                    BankName = "VietinBank",
                    AccountNumber = "100xxxxxx555",
                    Desc = "Làm quen qua Tinder/Zalo, xây dựng hình ảnh doanh nhân thành đạt. Dụ dỗ nạn nhân tham gia sàn giao dịch ngoại hối/crypto giả mạo. Sàn cho rút lãi lần đầu, nhưng sau đó báo 'tài khoản bị đóng băng', yêu cầu đóng thuế 20% để rút tiền gốc.",
                    Type = "Đầu tư tài chính / Tiền ảo / Mạo danh",
                    Amount = 450000000.00m,
                    Status = "Đã phê duyệt",
                    Victim = "Phạm Văn D.",
                    Tags = new List<string> { "Tinder", "Đầu tư", "Tiền ảo", "Forex" },
                    Facebook = "",
                    Images = new List<string>(),
                    CreatedAt = DateTime.Parse("2026-06-20T19:20:00Z").ToUniversalTime(),
                    Category = ScamCategory.FinancialScam
                },
                new ScamReport
                {
                    Id = "SR-2026-005",
                    Name = "Kẻ gian (Hack Facebook mượn tiền)",
                    Phone = "",
                    BankName = "BIDV",
                    AccountNumber = "105xxxxxx777",
                    Desc = "Đối tượng hack Facebook của người thân, nhắn tin mượn tiền gấp để xử lý công việc. Đặc biệt, đối tượng sử dụng công nghệ Deepfake AI để gọi Video Call khoảng 3-5 giây với khuôn mặt và giọng nói giống hệt người thân, khiến nạn nhân tin tưởng và chuyển tiền.",
                    Type = "Hack tài khoản / Deepfake",
                    Amount = 20000000.00m,
                    Status = "Đã phê duyệt",
                    Victim = "Trần T. M.",
                    Tags = new List<string> { "Hack Facebook", "Deepfake", "Mượn tiền" },
                    Facebook = "",
                    Images = new List<string>(),
                    CreatedAt = DateTime.Parse("2026-06-21T10:05:00Z").ToUniversalTime(),
                    Category = ScamCategory.FinancialScam
                }
            };

            bool changesMade = false;
            foreach (var report in reports)
            {
                if (!context.ScamReports.Any(r => r.Id == report.Id))
                {
                    context.ScamReports.Add(report);
                    changesMade = true;
                }
            }

            // Seed SystemConfiguration if not present
            if (!context.SystemConfigurations.Any())
            {
                context.SystemConfigurations.Add(new SystemConfiguration
                {
                    Id = 1,
                    RequireEvidence = true,
                    AutoApprove = false,
                    MinInsurance = 10000000.00m,
                    AdminName = "Ban điều hành Check Zone Việt Nam",
                    AdminEmail = "support@checkzone.vn",
                    TelegramBotToken = null,
                    DiscordWebhookUrl = null
                });
                changesMade = true;
            }

            // Seed BlogArticles if not present
            if (!context.BlogArticles.Any())
            {
                context.BlogArticles.AddRange(new List<BlogArticle>
                {
                    new BlogArticle
                    {
                        Id = "ART-1001",
                        Title = "Cảnh báo khẩn cấp chiêu trò giả mạo shipper gọi điện nhận hàng",
                        Category = "Cảnh báo khẩn cấp",
                        CreatedAt = DateTime.UtcNow.AddDays(-2),
                        Slug = "canh-bao-gia-mao-shipper-giao-hang",
                        Status = "Đã đăng",
                        Content = "Thời gian gần đây, xuất hiện tình trạng kẻ gian giả mạo nhân viên giao hàng gọi điện thông báo có đơn hàng COD. Đối tượng yêu cầu chuyển khoản trước hoặc tải ứng dụng giả mạo để xác nhận đơn. Người dân cần hết sức cảnh giác, tuyệt đối không chuyển tiền khi chưa trực tiếp cầm hàng.",
                        Thumbnail = "https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format&fit=crop&w=600&q=80",
                        ThumbnailUrl = "https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format&fit=crop&w=600&q=80"
                    },
                    new BlogArticle
                    {
                        Id = "ART-1002",
                        Title = "HƯỚNG DẪN: An toàn giao dịch trung gian khi mua bán tài khoản MMO",
                        Category = "HƯỚNG DẪN & THỦ THUẬT",
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                        Slug = "huong-dan-giao-dich-trung-gian-mmo",
                        Status = "Đã đăng",
                        Content = "Giao dịch tài khoản game, fanpage và tài nguyên MMO trực tuyến tiềm ẩn nhiều rủi ro bị đổi pass hoặc thu hồi. Hãy luôn sử dụng dịch vụ trung gian từ các thương nhân có ký quỹ uy tín tại Check Zone để bảo vệ tiền của bạn.",
                        Thumbnail = "https://images.unsplash.com/photo-1563986768609-322da13575f3?auto=format&fit=crop&w=600&q=80",
                        ThumbnailUrl = "https://images.unsplash.com/photo-1563986768609-322da13575f3?auto=format&fit=crop&w=600&q=80"
                    },
                    new BlogArticle
                    {
                        Id = "ART-1003",
                        Title = "Nhận biết sàn đầu tư phái sinh lừa đảo cam kết lãi suất 300%/tháng",
                        Category = "Cảnh báo tài chính",
                        CreatedAt = DateTime.UtcNow.AddDays(-8),
                        Slug = "dau-tu-phai-sinh-cam-ket-lai-suat-cao-lua-dao",
                        Status = "Đã đăng",
                        Content = "Các đối tượng lừa đảo thường vẽ ra mô hình đầu tư tài chính siêu lợi nhuận, cam kết bao lỗ và bao rút vốn. Khi số tiền nạp đủ lớn, sàn giả mạo sẽ khóa tài khoản hoặc yêu cầu nộp thêm tiền thuế/phí để rút vốn.",
                        Thumbnail = "https://images.unsplash.com/photo-1611974789855-9c2a0a7236a3?auto=format&fit=crop&w=600&q=80",
                        ThumbnailUrl = "https://images.unsplash.com/photo-1611974789855-9c2a0a7236a3?auto=format&fit=crop&w=600&q=80"
                    }
                });
                changesMade = true;
            }

            if (changesMade)
            {
                context.SaveChanges();
            }
        }
    }
}
