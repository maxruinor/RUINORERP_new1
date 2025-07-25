
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 17:35:21
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
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容
    /// </summary>
    public partial class tb_ProductTypeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProductTypeServices _tb_ProductTypeServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProductTypeController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProductTypeServices tb_ProductTypeServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProductTypeServices = tb_ProductTypeServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_ProductType info)
        {

           // tb_ProductTypeValidator validator = new tb_ProductTypeValidator();
           tb_ProductTypeValidator validator = _appContext.GetRequiredService<tb_ProductTypeValidator>();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        
        #region 扩展方法
        

        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProductTypeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        

        #endregion
        
        
        
        
    }
}



