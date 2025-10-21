using RUINORERP.Plugin;
using System;
using System.Windows.Forms;

namespace RUINORERP.Plugin.AlibabaStoreManager
{
    /// <summary>
    /// 阿里巴巴店铺管理插件
    /// </summary>
    public class AlibabaStoreManagerPlugin : PluginBase
    {
        public override string Name => "阿里巴巴店铺管理";

        public override string Description => "用于操作和管理阿里巴巴1688平台店铺的插件，支持订单数据抓取、表单填写和提交等操作";

        public override Version Version => new Version(1, 0, 0);

        protected override void OnInitialize()
        {
            base.OnInitialize();
            // 初始化插件时的其他操作
        }

        protected override void OnStart()
        {
            base.OnStart();
            // 启动插件时的逻辑
        }

        protected override void OnStop()
        {
            base.OnStop();
            // 停止插件时的逻辑
        }

        public override void Execute()
        {
            // 显示主界面
            ShowMainForm();
        }

        private void ShowMainForm()
        {
            try
            {
                using (var mainForm = new MainForm())
                {
                    mainForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ShowError($"打开阿里巴巴店铺管理插件时发生错误: {ex.Message}");
            }
        }
    }
}