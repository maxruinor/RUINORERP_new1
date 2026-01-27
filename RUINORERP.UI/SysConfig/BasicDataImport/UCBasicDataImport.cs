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
        private DataTable _rawExcelData;           // 原始Excel数据（预览用）
        private DataTable _parsedImportData;         // 根据映射配置解析后的数据
        private Type _selectedEntityType;
        
        // 多步骤导入相关字段
        private List<ImportTask> _importTasks;
        private int _currentTaskIndex;
        
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
            _rawExcelData = new DataTable();
            _parsedImportData = new DataTable();
            _importTasks = new List<ImportTask>();
            _currentTaskIndex = 0;

            // 初始化数据网格视图
            dgvRawExcelData.AutoGenerateColumns = true;
            dgvRawExcelData.DataSource = _rawExcelData;
            dgvRawExcelData.UseCustomColumnDisplay = false;

            dgvParsedImportData.AutoGenerateColumns = true;
            dgvParsedImportData.DataSource = _parsedImportData;
            dgvParsedImportData.UseCustomColumnDisplay = false;
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

                // 添加支持的实体类型（仅显示中文描述）
                kcmbDynamicEntityType.Items.Add("供应商表");
                kcmbDynamicEntityType.Items.Add("产品类目表");
                kcmbDynamicEntityType.Items.Add("产品基本信息表");
                kcmbDynamicEntityType.Items.Add("产品详情信息表");
                kcmbDynamicEntityType.Items.Add("产品属性表");
                kcmbDynamicEntityType.Items.Add("产品属性值表");

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
                    case "供应商表":
                        _selectedEntityType = typeof(tb_CustomerVendor);
                        break;
                    case "产品类目表":
                        _selectedEntityType = typeof(tb_ProdCategories);
                        break;
                    case "产品基本信息表":
                        _selectedEntityType = typeof(tb_Prod);
                        break;
                    case "产品详情信息表":
                        _selectedEntityType = typeof(tb_ProdDetail);
                        break;
                    case "产品属性表":
                        _selectedEntityType = typeof(tb_ProdProperty);
                        break;
                    case "产品属性值表":
                        _selectedEntityType = typeof(tb_ProdPropertyValue);
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
                    _currentMappings = new ColumnMappingCollection();
                    UpdateMappingControlStates();
                    return;
                }

                // 加载选中的映射配置
                LoadSelectedMapping();

                // 更新映射配置按钮状态（选中配置后应该可以直接编辑或查看）
                UpdateMappingControlStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 管理映射配置
        /// </summary>
        private void ManageMappingConfigs()
        {
            try
            {
                // 获取所有映射配置名称
                var mappingNames = _columnMappingManager.GetAllMappingNames();
                if (mappingNames.Count == 0)
                {
                    MessageBox.Show("没有保存的映射配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 显示配置管理对话框
                StringBuilder configList = new StringBuilder();
                configList.AppendLine("映射配置管理：\n");

                for (int i = 0; i < mappingNames.Count; i++)
                {
                    configList.AppendLine($"{i + 1}. {mappingNames[i]}");
                }

                configList.AppendLine("\n请选择操作：");
                configList.AppendLine("1. 修改配置");
                configList.AppendLine("2. 删除配置");
                configList.AppendLine("3. 取消");

                string input = Interaction.InputBox(configList.ToString(), "配置管理", "3");
                int choice;
                if (!int.TryParse(input, out choice) || choice < 1 || choice > 3)
                {
                    return;
                }

                switch (choice)
                {
                    case 1:
                        // 修改配置
                        ModifyMappingConfig();
                        break;
                    case 2:
                        // 删除配置
                        DeleteMappingConfig();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"管理配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 修改映射配置
        /// </summary>
        private void ModifyMappingConfig()
        {
            try
            {
                // 获取所有映射配置名称
                var mappingNames = _columnMappingManager.GetAllMappingNames();
                if (mappingNames.Count == 0)
                {
                    MessageBox.Show("没有保存的映射配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 让用户选择要修改的配置
                StringBuilder configList = new StringBuilder();
                configList.AppendLine("请选择要修改的映射配置：\n");

                for (int i = 0; i < mappingNames.Count; i++)
                {
                    configList.AppendLine($"{i + 1}. {mappingNames[i]}");
                }

                string input = Interaction.InputBox(configList.ToString(), "修改配置", "1");
                int choice;
                if (!int.TryParse(input, out choice) || choice < 1 || choice > mappingNames.Count)
                {
                    return;
                }

                string selectedConfig = mappingNames[choice - 1];

                // 加载配置
                _currentMappings = _columnMappingManager.LoadMapping(selectedConfig);

                // 显示映射配置对话框
                using (var frmMapping = new frmColumnMappingConfig())
                {
                    frmMapping.ExcelData = _rawExcelData;
                    frmMapping.TargetEntityType = _selectedEntityType;
                    frmMapping.ColumnMappings = _currentMappings;

                    // 订阅映射配置保存成功事件
                    frmMapping.MappingSaved += (s, e) =>
                    {
                        // 刷新映射配置下拉列表
                        LoadMappingConfigsForEntityType();

                        // 自动选中刚修改的配置
                        if (!string.IsNullOrEmpty(frmMapping.SavedMappingName))
                        {
                            int index = kcmbDynamicMappingName.FindStringExact(frmMapping.SavedMappingName);
                            if (index > 0)
                            {
                                kcmbDynamicMappingName.SelectedIndex = index;
                            }
                        }
                    };

                    // 显示映射配置窗体
                    if (frmMapping.ShowDialog() == DialogResult.OK)
                    {
                        // 更新映射配置
                        _currentMappings = frmMapping.ColumnMappings;

                        // 更新按钮状态
                        UpdateMappingControlStates();

                        MessageBox.Show("映射配置已修改", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除映射配置
        /// </summary>
        private void DeleteMappingConfig()
        {
            try
            {
                // 获取所有映射配置名称
                var mappingNames = _columnMappingManager.GetAllMappingNames();
                if (mappingNames.Count == 0)
                {
                    MessageBox.Show("没有保存的映射配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 让用户选择要删除的配置
                StringBuilder configList = new StringBuilder();
                configList.AppendLine("请选择要删除的映射配置：\n");

                for (int i = 0; i < mappingNames.Count; i++)
                {
                    configList.AppendLine($"{i + 1}. {mappingNames[i]}");
                }

                string input = Interaction.InputBox(configList.ToString(), "删除配置", "1");
                int choice;
                if (!int.TryParse(input, out choice) || choice < 1 || choice > mappingNames.Count)
                {
                    return;
                }

                string selectedConfig = mappingNames[choice - 1];

                // 确认删除
                if (MessageBox.Show($"确定要删除映射配置 '{selectedConfig}' 吗？\n\n此操作不可恢复", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // 删除配置
                    _columnMappingManager.DeleteMapping(selectedConfig);

                    // 刷新映射配置下拉列表
                    LoadMappingConfigsForEntityType();

                    // 如果删除的是当前选中的配置，重置当前映射
                    if (kcmbDynamicMappingName.SelectedItem != null && kcmbDynamicMappingName.SelectedItem.ToString() == selectedConfig)
                    {
                        kcmbDynamicMappingName.SelectedIndex = 0;
                        _currentMappings = new ColumnMappingCollection();
                    }

                    MessageBox.Show($"映射配置 '{selectedConfig}' 已删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新映射配置相关控件状态
        /// </summary>
        private void UpdateMappingControlStates()
        {
            bool hasEntityType = _selectedEntityType != null;
            bool hasRawData = _rawExcelData != null && _rawExcelData.Rows.Count > 0;
            bool hasMappingSelected = kcmbDynamicMappingName.SelectedIndex > 0;

            // 映射配置按钮启用条件：
            // 1. 已选择实体类型，且（已加载原始数据 OR 已选择映射配置）
            kbtnDynamicMap.Enabled = hasEntityType && (hasRawData || hasMappingSelected);

            // 解析按钮需要：有原始数据、选择了实体类型、有映射配置
            bool hasMappingConfig = _currentMappings != null && _currentMappings.Count > 0;
            kbtnDynamicParse.Enabled = hasRawData && hasEntityType && hasMappingConfig;
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
        /// 根据映射配置解析Excel数据
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
                    MessageBox.Show("请先选择要解析的工作表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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

                // 读取全部数据（不再限制行数）
                DataTable fullData = _dynamicExcelParser.ParseExcelToDataTable(
                    ktxtDynamicFilePath.Text,
                    kcmbDynamicSheetName.SelectedIndex,
                    0); // 0表示读取全部数据

                // 根据映射配置转换数据
                _parsedImportData = ApplyColumnMapping(fullData, _currentMappings);

                // 绑定到解析后数据表格
                dgvParsedImportData.DataSource = _parsedImportData;

                // 切换到解析数据预览页面
                kryptonNavigatorDynamic.SelectedPage = kryptonPageParsedData;

                // 启用导入按钮
                kbtnDynamicImport.Enabled = true;

                MessageBox.Show($"根据映射配置解析完成，共 {_parsedImportData.Rows.Count} 行数据",
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解析Excel文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 应用列映射配置转换数据
        /// </summary>
        /// <param name="sourceData">源数据表格</param>
        /// <param name="mappings">列映射配置集合</param>
        /// <returns>转换后的数据表格</returns>
        private DataTable ApplyColumnMapping(DataTable sourceData, ColumnMappingCollection mappings)
        {
            DataTable result = new DataTable();

            try
            {
                // 创建结果表结构（使用SystemField作为列名）
                foreach (var mapping in mappings.Where(m => !string.IsNullOrEmpty(m.ExcelColumn)))
                {
                    result.Columns.Add(mapping.SystemField, typeof(string));
                }

                // 转换数据行
                foreach (DataRow sourceRow in sourceData.Rows)
                {
                    DataRow targetRow = result.NewRow();

                    foreach (var mapping in mappings.Where(m => !string.IsNullOrEmpty(m.ExcelColumn)))
                    {
                        if (sourceData.Columns.Contains(mapping.ExcelColumn))
                        {
                            targetRow[mapping.SystemField] = sourceRow[mapping.ExcelColumn]?.ToString() ?? "";
                        }
                        else
                        {
                            targetRow[mapping.SystemField] = "";
                        }
                    }

                    result.Rows.Add(targetRow);
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用列映射失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
        }

        /// <summary>
        /// Sheet选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void KcmbDynamicSheetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // 当用户选择了Sheet后，立即加载原始数据到预览表格
                if (kcmbDynamicSheetName.SelectedIndex < 0)
                {
                    return;
                }

                // 读取前100行用于预览（性能优化）
                _rawExcelData = _dynamicExcelParser.ParseExcelToDataTable(
                    ktxtDynamicFilePath.Text,
                    kcmbDynamicSheetName.SelectedIndex,
                    100); // 限制预览行数

                // 绑定到原始数据预览表格
                dgvRawExcelData.DataSource = _rawExcelData;

                // 启用解析按钮
                kbtnDynamicParse.Enabled = true;

                // 更新映射配置按钮状态
                UpdateMappingControlStates();

                MessageBox.Show($"已加载工作表数据，预览显示前 {_rawExcelData.Rows.Count} 行数据",
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载工作表数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                // 判断是编辑模式还是新增模式
                bool isEditMode = kcmbDynamicMappingName.SelectedIndex > 0;
                string mappingName = isEditMode ? kcmbDynamicMappingName.SelectedItem.ToString() : string.Empty;

                // 如果是编辑模式，先加载选中的配置
                if (isEditMode)
                {
                    LoadSelectedMapping();
                }

                // 使用原始Excel数据进行映射配置
                using (var frmMapping = new frmColumnMappingConfig())
                {
                    // 设置参数
                    frmMapping.ExcelData = _rawExcelData;
                    frmMapping.TargetEntityType = _selectedEntityType;
                    frmMapping.ColumnMappings = _currentMappings;
                    frmMapping.IsEditMode = isEditMode;
                    frmMapping.OriginalMappingName = mappingName;

                    // 订阅映射配置保存成功事件
                    frmMapping.MappingSaved += (s, args) =>
                    {
                        // 刷新映射配置下拉列表
                        LoadMappingConfigsForEntityType();

                        // 自动选中刚保存的配置
                        if (!string.IsNullOrEmpty(frmMapping.SavedMappingName))
                        {
                            int index = kcmbDynamicMappingName.FindStringExact(frmMapping.SavedMappingName);
                            if (index > 0)
                            {
                                kcmbDynamicMappingName.SelectedIndex = index;
                            }
                        }
                    };

                    // 显示映射配置窗体
                    if (frmMapping.ShowDialog() == DialogResult.OK)
                    {
                        // 更新映射配置
                        _currentMappings = frmMapping.ColumnMappings;

                        // 更新按钮状态
                        UpdateMappingControlStates();

                        string operationType = isEditMode ? "编辑" : "新增";
                        MessageBox.Show($"列映射配置已{operationType}，可以点击\"解析\"按钮进行数据转换",
                            "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // 检查是否有解析后的数据
                if (_parsedImportData == null || _parsedImportData.Rows.Count == 0)
                {
                    MessageBox.Show("没有可导入的数据，请先点击\"解析\"按钮转换数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 询问是否添加到多步骤导入任务
                if (MessageBox.Show("是否将此导入添加到多步骤导入任务中？\n\n选择'是'：将此导入添加到任务列表，可与其他导入任务一起顺序执行\n选择'否'：立即执行此导入", "多步骤导入", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // 添加到导入任务列表
                    AddToImportTask();
                }
                else
                {
                    // 立即执行导入
                    ExecuteSingleImport();
                }
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

        /// <summary>
        /// 添加到导入任务列表
        /// </summary>
        private void AddToImportTask()
        {
            try
            {
                // 获取任务名称
                string taskName = Interaction.InputBox("请输入任务名称", "添加导入任务", $"导入{_selectedEntityType.Name}");
                if (string.IsNullOrWhiteSpace(taskName))
                {
                    return;
                }

                // 创建导入任务
                var task = new ImportTask
                {
                    TaskName = taskName,
                    FilePath = ktxtDynamicFilePath.Text,
                    SheetName = kcmbDynamicSheetName.SelectedItem.ToString(),
                    EntityType = _selectedEntityType,
                    Mappings = _currentMappings,
                    Status = TaskStatus.Pending
                };

                // 添加到任务列表
                _importTasks.Add(task);

                // 显示任务列表
                ShowImportTaskList();

                MessageBox.Show($"导入任务 '{taskName}' 已添加到任务列表", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加导入任务失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行单个导入任务
        /// </summary>
        private void ExecuteSingleImport()
        {
            try
            {
                // 重要：在导入前重新读取全部数据并应用映射
                MessageBox.Show("正在读取全部数据并应用映射...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataTable fullRawData = _dynamicExcelParser.ParseExcelToDataTable(
                    ktxtDynamicFilePath.Text,
                    kcmbDynamicSheetName.SelectedIndex,
                    0); // 0表示读取全部数据

                DataTable fullParsedData = ApplyColumnMapping(fullRawData, _currentMappings);

                // 数据验证
                var validationErrors = _dynamicDataValidator.Validate(fullParsedData, _currentMappings, _selectedEntityType);
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
                if (MessageBox.Show($"确定要导入 {fullParsedData.Rows.Count} 条数据到 {_selectedEntityType.Name} 吗？",
                    "确认导入", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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

                // 执行导入（使用解析后的数据）
                var result = _dynamicImporter.Import(fullParsedData, _currentMappings, _selectedEntityType);

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

        /// <summary>
        /// 显示导入任务列表
        /// </summary>
        private void ShowImportTaskList()
        {
            try
            {
                StringBuilder taskList = new StringBuilder();
                taskList.AppendLine("导入任务列表：\n");

                for (int i = 0; i < _importTasks.Count; i++)
                {
                    var task = _importTasks[i];
                    taskList.AppendLine($"{i + 1}. {task.TaskName}");
                    taskList.AppendLine($"   状态：{task.Status}");
                    taskList.AppendLine($"   目标表：{task.EntityType.Name}");
                    taskList.AppendLine($"   文件：{Path.GetFileName(task.FilePath)}");
                    taskList.AppendLine($"   工作表：{task.SheetName}");
                    taskList.AppendLine();
                }

                // 询问是否执行所有任务
                if (_importTasks.Count > 0)
                {
                    taskList.AppendLine("是否执行所有任务？");
                    if (MessageBox.Show(taskList.ToString(), "导入任务列表", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // 执行所有导入任务
                        ExecuteAllImportTasks();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示任务列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行所有导入任务
        /// </summary>
        private void ExecuteAllImportTasks()
        {
            try
            {
                if (_importTasks.Count == 0)
                {
                    MessageBox.Show("导入任务列表为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 加载数据库连接
                LoadDbConnection();

                // 显示开始执行提示
                MessageBox.Show($"开始执行 {_importTasks.Count} 个导入任务，请耐心等待...", "执行任务", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 执行所有任务
                _currentTaskIndex = 0;
                foreach (var task in _importTasks)
                {
                    _currentTaskIndex++;
                    try
                    {
                        // 更新任务状态
                        task.Status = TaskStatus.Running;

                        // 显示当前执行的任务
                        MessageBox.Show($"正在执行任务 {_currentTaskIndex}/{_importTasks.Count}：{task.TaskName}", "执行任务", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 读取Excel数据
                        DataTable rawData = _dynamicExcelParser.ParseExcelToDataTable(
                            task.FilePath,
                            task.SheetName,
                            0);

                        // 应用映射配置
                        DataTable parsedData = ApplyColumnMapping(rawData, task.Mappings);

                        // 数据验证
                        var validationErrors = _dynamicDataValidator.Validate(parsedData, task.Mappings, task.EntityType);
                        if (validationErrors.Count > 0)
                        {
                            string errorSummary = $"任务 {task.TaskName} 发现 {validationErrors.Count} 个数据验证错误：\n\n";
                            errorSummary += "是否继续导入（跳过有错误的记录）？";

                            if (MessageBox.Show(errorSummary, "数据验证警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                            {
                                task.Status = TaskStatus.Failed;
                                continue;
                            }
                        }

                        // 执行导入
                        _dynamicImporter = new DynamicImporter(_db);
                        task.Result = _dynamicImporter.Import(parsedData, task.Mappings, task.EntityType);

                        // 更新任务状态
                        task.Status = task.Result.FailedCount == 0 ? TaskStatus.Success : TaskStatus.Failed;

                        // 显示任务执行结果
                        StringBuilder resultMessage = new StringBuilder();
                        resultMessage.AppendLine($"任务 {task.TaskName} 执行完成！");
                        resultMessage.AppendLine($"总记录数：{task.Result.TotalCount}");
                        resultMessage.AppendLine($"成功记录数：{task.Result.SuccessCount}");
                        resultMessage.AppendLine($"失败记录数：{task.Result.FailedCount}");
                        resultMessage.AppendLine($"新增记录数：{task.Result.InsertedCount}");
                        resultMessage.AppendLine($"更新记录数：{task.Result.UpdatedCount}");
                        resultMessage.AppendLine($"耗时：{task.Result.ElapsedMilliseconds} 毫秒");

                        MessageBox.Show(resultMessage.ToString(), "任务执行结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        task.Status = TaskStatus.Failed;
                        MessageBox.Show($"任务 {task.TaskName} 执行失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // 显示所有任务执行完成
                StringBuilder finalMessage = new StringBuilder();
                finalMessage.AppendLine("所有导入任务执行完成！\n");

                int successCount = _importTasks.Count(t => t.Status == TaskStatus.Success);
                int failedCount = _importTasks.Count(t => t.Status == TaskStatus.Failed);

                finalMessage.AppendLine($"成功任务数：{successCount}");
                finalMessage.AppendLine($"失败任务数：{failedCount}");
                finalMessage.AppendLine($"总任务数：{_importTasks.Count}");

                MessageBox.Show(finalMessage.ToString(), "执行完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 清空任务列表
                _importTasks.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行任务失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// 导入任务类
    /// 用于管理多步骤导入的单个任务
    /// </summary>
    public class ImportTask
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Excel文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 工作表名称
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 目标实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 列映射配置
        /// </summary>
        public ColumnMappingCollection Mappings { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// 导入结果
        /// </summary>
        public DynamicImporter.ImportResult Result { get; set; }
    }

    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 待执行
        /// </summary>
        Pending,
        /// <summary>
        /// 执行中
        /// </summary>
        Running,
        /// <summary>
        /// 执行成功
        /// </summary>
        Success,
        /// <summary>
        /// 执行失败
        /// </summary>
        Failed
    }
}