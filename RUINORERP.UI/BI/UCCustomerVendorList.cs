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

using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using SqlSugar;
using Krypton.Navigator;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.SuperSocketClient;
using TransInstruction;
using AutoUpdateTools;
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

            Krypton.Toolkit.KryptonButton button检查数据 = new Krypton.Toolkit.KryptonButton();
            button检查数据.Text = "提取重复数据";
            button检查数据.ToolTipValues.Description = "提取重复数据，有一行会保留，没有显示出来。";
            button检查数据.ToolTipValues.EnableToolTips = true;
            button检查数据.ToolTipValues.Heading = "提示";
            button检查数据.Click += button检查数据_Click;
            base.frm.flowLayoutPanelButtonsArea.Controls.Add(button检查数据);


        }

        private void button检查数据_Click(object sender, EventArgs e)
        {
            ListDataSoure.DataSource = GetDuplicatesList();
            dataGridView1.DataSource = ListDataSoure;
        }

        private void UCCustomerVendorList_Load(object sender, EventArgs e)
        {

        }

        public override async Task<List<tb_CustomerVendor>> Save()
        {
            List<tb_CustomerVendor> list = await base.Save();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.Customer_id.HasValue)
                    {
                        //对象的目标客户设置为已转换
                        item.IsCustomer = true;
                        var result = MainForm.Instance.AppContext.Db.Updateable<tb_CRM_Customer>()
                            .SetColumns(it => it.Converted == true)//SetColumns是可以叠加的 写2个就2个字段赋值
                            .Where(it => it.Customer_id == item.Customer_id.Value)
                            .ExecuteCommandAsync();
                    }

                    if (item.tb_crm_customer != null)
                    {
                        //如果修改了客户名称，则和销售客户同步
                        if (item.CVName != item.tb_crm_customer.CustomerName)
                        {
                            item.tb_crm_customer.CustomerName = item.CVName;
                            var result = MainForm.Instance.AppContext.Db.Updateable<tb_CRM_Customer>(item.tb_crm_customer)
                            .SetColumns(it => it.CustomerName == item.CVName)//SetColumns是可以叠加的 写2个就2个字段赋值
                            .Where(it => it.Customer_id == item.Customer_id.Value)
                            .ExecuteCommandAsync();
                        }

                    }

                }

            }
            return list;
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

            if (CurMenuInfo.CaptionCN.Contains("所有"))
            {
                QueryConditionFilter.FilterLimitExpressions.Clear();
                QueryConditionFilter.FilterLimitExpressions.Add(lambda);
            }
            else
            {
                QueryConditionFilter.FilterLimitExpressions.Add(lambda);
            }




        }


    }
}
