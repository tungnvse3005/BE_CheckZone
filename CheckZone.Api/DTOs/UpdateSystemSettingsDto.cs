namespace CheckZone.Api.DTOs
{
    public class UpdateSystemSettingsDto
    {
        public bool RequireEvidence { get; set; }
        public bool AutoApprove { get; set; }
        public decimal MinInsurance { get; set; }
        public string AdminName { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string? TelegramBotToken { get; set; }
        public string? DiscordWebhookUrl { get; set; }
    }
}
