using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Global;

namespace RUINORERP.Business.CommService
{
    // 审计日志帮助类（适配原有接口，逐步迁移到服务）
    public class AuditLogHelper : IExcludeFromRegistration
    {
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<AuditLogHelper> _logger;

        // 改为公共构造函数，使用依赖注入代替直接引用MainForm
        public AuditLogHelper(IAuditLogService auditLogService, ILogger<AuditLogHelper> logger)
        {
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            try
            {
                await _auditLogService.LogAsync(action, entity, description);
            }
            catch (Exception ex)
            {
                // 使用ILogger代替直接引用MainForm.Instance
                _logger.LogError(ex, "审计日志记录失败");
            }
        }

        public void CreateAuditLog<T>(string action, T entity) where T : class
        {
            CreateAuditLog(action, entity, "");
        }

        public async Task CreateAuditLog(string action, string description)
        {
            try
            {
                await _auditLogService.LogAsync(action, description);
            }
            catch (Exception ex)
            {
                // 使用ILogger代替直接引用MainForm.Instance
                _logger.LogError(ex, "审计日志记录失败");
            }
        }



    }
}
