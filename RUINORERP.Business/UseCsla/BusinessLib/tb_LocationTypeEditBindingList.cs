
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2023 14:44:55
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
    /// 库位类别
    /// </summary>
    public class tb_LocationTypeEditBindingList : BusinessBindingListBase<tb_LocationTypeEditBindingList, tb_LocationTypeEditInfo>
    {

        public void Remove(int id)
        {
            foreach (tb_LocationTypeEditInfo item in this)
            {
                if (item.LocationType_ID == id)
                {
                    Remove(item);
                    break;
                }
            }
        }


        public tb_LocationTypeEditInfo GetEntityById(int id)
        {
            foreach (tb_LocationTypeEditInfo item in this)
            {
                if (item.LocationType_ID == id)
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
            Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditBindingList),
              new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject, Security.Roles.Administrator));
            Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditBindingList),
              new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.EditObject, Security.Roles.Administrator));
            Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditBindingList),
              new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.DeleteObject, Security.Roles.Administrator));
            Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditBindingList),
    new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ExecuteMethod, Security.Roles.Administrator));
            Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditBindingList),
    new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.GetObject, Security.Roles.Administrator));
            Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditBindingList),
    new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ReadProperty, Security.Roles.Administrator));
            Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditBindingList),
    new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.WriteProperty, Security.Roles.Administrator));


        }





        public static async Task<tb_LocationTypeEditBindingList> GetEntitiesAsync(IDataPortal<tb_LocationTypeEditBindingList> dp)
        {
            return await dp.FetchAsync();
        }

        public tb_LocationTypeEditBindingList()
        {
            Saved += tb_LocationTypes_Saved;
            AllowNew = true;
        }

        private void tb_LocationTypes_Saved(object sender, Csla.Core.SavedEventArgs e)
        {
            // this runs on the client and invalidates
            // the List cache
            //tb_LocationTypeList.InvalidateCache();
        }

        public static tb_LocationTypeEditBindingList GetEntities(IDataPortal<tb_LocationTypeEditBindingList> dp)
        {
            return dp.Fetch();
        }

        protected override void OnDeserialized()
        {
            base.OnDeserialized();
            this.Saved += tb_LocationTypes_Saved;
        }

        protected override void DataPortal_OnDataPortalInvokeComplete(Csla.DataPortalEventArgs e)
        {
            if (ApplicationContext.ExecutionLocation == ApplicationContext.ExecutionLocations.Server &&
                e.Operation == DataPortalOperations.Update)
            {
                // this runs on the server and invalidates
                // the RoleList cache
                //tb_LocationTypeList.InvalidateCache();
            }
        }

        [Create, RunLocal]
        private void Create() { }


        [Fetch]
        private void Fetch([Inject] tb_LocationTypeFactory dal)
        {
            using (LoadListMode)
            {
                List<tb_LocationType> list = null;
                list = dal.FetchTList();
                foreach (var item in list)
                {
                    var dest = ApplicationContext.CreateInstanceDI<tb_LocationTypeEditInfo>();
                    Csla.Data.DataMapper.Map(item, dest);
                    //var Mapper = ApplicationContext.GetRequiredService<IMapper>();
                    //dest = Mapper.Map<tb_LocationType, tb_LocationTypeEditInfo>(item);
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