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
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Processor;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.UI.BaseForm;
using AutoMapper;
using RUINORERP.UI.BI;
using RUINORERP.Business.AutoMapper;

namespace RUINORERP.UI.CRM
{

    [MenuAttrAssemblyInfo("目标客户", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.客户管理)]
    public partial class UCCRMCustomerList : BaseForm.BaseListGeneric<tb_CRM_Customer>
    {
        public UCCRMCustomerList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCRMCustomerEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_Customer, int?>> expLeadsStatus;
            expLeadsStatus = (p) => p.CustomerStatus;
            base.ColNameDataDictionary.TryAdd(expLeadsStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(CustomerStatus)));

            if (CurMenuInfo.CaptionCN.Contains("公海客户"))
            {
                toolStripButtonAdd.Visible = false;
                toolStripButtonDelete.Visible = false;
            }
        }

        //public override void QueryConditionBuilder()
        //{
        //    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Customer).Name + "Processor");
        //    QueryConditionFilter = baseProcessor.GetQueryFilter();
        //}

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_Customer>()
                               .AndIF(CurMenuInfo.CaptionCN.Contains("公海客户"), t => t.Employee_ID.Value == 0)
                               .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext) && CurMenuInfo.CaptionCN.Contains("目标客户"),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了销售只看到自己的客户,采 
            .ToExpression();    //拥有权控制

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        private void UCCRMCustomerList_Load(object sender, EventArgs e)
        {
            //base.dataGridView1.Use是否使用内置右键功能 = false;
            ContextMenuStrip newContextMenuStrip = base.dataGridView1.GetContextMenu(contextMenuStrip1);
            base.dataGridView1.ContextMenuStrip = newContextMenuStrip;
        }

        private async void 添加跟进计划ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (base.bindingSourceList.Current != null)
            {
                if (bindingSourceList.Current is tb_CRM_Customer customer)
                {
                    object frm = Activator.CreateInstance(typeof(UCCRMFollowUpPlansEdit));
                    if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                    {
                        BaseEditGeneric<tb_CRM_FollowUpPlans> frmaddg = frm as BaseEditGeneric<tb_CRM_FollowUpPlans>;
                        frmaddg.Text = "跟进计划编辑";
                        frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_FollowUpPlans>();
                        object obj = frmaddg.bindingSourceEdit.AddNew();
                        tb_CRM_FollowUpPlans EntityInfo = obj as tb_CRM_FollowUpPlans;
                        EntityInfo.Customer_id = customer.Customer_id;
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.加载;
                        BusinessHelper.Instance.EditEntity(bty);
                        frmaddg.BindData(bty, ActionStatus.新增);
                        if (frmaddg.ShowDialog() == DialogResult.OK)
                        {
                            BaseController<tb_CRM_FollowUpPlans> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpPlans>>(typeof(tb_CRM_FollowUpPlans).Name + "Controller");
                            ReturnResults<tb_CRM_FollowUpPlans> result = await ctrContactInfo.BaseSaveOrUpdate(EntityInfo);
                            if (result.Succeeded)
                            {
                                MainForm.Instance.ShowStatusText("添加成功!");
                            }
                            else
                            {
                                MainForm.Instance.ShowStatusText("添加失败!");
                            }
                        }
                    }
                }

            }

        }

        private async void 添加跟进记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (base.bindingSourceList.Current != null)
            {
                if (bindingSourceList.Current is tb_CRM_Customer customer)
                {
                    object frm = Activator.CreateInstance(typeof(UCCRMFollowUpRecordsEdit));
                    if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                    {
                        BaseEditGeneric<tb_CRM_FollowUpRecords> frmaddg = frm as BaseEditGeneric<tb_CRM_FollowUpRecords>;
                        frmaddg.Text = "跟进记录编辑";
                        frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_FollowUpRecords>();
                        object obj = frmaddg.bindingSourceEdit.AddNew();
                        tb_CRM_FollowUpRecords EntityInfo = obj as tb_CRM_FollowUpRecords;
                        EntityInfo.Customer_id = customer.Customer_id;
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.加载;
                        BusinessHelper.Instance.EditEntity(bty);
                        frmaddg.BindData(bty, ActionStatus.新增);
                        if (frmaddg.ShowDialog() == DialogResult.OK)
                        {
                            BaseController<tb_CRM_FollowUpRecords> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpRecords>>(typeof(tb_CRM_FollowUpRecords).Name + "Controller");
                            ReturnResults<tb_CRM_FollowUpRecords> result = await ctrContactInfo.BaseSaveOrUpdate(EntityInfo);
                            if (result.Succeeded)
                            {
                                MainForm.Instance.ShowStatusText("添加成功!");
                            }
                            else
                            {
                                MainForm.Instance.ShowStatusText("添加失败!");
                            }
                        }
                    }
                }

            }
        }

        private async void 转为销售客户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (base.bindingSourceList.Current != null)
            {
                if (bindingSourceList.Current is tb_CRM_Customer sourceEntity)
                {
                    object frm = Activator.CreateInstance(typeof(UCCustomerVendorEdit));
                    if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                    {
                        BaseEditGeneric<tb_CustomerVendor> frmaddg = frm as BaseEditGeneric<tb_CustomerVendor>;
                        frmaddg.Text = "销售客户编辑";
                        frmaddg.bindingSourceEdit.DataSource = new List<tb_CustomerVendor>();
                        object obj = frmaddg.bindingSourceEdit.AddNew();
                        tb_CustomerVendor EntityInfo = obj as tb_CustomerVendor;
                        IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                        EntityInfo = mapper.Map<tb_CustomerVendor>(sourceEntity);
                        EntityInfo.Customer_id = sourceEntity.Customer_id;
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.新增;
                        BusinessHelper.Instance.EditEntity(bty);
                        frmaddg.BindData(bty, ActionStatus.新增);
                        if (frmaddg.ShowDialog() == DialogResult.OK)
                        {
                            BaseController<tb_CustomerVendor> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CustomerVendor>>(typeof(tb_CustomerVendor).Name + "Controller");
                            ReturnResults<tb_CustomerVendor> result = await ctrContactInfo.BaseSaveOrUpdate(EntityInfo);
                            if (result.Succeeded)
                            {
                                MainForm.Instance.ShowStatusText("添加成功!");
                            }
                            else
                            {
                                MainForm.Instance.ShowStatusText("添加失败!");
                            }
                        }
                    }
                }
            }
        }
    }
}
