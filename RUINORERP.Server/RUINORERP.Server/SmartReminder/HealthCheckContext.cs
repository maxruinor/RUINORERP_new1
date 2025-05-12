using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    using System.Collections.Generic;

    public class HealthCheckContext
    {
        /// <summary>
        /// 健康检查的标签，用于筛选健康检查
        /// </summary>
        public IReadOnlyList<string> Tags { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tags">健康检查的标签</param>
        public HealthCheckContext(IEnumerable<string> tags = null)
        {
            Tags = tags?.ToList() ?? new List<string>();
        }
    }
}
