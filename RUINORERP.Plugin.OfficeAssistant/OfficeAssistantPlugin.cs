using RUINORERP.Plugin;
using System;
using System.Windows.Forms;

namespace RUINORERP.Plugin.OfficeAssistant
{
    public class OfficeAssistantPlugin : PluginBase
    {
        public override string Name => "办公助手";
        
        public override string Description => "统一的办公增强插件，包含Excel对比、数据验证等模块";
        
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
                ShowError($"打开办公助手时发生错误: {ex.Message}");
            }
        }
    }
}