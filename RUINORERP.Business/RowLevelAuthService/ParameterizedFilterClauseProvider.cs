using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 参数化过滤条件提供者 - 静态辅助类
    /// 支持在过滤条件中使用动态参数占位符
    /// </summary>
    public static class ParameterizedFilterHelper
    {
        private static readonly Regex ParameterRegex = new Regex(@"\{(\w+)\}", RegexOptions.Compiled);

        /// <summary>
        /// 解析并替换过滤条件中的参数占位符
        /// </summary>
        /// <param name="template">过滤条件模板</param>
        /// <param name="context">权限上下文</param>
        /// <returns>解析后的过滤条件</returns>
        public static string ResolveFilterTemplate(string template, RowAuthContext context)
        {
            if (string.IsNullOrEmpty(template))
            {
                return string.Empty;
            }

            if (context == null)
            {
                return template;
            }

            try
            {
                var result = template;
                var matches = ParameterRegex.Matches(template);

                foreach (Match match in matches)
                {
                    if (match.Groups.Count > 1)
                    {
                        string parameterName = match.Groups[1].Value;
                        string placeholder = match.Groups[0].Value;

                        if (RuleParameterResolver.IsParameterSupported(parameterName))
                        {
                            string value = RuleParameterResolver.ResolveParameter(parameterName, context);
                            result = result.Replace(placeholder, value);
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return template; // 返回原始模板
            }
        }

        /// <summary>
        /// 检查过滤条件模板是否包含参数占位符
        /// </summary>
        /// <param name="template">过滤条件模板</param>
        /// <returns>是否包含参数占位符</returns>
        public static bool ContainsParameters(string template)
        {
            if (string.IsNullOrEmpty(template))
            {
                return false;
            }

            return ParameterRegex.IsMatch(template);
        }
    }
}
