using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using RUINORERP.Model.QueryDto;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using SqlSugar;
using Krypton.Navigator;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
namespace RUINORERP.UI.BI
{

    /// <summary>
    /// 新的基本资料编辑 并且实现了高级查询 
    /// 客户和供应商共用了 ，特殊处理， 菜单中文标题中有客户两字的。认为是客户，反之有供应商的，就是供应商
    /// </summary>
    [MenuAttrAssemblyInfo("往来单位", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.供销资料)]
    public partial class UCCustomerVendorList : BaseForm.BaseListGeneric<tb_CustomerVendor>
    {
        public UCCustomerVendorList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCustomerVendorEdit);
            this.Load += UCCustomerVendorList_Load;
        }

        private void UCCustomerVendorList_Load(object sender, EventArgs e)
        {

        }


        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {

            //创建表达式
            //var lambda = Expressionable.Create<tb_CustomerVendor>()
            //                .AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
            //                 .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
            //                  .AndIF(CurMenuInfo.CaptionCN.Contains("其他"), t => t.IsVendor == false && t.IsCustomer == false)
            //                  .And(t => t.isdeleted == false)
            //                  .And(t => t.Is_available == true)
            //                  .AndIF(AuthorizeController.GetExclusiveLimitedAuth(MainForm.Instance.AppContext), t => t.IsExclusive == false)
            //                   .And(t => t.Is_enabled == true)
            //                   .AndIF(
            //    (AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && CurMenuInfo.CaptionCN.Contains("供应商"))
            //    ||
            //    (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && CurMenuInfo.CaptionCN.Contains("客户")),
            //    t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了销售只看到自己的客户,采购不限制
            //      .OrIF(AuthorizeController.GetExclusiveLimitedAuth(MainForm.Instance.AppContext), t => t.IsExclusive == true && t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
            //                .ToExpression();//注意 这一句 不能少
            //base.LimitQueryConditions = lambda;
            //QueryConditionFilter.FilterLimitExpression = lambda;


            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                              .AndIF(CurMenuInfo.CaptionCN.Contains("其他"), t => t.IsVendor == false && t.IsCustomer == false)
                               .AndIF(
                (AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && CurMenuInfo.CaptionCN.Contains("供应商"))
                ||
                (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && CurMenuInfo.CaptionCN.Contains("客户")),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少

            //QueryConditionFilter.FilterLimitExpression = lambda;
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);



        }

    }
}
