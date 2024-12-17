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
using RUINORERP.UI.AdvancedUIModule;
using Netron.GraphLib;
using RUINORERP.UI.CommonUI;
using RUINORERP.Business.CommService;
using TransInstruction;

namespace RUINORERP.UI.CRM
{

    [MenuAttrAssemblyInfo("目标客户", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.客户管理)]
    public partial class UCCRMCustomerList : BaseForm.BaseListGeneric<tb_CRM_Customer>, IFormAuth
    {
        public UCCRMCustomerList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCRMCustomerEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_Customer, int?>> expLeadsStatus;
            expLeadsStatus = (p) => p.CustomerStatus;
            base.ColNameDataDictionary.TryAdd(expLeadsStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(CustomerStatus)));


            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给

            System.Linq.Expressions.Expression<Func<tb_CRM_Customer, int?>> exp;
            exp = (p) => p.CustomerStatus;
            base.ColNameDataDictionary.TryAdd(exp.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(CustomerStatus)));

        }


 

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_Customer>()
                               .AndIF(CurMenuInfo.CaptionCN.Contains("公海客户"), t => t.Employee_ID == null)
                               .AndIF(CurMenuInfo.CaptionCN.Contains("目标客户"), t => t.Employee_ID != null)
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

            if (CurMenuInfo.CaptionCN.Contains("公海客户"))
            {
                toolStripButtonAdd.Visible = false;
                toolStripButtonDelete.Visible = false;
            }

            AddExtendButton(CurMenuInfo);

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
                        BusinessHelper.Instance.InitEntity(EntityInfo);
                        EntityInfo.Customer_id = customer.Customer_id;
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.加载;
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
                        BusinessHelper.Instance.InitEntity(EntityInfo);
                        EntityInfo.Customer_id = customer.Customer_id;
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.加载;

                        frmaddg.BindData(bty, ActionStatus.新增);
                        if (frmaddg.ShowDialog() == DialogResult.OK)
                        {
                            BaseController<tb_CRM_FollowUpRecords> ctrRecords = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpRecords>>(typeof(tb_CRM_FollowUpRecords).Name + "Controller");
                            ReturnResults<tb_CRM_FollowUpRecords> result = await ctrRecords.BaseSaveOrUpdate(EntityInfo);
                            if (result.Succeeded)
                            {
                                if (customer.CustomerStatus == (int)CustomerStatus.新增客户)
                                {
                                    customer.CustomerStatus = (int)CustomerStatus.潜在客户;
                                    BaseController<tb_CRM_Customer> ctr_Customer = Startup.GetFromFacByName<BaseController<tb_CRM_Customer>>(typeof(tb_CRM_Customer).Name + "Controller");
                                    ReturnResults<tb_CRM_Customer> resultCustomer = await ctr_Customer.BaseSaveOrUpdate(customer);
                                    if (resultCustomer.Succeeded)
                                    {

                                    }
                                }
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
                        mapper.Map(sourceEntity, EntityInfo);  // 直接将 crmLeads 的值映射到传入的 entity 对象上，保持了引用
                                                               // EntityInfo = mapper.Map<tb_CustomerVendor>(sourceEntity);
                        EntityInfo.Customer_id = sourceEntity.Customer_id;
                        EntityInfo.Employee_ID = sourceEntity.Employee_ID;
                        BusinessHelper.Instance.InitEntity(EntityInfo);
                        BaseEntity bty = EntityInfo as BaseEntity;
                        bty.ActionStatus = ActionStatus.新增;

                        frmaddg.BindData(bty, ActionStatus.新增);
                        if (frmaddg.ShowDialog() == DialogResult.OK)
                        {
                            BaseController<tb_CustomerVendor> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CustomerVendor>>(typeof(tb_CustomerVendor).Name + "Controller");
                            ReturnResults<tb_CustomerVendor> result = await ctrContactInfo.BaseSaveOrUpdate(EntityInfo);
                            if (result.Succeeded)
                            {
                                if (result.Succeeded)
                                {
                                    //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                                    //只处理需要缓存的表
                                    if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_CustomerVendor).Name, out pair))
                                    {
                                        //如果有更新变动就上传到服务器再分发到所有客户端
                                        OriginalData odforCache = ActionForClient.更新缓存<tb_CustomerVendor>(result.ReturnObject);
                                        byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                                        MainForm.Instance.ecs.client.Send(buffer);
                                    }
                                }
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



        /// <summary>
        /// 因为客户要查出相关的记录计划这些
        /// </summary>
        /// <param name="UseAutoNavQuery"></param>
        public override void Query(bool UseAutoNavQuery = false)
        {
            base.Query(true);
        }

  

            #region 添加回收 分配

            /// <summary>
            /// 添加回收
            /// </summary>
            /// <returns></returns>
            public ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
            {
                //一个是公海一个是目标客户
                if (menuInfo.CaptionCN.Contains("公海客户"))
                {
                ToolStripButton toolStripButton分配 = new System.Windows.Forms.ToolStripButton();
                toolStripButton分配.Text = "分配";
                toolStripButton分配.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
                toolStripButton分配.ImageTransparentColor = System.Drawing.Color.Magenta;
                toolStripButton分配.Name = "分配AssignmentToBizEmp";
                toolStripButton分配.Visible = false;//默认隐藏
                ControlButton(toolStripButton分配);
                toolStripButton分配.ToolTipText = "分配给指定业务员。";
                toolStripButton分配.Click += new System.EventHandler(this.toolStripButton分配_Click);

                System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton分配 };
                    this.BaseToolStrip.Items.AddRange(extendButtons);
                    return extendButtons;
                }
                else
                {
                ToolStripButton toolStripButton回收 = new System.Windows.Forms.ToolStripButton();
                toolStripButton回收.Text = "回收";
                toolStripButton回收.Image = global::RUINORERP.UI.Properties.Resources.reset;
                toolStripButton回收.ImageTransparentColor = System.Drawing.Color.Magenta;
                toolStripButton回收.Name = "回收RecyclingToHighSeas";
                toolStripButton回收.Visible = false;//默认隐藏
                ControlButton(toolStripButton回收);
                toolStripButton回收.ToolTipText = "回收到公海。";
                toolStripButton回收.Click += new System.EventHandler(this.toolStripButton回收_Click);

                System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton回收 };
                    this.BaseToolStrip.Items.AddRange(extendButtons);
                    return extendButtons;
                }

                // this.BaseToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {            this.toolStripButtonAdd});

            }

            private async void toolStripButton回收_Click(object sender, EventArgs e)
            {
                if (bindingSourceList.Current != null && dataGridView1.CurrentCell != null)
                {
                    //  弹出提示说：您确定将这个公司回收投入到公海吗？
                    if (bindingSourceList.Current is tb_CRM_Customer sourceEntity)
                    {
                        if (MessageBox.Show($"您确定将这个客户：{sourceEntity.CustomerName}回收到公海吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            sourceEntity.Employee_ID = null;
                            int result = await MainForm.Instance.AppContext.Db.Updateable<tb_CRM_Customer>(sourceEntity).ExecuteCommandAsync();
                            if (result > 0)
                            {
                                MainForm.Instance.ShowStatusText("回收成功!");
                                Query();
                            }
                        }
                    }
                }
            }


            private async void toolStripButton分配_Click(object sender, EventArgs e)
            {
                if (bindingSourceList.Current != null && dataGridView1.CurrentCell != null)
                {
                    //  弹出提示说：？
                    if (bindingSourceList.Current is tb_CRM_Customer sourceEntity)
                    {
                        frmSelectObject frm = new frmSelectObject();
                        //frm.selectedObject = sourceEntity;
                        frm.SetSelectDataList<tb_Employee>(sourceEntity, C => C.Employee_ID, n => n.Employee_Name);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            if (MessageBox.Show($"您确定将这个客户：【{sourceEntity.CustomerName}】分配给【{frm.SelectItemText}】吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                int result = await MainForm.Instance.AppContext.Db.Updateable<tb_CRM_Customer>(sourceEntity).ExecuteCommandAsync();
                                if (result > 0)
                                {
                                    MainForm.Instance.ShowStatusText("分配成功!");
                                    Query();
                                }
                            }
                        }
                    }
                }
            }


            #endregion

     
    }
}
