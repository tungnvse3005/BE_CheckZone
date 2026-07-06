using System.Threading.Tasks;
using CheckZone.Api.Entities;

namespace CheckZone.Api.Services
{
    public interface IDiscordNotificationService
    {
        Task SendScamReportNotificationAsync(ScamReport report);
    }
}
