using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace RUINORERP.UI.ATechnologyStack
{
    public interface ICustomValidationRule
    {
        bool Validate(string input, out string errorMessage);
    }

    // 2. 实现具体的验证规则类
    public class EmailValidationRule : ICustomValidationRule
    {
        public bool Validate(string input, out string errorMessage)
        {
            errorMessage = string.Empty;
            // 使用正则表达式验证邮箱格式
            return Regex.IsMatch(input, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
    }

    public class AmountValidationRule : ICustomValidationRule
    {
        public bool Validate(string input, out string errorMessage)
        {
            errorMessage = string.Empty;
            // 使用正则表达式验证金额格式
            return Regex.IsMatch(input, @"^\d+(\.\d{1,2})?$");
        }
    }




    /// <summary>
    /// 字符串通用长度 + 汉字数量限制 验证规则
    /// </summary>
    public class LengthAndChineseValidationRule : ICustomValidationRule
    {
        /// <summary>字符串最小长度（含）</summary>
        public int MinLength { get; set; } = 5;

        /// <summary>字符串最大长度（含）</summary>
        public int MaxLength { get; set; } = 500;

        /// <summary>允许的最大汉字数（含）</summary>
        public int MaxChineseChars { get; set; } = 500;

        /// <summary>
        /// 执行验证，失败时通过 out 参数返回错误信息
        /// </summary>
        public bool Validate(string input, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "输入不能为空或仅空白字符。";
                return false;
            }

            int totalLen = input.Length;
            if (totalLen < MinLength || totalLen > MaxLength)
            {
                errorMessage = $"总长度必须在 {MinLength} 到 {MaxLength} 个字符之间（当前 {totalLen}）。";
                return false;
            }

            int chineseCount = Regex.Matches(input, @"\p{IsCJKUnifiedIdeographs}").Count;
            if (chineseCount > MaxChineseChars)
            {
                errorMessage = $"汉字数量不能超过 {MaxChineseChars} 个（当前 {chineseCount}）。";
                return false;
            }

            errorMessage = null;
            return true;
        }

        // 为了兼容 ICustomValidationRule 接口
        public bool Validate(string input) => Validate(input, out _);
    }
}
