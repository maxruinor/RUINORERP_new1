using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.UI.ToolForm;
using RUINORERP.UI.SmartReminderClient;
using RUINORERP.Model.ReminderModel;
using Netron.GraphLib;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RUINORERP.Common.Helper;
using RUINORERP.Business.CommService;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using NPOI.OpenXmlFormats.Spreadsheet;



namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("提醒规则编辑", true, UIType.单表数据)]
    public partial class UCReminderRuleEdit : BaseEditGeneric<tb_ReminderRule>
    {
        public UCReminderRuleEdit()
        {
            InitializeComponent();
        }


        public tb_ReminderRule entity { get; set; }

        List<tb_UserInfo> UserInfos = new List<tb_UserInfo>();
        // 链路关联相关字段
        private tb_ReminderObjectLinkController<tb_ReminderObjectLink> _linkController;
        private tb_ReminderLinkRuleRelationController<tb_ReminderLinkRuleRelation> _relationController;
        private List<tb_ReminderObjectLink> _linkedLinks;

        public override void BindData(BaseEntity _entity)
        {
            entity = _entity as tb_ReminderRule;
            if (entity.RuleId == 0)
            {
                entity.EffectiveDate = System.DateTime.Now;
                entity.ExpireDate = System.DateTime.Now.AddDays(60);
                entity.Created_at = DateTime.UtcNow;
                entity.ReminderPriority = (int)ReminderPriority.Medium;
                BusinessHelper.Instance.InitEntity(entity);
            }
            else
            {
                BusinessHelper.Instance.EditEntity(entity);
            }

            // 初始化控制器
            _linkController = MainForm.Instance.AppContext.GetRequiredService<tb_ReminderObjectLinkController<tb_ReminderObjectLink>>();
            _relationController = MainForm.Instance.AppContext.GetRequiredService<tb_ReminderLinkRuleRelationController<tb_ReminderLinkRuleRelation>>();

            UserInfos = _cacheManager.GetEntityList<tb_UserInfo>();

            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.RuleName, txtRuleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_ReminderRule, RuleEngineType>(entity, k => k.RuleEngineType, cmbRuleEngineType, false);

            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4CmbByEnum<tb_ReminderRule, ReminderBizType>(entity, k => k.ReminderBizType, cmbReminderBizType, false);

            DataBindingHelper.BindData4CmbByEnum<tb_ReminderRule, ReminderPriority>(entity, k => k.ReminderPriority, cmbPriority, false);

            DataBindingHelper.BindData4CheckBox<tb_ReminderRule>(entity, t => t.IsEnabled, chkIsEnabled, false);
            DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.EffectiveDate, dtpEffectiveDate, false);

            CheckedListBoxHelper.BindData4CheckedListBox<tb_ReminderRule, NotifyChannel>(entity, t => t.NotifyChannels, clbNotifyChannels, NotifyChannel.Workflow, NotifyChannel.SMS);

            // 绑定到CheckedListBox
            CheckedListBoxHelper.BindData4CheckedListBox(
                entity: entity,
                propertyExpression: e => e.NotifyRecipients,
                checkedList: txtNotifyRecipientNames,
                dataSource: UserInfos,
                idExpression: u => u.User_ID,
                displayExpression: u => u.UserName,
                excludeIds: 0   // 排除系统用户
            );

            DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.ExpireDate, dtpExpireDate, false);

            if (entity != null && !string.IsNullOrEmpty(entity.JsonConfig))
            {
                jsonViewer1.LoadAuditData(entity.JsonConfig);
            }

            // 加载关联的链路
            LoadLinkedLinks();

            // 绑定按钮事件
            BindLinkButtons();

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ReminderRuleValidator>(), kryptonPanel1.Controls);
                //base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (entity == null)
                {
                    return;
                }

                //权限允许
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    //if (s2.PropertyName == entity.GetPropertyName<tb_ReminderRule>(c => c.ReminderBizType) || s2.PropertyName == entity.GetPropertyName<tb_ReminderRule>(c => c.ReminderBizType > 0))
                    //{
                    //    //加载对应的配置窗体
                    //    entity.BusinessConfig = LoadBusinessConfig(entity.ReminderBizType);
                    //}

                }

            };


            base.BindData(entity);
        }

        /// <summary>
        /// 绑定链路相关按钮事件
        /// </summary>
        private void BindLinkButtons()
        {
            btnAddLink.Click += btnAddLink_Click;
            btnRemoveLink.Click += btnRemoveLink_Click;
        }

        /// <summary>
        /// 加载已关联的链路
        /// </summary>
        private async void LoadLinkedLinks()
        {
            try
            {
                if (entity == null || entity.RuleId == 0)
                {
                    dgvLinkedLinks.DataSource = new List<tb_ReminderObjectLink>();
                    return;
                }

                // 查询关联关系
                var relations = await _relationController.QueryAsync(r => r.RuleId == entity.RuleId);
                if (relations == null || relations.Count == 0)
                {
                    dgvLinkedLinks.DataSource = new List<tb_ReminderObjectLink>();
                    return;
                }

                // 查询关联的链路
                var linkIds = relations.Select(r => r.LinkId).ToList();
                _linkedLinks = await _linkController.QueryAsync(l => linkIds.Contains(l.LinkId));
                dgvLinkedLinks.DataSource = _linkedLinks;

                // 设置列标题
                if (dgvLinkedLinks.Columns.Count > 0)
                {
                    dgvLinkedLinks.Columns["LinkId"].HeaderText = "链路ID";
                    dgvLinkedLinks.Columns["LinkName"].HeaderText = "链路名称";
                    dgvLinkedLinks.Columns["Description"].HeaderText = "链路描述";
                    dgvLinkedLinks.Columns["SourceType"].HeaderText = "提醒源类型";
                    dgvLinkedLinks.Columns["BizType"].HeaderText = "单据类型";
                    dgvLinkedLinks.Columns["ActionType"].HeaderText = "操作类型";
                    dgvLinkedLinks.Columns["TargetType"].HeaderText = "提醒目标类型";
                    dgvLinkedLinks.Columns["IsEnabled"].HeaderText = "是否启用";

                    // 隐藏不必要的列
                    dgvLinkedLinks.Columns["SourceValue"].Visible = false;
                    dgvLinkedLinks.Columns["TargetValue"].Visible = false;
                    dgvLinkedLinks.Columns["BillStatus"].Visible = false;
                    dgvLinkedLinks.Columns["Created_at"].Visible = false;
                    dgvLinkedLinks.Columns["CreateUserId"].Visible = false;
                    dgvLinkedLinks.Columns["Updated_at"].Visible = false;
                    dgvLinkedLinks.Columns["UpdateUserId"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载关联链路失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加链路按钮点击事件
        /// </summary>
        private async void btnAddLink_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new frmReminderLinkConfig())
                {
                    if (form.ShowDialog() == DialogResult.OK && form.SelectedLinkId > 0)
                    {
                        // 检查是否已关联
                        if (_linkedLinks != null && _linkedLinks.Any(l => l.LinkId == form.SelectedLinkId))
                        {
                            MessageBox.Show("该链路已关联，无需重复添加", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // 查询链路信息
                        var link = await _linkController.BaseQueryByIdNavAsync(form.SelectedLinkId);
                        if (link != null)
                        {
                            // 添加到本地列表
                            if (_linkedLinks == null)
                            {
                                _linkedLinks = new List<tb_ReminderObjectLink>();
                            }
                            _linkedLinks.Add(link);

                            // 更新数据源
                            dgvLinkedLinks.DataSource = null;
                            dgvLinkedLinks.DataSource = _linkedLinks;

                            // 设置列标题
                            if (dgvLinkedLinks.Columns.Count > 0)
                            {
                                dgvLinkedLinks.Columns["LinkId"].HeaderText = "链路ID";
                                dgvLinkedLinks.Columns["LinkName"].HeaderText = "链路名称";
                                dgvLinkedLinks.Columns["Description"].HeaderText = "链路描述";
                                dgvLinkedLinks.Columns["SourceType"].HeaderText = "提醒源类型";
                                dgvLinkedLinks.Columns["BizType"].HeaderText = "单据类型";
                                dgvLinkedLinks.Columns["ActionType"].HeaderText = "操作类型";
                                dgvLinkedLinks.Columns["TargetType"].HeaderText = "提醒目标类型";
                                dgvLinkedLinks.Columns["IsEnabled"].HeaderText = "是否启用";

                                // 隐藏不必要的列
                                dgvLinkedLinks.Columns["SourceValue"].Visible = false;
                                dgvLinkedLinks.Columns["TargetValue"].Visible = false;
                                dgvLinkedLinks.Columns["BillStatus"].Visible = false;
                                dgvLinkedLinks.Columns["Created_at"].Visible = false;
                                dgvLinkedLinks.Columns["CreateUserId"].Visible = false;
                                dgvLinkedLinks.Columns["Updated_at"].Visible = false;
                                dgvLinkedLinks.Columns["UpdateUserId"].Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加链路失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 移除链路按钮点击事件
        /// </summary>
        private void btnRemoveLink_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLinkedLinks.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvLinkedLinks.SelectedRows[0];
                    if (selectedRow.DataBoundItem is tb_ReminderObjectLink selectedLink)
                    {
                        if (MessageBox.Show($"确定要移除链路 '{selectedLink.LinkName}' 吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // 从本地列表中移除
                            _linkedLinks.Remove(selectedLink);

                            // 更新数据源
                            dgvLinkedLinks.DataSource = null;
                            dgvLinkedLinks.DataSource = _linkedLinks;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择要移除的链路", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移除链路失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private object LoadBusinessConfig(int reminderBizType)
        {
            ReminderBizType reminderBiz = (ReminderBizType)reminderBizType;
            switch (reminderBiz)
            {
                case ReminderBizType.安全库存提醒:
                    var ucSafetyStockConfigEdit = new UCSafetyStockConfigEdit();
                    ucSafetyStockConfigEdit.Text = "安全库存提醒配置";

                    if (entity.JsonConfig.IsNullOrEmpty())
                    {
                        entity.JsonConfig = "{}";
                    }

                    JObject obj = JsonHelper.SafeParseJson(entity.JsonConfig);

                    SafetyStockConfig safetyStockConfig = obj.ToObject<SafetyStockConfig>();
                    if (safetyStockConfig == null)
                    {
                        safetyStockConfig = new SafetyStockConfig();
                    }
                    ucSafetyStockConfigEdit.safetyStockConfig = safetyStockConfig;
                    ucSafetyStockConfigEdit.bindingSourceEdit.DataSource = new List<SafetyStockConfig>() { safetyStockConfig };
                    if (ucSafetyStockConfigEdit.ShowDialog() == DialogResult.OK)
                    {


                        return ucSafetyStockConfigEdit.safetyStockConfig;
                    }
                    break;

                case ReminderBizType.单据提交审批提醒:
                    var ucDocumentApprovalConfigEdit = new UCDocumentApprovalConfigEdit();
                    ucDocumentApprovalConfigEdit.Text = "单据提交审批提醒配置";

                    if (entity.JsonConfig.IsNullOrEmpty())
                    {
                        entity.JsonConfig = "{}";
                    }

                    JObject docObj = JsonHelper.SafeParseJson(entity.JsonConfig);

                    DocApprovalConfig docApprovalConfig = docObj.ToObject<DocApprovalConfig>();
                    if (docApprovalConfig == null)
                    {
                        docApprovalConfig = new DocApprovalConfig();
                    }
                    ucDocumentApprovalConfigEdit.docApprovalConfig = docApprovalConfig;
                    ucDocumentApprovalConfigEdit.bindingSourceEdit.DataSource = new List<DocApprovalConfig>() { docApprovalConfig };
                    if (ucDocumentApprovalConfigEdit.ShowDialog() == DialogResult.OK)
                    {
                        return ucDocumentApprovalConfigEdit.docApprovalConfig;
                    }
                    break;

                default:
                    break;
            }

            return null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private async void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();

                if (bindingSourceEdit.Current is tb_ReminderRule reminderRule)
                {
                    if (reminderRule.NotifyRecipients.Count == 0)
                    {
                        MessageBox.Show("通知接收人员不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // 保存关联关系
                    await SaveLinkedLinksAsync(reminderRule);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 保存关联的链路关系
        /// </summary>
        /// <param name="rule">提醒规则</param>
        private async Task SaveLinkedLinksAsync(tb_ReminderRule rule)
        {
            try
            {
                if (rule == null || rule.RuleId == 0)
                    return;

                // 删除现有关联
                var existingRelations = await _relationController.QueryAsync(r => r.RuleId == rule.RuleId);
                if (existingRelations != null && existingRelations.Count > 0)
                {
                    await _relationController.BaseDeleteAsync(existingRelations);
                }

                // 保存新关联
                if (_linkedLinks != null && _linkedLinks.Count > 0)
                {
                    var newRelations = _linkedLinks.Select(link => new tb_ReminderLinkRuleRelation
                    {
                        RuleId = rule.RuleId,
                        LinkId = link.LinkId,
                        Created_at = DateTime.UtcNow,
                        Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID
                    }).ToList();

                    await _relationController.AddAsync(newRelations);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存关联关系失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConfigParser_Click(object sender, EventArgs e)
        {

            if (entity.ReminderBizType == 0)
            {
                MessageBox.Show("请选择提醒业务类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ConfigObject = LoadBusinessConfig(entity.ReminderBizType);
            if (ConfigObject != null)
            {
                //发送缓存数据
                string json = JsonConvert.SerializeObject(ConfigObject,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                   });

                entity.JsonConfig = json;
                jsonViewer1.LoadAuditData(entity.JsonConfig);
            }
        }

        private async void btnSelectNotifyRecipients_Click(object sender, EventArgs e)
        {
            var userInfoController = MainForm.Instance.AppContext.GetRequiredService<tb_UserInfoController<tb_UserInfo>>();

            // 2. 查找可抵扣的预收付款单
            var SelectedUserInfo = await userInfoController.QueryAsync();
            if (!SelectedUserInfo.Any())
            {
                MessageBox.Show("没有可以选择的接收用户！");
                return;
            }
            // 初始化选择器
            using (var selector = new frmAdvanceSelector<tb_UserInfo>())
            {

                selector.ConfirmButtonText = "确认";
                selector.AllowMultiSelect = true;

                // 使用表达式树配置列映射
                selector.ConfigureColumn(x => x.UserName, "用户名");
                selector.InitializeSelector(SelectedUserInfo, $"选择接收人员");

                // 设置金额格式化
                //selector.SetColumnFormatter("Amount", value => $"{value:N2}");
                //selector.SetColumnFormatter("RemainAmount", value => $"{value:N2}");

                if (selector.ShowDialog() == DialogResult.OK)
                {
                    //取前5000个。实际是全部。如果要指定则按实际情况改成指定个数
                    var SelectUsers = selector.SelectedItems;
                    string SelectedNames = string.Join(", ", SelectUsers.Take(5000).Select(item => item.UserName));
                    SelectedNames = SelectedNames.TrimEnd(',');

                    //string SelectedIds = string.Join(", ", SelectUsers.Take(5000).Select(item => item.User_ID));
                    //SelectedIds = SelectedIds.TrimEnd(',');

                    if (SelectUsers.Count > 0)
                    {
                        entity.NotifyRecipientDisplayText = SelectedNames;
                        entity.NotifyRecipients = SelectUsers.Take(5000).Select(item => item.User_ID).ToList();
                    }
                }
            }
        }

       

        private  void UCReminderRuleEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
