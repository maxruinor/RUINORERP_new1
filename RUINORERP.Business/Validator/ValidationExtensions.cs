using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// 自定义长度验证，中文按2个字符计算，英文及数字按1个计算
        /// </summary>
        public static IRuleBuilderOptions<T, string> MaximumMixedLength<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int maxLength)
        {
            return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
            {
                if (string.IsNullOrEmpty(value))
                    return;

                // 计算混合长度（中文算2，英文数字算1）
                int mixedLength = CalculateMixedLength(value);

                if (mixedLength > maxLength)
                {
                    context.AddFailure($"备注:不能超过最大长度{maxLength}（中文算2个字符，英文/数字算1个）。");
                }
            });
        }

        /// <summary>
        /// 计算混合长度的核心方法
        /// </summary>
        private static int CalculateMixedLength(string value)
        {
            int length = 0;
            foreach (char c in value)
            {
                // 判断是否为中文字符（Unicode 范围）
                if (c >= 0x4E00 && c <= 0x9FA5)
                {
                    length += 2; // 中文字符算2个长度
                }
                else
                {
                    length += 1; // 其他字符算1个长度
                }
            }
            return length;
        }
    }
}
