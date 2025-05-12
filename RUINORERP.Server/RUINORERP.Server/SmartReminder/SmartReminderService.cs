using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    public class SmartReminderService
    {
        public virtual Task<bool> CheckRulesAsync()
        {
            return Task.FromResult(true);
        }
        public virtual Task<bool> HandleUserResponse()
        {
            return Task.FromResult(true);
        }
    }
}