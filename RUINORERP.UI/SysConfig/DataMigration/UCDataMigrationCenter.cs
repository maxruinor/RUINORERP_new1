using System;
using System.Windows.Forms;
using System.Data;
using System.Threading.Tasks;
using RUINORERP.Business.ImportEngine;
using RUINORERP.Business.ImportEngine.Models;
using System.Collections.Generic;
using RUINORERP.Model;
using System.Linq;
using SqlSugar;

namespace RUINORERP.UI.SysConfig.DataMigration
{
    /// <summary>
    /// 数据迁移中心用户控件
    /// 用于多表联动数据导入操作
    /// </summary>
    [RUINORERP.UI.Common.MenuAttrAssemblyInfo("数据迁移中心", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataMigrationCenter : UserControl
    {
        private readonly ISmartImportEngine _importEngine;
        private ISqlSugarClient _db;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCDataMigrationCenter()
        {
            InitializeComponent();
            _importEngine = new SmartImportEngine();
        }

        /// <summary>
        /// 控件加载事件
        /// </summary>
        private void UCDataMigrationCenter_Load(object sender, EventArgs e)
        {
            _db = MainForm.Instance?.AppContext?.Db;
            LoadProfiles();
        }

        /// <summary>
        /// 加载导入方案列表
        /// </summary>
        private void LoadProfiles()
        {
            try
            {
                var profiles = _importEngine.GetAvailableProfiles();
                clb方案.Items.Clear();
                foreach (var p in profiles)
                {
                    clb方案.Items.Add(p);
                }
                
                // 简单绘制依赖图（此处为文本模拟，实际可用 GraphControl）
                tree依赖.Nodes.Clear();
                foreach (var p in profiles)
                {
                    var node = tree依赖.Nodes.Add(p);
                    // 假设我们有一个方法能获取配置的依赖信息
                    node.Nodes.Add("(点击预览查看列映射)"); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载方案失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 预览按钮点击事件
        /// </summary>
        private async void btn预览_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt文件路径.Text))
            {
                MessageBox.Show("请先选择 Excel 文件。");
                return;
            }

            // 预览模式：取第一个选中的方案
            string profile = null;
            if (clb方案.CheckedItems.Count > 0)
            {
                profile = clb方案.CheckedItems[0].ToString();
            }

            if (string.IsNullOrEmpty(profile))
            {
                MessageBox.Show("请至少选择一个导入方案进行预览。");
                return;
            }

            try
            {
                btn预览.Enabled = false;
                btn预览.Text = "加载中...";
                
                var data = await _importEngine.PreviewDataAsync(txt文件路径.Text, profile);
                dataGridView预览.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预览失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn预览.Enabled = true;
                btn预览.Text = "数据预览";
            }
        }

        /// <summary>
        /// 选择文件按钮点击事件
        /// </summary>
        private void btn选择文件_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xlsx;*.xls|All Files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt文件路径.Text = ofd.FileName;
                }
            }
        }

        /// <summary>
        /// 执行导入按钮点击事件
        /// </summary>
        private async void btn执行导入_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt文件路径.Text))
            {
                MessageBox.Show("请先选择 Excel 文件。");
                return;
            }

            var selectedProfiles = new List<string>();
            foreach (var item in clb方案.CheckedItems)
            {
                selectedProfiles.Add(item.ToString());
            }

            if (!selectedProfiles.Any())
            {
                MessageBox.Show("请至少选择一个导入方案。");
                return;
            }

            try
            {
                btn执行导入.Enabled = false;
                btn执行导入.Text = "导入中...";

                // 调用编排器执行多表联动导入
                var db = MainForm.Instance.AppContext.Db;
                var orchestrator = new ImportOrchestrator(db);
                var report = await orchestrator.ExecuteComplexImportAsync(txt文件路径.Text, selectedProfiles);
                
                if (report.IsSuccess)
                {
                    MessageBox.Show(report.Message, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"导入失败: {report.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行出错: {ex.Message}");
            }
            finally
            {
                btn执行导入.Enabled = true;
                btn执行导入.Text = "执行导入";
            }
        }
    }
}
