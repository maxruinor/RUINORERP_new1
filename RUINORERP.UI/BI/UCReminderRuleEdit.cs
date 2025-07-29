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

        public override void BindData(BaseEntity _entity)
        {
            entity = _entity as tb_ReminderRule;
            if (entity.RuleId == 0)
            {
                entity.EffectiveDate = System.DateTime.Now;
                entity.ExpireDate = System.DateTime.Now.AddDays(60);
            }
            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.RuleName, txtRuleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_ReminderRule, RuleEngineType>(entity, k => k.RuleEngineType, cmbRuleEngineType, false);

            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_ReminderRule, ReminderBizType>(entity, k => k.ReminderBizType, cmbReminderBizType, false);
            DataBindingHelper.BindData4CmbByEnum<tb_ReminderRule, ReminderPriority>(entity, k => k.ReminderPriority, cmbPriority, false);
            DataBindingHelper.BindData4CheckBox<tb_ReminderRule>(entity, t => t.IsEnabled, chkIsEnabled, false);
            DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.EffectiveDate, dtpEffectiveDate, false);


            // 创建数据源
            BindingList<string> itemsSource = new BindingList<string>() { "Item 1", "Item 2", "Item 3" };
            // 设置CheckedListBox的数据源
            chkNotifyChannels.DataSource = null; // 清除原有绑定
            chkNotifyChannels.DataSource = itemsSource;
            chkNotifyChannels.ValueMember = "Value";
            // 创建表达式绑定
            Binding binding = new Binding("SelectedValue", new BindingSource(itemsSource, null), "Value", true);
            binding.Format += (sender, e) =>
            {
                // 在获取值之前进行格式化（例如转换为大写）
                e.Value = ((string)e.Value).ToUpper();
            };
            binding.Parse += (sender, e) =>
            {
                // 在设置值之前进行解析（例如将输入转换为小写）
                e.Value = ((string)e.Value).ToLower();
            };
            chkNotifyChannels.DataBindings.Add(binding);



            //CheckedListBoxHelper.BindData4CheckedListByEnum<tb_ReminderRule, NotifyChannel>(entity, c => c.NotifyChannels, chkNotifyChannels, false);
            CheckedListBoxHelper.BindData4CheckedListBox<tb_ReminderRule, NotifyChannel>(entity, t => t.NotifyChannels, chkNotifyChannels, NotifyChannel.Workflow, NotifyChannel.SMS);

            DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.ExpireDate, dtpExpireDate, false);
            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Condition, txtCondition, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.NotifyRecipients, txtNotifyRecipients, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.NotifyRecipientNames, txtNotifyRecipientNames, BindDataType4TextBox.Text, false);
            txtNotifyRecipientNames.ReadOnly = true;

            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.NotifyMessage, txtNotifyMessage, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.JsonConfig, txtJsonConfig, BindDataType4TextBox.Text, false);
            txtJsonConfig.ReadOnly = true;//要配置不能手输入。不然格式错误


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
                    // 使用示例
                    JObject obj = JsonHelper.SafeParseJson(entity.JsonConfig);

                    SafetyStockConfig safetyStockConfig = obj.ToObject<SafetyStockConfig>();
                    if (safetyStockConfig == null)
                    {
                        safetyStockConfig = new SafetyStockConfig();
                    }
                    ucSafetyStockConfigEdit.safetyStockConfig = safetyStockConfig;
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
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnConfigParser_Click(object sender, EventArgs e)
        {
            var ConfigObject = LoadBusinessConfig(entity.ReminderBizType);
            if (ConfigObject != null)
            {
                //发送缓存数据
                string json = JsonConvert.SerializeObject(ConfigObject,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                   });

                txtJsonConfig.Text = json;
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

                    string SelectedIds = string.Join(", ", SelectUsers.Take(5000).Select(item => item.User_ID));
                    SelectedIds = SelectedIds.TrimEnd(',');

                    if (SelectUsers.Count > 0)
                    {
                        entity.NotifyRecipientNames = SelectedNames;
                        entity.NotifyRecipients = SelectedIds;
                    }
                }
            }
        }
    }
}
