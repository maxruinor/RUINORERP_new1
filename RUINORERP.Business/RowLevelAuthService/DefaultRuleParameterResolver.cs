using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 规则参数解析器 - 静态辅助类
    /// 支持系统内置的参数类型解析，无需实例化
    /// </summary>
    public static class RuleParameterResolver
    {
        /// <summary>
        /// 获取所有支持的参数类型
        /// </summary>
        /// <returns>参数名称到参数类型的字典</returns>
        public static readonly Dictionary<string, Type> SupportedParameters = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "UserId", typeof(long) },
            { "EmployeeId", typeof(long) },
            { "Employee_ID", typeof(long) },
            { "RoleId", typeof(long) },
            { "DepartmentId", typeof(long) },
            { "Department_ID", typeof(long) },
            { "CurrentDate", typeof(DateTime) },
            { "CurrentTime", typeof(DateTime) },
            { "CurrentYear", typeof(int) },
            { "CurrentMonth", typeof(int) },
            { "CurrentDay", typeof(int) }
        };

        /// <summary>
        /// 解析指定参数的值
        /// </summary>
        /// <param name="parameterName">参数名称(不区分大小写)</param>
        /// <param name="context">权限上下文</param>
        /// <returns>参数值的字符串表示</returns>
        public static string ResolveParameter(string parameterName, RowAuthContext context)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // 使用默认解析逻辑
            string result = parameterName.ToUpper() switch
            {
                "USERID" => context.UserId.ToString(),
                "EMPLOYEEID" or "EMPLOYEE_ID" => context.EmployeeId?.ToString() ?? "0",
                "ROLEID" => context.RoleId.ToString(),
                "DEPARTMENTID" or "DEPARTMENT_ID" => context.DepartmentId?.ToString() ?? "0",
                "CURRENTDATE" => DateTime.Now.ToString("yyyy-MM-dd"),
                "CURRENTTIME" => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                "CURRENTYEAR" => DateTime.Now.Year.ToString(),
                "CURRENTMONTH" => DateTime.Now.Month.ToString(),
                "CURRENTDAY" => DateTime.Now.Day.ToString(),
                _ => throw new ArgumentException($"不支持的参数: {parameterName}")
            };

            return result;
        }

        /// <summary>
        /// 检查是否支持指定的参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns>是否支持该参数</returns>
        public static bool IsParameterSupported(string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                return false;
            }

            return SupportedParameters.ContainsKey(parameterName);
        }
    }
}
