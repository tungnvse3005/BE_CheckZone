using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CheckZone.Api.Services;

namespace CheckZone.Api.Controllers
{
    public class ContactRequestDto
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/public/contact")]
    public class ContactController : ControllerBase
    {
        private readonly IDiscordNotificationService _discordNotificationService;

        public ContactController(IDiscordNotificationService discordNotificationService)
        {
            _discordNotificationService = discordNotificationService;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactMessage([FromBody] ContactRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Trigger Discord Webhook Notification asynchronously
            _ = Task.Run(() => _discordNotificationService.SendContactNotificationAsync(dto.Name, dto.Email, dto.Message));

            return Ok(new { message = "Thông điệp liên hệ đã được gửi thành công đến Ban vận hành." });
        }
    }
}
