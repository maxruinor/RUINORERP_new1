using NPinyin;
using RUINORERP.Business.BNR;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// 中文首字母编码参数处理器
    /// 用于将中文字符转换为首字母
    /// 格式: {CN:中文}
    /// 使用NPinyin库实现更准确的中文转拼音功能
    /// </summary>
    [ParameterType("CN")]
    public class ChineseSpellCodeParameter : IParameterHandler
    {
        /// <summary>
        /// 工厂对象，用于处理嵌套表达式
        /// </summary>
        public object Factory { get; set; }

        /// <summary>
        /// 执行中文首字母提取
        /// </summary>
        /// <param name="sb">字符串构建器，用于存储处理结果</param>
        /// <param name="value">需要提取首字母的中文字符串</param>
        public void Execute(StringBuilder sb, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            // 检查值是否包含嵌套表达式（是否包含{和}字符）
            if (value.Contains("{") && value.Contains("}"))
            {
                // 检查是否是有效的嵌套表达式（包含处理器类型）
                if (value.Contains(":"))
                {
                    // 如果包含嵌套表达式，先递归处理嵌套表达式
                    // 构建完整的表达式格式
                    string nestedExpression = "{" + value + "}";
                    // 使用工厂递归处理嵌套表达式
                    BNRFactory factory = Factory as BNRFactory;
                    string processedValue = factory?.Create(nestedExpression) ?? value;
                    value = processedValue;
                }
                else
                {
                    // 如果不是有效的嵌套表达式，移除大括号后直接处理
                    value = value.Replace("{", "").Replace("}", "");
                }
            }

            // 使用NPinyin提取中文首字母并追加到结果中
            try
            {
                string initials = Pinyin.GetInitials(value).ToUpper();
                sb.Append(initials);
            }
            catch
            {
                // 如果NPinyin处理失败，使用默认值
                sb.Append("Z");
            }
        }

        /// <summary>
        /// 提取产品类目名称的首字母（适配编码生成）
        /// 使用NPinyin库实现更准确的中文转拼音功能
        /// </summary>
        /// <param name="categoryName">产品类目名称（如：智能家居设备）</param>
        /// <param name="takeCount">取前N个首字母（默认取全部）</param>
        /// <returns>编码用的首字母串（如：ZNJJ）</returns>
        public static string GetCategoryInitialsForCode(string categoryName, int takeCount = int.MaxValue)
        {
            // 1. 空值/空字符串处理（避免编码异常）
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return string.Empty; // 或返回默认值如"DEFAULT"
            }

            // 2. 提取所有中文字符的首字母（自动忽略数字/符号/空格，统一大写）
            StringBuilder initialsSb = new StringBuilder();
            foreach (char c in categoryName)
            {
                // 只处理中文字符，跳过数字、符号、空格等无用字符
                if (char.IsLetter(c) && c >= '\u4e00' && c <= '\u9fff')
                {
                    // 使用NPinyin提取中文首字母并追加到结果中
                    try
                    {
                        // 确保将字符转换为字符串
                        string charStr = c.ToString();
                        string initial = Pinyin.GetInitials(charStr).ToUpper();
                        initialsSb.Append(initial);
                    }
                    catch
                    {
                        // 如果NPinyin处理失败，使用默认值
                        initialsSb.Append("Z");
                    }
                }
            }

            // 3. 取前N位（若首字母总数不足N位，直接返回全部）
            string initials = initialsSb.ToString();
            return initials.Length > takeCount
                ? initials.Substring(0, takeCount)
                : initials;
        }
    }
}