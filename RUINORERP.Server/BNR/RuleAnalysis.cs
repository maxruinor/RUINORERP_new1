using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.Server.BNR
{
    public class RuleAnalysis
    {
        [ThreadStatic]
        private static Stack<char> stack;

        /// <summary>
        /// 执行规则分析，解析包含嵌套表达式的规则字符串
        /// 支持解析形如{{redis:{S:销售订单}{D:yyMM}/000}}的嵌套表达式
        /// </summary>
        /// <param name="rule">规则字符串</param>
        /// <returns>解析后的表达式数组</returns>
        public static string[] Execute(string rule)
        {
            // 添加空值检查
            if (string.IsNullOrEmpty(rule))
            {
                return new string[0];
            }
            
            List<string> items = new List<string>();
            StringBuilder sb = new StringBuilder();
            
            int i = 0;
            while (i < rule.Length)
            {
                // 查找以{{开头的表达式
                if (i < rule.Length - 1 && rule[i] == '{' && rule[i + 1] == '{')
                {
                    // 跳过第一个{{
                    i += 2;
                    int startIndex = i;
                    int braceDepth = 1;
                    
                    // 寻找匹配的}}，同时正确处理嵌套花括号
                    while (i < rule.Length && braceDepth > 0)
                    {
                        if (i < rule.Length - 1 && rule[i] == '{' && rule[i + 1] == '{')
                        {
                            braceDepth++;
                            i += 2;
                        }
                        else if (i < rule.Length - 1 && rule[i] == '}' && rule[i + 1] == '}')
                        {
                            braceDepth--;
                            i += 2;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    
                    // 如果找到了完整的表达式
                    if (braceDepth == 0 && startIndex < i - 2)
                    {
                        // 添加表达式内容（不包含{{和}}）
                        items.Add(rule.Substring(startIndex, i - 2 - startIndex));
                    }
                    else
                    {
                        // 没有找到匹配的}}，跳过当前字符
                        i++;
                    }
                }
                else
                {
                    // 跳过非表达式字符
                    i++;
                }
            }
            
            return items.ToArray();
        }

        /// <summary>
        /// 从参数字符串中提取属性类型和值
        /// 格式为"类型:值"
        /// </summary>
        /// <param name="value">参数字符串</param>
        /// <returns>包含类型和值的字符串数组</returns>
        public static string[] GetProperties(string value)
        {
            // 添加空值检查
            if (string.IsNullOrEmpty(value))
            {
                return new string[] { string.Empty, string.Empty };
            }
            
            int index = value.IndexOf(':');
            
            // 如果没有找到冒号，返回默认值以避免异常
            if (index == -1)
            {
                return new string[] { value, string.Empty };
            }
            
            // 确保不会出现索引越界异常
            if (index >= value.Length - 1)
            {
                return new string[] { value.Substring(0, index), string.Empty };
            }
            
            return new string[] { value.Substring(0, index), value.Substring(index + 1) };
        }
    }
}
