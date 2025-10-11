using System;
using System.Windows.Forms;

namespace RUINORERP.Plugin.OfficeAssistant
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }
        
        private void InitializeCustomComponents()
        {
            this.Text = "办公助手 - Excel对比工具";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 创建主布局面板
            CreateMainLayout();
        }
        
        private void CreateMainLayout()
        {
            // 文件选择区域
            var fileSelectionPanel = CreateFileSelectionPanel();
            fileSelectionPanel.Dock = DockStyle.Top;
            fileSelectionPanel.Height = 100;
            
            // 列映射配置区域
            var columnMappingPanel = CreateColumnMappingPanel();
            columnMappingPanel.Dock = DockStyle.Top;
            columnMappingPanel.Height = 200;
            
            // 对比设置区域
            var comparisonSettingsPanel = CreateComparisonSettingsPanel();
            comparisonSettingsPanel.Dock = DockStyle.Top;
            comparisonSettingsPanel.Height = 100;
            
            // 结果展示区域
            var resultDisplayPanel = CreateResultDisplayPanel();
            resultDisplayPanel.Dock = DockStyle.Fill;
            
            // 操作按钮区域
            var actionPanel = CreateActionPanel();
            actionPanel.Dock = DockStyle.Bottom;
            actionPanel.Height = 50;
            
            // 添加到主窗体
            this.Controls.Add(resultDisplayPanel);
            this.Controls.Add(columnMappingPanel);
            this.Controls.Add(comparisonSettingsPanel);
            this.Controls.Add(fileSelectionPanel);
            this.Controls.Add(actionPanel);
        }
        
        private Panel CreateFileSelectionPanel()
        {
            var panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            
            var label = new Label();
            label.Text = "文件选择";
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            label.Dock = DockStyle.Top;
            
            panel.Controls.Add(label);
            return panel;
        }
        
        private Panel CreateColumnMappingPanel()
        {
            var panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            
            var label = new Label();
            label.Text = "列映射配置";
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            label.Dock = DockStyle.Top;
            
            panel.Controls.Add(label);
            return panel;
        }
        
        private Panel CreateComparisonSettingsPanel()
        {
            var panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            
            var label = new Label();
            label.Text = "对比设置";
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            label.Dock = DockStyle.Top;
            
            panel.Controls.Add(label);
            return panel;
        }
        
        private Panel CreateResultDisplayPanel()
        {
            var panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            
            var label = new Label();
            label.Text = "结果展示";
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            label.Dock = DockStyle.Top;
            
            panel.Controls.Add(label);
            return panel;
        }
        
        private Panel CreateActionPanel()
        {
            var panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            
            var label = new Label();
            label.Text = "操作区域";
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            label.Dock = DockStyle.Top;
            
            panel.Controls.Add(label);
            return panel;
        }
    }
}