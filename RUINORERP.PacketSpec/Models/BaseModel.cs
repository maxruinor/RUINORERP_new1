using System;

using RUINORERP.PacketSpec.Core;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 基础模型类 - 提供通用的时间追踪和验证功能
    /// </summary>
    public abstract class BaseModel : BaseTraceable, IValidatable
    {
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public virtual bool IsValid()
        {
            return CreatedTime <= DateTime.UtcNow &&
                   CreatedTime >= DateTime.UtcNow.AddYears(-1); // 创建时间在1年内
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>模型信息字符串</returns>
        public override string ToString()
        {
            return $"BaseModel[Created:{CreatedTime:yyyy-MM-dd HH:mm:ss} UTC]";
        }
    }
}
