using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace RUINORERP.Business.CommService
{

    public  class MatchSql
    {
        /// <summary>
        /// 正则匹配不同场景的sql语句  CRUD
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<MatchSqlModel> MatchAllTypeSql(string sql, out string? newSql)
        {
            newSql = sql;
            var result = new List<MatchSqlModel>();
            bool isExistWhere = sql.IndexOf("where") > -1;
            if (sql.StartsWith("select", StringComparison.OrdinalIgnoreCase))
            {
                //校验sql脚本是否存在 双引号
                var regx = new Regex("\\s+(from|join)\\s+\"([^\"]*)\"", RegexOptions.IgnoreCase);
                if (regx.IsMatch(sql))
                {
                    var keys = new string[] { "where", "left", "join", "inner", "group", "full", "order", "right", "on" };
                    //var reg1 = new Regex("\\s+(from|join)\\s+\"([^\"]*)\"\\s+(\\w+)\\s+(where|left|join|inner|group|full|order|right|on)*", RegexOptions.IgnoreCase);
                    var reg1 = new Regex("\\s+(from|join)\\s+\"([^\"]*)\"(\\s+(\\w+)\\s+)?", RegexOptions.IgnoreCase);//不包含as
                    MatchCollection mces1 = reg1.Matches(sql);
                    foreach (Match mc in mces1)
                    {
                        var obj = new MatchSqlModel()
                        {
                            TableName = $"\"{mc.Groups[2].Value}\"",
                            AliasName = string.IsNullOrWhiteSpace(mc.Groups[3].Value) || keys.Where(x => x.Equals(mc.Groups[3].Value.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault() != null ? $"\"{mc.Groups[2].Value}\"" : mc.Groups[3].Value.Trim(),
                            IsWhere = isExistWhere,
                            SqlOpt = "select"
                        };
                        //防止相同（表名+别名）重复追加
                        var currentMod = result.Where(x => x.TableName == obj.TableName).FirstOrDefault();
                        if (currentMod == null)
                        {
                            result.Add(obj);
                        }
                    }
                }
                else
                {
                    //第一次正则解析  带别名
                    var reg1 = new Regex(@"\s+(from|join)\s+(\w+)\s+(\s*(as)*\s*)*(\w+)\s+(where|left|join|inner|group|full|order|right|on)+", RegexOptions.IgnoreCase);
                    MatchCollection mces1 = reg1.Matches(sql);
                    foreach (Match mc in mces1)
                    {
                        if (mc.Groups[2].Value.Trim().Contains(" ") || mc.Groups[5].Value.Trim().Contains(" "))
                            continue;
                        var obj = new MatchSqlModel()
                        {
                            TableName = mc.Groups[2].Value.Trim(),
                            AliasName = mc.Groups[5].Value.Trim(),
                            IsWhere = isExistWhere,
                            SqlOpt = "select"
                        };
                        //防止相同（表名+别名）重复追加
                        var currentMod = result.Where(x => x.TableName == obj.TableName && x.AliasName == obj.AliasName).FirstOrDefault();
                        if (currentMod == null)
                        {
                            result.Add(obj);
                        }
                    }

                    //第二次正则解析  无别名
                    var reg2 = new Regex(@"\s+(from|join)\s+(\w+)\s+(where|left|join|inner|group|full|order|right|on)+", RegexOptions.IgnoreCase);
                    MatchCollection mces2 = reg2.Matches(sql);
                    foreach (Match mc in mces2)
                    {
                        var obj = new MatchSqlModel()
                        {
                            TableName = mc.Groups[2].Value.Trim(),
                            AliasName = mc.Groups[2].Value.Trim(),
                            IsWhere = isExistWhere,
                            SqlOpt = "select"
                        };
                        //防止相同（表名+别名）重复追加
                        var currentMod = result.Where(x => x.TableName == obj.TableName).FirstOrDefault();
                        if (currentMod == null)
                        {
                            result.Add(obj);
                        }
                    }
                    //第三次正则解析 匹配失败时，通过新的正则再次匹配 带别名
                    if (result.Count == 0)
                    {
                        var pattern = @"(?<=((from[ \n\r]+)|(join[ \n\r]+)|(apply[ \n\r]+)))(.?[a-zA-Z0-9_]+)\s(.?[a-zA-Z0-9_]+)";

                        var lst = Regex.Matches(sql, pattern, RegexOptions.IgnoreCase).Cast<Match>().ToList();
                        foreach (Match mc in lst)
                        {
                            if (mc.Groups[1].Value.Trim().Contains(" ") || mc.Groups[2].Value.Trim().Contains(" "))
                                continue;
                            var regexTableNames = mc.Groups[1].Value;
                            var obj = new MatchSqlModel()
                            {
                                TableName = mc.Groups[5].Value,
                                AliasName = mc.Groups[6].Value,
                                IsWhere = isExistWhere,
                                SqlOpt = "select"
                            };
                            //防止相同（表名+别名）重复追加
                            int iRet = result.Where(x => x.TableName == obj.TableName && x.AliasName == obj.AliasName).Count();
                            if (iRet == 0)
                                result.Add(obj);
                        }
                    }
                    //第四次正则解析 匹配失败时，通过新的正则再次匹配 无别名
                    if (result.Count == 0)
                    {
                        var pattern = @"(?<=((from[ \n\r]+)|(join[ \n\r]+)|(apply[ \n\r]+)))(.?[a-zA-Z0-9_]+)";

                        var lst = Regex.Matches(sql, pattern, RegexOptions.IgnoreCase).Cast<Match>().ToList();
                        foreach (Match m in lst)
                        {
                            var regexTableNames = m.Groups[1].Value;
                            var obj = new MatchSqlModel()
                            {
                                TableName = m.Groups[5].Value,
                                AliasName = m.Groups[5].Value,
                                IsWhere = isExistWhere,
                                SqlOpt = "select"
                            };
                            //防止相同（表名+别名）重复追加
                            int iRet = result.Where(x => x.TableName == obj.TableName).Count();
                            if (iRet == 0)
                                result.Add(obj);
                        }
                    }

                }
                newSql = sql;
                return result;
            }
            if (sql.StartsWith("insert", StringComparison.OrdinalIgnoreCase))
            {
                var matcher = Regex.Matches(sql, @"[\s\t]*insert [\s\t\r\n]*(into|)[\s\t\r\n]*[A-Za-z0-9_.""\[\]]*[\s\t\r\n]*", RegexOptions.IgnoreCase).Cast<Match>().ToList();
                foreach (Match? item in matcher)
                {
                    var obj = new MatchSqlModel()
                    {
                       // TableName = item.Value[item.Value.Trim().LastIndexOf(' ')..].Trim(),
                        AliasName = "",
                        IsWhere = isExistWhere,
                        SqlOpt = "insert"
                    };
                    result.Add(obj);
                }
                return result;
            }
            if (sql.StartsWith("update", StringComparison.OrdinalIgnoreCase))
            {
                var matcher = Regex.Matches(sql, "update\\s(.+)set\\s.*").Cast<Match>().ToList();
                foreach (Match? item in matcher)
                {
                    var obj = new MatchSqlModel()
                    {
                        TableName = item.Groups[1].Value.Trim(),
                        AliasName = "",
                        IsWhere = isExistWhere,
                        SqlOpt = "update"
                    };
                    result.Add(obj);
                }
                return result;
            }
            if (sql.StartsWith("delete", StringComparison.OrdinalIgnoreCase))
            {
                var matcher = Regex.Matches(sql, @"[\s\t]*delete *(from|)[\s\t\r\n]*(where|)[\s\t\r\n]*[A-Za-z0-9_.""\[\]]*[\s\t\r\n]*", RegexOptions.IgnoreCase).Cast<Match>().ToList();
                foreach (Match? item in matcher)
                {
                    var obj = new MatchSqlModel()
                    {
                        //TableName = item.Value[item.Value.Trim().LastIndexOf(' ')..].Trim(),
                        AliasName = "",
                        IsWhere = isExistWhere,
                        SqlOpt = "delete"
                    };
                    result.Add(obj);
                }
                return result;
            }
            return result;
        }

        /// <summary>
        /// 过滤不规则的参数符号,
        /// 注意：切记将字符串进行小写转换
        /// </summary>
        /// <param name="filterSql"></param>
        /// <returns></returns>
        public string FilterMathSymbol(string filterSql)
        {
            filterSql = filterSql.Replace("public.", "").Trim();//并且去除 脚本前后空格
            filterSql = filterSql.Replace(" <= ", "<=").Replace(" <=", "<=").Replace("<= ", "<=");//<= 处理
            filterSql = filterSql.Replace(" >= ", ">=").Replace(" >=", ">=").Replace(">= ", ">=");//>= 处理
            filterSql = filterSql.Replace(" !=", "!=").Replace(" != ", "!=").Replace("!= ", "!=");//!= 处理
            filterSql = filterSql.Replace(" > ", ">").Replace(" >", ">").Replace("> ", ">");//> 处理
            filterSql = filterSql.Replace(" < ", "<").Replace(" <", "<").Replace("< ", "<");//< 处理
            filterSql = filterSql.Replace(" = ", "=").Replace(" =", "=").Replace("= ", "=");//= 处理
            filterSql = filterSql.Replace(" , ", ",").Replace(" ,", ",").Replace(", ", ",");//,处理

            filterSql = Regex.Replace(filterSql, @"\sfrom\s", @" from ", RegexOptions.IgnoreCase);//正则处理from
            filterSql = Regex.Replace(filterSql, @"\sjoin\s", @" join ", RegexOptions.IgnoreCase);//正则处理join
            filterSql = Regex.Replace(filterSql, @"\sas\s", @" as ", RegexOptions.IgnoreCase);//正则处理as

            //去除所有回车符号
            var isEnterWhile = true;
            while (isEnterWhile)
            {
                if (filterSql.Contains("\r\n"))
                {
                    filterSql = filterSql.Replace("\r\n", " ");
                }
                else
                {
                    break;
                }
            }

            //去除所有Tab符号
            var isTabWhile = true;
            while (isTabWhile)
            {
                if (filterSql.Contains("\t"))
                {
                    filterSql = filterSql.Replace("\t", " ");
                }
                else
                {
                    break;
                }
            }

            //去除所有R符号
            var isRWhile = true;
            while (isRWhile)
            {
                if (filterSql.Contains("\r"))
                {
                    filterSql = filterSql.Replace("\r", " ");
                }
                else
                {
                    break;
                }
            }

            //去除所有Tab符号
            var isNWhile = true;
            while (isNWhile)
            {
                if (filterSql.Contains("\n"))
                {
                    filterSql = filterSql.Replace("\n", " ");
                }
                else
                {
                    break;
                }
            }

            //去除空格
            var isSpaceWhile = true;
            while (isSpaceWhile)
            {
                if (filterSql.Contains("  "))
                {
                    filterSql = filterSql.Replace("  ", " ");
                }
                else
                {
                    break;
                }
            }

            //去掉最後的;
            if (filterSql.Substring(filterSql.Length - 1, 1) == ";")
                filterSql = filterSql.Substring(0, filterSql.Length - 1);

            //在)前 添加一個 空格);
            filterSql = filterSql.Replace(")", " )");
            //在)前 添加一個 空格);
            //filterSql = filterSql.Replace("\"", " \"");

            // 最後添加一個空格
            filterSql += " ";

            return filterSql;
        }

        /// <summary>
        /// 正则解析实体
        /// </summary>
        public class MatchSqlModel
        {
            /// <summary>
            /// 表名
            /// </summary>
            public string? TableName { get; set; }

            /// <summary>
            /// 表别名
            /// </summary>
            public string? AliasName { get; set; }

            /// <summary>
            /// sql条件是否存在Where条件
            /// </summary>
            public bool IsWhere { get; set; }

            /// <summary>
            /// sql 脚本 CRUD类型
            /// </summary>
            public string? SqlOpt { get; set; }
        }
    }




}
