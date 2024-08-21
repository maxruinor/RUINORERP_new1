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
    /// 销售订单控制器
    /// </summary>
    public class SaleOrderController
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        public readonly ILogger<tb_SaleOrderController<tb_SaleOrder>> _logger;
        public Itb_SaleOrderServices _tb_sales_orderServices { get; set; }
        public Itb_SalesOrderDetailServices _tb_sales_order_detailServices { get; set; }

        public SaleOrderController(ILogger<tb_SaleOrderController<tb_SaleOrder>> logger,
            IUnitOfWorkManage unitOfWorkManage,
            tb_SaleOrderServices tb_sales_orderServices, Itb_SalesOrderDetailServices itb_Sales_Order_DetailServices
            )
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _tb_sales_orderServices = tb_sales_orderServices;
            _tb_sales_order_detailServices = itb_Sales_Order_DetailServices;
        }

        public async Task<tb_SaleOrder> AddReEntityAsync(tb_SaleOrder entity)
        {
            tb_SaleOrder AddEntity = await _tb_sales_orderServices.AddReEntityAsync(entity);

           MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(AddEntity);
            entity.actionStatus = ActionStatus.无操作;
            return AddEntity;
        }



        public async Task<tb_SaleOrder> AddAsync(tb_SaleOrder entity, List<tb_SalesOrderDetail> details)
        {
            tb_SaleOrder main = await _tb_sales_orderServices.AddReEntityAsync(entity);
            foreach (tb_SalesOrderDetail item in details)
            {
                item.Order_ID = main.Order_ID;
            }
            long subAddCounter = await _tb_sales_order_detailServices.Add(details);
            if (subAddCounter > 0)
            {

            }
            return main;
        }


        public async Task<tb_SaleOrder> UpdateAsync(tb_SaleOrder entity, List<tb_SaleOrderDetail> details)
        {
            await _tb_sales_orderServices.Update(entity);
            List<tb_SaleOrderDetail> newDetails = details.FindAll(d => d.SaleOrderDetail_ID == 0);
            newDetails.ForEach(d => d.Order_ID = entity.Order_ID
                );
            if (newDetails.Count>0)
            {
                await _tb_sales_order_detailServices.Add(newDetails);
            }
            if (details.FindAll(d => d.SaleOrderDetail_ID > 0).Count>0)
            {
                await _tb_sales_order_detailServices.Update(details.FindAll(d => d.SaleOrderDetail_ID > 0));
            }
            
            return entity;
        }


        public async Task<long> AddAsync(List<tb_SaleOrder> infos)
        {
            long id = await _tb_sales_orderServices.Add(infos);
            return id;
        }


        public async Task<bool> DeleteAsync(tb_SaleOrder entity)
        {
            bool rs = await _tb_sales_orderServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrder>(entity);

            }
            return rs;
        }

        public async Task<bool> UpdateAsync(tb_SaleOrder entity)
        {
            bool rs = await _tb_sales_orderServices.Update(entity);
            if (rs)
            {
                MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(entity);
                entity.actionStatus = ActionStatus.无操作;
            }
            return rs;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            bool rs = await _tb_sales_orderServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrder>(id);
            }
            return rs;
        }



        public async Task<bool> DeleteAsyncForDetail(long[] ids)
        {
            bool rs = await _tb_sales_order_detailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SalesOrderDetail>(ids);
            }
            return rs;
        }

        public virtual async Task<List<tb_SaleOrder>> QueryAsync()
        {
            List<tb_SaleOrder> list = await _tb_sales_orderServices.QueryAsync();
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }

        public virtual List<tb_SaleOrder> Query()
        {
            List<tb_SaleOrder> list = _tb_sales_orderServices.Query();
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }

        public virtual List<tb_SaleOrder> Query(string wheresql)
        {
            List<tb_SaleOrder> list = _tb_sales_orderServices.Query(wheresql);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }

        public virtual async Task<List<tb_SaleOrder>> QueryAsync(string wheresql)
        {
            List<tb_SaleOrder> list = await _tb_sales_orderServices.QueryAsync(wheresql);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
            return list;
        }


        public virtual async Task<tb_SaleOrder> QueryByIdAsync(object id)
        {
            tb_SaleOrder entity = await _tb_sales_orderServices.QueryByIdAsync(id);
            return entity;
        }




    }
}
