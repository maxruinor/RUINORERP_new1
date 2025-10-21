using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Plugin.AlibabaStoreManager.Models;

namespace RUINORERP.Plugin.AlibabaStoreManager
{
    public partial class LoginForm : Form
    {
        private PluginConfig pluginConfig;
        private Action<PluginConfig> onSaveConfig;

        public LoginForm(PluginConfig config, Action<PluginConfig> onSaveConfig)
        {
            InitializeComponent();
            this.pluginConfig = config ?? new PluginConfig();
            this.onSaveConfig = onSaveConfig;
            LoadConfig();
        }

        private void LoadConfig()
        {
            txtUsername.Text = pluginConfig.Username ?? "";
            txtPassword.Text = pluginConfig.Password ?? "";
            chkAutoLogin.Checked = pluginConfig.AutoLogin;
        }

        private void SaveConfig()
        {
            pluginConfig.Username = txtUsername.Text.Trim();
            pluginConfig.Password = txtPassword.Text.Trim(); // 在实际应用中应该加密存储
            pluginConfig.AutoLogin = chkAutoLogin.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("请输入用户名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
                return;
            }

            SaveConfig();
            onSaveConfig?.Invoke(pluginConfig);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}