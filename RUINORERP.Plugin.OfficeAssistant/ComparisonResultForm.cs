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
                if (firstRecord.Data != null)
                {
                    foreach (var key in firstRecord.Data.Keys)
                    {
                        addedRecordsTable.Columns.Add(key);
                    }
                }
                
                // 添加数据行
                foreach (var record in comparisonResult.AddedRecords)
                {
                    var row = addedRecordsTable.NewRow();
                    
                    // 填充键值
                    for (int i = 0; i < record.KeyValues.Length; i++)
                    {
                        row[i] = record.KeyValues[i];
                    }
                    
                    // 填充数据值
                    if (record.Data != null)
                    {
                        var dataValues = record.Data.Values.ToList();
                        for (int i = 0; i < dataValues.Count; i++)
                        {
                            row[firstRecord.KeyValues.Length + i] = dataValues[i]?.ToString() ?? "";
                        }
                    }
                    
                    addedRecordsTable.Rows.Add(row);
                }
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
                if (firstRecord.Data != null)
                {
                    foreach (var key in firstRecord.Data.Keys)
                    {
                        deletedRecordsTable.Columns.Add(key);
                    }
                }
                
                // 添加数据行
                foreach (var record in comparisonResult.DeletedRecords)
                {
                    var row = deletedRecordsTable.NewRow();
                    
                    // 填充键值
                    for (int i = 0; i < record.KeyValues.Length; i++)
                    {
                        row[i] = record.KeyValues[i];
                    }
                    
                    // 填充数据值
                    if (record.Data != null)
                    {
                        var dataValues = record.Data.Values.ToList();
                        for (int i = 0; i < dataValues.Count; i++)
                        {
                            row[firstRecord.KeyValues.Length + i] = dataValues[i]?.ToString() ?? "";
                        }
                    }
                    
                    deletedRecordsTable.Rows.Add(row);
                }
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
                if (firstRecord.Differences != null)
                {
                    foreach (var key in firstRecord.Differences.Keys)
                    {
                        modifiedRecordsTable.Columns.Add($"{key}_旧值");
                        modifiedRecordsTable.Columns.Add($"{key}_新值");
                    }
                }
                
                // 添加数据行
                foreach (var record in comparisonResult.ModifiedRecords)
                {
                    var row = modifiedRecordsTable.NewRow();
                    
                    // 填充键值
                    for (int i = 0; i < record.KeyValues.Length; i++)
                    {
                        row[i] = record.KeyValues[i];
                    }
                    
                    // 填充差异值
                    if (record.Differences != null)
                    {
                        var differences = record.Differences.ToList();
                        for (int i = 0; i < differences.Count; i++)
                        {
                            var columnIndex = firstRecord.KeyValues.Length + i * 2;
                            row[columnIndex] = differences[i].Value.OldValue ?? "";
                            row[columnIndex + 1] = differences[i].Value.NewValue ?? "";
                        }
                    }
                    
                    modifiedRecordsTable.Rows.Add(row);
                }
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
    }
}