
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/22/2023 17:06:29
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 产品属性值表
    /// </summary>
    public partial class tb_ProdPropertyValueController<T>
    {

        public virtual async Task<List<tb_ProdPropertyValue>> QueryByPropertyIDAsync(long id)
        {
            //tb_ProdPropertyValue entity = await _tb_ProdPropertyValueServices.QueryByIdAsync(id);
            List<tb_ProdPropertyValue> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdPropertyValue>().Where(p => p.Property_ID == id).ToListAsync();
            return list;
        }


    }
}