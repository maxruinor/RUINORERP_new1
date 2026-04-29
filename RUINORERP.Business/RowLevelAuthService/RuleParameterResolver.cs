using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 规则参数解析器
    /// 负责解析权限规则中的参数占位符
    /// </summary>
    public static class RuleParameterResolver
    {
        private static readonly Dictionary<string, Func<RowAuthContext, string>> _parameterResolvers = 
            new Dictionary<string, Func<RowAuthContext, string>>(StringComparer.OrdinalIgnoreCase);

        static RuleParameterResolver()
        {
            // 注册内置参数解析器
            RegisterParameter("UserId", ctx => ctx.UserId.ToString());
            RegisterParameter("RoleId", ctx => ctx.RoleId.ToString());
            RegisterParameter("EmployeeId", ctx => ctx.EmployeeId?.ToString() ?? "0");
            RegisterParameter("DepartmentId", ctx => ctx.DepartmentId?.ToString() ?? "0");
            RegisterParameter("MenuId", ctx => ctx.MenuId.ToString());
            RegisterParameter("EntityName", ctx => ctx.EntityName ?? string.Empty);
            
            // 注册角色列表参数(用于IN查询)
            RegisterParameter("RoleIds", ctx => 
                ctx.RoleIds != null && ctx.RoleIds.Count > 0 
                    ? string.Join(",", ctx.RoleIds) 
                    : "0");
            
            // ✅ 新增: 项目组ID列表参数(用于IN查询)
            RegisterParameter("ProjectGroupIds", ctx => 
                ctx.ProjectGroupIds != null && ctx.ProjectGroupIds.Count > 0 
                    ? string.Join(",", ctx.ProjectGroupIds) 
                    : "0");
            
            // ✅ 新增: 单个项目组ID(取第一个,用于简单场景)
            RegisterParameter("ProjectGroupId", ctx => 
                ctx.ProjectGroupIds != null && ctx.ProjectGroupIds.Count > 0 
                    ? ctx.ProjectGroupIds[0].ToString() 
                    : "0");
        }

        /// <summary>
        /// 注册自定义参数解析器
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="resolver">解析函数</param>
        public static void RegisterParameter(string parameterName, Func<RowAuthContext, string> resolver)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName));
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver));

            _parameterResolvers[parameterName] = resolver;
        }

        /// <summary>
        /// 解析单个参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="context">权限上下文</param>
        /// <returns>解析后的值</returns>
        public static string ResolveParameter(string parameterName, RowAuthContext context)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                return string.Empty;

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (_parameterResolvers.TryGetValue(parameterName, out var resolver))
            {
                try
                {
                    string value = resolver(context);
                    return SanitizeParameterValue(value);
                }
                catch (Exception ex)
                {
                    // 记录日志但不抛出异常,避免影响主流程
                    System.Diagnostics.Debug.WriteLine($"解析参数 {parameterName} 失败: {ex.Message}");
                    return "0"; // 返回安全默认值
                }
            }

            // 尝试从扩展属性中获取
            if (context.ExtendedProperties != null && 
                context.ExtendedProperties.TryGetValue(parameterName, out var extValue))
            {
                return SanitizeParameterValue(extValue?.ToString() ?? "0");
            }

            // 未知参数返回0
            return "0";
        }

        /// <summary>
        /// 检查参数是否受支持
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns>是否支持</returns>
        public static bool IsParameterSupported(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                return false;

            return _parameterResolvers.ContainsKey(parameterName);
        }

        /// <summary>
        /// 获取所有支持的参数名称
        /// </summary>
        /// <returns>参数名称列表</returns>
        public static List<string> GetSupportedParameters()
        {
            return new List<string>(_parameterResolvers.Keys);
        }

        /// <summary>
        /// 清理参数值,防止SQL注入
        /// </summary>
        /// <param name="value">原始值</param>
        /// <returns>清理后的值</returns>
        private static string SanitizeParameterValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "0";

            // 只允许数字、逗号(用于IN列表)、点和下划线
            var sanitized = System.Text.RegularExpressions.Regex.Replace(value, @"[^0-9,\._\-]", "");
            
            // 如果清理后为空,返回0
            return string.IsNullOrEmpty(sanitized) ? "0" : sanitized;
        }
    }
}
