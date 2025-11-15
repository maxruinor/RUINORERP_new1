
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/28/2023 19:01:07
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
    /// 产品类别表 与行业相关的产品分类
    /// </summary>
    public partial class tb_ProdCategoriesController<T>
    {
 
        
    
    
        /// <summary>
        /// 雪花ID模式下的新增和修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  List<long> GetChildids(long Parent_id)
        {
            List<long> ids = new List<long>();
            try
            {

               // _appContext.Db.CopyNew().Queryable<tb_ProdCategories>().Select(s=>s.Category_ID).Where(c => c.Parent_id == entity.Category_ID).ToList();

                List<tb_ProdCategories> allchilds =  _appContext.Db.CopyNew().Queryable<tb_ProdCategories>() .ToChildList(it => it.Parent_id, Parent_id);
                ids= allchilds.Select(s => s.Category_ID).ToList();
            }
            catch (Exception ex)
            {
                ////这里需要进一步优化处理？
                throw ex;
            }
            return ids;
        }
        
        
     
        
        
        
     
        
        
        
        
    }
}



