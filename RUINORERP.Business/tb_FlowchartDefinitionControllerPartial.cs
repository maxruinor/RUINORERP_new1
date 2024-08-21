
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/03/2023 22:06:18
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 流程图定义
    /// </summary>
    public partial class tb_FlowchartDefinitionController<T>
    {

        //public virtual List<tb_FlowchartDefinition> GetFlowChartAll()
        //{
        //    List<tb_FlowchartDefinition> entity = _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>()
        //        .Where(e => e.IsIdValid == true).ToList();
        //    return entity;
        // }


        public virtual List<tb_FlowchartDefinition> GetFlowCharts(string name)
        {
            List<tb_FlowchartDefinition> entity = _unitOfWorkManage.GetDbClient().Queryable<tb_FlowchartDefinition>()
                .Includes(t => t.tb_FlowchartItems)
               .Includes(l=>l.tb_FlowchartLines)
                .Where(e => e.FlowchartName==name).ToList();
            return entity;
        }


    }
}