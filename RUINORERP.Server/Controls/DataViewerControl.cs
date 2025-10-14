using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Server.Comm;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;

namespace RUINORERP.Server.Controls
{
    public partial class DataViewerControl : UserControl
    {
        public DataViewerControl()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                // 初始化缓存表列表
                LoadCacheTableList();
                
                // 初始化DataGridView
                dataGridViewData.AutoGenerateColumns = false;
                dataGridViewData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化数据查看器时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataViewerControl_Load(object sender, EventArgs e)
        {
            // 设置控件属性
        }

        /// <summary>
        /// 加载缓存表列表
        /// </summary>
        private void LoadCacheTableList()
        {
            try
            {
                listBoxTableList.Items.Clear();
                
                // 添加锁定信息列表
                listBoxTableList.Items.Add("锁定信息列表");
                
                // 添加缓存表列表
                if (MyCacheManager.Instance != null && MyCacheManager.Instance.NewTableList != null)
                {
                    foreach (KeyValuePair<string, KeyValuePair<string, string>> table in MyCacheManager.Instance.NewTableList)
                    {
                        listBoxTableList.Items.Add(table.Value.Value);
                    }
                }
                
                // 如果有项目，选择第一个
                if (listBoxTableList.Items.Count > 0)
                {
                    listBoxTableList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载缓存表列表时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 列表框事件处理

        private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedTable = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载指定表的数据
        /// </summary>
        /// <param name="tableName">表名</param>
        private void LoadTableData(string tableName)
        {
            try
            {
                // 清空当前数据
                dataGridViewData.DataSource = null;
                
                if (tableName == "锁定信息列表")
                {
                    // 显示锁定信息
                    var lockItems = frmMainNew.Instance.lockManager.GetLockItems();
                    dataGridViewData.DataSource = lockItems;
                    return;
                }
                
                // 查找对应的表键名
                string tableKey = null;
                foreach (KeyValuePair<string, KeyValuePair<string, string>> table in MyCacheManager.Instance.NewTableList)
                {
                    if (table.Value.Value.Equals(tableName))
                    {
                        tableKey = table.Key;
                        break;
                    }
                }
                
                if (string.IsNullOrEmpty(tableKey))
                {
                    MessageBox.Show("未找到对应的表数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 获取缓存数据
                var cacheData = MyCacheManager.Instance.GetDictDataSource(tableKey);
                if (cacheData != null && cacheData.Count > 0)
                {
                    // 将数据绑定到DataGridView
                    var bindingList = new BindingList<object>(cacheData.Values.ToList());
                    dataGridViewData.DataSource = bindingList;
                }
                else
                {
                    MessageBox.Show("未找到缓存数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 按钮事件处理

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedTable = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedTable);
                    
                    MessageBox.Show("数据已刷新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV文件|*.csv|文本文件|*.txt|所有文件|*.*";
                saveFileDialog.Title = "导出数据到文件";
                saveFileDialog.FileName = "data_export.csv";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportDataToFile(saveFileDialog.FileName);
                    MessageBox.Show($"数据已导出到 {saveFileDialog.FileName}", "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnReloadCache_Click(object sender, EventArgs e)
        {
            try
            {
                // 重新加载缓存
                await frmMainNew.Instance.InitConfig(true);
                
                // 刷新表列表
                LoadCacheTableList();
                
                MessageBox.Show("缓存已重新加载", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重新加载缓存时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 数据导出

        /// <summary>
        /// 导出数据到文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        private void ExportDataToFile(string fileName)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();
                
                // 添加列标题
                if (dataGridViewData.Columns.Count > 0)
                {
                    List<string> headers = new List<string>();
                    foreach (DataGridViewColumn column in dataGridViewData.Columns)
                    {
                        headers.Add(column.HeaderText);
                    }
                    csvContent.AppendLine(string.Join(",", headers));
                }
                
                // 添加数据行
                foreach (DataGridViewRow row in dataGridViewData.Rows)
                {
                    if (row.IsNewRow) continue;
                    
                    List<string> values = new List<string>();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        values.Add(cell.Value?.ToString() ?? "");
                    }
                    csvContent.AppendLine(string.Join(",", values));
                }
                
                // 写入文件
                System.IO.File.WriteAllText(fileName, csvContent.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"导出数据到文件时出错: {ex.Message}");
            }
        }

        #endregion

        #region DataGridView事件处理

        private void dataGridViewData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewData.Rows.Count)
            {
                // 显示详细信息
                var rowData = dataGridViewData.Rows[e.RowIndex].DataBoundItem;
                if (rowData != null)
                {
                    string details = GetObjectDetails(rowData);
                    MessageBox.Show(details, "详细信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 获取对象的详细信息
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>详细信息字符串</returns>
        private string GetObjectDetails(object obj)
        {
            try
            {
                StringBuilder details = new StringBuilder();
                Type type = obj.GetType();
                
                details.AppendLine($"类型: {type.Name}");
                details.AppendLine();
                
                // 获取所有公共属性
                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    try
                    {
                        var value = prop.GetValue(obj);
                        details.AppendLine($"{prop.Name}: {value ?? "null"}");
                    }
                    catch (Exception ex)
                    {
                        details.AppendLine($"{prop.Name}: [获取值时出错: {ex.Message}]");
                    }
                }
                
                return details.ToString();
            }
            catch (Exception ex)
            {
                return $"获取对象详细信息时出错: {ex.Message}";
            }
        }

        #endregion
    }
}