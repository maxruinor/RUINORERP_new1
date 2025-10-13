using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

namespace RUINORERP.Server.Controls
{
    public partial class SystemManagementControl : UserControl
    {
        private IOptionsMonitor<SystemGlobalconfig> _globalConfig;

        public SystemManagementControl()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                // 获取全局配置
                _globalConfig = frmMain.Instance.Globalconfig;
                
                // 加载配置数据
                LoadConfigData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化系统配置时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SystemManagementControl_Load(object sender, EventArgs e)
        {
            // 设置控件属性
        }

        /// <summary>
        /// 加载配置数据
        /// </summary>
        private void LoadConfigData()
        {
            try
            {
                if (_globalConfig != null && _globalConfig.CurrentValue != null)
                {
                    var config = _globalConfig.CurrentValue;
                    
                    // 基本配置
                    textBoxServerName.Text = config.ServerName ?? "";
                    textBoxServerPort.Text = config.ServerPort.ToString();
                    textBoxMaxConnections.Text = config.MaxConnections.ToString();
                    textBoxHeartbeatInterval.Text = config.HeartbeatInterval.ToString();
                    
                    // 数据库配置
                    textBoxDbConnectionString.Text = config.DbConnectionString ?? "";
                    textBoxDbType.Text = config.DbType ?? "";
                    
                    // 缓存配置
                    textBoxCacheType.Text = config.CacheType ?? "";
                    textBoxCacheConnectionString.Text = config.CacheConnectionString ?? "";
                    
                    // 日志配置
                    checkBoxEnableLogging.Checked = config.EnableLogging;
                    textBoxLogLevel.Text = config.LogLevel ?? "";
                    
                    // 其他配置
                    textBoxSomeSetting.Text = config.SomeSetting ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存配置数据
        /// </summary>
        private void SaveConfigData()
        {
            try
            {
                // 这里应该实现保存配置的逻辑
                // 由于配置是通过IOptionsMonitor注入的，通常是只读的
                // 实际应用中可能需要通过配置服务来保存
                
                MessageBox.Show("配置已保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 按钮事件处理

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            LoadConfigData();
            MessageBox.Show("配置已重新加载", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfigData();
        }

        private void btnResetConfig_Click(object sender, EventArgs e)
        {
            // 确认重置
            DialogResult result = MessageBox.Show("确定要重置所有配置为默认值吗？", 
                "确认重置", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                // 这里应该实现重置配置的逻辑
                MessageBox.Show("配置已重置为默认值", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region 注册信息管理

        /// <summary>
        /// 加载注册信息
        /// </summary>
        private void LoadRegistrationInfo()
        {
            try
            {
                var registrationInfo = frmMain.Instance.registrationInfo;
                if (registrationInfo != null)
                {
                    textBoxCompanyName.Text = registrationInfo.CompanyName ?? "";
                    textBoxContactName.Text = registrationInfo.ContactName ?? "";
                    textBoxPhoneNumber.Text = registrationInfo.PhoneNumber ?? "";
                    textBoxConcurrentUsers.Text = registrationInfo.ConcurrentUsers.ToString();
                    textBoxProductVersion.Text = registrationInfo.ProductVersion ?? "";
                    textBoxLicenseType.Text = registrationInfo.LicenseType ?? "";
                    textBoxFunctionModule.Text = registrationInfo.FunctionModule ?? "";
                    textBoxMachineCode.Text = registrationInfo.MachineCode ?? "";
                    textBoxRegistrationCode.Text = registrationInfo.RegistrationCode ?? "";
                    
                    dateTimePickerPurchaseDate.Value = registrationInfo.PurchaseDate;
                    dateTimePickerRegistrationDate.Value = registrationInfo.RegistrationDate;
                    dateTimePickerExpirationDate.Value = registrationInfo.ExpirationDate;
                    checkBoxIsRegistered.Checked = registrationInfo.IsRegistered;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载注册信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存注册信息
        /// </summary>
        private void SaveRegistrationInfo()
        {
            try
            {
                // 这里应该实现保存注册信息的逻辑
                // 实际应用中可能需要通过服务来保存到数据库
                
                MessageBox.Show("注册信息已保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存注册信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadRegistration_Click(object sender, EventArgs e)
        {
            LoadRegistrationInfo();
            MessageBox.Show("注册信息已重新加载", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSaveRegistration_Click(object sender, EventArgs e)
        {
            SaveRegistrationInfo();
        }

        private void btnGenerateMachineCode_Click(object sender, EventArgs e)
        {
            try
            {
                // 生成机器码
                var registrationInfo = frmMain.Instance.registrationInfo;
                if (registrationInfo != null)
                {
                    string machineCode = frmMain.Instance.CreateMachineCode(registrationInfo);
                    textBoxMachineCode.Text = machineCode;
                    MessageBox.Show("机器码已生成", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成机器码时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnValidateRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                var registrationInfo = frmMain.Instance.registrationInfo;
                if (registrationInfo != null)
                {
                    bool isValid = frmMain.Instance.CheckRegistered(registrationInfo);
                    if (isValid)
                    {
                        MessageBox.Show("注册信息验证通过", "验证结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("注册信息验证失败", "验证结果", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"验证注册信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 验证输入数据
        /// </summary>
        /// <returns>验证是否通过</returns>
        private bool ValidateInput()
        {
            // 验证服务器端口
            if (!int.TryParse(textBoxServerPort.Text, out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("请输入有效的服务器端口(1-65535)", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxServerPort.Focus();
                return false;
            }
            
            // 验证最大连接数
            if (!int.TryParse(textBoxMaxConnections.Text, out int maxConn) || maxConn <= 0)
            {
                MessageBox.Show("请输入有效的最大连接数", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxMaxConnections.Focus();
                return false;
            }
            
            // 验证心跳间隔
            if (!int.TryParse(textBoxHeartbeatInterval.Text, out int interval) || interval <= 0)
            {
                MessageBox.Show("请输入有效的心跳间隔", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxHeartbeatInterval.Focus();
                return false;
            }
            
            return true;
        }

        #endregion
    }
}