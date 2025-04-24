
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/23/2025 23:27:33
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 收付款方式与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面
    /// </summary>
    public partial class tb_PayMethodAccountMapperProcessor:BaseProcessor 
    {
       
        public tb_PayMethodAccountMapperProcessor(ILogger<tb_PayMethodAccountMapperProcessor> logger, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
            _appContext = appContext;
        }
        
    }
}



