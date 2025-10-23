using System;

namespace RUINORERP.Model.ConfigModel
{
    /// <summary>
    /// 敏感配置特性
    /// 用于标记配置模型中的敏感字段，这些字段在保存时会被加密
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveConfigAttribute : Attribute
    {
        /// <summary>
        /// 是否在UI中显示掩码
        /// </summary>
        public bool MaskInUI { get; set; }

        /// <summary>
        /// 掩码字符
        /// </summary>
        public char MaskChar { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SensitiveConfigAttribute()
        {
            MaskInUI = true;
            MaskChar = '*';
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maskInUI">是否在UI中显示掩码</param>
        public SensitiveConfigAttribute(bool maskInUI)
        {
            MaskInUI = maskInUI;
            MaskChar = '*';
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maskInUI">是否在UI中显示掩码</param>
        /// <param name="maskChar">掩码字符</param>
        public SensitiveConfigAttribute(bool maskInUI, char maskChar)
        {
            MaskInUI = maskInUI;
            MaskChar = maskChar;
        }
    }
}