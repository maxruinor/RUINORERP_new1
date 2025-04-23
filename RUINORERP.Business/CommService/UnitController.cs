using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using System.Data;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using SqlSugar;
using RUINORERP.Business.Processor;
using RUINORERP.Model.Context;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 公共的一个控制器。直接调用底层的方法
    /// </summary>
    public class CommonController
    {

        public ApplicationContext _appContext;

        private readonly IUnitOfWorkManage _unitOfWorkManage;
        public IUnitServices _unitServices { get; set; }
        private readonly ILogger<CommonController> _logger;
        public Itb_UnitServices _Itb_UnitServices { get; set; }
        public CommonController(ILogger<CommonController> logger, Itb_UnitServices Itb_UnitServices, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _Itb_UnitServices = Itb_UnitServices;
            _appContext = appContext;
        }

        public List<T> GetBindSource<T>(string tableName) where T : class
        {
            if (tableName == "tb_CRM_FollowUpPlans")
            {

            }
            BaseController<T> bdc = _appContext.GetRequiredServiceByName<BaseController<T>>(typeof(T).Name + "Controller");
            BaseProcessor baseProcessor = _appContext.GetRequiredServiceByName<BaseProcessor>(tableName + "Processor");
            QueryFilter queryFilter = baseProcessor.GetQueryFilter();
            //最好情况是新增时数据限制。其他可以显示
            queryFilter.FilterLimitExpressions.Clear();//缓存清除限制条件,比方员工离职：被禁用的，实际他原来录的数据，还是可以用的。也能显示他的名字。
            ISugarQueryable<T> querySqlQueryable;
            querySqlQueryable = (ISugarQueryable<T>)bdc.BaseGetISugarQueryable(false, queryFilter, null);
            return querySqlQueryable.ToList() as List<T>;
        }



        public List<T> GetBindSource<T>(string tableName, Expression<Func<T, bool>> expCondition)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(expCondition).AS(tableName).ToList();
        }

        public object GetBindSourceList(string tableName)
        {
            return _unitOfWorkManage.GetDbClient().Queryable(tableName, "shortName");
        }

        public async Task<List<tb_Unit>> Query()
        {
            List<tb_Unit> list = await _Itb_UnitServices.QueryAsync();
            return list;
        }

        public async Task<bool> TransTest()
        {
            //var data = new MessageModel<string>() { success = true, msg = "" };
            bool rs = true;
            if (true)
            {
                try
                {
                    Model.tb_Unit unit = new tb_Unit();
                    unit.Unit_ID = 6;
                    unit.UnitName = "transtest";
                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();
                    //先添加再删除
                    //await _unitServices.Add(unit);
                    //await _unitServices.DeleteById(1);
                    ////先删除再添加
                    ///
                    Model.tb_Unit unit1 = new tb_Unit();
                    unit1.Unit_ID = 4;
                    unit1.UnitName = "transtest44";

                    await _unitServices.Add(unit);

                    await _unitServices.Add(unit1);

                    await _unitServices.DeleteById(110);

                    // 注意信息的完整性
                    _unitOfWorkManage.CommitTran();

                    rs = true;
                }
                catch (Exception ex)
                {
                    _unitOfWorkManage.RollbackTran();
                    _logger.Error(ex, "事务回滚" + ex.Message);
                }
            }
            else
            {
                //data.success = false;
                //data.msg = "当前不处于开发模式，代码生成不可用！";
            }

            return rs;
        }


    }
}
