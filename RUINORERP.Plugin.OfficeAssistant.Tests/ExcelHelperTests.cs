using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RUINORERP.Plugin.OfficeAssistant.Shared;
using ClosedXML.Excel;

namespace RUINORERP.Plugin.OfficeAssistant.Tests
{
    [TestClass]
    public class ExcelHelperTests
    {
        private string testDirectory;
        
        [TestInitialize]
        public void Setup()
        {
            testDirectory = Path.Combine(Path.GetTempPath(), "OfficeAssistantTests");
            if (!Directory.Exists(testDirectory))
            {
                Directory.CreateDirectory(testDirectory);
            }
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(testDirectory))
            {
                Directory.Delete(testDirectory, true);
            }
        }
        
        [TestMethod]
        public void ReadExcelData_ValidFile_ReturnsDataTable()
        {
            // Arrange
            var filePath = Path.Combine(testDirectory, "test.xlsx");
            CreateTestExcelFile(filePath);
            
            // Act
            var result = ExcelHelper.ReadExcelData(filePath);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual(3, result.Columns.Count);
            Assert.AreEqual("Name", result.Columns[0].ColumnName);
            Assert.AreEqual("Age", result.Columns[1].ColumnName);
            Assert.AreEqual("City", result.Columns[2].ColumnName);
        }
        
        [TestMethod]
        public void GetWorksheetNames_ValidFile_ReturnsNames()
        {
            // Arrange
            var filePath = Path.Combine(testDirectory, "test.xlsx");
            CreateTestExcelFile(filePath);
            
            // Act
            var result = ExcelHelper.GetWorksheetNames(filePath);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Sheet1", result[0]);
        }
        
        private void CreateTestExcelFile(string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");
                
                // 添加标题行
                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Age";
                worksheet.Cell(1, 3).Value = "City";
                
                // 添加数据行
                worksheet.Cell(2, 1).Value = "Alice";
                worksheet.Cell(2, 2).Value = 30;
                worksheet.Cell(2, 3).Value = "New York";
                
                worksheet.Cell(3, 1).Value = "Bob";
                worksheet.Cell(3, 2).Value = 25;
                worksheet.Cell(3, 3).Value = "Los Angeles";
                
                workbook.SaveAs(filePath);
            }
        }
    }
}