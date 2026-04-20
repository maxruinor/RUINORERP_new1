using Krypton.Toolkit;
using System;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 清理配置编辑对话框
    /// </summary>
    public partial class FrmCleanupConfigEdit : KryptonForm
    {
        /// <summary>
        /// 是否为新建模式
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 实体类型名称
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// 清理配置
        /// </summary>
        public CleanupConfiguration Configuration { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCleanupConfigEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmCleanupConfigEdit_Load(object sender, EventArgs e)
        {
            if (IsNew)
            {
                this.Text = "新建清理配置";
                Configuration = new CleanupConfiguration();
                Configuration.TargetEntityType = EntityTypeName;
                Configuration.TargetTable = EntityTypeName;
            }
            else
            {
                this.Text = "编辑清理配置";
                if (Configuration == null)
                {
                    MessageBox.Show("配置对象为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }
                LoadConfiguration();
            }
        }

        /// <summary>
        /// 加载配置数据到界面
        /// </summary>
        private void LoadConfiguration()
        {
            ktxtConfigName.Text = Configuration.ConfigName;
            ktxtDescription.Text = Configuration.Description;
            kchkEnableTransaction.Checked = Configuration.EnableTransaction;
            knumTransactionBatchSize.Value = Configuration.TransactionBatchSize;
            kchkEnableBackup.Checked = Configuration.EnableBackup;
            ktxtBackupSuffix.Text = Configuration.BackupTableSuffix;
            kchkEnableDetailedLog.Checked = Configuration.EnableDetailedLog;
            kchkAllowTestMode.Checked = Configuration.AllowTestMode;
            knumMaxProcessCount.Value = Configuration.MaxProcessCount;
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void KbtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(ktxtConfigName.Text))
                {
                    MessageBox.Show("配置名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ktxtConfigName.Focus();
                    return;
                }

                // 保存配置
                Configuration.ConfigName = ktxtConfigName.Text.Trim();
                Configuration.Description = ktxtDescription.Text;
                Configuration.EnableTransaction = kchkEnableTransaction.Checked;
                Configuration.TransactionBatchSize = (int)knumTransactionBatchSize.Value;
                Configuration.EnableBackup = kchkEnableBackup.Checked;
                Configuration.BackupTableSuffix = ktxtBackupSuffix.Text;
                Configuration.EnableDetailedLog = kchkEnableDetailedLog.Checked;
                Configuration.AllowTestMode = kchkAllowTestMode.Checked;
                Configuration.MaxProcessCount = (int)knumMaxProcessCount.Value;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void KbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
