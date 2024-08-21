
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
    /// 产品属性组合
    /// </summary>
    public partial class View_ProdPropGoupController
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        public readonly ILogger<View_ProdPropGoupController> _logger;
        public IView_ProdPropGoupServices _View_ProdPropGoupServices { get; set; }

        public View_ProdPropGoupController(ILogger<View_ProdPropGoupController> logger, IUnitOfWorkManage unitOfWorkManage, View_ProdPropGoupServices View_ProdPropGoupServices)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _View_ProdPropGoupServices = View_ProdPropGoupServices;
        }


        public async Task<View_ProdPropGoup> AddReEntityAsync(View_ProdPropGoup entity)
        {
            View_ProdPropGoup AddEntity = await _View_ProdPropGoupServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<View_ProdPropGoup>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<long> AddAsync(View_ProdPropGoup entity)
        {
            long id = await _View_ProdPropGoupServices.Add(entity);
            if (id > 0)
            {

            }
            return id;
        }

        public async Task<List<long>> AddAsync(List<View_ProdPropGoup> infos)
        {
            List<long> ids = await _View_ProdPropGoupServices.Add(infos);
            return ids;
        }


        public async Task<bool> DeleteAsync(View_ProdPropGoup entity)
        {
            bool rs = await _View_ProdPropGoupServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<View_ProdPropGoup>(entity);

            }
            return rs;
        }

        public async Task<bool> UpdateAsync(View_ProdPropGoup entity)
        {
            bool rs = await _View_ProdPropGoupServices.Update(entity);
            if (rs)
            {
                MyCacheManager.Instance.UpdateEntityList<View_ProdPropGoup>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool rs = await _View_ProdPropGoupServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<View_ProdPropGoup>(id);
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _View_ProdPropGoupServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<View_ProdPropGoup>(ids);
            }
            return rs;
        }

        public virtual async Task<List<View_ProdPropGoup>> QueryAsync()
        {
            List<View_ProdPropGoup> list = await _View_ProdPropGoupServices.QueryAsync();
            MyCacheManager.Instance.UpdateEntityList<View_ProdPropGoup>(list);
            return list;
        }

        public virtual List<View_ProdPropGoup> Query()
        {
            List<View_ProdPropGoup> list = _View_ProdPropGoupServices.Query();
            MyCacheManager.Instance.UpdateEntityList<View_ProdPropGoup>(list);
            return list;
        }

        public virtual List<View_ProdPropGoup> Query(string wheresql)
        {
            List<View_ProdPropGoup> list = _View_ProdPropGoupServices.Query(wheresql);
            MyCacheManager.Instance.UpdateEntityList<View_ProdPropGoup>(list);
            return list;
        }

        public virtual async Task<List<View_ProdPropGoup>> QueryAsync(string wheresql)
        {
            List<View_ProdPropGoup> list = await _View_ProdPropGoupServices.QueryAsync(wheresql);
            MyCacheManager.Instance.UpdateEntityList<View_ProdPropGoup>(list);
            return list;
        }


        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<View_ProdPropGoup>> QueryByNavAsync()
        {
            List<View_ProdPropGoup> list = await _unitOfWorkManage.GetDbClient().Queryable<View_ProdPropGoup>()
                                    .ToListAsync();
            MyCacheManager.Instance.UpdateEntityList<View_ProdPropGoup>(list);
            return list;
        }


        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<View_ProdPropGoup>> QueryByAdvancedAsync(object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<View_ProdPropGoup>();
            querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<View_ProdPropGoup>().Where(true, dto);
            return await querySqlQueryable.ToListAsync();
        }



        public virtual async Task<View_ProdPropGoup> QueryByIdAsync(object id)
        {
            View_ProdPropGoup entity = await _View_ProdPropGoupServices.QueryByIdAsync(id);
            return entity;
        }


    }
}