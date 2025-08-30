
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:05
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
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 产品主次及属性关系表
    /// </summary>
    public partial class tb_Prod_Attr_RelationProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {

            QueryFilter queryFilter = new QueryFilter();
            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_Prod_Attr_Relation>(c => c.Property_ID);
            queryFilter.SetQueryField<tb_Prod_Attr_Relation>(c => c.PropertyValueID, true);
            queryFilter.SetQueryField<tb_Prod_Attr_Relation>(c => c.ProdBaseID, true);
            queryFilter.SetQueryField<tb_Prod_Attr_Relation>(c => c.ProdDetailID, true);
            return queryFilter;
        }

    }
}



