using RUINORERP.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{

    /*

    /// <summary>
    /// MySqlParameter ---SqlParameter
    /// 没有测试，应该可以修改好。暂时没有使用 只为编译通过
    /// </summary>
    public static class LambdaToSqlHelperForMysql
    {
        #region 实现方法

        public static int GetPageByDataTable(this DataTable dt, int size)
        {
            int totalCount = dt.Rows[0][0].ToString().ToInt();
            return totalCount; //% size > 0 ? totalCount / size + 1 : totalCount / size;
        }

        #region DataTable转化成List

        public static List<T> ConvertToModelList<T>(DataTable dt) where T : new()
        {
            List<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        if (pi.PropertyType.Name == "Boolean")
                        {
                            bool value = false;
                            if (dr[tempName].ToString() == "1")
                            {
                                value = true;
                            }

                            pi.SetValue(t, value, null);
                        }
                        else
                        {
                            object value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        #endregion DataTable转化成List

        #region List转换成DataTable

        public static DataTable ConvertToDataTable<T>(IEnumerable<T> collection)
        {
            DataTable dtReturn = new DataTable();

            PropertyInfo[] oProps = null;

            foreach (T rec in collection)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();

                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType; if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow(); foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }

                dtReturn.Rows.Add(dr);
            }

            return (dtReturn);
        }

        #endregion List转换成DataTable

        #region 获取 插入语句

        public static string GetInsertSqlByT<T>(T t) where T : class
        {
            var type = typeof(T);

            var typeAttrList = type.GetProperties();
            string sb = "INSERT INTO `{0}` ({1}) values ({2})";
            StringBuilder sbName = new StringBuilder("");
            StringBuilder sbValue = new StringBuilder("");

            foreach (var ta in typeAttrList)
            {
                sbName.Append("`");
                sbName.Append(ta.Name);
                sbName.Append("`");
                sbName.Append(",");
                sbValue.Append("`");
                //sbValue.Append(GetValueStringByType(ta.GetValue(t, null)));
                sbValue.Append("`");
                sbValue.Append(",");
            }
            string name = sbName.ToString().Substring(0, sbName.Length - 1);
            string value = sbValue.ToString().Substring(0, sbValue.Length - 1);
            return string.Format(sb, type.Name.ToLower(), name, value);
        }

        public static string GetInsertSqlByT<T>(T t, out SqlParameter[] pars) where T : class
        {
            var type = typeof(T);
            var typeAttrList = type.GetProperties();
            string sb = "INSERT INTO `{0}` ({1}) values ({2})";
            StringBuilder sbName = new StringBuilder("");
            StringBuilder sbValue = new StringBuilder("");
            int markPar = 0;
            pars = new SqlParameter[typeAttrList.Length - 1];
            foreach (var ta in typeAttrList)
            {
                if (ta.Name.ToLower() == "id")
                {
                    continue;
                }
                sbName.Append("`");
                sbName.Append(ta.Name);
                sbName.Append("`");
                sbName.Append(",");
                sbValue.Append("");
                sbValue.Append("@par" + markPar);
                sbValue.Append("");
                sbValue.Append(",");
                SqlParameter p = new SqlParameter();
                p.ParameterName = "@par" + markPar;
                p.Value = ta.GetValue(t, null);

                // p.SqlDbType =SqlTypeString2SqlType(ta.PropertyType.Name.ToLower());
                pars[markPar] = p;
                markPar++;
            }
            string name = sbName.ToString().Substring(0, sbName.Length - 1);
            string value = sbValue.ToString().Substring(0, sbValue.Length - 1);
            return string.Format(sb, type.Name.ToLower(), name, value);
        }

        public static string GetInsertSqlToIDByT<T>(T t, out SqlParameter[] pars) where T : class
        {
            var type = typeof(T);
            var typeAttrList = type.GetProperties();
            string sb = "INSERT INTO `{0}` ({1}) values ({2})";
            StringBuilder sbName = new StringBuilder("");
            StringBuilder sbValue = new StringBuilder("");
            int markPar = 0;
            pars = new SqlParameter[typeAttrList.Length];
            foreach (var ta in typeAttrList)
            {
                sbName.Append("`");
                sbName.Append(ta.Name);
                sbName.Append("`");
                sbName.Append(",");
                sbValue.Append("");
                sbValue.Append("@par" + markPar);
                sbValue.Append("");
                sbValue.Append(",");
                SqlParameter p = new SqlParameter();
                p.ParameterName = "@par" + markPar;
                p.Value = ta.GetValue(t, null);

                // p.SqlDbType =SqlTypeString2SqlType(ta.PropertyType.Name.ToLower());
                pars[markPar] = p;
                markPar++;
            }
            string name = sbName.ToString().Substring(0, sbName.Length - 1);
            string value = sbValue.ToString().Substring(0, sbValue.Length - 1);
            return string.Format(sb, type.Name.ToLower(), name, value);
        }

        #endregion 获取 插入语句

        #region 获取修改语句

        public static string GetUpdateSqlByT<T>(T t, Expression<Func<T, bool>> func, out MySqlParameter[] pars) where T : class
        {
            var type = typeof(T);
            var typeAttrList = type.GetProperties();
            string sb = "UPDATE " + type.Name.ToLower() + " SET {0} where 1=1 and {1}";

            StringBuilder sbValue = new StringBuilder("");
            int markPar = 0;

            List<ParMODEL> listPar = new List<ParMODEL>();
            string where = GetWhereSql(func, listPar);

            pars = new SqlParameter[typeAttrList.Length + listPar.Count - 1];
            foreach (var ta in typeAttrList)
            {
                if (ta.Name == "ID")
                {
                    continue;
                }
                sbValue.Append("`");
                sbValue.Append(ta.Name);
                sbValue.Append("`");
                sbValue.Append("=");
                sbValue.Append("");
                sbValue.Append("@parValue" + markPar);
                sbValue.Append("");
                sbValue.Append(",");
                SqlParameter p = new SqlParameter();
                p.ParameterName = "@parValue" + markPar;
                p.Value = ta.GetValue(t, null);
                pars[markPar] = p;
                markPar++;
            }
            string value = sbValue.ToString().Substring(0, sbValue.Length - 1);

            foreach (var l in listPar)
            {
                SqlParameter p = new SqlParameter();
                p.ParameterName = l.name;
                p.Value = l.value;
                pars[markPar] = p;
                markPar++;
            }

            return string.Format(sb, value, where);
        }

        #endregion 获取修改语句

        #region 获取分页语句

        public static string GetSelectSqlByPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> where, out MySqlParameter[] parameter) where T : class
        {
            List<ParMODEL> parModelList = new List<ParMODEL>();

            string sqlWhere = LambdaToSqlHelper.GetWhereSql<T>(where, parModelList);
            parameter = LambdaToSqlHelper.GetParmetersByList(parModelList);

            string sqlTable = LambdaToSqlHelper.GetSelectTableSqlPage<T>();
            string sqlPageTotal = LambdaToSqlHelper.GetTotalPage(pageIndex, pageSize);

            StringBuilder sb = new StringBuilder("");
            sb.Append(sqlTable);
            sb.Append(" and ");
            sb.Append(sqlWhere);
            sb.Append("  ");

            sb.Append(sqlPageTotal);
            return sb.ToString();
        }

        public static string GetSelectSqlByPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> where, Expression<Func<T, object>> order, out MySqlParameter[] parameter) where T : class
        {
            List<ParMODEL> parModelList = new List<ParMODEL>();

            string sqlWhere = LambdaToSqlHelper.GetWhereSql<T>(where, parModelList);
            parameter = LambdaToSqlHelper.GetParmetersByList(parModelList);
            string sqlOrder = LambdaToSqlHelper.GetOrderSql<T>(order);

            string sqlTable = LambdaToSqlHelper.GetSelectTableSqlPage<T>();
            string sqlPageTotal = LambdaToSqlHelper.GetTotalPage(pageIndex, pageSize);

            StringBuilder sb = new StringBuilder("");
            sb.Append(sqlTable);
            sb.Append(" and ");
            sb.Append(sqlWhere);
            sb.Append("  ");
            sb.Append(sqlOrder);
            sb.Append(sqlPageTotal);
            return sb.ToString();
        }

        public static string GetSelectSql<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, out MySqlParameter[] parameter) where T : class
        {
            List<ParMODEL> parModelList = new List<ParMODEL>();

            string sqlWhere = LambdaToSqlHelper.GetWhereSql<T>(where, parModelList);
            parameter = LambdaToSqlHelper.GetParmetersByList(parModelList);
            string sqlOrder = LambdaToSqlHelper.GetOrderSql<T>(order);

            string sqlTable = LambdaToSqlHelper.GetSelectTableSql<T>();

            StringBuilder sb = new StringBuilder("");
            sb.Append(sqlTable);
            sb.Append("  and ");
            sb.Append(sqlWhere);
            sb.Append("  ");
            sb.Append(sqlOrder);
            return sb.ToString();
        }

        public static string GetSelectSql<T>(Expression<Func<T, bool>> where, out MySqlParameter[] pars) where T : class
        {
            List<ParMODEL> parModelList = new List<ParMODEL>();

            string sqlWhere = LambdaToSqlHelper.GetWhereSql<T>(where, parModelList);
            pars = LambdaToSqlHelper.GetParmetersByList(parModelList);

            string sqlTable = LambdaToSqlHelper.GetSelectTableSql<T>();

            StringBuilder sb = new StringBuilder("");
            sb.Append(sqlTable);
            sb.Append(" and  ");
            sb.Append(sqlWhere);
            sb.Append("  ");

            return sb.ToString();
        }

        public static string GetSelectSql<T>() where T : class
        {
            List<ParMODEL> parModelList = new List<ParMODEL>();
            string sqlTable = LambdaToSqlHelper.GetSelectTableSql<T>();
            StringBuilder sb = new StringBuilder("");
            sb.Append(sqlTable);
            sb.Append("  ");

            return sb.ToString();
        }

        #endregion 获取分页语句

        #region 获取删除语句

        public static string GetDeleteSqlByID<T>(List<int> list, out MySqlParameter[] pars) where T : class
        {
            var typeT = typeof(T);
            StringBuilder sb = new StringBuilder();
            pars = new MySqlParameter[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append("@par" + i + ",");

                MySqlParameter p = new MySqlParameter();
                p.ParameterName = "@par" + i;
                p.Value = list[i];

                // p.SqlDbType =SqlTypeString2SqlType(ta.PropertyType.Name.ToLower());
                pars[i] = p;
            }
            string value = sb.ToString().Substring(0, sb.Length - 1);
            return "update  `" + typeT.Name.ToLower() + "` set isdelete=" + (int)CommonEnum.IsDelete.已删除 + "  where  id in(" + value + ") ";
        }

        public static string GetDeleteSqlByID<T>(int id, out MySqlParameter[] pars) where T : class
        {
            var typeT = typeof(T);
            StringBuilder sb = new StringBuilder();
            pars = new MySqlParameter[1];

            sb.Append("@par" + id + ",");

            MySqlParameter p = new MySqlParameter();
            p.ParameterName = "@par" + id;
            p.Value = id;

            // p.SqlDbType =SqlTypeString2SqlType(ta.PropertyType.Name.ToLower());
            pars[0] = p;

            string value = sb.ToString().Substring(0, sb.Length - 1);
            return "update  `" + typeT.Name.ToLower() + "` set isdelete=" + (int)CommonEnum.IsDelete.已删除 + "  where  id in(" + value + ") ";
        }

        public static string GetDeleteSqlByIDsTrue<T>(List<int> list, out MySqlParameter[] pars) where T : class
        {
            var typeT = typeof(T);
            StringBuilder sb = new StringBuilder();
            pars = new MySqlParameter[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append("@par" + i + ",");

                MySqlParameter p = new MySqlParameter();
                p.ParameterName = "@par" + i;
                p.Value = list[i];

                // p.SqlDbType =SqlTypeString2SqlType(ta.PropertyType.Name.ToLower());
                pars[i] = p;
            }
            string value = sb.ToString().Substring(0, sb.Length - 1);
            return "delete from  `" + typeT.Name.ToLower() + "`  where  id in(" + value + ") ";
        }

        public static string DeleteEntityByWhereTrue<T>(Expression<Func<T, bool>> func, out MySqlParameter[] pars) where T : class
        {
            var typeT = typeof(T);

            List<ParMODEL> list = new List<ParMODEL>();

            string sql = "delete from  `" + typeT.Name.ToLower() + "`  where  1=1 and  " + GetWhereSql<T>(func, list);
            pars = new MySqlParameter[list.Count];
            int markPar = 0;
            foreach (var l in list)
            {
                MySqlParameter p = new MySqlParameter();
                p.ParameterName = l.name;
                p.Value = l.value;
                pars[markPar] = p;
                markPar++;
            }
            return sql;
        }

        #endregion 获取删除语句

        #region 获取数量语句

        public static string GetSelectSqlCount<T>(Expression<Func<T, bool>> where, out MySqlParameter[] parameter) where T : class
        {
            List<ParMODEL> parModelList = new List<ParMODEL>();

            string sqlWhere = LambdaToSqlHelper.GetWhereSql<T>(where, parModelList);
            parameter = LambdaToSqlHelper.GetParmetersByList(parModelList);

            string sqlTable = LambdaToSqlHelper.GetSelectTableSqlCount<T>();

            StringBuilder sb = new StringBuilder("");
            sb.Append(sqlTable);
            sb.Append("  and ");
            sb.Append(sqlWhere);
            sb.Append("  ");

            return sb.ToString();
        }

        #endregion 获取数量语句

        #endregion 实现方法
    }


    */



}
