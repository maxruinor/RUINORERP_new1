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
using NPOI.SS.Formula.Functions;
namespace RUINORERP.UI.BI
{

    /// <summary>
    /// 新的基本资料编辑 并且实现了高级查询 
    /// 客户和供应商共用了 ，特殊处理， 菜单中文标题中有客户两字的。认为是客户，反之有供应商的，就是供应商
    /// </summary>

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
            List<tb_CustomerVendor> list = new List<tb_CustomerVendor>();
            var ctr = MainForm.Instance.AppContext.GetRequiredService<tb_CustomerVendorController<tb_CustomerVendor>>();
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_CustomerVendor;
                switch (entity.ActionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                    case ActionStatus.修改:
                        ReturnResults<tb_CustomerVendor> rr = new ReturnResults<tb_CustomerVendor>();
                        rr = await ctr.SaveOrUpdate(entity);
                        if (rr.Succeeded)
                        {
                            entity.AcceptChanges();
                            if (entity.Customer_id.HasValue && entity.Customer_id.Value > 0)
                            {
                                //同步名称的修改
                                //entity.tb_crm_customer.CustomerName = entity.CVName;
                                //entity.tb_crm_customer.Converted = true;
                                //var result = await MainForm.Instance.AppContext.Db.Updateable<tb_CRM_Customer>(entity.tb_crm_customer)
                                //    .UpdateColumns(it => new { it.CustomerName,it.Converted })
                                //// .SetColumns(it => it.CustomerName == entity.tb_crm_customer)//SetColumns是可以叠加的 写2个就2个字段赋值
                                //.Where(it => it.Customer_id == entity.Customer_id.Value)
                                //.ExecuteCommandAsync();
                                long cid= entity.Customer_id.Value;
                                 
                                var result = await MainForm.Instance.AppContext.Db.Updateable<tb_CRM_Customer>()
                                 .Where(it => it.Customer_id == cid)
                                 .SetColumns(it => it.CustomerName == entity.CVName)//SetColumns是可以叠加的 写2个就2个字段赋值
                                 .SetColumns(it => it.Converted == true)//SetColumns是可以叠加的 写2个就2个字段赋值
                                .ExecuteCommandHasChangeAsync();


                            }


                            list.Add(rr.ReturnObject);
                            ToolBarEnabledControl(MenuItemEnums.保存);
                            MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_CustomerVendor>("保存", rr.ReturnObject);
                        }

                        break;
                    case ActionStatus.删除:
                        break;
                    default:
                        break;
                }
                entity.HasChanged = false;
            }
            return list;
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
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
