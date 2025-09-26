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
    public class CommonController : ICommonController
    {

        public ApplicationContext _appContext;

        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<CommonController> _logger;
        public Itb_UnitServices _itbUnitServices { get; set; }

        public CommonController(ILogger<CommonController> logger, Itb_UnitServices itbUnitServices, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _itbUnitServices = itbUnitServices;
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
            List<tb_Unit> list = await _itbUnitServices.QueryAsync();
            return list;
        }



    }
}
