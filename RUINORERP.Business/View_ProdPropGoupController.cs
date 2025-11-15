
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:57:47
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
    public partial class View_ProdPropGoupController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public IView_ProdPropGoupServices _View_ProdPropGoupServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public View_ProdPropGoupController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,View_ProdPropGoupServices View_ProdPropGoupServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _View_ProdPropGoupServices = View_ProdPropGoupServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(View_ProdPropGoup info)
        {

           // View_ProdPropGoupValidator validator = new View_ProdPropGoupValidator();
           View_ProdPropGoupValidator validator = _appContext.GetRequiredService<View_ProdPropGoupValidator>();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        
        #region 扩展方法
        
      


        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _View_ProdPropGoupServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        

        #endregion
        
        
        
        
    }
}



