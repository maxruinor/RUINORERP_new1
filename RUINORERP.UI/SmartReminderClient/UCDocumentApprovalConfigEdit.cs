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
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.CommService;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using HLH.WinControl.MyTypeConverter;
using System.Linq.Expressions;

namespace RUINORERP.UI.SmartReminderClient
{
    [MenuAttrAssemblyInfo("单据审批配置编辑", true, UIType.单表数据)]
    public partial class UCDocumentApprovalConfigEdit : BaseEditGeneric<DocApprovalConfig>
    {
        public UCDocumentApprovalConfigEdit()
        {
            InitializeComponent();
        }

        public DocApprovalConfig docApprovalConfig { get; set; }

        /// <summary>
        /// 绑定数据到控件
        /// </summary>
        public void BindData(DocApprovalConfig entity)
        {
            if (entity == null)
            {
                entity = new DocApprovalConfig();
            }
            docApprovalConfig = entity;

            // 绑定检测频率
            DataBindingHelper.BindData4TextBox<DocApprovalConfig>(entity, t => t.CheckIntervalByMinutes, txtCheckIntervalByMinutes, BindDataType4TextBox.Qty, false);

            // 绑定单据类型
            BindDocumentTypes();

            // 绑定目标状态下拉框
            BindTargetStatus();

            // 绑定接收人员、角色
            BindRecipients();

            // 绑定触发时机复选框
            BindTriggerTiming();
        }

        /// <summary>
        /// 绑定单据类型到DataGridView
        /// </summary>
        private void BindDocumentTypes()
        {
            dgvDocTypes.ReadOnly = true;
            if (docApprovalConfig.DocumentTypes != null && docApprovalConfig.DocumentTypes.Any())
            {
                var docTypeList = docApprovalConfig.DocumentTypes.Select(dt => (BizType)dt).Select(dt => new
                {
                    Value = dt,
                    Display = dt.ToString()
                }).ToList();
                bindingSourceDocTypes.DataSource = ListExtension.ToBindingSortCollection(docTypeList);
                dgvDocTypes.DataSource = bindingSourceDocTypes;
            }
            else
            {
                bindingSourceDocTypes.DataSource = new List<object>();
                dgvDocTypes.DataSource = bindingSourceDocTypes;
            }
        }

        /// <summary>
        /// 绑定目标状态下拉框
        /// </summary>
        private void BindTargetStatus()
        {
            var statuses = Enum.GetValues(typeof(DataStatus)).Cast<DataStatus>().ToList();
            cmbTargetStatus.DataSource = statuses;
            cmbTargetStatus.DisplayMember = "ToString()";
            // 使用ToBizType作为目标状态
            if (docApprovalConfig.ToBizType > 0)
            {
                cmbTargetStatus.SelectedItem = (DataStatus)docApprovalConfig.ToBizType;
            }
        }

        /// <summary>
        /// 绑定接收角色到DataGridView
        /// </summary>
        private void BindRecipients()
        {
            // 绑定接收角色
            if (docApprovalConfig.TargetRoles != null && docApprovalConfig.TargetRoles.Any())
            {
                var roleList = _cacheManager.GetEntityList<tb_RoleInfo>();
                if (roleList != null && roleList.Any())
                {
                    var selectedRoles = roleList.Where(r => docApprovalConfig.TargetRoles.Contains(r.RoleID)).ToList();
                    bindingSourceRoles.DataSource = ListExtension.ToBindingSortCollection(selectedRoles);
                    dgvRoles.DataSource = bindingSourceRoles;
                }
                else
                {
                    bindingSourceRoles.DataSource = new List<object>();
                    dgvRoles.DataSource = bindingSourceRoles;
                }
            }
            else
            {
                bindingSourceRoles.DataSource = new List<object>();
                dgvRoles.DataSource = bindingSourceRoles;
            }

            // 绑定审批人（如果有）
            if (docApprovalConfig.Approvers != null && docApprovalConfig.Approvers.Any())
            {
                var userList = _cacheManager.GetEntityList<tb_UserInfo>();
                if (userList != null && userList.Any())
                {
                    var selectedUsers = userList.Where(u => docApprovalConfig.Approvers.Contains(u.User_ID)).ToList();
                    bindingSourceUsers.DataSource = ListExtension.ToBindingSortCollection(selectedUsers);
                    dgvUsers.DataSource = bindingSourceUsers;
                }
                else
                {
                    bindingSourceUsers.DataSource = new List<object>();
                    dgvUsers.DataSource = bindingSourceUsers;
                }
            }
            else
            {
                bindingSourceUsers.DataSource = new List<object>();
                dgvUsers.DataSource = bindingSourceUsers;
            }
        }

