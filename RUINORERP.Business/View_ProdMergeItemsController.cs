
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:23
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
using Mapster;

namespace RUINORERP.Business
{
    /// <summary>
    /// 组合明细统计-只管明细
    /// </summary>
    public partial class View_ProdMergeItemsController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public IView_ProdMergeItemsServices _View_ProdMergeItemsServices { get; set; }
        // private readonly ApplicationContext _appContext;

        public View_ProdMergeItemsController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage, View_ProdMergeItemsServices View_ProdMergeItemsServices, ApplicationContext appContext = null) : base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _View_ProdMergeItemsServices = View_ProdMergeItemsServices;
            _appContext = appContext;
        }




        public ValidationResult Validator(View_ProdMergeItems info)
        {
            View_ProdMergeItemsValidator validator = _appContext.GetRequiredService<View_ProdMergeItemsValidator>();
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
            T entity = await _View_ProdMergeItemsServices.QueryByIdAsync(id) as T;
            return entity;
        }



        #endregion




    }
}



