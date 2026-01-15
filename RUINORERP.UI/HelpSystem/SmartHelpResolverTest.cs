// **********************************************************************************
// 智能帮助解析器测试演示
// 用于演示和测试智能帮助解析功能
//
// 使用说明：
// 1. 在测试窗体中添加各种命名规范的控件
// 2. 按F1键查看智能解析效果
// 3. 查看调试输出，了解解析过程
//
// 作者: 智能帮助系统
// 时间: 2026-01-15
// **********************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.UI.HelpSystem.Core;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 智能帮助解析器测试窗体
    /// 演示智能帮助匹配功能
    /// </summary>
    public partial class SmartHelpResolverTest : KryptonForm
    {
        private SmartHelpResolver _resolver;
        private ListBox _lstResults;
        private KryptonLabel _lblResolverInfo;

        public SmartHelpResolverTest()
        {
            InitializeComponent();
            InitializeResolver();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 窗体设置
            this.Text = "智能帮助解析器测试";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 创建主面板
            KryptonPanel mainPanel = new KryptonPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(10);

            // 创建结果列表
            _lstResults = new ListBox();
            _lstResults.Dock = DockStyle.Fill;
            _lstResults.Font = new Font("Consolas", 9F);
            _lstResults.IntegralHeight = false;

            // 创建信息标签
            _lblResolverInfo = new KryptonLabel();
            _lblResolverInfo.Text = "准备就绪";
            _lblResolverInfo.Dock = DockStyle.Bottom;
            _lblResolverInfo.LabelStyle = LabelStyle.BoldControl;

            // 创建测试控件面板
            KryptonPanel testPanel = CreateTestControlsPanel();
            testPanel.Height = 200;
            testPanel.Dock = DockStyle.Top;

            // 添加控件
            mainPanel.Controls.Add(_lstResults);
            mainPanel.Controls.Add(testPanel);
            mainPanel.Controls.Add(_lblResolverInfo);
            this.Controls.Add(mainPanel);

            this.ResumeLayout(false);
            this.PerformLayout();

            // 注册F1帮助
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;

            // 为所有控件注册帮助
            RegisterHelpForControls(this);
        }

        /// <summary>
        /// 创建测试控件面板
        /// </summary>
        private KryptonPanel CreateTestControlsPanel()
        {
            KryptonPanel panel = new KryptonPanel();
            panel.PanelBorderStyle = PaletteBorderStyle.HeaderPrimary;

            // 添加各种命名规范的控件
            int y = 10;

            // 示例1: 符合规范的控件
            y = AddTestControl(panel, "cmbCustomerVendor_ID", "ComboBox", y);
            y = AddTestControl(panel, "txtTotalCost", "TextBox", y);
            y = AddTestControl(panel, "dtpOrderDate", "DateTimePicker", y);
            y = AddTestControl(panel, "chkIsCustomizedOrder", "CheckBox", y);

            // 添加分隔线
            y += 10;
            KryptonSeparator separator = new KryptonSeparator();
            separator.Location = new Point(10, y);
            separator.Width = 760;
            panel.Controls.Add(separator);
            y += 30;

            // 示例2: 不符合规范的控件（应无法自动匹配）
            y = AddTestControl(panel, "CustomerVendor_ID", "TextBox (无前缀)", y);
            y = AddTestControl(panel, "cmbCustID", "ComboBox (过度缩写)", y);
            y = AddTestControl(panel, "txt123", "TextBox (无意义)", y);

            // 说明标签
            KryptonLabel lblHint = new KryptonLabel();
            lblHint.Text = "提示: 在上述控件上按F1键，查看智能解析效果";
            lblHint.Location = new Point(10, y);
            lblHint.ForeColor = Color.Gray;
            panel.Controls.Add(lblHint);

            return panel;
        }

        /// <summary>
        /// 添加测试控件
        /// </summary>
        private int AddTestControl(KryptonPanel panel, string controlName, string labelText, int y)
        {
            KryptonLabel lbl = new KryptonLabel();
            lbl.Text = labelText + ":";
            lbl.Location = new Point(10, y);
            lbl.Size = new Size(150, 20);

            Control control = CreateControlByName(controlName);
            control.Location = new Point(170, y);
            control.Size = new Size(150, 25);
            control.Name = controlName;

            panel.Controls.Add(lbl);
            panel.Controls.Add(control);

            return y + 35;
        }

        /// <summary>
        /// 根据控件名创建控件
        /// </summary>
        private Control CreateControlByName(string name)
        {
            if (name.StartsWith("cmb"))
            {
                return new KryptonComboBox();
            }
            else if (name.StartsWith("txt"))
            {
                return new KryptonTextBox();
            }
            else if (name.StartsWith("dtp"))
            {
                return new KryptonDateTimePicker();
            }
            else if (name.StartsWith("chk"))
            {
                return new KryptonCheckBox();
            }
            else
            {
                return new KryptonTextBox();
            }
        }

        /// <summary>
        /// 初始化解析器
        /// </summary>
        private void InitializeResolver()
        {
            _resolver = new SmartHelpResolver();
            UpdateResolverInfo();
        }

        /// <summary>
        /// 更新解析器信息显示
        /// </summary>
        private void UpdateResolverInfo()
        {
            string stats = _resolver.GetCacheStatistics();
            _lblResolverInfo.Text = stats.Replace("\n", " | ");
        }

        /// <summary>
        /// 为所有控件注册帮助
        /// </summary>
        private void RegisterHelpForControls(Control parent)
        {
            foreach (Control control in GetAllControls(parent))
            {
                if (control is KryptonTextBox || 
                    control is KryptonComboBox || 
                    control is KryptonCheckBox ||
                    control is KryptonDateTimePicker)
                {
                    HelpManager.Instance.EnableSmartTooltipForControl(control);
                }
            }
        }

        /// <summary>
        /// 获取所有控件
        /// </summary>
        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;

                if (control.Controls.Count > 0)
                {
                    foreach (Control child in GetAllControls(control))
                    {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// 窗体KeyDown事件
        /// </summary>
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                Control activeControl = this.ActiveControl;
                if (activeControl != null)
                {
                    TestResolve(activeControl);
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                // 清空缓存
                _resolver.ClearCache();
                UpdateResolverInfo();
                AddResult("缓存已清空");
                e.Handled = true;
            }
        }

        /// <summary>
        /// 测试解析控件
        /// </summary>
        private void TestResolve(Control control)
        {
            _lstResults.Items.Clear();
            AddResult($"========== 测试控件: {control.Name} ==========");
            AddResult($"控件类型: {control.GetType().Name}");

            try
            {
                // 1. 提取字段名
                string fieldName = _resolver.ExtractFieldNameFromControlName(control.Name);
                AddResult($"");
                AddResult($"1. 提取字段名: {fieldName ?? "(无法提取)"}");

                // 2. 解析实体类型
                Type entityType = _resolver.ResolveEntityType(this.GetType());
                AddResult($"2. 解析实体类型: {entityType?.Name ?? "(无法解析)"}");

                // 3. 验证字段存在
                bool fieldExists = false;
                if (entityType != null && !string.IsNullOrEmpty(fieldName))
                {
                    fieldExists = _resolver.EntityHasField(entityType, fieldName);
                    AddResult($"3. 验证字段存在: {(fieldExists ? "存在 ✓" : "不存在 ✗")}");
                }

                // 4. 解析帮助键
                List<string> helpKeys = _resolver.ResolveHelpKeys(control);
                AddResult($"");
                AddResult($"4. 解析出的帮助键 ({helpKeys.Count} 个):");
                for (int i = 0; i < helpKeys.Count; i++)
                {
                    AddResult($"   优先级 {i + 1}: {helpKeys[i]}");
                }

                // 5. 显示帮助
                AddResult($"");
                AddResult($"5. 显示帮助...");
                HelpManager.Instance.ShowControlHelp(control);

                // 更新解析器信息
                UpdateResolverInfo();
            }
            catch (Exception ex)
            {
                AddResult($"");
                AddResult($"错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加结果到列表
        /// </summary>
        private void AddResult(string text)
        {
            _lstResults.Items.Add($"[{DateTime.Now:HH:mm:ss.fff}] {text}");
            _lstResults.TopIndex = _lstResults.Items.Count - 1;
        }
    }

    /// <summary>
    /// 假设的测试实体类
    /// </summary>
    public class TestEntity : BaseEntity
    {
        public long CustomerVendor_ID { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCustomizedOrder { get; set; }
    }
}
