
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/26/2024 19:54:05
// **************************************
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
using RUINORERP.Services;

using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;

namespace RUINORERP.Business
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Proc_SaleOutStatisticsByEmployeeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public IProc_SaleOutStatisticsByEmployeeServices _Proc_SaleOutStatisticsByEmployeeServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public Proc_SaleOutStatisticsByEmployeeController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,Proc_SaleOutStatisticsByEmployeeServices Proc_SaleOutStatisticsByEmployeeServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _Proc_SaleOutStatisticsByEmployeeServices = Proc_SaleOutStatisticsByEmployeeServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(Proc_SaleOutStatisticsByEmployee info)
        {
            Proc_SaleOutStatisticsByEmployeeValidator validator = new Proc_SaleOutStatisticsByEmployeeValidator();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        
        #region 扩展方法
        
        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public override async Task<bool> ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return await _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).AnyAsync();
        }
      
        
       



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _Proc_SaleOutStatisticsByEmployeeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        

        #endregion
        
        
        
        
    }
}



