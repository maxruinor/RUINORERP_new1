using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    // 审计日志配置类
    public class AuditLogOptions
    {
        public int BatchSize { get; set; } = 10; // 批量写入大小
        public int FlushInterval { get; set; } = 3000; // 自动刷新间隔(毫秒)
        public bool EnableAudit { get; set; } = true; // 是否启用审计
    }
}
