using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Plugin.OfficeAssistant.Core
{
    /// <summary>
    /// 对比结果导出器
    /// </summary>
    public class ComparisonResultExporter
    {
        /// <summary>
        /// 将对比结果导出到Excel文件
        /// </summary>
        /// <param name="result">对比结果</param>
        /// <param name="filePath">导出文件路径</param>
        public void ExportToExcel(ComparisonResult result, string filePath)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    // 创建新增记录工作表
                    if (result.AddedRecords != null && result.AddedRecords.Any())
                    {
                        var addedWorksheet = workbook.Worksheets.Add("新增记录");
                        WriteRecordsToWorksheet(addedWorksheet, result.AddedRecords);
                    }

                    // 创建删除记录工作表
                    if (result.DeletedRecords != null && result.DeletedRecords.Any())
                    {
                        var deletedWorksheet = workbook.Worksheets.Add("删除记录");
                        WriteRecordsToWorksheet(deletedWorksheet, result.DeletedRecords);
                    }

                    // 创建修改记录工作表
                    if (result.ModifiedRecords != null && result.ModifiedRecords.Any())
                    {
                        var modifiedWorksheet = workbook.Worksheets.Add("修改记录");
                        WriteModifiedRecordsToWorksheet(modifiedWorksheet, result.ModifiedRecords);
                    }

                    // 创建摘要工作表
                    var summaryWorksheet = workbook.Worksheets.Add("对比摘要");
                    WriteSummaryToWorksheet(summaryWorksheet, result.Summary);

                    workbook.SaveAs(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"导出Excel文件时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 将记录写入工作表
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="records">记录列表</param>
        private void WriteRecordsToWorksheet(IXLWorksheet worksheet, List<DiffRecord> records)
        {
            if (records == null || !records.Any()) return;

            // 写入标题行
            var firstRecord = records.First();
            if (firstRecord == null) return;
            
            // 写入键列标题
            for (int i = 0; i < firstRecord.KeyValues?.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = $"键{i + 1}";
            }
            
            // 写入数据列标题
            if (firstRecord.Data != null)
            {
                var dataKeys = firstRecord.Data.Keys.ToList();
                for (int i = 0; i < dataKeys.Count; i++)
                {
                    worksheet.Cell(1, (firstRecord.KeyValues?.Length ?? 0) + i + 1).Value = dataKeys[i];
                }
            }

            // 写入数据
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                if (record == null) continue;
                
                // 写入键值
                for (int j = 0; j < record.KeyValues?.Length; j++)
                {
                    worksheet.Cell(i + 2, j + 1).Value = record.KeyValues[j];
                }
                
                // 写入数据值
                if (record.Data != null)
                {
                    var dataValues = record.Data.Values.ToList();
                    for (int j = 0; j < dataValues.Count; j++)
                    {
                        worksheet.Cell(i + 2, (record.KeyValues?.Length ?? 0) + j + 1).Value = dataValues[j]?.ToString() ?? "";
                    }
                }
            }
        }

        /// <summary>
        /// 将修改记录写入工作表
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="records">修改记录列表</param>
        private void WriteModifiedRecordsToWorksheet(IXLWorksheet worksheet, List<ModifiedRecord> records)
        {
            if (records == null || !records.Any()) return;

            // 写入标题行
            var firstRecord = records.First();
            if (firstRecord == null) return;
            
            // 写入键列标题
            for (int i = 0; i < firstRecord.KeyValues?.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = $"键{i + 1}";
            }
            
            // 写入差异列标题
            if (firstRecord.Differences != null)
            {
                var diffKeys = firstRecord.Differences.Keys.ToList();
                for (int i = 0; i < diffKeys.Count; i++)
                {
                    var columnIndex = (firstRecord.KeyValues?.Length ?? 0) + i * 3 + 1;
                    worksheet.Cell(1, columnIndex).Value = $"{diffKeys[i]}_字段";
                    worksheet.Cell(1, columnIndex + 1).Value = $"{diffKeys[i]}_旧值";
                    worksheet.Cell(1, columnIndex + 2).Value = $"{diffKeys[i]}_新值";
                }
            }

            // 写入数据
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                if (record == null) continue;
                
                // 写入键值
                for (int j = 0; j < record.KeyValues?.Length; j++)
                {
                    worksheet.Cell(i + 2, j + 1).Value = record.KeyValues[j];
                }
                
                // 写入差异值
                if (record.Differences != null)
                {
                    var diffEntries = record.Differences.ToList();
                    for (int j = 0; j < diffEntries.Count; j++)
                    {
                        var columnIndex = (record.KeyValues?.Length ?? 0) + j * 3 + 1;
                        worksheet.Cell(i + 2, columnIndex).Value = diffEntries[j].Key;
                        worksheet.Cell(i + 2, columnIndex + 1).Value = diffEntries[j].Value?.OldValue ?? "";
                        worksheet.Cell(i + 2, columnIndex + 2).Value = diffEntries[j].Value?.NewValue ?? "";
                    }
                }
            }
        }

        /// <summary>
        /// 将摘要写入工作表
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="summary">摘要</param>
        private void WriteSummaryToWorksheet(IXLWorksheet worksheet, ComparisonSummary summary)
        {
            if (summary == null) return;
            
            worksheet.Cell(1, 1).Value = "对比摘要";
            worksheet.Cell(2, 1).Value = "旧文件记录数";
            worksheet.Cell(2, 2).Value = summary.TotalOldRecords;
            worksheet.Cell(3, 1).Value = "新文件记录数";
            worksheet.Cell(3, 2).Value = summary.TotalNewRecords;
            worksheet.Cell(4, 1).Value = "新增记录数";
            worksheet.Cell(4, 2).Value = summary.AddedCount;
            worksheet.Cell(5, 1).Value = "删除记录数";
            worksheet.Cell(5, 2).Value = summary.DeletedCount;
            worksheet.Cell(6, 1).Value = "修改记录数";
            worksheet.Cell(6, 2).Value = summary.ModifiedCount;
        }
    }
}