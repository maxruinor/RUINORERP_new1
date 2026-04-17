using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.Common;
using SqlSugar;
using RUINORERP.Model;

namespace RUINORERP.UI.DevTools
{
    [MenuAttrAssemblyInfo("数据清理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataCleanupTool : UserControl
    {
        public UCDataCleanupTool()
        {
            InitializeComponent();
        }

        private void frmDataCleanupTool_Load(object sender, EventArgs e)
        {
            LoadTableList();
            cmb清理模式.SelectedIndex = 0; // 默认选中基础数据
        }

        private void LoadTableList()
        {
            // 加载所有表名，可以根据需要过滤
            var tables = MainForm.Instance.AppContext.Db.Ado.GetDataTable(
                "select TABLE_NAME from information_schema.tables where table_catalog = @db order by TABLE_NAME",
                new { db = MainForm.Instance.AppContext.Db.Ado.Connection.Database });

            HLH.Lib.Helper.DropDownListHelper.InitDropList(
                tables.DataSet, 
                cmb目标数据表, 
                "TABLE_NAME", 
                "TABLE_NAME", 
                ComboBoxStyle.DropDown, 
                true, 
                true);
        }

        private void btn执行清理_Click(object sender, EventArgs e)
        {
            if (cmb目标数据表.SelectedValue == null || string.IsNullOrEmpty(cmb目标数据表.SelectedValue.ToString()))
            {
                MessageBox.Show("请选择要清理的数据表。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = cmb目标数据表.SelectedValue.ToString();
            string condition = txt清理条件.Text.Trim();
            bool isDryRun = chk模拟运行.Checked;

            // 安全检查：必须输入确认码
            if (!isDryRun && txt确认码.Text != "DELETE")
            {
                MessageBox.Show("为了安全起见，请在确认框中输入 'DELETE'。", "安全警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt确认码.Focus();
                return;
            }

            try
            {
                int affectedRows = 0;
                if (isDryRun)
                {
                    // 【核心改进】执行预览查询
                    var queryService = new RUINORERP.Business.DynamicQueryService(MainForm.Instance.AppContext.Db);
                    var dt = queryService.QueryTableAsync(tableName, null, 50).Result; // 简单预览前50条
                    dataGridView预览.DataSource = dt;
                    
                    affectedRows = queryService.GetCountAsync(tableName, condition).Result;
                    MainForm.Instance.PrintInfoLog($"[模拟运行] 表 [{tableName}] 预计影响 {affectedRows} 行数据。已加载前 50 条预览。", Color.Blue);
                }
                else
                {
                    if (MessageBox.Show($"确定要永久删除表 [{tableName}] 中的数据吗？\r\n此操作不可恢复！", "高危操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var queryService = new RUINORERP.Business.DynamicQueryService(MainForm.Instance.AppContext.Db);
                        affectedRows = queryService.ExecuteDeleteAsync(tableName, condition).Result;
                        MainForm.Instance.PrintInfoLog($"[清理完成] 表 [{tableName}] 成功删除 {affectedRows} 行数据。", Color.Red);
                        
                        // 清理后刷新预览
                        dataGridView预览.DataSource = null;
                        txt确认码.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog($"清理失败: {ex.Message}");
                MessageBox.Show($"清理过程中发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetAffectedCount(string tableName, string condition)
        {
            var sql = $"SELECT COUNT(1) FROM [{tableName}]";
            if (!string.IsNullOrEmpty(condition))
            {
                sql += $" WHERE {condition}";
            }
            return MainForm.Instance.AppContext.Db.Ado.GetInt(sql);
        }

        private int ExecuteDelete(string tableName, string condition)
        {
            // 【安全加固】简单的 SQL 注入防护
            if (!IsSafeSql(condition))
            {
                throw new Exception("清理条件包含非法字符或危险关键字（如 DROP, DELETE, ; 等）。");
            }

            var sql = $"DELETE FROM [{tableName}]";
            if (!string.IsNullOrEmpty(condition))
            {
                sql += $" WHERE {condition}";
            }
            return MainForm.Instance.AppContext.Db.Ado.ExecuteCommand(sql);
        }

        private bool IsSafeSql(string input)
        {
            if (string.IsNullOrEmpty(input)) return true;
            string lowerInput = input.ToLower();
            // 禁止危险关键字和特殊符号
            string[] dangerousKeywords = { "drop ", "delete ", "update ", "insert ", "alter ", "create ", "exec ", "--", ";", "xp_" };
            return !dangerousKeywords.Any(k => lowerInput.Contains(k));
        }

        private void btn快速示例_Click(object sender, EventArgs e)
        {
            string tableName = cmb目标数据表.SelectedValue?.ToString() ?? "";
            if (tableName.Contains("Prod"))
            {
                txt清理条件.Text = "ProdCode LIKE 'TEST%' OR CreateTime < '2023-01-01'";
            }
            else if (tableName.Contains("Order"))
            {
                txt清理条件.Text = "OrderStatus = 'Cancelled' AND CreateTime < '2022-01-01'";
            }
            else
            {
                txt清理条件.Text = "ID > 0"; // 危险示例
            }
            MessageBox.Show("已填入示例条件，请根据实际情况修改！\n注意：留空将清空整张表。", "提示");
        }
    }
}
