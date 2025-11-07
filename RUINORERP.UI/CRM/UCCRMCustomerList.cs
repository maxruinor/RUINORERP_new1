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

using RUINORERP.Global;
using RUINORERP.Model.TransModel;

using FastReport.DevComponents.DotNetBar;
using RUINORERP.Extensions.Middlewares;


namespace RUINORERP.UI.CRM
{

    public partial class UCCRMCustomerList : BaseForm.BaseListGeneric<tb_CRM_Customer>, IToolStripMenuInfoAuth
    {
        public UCCRMCustomerList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCRMCustomerEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_Customer, int?>> expLeadsStatus;
            expLeadsStatus = (p) => p.CustomerStatus;
            base.ColNameDataDictionary.TryAdd(expLeadsStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(CustomerStatus)));
            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.CustomerStatus, typeof(CustomerStatus));
            //DisplayTextResolver.AddFixedDictionaryMappingByEnum<tb_CRM_Customer>(t => t.CustomerStatus, typeof(CustomerStatus));

            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给

            System.Linq.Expressions.Expression<Func<tb_CRM_Customer, int?>> exp;
            exp = (p) => p.CustomerStatus;
            base.ColNameDataDictionary.TryAdd(exp.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(CustomerStatus)));
            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.CustomerStatus, typeof(CustomerStatus));
        }

        protected override Task<bool> Delete()
        {
            tb_CRM_Customer rowInfo = (tb_CRM_Customer)this.bindingSourceList.Current;
            if (rowInfo.Employee_ID != MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID && !MainForm.Instance.AppContext.IsSuperUser)
            {
                //只能删除自己的收款信息。
                MessageBox.Show("只能删除自己的目标客户信息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return Task.FromResult(false);
            }
            if (
               (rowInfo.tb_CRM_FollowUpPlanses != null && rowInfo.tb_CRM_FollowUpPlanses.Count > 0)
                || (rowInfo.tb_CRM_FollowUpRecordses != null && rowInfo.tb_CRM_FollowUpRecordses.Count > 0)
                || rowInfo.CustomerStatus != (int)CustomerStatus.新增客户)
            {
                //只能删除自己的信息。
                MessageBox.Show("只有【新增客户】,并且没有任何跟进信息时才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return Task.FromResult(false);
            }
            return base.Delete();
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


            if (CurMenuInfo.CaptionCN.Contains("公海客户"))
            {
                toolStripButtonAdd.Visible = false;
                toolStripButtonDelete.Visible = false;
            }
            else
            {
                //目标客户才可以转
                ContextMenuStrip newContextMenuStrip = base.dataGridView1.GetContextMenu(contextMenuStrip1);
                base.dataGridView1.ContextMenuStrip = newContextMenuStrip;
            }
            //基类添加了。这里重复了
            //AddExtendButton(CurMenuInfo);

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
                        frmaddg.CurMenuInfo = this.CurMenuInfo;
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
                        frmaddg.CurMenuInfo = this.CurMenuInfo;
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
                    if (sourceEntity.Converted.HasValue && sourceEntity.Converted.Value)
                    {
                        //提示当前目标客户已经转换为销售客户。不能重复转换。
                        MessageBox.Show("当前目标客户已经转换为销售客户，不能重复转换", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    object frm = Activator.CreateInstance(typeof(UCCustomerVendorEdit));
                    if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                    {
                        BaseEditGeneric<tb_CustomerVendor> frmaddg = frm as BaseEditGeneric<tb_CustomerVendor>;
                        frmaddg.CurMenuInfo = this.CurMenuInfo;
                        frmaddg.Text = "销售客户编辑";
                        frmaddg.bindingSourceEdit.DataSource = new List<tb_CustomerVendor>();
                        object obj = frmaddg.bindingSourceEdit.AddNew();
                        tb_CustomerVendor EntityInfo = obj as tb_CustomerVendor;

                        MainForm.Instance.mapper.Map(sourceEntity, EntityInfo);  // 直接将 crmLeads 的值映射到传入的 entity 对象上，保持了引用
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
                                sourceEntity.Converted = true;
                                BaseController<tb_CRM_Customer> ctr_Customer = Startup.GetFromFacByName<BaseController<tb_CRM_Customer>>(typeof(tb_CRM_Customer).Name + "Controller");
                                ReturnResults<tb_CRM_Customer> resultCustomer = await ctr_Customer.BaseSaveOrUpdate(sourceEntity);
                                if (resultCustomer.Succeeded)
                                {
                                    base._eventDrivenCacheManager.UpdateEntity<tb_CustomerVendor>(result.ReturnObject);
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

        public override async Task<List<tb_CRM_Customer>> Save()
        {
            List<tb_CRM_Customer> list = new List<tb_CRM_Customer>();
            try
            {
                list = await base.Save();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("AK_KEY_CUSTOMERNAME_TB_CRM_C"))
                {
                    MessageBox.Show("客户名称不能重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    throw;
                }

            }

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.Converted.HasValue && item.Converted.Value)
                    {
                        //对象的目标客户设置为已转换
                        var result = await MainForm.Instance.AppContext.Db.Queryable<tb_CustomerVendor>()
                            .Where(it => it.Customer_id == item.Customer_id)
                            .SingleAsync();

                        //如果修改了客户名称，则和销售客户同步
                        if (result != null && result.CVName != item.CustomerName)
                        {
                            result.CVName = item.CustomerName;
                            result.Phone = item.Contact_Phone;
                            await MainForm.Instance.AppContext.Db.Updateable<tb_CustomerVendor>(result)
                                .UpdateColumns(it => new { it.CVName, it.Phone })
                            //.SetColumns(it => it.CVName == item.CustomerName)//SetColumns是可以叠加的 写2个就2个字段赋值
                            .Where(it => it.Customer_id == item.Customer_id)
                            .ExecuteCommandAsync();
                        }
                    }
                }
            }
            return list;
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
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            //一个是公海一个是目标客户
            if (menuInfo.CaptionCN.Contains("公海客户"))
            {
                ToolStripButton toolStripButton分配 = new System.Windows.Forms.ToolStripButton();
                toolStripButton分配.Text = "分配";
                toolStripButton分配.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
                toolStripButton分配.ImageTransparentColor = System.Drawing.Color.Magenta;
                toolStripButton分配.Name = "分配AssignmentToBizEmp";
                if (MainForm.Instance.AppContext.IsSuperUser)
                {
                    toolStripButton分配.Visible = true;//默认
                }
                else
                {
                    toolStripButton分配.Visible = false;//默认隐藏
                }

                UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton分配);
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

                if (MainForm.Instance.AppContext.IsSuperUser)
                {
                    toolStripButton回收.Visible = true;//默认
                }
                else
                {
                    toolStripButton回收.Visible = false;//默认隐藏
                }
                UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton回收);
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
            UIHelper.CheckValidation(this);
            List<tb_CRM_Customer> updateList = new List<tb_CRM_Customer>();

            //多选模式时
            if (dataGridView1.UseSelectedColumn)
            {
                foreach (var item in bindingSourceList)
                {
                    if (item is tb_CRM_Customer sourceEntity)
                    {
                        if (sourceEntity.Employee_ID.HasValue && sourceEntity.Employee_ID.Value > 0 && sourceEntity.Selected.HasValue && sourceEntity.Selected.Value)
                        {
                            updateList.Add(sourceEntity);
                        }

                    }
                }
            }
            else
            {

                if (bindingSourceList.Current != null && dataGridView1.CurrentCell != null)
                {
                    if (bindingSourceList.Current is tb_CRM_Customer sourceEntity)
                    {
                        updateList.Add(sourceEntity);
                    }
                }
            }

            if (updateList.Count > 0)
            {
                string msg = string.Empty;
                int counter = 0;
                msg = "\r\n";
                foreach (var item in updateList)
                {
                    counter++;
                    //将客户的名称 添加到提示信息中。并\r\n分开
                    msg += "【" + item.CustomerName + "】" + "\r\n";
                    if (counter > 10)
                    {
                        msg += $".... 等 {updateList.Count}位客户";
                        break;
                    }
                }
                ////去掉最后的\r\n
                //msg = msg.TrimEnd('\r', '\n');

                if (MessageBox.Show($"您确定将：{msg}回收到公海吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    updateList.ForEach(c => c.Employee_ID = null);
                    int result = await MainForm.Instance.AppContext.Db.Updateable(updateList).UpdateColumns(it => new { it.Employee_ID }).ExecuteCommandAsync();
                    if (result > 0)
                    {
                        MainForm.Instance.ShowStatusText($"回收成功{result}条数据!");
                        Query();
                    }
                }
            }
        }


        private async void toolStripButton分配_Click(object sender, EventArgs e)
        {
            UIHelper.CheckValidation(this);
            List<tb_CRM_Customer> updateList = new List<tb_CRM_Customer>();

            //多选模式时
            if (dataGridView1.UseSelectedColumn)
            {
                foreach (var item in bindingSourceList)
                {
                    if (item is tb_CRM_Customer sourceEntity)
                    {
                        if (sourceEntity.Employee_ID == null && sourceEntity.Selected.HasValue && sourceEntity.Selected.Value)
                        {
                            updateList.Add(sourceEntity);
                        }

                    }
                }
            }
            else
            {

                if (bindingSourceList.Current != null && dataGridView1.CurrentCell != null)
                {
                    if (bindingSourceList.Current is tb_CRM_Customer sourceEntity)
                    {
                        updateList.Add(sourceEntity);
                    }
                }
            }

            if (updateList.Count > 0)
            {
                frmSelectObject frm = new frmSelectObject();
                frm.SetSelectDataList<tb_Employee>(updateList[0], C => C.Employee_ID, n => n.Employee_Name);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    long empID = updateList[0].Employee_ID.Value;
                    string msg = string.Empty;
                    int counter = 0;
                    msg = "\r\n";
                    foreach (var item in updateList)
                    {
                        counter++;
                        //将客户的名称 添加到提示信息中。并\r\n分开
                        msg += "【" + item.CustomerName + "】" + "\r\n";
                        if (counter > 10)
                        {
                            msg += $".... 等 {updateList.Count}位客户";
                            break;
                        }
                    }
                    ////去掉最后的\r\n
                    //msg = msg.TrimEnd('\r', '\n');
                    updateList.ForEach(c => c.Employee_ID = empID);
                    if (MessageBox.Show($"您确定将：{msg}分配给【{frm.SelectItemText}】吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        try
                        {
                            await MainForm.Instance.AppContext.Db.Ado.BeginTranAsync();
                            int result = await MainForm.Instance.AppContext.Db.Updateable(updateList).UpdateColumns(it => new { it.Employee_ID }).ExecuteCommandAsync();
                            if (result > 0)
                            {
                                //同时重新分配客户表中的责任人
                                long[] Crm_cusids = updateList.Select(c => c.Customer_id).ToArray();
                                var CustomerVendors = await MainForm.Instance.AppContext.Db.Queryable<tb_CustomerVendor>().Where(c => Crm_cusids.Contains(c.Customer_id.Value)).ToListAsync();
                                CustomerVendors.ForEach(c => c.Employee_ID = empID);
                                await MainForm.Instance.AppContext.Db.Updateable(CustomerVendors).UpdateColumns(it => new { it.Employee_ID }).ExecuteCommandAsync();
                                await MainForm.Instance.AppContext.Db.Ado.CommitTranAsync();
                                MainForm.Instance.ShowStatusText($"分配成功{result}条数据!");
                                Query();
                            }

                        }
                        catch (Exception ex)
                        {
                            await MainForm.Instance.AppContext.Db.Ado.RollbackTranAsync();
                            MainForm.Instance.logManager.AddLog("错误", $"分配失败{ex.Message}!");
                        }
                    }
                    else
                    {
                        //恢复
                        updateList.ForEach(c => c.Employee_ID = null);
                    }
                }

            }
        }


        #endregion


    }
}
