using Krypton.Navigator;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 基础数据导入UI组件
    /// 用于产品数据的导入操作
    [MenuAttrAssemblyInfo("基础数据导入", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    /// </summary>
    public partial class UCBasicDataImport : UserControl
    {
        private ISqlSugarClient _db;
        private ExcelDataParser _excelParser;
        private DataValidator _dataValidator;
        private CategoryImporter _categoryImporter;
        private ProductImporter _productImporter;
        private ImageProcessor _imageProcessor;
        
        private List<ProductImportModel> _importData;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCBasicDataImport()
        {
            InitializeComponent();
            InitializeData();
        }
        
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitializeData()
        {
            // 初始化数据列表
            _importData = new List<ProductImportModel>();
            
            // 初始化数据网格视图
            dgvImportData.AutoGenerateColumns = true;
            dgvImportData.DataSource = _importData;
            
            // 设置图片保存路径
            string imageSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");
            _imageProcessor = new ImageProcessor(imageSavePath);
            
            // 初始化其他组件
            _excelParser = new ExcelDataParser();
            _dataValidator = new DataValidator();
        }
        
        /// <summary>
        /// 加载数据库连接
        /// </summary>
        private void LoadDbConnection()
        {
            // 这里需要根据实际项目的数据库连接方式进行调整
            // 假设项目中已经有获取SqlSugarClient的方法
            _db = MainForm.Instance.AppContext.Db;
            
            // 初始化导入器
            _categoryImporter = new CategoryImporter(_db);
            _productImporter = new ProductImporter(_db, _categoryImporter, _imageProcessor);
        }
        
        /// <summary>
        /// 浏览Excel文件按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel文件|*.xls;*.xlsx";
                openFileDialog.Title = "选择产品数据Excel文件";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ktxtFilePath.Text = openFileDialog.FileName;
                    kbtnParse.Enabled = true;
                }
            }
        }
        
        /// <summary>
        /// 解析Excel文件按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnParse_Click(object sender, EventArgs e)
        {
            try
            {
                // 解析Excel文件
                _importData = _excelParser.ParseExcel(ktxtFilePath.Text);
                
                // 验证数据
                _importData = _dataValidator.ValidateProducts(_importData);
                
                // 绑定到数据网格视图
                dgvImportData.DataSource = _importData;
                
                // 更新状态信息
                int successCount = _importData.Count(p => p.ImportStatus);
                int failedCount = _importData.Count(p => !p.ImportStatus);
                
                // 启用导入按钮
                kbtnImport.Enabled = _importData.Count > 0;
                
                // 显示无效记录
                if (failedCount > 0)
                {
                    var failedRecords = _importData.Where(p => !p.ImportStatus).ToList();
                    dgvImportData.DataSource = failedRecords;
                    MessageBox.Show($"发现 {failedCount} 条无效记录，请检查数据格式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解析Excel文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 导入数据按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // 加载数据库连接
                LoadDbConnection();
                
                // 获取有效数据
                var validData = _importData.Where(p => p.ImportStatus).ToList();
                if (validData.Count == 0)
                {
                    MessageBox.Show("没有可导入的有效数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 显示确认对话框
                if (MessageBox.Show($"确定要导入 {validData.Count} 条产品数据吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                
                // 开始导入
                kbtnImport.Enabled = false;
                kbtnBrowse.Enabled = false;
                kbtnParse.Enabled = false;
                
                Application.DoEvents();
                
                // 执行导入
                var result = _productImporter.BatchImportProducts(validData);
                
                // 显示导入结果
                StringBuilder message = new StringBuilder();
                message.AppendLine($"导入完成！");
                message.AppendLine($"总记录数：{result.TotalCount}");
                message.AppendLine($"成功记录数：{result.SuccessCount}");
                message.AppendLine($"失败记录数：{result.FailedCount}");
                message.AppendLine($"耗时：{result.ElapsedMilliseconds} 毫秒");
                
                if (result.FailedCount > 0)
                {
                    message.AppendLine($"\n失败记录详情：");
                    foreach (var failedRecord in result.FailedRecords)
                    {
                        message.AppendLine($"行号 {failedRecord.RowNumber}：{failedRecord.ErrorMessage}");
                    }
                }
                
                MessageBox.Show(message.ToString(), "导入结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // 更新导入结果页面
                klblTotalCount.Text = result.TotalCount.ToString();
                klblSuccessCount.Text = result.SuccessCount.ToString();
                klblFailedCount.Text = result.FailedCount.ToString();
                klblElapsedTime.Text = $"{result.ElapsedMilliseconds} 毫秒";
                
                // 切换到结果页面
                kryptonNavigator1.SelectedPage = kryptonPageResult;
                
                // 重置状态
                ResetControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetControls();
            }
        }
        
        /// <summary>
        /// 重置控件状态
        /// </summary>
        private void ResetControls()
        {
            kbtnImport.Enabled = true;
            kbtnBrowse.Enabled = true;
            kbtnParse.Enabled = !string.IsNullOrEmpty(ktxtFilePath.Text);
        }
        
        /// <summary>
        /// 导出模板按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                // 这里可以实现导出Excel模板的功能
                MessageBox.Show("导出模板功能将在后续版本中实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出模板失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}