using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CheckZone.Api.Data;
using CheckZone.Api.Entities;
using CheckZone.Api.DTOs;

namespace CheckZone.Api.Controllers
{
    [ApiController]
    [Route("api/admin/settings")]
    [Authorize]
    public class SystemSettingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SystemSettingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var settings = await _context.SystemConfigurations.FirstOrDefaultAsync(s => s.Id == 1);
            if (settings == null)
            {
                settings = new SystemConfiguration
                {
                    Id = 1,
                    RequireEvidence = true,
                    AutoApprove = false,
                    MinInsurance = 10000000.00m,
                    AdminName = "Ban điều hành Check Zone Việt Nam",
                    AdminEmail = "support@checkzone.vn",
                    TelegramBotToken = null,
                    DiscordWebhookUrl = null
                };
                _context.SystemConfigurations.Add(settings);
                await _context.SaveChangesAsync();
            }
            return Ok(settings);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateSystemSettingsDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Dữ liệu cấu hình không hợp lệ." });
            }

            var settings = await _context.SystemConfigurations.FirstOrDefaultAsync(s => s.Id == 1);
            if (settings == null)
            {
                settings = new SystemConfiguration { Id = 1 };
                _context.SystemConfigurations.Add(settings);
            }

            settings.RequireEvidence = dto.RequireEvidence;
            settings.AutoApprove = dto.AutoApprove;
            settings.MinInsurance = dto.MinInsurance;
            settings.AdminName = dto.AdminName;
            settings.AdminEmail = dto.AdminEmail;
            settings.TelegramBotToken = dto.TelegramBotToken;
            settings.DiscordWebhookUrl = dto.DiscordWebhookUrl;

            await _context.SaveChangesAsync();
            return Ok(settings);
        }
    }
}
