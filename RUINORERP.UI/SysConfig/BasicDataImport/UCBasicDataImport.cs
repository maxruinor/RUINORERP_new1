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
using Microsoft.VisualBasic;

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

        // 动态导入相关字段
        private DynamicExcelParser _dynamicExcelParser;
        private ColumnMappingManager _columnMappingManager;
        private DynamicImporter _dynamicImporter;
        private DynamicDataValidator _dynamicDataValidator;
        private ColumnMappingCollection _currentMappings;
        private DataTable _dynamicImportData;
        private Type _selectedEntityType;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCBasicDataImport()
        {
            InitializeComponent();
            InitializeData();
            InitializeDynamicImport();
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
        /// 初始化动态导入功能
        /// </summary>
        private void InitializeDynamicImport()
        {
            // 初始化动态导入组件
            _dynamicExcelParser = new DynamicExcelParser();
            _columnMappingManager = new ColumnMappingManager();
            _dynamicDataValidator = new DynamicDataValidator();
            _currentMappings = new ColumnMappingCollection();
            _dynamicImportData = new DataTable();

            // 初始化数据网格视图
            dgvDynamicImportData.AutoGenerateColumns = true;
            dgvDynamicImportData.DataSource = _dynamicImportData;

            // 初始化实体类型选择下拉框
            InitializeEntityTypes();

            // 加载映射配置列表
            LoadMappingConfigs();

            // 绑定事件
            kbtnDynamicBrowse.Click += KbtnDynamicBrowse_Click;
            kbtnDynamicParse.Click += KbtnDynamicParse_Click;
            kbtnDynamicMap.Click += KbtnDynamicMap_Click;
            kbtnDynamicImport.Click += KbtnDynamicImport_Click;
            kcmbDynamicSheetName.SelectedIndexChanged += KcmbDynamicSheetName_SelectedIndexChanged;
            kcmbDynamicEntityType.SelectedIndexChanged += KcmbDynamicEntityType_SelectedIndexChanged;
            kcmbDynamicMappingName.SelectedIndexChanged += KcmbDynamicMappingName_SelectedIndexChanged;

            // 初始状态：未选择实体类型时禁用映射配置相关按钮
            UpdateMappingControlStates();
        }

        /// <summary>
        /// 初始化实体类型选择下拉框
        /// </summary>
        private void InitializeEntityTypes()
        {
            try
            {
                kcmbDynamicEntityType.Items.Clear();
                kcmbDynamicEntityType.Items.Add("请选择");

                // 添加支持的实体类型
                kcmbDynamicEntityType.Items.Add("产品信息 (tb_Prod)");
                kcmbDynamicEntityType.Items.Add("产品类目 (tb_ProdCategories)");
                kcmbDynamicEntityType.Items.Add("产品属性类型 (tb_ProdPropertyType)");
                kcmbDynamicEntityType.Items.Add("产品属性 (tb_ProdProperty)");
                kcmbDynamicEntityType.Items.Add("产品属性值 (tb_ProdPropertyValue)");
                kcmbDynamicEntityType.Items.Add("产品明细 (tb_ProdDetail)");

                kcmbDynamicEntityType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化实体类型列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 实体类型选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void KcmbDynamicEntityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (kcmbDynamicEntityType.SelectedIndex <= 0)
                {
                    _selectedEntityType = null;
                    UpdateMappingControlStates();
                    return;
                }

                // 根据选择获取实体类型
                string selectedText = kcmbDynamicEntityType.SelectedItem.ToString();
                switch (selectedText)
                {
                    case "产品信息 (tb_Prod)":
                        _selectedEntityType = typeof(tb_Prod);
                        break;
                    case "产品类目 (tb_ProdCategories)":
                        _selectedEntityType = typeof(tb_ProdCategories);
                        break;
                    case "产品属性类型 (tb_ProdPropertyType)":
                        _selectedEntityType = typeof(tb_ProdPropertyType);
                        break;
                    case "产品属性 (tb_ProdProperty)":
                        _selectedEntityType = typeof(tb_ProdProperty);
                        break;
                    case "产品属性值 (tb_ProdPropertyValue)":
                        _selectedEntityType = typeof(tb_ProdPropertyValue);
                        break;
                    case "产品明细 (tb_ProdDetail)":
                        _selectedEntityType = typeof(tb_ProdDetail);
                        break;
                }

                // 加载对应的映射配置
                LoadMappingConfigsForEntityType();

                // 更新映射配置相关控件状态
                UpdateMappingControlStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择实体类型失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 映射配置选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void KcmbDynamicMappingName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (kcmbDynamicMappingName.SelectedIndex <= 0)
                {
                    return;
                }

                // 加载选中的映射配置
                LoadSelectedMapping();

                // 启用映射配置按钮
                kbtnDynamicMap.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新映射配置相关控件状态
        /// </summary>
        private void UpdateMappingControlStates()
        {
            bool hasEntityType = _selectedEntityType != null;
            bool hasData = _dynamicImportData != null && _dynamicImportData.Rows.Count > 0;

            // 映射配置按钮需要：已选择实体类型 且 已解析数据
            kbtnDynamicMap.Enabled = hasEntityType && hasData;
        }
        
        /// <summary>
        /// 加载映射配置列表
        /// </summary>
        private void LoadMappingConfigs()
        {
            LoadMappingConfigsForEntityType();
        }

        /// <summary>
        /// 加载指定实体类型的映射配置列表
        /// </summary>
        private void LoadMappingConfigsForEntityType()
        {
            try
            {
                var mappingNames = _columnMappingManager.GetAllMappingNames();
                kcmbDynamicMappingName.Items.Clear();
                kcmbDynamicMappingName.Items.Add("请选择");

                string entityTypeName = _selectedEntityType?.Name ?? "";

                foreach (var name in mappingNames)
                {
                    try
                    {
                        // 尝试加载映射配置以检查实体类型
                        var mapping = _columnMappingManager.LoadMapping(name);
                        if (mapping.Any(m => m.EntityType == entityTypeName) || string.IsNullOrEmpty(entityTypeName))
                        {
                            kcmbDynamicMappingName.Items.Add(name);
                        }
                    }
                    catch
                    {
                        // 忽略加载失败的配置文件
                    }
                }

                kcmbDynamicMappingName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载映射配置列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        
        #region 动态导入事件处理
        
        /// <summary>
        /// 动态导入-浏览文件按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void KbtnDynamicBrowse_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel文件|*.xls;*.xlsx";
                openFileDialog.Title = "选择动态导入的Excel文件";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ktxtDynamicFilePath.Text = openFileDialog.FileName;
                    kbtnDynamicParse.Enabled = true;
                    
                    // 加载工作表名称
                    LoadSheetNames(openFileDialog.FileName);
                }
            }
        }
        
        /// <summary>
        /// 加载Excel文件的工作表名称
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        private void LoadSheetNames(string filePath)
        {
            try
            {
                string[] sheetNames = _dynamicExcelParser.GetSheetNames(filePath);
                kcmbDynamicSheetName.Items.Clear();
                
                foreach (var sheetName in sheetNames)
                {
                    kcmbDynamicSheetName.Items.Add(sheetName);
                }
                
                if (kcmbDynamicSheetName.Items.Count > 0)
                {
                    kcmbDynamicSheetName.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载工作表名称失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 动态导入-解析文件按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void KbtnDynamicParse_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选择了工作表
                if (kcmbDynamicSheetName.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择要解析的工作表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 性能优化：先读取前100行用于预览（减少内存占用）
                // 实际导入时会读取全部数据
                _dynamicImportData = _dynamicExcelParser.ParseExcelToDataTable(ktxtDynamicFilePath.Text, kcmbDynamicSheetName.SelectedIndex, 100);

                // 绑定到数据网格视图
                dgvDynamicImportData.DataSource = _dynamicImportData;

                // 更新映射配置相关控件状态
                UpdateMappingControlStates();

                MessageBox.Show($"解析完成，预览显示前 {_dynamicImportData.Rows.Count} 行数据\n实际导入时会读取全部数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解析Excel文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sheet选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void KcmbDynamicSheetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当用户选择了Sheet后，启用解析按钮
            kbtnDynamicParse.Enabled = kcmbDynamicSheetName.SelectedIndex >= 0;
        }
        
        /// <summary>
        /// 动态导入-映射配置按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void KbtnDynamicMap_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选择了实体类型
                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择数据类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var frmMapping = new frmColumnMappingConfig())
                {
                    // 设置参数
                    frmMapping.ExcelData = _dynamicImportData;
                    frmMapping.TargetEntityType = _selectedEntityType;
                    frmMapping.ColumnMappings = _currentMappings;

                    // 显示映射配置窗体
                    if (frmMapping.ShowDialog() == DialogResult.OK)
                    {
                        // 更新映射配置
                        _currentMappings = frmMapping.ColumnMappings;

                        // 启用导入按钮
                        kbtnDynamicImport.Enabled = true;

                        MessageBox.Show("列映射配置已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开映射配置界面失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 动态导入-导入数据按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void KbtnDynamicImport_Click(object sender, EventArgs e)
        {
            try
            {
                // 加载数据库连接
                LoadDbConnection();

                // 执行动态导入
                ExecuteDynamicImport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        #endregion

        /// <summary>
        /// 保存当前映射配置
        /// </summary>
        private void SaveCurrentMapping()
        {
            try
            {
                if (_currentMappings == null || _currentMappings.Count == 0)
                {
                    MessageBox.Show("没有可保存的映射配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mappingName = Microsoft.VisualBasic.Interaction.InputBox("请输入映射配置名称", "保存映射配置", "");
                if (string.IsNullOrEmpty(mappingName))
                {
                    return;
                }

                _columnMappingManager.SaveMapping(_currentMappings, mappingName, _selectedEntityType?.Name);
                LoadMappingConfigsForEntityType();

                MessageBox.Show("映射配置保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载选中的映射配置
        /// </summary>
        private void LoadSelectedMapping()
        {
            try
            {
                if (kcmbDynamicMappingName.SelectedIndex <= 0)
                {
                    return;
                }

                string mappingName = kcmbDynamicMappingName.SelectedItem.ToString();
                _currentMappings = _columnMappingManager.LoadMapping(mappingName);

                MessageBox.Show($"已加载映射配置：{mappingName}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行动态导入
        /// </summary>
        private void ExecuteDynamicImport()
        {
            try
            {
                // 检查是否选择了实体类型
                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择数据类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查是否配置了映射
                if (_currentMappings == null || _currentMappings.Count == 0)
                {
                    MessageBox.Show("请先配置列映射关系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查是否有数据
                if (_dynamicImportData == null || _dynamicImportData.Rows.Count == 0)
                {
                    MessageBox.Show("没有可导入的数据，请先解析Excel文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 重要：在导入前重新读取全部数据（预览时只读取了前100行）
                MessageBox.Show("正在读取全部数据...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataTable fullData = _dynamicExcelParser.ParseExcelToDataTable(ktxtDynamicFilePath.Text, kcmbDynamicSheetName.SelectedIndex);

                // 数据验证
                var validationErrors = _dynamicDataValidator.Validate(fullData, _currentMappings, _selectedEntityType);
                if (validationErrors.Count > 0)
                {
                    string errorSummary = $"发现 {validationErrors.Count} 个数据验证错误：\n\n";

                    // 只显示前10个错误
                    int displayCount = Math.Min(10, validationErrors.Count);
                    for (int i = 0; i < displayCount; i++)
                    {
                        var error = validationErrors[i];
                        errorSummary += $"行 {error.RowNumber} - {error.FieldName}: {error.ErrorMessage}\n";
                    }

                    if (validationErrors.Count > displayCount)
                    {
                        errorSummary += $"\n... 还有 {validationErrors.Count - displayCount} 个错误未显示";
                    }

                    errorSummary += "\n\n是否继续导入（跳过有错误的记录）？";

                    if (MessageBox.Show(errorSummary, "数据验证警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // 显示确认对话框
                if (MessageBox.Show($"确定要导入 {fullData.Rows.Count} 条数据到 {_selectedEntityType.Name} 吗？", "确认导入", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                // 开始导入
                kbtnDynamicImport.Enabled = false;
                kbtnDynamicBrowse.Enabled = false;
                kbtnDynamicParse.Enabled = false;
                kbtnDynamicMap.Enabled = false;

                Application.DoEvents();

                // 初始化导入器
                _dynamicImporter = new DynamicImporter(_db);

                // 执行导入（使用全部数据）
                var result = _dynamicImporter.Import(fullData, _currentMappings, _selectedEntityType);

                // 显示导入结果
                StringBuilder message = new StringBuilder();
                message.AppendLine("动态导入完成！");
                message.AppendLine($"总记录数：{result.TotalCount}");
                message.AppendLine($"成功记录数：{result.SuccessCount}");
                message.AppendLine($"失败记录数：{result.FailedCount}");
                message.AppendLine($"新增记录数：{result.InsertedCount}");
                message.AppendLine($"更新记录数：{result.UpdatedCount}");
                message.AppendLine($"耗时：{result.ElapsedMilliseconds} 毫秒");

                if (result.FailedCount > 0)
                {
                    message.AppendLine($"\n失败记录详情：");
                    int displayCount = Math.Min(10, result.FailedRecords.Count);
                    for (int i = 0; i < displayCount; i++)
                    {
                        message.AppendLine($"行号 {result.FailedRecords[i].RowNumber}：{result.FailedRecords[i].ErrorMessage}");
                    }

                    if (result.FailedRecords.Count > 10)
                    {
                        message.AppendLine($"\n... 还有 {result.FailedRecords.Count - 10} 条失败记录未显示");
                    }
                }

                MessageBox.Show(message.ToString(), "导入结果", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 重置状态
                kbtnDynamicImport.Enabled = true;
                kbtnDynamicBrowse.Enabled = true;
                kbtnDynamicParse.Enabled = kcmbDynamicSheetName.SelectedIndex >= 0;
                kbtnDynamicMap.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                kbtnDynamicImport.Enabled = true;
                kbtnDynamicBrowse.Enabled = true;
                kbtnDynamicParse.Enabled = true;
                kbtnDynamicMap.Enabled = true;
            }
        }
    }
}