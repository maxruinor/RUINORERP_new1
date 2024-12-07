// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/20/2023 15:58:16
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
using System.Linq;
using SqlSugar;

namespace RUINORERP.Business
{
    public partial class tb_UserInfoController<T>
    {



        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual List<tb_UserInfo> QueryByNavWithMoreInfo(Expression<Func<tb_UserInfo, bool>> exp)
        {

            List<tb_UserInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UserInfo>().Where(exp)
                            .Includes(t => t.tb_employee, e => e.tb_department)
                            .AsNavQueryable()
                            .Includes(t => t.tb_User_Roles, ur => ur.tb_UserPersonalizeds,
                            r => r.tb_UIMenuPersonalizations, m => m.tb_UIQueryConditions)
                            .Includes(t => t.tb_User_Roles, ur => ur.tb_UserPersonalizeds,
                            r => r.tb_UIMenuPersonalizations, m => m.tb_UIGridSettings)
                            .Includes(t => t.tb_User_Roles, ur => ur.tb_roleinfo, r => r.tb_P4Modules)
                            .Includes(t => t.tb_User_Roles, ur => ur.tb_roleinfo, r => r.tb_P4Menus)
                            .Includes(t => t.tb_User_Roles, ur => ur.tb_roleinfo, r => r.tb_P4Fields)
                            .Includes(t => t.tb_User_Roles, ur => ur.tb_roleinfo, r => r.tb_P4Buttons)
                            .Includes(t => t.tb_User_Roles, ur => ur.tb_roleinfo, r => r.tb_rolepropertyconfig)
                            .ToList();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_UserInfo>(list);
            return list;
        }



        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual List<tb_RoleInfo> QueryALLPowerByNavWithMoreInfo(List<tb_RoleInfo> roles)
        {
            List<tb_RoleInfo> list = new List<tb_RoleInfo>();
            try
            {
                var ids = roles.Select(it => new { it.RoleID }).ToArray();
                long[] idss = new long[ids.Length];
                for (int i = 0; i < ids.Length; i++)
                {
                    idss[i] = ids[i].RoleID;
                }
                list = _unitOfWorkManage.GetDbClient().Queryable<tb_RoleInfo>()
               .In(it => it.RoleID, idss)
               .Includes(a => a.tb_P4Modules, b => b.tb_moduledefinition)
               .Includes(a => a.tb_P4Modules, b => b.tb_moduledefinition, c => c.tb_MenuInfos)
               .Includes(a => a.tb_P4Menus, b => b.tb_menuinfo)
               .Includes(a => a.tb_P4Buttons, b => b.tb_buttoninfo)
               .Includes(a => a.tb_P4Fields, b => b.tb_fieldinfo)
               .Includes(a => a.tb_rolepropertyconfig)
               .ToList();
                foreach (var item in list)
                {
                    item.HasChanged = false;
                }
                //MyCacheManager.Instance.UpdateEntityList<tb_User_Role>(list);
            }
            catch (Exception ex)
            {


            }
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// 有问题@@@@！！！！
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual List<tb_User_Role> QueryRoleByNavWithMoreInfo(List<tb_RoleInfo> roles)
        {
            List<tb_User_Role> list = new List<tb_User_Role>();
            try
            {

                var ids = roles.Select(it => new { it.RoleID }).ToArray();
                long[] idss = new long[ids.Length];
                for (int i = 0; i < ids.Length; i++)
                {
                    idss[i] = ids[i].RoleID;
                }
                //去重
                // idss = ids.Distinct().ToArray();
                list = _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>()
               .In(it => it.RoleID, idss)
               .Includes(t => t.tb_roleinfo, a => a.tb_P4Modules, b => b.tb_moduledefinition)
               .Includes(t => t.tb_roleinfo, a => a.tb_P4Menus, b => b.tb_menuinfo)
               .Includes(t => t.tb_roleinfo, a => a.tb_P4Buttons, b => b.tb_buttoninfo)
               .Includes(t => t.tb_roleinfo, a => a.tb_P4Fields, b => b.tb_fieldinfo)
               .ToList();
                foreach (var item in list)
                {
                    item.HasChanged = false;
                }
                //MyCacheManager.Instance.UpdateEntityList<tb_User_Role>(list);
            }
            catch (Exception ex)
            {


            }
            return list;
        }

        /// <summary>
        /// 用事务？
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAllAsync(tb_Prod entity)
        {
            //https://www.donet5.com/Home/Doc?typeId=2431
            // 4.5 删A、B和中间表（3张表） 多级导航删除
            //return await _unitOfWorkManage.GetDbClient()
            //.DeleteNav<tb_ProdBase>(x => x.ProdBaseID == entity.ProdBaseID)
            //    .Include(x => x.tb_Prod_Attr_Relations, new DeleteNavOptions()
            //    {
            //        ManyToManyIsDeleteB = true,
            //        ManyToManyIsDeleteA = true
            //    })
            //    .ExecuteCommandAsync();
            return await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Prod>(x => x.ProdBaseID == entity.ProdBaseID)
          .Include(x => x.tb_Prod_Attr_Relations).ThenInclude(y => y.tb_proddetail)
          .ExecuteCommandAsync();

        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> DeleteByLogicAsync(tb_Prod entity)
        {
            return await _unitOfWorkManage.GetDbClient().Deleteable<tb_Prod>().In(entity.ProdBaseID).IsLogic().ExecuteCommandAsync();
        }
        /// <summary>
        /// 用事务？
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAllABAsync(tb_Prod entity)
        {
            //https://www.donet5.com/Home/Doc?typeId=2431
            // 4.5 删A、B和中间表（3张表） 多级导航删除
            //return await _unitOfWorkManage.GetDbClient()
            //.DeleteNav<tb_ProdBase>(x => x.ProdBaseID == entity.ProdBaseID)
            ////.Include(y => y.tb_ProdDetails)
            //    .Include(x => x.tb_Prod_Attr_Relations, new DeleteNavOptions()
            //    {
            //        ManyToManyIsDeleteB = true,
            //        ManyToManyIsDeleteA = true

            //    }).ExecuteCommandAsync();

            return await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Prod>(x => x.ProdBaseID == entity.ProdBaseID)
          .Include(z1 => z1.tb_Prod_Attr_Relations).ThenInclude(z1 => z1.tb_proddetail) //删除2层 
          .Include(x => x.tb_Prod_Attr_Relations)
          .ExecuteCommandAsync();

        }

        /*
        public async  Task<List<tb_ProdBase>> BaseQueryByNavAsync()
        {
            List<T> list = await _tb_ProdBaseServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdBase entity = item as <#= table.ClassName #>;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.AddCacheEntityList<List<T>>(list);
                foreach (var item in list)
                {
                    MyCacheManager.Instance.AddCacheEntity <<#= table.ClassName #>>(item);

                }
            }
            return list;
        }
        */
        /*

 /// <summary>
 /// 物理删除  删除时需要先删除关系再明细再主表
 /// </summary>
 /// <param name="model"></param>
 /// <returns></returns>
 public async Task<bool> DeleteByNavAsync(T model)
 {
     tb_UserInfo entity = model as tb_UserInfo;
     bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UserInfo>(m => m.ProdBaseID == entity.ProdBaseID)
                     .Include(m => m.tb_ProdDetails).ThenInclude(d => d.tb_Prod_Attr_Relations)
                     .ExecuteCommandAsync();
     if (rs)
     {
         //////生成时暂时只考虑了一个主键的情况
         MyCacheManager.Instance.DeleteEntityList<T>(model);
     }
     return rs;
 }


 /// <summary>
 /// 保存更新产品 【事务操作？】
 /// </summary>
 /// <param name="info"></param>
 /// <returns></returns>
 public async Task<ReturnResults<tb_ProdBase>> SaveOrUpdateAsync(tb_ProdBase info)
 {
     ReturnResults<tb_ProdBase> rr = new ReturnResults<tb_ProdBase>();
     long MainID = -1;
     try
     {
         // 开启事务，保证数据一致性
         _unitOfWorkManage.BeginTran();
         //主体是新加，后面的都是新加
         if (info.actionStatus == ActionStatus.新增 || info.ProdBaseID <= 0)
         {
             //要开启事务 不然主表增加 明细出错也会成功一半
             // 开启事务，保证数据一致性
             var newinfo = await _tb_ProdBaseServices.AddReEntityAsync(info);
             MainID = newinfo.ProdBaseID;
             for (int d = 0; d < info.tb_ProdDetails.Count; d++)
             {
                 info.tb_ProdDetails[d].ProdBaseID = MainID;
                 long detailID = await _unitOfWorkManage.GetDbClient().Insertable(info.tb_ProdDetails[d]).ExecuteReturnSnowflakeIdAsync();
                 for (int r = 0; r < info.tb_ProdDetails[d].tb_Prod_Attr_Relations.Count; r++)
                 {
                     info.tb_ProdDetails[d].tb_Prod_Attr_Relations[r].ProdDetailID = detailID;
                     info.tb_ProdDetails[d].tb_Prod_Attr_Relations[r].ProdBaseID = MainID;
                     await _unitOfWorkManage.GetDbClient().Insertable(info.tb_ProdDetails[d].tb_Prod_Attr_Relations[r]).ExecuteReturnEntityAsync();//会自动生成雪花ID
                 }
             }
             //if (info.tb_Prod_Attr_Relations != null)
             //{
             //    //按sku分组，相同的SKU则组成一个属性组合指向一个产品
             //    var skus = info.tb_Prod_Attr_Relations.GroupBy(x => x.tb_ProdDetail.SKU);
             //    foreach (var sku in skus)
             //    {
             //        tb_ProdDetail detail = info.tb_Prod_Attr_Relations.FirstOrDefault(x => x.tb_ProdDetail.SKU == sku.Key.ToString()).tb_ProdDetail;
             //        detail.ProdBaseID = newinfo.ProdBaseID;
             //        //sku count
             //        long detailID = await _unitOfWorkManage.GetDbClient().Insertable(detail).ExecuteReturnSnowflakeIdAsync();
             //        foreach (var relation in sku)
             //        {
             //            relation.ProdBaseID = newinfo.ProdBaseID;
             //            relation.ProdDetailID = detailID;
             //            await _unitOfWorkManage.GetDbClient().Insertable(relation).ExecuteReturnEntityAsync();//会自动生成雪花ID
             //                                                                                                  //await _unitOfWorkManage.GetDbClient().Insertable(relation).ExecuteCommandAsync();//不会自动生成雪花ID
             //        }

             //    }
             //}
             rr.ReturnObject = newinfo;

         }
         if (info.actionStatus == ActionStatus.修改)
         {
             //主表没变过，明细更新时
             if (info.HasChanged)
             {
                 await _tb_ProdBaseServices.Update(info);
             }

             MainID = info.ProdBaseID;
             foreach (tb_ProdDetail detail in info.tb_ProdDetails)
             {
                 if (detail.actionStatus == ActionStatus.新增)
                 {
                     detail.ProdBaseID = info.ProdBaseID;
                     long detailID = await _unitOfWorkManage.GetDbClient().Insertable(detail).ExecuteReturnSnowflakeIdAsync();
                     for (int i = 0; i < detail.tb_Prod_Attr_Relations.Count; i++)
                     {
                         detail.tb_Prod_Attr_Relations[i].ProdDetailID = detailID;
                         detail.tb_Prod_Attr_Relations[i].ProdBaseID = info.ProdBaseID; ;
                     }
                     await _unitOfWorkManage.GetDbClient().Insertable(detail.tb_Prod_Attr_Relations).ExecuteReturnSnowflakeIdListAsync();
                 }

                 if (detail.actionStatus == ActionStatus.修改)
                 {
                     detail.ProdBaseID = info.ProdBaseID;
                     if (detail.ProdDetailID == 0)
                     {
                         detail.ProdBaseID = info.ProdBaseID;
                         long detailID = await _unitOfWorkManage.GetDbClient().Insertable(detail).ExecuteReturnSnowflakeIdAsync();
                         for (int i = 0; i < detail.tb_Prod_Attr_Relations.Count; i++)
                         {
                             detail.tb_Prod_Attr_Relations[i].ProdDetailID = detailID;
                             detail.tb_Prod_Attr_Relations[i].ProdBaseID = info.ProdBaseID; ;
                         }
                         await _unitOfWorkManage.GetDbClient().Insertable(detail.tb_Prod_Attr_Relations).ExecuteReturnSnowflakeIdListAsync();
                     }
                     else
                     {
                         await _unitOfWorkManage.GetDbClient().Updateable(detail).ExecuteCommandAsync();
                         for (int i = 0; i < detail.tb_Prod_Attr_Relations.Count; i++)
                         {
                             //detail.tb_Prod_Attr_Relations[i].ProdDetailID = detail.detailID;
                             //detail.tb_Prod_Attr_Relations[i].ProdBaseID = info.ProdBaseID; ;
                         }
                         await _unitOfWorkManage.GetDbClient().Updateable(detail.tb_Prod_Attr_Relations).ExecuteCommandAsync();
                         //实际上 ，业务上因为修改的只是详情。关系没有变化?
                     }


                 }
                 if (detail.actionStatus == ActionStatus.删除)
                 {
                     await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdDetail>(d => d.ProdDetailID == detail.ProdDetailID)
                         .Include(d => d.tb_Prod_Attr_Relations).ExecuteCommandAsync();

                     //逻辑删除
                     //await _unitOfWorkManage.GetDbClient().Deleteable(detail.tb_Prod_Attr_Relations).IsLogic().ExecuteCommandAsync();
                     //await _unitOfWorkManage.GetDbClient().Deleteable(detail).IsLogic().ExecuteCommandAsync();
                 }
             }

         }
         if (info.actionStatus == ActionStatus.删除)
         {
             foreach (var item in info.tb_ProdDetails)
             {
                 foreach (var rale in item.tb_Prod_Attr_Relations)
                 {
                     await _unitOfWorkManage.GetDbClient().Deleteable(rale).ExecuteCommandAsync();
                 }
                 await _unitOfWorkManage.GetDbClient().Deleteable(item).ExecuteCommandAsync();
             }
             await _unitOfWorkManage.GetDbClient().Deleteable(info).ExecuteCommandAsync();
         }
         rr.ReturnObject = info;
         // 注意信息的完整性
         _unitOfWorkManage.CommitTran();
         rr.Succeeded = true;
         _logger.Error("事务成功");

     }
     catch (Exception ex)
     {
         _logger.Error(ex);
         _unitOfWorkManage.RollbackTran();
         _logger.Error("事务回滚");
         rr.ErrorMsg = "事务回滚=>" + ex.Message;
     }

     return rr;
 }
*/


    }
}

