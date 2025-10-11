using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RUINORERP.Plugin.OfficeAssistant.Core;

namespace RUINORERP.Plugin.OfficeAssistant
{
    public partial class ComparisonResultForm : Form
    {
        private ComparisonResult comparisonResult;
        private DataTable addedRecordsTable;
        private DataTable deletedRecordsTable;
        private DataTable modifiedRecordsTable;

        public ComparisonResultForm(ComparisonResult result)
        {
            InitializeComponent();
            this.comparisonResult = result;
            InitializeResultDisplay();
        }

        private void InitializeResultDisplay()
        {
            // 创建新增记录表格
            CreateAddedRecordsTable();
            
            // 创建删除记录表格
            CreateDeletedRecordsTable();
            
            // 创建修改记录表格
            CreateModifiedRecordsTable();
            
            // 绑定数据
            dataGridViewAdded.DataSource = addedRecordsTable;
            dataGridViewDeleted.DataSource = deletedRecordsTable;
            dataGridViewModified.DataSource = modifiedRecordsTable;
            
            // 设置行号显示
            dataGridViewAdded.RowPostPaint += DataGridView_RowPostPaint;
            dataGridViewDeleted.RowPostPaint += DataGridView_RowPostPaint;
            dataGridViewModified.RowPostPaint += DataGridView_RowPostPaint;
            
            // 应用颜色标识
            ApplyColorCoding();
        }

        private void CreateAddedRecordsTable()
        {
            addedRecordsTable = new DataTable();
            
            if (comparisonResult.AddedRecords != null && comparisonResult.AddedRecords.Any())
            {
                var firstRecord = comparisonResult.AddedRecords.First();
                
                // 添加键列
                for (int i = 0; i < firstRecord.KeyValues.Length; i++)
                {
                    addedRecordsTable.Columns.Add($"键{i + 1}");
                }
                
                // 添加数据列
                var columnNames = new List<string>();
                if (firstRecord.Data != null)
                {
                    foreach (var key in firstRecord.Data.Keys)
                    {
                        columnNames.Add(key);
                        addedRecordsTable.Columns.Add(key);
                    }
                }
                
                // 显示进度条
                progressBar.Visible = true;
                progressBar.Maximum = comparisonResult.AddedRecords.Count;
                progressBar.Value = 0;
                
                // 添加数据行
                int rowIndex = 0;
                foreach (var record in comparisonResult.AddedRecords)
                {
                    var row = addedRecordsTable.NewRow();
                    
                    // 填充键值
                    for (int i = 0; i < record.KeyValues.Length; i++)
                    {
                        row[i] = record.KeyValues[i];
                    }
                    
                    // 填充数据值 - 按列名正确填充
                    if (record.Data != null)
                    {
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            var columnName = columnNames[i];
                            if (record.Data.ContainsKey(columnName))
                            {
                                row[firstRecord.KeyValues.Length + i] = record.Data[columnName]?.ToString() ?? "";
                            }
                        }
                    }
                    
                    addedRecordsTable.Rows.Add(row);
                    
                    // 更新进度条
                    rowIndex++;
                    if (rowIndex % 100 == 0 || rowIndex == comparisonResult.AddedRecords.Count) // 每100行或最后一条记录时更新进度条
                    {
                        progressBar.Value = Math.Min(rowIndex, progressBar.Maximum);
                        Application.DoEvents(); // 允许UI更新
                    }
                }
                
                progressBar.Visible = false;
            }
            else
            {
                // 如果没有新增记录，添加提示信息
                addedRecordsTable.Columns.Add("提示信息");
                var row = addedRecordsTable.NewRow();
                row[0] = "没有在新文件中找到旧文件中不存在的记录";
                addedRecordsTable.Rows.Add(row);
            }
        }

