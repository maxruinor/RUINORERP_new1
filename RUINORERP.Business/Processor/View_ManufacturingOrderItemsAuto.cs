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
using RUINORERP.Common.Helper;

namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 制令单明细统计分析 - 自动生成部分
    /// </summary>
    public partial class View_ManufacturingOrderItemsProcessor : BaseProcessor
    {
        /// <summary>
        /// 构造函数 - 初始化制令单统计处理器
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="unitOfWorkManage">工作单元管理</param>
        /// <param name="appContext">应用上下文</param>
        public View_ManufacturingOrderItemsProcessor(ILogger<View_ManufacturingOrderItemsProcessor> logger, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null) : base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _appContext = appContext;
        }
    }
}
