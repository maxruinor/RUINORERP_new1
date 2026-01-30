using Krypton.Navigator;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.UI.SysConfig.BasicDataImport;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using RUINORERP.Business.BizMapperService;

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
        private ImageProcessor _imageProcessor;
        private IEntityMappingService _entityInfoService;

        // 动态导入相关字段
        private DynamicExcelParser _dynamicExcelParser;
        private ColumnMappingManager _columnMappingManager;
        private DynamicImporter _dynamicImporter;
        private DynamicDataValidator _dynamicDataValidator;
        private DataDeduplicationService _deduplicationService;
        private IForeignKeyService _foreignKeyService;
        private ImportConfiguration _currentConfig;
        private DataTable _rawExcelData;           // 原始Excel数据（预览用）
        private DataTable _parsedImportData;         // 根据映射配置解析后的数据
        private Type _selectedEntityType;

        /// <summary>
        /// 实体类型映射字典
        /// 存储中文描述到实体类型的映射关系
        /// </summary>
        public static Dictionary<string, Type> EntityTypeMappings { get; private set; }

        /// <summary>
        /// 静态构造函数
        /// 初始化实体类型映射关系
        /// </summary>
        static UCBasicDataImport()
        {
            EntityTypeMappings = new Dictionary<string, Type>
            {
                { "供应商表", typeof(tb_CustomerVendor) },
                { "客户表", typeof(tb_CustomerVendor) },
                { "产品类目表", typeof(tb_ProdCategories) },
                { "产品基本信息表", typeof(tb_Prod) },
                { "产品详情信息表", typeof(tb_ProdDetail) },
                { "产品属性表", typeof(tb_ProdProperty) },
                { "产品属性值表", typeof(tb_ProdPropertyValue) },
                { "库位表", typeof(tb_Location) },
                { "货架表", typeof(tb_StorageRack) },
                { "单位表", typeof(tb_Unit) },
                { "产品类型表", typeof(tb_ProductType) },
                { "部门表", typeof(tb_Department) },
            };
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCBasicDataImport()
        {
            InitializeComponent();
            _entityInfoService = Startup.GetFromFac<IEntityMappingService>();
            InitializeData();
            InitializeDynamicImport();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitializeData()
        {

            // 初始化数据网格视图
            dgvImportData.AutoGenerateColumns = true;
            //dgvImportData.DataSource = _importData;

            // 设置图片保存路径
            string imageSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");
            _imageProcessor = new ImageProcessor(imageSavePath);

            // 初始化其他组件
            _excelParser = new ExcelDataParser();
        }

        /// <summary>
        /// 初始化动态导入功能
        /// </summary>
        private void InitializeDynamicImport()
        {
            // 初始化数据库连接
            LoadDbConnection();

            // 初始化外键服务（单例模式，整个导入流程共享）
            _foreignKeyService = new ForeignKeyService(_db);

            // 初始化动态导入组件
            _dynamicExcelParser = new DynamicExcelParser();
            _columnMappingManager = new ColumnMappingManager();
            _dynamicDataValidator = new DynamicDataValidator(_foreignKeyService);
            _deduplicationService = new DataDeduplicationService();
            _currentConfig = new ImportConfiguration();
            _rawExcelData = new DataTable();
            _parsedImportData = new DataTable();

            // 初始化数据网格视图
            dgvRawExcelData.AutoGenerateColumns = true;
            dgvRawExcelData.DataSource = _rawExcelData;
            dgvRawExcelData.UseCustomColumnDisplay = false;
            dgvRawExcelData.CellFormatting += DgvRawExcelData_CellFormatting;

            dgvParsedImportData.AutoGenerateColumns = true;
            dgvParsedImportData.DataSource = _parsedImportData;
            dgvParsedImportData.UseCustomColumnDisplay = false;
            dgvParsedImportData.UseSelectedColumn = true;
            dgvParsedImportData.CellFormatting += DgvParsedImportData_CellFormatting;
            // 初始化实体类型选择下拉框
            InitializeEntityTypes();

            // 加载映射配置列表
            LoadMappingConfigs();

            // 绑定事件
            kbtnDynamicBrowse.Click += KbtnDynamicBrowse_Click;
            kbtnDynamicParse.Click += KbtnDynamicParse_Click;

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

                // 从EntityTypeMappings字典中获取所有实体类型的中文描述
                foreach (var mapping in EntityTypeMappings)
                {
                    kcmbDynamicEntityType.Items.Add(mapping.Key);
                }

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
                if (EntityTypeMappings.ContainsKey(selectedText))
                {
                    _selectedEntityType = EntityTypeMappings[selectedText];
                }
                else
                {
                    _selectedEntityType = null;
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
                    _currentConfig = new ImportConfiguration();
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

                // 加载配置（传递实体类型以加载SugarColumn）
                _currentConfig = _columnMappingManager.LoadConfiguration(selectedConfig, _selectedEntityType);

                // 显示映射配置对话框
                using (var frmMapping = new frmColumnMappingConfig())
                {
                    frmMapping.ExcelData = _rawExcelData;
                    frmMapping.TargetEntityType = _selectedEntityType;
                    frmMapping.ImportConfig = _currentConfig;

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
                        _currentConfig = frmMapping.ImportConfig;

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
                        _currentConfig = new ImportConfiguration();
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
            bool hasMappingConfig = _currentConfig != null && _currentConfig.ColumnMappings != null && _currentConfig.ColumnMappings.Count > 0;
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
                // 保存当前选择的映射名称
                string currentMappingName = kcmbDynamicMappingName.SelectedIndex > 0
                    ? kcmbDynamicMappingName.SelectedItem.ToString()
                    : null;

                var mappingNames = _columnMappingManager.GetAllMappingNames();
                kcmbDynamicMappingName.Items.Clear();
                kcmbDynamicMappingName.Items.Add("请选择");

                string entityTypeName = _selectedEntityType?.Name ?? "";
                int selectedIndex = 0; // 默认选中"请选择"

                int index = 1; // 从索引1开始（索引0是"请选择"）
                foreach (var name in mappingNames)
                {
                    try
                    {
                        // 尝试加载映射配置以检查实体类型
                        var config = _columnMappingManager.LoadConfiguration(name);
                        if (config?.EntityType == entityTypeName || string.IsNullOrEmpty(entityTypeName))
                        {
                            kcmbDynamicMappingName.Items.Add(name);

                            // 如果这个名称与之前选择的名称相同，记录索引
                            if (currentMappingName != null && name == currentMappingName)
                            {
                                selectedIndex = index;
                            }

                            index++;
                        }
                    }
                    catch
                    {
                        // 忽略加载失败的配置文件
                    }
                }

                // 恢复之前的选择（如果还在列表中）
                kcmbDynamicMappingName.SelectedIndex = selectedIndex;
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
            if (_db == null)
            {
                _db = MainForm.Instance.AppContext.Db;
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
                if (_currentConfig == null || _currentConfig.ColumnMappings == null || _currentConfig.ColumnMappings.Count == 0)
                {
                    MessageBox.Show("请先配置列映射关系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 读取全部数据（不再限制行数）
                DataTable fullData = _dynamicExcelParser.ParseExcelToDataTable(
                    ktxtDynamicFilePath.Text,
                    kcmbDynamicSheetName.SelectedIndex,
                    0); // 0表示读取全部数据

                //
                // 根据映射配置转换数据
                _parsedImportData = ApplyColumnMapping(fullData, _currentConfig.ColumnMappings);

                // 应用去重复逻辑（如果配置了去重）
                if (_currentConfig.EnableDeduplication)
                {
                    var deduplicationResult = _deduplicationService.Deduplicate(_parsedImportData, _currentConfig);

                    // 使用去重后的数据替换原始数据
                    _parsedImportData = deduplicationResult.DeduplicatedData;

                    if (deduplicationResult.DuplicateCount > 0)
                    {
                        MainForm.Instance.ShowStatusText($"根据映射配置解析完成，原始数据 {deduplicationResult.OriginalCount} 行，去重后 {_parsedImportData.Rows.Count} 行（移除 {deduplicationResult.DuplicateCount} 行重复数据）");
                    }
                    else
                    {
                        MainForm.Instance.ShowStatusText($"根据映射配置解析完成，共 {_parsedImportData.Rows.Count} 行数据（无重复数据）");
                    }
                }
                else
                {
                    MainForm.Instance.ShowStatusText($"根据映射配置解析完成，共 {_parsedImportData.Rows.Count} 行数据");
                }

                // 绑定到解析后数据表格
                dgvParsedImportData.DataSource = _parsedImportData;

                // 切换到解析数据预览页面
                kryptonNavigatorDynamic.SelectedPage = kryptonPageParsedData;

                // 启用导入按钮
                kbtnDynamicImport.Enabled = true;
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
        private DataTable ApplyColumnMapping(DataTable sourceData, List<ColumnMapping> mappings)
        {
            DataTable result = new DataTable(_currentConfig?.EntityType ?? "ImportData");

            try
            {
                // 用于跟踪已添加的列，避免重复添加
                HashSet<string> addedColumns = new HashSet<string>();

                // 创建结果表结构（使用SystemField.Value作为列名）显示给用户看的
                foreach (var mapping in mappings)
                {
                    string columnName = mapping.SystemField?.Value;

                    // 避免重复添加列
                    if (!string.IsNullOrEmpty(columnName) && !addedColumns.Contains(columnName))
                    {
                        result.Columns.Add(columnName, typeof(string));
                        addedColumns.Add(columnName);
                    }

                    // 对于外键关联类型，需要额外添加外键来源列到解析结果中
                    // 这样在导入时才能从解析后的数据中获取外键参考值
                    if (mapping.DataSourceType == DataSourceType.ForeignKey &&
                        mapping.ForeignConfig != null &&
                        mapping.ForeignConfig.ForeignKeySourceColumn != null &&
                        !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn.ExcelColumnName))
                    {
                        string sourceColumnName = mapping.ForeignConfig.ForeignKeySourceColumn.ExcelColumnName;

                        // 检查源数据中是否包含该列
                        if (sourceData.Columns.Contains(sourceColumnName) && !addedColumns.Contains(sourceColumnName))
                        {
                            // 添加外键来源列到结果表
                            result.Columns.Add(sourceColumnName, typeof(string));
                            addedColumns.Add(sourceColumnName);
                        }
                    }
                }


                // 转换数据行
                foreach (DataRow sourceRow in sourceData.Rows)
                {
                    DataRow targetRow = result.NewRow();

                    foreach (var mapping in mappings)
                    {
                        // 根据数据来源类型处理1
                        switch (mapping.DataSourceType)
                        {
                            case DataSourceType.Excel:
                                // Excel数据源
                                if (sourceData.Columns.Contains(mapping.ExcelColumn))
                                {
                                    object cellValue = sourceRow[mapping.ExcelColumn];

                                    // 检查是否为空值
                                    bool isEmpty = cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue?.ToString());

                                    // 如果配置了忽略空值且值为空，则不处理该字段
                                    if (mapping.IgnoreEmptyValue && isEmpty)
                                    {
                                        targetRow[mapping.SystemField?.Value] = DBNull.Value;
                                    }
                                    else
                                    {
                                        // 如果是图片列，直接使用Excel中的值（图片路径）
                                        if (mapping.IsImageColumn)
                                        {
                                            targetRow[mapping.SystemField?.Value] = cellValue?.ToString() ?? "";
                                        }
                                        else
                                        {
                                            targetRow[mapping.SystemField?.Value] = cellValue?.ToString() ?? "";
                                        }
                                    }
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Value] = "";
                                }
                                break;

                            case DataSourceType.SystemGenerated:
                                // 系统生成的值，暂时留空或使用特殊标记
                                targetRow[mapping.SystemField?.Value] = "[系统生成]";
                                break;

                            case DataSourceType.DefaultValue:
                                // 默认值映射
                                string defaultValue = mapping.DefaultValue ?? "";
                                // 检查是否需要转换为枚举值
                                Type enumType = EntityImportHelper.GetPredefinedEnumType(_currentConfig?.EntityType ?? "", mapping.SystemField?.Key ?? "");
                                if (enumType != null && !string.IsNullOrEmpty(defaultValue))
                                {
                                    // 解析枚举名称并转换为int
                                    try
                                    {
                                        defaultValue = Convert.ChangeType(Enum.Parse(enumType, defaultValue), typeof(int)).ToString();
                                    }
                                    catch
                                    {
                                        // 解析失败保持原值
                                    }
                                }
                                targetRow[mapping.SystemField?.Value] = defaultValue;
                                break;

                            case DataSourceType.ForeignKey:
                                // 外键关联
                                // 在导入时需要通过关联表查询获取值
                                // 显示外键来源列的值和关联信息
                                string foreignKeySourceValue = "";
                                string sourceColumn = mapping.ForeignConfig?.ForeignKeySourceColumn?.ExcelColumnName ?? mapping.ExcelColumn;
                                // DisplayName = 显示名称
                                string sourceColumnDisplay = mapping.ForeignConfig?.ForeignKeySourceColumn?.DisplayName ?? sourceColumn;

                                if (!string.IsNullOrEmpty(sourceColumn) &&
                                    !sourceColumn.StartsWith("[") &&
                                    sourceData.Columns.Contains(sourceColumn))
                                {
                                    foreignKeySourceValue = sourceRow[sourceColumn]?.ToString() ?? "";

                                    // 将外键来源列的值复制到结果表中
                                    // 这样在后续导入时，可以从解析后的数据中获取参考值
                                    if (result.Columns.Contains(sourceColumn))
                                    {
                                        targetRow[sourceColumn] = foreignKeySourceValue;
                                    }
                                }

                                if (!string.IsNullOrEmpty(foreignKeySourceValue))
                                {
                                    targetRow[mapping.SystemField?.Value] = $"[通过关联外键:{sourceColumnDisplay}:{foreignKeySourceValue}->找{mapping.ForeignConfig?.ForeignKeyTable?.Value}.{mapping.ForeignConfig?.ForeignKeyField?.Value}]";
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Value] = $"[外键关联:{mapping.ForeignConfig?.ForeignKeyTable?.Value}.{mapping.ForeignConfig?.ForeignKeyField?.Value}]";
                                }
                                break;

                            case DataSourceType.SelfReference:
                                // 自身字段引用
                                // 在导入时需要通过已导入的数据获取值
                                // 这里暂时使用占位符，实际处理在DynamicImporter中完成
                                targetRow[mapping.SystemField?.Value] = $"[自身引用:{mapping.SelfReferenceField?.Value}]";
                                break;

                            case DataSourceType.FieldCopy:
                                // 字段复制
                                // 复制同一记录中另一个字段的值
                                if (!string.IsNullOrEmpty(mapping.CopyFromField?.Key))
                                {
                                    // 获取被复制字段的映射配置
                                    var copyFromMapping = mappings.FirstOrDefault(m => m.SystemField?.Key == mapping.CopyFromField?.Key);

                                    if (copyFromMapping != null)
                                    {
                                        // 优先从已处理的 targetRow 中读取（如果该字段已被处理）
                                        if (targetRow.Table.Columns.Contains(copyFromMapping.SystemField?.Value) &&
                                            targetRow[copyFromMapping.SystemField?.Value] != DBNull.Value &&
                                            !string.IsNullOrEmpty(targetRow[copyFromMapping.SystemField?.Value]?.ToString()))
                                        {
                                            object copiedValue = targetRow[copyFromMapping.SystemField?.Value];
                                            targetRow[mapping.SystemField?.Value] = copiedValue?.ToString() ?? "";
                                        }
                                        // 如果 targetRow 中还没有该值，尝试从 sourceRow 的 Excel 列中读取
                                        else if (!string.IsNullOrEmpty(copyFromMapping.ExcelColumn) &&
                                                sourceData.Columns.Contains(copyFromMapping.ExcelColumn))
                                        {
                                            object copiedValue = sourceRow[copyFromMapping.ExcelColumn];
                                            targetRow[mapping.SystemField?.Value] = copiedValue?.ToString() ?? "";
                                        }
                                        else
                                        {
                                            targetRow[mapping.SystemField?.Value] = "[字段复制:源数据为空]";
                                        }
                                    }
                                    else
                                    {
                                        targetRow[mapping.SystemField?.Value] = "[字段复制:找不到源字段]";
                                    }
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Value] = "[字段复制:未设置]";
                                }
                                break;

                            case DataSourceType.ColumnConcat:
                                // 列拼接
                                // 将Excel中的多个列值拼接后赋值给目标字段
                                if (mapping.ConcatConfig != null &&
                                    mapping.ConcatConfig.SourceColumns != null &&
                                    mapping.ConcatConfig.SourceColumns.Count >= 2)
                                {
                                    var concatValues = new List<string>();

                                    foreach (var sourceCol in mapping.ConcatConfig.SourceColumns)
                                    {
                                        if (sourceData.Columns.Contains(sourceCol))
                                        {
                                            object cellValue = sourceRow[sourceCol];
                                            string valueStr = cellValue?.ToString() ?? "";

                                            // 去除前后空格
                                            if (mapping.ConcatConfig.TrimWhitespace)
                                            {
                                                valueStr = valueStr.Trim();
                                            }

                                            // 如果配置了忽略空值，跳过空列
                                            if (mapping.ConcatConfig.IgnoreEmptyColumns &&
                                                string.IsNullOrEmpty(valueStr))
                                            {
                                                continue;
                                            }

                                            concatValues.Add(valueStr);
                                        }
                                        else
                                        {
                                            // 列不存在时，根据配置决定是否继续
                                            if (!mapping.ConcatConfig.IgnoreEmptyColumns)
                                            {
                                                concatValues.Add("");
                                            }
                                        }
                                    }

                                    // 拼接所有值
                                    string concatenatedValue = string.Join(
                                        mapping.ConcatConfig.Separator ?? "",
                                        concatValues);

                                    targetRow[mapping.SystemField?.Value] = concatenatedValue;
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Value] = "[列拼接:配置无效]";
                                }
                                break;
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
                MainForm.Instance.ShowStatusText($"已加载工作表数据，预览显示前 {_rawExcelData.Rows.Count} 行数据");
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
                    frmMapping.ImportConfig = _currentConfig;
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
                        _currentConfig = frmMapping.ImportConfig;

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
                _currentConfig = _columnMappingManager.LoadConfiguration(mappingName, _selectedEntityType);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行动态导入
        /// </summary>
        private async Task ExecuteDynamicImport()
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
                if (_currentConfig == null || _currentConfig.ColumnMappings == null || _currentConfig.ColumnMappings.Count == 0)
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

                // 初始化DynamicImporter
                _dynamicImporter = new DynamicImporter(_db, _entityInfoService);

                // 直接执行导入
                await ExecuteSingleImport();
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
        /// 执行单个导入任务
        /// </summary>
        private async Task ExecuteSingleImport()
        {
            try
            {
                // 直接使用解析后已经去重好的数据，不再重新读取Excel
                // _parsedImportData 已经在解析时完成了映射和去重处理

                DataTable fullParsedData = _parsedImportData;

                // 检查是否有勾选的行
                var selectedRows = GetSelectedRows();
                if (selectedRows.Rows.Count == 0)
                {
                    if (fullParsedData.Rows.Count > 0)
                    {
                        MessageBox.Show("请先勾选需要导入的数据行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("没有可导入的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // 使用勾选的行数据
                DataTable importData = selectedRows;

                // 唯一性检查（在数据验证之前执行）
                var duplicateErrors = CheckUniqueValues(importData);
                if (duplicateErrors.Count > 0)
                {
                    string duplicateSummary = $"发现 {duplicateErrors.Count} 条数据与数据库中已存在的数据重复：\n\n";

                    // 只显示前10个重复记录
                    int displayCount = Math.Min(10, duplicateErrors.Count);
                    for (int i = 0; i < displayCount; i++)
                    {
                        var error = duplicateErrors[i];
                        duplicateSummary += $"行 {error.RowNumber} - {error.FieldName}: {error.ErrorMessage}\n";
                    }

                    if (duplicateErrors.Count > displayCount)
                    {
                        duplicateSummary += $"\n... 还有 {duplicateErrors.Count - displayCount} 条重复记录未显示";
                    }

                    duplicateSummary += "\n\n是否继续导入（跳过重复的记录）？";

                    if (MessageBox.Show(duplicateSummary, "唯一性检查警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        return;
                    }

                    // 移除重复的记录
                    var rowsToRemove = duplicateErrors.Select(e => e.RowNumber).Distinct().ToList();
                    for (int i = importData.Rows.Count - 1; i >= 0; i--)
                    {
                        // Excel行号从1开始，转换为DataTable索引（从0开始）
                        int rowNumber = i + 1;
                        if (rowsToRemove.Contains(rowNumber))
                        {
                            importData.Rows.RemoveAt(i);
                        }
                    }
                }

                // 数据验证
                var validationErrors = _dynamicDataValidator.Validate(importData, new ColumnMappingCollection(_currentConfig?.ColumnMappings ?? new List<ColumnMapping>()), _selectedEntityType);
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
                if (importData.Rows.Count == 0)
                {
                    // 显示确认对话框
                    MessageBox.Show($"导入数据为： {importData.Rows.Count} 条数据到 {_selectedEntityType.Name}！", "导入取消", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                // 开始导入
                kbtnDynamicImport.Enabled = false;
                kbtnDynamicBrowse.Enabled = false;
                kbtnDynamicParse.Enabled = false;
                kbtnDynamicMap.Enabled = false;

                Application.DoEvents();

                // 初始化导入器，传入共享的ForeignKeyService实例
                _dynamicImporter = new DynamicImporter(_db, _entityInfoService, _foreignKeyService);

                // 获取导入类型标识（用于区分客户和供应商等使用相同表的情况）
                string importType = GetImportType();

                // 执行导入（异步）
                var importResult = await _dynamicImporter.ImportAsync(importData, new ColumnMappingCollection(_currentConfig?.ColumnMappings ?? new List<ColumnMapping>()), _selectedEntityType, importType);

                // 显示导入结果
                DisplayImportResult(importResult);

                // 重置状态
                kbtnDynamicImport.Enabled = true;
                kbtnDynamicBrowse.Enabled = true;
                kbtnDynamicParse.Enabled = kcmbDynamicSheetName.SelectedIndex >= 0;
                kbtnDynamicMap.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入数据失败，请检查数据格式和映射配置。{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                kbtnDynamicImport.Enabled = true;
                kbtnDynamicBrowse.Enabled = true;
                kbtnDynamicParse.Enabled = true;
                kbtnDynamicMap.Enabled = true;
            }
        }

        /// <summary>
        /// 获取导入类型标识
        /// 根据用户选择的实体类型返回对应的导入类型标识
        /// 用于区分客户和供应商等使用相同表的情况
        /// </summary>
        /// <returns>导入类型标识</returns>
        private string GetImportType()
        {
            if (kcmbDynamicEntityType.SelectedIndex <= 0)
            {
                return null;
            }

            string selectedText = kcmbDynamicEntityType.SelectedItem.ToString();
            return selectedText;
        }

        /// <summary>
        /// 检查唯一性字段值是否已存在于数据库中
        /// </summary>
        /// <param name="dataTable">要检查的数据表</param>
        /// <returns>重复记录错误列表</returns>
        private List<ValidationError> CheckUniqueValues(DataTable dataTable)
        {
            var duplicateErrors = new List<ValidationError>();

            // 获取配置了唯一性的字段
            var uniqueMappings = (_currentConfig?.ColumnMappings ?? new List<ColumnMapping>()).Where(m => m.IsUniqueValue).ToList();
            if (uniqueMappings.Count == 0)
            {
                return duplicateErrors;
            }

            try
            {
                // 对每个唯一性字段进行检查
                foreach (var uniqueMapping in uniqueMappings)
                {
                    string fieldName = uniqueMapping.SystemField?.Key;
                    if (!dataTable.Columns.Contains(fieldName))
                    {
                        continue;
                    }

                    // 从数据库查询已存在的唯一性字段值
                    var existingValues = QueryExistingUniqueValues(fieldName);

                    if (existingValues == null || existingValues.Count == 0)
                    {
                        continue;
                    }

                    // 检查导入数据中是否包含已存在的值
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow row = dataTable.Rows[i];
                        object value = row[fieldName];

                        // 跳过空值
                        if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value?.ToString()))
                        {
                            continue;
                        }

                        // 检查值是否已存在
                        string valueStr = value.ToString().Trim();
                        if (existingValues.Contains(valueStr, StringComparer.OrdinalIgnoreCase))
                        {
                            duplicateErrors.Add(new ValidationError
                            {
                                RowNumber = i + 1, // Excel行号从1开始
                                FieldName = fieldName,
                                ErrorMessage = $"值【{valueStr}】已存在于数据库中，不能重复"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"唯一性检查失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return duplicateErrors;
        }

        /// <summary>
        /// 从数据库查询已存在的唯一性字段值
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns>已存在的值集合</returns>
        private HashSet<string> QueryExistingUniqueValues(string fieldName)
        {
            var existingValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                // 使用反射获取实体类型的主键字段名
                string tableName = _selectedEntityType.Name;

                // 构建SQL查询语句
                string sql = $"SELECT DISTINCT [{fieldName}] FROM [{tableName}] WHERE [{fieldName}] IS NOT NULL AND [{fieldName}] <> ''";

                // 执行查询
                var results = _db.Ado.SqlQuery<string>(sql);

                // 添加到HashSet中（自动去重）
                foreach (var value in results)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        existingValues.Add(value.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowStatusText($"查询唯一性字段值失败: {ex.Message}");
            }

            return existingValues;
        }

        /// <summary>
        /// 获取勾选的数据行
        /// </summary>
        /// <returns>包含勾选行的新数据表格</returns>
        private DataTable GetSelectedRows()
        {
            DataTable selectedData = _parsedImportData.Clone();

            // 遍历DataGridView的每一行
            foreach (DataGridViewRow dgvRow in dgvParsedImportData.Rows)
            {
                // 检查是否为数据行（非新增行）
                if (dgvRow.IsNewRow)
                {
                    continue;
                }

                // 检查是否勾选
                if (dgvParsedImportData.Columns.Contains("Selected") &&
                    dgvRow.Cells["Selected"] != null &&
                    dgvRow.Cells["Selected"].Value != null &&
                    dgvRow.Cells["Selected"].Value is bool &&
                    (bool)dgvRow.Cells["Selected"].Value)
                {
                    // 从DataTable中获取对应的行并导入
                    int rowIndex = dgvRow.Index;
                    if (rowIndex >= 0 && rowIndex < _parsedImportData.Rows.Count)
                    {
                        selectedData.ImportRow(_parsedImportData.Rows[rowIndex]);
                    }
                }
            }

            return selectedData;
        }

        /// <summary>
        /// 显示导入结果
        /// </summary>
        /// <param name="result">导入结果</param>
        private void DisplayImportResult(DynamicImporter.ImportResult result)
        {
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
        }

        /// <summary>
        /// 根据实体类型获取对应的Controller实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>Controller实例，如果未找到则返回null</returns>
        private object GetControllerForEntityType(Type entityType)
        {
            try
            {
                // 简化实现：只返回null，让调用方回退到直接数据库导入
                // 在实际使用中，用户需要手动在调用代码中获取正确的Controller实例
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 动态导入-导入数据按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void kbtnDynamicImport_Click(object sender, EventArgs e)
        {
            try
            {
                //导入前，通过当前配置找到这个配置中的列的情况，找到有外键关联的情况，再去找到具体关联的哪个表。
                //再先从数据库中查找到结果集合。缓存起来。用于 外键服务类和验证服务类。
                PreloadForeignKeyData();

                // 执行动态导入（异步）
                await ExecuteDynamicImport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 预加载外键数据
        /// 在导入前批量查询所有关联表数据，缓存起来用于外键服务类和验证服务类
        /// 使用共享的ForeignKeyService实例，确保缓存复用
        /// </summary>
        private void PreloadForeignKeyData()
        {
            try
            {
                if (_currentConfig == null || _currentConfig.ColumnMappings == null)
                {
                    return;
                }

                // 筛选出所有外键关联的映射
                var foreignKeyMappings = _currentConfig.ColumnMappings
                    .Where(m => m.DataSourceType == DataSourceType.ForeignKey)
                    .ToList();

                if (!foreignKeyMappings.Any())
                {
                    return;
                }

                // 使用共享的ForeignKeyService实例预加载外键数据
                // 确保验证和导入流程使用同一个缓存
                _foreignKeyService.PreloadForeignKeyData(_currentConfig.ColumnMappings);

                // 记录预加载成功的信息
                MainForm.Instance.ShowStatusText($"成功预加载 {foreignKeyMappings.Count} 个外键关联表的数据");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowStatusText($"预加载外键数据失败: {ex.Message}");
                // 记录错误但不阻止导入
            }
        }

        /// <summary>
        /// 原始Excel数据预览表格的单元格格式化事件
        /// 用于显示图片列的图片
        /// </summary>
        private void DgvRawExcelData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // 如果没有当前配置或映射，不处理
                if (_currentConfig?.ColumnMappings == null || _currentConfig.ColumnMappings.Count == 0)
                {
                    return;
                }

                // 检查当前列是否配置为图片列
                string columnName = dgvRawExcelData.Columns[e.ColumnIndex]?.Name;
                if (string.IsNullOrEmpty(columnName))
                {
                    return;
                }

                // 根据Excel列名查找映射配置
                var imageMapping = _currentConfig.ColumnMappings.FirstOrDefault(m =>
                    m.ExcelColumn.Equals(columnName, StringComparison.OrdinalIgnoreCase) &&
                    m.IsImageColumn);

                if (imageMapping != null && e.Value != null)
                {
                    string cellValue = e.Value.ToString();

                    // 如果是图片路径，尝试加载并显示图片
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        if (imageMapping.ImageColumnType == ImageColumnType.Path)
                        {
                            // 图片路径类型：尝试加载图片
                            try
                            {
                                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages", cellValue);
                                if (File.Exists(imagePath))
                                {
                                    Image img = Image.FromFile(imagePath);
                                    // 缩放图片以适应单元格
                                    int maxSize = Math.Min(dgvRawExcelData.Rows[e.RowIndex].Height - 4, 80);
                                    Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                    e.Value = scaledImg;
                                    img.Dispose();
                                }
                            }
                            catch
                            {
                                // 图片加载失败，显示文本
                                e.Value = cellValue;
                            }
                        }
                        else if (imageMapping.ImageColumnType == ImageColumnType.Binary)
                        {
                            // 二进制图片类型：如果值是Base64编码，尝试解码并显示
                            try
                            {
                                if (cellValue.Length > 0 && !cellValue.Contains(Path.DirectorySeparatorChar))
                                {
                                    // 尝试作为Base64解码
                                    byte[] imageBytes = Convert.FromBase64String(cellValue);
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        Image img = Image.FromStream(ms);
                                        int maxSize = Math.Min(dgvRawExcelData.Rows[e.RowIndex].Height - 4, 80);
                                        Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                        e.Value = scaledImg;
                                    }
                                }
                                else
                                {
                                    // 如果是路径，尝试加载
                                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages", cellValue);
                                    if (File.Exists(imagePath))
                                    {
                                        Image img = Image.FromFile(imagePath);
                                        int maxSize = Math.Min(dgvRawExcelData.Rows[e.RowIndex].Height - 4, 80);
                                        Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                        e.Value = scaledImg;
                                        img.Dispose();
                                    }
                                }
                            }
                            catch
                            {
                                // 二进制数据处理失败，显示文本
                                e.Value = cellValue;
                            }
                        }
                    }
                }
            }
            catch
            {
                // 格式化失败不影响其他单元格
            }
        }

        /// <summary>
        /// 解析后数据预览表格的单元格格式化事件
        /// 用于显示图片列的图片
        /// </summary>
        private void DgvParsedImportData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // 如果没有当前配置或映射，不处理
                if (_currentConfig?.ColumnMappings == null || _currentConfig.ColumnMappings.Count == 0)
                {
                    return;
                }

                // 检查当前列是否配置为图片列
                string columnName = dgvParsedImportData.Columns[e.ColumnIndex]?.Name;
                if (string.IsNullOrEmpty(columnName))
                {
                    return;
                }

                // 根据系统字段名查找映射配置
                var imageMapping = _currentConfig.ColumnMappings.FirstOrDefault(m =>
                    m.SystemField?.Value.Equals(columnName, StringComparison.OrdinalIgnoreCase) == true &&
                    m.IsImageColumn);

                if (imageMapping != null && e.Value != null)
                {
                    string cellValue = e.Value.ToString();

                    // 如果是图片路径，尝试加载并显示图片
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        if (imageMapping.ImageColumnType == ImageColumnType.Path)
                        {
                            // 图片路径类型：尝试加载图片
                            try
                            {
                                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages", cellValue);
                                if (File.Exists(imagePath))
                                {
                                    Image img = Image.FromFile(imagePath);
                                    // 缩放图片以适应单元格
                                    int maxSize = Math.Min(dgvParsedImportData.Rows[e.RowIndex].Height - 4, 80);
                                    Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                    e.Value = scaledImg;
                                    img.Dispose();
                                }
                            }
                            catch
                            {
                                // 图片加载失败，显示文本
                                e.Value = cellValue;
                            }
                        }
                        else if (imageMapping.ImageColumnType == ImageColumnType.Binary)
                        {
                            // 二进制图片类型：如果值是Base64编码，尝试解码并显示
                            try
                            {
                                if (cellValue.Length > 0 && !cellValue.Contains(Path.DirectorySeparatorChar))
                                {
                                    // 尝试作为Base64解码
                                    byte[] imageBytes = Convert.FromBase64String(cellValue);
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        Image img = Image.FromStream(ms);
                                        int maxSize = Math.Min(dgvParsedImportData.Rows[e.RowIndex].Height - 4, 80);
                                        Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                        e.Value = scaledImg;
                                    }
                                }
                                else
                                {
                                    // 如果是路径，尝试加载
                                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages", cellValue);
                                    if (File.Exists(imagePath))
                                    {
                                        Image img = Image.FromFile(imagePath);
                                        int maxSize = Math.Min(dgvParsedImportData.Rows[e.RowIndex].Height - 4, 80);
                                        Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                        e.Value = scaledImg;
                                        img.Dispose();
                                    }
                                }
                            }
                            catch
                            {
                                // 二进制数据处理失败，显示文本
                                e.Value = cellValue;
                            }
                        }
                    }
                }
            }
            catch
            {
                // 格式化失败不影响其他单元格
            }
        }

    }
}