        private void CreateDeletedRecordsTable()
        {
            deletedRecordsTable = new DataTable();
            
            if (comparisonResult.DeletedRecords != null && comparisonResult.DeletedRecords.Any())
            {
                var firstRecord = comparisonResult.DeletedRecords.First();
                
                // 添加键列
                for (int i = 0; i < firstRecord.KeyValues.Length; i++)
                {
                    deletedRecordsTable.Columns.Add($"键{i + 1}");
                }
                
                // 添加数据列
                var columnNames = new List<string>();
                if (firstRecord.Data != null)
                {
                    foreach (var key in firstRecord.Data.Keys)
                    {
                        columnNames.Add(key);
                        deletedRecordsTable.Columns.Add(key);
                    }
                }
                
                // 显示进度条
                progressBar.Visible = true;
                progressBar.Maximum = comparisonResult.DeletedRecords.Count;
                progressBar.Value = 0;
                
                // 添加数据行
                int rowIndex = 0;
                foreach (var record in comparisonResult.DeletedRecords)
                {
                    var row = deletedRecordsTable.NewRow();
                    
                    // 填充键值
                    for (int i = 0; i < record.KeyValues.Length; i++)
                    {
                        row[i] = record.KeyValues[i];
                    }
                    
                    // 填充数据值 - 按列名正确填充
                    if (record.Data != null)
                    {
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            var columnName = columnNames[i];
                            if (record.Data.ContainsKey(columnName))
                            {
                                row[firstRecord.KeyValues.Length + i] = record.Data[columnName]?.ToString() ?? "";
                            }
                        }
                    }
                    
                    deletedRecordsTable.Rows.Add(row);
                    
                    // 更新进度条
                    rowIndex++;
                    if (rowIndex % 100 == 0 || rowIndex == comparisonResult.DeletedRecords.Count) // 每100行或最后一条记录时更新进度条
                    {
                        progressBar.Value = Math.Min(rowIndex, progressBar.Maximum);
                        Application.DoEvents(); // 允许UI更新
                    }
                }
                
                progressBar.Visible = false;
            }
            else
            {
                // 如果没有删除记录，添加提示信息
                deletedRecordsTable.Columns.Add("提示信息");
                var row = deletedRecordsTable.NewRow();
                row[0] = "没有在旧文件中找到新文件中不存在的记录";
                deletedRecordsTable.Rows.Add(row);
            }
        }

        private void CreateModifiedRecordsTable()
        {
            modifiedRecordsTable = new DataTable();
            
            if (comparisonResult.ModifiedRecords != null && comparisonResult.ModifiedRecords.Any())
            {
                var firstRecord = comparisonResult.ModifiedRecords.First();
                
                // 添加键列
                for (int i = 0; i < firstRecord.KeyValues.Length; i++)
                {
                    modifiedRecordsTable.Columns.Add($"键{i + 1}");
                }
                
                // 添加差异列
                var columnNames = new List<string>();
                if (firstRecord.Differences != null)
                {
                    foreach (var key in firstRecord.Differences.Keys)
                    {
                        columnNames.Add(key);
                        modifiedRecordsTable.Columns.Add($"{key}_旧值");
                        modifiedRecordsTable.Columns.Add($"{key}_新值");
                    }
                }
                
                // 显示进度条
                progressBar.Visible = true;
                progressBar.Maximum = comparisonResult.ModifiedRecords.Count;
                progressBar.Value = 0;
                
                // 添加数据行
                int rowIndex = 0;
                foreach (var record in comparisonResult.ModifiedRecords)
                {
                    var row = modifiedRecordsTable.NewRow();
                    
                    // 填充键值
                    for (int i = 0; i < record.KeyValues.Length; i++)
                    {
                        row[i] = record.KeyValues[i];
                    }
                    
                    // 填充差异值 - 按列名正确填充
                    if (record.Differences != null)
                    {
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            var columnName = columnNames[i];
                            if (record.Differences.ContainsKey(columnName))
                            {
                                var columnIndex = firstRecord.KeyValues.Length + i * 2;
                                row[columnIndex] = record.Differences[columnName].OldValue ?? "";
                                row[columnIndex + 1] = record.Differences[columnName].NewValue ?? "";
                            }
                        }
                    }
                    
                    modifiedRecordsTable.Rows.Add(row);
                    
                    // 更新进度条
                    rowIndex++;
                    if (rowIndex % 100 == 0 || rowIndex == comparisonResult.ModifiedRecords.Count) // 每100行或最后一条记录时更新进度条
                    {
                        progressBar.Value = Math.Min(rowIndex, progressBar.Maximum);
                        Application.DoEvents(); // 允许UI更新
                    }
                }
                
                progressBar.Visible = false;
            }
            else
            {
                // 如果没有修改记录，添加提示信息
                modifiedRecordsTable.Columns.Add("提示信息");
                var row = modifiedRecordsTable.NewRow();
                row[0] = "没有找到数据发生变化的记录";
                modifiedRecordsTable.Rows.Add(row);
            }
        }

        private void ApplyColorCoding()
        {
            // 为新增记录应用绿色背景
            foreach (DataGridViewRow row in dataGridViewAdded.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.LightGreen;
                }
            }
            
            // 为删除记录应用红色背景
            foreach (DataGridViewRow row in dataGridViewDeleted.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.LightCoral;
                }
            }
            
            // 为修改记录应用黄色背景
            foreach (DataGridViewRow row in dataGridViewModified.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.LightYellow;
                }
            }
        }
        
        /// <summary>
        /// DataGridView行号绘制事件处理
        /// </summary>
        private void DataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
    }
}