        /// <summary>
        /// 绑定触发时机复选框
        /// </summary>
        private void BindTriggerTiming()
        {
            // 基于ActionType设置复选框
            chkTriggerOnSubmit.Checked = docApprovalConfig.ActionType == 1; // 提交
            chkTriggerOnApprove.Checked = docApprovalConfig.ActionType == 2; // 审核
            chkTriggerOnClose.Checked = docApprovalConfig.ActionType == 4; // 结案
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.EndEdit();
            if (bindingSourceEdit.Current is DocApprovalConfig config)
            {
                // 更新目标状态
                if (cmbTargetStatus.SelectedItem != null)
                {
                    config.ToBizType = (int)(DataStatus)cmbTargetStatus.SelectedItem;
                }

                // 更新触发时机（简化处理，实际可能需要更复杂的逻辑）
                if (chkTriggerOnSubmit.Checked)
                {
                    config.ActionType = 1; // 提交
                }
                else if (chkTriggerOnApprove.Checked)
                {
                    config.ActionType = 2; // 审核
                }
                else if (chkTriggerOnClose.Checked)
                {
                    config.ActionType = 4; // 结案
                }

                // 执行验证
                var result = config.Validate();
                if (!result.IsValid)
                {
                    MessageBox.Show($"错误:\r\n{result.GetCombinedErrors()}", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 选择单据类型按钮点击事件
        /// </summary>
        private void btnSelectDocTypes_Click(object sender, EventArgs e)
        {
            // 直接使用枚举类型
            var docTypes = Enum.GetValues(typeof(BizType)).Cast<BizType>()
                .Where(dt => dt != BizType.无对应数据 && dt != BizType.未知类型)
                .ToList();

            // 使用自定义选择对话框
            using (var form = new frmAdvanceSelector<BizType>())
            {
                form.AllowMultiSelect = true;
                form.ConfirmButtonText = "确认";
                form.InitializeSelector(docTypes, "选择单据类型");
                form.ConfigureColumn(x => x.ToString(), "单据类型");

                if (form.ShowDialog() == DialogResult.OK)
                {
                    var selected = form.SelectedItems;
                    docApprovalConfig.DocumentTypes = selected.Select(dt => (int)dt).ToList();
                    BindDocumentTypes();
                }
            }
        }

        /// <summary>
        /// 选择接收人员按钮点击事件
        /// </summary>
        private void btnSelectUsers_Click(object sender, EventArgs e)
        {
            var userInfoController = MainForm.Instance.AppContext.GetRequiredService<tb_UserInfoController<tb_UserInfo>>();
            var userInfos = userInfoController.Query();

            if (!userInfos.Any())
            {
                MessageBox.Show("没有可以选择的用户！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var selector = new frmAdvanceSelector<tb_UserInfo>())
            {
                selector.AllowMultiSelect = true;
                selector.ConfirmButtonText = "确认";
                selector.ConfigureColumn(x => x.UserName, "用户名");
                selector.InitializeSelector(userInfos, "选择审批人员");

                if (selector.ShowDialog() == DialogResult.OK)
                {
                    var selectedUsers = selector.SelectedItems.Take(5000).ToList();
                    docApprovalConfig.Approvers = selectedUsers.Select(u => u.User_ID).ToList();
                    bindingSourceUsers.DataSource = ListExtension.ToBindingSortCollection(selectedUsers);
                    dgvUsers.DataSource = bindingSourceUsers;
                }
            }
        }

        /// <summary>
        /// 选择接收角色按钮点击事件
        /// </summary>
        private void btnSelectRoles_Click(object sender, EventArgs e)
        {
            var roleList = _cacheManager.GetEntityList<tb_RoleInfo>();

            if (!roleList.Any())
            {
                MessageBox.Show("没有可以选择的角色！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var selector = new frmAdvanceSelector<tb_RoleInfo>())
            {
                selector.AllowMultiSelect = true;
                selector.ConfirmButtonText = "确认";
                selector.ConfigureColumn(x => x.RoleName, "角色名称");
                selector.InitializeSelector(roleList, "选择接收角色");

                if (selector.ShowDialog() == DialogResult.OK)
                {
                    var selectedRoles = selector.SelectedItems.Take(5000).ToList();
                    docApprovalConfig.TargetRoles = selectedRoles.Select(r => r.RoleID).ToList();
                    bindingSourceRoles.DataSource = ListExtension.ToBindingSortCollection(selectedRoles);
                    dgvRoles.DataSource = bindingSourceRoles;
                }
            }
        }

        /// <summary>
        /// 选择接收部门按钮点击事件
        /// </summary>
        private void btnSelectDepartments_Click(object sender, EventArgs e)
        {
            // 暂不支持部门选择，因为DocApprovalConfig没有部门字段
            MessageBox.Show("当前版本暂不支持按部门选择，请选择角色或人员。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void UCDocumentApprovalConfigEdit_Load(object sender, EventArgs e)
        {
            BindData(docApprovalConfig);
        }
    }
}
