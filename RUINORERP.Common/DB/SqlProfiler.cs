using RUINORERP.Common.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.Common.DB
{
    /// <summary>
    /// SqlSugar 打印SQL语句参数格式化帮助类
    /// 【使用方式】：在需要打印SQL语句的地方，如 Startup，将
    /// App.PrintToMiniProfiler("SqlSugar1", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
    /// 替换为
    /// App.PrintToMiniProfiler("SqlSugar", "Info", FormatHelper.FormatParam(sql, pars));
    /// </summary>
    public class SqlProfiler
    {
        /// <summary>
        /// 格式化参数拼接成完整的SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static string FormatParam(string sql, SugarParameter[] pars)
        {
            if (pars == null || pars.Length == 0) return sql;

            // 逆向遍历，防止 @TenantId1/@TenantId10 类问题
            for (int i = pars.Length - 1; i >= 0; i--)
            {
                var p = pars[i];
                if (p == null) continue;

                // 构造匹配“完整参数名”的正则，例如 @UserPersonalizedID\b
                var regex = new Regex($@"\{Regex.Escape(p.ParameterName)}\b", RegexOptions.None, TimeSpan.FromSeconds(1));

                string valueLiteral;

                switch (p.DbType)
                {
                    case System.Data.DbType.String:
                    case System.Data.DbType.DateTime:
                    case System.Data.DbType.Date:
                    case System.Data.DbType.Time:
                    case System.Data.DbType.DateTime2:
                    case System.Data.DbType.DateTimeOffset:
                    case System.Data.DbType.Guid:
                    case System.Data.DbType.VarNumeric:
                    case System.Data.DbType.AnsiStringFixedLength:
                    case System.Data.DbType.StringFixedLength:
                        valueLiteral = "'" + p.Value?.ToString()?.Replace("'", "''") + "'";
                        break;

                    case System.Data.DbType.Boolean:
                        valueLiteral = Convert.ToBoolean(p.Value) ? "1" : "0";
                        break;

                    default:
                        valueLiteral = p.Value?.ToString() ?? "NULL";
                        break;
                }

                sql = regex.Replace(sql, valueLiteral);
            }

            return sql;
        }




        /// <summary>
        /// 格式化参数拼接成完整的SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static string ParameterFormat(string sql, object pars)
        {
            SugarParameter[] param = (SugarParameter[])pars;
            return FormatParam(sql, param);
        }
    }
}

