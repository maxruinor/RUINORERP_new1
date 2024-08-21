
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2023 14:24:54
// **************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RUINORERP.Model;
using Csla;
using RUINORERP.Business.UseCsla;
using System.Threading.Tasks;
using Csla.Server;
using AutoMapper;

namespace RUINORERP.Business.UseCsla
{
  
    [Serializable()]
    /// <summary>
    /// 用户表
    /// </summary>
    public class tb_UserInfoEditBindingList :BusinessBindingListBase<tb_UserInfoEditBindingList, tb_UserInfoEditInfo>
    {
    
      public void Remove(int id)
      {
        foreach (tb_UserInfoEditInfo item in this)
        {
          if (item.User_ID == id)
          {
            Remove(item);
            break;
          }
        }
      }
      
      
       public tb_UserInfoEditInfo GetEntityById(int id)
      {
        foreach (tb_UserInfoEditInfo item in this)
        {
          if (item.User_ID == id)
          {
            return item;
          }
        }

        return null;
      }
 
        //添加授权规则后面完善
      [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
      [ObjectAuthorizationRules]
      public static void AddObjectAuthorizationRules()
      {
        Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditBindingList),
          new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject, Security.Roles.Administrator));
        Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditBindingList),
          new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.EditObject, Security.Roles.Administrator));
        Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditBindingList),
          new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.DeleteObject, Security.Roles.Administrator));
                  Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditBindingList),
          new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ExecuteMethod, Security.Roles.Administrator));
                  Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditBindingList),
          new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.GetObject, Security.Roles.Administrator));
                  Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditBindingList),
          new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ReadProperty, Security.Roles.Administrator));
                  Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditBindingList),
          new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.WriteProperty, Security.Roles.Administrator));

          
      }
 
 
 
       public static async Task<tb_UserInfoEditBindingList> GetEntitiesAsync(IDataPortal<tb_UserInfoEditBindingList> dp)
      {
        return await dp.FetchAsync();
      }
 
       public tb_UserInfoEditBindingList()
      {
        Saved += tb_UserInfos_Saved;
        AllowNew = true;
      }
 
       private void tb_UserInfos_Saved(object sender, Csla.Core.SavedEventArgs e)
      {
        // this runs on the client and invalidates
        // the List cache
        //tb_UserInfoList.InvalidateCache();
      }

      public static tb_UserInfoEditBindingList GetEntities(IDataPortal<tb_UserInfoEditBindingList> dp)
      {
        return dp.Fetch();
      }

      protected override void OnDeserialized()
      {
        base.OnDeserialized();
        this.Saved += tb_UserInfos_Saved;
      }

      protected override void DataPortal_OnDataPortalInvokeComplete(Csla.DataPortalEventArgs e)
      {
        if (ApplicationContext.ExecutionLocation == ApplicationContext.ExecutionLocations.Server &&
            e.Operation == DataPortalOperations.Update)
        {
          // this runs on the server and invalidates
          // the RoleList cache
          //tb_UserInfoList.InvalidateCache();
        }
      }

    [Create, RunLocal]
    private void Create() { }
    
    
      [Fetch]
      private void Fetch([Inject] Itb_UserInfoDal dal)
      {
            using (LoadListMode)
            {
                List<tb_UserInfo> list = null;
                list = dal.Fetch();
                foreach (var item in list)
                {
                    var Mapper = ApplicationContext.GetRequiredService<IMapper>();
                    tb_UserInfoEditInfo dest = Mapper.Map<tb_UserInfo, tb_UserInfoEditInfo>(item);
                    Add(dest);
                }

            }
      }

      [Update]
      [Transactional(TransactionalTypes.TransactionScope)]
      private void Update()
      {
        using (LoadListMode)
        {
          Child_Update();
        }
      }
    }
}