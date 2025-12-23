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


        private void btnOk_Click(object sender, EventArgs e)
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
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
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



        private async void UCReminderRuleEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
