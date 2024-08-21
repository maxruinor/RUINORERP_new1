
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/09/2023 12:16:10
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


namespace RUINORERP.Business
{
    /// <summary>
    /// 产品属性关系
    /// </summary>
    public partial class View_ProdPropertyController
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        public readonly ILogger<View_ProdPropertyController> _logger;
        public IView_ProdPropertyServices _View_ProdPropertyServices { get; set; }
        
        public View_ProdPropertyController(ILogger<View_ProdPropertyController> logger, IUnitOfWorkManage unitOfWorkManage,View_ProdPropertyServices View_ProdPropertyServices )
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _View_ProdPropertyServices = View_ProdPropertyServices;
        }
        
  
        
         public async Task<View_ProdProperty> AddReEntityAsync(View_ProdProperty entity)
        {
            View_ProdProperty AddEntity = await _View_ProdPropertyServices.AddReEntityAsync(entity);

            MyCacheManager.Instance.UpdateEntityList<View_ProdProperty>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(View_ProdProperty entity)
        {
            long id = await _View_ProdPropertyServices.Add(entity);
            if(id>0)
            {
                
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<View_ProdProperty> infos)
        {
            List<long> ids = await _View_ProdPropertyServices.Add(infos);
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(View_ProdProperty entity)
        {
            bool rs = await _View_ProdPropertyServices.Delete(entity);
            if (rs)
            {

                MyCacheManager.Instance.DeleteEntityList<View_ProdProperty>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(View_ProdProperty entity)
        {
            bool rs = await _View_ProdPropertyServices.Update(entity);
            if (rs)
            {

                MyCacheManager.Instance.UpdateEntityList<View_ProdProperty>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            bool rs = await _View_ProdPropertyServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<View_ProdProperty>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _View_ProdPropertyServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<View_ProdProperty>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<View_ProdProperty>> QueryAsync()
        {
            List<View_ProdProperty> list = await  _View_ProdPropertyServices.QueryAsync();

            MyCacheManager.Instance.UpdateEntityList<View_ProdProperty>(list);
            return list;
        }
        
        public virtual List<View_ProdProperty> Query()
        {
            List<View_ProdProperty> list =  _View_ProdPropertyServices.Query();

            MyCacheManager.Instance.UpdateEntityList<View_ProdProperty>(list);
            return list;
        }
        
        public virtual List<View_ProdProperty> Query(string wheresql)
        {
            List<View_ProdProperty> list =  _View_ProdPropertyServices.Query(wheresql);
              MyCacheManager.Instance.UpdateEntityList<View_ProdProperty>(list);
            return list;
        }
        
        public virtual async Task<List<View_ProdProperty>> QueryAsync(string wheresql)
        {
            List<View_ProdProperty> list = await _View_ProdPropertyServices.QueryAsync(wheresql);
            MyCacheManager.Instance.UpdateEntityList<View_ProdProperty>(list);
            return list;
        }
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<View_ProdProperty>> QueryByNavAsync()
        {
            List<View_ProdProperty> list = await _unitOfWorkManage.GetDbClient().Queryable<View_ProdProperty>()
                                    .ToListAsync();
            MyCacheManager.Instance.UpdateEntityList<View_ProdProperty>(list);
            return list;
        }


        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<View_ProdProperty>> QueryByAdvancedAsync(object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<View_ProdProperty>();
            querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<View_ProdProperty>().Where(true,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public virtual async Task<View_ProdProperty> QueryByIdAsync(object id)
        {
            View_ProdProperty entity = await _View_ProdPropertyServices.QueryByIdAsync(id);
            return entity;
        }
        
        
    }
}