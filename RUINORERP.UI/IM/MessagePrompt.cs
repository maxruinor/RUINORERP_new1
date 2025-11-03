using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.UserCenter;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RUINORERP.UI.IM
{
    public partial class MessagePrompt : BaseMessagePrompt
    {
        private FlowLayoutPanel messageFlowLayoutPanel;
        private Timer messageTimer;

        /// <summary>
        /// 初始化组件
        /// </summary>
        protected override void InitializeComponents()
        {
            InitializeComponent();
            InitializeForm();
        }

        /// <summary>
        /// 设置发送者文本
        /// </summary>
        /// <param name="text">发送者名称</param>
        public override void SetSenderText(string text)
        {
            if (txtSender != null)
            {
                txtSender.Text = text;
            }
        }

        /// <summary>
        /// 设置主题文本
        /// </summary>
        /// <param name="text">消息主题</param>
        public override void SetSubjectText(string text)
        {
            if (txtSubject != null)
            {
                txtSubject.Text = text;
            }
        }

        public MessagePrompt() : base()
        {
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="messageService">消息服务</param>
        public MessagePrompt(MessageData messageData, EnhancedMessageManager messageManager = null, ILogger<MessagePrompt> logger = null)
            : base(messageData, logger, messageManager)
        {
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            // 可以在这里添加额外的初始化代码
            // 设置窗体的初始位置为屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );

            // 初始化消息流布局面板
            messageFlowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            this.Controls.Add(messageFlowLayoutPanel);

            // 初始化消息计时器
            messageTimer = new Timer
            {
                Interval = 5000 // 消息显示的时间间隔，单位毫秒
            };
            messageTimer.Tick += MessageTimer_Tick;
        }
        /// <summary>
        /// 更新消息显示
        /// 根据消息数据更新UI组件
        /// </summary>
        protected override void UpdateMessageDisplay()
        {
            try
            {

                if (MessageData == null) return;

                // 设置基本信息
                if (txtSender != null) txtSender.Text = MessageData.Sender ?? "系统";
                if (txtSubject != null) txtSubject.Text = MessageData.Title ?? "消息";
                if (txtContent != null) txtContent.Text = MessageData.Content;

                // 根据消息类型设置不同的显示样式
                //switch (MessageData.MessageType)
                //{                    
                //    case MessageType.Prompt:
                //        this.Icon = Properties.Resources.info;
                //        break;
                //    case MessageType.BusinessData:
                //        this.Icon = Properties.Resources.Business;
                //        break;
                //    case MessageType.System:
                //        this.Icon = Properties.Resources.System;
                //        break;
                //    default:
                //        this.Icon = Properties.Resources.Message;
                //        break;
                //}

                // 设置确认相关控件的可见性
                if (MessageData.NeedConfirmation)
                {
                    // 显示确认按钮等控件
                    //if (btnConfirm != null) btnConfirm.Visible = true;
                    //if (btnReject != null) btnReject.Visible = true;
                }

                // 显示业务相关信息
                if (MessageData.BizId >= 0)
                {
                    // 可以在这里显示业务数据相关信息
                    Logger.LogDebug($"显示业务消息: 类型={MessageData.BizType}, ID={MessageData.BizId}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "更新消息显示时发生错误");
            }
        }


        public string Content { get; set; } = string.Empty;

        private void MessageTimer_Tick(object sender, EventArgs e)
        {
            // 模拟消息到达，显示新消息
            ShowMessage(Content);
        }

        private void ShowMessage(string message)
        {
            // 创建一个新的消息标签
            Label messageLabel = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = SystemColors.ControlText,
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle
            };

            // 将消息标签添加到流布局面板
            messageFlowLayoutPanel.Controls.Add(messageLabel);

            // 调整窗体大小以适应消息
            this.Width = messageFlowLayoutPanel.Width + messageFlowLayoutPanel.Padding.Horizontal;
            this.Height = messageFlowLayoutPanel.Height + messageFlowLayoutPanel.Padding.Vertical;

            // 计时器启动，用于在一定时间后隐藏消息
            messageTimer.Start();
        }

        QueryParameter parameter { get; set; }


        private void MessagePrompt_Load(object sender, EventArgs e)
        {




            txtContent.Text = Content;
            lblSendTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // 确保窗体在显示时位于屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );

            AddCommandForWait();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 将消息标记为已读状态（取消操作后仍视为已读）
            ResponseToServer(true);
            // this.DialogResult = DialogResult.Cancel;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            //计划提醒，则把要提醒的计划查出条件找到
            Type tableType = EntityMappingHelper.GetEntityType(MessageData.BizType);
            //找到要提醒的数据
            var conModel = new List<IConditionalModel>();
            // conModel.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "3", CSharpTypeName = "int" });

            string FieldName = BaseUIHelper.GetEntityPrimaryKey(tableType);

            conModel.Add(new ConditionalModel { FieldName = FieldName, ConditionalType = ConditionalType.Equal, FieldValue = MessageData.BizId.ToString(), CSharpTypeName = "long" });
            //如果有限制条件
            //if (AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext))
            //{
            //    conModel.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            //}

            parameter = new QueryParameter();
            parameter.conditionals = conModel;
            parameter.tableType = tableType;
            // 创建实例
            object instance = Activator.CreateInstance(parameter.tableType);
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(parameter.tableType.Name + "Processor");
            QueryFilter queryFilter = baseProcessor.GetQueryFilter();


            //这里知道ID 在这里虚拟一下主键的查询条件。将ID给过去。一次性查询。或 时间也给过去。
            //应该是给计划特殊处理。指令系统用上？
            QueryField queryField = new QueryField();
            queryField.QueryTargetType = tableType;
            queryField.FieldName = FieldName;
            queryField.FieldPropertyInfo = tableType.GetProperties().FirstOrDefault(c => c.Name == FieldName);
            if (!queryFilter.QueryFields.Contains(queryField))
            {
                queryFilter.QueryFields.Add(queryField);
            }
            parameter.queryFilter = queryFilter;
            var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == parameter.tableType.Name && m.ClassPath.Contains("")).FirstOrDefault();
            if (RelatedBillMenuInfo != null)
            {
                MenuPowerHelper menuPowerHelper;
                menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

                menuPowerHelper.OnSetQueryConditionsDelegate += MenuPowerHelper_OnSetQueryConditionsDelegate;
                await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, instance, parameter);
                //要卸载，不然会多次执行
                menuPowerHelper.OnSetQueryConditionsDelegate -= MenuPowerHelper_OnSetQueryConditionsDelegate;
                // 将消息标记为已处理状态（处理后视为已读）
                ResponseToServer(true);
                this.DialogResult = DialogResult.OK;
                this.Close();

            }

        }

        private void MenuPowerHelper_OnSetQueryConditionsDelegate(object QueryDto, UserCenter.QueryParameter nodeParameter)
        {
            if (QueryDto == null)
            {
                return;
            }
            //查询条件给值前先将条件清空
            foreach (var item in nodeParameter.queryFilter.QueryFields)
            {
                if (item.FKTableName.IsNotEmptyOrNull() && item.IsRelated)
                {
                    QueryDto.SetPropertyValue(item.FieldName, -1L);
                    continue;
                }
                if (item.FieldPropertyInfo.PropertyType.IsGenericType && item.FieldPropertyInfo.PropertyType.GetBaseType().Name == "DateTime")
                {
                    QueryDto.SetPropertyValue(item.FieldName, null);
                    if (QueryDto.ContainsProperty(item.FieldName + "_Start"))
                    {
                        QueryDto.SetPropertyValue(item.FieldName + "_Start", null);
                    }
                    if (QueryDto.ContainsProperty(item.FieldName + "_End"))
                    {
                        QueryDto.SetPropertyValue(item.FieldName + "_End", null);
                    }
                    continue;
                }

            }



            //传入查询对象的实例，
            foreach (ConditionalModel item in nodeParameter.conditionals)
            {
                if (item.ConditionalType == ConditionalType.Equal)
                {
                    switch (item.CSharpTypeName)
                    {
                        case "int":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToInt());
                            break;
                        case "long":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToLong());
                            break;
                        case "bool":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToBool());
                            break;
                        default:
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue);
                            break;
                    }
                }
            }



        }

        private void btnWaitReminder_Click(object sender, EventArgs e)
        {
            WaitReminder(sender);
        }


        /// <summary>
        /// 响应服务器，更新消息状态
        /// </summary>
        /// <param name="isRead">消息是否已读</param>
        /// <param name="interval">提醒间隔</param>
        private void ResponseToServer(bool isRead, int interval = 20)
        {
            if (MessageData == null)
                return;

            try
            {
                var requestData = new Dictionary<string, object>
                {
                    { "BizId", MessageData.BizId },
                    { "IsRead", isRead },
                    { "RemindInterval", interval }
                };

                var messageRequest = new MessageRequest(MessageType.Message, requestData);
                //TODO fix
                //  MessageService.SendMessageToUserAsync(messageRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger?.Error(ex, "向服务器发送消息状态更新时发生错误");
            }
        }

        #region 生成稍候提醒的指令

        private void AddCommandForWait()
        {

            KryptonContextMenuItems contextMenuItems = new KryptonContextMenuItems();
            KryptonContextMenuItem menuItem5分钟后 = new KryptonContextMenuItem();
            KryptonCommand command5分钟后 = new KryptonCommand();
            command5分钟后.Execute += kryptonCommandWait_Execute;
            menuItem5分钟后.KryptonCommand = command5分钟后;
            menuItem5分钟后.Text = "五分钟后";
            command5分钟后.Text = menuItem5分钟后.Text;


            KryptonContextMenuItem menuItem10分钟后 = new KryptonContextMenuItem();
            KryptonCommand command十分钟后 = new KryptonCommand();
            command十分钟后.Execute += kryptonCommandWait_Execute;
            menuItem10分钟后.KryptonCommand = command十分钟后;
            menuItem10分钟后.Text = "十分钟后";
            command十分钟后.Text = menuItem10分钟后.Text;

            KryptonContextMenuItem menuItem一小时后 = new KryptonContextMenuItem();
            KryptonCommand command一小时后 = new KryptonCommand();
            command一小时后.Execute += kryptonCommandWait_Execute;
            menuItem一小时后.KryptonCommand = command一小时后;
            menuItem一小时后.Text = "一小时后";
            command一小时后.Text = menuItem一小时后.Text;

            KryptonContextMenuItem menuItem一天后 = new KryptonContextMenuItem();
            KryptonCommand command一天后 = new KryptonCommand();
            command一天后.Execute += kryptonCommandWait_Execute;
            menuItem一天后.KryptonCommand = command一天后;
            menuItem一天后.Text = "一天后";
            command一天后.Text = menuItem一天后.Text;

            this.kryptonContextMenu1.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            contextMenuItems});

            contextMenuItems.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            menuItem5分钟后,
            menuItem10分钟后,
            menuItem一小时后,
            menuItem一天后
            });
            if (this.kryptonContextMenu1.Items.Count == 0)
            {
                this.kryptonContextMenu1.Items.Add(contextMenuItems);
            }

        }

        #endregion

        private void kryptonCommandWait_Execute(object sender, EventArgs e)
        {
            WaitReminder(sender);
        }

        private void WaitReminder(object sender)
        {
            int interval = 60;
            if (sender is KryptonDropButton dropButton)
            {
                //默认5分钟
                interval = 60 * 5;
            }
            else if (sender is KryptonCommand command)
            {
                switch (command.Text)
                {
                    case "五分钟后":
                        interval = 300;
                        break;
                    case "十分钟后":
                        interval = 600;
                        break;
                    case "一小时后":
                        interval = 3600;
                        break;
                    case "一天后":
                        interval = 3600 * 24;
                        break;
                    default:
                        break;
                }
            }
            // 将消息标记为等待提醒状态，使用IsRead=false表示未读
            ResponseToServer(false, interval);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 添加导航到业务文档的方法
        private void NavigateToBusinessDocument()
        {
            try
            {
                if (MessageData == null)
                    return;

                var bizManager = Startup.GetFromFac<EnhancedMessageManager>();
                bizManager?.NavigateToBusinessDocument(MessageData.BizType, MessageData.BizId);
            }
            catch (Exception ex)
            {
                Logger?.Error(ex, "导航到业务文档时发生错误");
                throw;
            }
        }

    }
}
