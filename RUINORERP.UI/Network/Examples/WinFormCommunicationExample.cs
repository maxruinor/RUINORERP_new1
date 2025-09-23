using System;
using System.Windows.Forms;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// WinForm通信集成使用示例类
    /// 展示如何在实际的WinForm应用程序中使用WinFormCommunicationIntegration类
    /// </summary>
    public partial class CommunicationExampleForm : Form
    {
        // 通信集成管理器
        private WinFormCommunicationIntegration _communicationIntegration;
        // 通信管理器
        private CommunicationManager _communicationManager;
        // 是否已初始化通信模块
        private bool _isCommunicationInitialized = false;

        /// <summary>
        /// 初始化CommunicationExampleForm类的新实例
        /// </summary>
        public CommunicationExampleForm()
        {
            InitializeComponent();
            InitializeCommunication();
        }

        /// <summary>
        /// 初始化通信模块
        /// </summary>
        private void InitializeCommunication()
        {
            try
            {
                // 创建通信管理器实例
                // 注意：实际项目中可能需要通过依赖注入获取CommunicationManager实例
                _communicationManager = CreateCommunicationManager();

                // 创建通信集成实例
                _communicationIntegration = new WinFormCommunicationIntegration(_communicationManager, this);

                // 初始化通信模块
                _communicationIntegration.Initialize();
                _isCommunicationInitialized = true;

                // 更新UI状态
                UpdateUIState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "初始化通信模块失败: " + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 创建通信管理器实例
        /// 注意：在实际项目中，这可能通过依赖注入容器完成
        /// </summary>
        /// <returns>CommunicationManager实例</returns>
        private CommunicationManager CreateCommunicationManager()
        {
            // 这里简化了创建过程，实际项目中可能需要更复杂的初始化逻辑
            // 例如：通过IoC容器解析依赖、配置连接参数等
            // 以下为示例代码，实际实现请根据项目的依赖注入方式调整
            
            // 示例：假设通过某种方式获取或创建了所需的服务实例
            var communicationService = CreateCommunicationService();
            var heartbeatManager = CreateHeartbeatManager();

            // 创建并返回CommunicationManager实例
            return new CommunicationManager(communicationService, heartbeatManager);
        }

        /// <summary>
        /// 创建通信服务实例
        /// 注意：在实际项目中，这可能通过依赖注入容器完成
        /// </summary>
        /// <returns>IClientCommunicationService实例</returns>
        private IClientCommunicationService CreateCommunicationService()
        {
            // 这里简化了创建过程，实际项目中可能需要更复杂的初始化逻辑
            // 示例：通过IoC容器解析或直接创建实例
            var socketClient = new SuperSocketClient();
            var commandDispatcher = new ClientCommandDispatcher();
            return new ClientCommunicationService(socketClient, commandDispatcher);
        }

        /// <summary>
        /// 创建心跳管理器实例
        /// 注意：在实际项目中，这可能通过依赖注入容器完成
        /// </summary>
        /// <returns>HeartbeatManager实例</returns>
        private HeartbeatManager CreateHeartbeatManager()
        {
            // 这里简化了创建过程，实际项目中可能需要更复杂的初始化逻辑
            // 示例：通过IoC容器解析或直接创建实例
            var communicationService = CreateCommunicationService();
            return new HeartbeatManager(communicationService);
        }

        /// <summary>
        /// 连接按钮点击事件处理
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!_isCommunicationInitialized)
            {
                MessageBox.Show(this, "通信模块尚未初始化，请先初始化通信模块", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 获取服务器地址和端口
                string serverUrl = txtServerUrl.Text.Trim();
                int port = int.Parse(txtPort.Text.Trim());

                // 连接到服务器
                _communicationIntegration.Connect(serverUrl, port);
            }
            catch (FormatException)
            {
                MessageBox.Show(this, "请输入有效的端口号", "输入错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "连接服务器时出错: " + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 断开连接按钮点击事件处理
        /// </summary>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (!_isCommunicationInitialized)
            {
                MessageBox.Show(this, "通信模块尚未初始化", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _communicationIntegration.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "断开连接时出错: " + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 发送命令按钮点击事件处理
        /// </summary>
        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            if (!_isCommunicationInitialized)
            {
                MessageBox.Show(this, "通信模块尚未初始化", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 获取命令ID和数据
                // 注意：这里需要根据实际的命令ID类型进行调整
                // CommandId commandId = (CommandId)cmbCommandId.SelectedItem;
                string data = txtCommandData.Text.Trim();

                // 发送命令
                // _communicationIntegration.SendCommand(commandId, data);
                MessageBox.Show(this, "发送命令功能需要根据实际需求实现", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "发送命令时出错: " + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新UI状态
        /// </summary>
        private void UpdateUIState()
        {
            // 根据通信模块是否初始化更新UI状态
            bool isEnabled = _isCommunicationInitialized;
            btnConnect.Enabled = isEnabled;
            btnDisconnect.Enabled = isEnabled;
            btnSendCommand.Enabled = isEnabled;
            txtServerUrl.Enabled = isEnabled;
            txtPort.Enabled = isEnabled;
            // cmbCommandId.Enabled = isEnabled;
            txtCommandData.Enabled = isEnabled;

            // 如果通信模块已初始化，填充命令ID下拉列表
            // if (isEnabled && cmbCommandId.Items.Count == 0)
            // {
            //     // 填充命令ID列表
            //     foreach (CommandId commandId in Enum.GetValues(typeof(CommandId)))
            //     {
            //         cmbCommandId.Items.Add(commandId);
            //     }

            //     // 默认选择第一个命令
            //     if (cmbCommandId.Items.Count > 0)
            //     {
            //         cmbCommandId.SelectedIndex = 0;
            //     }
            // }
        }

        /// <summary>
        /// 窗口关闭事件处理
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // 释放通信资源
            if (_communicationIntegration != null)
            {
                _communicationIntegration.Dispose();
                _communicationIntegration = null;
            }
        }
    }
}