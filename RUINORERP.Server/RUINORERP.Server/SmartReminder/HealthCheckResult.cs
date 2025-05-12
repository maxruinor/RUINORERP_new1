
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Server.SmartReminder
{
    public class HealthCheckResult
    {
        private HealthStatus HealthStatus;
        private string description;
        private System.Exception exception;
        public HealthCheckResult(HealthStatus healthy, string description, System.Exception exception=null)
        {
            this.HealthStatus = healthy;
            this.description = description;
            this.exception = exception;
        }
    }
}