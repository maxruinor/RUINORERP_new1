using Krypton.Toolkit;
using RUINORERP.Model;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RUINORERP.UI.IM
{
    public partial class frmIMSetting : KryptonForm
    {
        private MessageReminderConfig _currentConfig;

        public frmIMSetting()
        {
            InitializeComponent();
            InitializeConfig();
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void InitializeConfig()
        {
            try
            {
                // 使用配置服务获取消息提醒配置
                _currentConfig = MessageReminderConfigService.GetConfig();
                
                // 绑定数据到UI控件
                BindConfigToUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _currentConfig = new MessageReminderConfig();
            }
        }

        /// <summary>
        /// 绑定配置数据到UI控件
        /// </summary>
        private void BindConfigToUI()
        {
            // 语音提醒开关
            chkVoiceReminder.Checked = _currentConfig.VoiceReminderEnabled;
            
            // 音量调节滑块
            trackBarVolume.Value = _currentConfig.Volume;
            lblVolumeValue.Text = $"{_currentConfig.Volume}%";
            
            // 双击自动打开单据
            chkAutoOpenDocument.Checked = _currentConfig.AutoOpenDocumentOnDoubleClick;
            
            // 提醒频率
            cmbReminderFrequency.SelectedItem = _currentConfig.ReminderFrequency.ToString();
            
            // 免打扰时段
            chkQuietTime.Checked = _currentConfig.QuietTimeEnabled;
            dtpQuietStart.Value = DateTime.Today.Add(_currentConfig.QuietStartTime);
            dtpQuietEnd.Value = DateTime.Today.Add(_currentConfig.QuietEndTime);
            
            // 更新控件状态
            UpdateControlStates();
        }

        /// <summary>
        /// 从UI控件获取配置数据
        /// </summary>
        private void GetConfigFromUI()
        {
            _currentConfig.VoiceReminderEnabled = chkVoiceReminder.Checked;
            _currentConfig.Volume = trackBarVolume.Value;
            _currentConfig.AutoOpenDocumentOnDoubleClick = chkAutoOpenDocument.Checked;
            
            if (int.TryParse(cmbReminderFrequency.SelectedItem?.ToString(), out int frequency))
            {
                _currentConfig.ReminderFrequency = frequency;
            }
            
            _currentConfig.QuietTimeEnabled = chkQuietTime.Checked;
            _currentConfig.QuietStartTime = dtpQuietStart.Value.TimeOfDay;
            _currentConfig.QuietEndTime = dtpQuietEnd.Value.TimeOfDay;
        }

        /// <summary>
        /// 更新控件状态
        /// </summary>
        private void UpdateControlStates()
        {
            // 当语音提醒关闭时，禁用音量调节
            trackBarVolume.Enabled = chkVoiceReminder.Checked;
            lblVolumeValue.Enabled = chkVoiceReminder.Checked;
            
            // 当免打扰时段关闭时，禁用时间选择
            dtpQuietStart.Enabled = chkQuietTime.Checked;
            dtpQuietEnd.Enabled = chkQuietTime.Checked;
            lblQuietTimeTo.Enabled = chkQuietTime.Checked;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                GetConfigFromUI();
                
                // 验证配置
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (!ConfigurationService.ValidateConfig(_currentConfig, validationResults))
                {
                    string errorMessage = "配置数据无效，请检查以下问题：\n";
                    foreach (var result in validationResults)
                    {
                        errorMessage += $"- {result.ErrorMessage}\n";
                    }
                    MessageBox.Show(errorMessage, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 使用配置服务保存配置
                if (MessageReminderConfigService.SaveConfig(_currentConfig))
                {
                    MessageBox.Show("配置保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("保存配置失败，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 恢复默认设置
        /// </summary>
        private void RestoreDefaults()
        {
            var result = MessageBox.Show("确定要恢复默认设置吗？当前设置将被覆盖。", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                _currentConfig = new MessageReminderConfig();
                BindConfigToUI();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnRestoreDefaults_Click(object sender, EventArgs e)
        {
            RestoreDefaults();
        }

        private void chkVoiceReminder_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        private void chkQuietTime_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            lblVolumeValue.Text = $"{trackBarVolume.Value}%";
        }

        /// <summary>
        /// 窗体加载时设置默认值
        /// </summary>
        private void frmIMSetting_Load(object sender, EventArgs e)
        {
            // 确保组合框有默认选择
            if (cmbReminderFrequency.SelectedIndex == -1 && cmbReminderFrequency.Items.Count > 0)
            {
                cmbReminderFrequency.SelectedIndex = 0;
            }
        }
    }
}
