using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizCodeGeneration
{
    // 序号生成器接口，支持多种生成策略
    public interface ISerialNumberGenerator
    {
        /// <summary>
        /// 根据规则名生成，实际可能是业务类型
        /// </summary>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        string Generate(string ruleName);
    }

    // 生成器类型枚举
    public enum GeneratorType
    {
        Redis,
        Snowflake,
        Database
    }
}
