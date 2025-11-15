
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/29/2024 13:46:09
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
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 产品属性值表
    /// </summary>
    public partial class tb_ProdPropertyValueProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_ProdPropertyValue>(c => c.Property_ID,true);
            queryFilter.SetQueryField<tb_ProdPropertyValue>(c => c.PropertyValueName);
            queryFilter.SetQueryField<tb_ProdPropertyValue>(c => c.PropertyValueDesc);
            //queryFilter.SetQueryField<tb_ProdPropertyValue>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
          
            return queryFilter;

        }



    }
}



