using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ConfigModel
{
    /// <summary>
    /// 功能配置
    /// </summary>
    public class FunctionConfiguration
    {
        /// <summary>
        /// 启用行级权限控制
        /// </summary>
        public bool EnableRowLevelAuth { get; set; } = false;
    }
}
