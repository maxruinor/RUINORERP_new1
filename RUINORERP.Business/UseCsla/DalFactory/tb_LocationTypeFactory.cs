
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2023 14:44:57
// **************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RUINORERP.Common.DI;
using RUINORERP.Model;
using SqlSugar;
using Csla;

namespace RUINORERP.Business.UseCsla
{

    /// <summary>
    /// 库位类别
    /// </summary>
    public class tb_LocationTypeFactory : Csla.Server.ObjectFactory, IDependencyDal
    {

        public tb_LocationTypeFactory(ApplicationContext applicationContext)
        : base(applicationContext) { }


        public tb_LocationTypeEditInfo Create()
        {
            var result = ApplicationContext.CreateInstanceDI<tb_LocationTypeEditInfo>();
            // LoadProperty(result, PersonEdit.NameProperty, "Xiaoping");
            CheckRules(result);
            MarkNew(result);
            return result;
        }

        public tb_LocationTypeEditInfo Fetch()
        {
            var result = ApplicationContext.CreateInstanceDI<tb_LocationTypeEditInfo>();
            //LoadProperty(result, PersonEdit.NameProperty, "Xiaoping");
            CheckRules(result);
            MarkOld(result);
            return result;
        }


        public List<tb_LocationTypeEditInfo> FetchList()
        {
            List<tb_LocationType> list = new List<tb_LocationType>();
            List<tb_LocationTypeEditInfo> newlist = new List<tb_LocationTypeEditInfo>();

            try
            {


                SqlSugarClient db = new SqlSugarClient(
                 new ConnectionConfig()

                 {
                     ConnectionString = "server=192.168.0.254;uid=sa;pwd=SA!@#123sa ;database=erp",
                     //ConnectionString = "server=192.168.0.250;uid=sa;pwd=sa ;database=erp",

                     DbType = SqlSugar.DbType.SqlServer,//设置数据库类型

                     IsAutoCloseConnection = true,//自动释放数据库，如果存在事务，在事务结束之后释放。

                     InitKeyType = InitKeyType.Attribute//从实体特性中读取主键自增列信息

                 });

                //aop监听sql，此段会在每一个"操作语句"执行时都进入....eg:getbyWhere这里会执行两次

                db.Aop.OnLogExecuting = (sql, pars) =>
                {

                    string sqlStempt = sql + "参数值：" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));

                };


                list = db.Queryable<tb_LocationType>().ToList();
                // var personList = await _portal.FetchAsync();

                Csla.Data.DataMapper.Map(list, newlist);
                // dataGridView1.DataSource = personList;

            }
            catch (Exception ex)
            {


            }

            return newlist;

            // return projection of entire list
            //return _personTable.Where(r => true).ToList();
        }

        public List<tb_LocationType> FetchTList()
        {
            List<tb_LocationType> list = new List<tb_LocationType>();


            try
            {


                SqlSugarClient db = new SqlSugarClient(
                 new ConnectionConfig()

                 {
                     ConnectionString = "server=192.168.0.254;uid=sa;pwd=SA!@#123sa ;database=erp",
                     //ConnectionString = "server=192.168.0.250;uid=sa;pwd=sa ;database=erp",

                     DbType = SqlSugar.DbType.SqlServer,//设置数据库类型

                     IsAutoCloseConnection = true,//自动释放数据库，如果存在事务，在事务结束之后释放。

                     InitKeyType = InitKeyType.Attribute//从实体特性中读取主键自增列信息

                 });

                //aop监听sql，此段会在每一个"操作语句"执行时都进入....eg:getbyWhere这里会执行两次

                db.Aop.OnLogExecuting = (sql, pars) =>
                {

                    string sqlStempt = sql + "参数值：" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));

                };


                list = db.Queryable<tb_LocationType>().ToList();
                // var personList = await _portal.FetchAsync();

                //Csla.Data.DataMapper.Map(list, newlist);
                // dataGridView1.DataSource = personList;

            }
            catch (Exception ex)
            {


            }

            return list;

            // return projection of entire list
            //return _personTable.Where(r => true).ToList();
        }


        public void Insert(tb_LocationTypeEditInfo item)
        {
            throw new NotImplementedException();
        }

        public void Update(tb_LocationTypeEditInfo item)
        {
            throw new NotImplementedException();
        }










    }
}