
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2024 19:40:31
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
using RUINORERP.Extensions.Middlewares;
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
    public partial class Proc_WorkCenterSaleController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public IProc_WorkCenterSaleServices _Proc_WorkCenterSaleServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public Proc_WorkCenterSaleController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,Proc_WorkCenterSaleServices Proc_WorkCenterSaleServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _Proc_WorkCenterSaleServices = Proc_WorkCenterSaleServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(Proc_WorkCenterSale info)
        {
            Proc_WorkCenterSaleValidator validator = new Proc_WorkCenterSaleValidator();
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
            T entity = await _Proc_WorkCenterSaleServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        

        #endregion
        
        
        
        
    }
}



