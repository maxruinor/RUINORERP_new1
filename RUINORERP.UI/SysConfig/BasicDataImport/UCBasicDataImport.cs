using Krypton.Navigator;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;  // NewSumDataGridView控件
using RUINORERP.Common;
using RUINORERP.UI.SysConfig.BasicDataImport;
using RUINORERP.Repository.UnitOfWorks;  // ✅ 新增：IUnitOfWorkManage
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Model.ImportEngine.Enums;

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
        private IUnitOfWorkManage _unitOfWorkManage;  // ✅ 新增：统一事务管理器
        private ImageProcessor _imageProcessor;

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
        private DataTable _finalPreviewData;         // 最终预览数据（包含所有可预生成的值）
        private Type _selectedEntityType;

        #region 图片缓存管理
        // 【内存优化】统一的图片缓存，防止重复加载同一图片
        private readonly Dictionary<string, Image> _imageCache = new Dictionary<string, Image>();
        private readonly object _imageCacheLock = new object();
        private const int MaxImageCacheSize = 100;

        /// <summary>
        /// 注册图片到缓存（线程安全）
        /// </summary>
        private void RegisterImageCache(Image image, string key)
        {
            if (image == null || string.IsNullOrEmpty(key)) return;

            lock (_imageCacheLock)
            {
                // 如果缓存已满，清理最旧的图片
                if (_imageCache.Count >= MaxImageCacheSize)
                {
                    var keysToRemove = _imageCache.Keys.Take(10).ToList();
                    foreach (var k in keysToRemove)
                    {
                        _imageCache[k]?.Dispose();
                        _imageCache.Remove(k);
                    }
                }

                // 避免重复添加相同键
                if (!_imageCache.ContainsKey(key))
                {
                    _imageCache[key] = image;
                }
            }
        }

        /// <summary>
        /// 清理所有缓存的图片资源
        /// </summary>
        private void ClearImageCache()
        {
            lock (_imageCacheLock)
            {
                foreach (var img in _imageCache.Values)
                {
                    img?.Dispose();
                }
                _imageCache.Clear();
            }
        }
        #endregion

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
            _finalPreviewData = new DataTable();

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

            dgvFinalPreview.AutoGenerateColumns = true;
            dgvFinalPreview.DataSource = _finalPreviewData;
            dgvFinalPreview.UseCustomColumnDisplay = false;
            dgvFinalPreview.UseSelectedColumn = true;
            dgvFinalPreview.CellFormatting += DgvParsedImportData_CellFormatting;
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

                string selectedText = kcmbDynamicMappingName.SelectedItem.ToString();
                
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
                using (var frmMapping = new frmColumnMappingConfig(_selectedEntityType, _rawExcelData))
                {
                    frmMapping.ImportConfig = _currentConfig;
                    frmMapping.IsEditMode = true;
                    frmMapping.OriginalMappingName = selectedConfig;

                    // 订阅映射配置保存成功事件
                    frmMapping.MappingSaved += (s, e) =>
                    {
                        // 刷新映射配置下拉列表
                        LoadMappingConfigsForEntityType();

                        // 【修复】自动选中刚修改的配置 - 需要匹配带前缀的格式
                        if (!string.IsNullOrEmpty(frmMapping.SavedMappingName))
                        {
                            string configText = $"[配置] {frmMapping.SavedMappingName}";
                            int index = kcmbDynamicMappingName.FindStringExact(configText);
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

            // 普通模式：根据条件启用按钮
            
            // 映射配置按钮：已选择实体类型，且（已加载原始数据 OR 已选择映射配置）
            kbtnDynamicMap.Enabled = hasEntityType && (hasRawData || hasMappingSelected);

            // 解析按钮：需要原始数据、实体类型、映射配置都具备
            bool hasMappingConfig = _currentConfig != null && _currentConfig.ColumnMappings != null && _currentConfig.ColumnMappings.Count > 0;
            kbtnDynamicParse.Enabled = hasRawData && hasEntityType && hasMappingConfig;
            
            // 生成结果预览按钮：需要有解析后的数据
            if (kbtnGeneratePreview != null)
            {
                kbtnGeneratePreview.Enabled = _parsedImportData != null && _parsedImportData.Rows.Count > 0;
            }
            
            // 导入按钮：需要在解析后或生成预览后才能启用
            if (_parsedImportData != null && _parsedImportData.Rows.Count > 0)
            {
                // 有解析数据就可以导入，不需要强制点“生成结果预览”
                kbtnDynamicImport.Enabled = true;
            }
            else
            {
                kbtnDynamicImport.Enabled = false;
            }
            
            // 启用实体类型选择
            if (kcmbDynamicEntityType != null)
            {
                kcmbDynamicEntityType.Enabled = true;
            }
        }

        /// <summary>
        /// 加载映射配置列表
        /// </summary>
        private void LoadMappingConfigs()
        {
            // 单表模式：只加载单表配置
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
                kcmbDynamicMappingName.Items.Add("-- 请选择导入配置 --");

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
                            // 使用"[配置]"前缀标识单表配置
                            kcmbDynamicMappingName.Items.Add($"[配置] {name}");

                            // 如果这个名称与之前选择的名称相同，记录索引
                            if (currentMappingName != null && ($"[配置] {name}" == currentMappingName || name == currentMappingName))
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
                
                // ✅ 获取统一事务管理器
                try
                {
                    _unitOfWorkManage = MainForm.Instance.AppContext.GetRequiredService<IUnitOfWorkManage>();
                    System.Diagnostics.Debug.WriteLine("成功获取 IUnitOfWorkManage 实例");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"获取 IUnitOfWorkManage 失败: {ex.Message}");
                    _unitOfWorkManage = null;  // 降级方案：不使用事务管理器
                }
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
        /// 【功能说明】根据映射配置解析Excel数据，完成以下操作：
        /// 1. 读取Excel文件数据
        /// 2. 根据列映射配置转换数据（列名映射、类型转换、外键查询、默认值填充）
        /// 3. 执行数据去重（如果启用）
        /// 4. 在界面预览解析后的数据，供用户勾选需要导入的行
        /// 【注意】此步骤不写入数据库，仅做数据转换和预览
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

                // 性能优化：使用按需读取列的方法，只读取配置中映射的Excel列
                // 这样可以大幅减少内存占用和解析时间，特别是当Excel文件包含大量无关列时
                DataTable fullData = _dynamicExcelParser.ParseExcelWithColumns(
                    ktxtDynamicFilePath.Text,
                    kcmbDynamicSheetName.SelectedIndex,
                    _currentConfig.ColumnMappings,
                    -1); // -1表示读取全部数据行

                // 确保DynamicImporter已初始化
                if (_dynamicImporter == null)
                {
                    _dynamicImporter = new DynamicImporter(_db, _unitOfWorkManage, _foreignKeyService);
                }

                // 使用DynamicImporter的统一列映射方法（避免UI层重复实现）
                var mappings = new ColumnMappingCollection(_currentConfig.ColumnMappings);
                _parsedImportData = _dynamicImporter.ApplyColumnMapping(fullData, mappings, _selectedEntityType);

                // 应用去重复逻辑（如果配置了去重）
                if (_currentConfig.EnableDeduplication)
                {
                    var deduplicationResult = _deduplicationService.Deduplicate(_parsedImportData, _currentConfig);

                    // 使用去重后的数据替换原始数据
                    _parsedImportData = deduplicationResult.DeduplicatedData;

                    if (deduplicationResult.DuplicateCount > 0)
                    {
                        MainForm.Instance.ShowStatusText($"数据转换完成，原始数据 {deduplicationResult.OriginalCount} 行，去重后 {_parsedImportData.Rows.Count} 行（移除 {deduplicationResult.DuplicateCount} 行重复数据）");
                    }
                    else
                    {
                        MainForm.Instance.ShowStatusText($"数据转换完成，共 {_parsedImportData.Rows.Count} 行数据（无重复数据）");
                    }
                }
                else
                {
                    MainForm.Instance.ShowStatusText($"数据转换完成，共 {_parsedImportData.Rows.Count} 行数据");
                }

                // 绑定到解析后数据表格
                dgvParsedImportData.DataSource = _parsedImportData;

                // 切换到解析数据预览页面
                kryptonNavigatorDynamic.SelectedPage = kryptonPageParsedData;

                // 解析完成后，启用导入按钮和生成预览按钮
                UpdateMappingControlStates();

                MainForm.Instance.ShowStatusText($"数据解析完成，共 {_parsedImportData.Rows.Count} 行数据，可以点击\"导入\"按钮直接导入");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解析Excel文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 应用列映射配置转换数据
        /// </summary>
        /// <param name="sourceData">源数据表格（从Excel解析得到）</param>
        /// <param name="mappings">列映射配置集合</param>
        /// <returns>转换后的数据表格（数据库字段名格式）</returns>
        /// <remarks>
        /// 【重要】本方法负责将Excel列名转换为数据库字段名，并处理各种数据源类型：
        /// 
        /// 1. Excel数据源 (DataSourceType.Excel):
        ///    - Excel列名: mapping.ExcelColumn (例如: "供应商名称", "产品编码")
        ///    - 数据库字段: mapping.SystemField.Value (例如: "SupplierName", "ProductCode")
        ///    - 示例: Excel中的"供应商名称"列 -> 数据库的SupplierName字段
        /// 
        /// 2. 外键关联 (DataSourceType.ForeignKey):
        ///    - Excel来源列: mapping.ForeignConfig.ForeignKeySourceColumn.Key (例如: "供应商名称")
        ///    - 外键表: mapping.ForeignConfig.ForeignKeyTable.Key (例如: "tb_Supplier")
        ///    - 外键字段: mapping.ForeignConfig.ForeignKeyField.Key (例如: "SupplierID")
        ///    - 目标字段: mapping.SystemField.Value (例如: "SupplierID")
        ///    - 示例: Excel中的"供应商名称" -> 查询tb_Supplier表获取SupplierID -> 存入数据库的SupplierID字段
        /// 
        /// 3. 列拼接 (DataSourceType.ColumnConcat):
        ///    - Excel源列列表: mapping.ConcatConfig.SourceColumns (例如: ["省", "市", "区"])
        ///    - 分隔符: mapping.ConcatConfig.Separator (例如: "-")
        ///    - 目标字段: mapping.SystemField.Value (例如: "FullAddress")
        ///    - 示例: Excel中的"省"+"-"+"市"+"-"+"区" -> 数据库的FullAddress字段
        /// 
        /// 4. 字段复制 (DataSourceType.FieldCopy):
        ///    - 源字段: mapping.CopyFromField.Key (例如: "ProductCode")
        ///    - 目标字段: mapping.SystemField.Value (例如: "ProductName")
        ///    - 示例: 将ProductCode字段的值复制到ProductName字段
        /// 
        /// 5. 系统生成 (DataSourceType.SystemGenerated):
        ///    - 目标字段: mapping.SystemField.Value (例如: "CreateTime", "CreateUser")
        ///    - 示例: 系统自动生成创建时间、创建人等字段
        /// 
        /// 6. 默认值 (DataSourceType.DefaultValue):
        ///    - 默认值: mapping.DefaultValue (例如: "0", "Active")
        ///    - 目标字段: mapping.SystemField.Value (例如: "Status")
        ///    - 示例: Status字段默认为0(禁用)或Active状态
        /// 
        /// 注意：
        /// - sourceData中的列名是Excel中的原始列名（中文显示名）
        /// - result中的列名是数据库字段名（英文标识符）
        /// - 转换过程通过ColumnMapping配置建立映射关系
        /// </remarks>

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

                // 重置解析后的数据和按钮状态
                _parsedImportData = new DataTable();
                dgvParsedImportData.DataSource = _parsedImportData;
                if (kbtnGeneratePreview != null)
                {
                    kbtnGeneratePreview.Enabled = false;
                }
                kbtnDynamicImport.Enabled = false;

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
        /// 【功能说明】打开列映射配置界面，用户可以新增或编辑映射关系
        /// 【操作流程】选择数据类型 → 点击此按钮 → 配置映射 → 保存配置
        /// </summary>
        private void KbtnDynamicMap_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择数据类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isEditMode = kcmbDynamicMappingName.SelectedIndex > 0;
                string mappingName = isEditMode ? GetMappingNameFromCombo() : string.Empty;
                
                if (isEditMode)
                {
                    LoadSelectedMapping();
                }

                using (var frmMapping = new frmColumnMappingConfig(_selectedEntityType, _rawExcelData))
                {
                    frmMapping.ImportConfig = _currentConfig;
                    frmMapping.IsEditMode = isEditMode;
                    frmMapping.OriginalMappingName = mappingName;

                    frmMapping.MappingSaved += (s, args) =>
                    {
                        LoadMappingConfigsForEntityType();
                        AutoSelectSavedMapping(frmMapping.SavedMappingName);
                    };

                    if (frmMapping.ShowDialog() == DialogResult.OK)
                    {
                        _currentConfig = frmMapping.ImportConfig;
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
        /// 从下拉框获取映射配置名称（去除前缀）
        /// </summary>
        private string GetMappingNameFromCombo()
        {
            string selectedText = kcmbDynamicMappingName.SelectedItem.ToString();
            if (selectedText.StartsWith("[配置] "))
                return selectedText.Substring(5);
            if (selectedText.StartsWith("[Profile] "))
                return selectedText.Substring(10);
            return selectedText;
        }

        /// <summary>
        /// 自动选中刚保存的映射配置
        /// </summary>
        private void AutoSelectSavedMapping(string savedMappingName)
        {
            if (string.IsNullOrEmpty(savedMappingName)) return;
            
            string configText = $"[配置] {savedMappingName}";
            int index = kcmbDynamicMappingName.FindStringExact(configText);
            if (index > 0)
            {
                kcmbDynamicMappingName.SelectedIndex = index;
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

                // 【修复】从下拉框获取配置名时需要去除"[配置] "前缀
                string selectedText = kcmbDynamicMappingName.SelectedItem.ToString();
                string mappingName = selectedText;
                
                if (selectedText.StartsWith("[配置] "))
                {
                    mappingName = selectedText.Substring(5); // 移除 "[配置] " 前缀
                }
                else if (selectedText.StartsWith("[Profile] "))
                {
                    mappingName = selectedText.Substring(10); // 移除 "[Profile] " 前缀
                }
                
                _currentConfig = _columnMappingManager.LoadConfiguration(mappingName, _selectedEntityType);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行动态导入（统一入口）
        /// 【功能说明】将解析后的数据写入数据库
        /// </summary>
        private async Task ExecuteDynamicImport()
        {
            try
            {
                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择数据类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_currentConfig == null || _currentConfig.ColumnMappings == null || _currentConfig.ColumnMappings.Count == 0)
                {
                    MessageBox.Show("请先配置列映射关系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_parsedImportData == null || _parsedImportData.Rows.Count == 0)
                {
                    MessageBox.Show("没有可导入的数据，请先点击\"解析\"按钮转换数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 初始化DynamicImporter
                _dynamicImporter = new DynamicImporter(_db, _unitOfWorkManage, _foreignKeyService);
                await ExecuteSingleImport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行单个导入任务
        /// </summary>
        private async Task ExecuteSingleImport()
        {
            // 优先使用最终预览数据（包含所有可预生成的值）
            DataTable importData = _finalPreviewData?.Rows.Count > 0 ? _finalPreviewData : _parsedImportData;

            // 获取勾选的行
            var selectedRows = GetSelectedRows();
            if (selectedRows.Rows.Count == 0)
            {
                if (importData.Rows.Count > 0)
                {
                    MessageBox.Show("请先勾选需要导入的数据行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("没有可导入的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            importData = selectedRows;

            // 唯一性检查
            var duplicateErrors = CheckUniqueValues(importData);
            if (duplicateErrors.Count > 0 && !ConfirmContinueWithDuplicates(duplicateErrors))
            {
                return;
            }

            // 移除重复的记录
            if (duplicateErrors.Count > 0)
            {
                RemoveDuplicateRows(importData, duplicateErrors);
            }

            // 数据验证
            var validationErrors = _dynamicDataValidator.Validate(importData, 
                new ColumnMappingCollection(_currentConfig?.ColumnMappings ?? new List<ColumnMapping>()), 
                _selectedEntityType);
            
            if (validationErrors.Count > 0 && !ConfirmContinueWithValidationErrors(validationErrors))
            {
                return;
            }

            if (importData.Rows.Count == 0)
            {
                MessageBox.Show("没有有效数据可导入", "导入取消", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MainForm.Instance.ShowStatusText("正在导入数据到数据库...");

            // 初始化导入器
            _dynamicImporter = new DynamicImporter(_db, _unitOfWorkManage, _foreignKeyService);
            _dynamicImporter.SetCurrentConfiguration(_currentConfig);

            string importType = GetImportType();
            var mappings = new ColumnMappingCollection(_currentConfig?.ColumnMappings ?? new List<ColumnMapping>());
            bool hasImageFields = mappings.Any(m => m.DataSourceType == DataSourceType.ExcelImage || m.IsImageColumn);
            bool isPreprocessed = (_finalPreviewData?.Rows.Count > 0 && importData == _finalPreviewData);

            if (isPreprocessed)
            {
                MainForm.Instance.ShowStatusText("检测到已预处理的预览数据，将跳过重复的外键解析和系统字段生成...");
            }

            DynamicImporter.ImportResult importResult;
            string excelFilePath = ktxtDynamicFilePath?.Text;

            if (hasImageFields && !string.IsNullOrEmpty(excelFilePath) && File.Exists(excelFilePath))
            {
                int sheetIndex = kcmbDynamicSheetName.SelectedIndex;
                importResult = await _dynamicImporter.ImportFromExcelAsync(
                    excelFilePath, mappings, _selectedEntityType, sheetIndex, 0, importType);
            }
            else
            {
                importResult = await _dynamicImporter.ImportAsync(importData, mappings, _selectedEntityType, importType, isPreprocessed);
            }

            DisplayImportResult(importResult);
            UpdateButtonStatesAfterImport();
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
        /// 【P0修复】优化为批量查询，减少数据库访问次数
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
                // 【P0修复】批量收集所有需要检查的唯一性字段值
                var fieldsToCheck = new Dictionary<string, HashSet<string>>();
                
                foreach (var uniqueMapping in uniqueMappings)
                {
                    string fieldName = uniqueMapping.SystemField?.Key;
                    if (!dataTable.Columns.Contains(fieldName))
                    {
                        continue;
                    }
                    
                    // 收集该字段的所有非空值
                    var fieldValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        object value = row[fieldName];
                        if (value != null && value != DBNull.Value && !string.IsNullOrEmpty(value.ToString()))
                        {
                            fieldValues.Add(value.ToString().Trim());
                        }
                    }
                    
                    if (fieldValues.Count > 0)
                    {
                        fieldsToCheck[fieldName] = fieldValues;
                    }
                }
                
                // 【P0修复】一次性批量查询所有字段的已存在值
                var existingValuesMap = BatchQueryExistingUniqueValues(fieldsToCheck);
                
                // 在内存中比对，找出重复的记录
                foreach (var uniqueMapping in uniqueMappings)
                {
                    string fieldName = uniqueMapping.SystemField?.Key;
                    if (!fieldsToCheck.ContainsKey(fieldName) || !existingValuesMap.ContainsKey(fieldName))
                    {
                        continue;
                    }
                    
                    var existingValues = existingValuesMap[fieldName];
                    
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
        /// 【P0修复】批量查询多个字段的已存在值
        /// 一次性查询所有需要的字段，减少数据库访问次数
        /// </summary>
        /// <param name="fieldsToCheck">需要检查的字段及其值集合</param>
        /// <returns>每个字段的已存在值集合</returns>
        private Dictionary<string, HashSet<string>> BatchQueryExistingUniqueValues(Dictionary<string, HashSet<string>> fieldsToCheck)
        {
            var result = new Dictionary<string, HashSet<string>>();
            
            if (fieldsToCheck == null || fieldsToCheck.Count == 0)
            {
                return result;
            }
            
            try
            {
                string tableName = _selectedEntityType.Name;
                
                // 【P0修复】构建一个SQL查询，一次性获取所有字段的已存在值
                // 使用 UNION ALL 合并多个字段的查询结果
                var sqlParts = new List<string>();
                var parameters = new Dictionary<string, object>();
                
                int paramIndex = 0;
                foreach (var kvp in fieldsToCheck)
                {
                    string fieldName = kvp.Key;
                    var values = kvp.Value;
                    
                    if (values.Count == 0)
                        continue;
                    
                    // 为每个字段构建 IN 查询
                    var paramNames = new List<string>();
                    int valueIndex = 0;
                    foreach (var value in values)
                    {
                        string paramName = $"@val_{fieldName}_{valueIndex}";
                        paramNames.Add(paramName);
                        parameters[paramName] = value;
                        valueIndex++;
                    }
                    
                    string inClause = string.Join(",", paramNames);
                    sqlParts.Add($"SELECT '{fieldName}' AS FieldName, [{fieldName}] AS Value FROM [{tableName}] WHERE [{fieldName}] IN ({inClause})");
                }
                
                if (sqlParts.Count == 0)
                {
                    return result;
                }
                
                // 合并所有查询
                string sql = string.Join(" UNION ALL ", sqlParts);
                
                // 执行查询
                var queryResult = _db.Ado.GetDataTable(sql, parameters);
                
                // 解析结果，按字段分组
                foreach (System.Data.DataRow row in queryResult.Rows)
                {
                    string fieldName = row["FieldName"]?.ToString();
                    string value = row["Value"]?.ToString();
                    
                    if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(value))
                        continue;
                    
                    if (!result.ContainsKey(fieldName))
                    {
                        result[fieldName] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    }
                    
                    result[fieldName].Add(value.Trim());
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowStatusText($"批量查询唯一性字段值失败: {ex.Message}");
                // 如果批量查询失败，回退到逐个查询
                foreach (var kvp in fieldsToCheck)
                {
                    result[kvp.Key] = QueryExistingUniqueValues(kvp.Key);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 从数据库查询已存在的唯一性字段值（保留作为备用方法）
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
            // 根据当前选中的页面获取数据源
            DataTable sourceData = null;
            DataGridView dataGridView = null;

            if (kryptonNavigatorDynamic.SelectedPage == kryptonPageFinalPreview && _finalPreviewData?.Rows.Count > 0)
            {
                // 使用最终预览数据
                sourceData = _finalPreviewData;
                dataGridView = dgvFinalPreview;
            }
            else
            {
                // 使用解析后的数据
                sourceData = _parsedImportData;
                dataGridView = dgvParsedImportData;
            }

            if (sourceData == null || sourceData.Rows.Count == 0)
            {
                return new DataTable();
            }

            DataTable selectedData = sourceData.Clone();

            // 遍历DataGridView的每一行
            foreach (DataGridViewRow dgvRow in dataGridView.Rows)
            {
                // 检查是否为数据行（非新增行）
                if (dgvRow.IsNewRow)
                {
                    continue;
                }

                // 检查是否勾选
                if (dataGridView.Columns.Contains("Selected") &&
                    dgvRow.Cells["Selected"] != null &&
                    dgvRow.Cells["Selected"].Value != null &&
                    dgvRow.Cells["Selected"].Value is bool &&
                    (bool)dgvRow.Cells["Selected"].Value)
                {
                    // 从DataTable中获取对应的行并导入
                    int rowIndex = dgvRow.Index;
                    if (rowIndex >= 0 && rowIndex < sourceData.Rows.Count)
                    {
                        selectedData.ImportRow(sourceData.Rows[rowIndex]);
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
            
            // 显示图片导入信息
            if (result.ImageCount > 0)
            {
                message.AppendLine($"导入图片数：{result.ImageCount}");
            }
            
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

            // 显示图片保存路径信息
            if (result.ImportedImagePaths?.Count > 0)
            {
                message.AppendLine($"\n图片保存路径：");
                int displayPathCount = Math.Min(5, result.ImportedImagePaths.Count);
                for (int i = 0; i < displayPathCount; i++)
                {
                    message.AppendLine($"  {result.ImportedImagePaths[i]}");
                }
                if (result.ImportedImagePaths.Count > 5)
                {
                    message.AppendLine($"  ... 还有 {result.ImportedImagePaths.Count - 5} 张图片");
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
        /// 生成结果预览按钮点击事件
        /// 生成包含所有可预生成值的最终预览数据（除主键外）
        /// </summary>
        private async void kbtnGeneratePreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (_parsedImportData == null || _parsedImportData.Rows.Count == 0)
                {
                    MessageBox.Show("请先点击\"解析\"按钮转换数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MainForm.Instance.ShowStatusText("正在预处理数据...");

                // 使用DynamicImporter的预处理功能（外键预加载已在内部处理）
                _dynamicImporter = new DynamicImporter(_db, _unitOfWorkManage, _foreignKeyService);
                var mappings = new ColumnMappingCollection(_currentConfig?.ColumnMappings ?? new List<ColumnMapping>());
                
                _finalPreviewData = await _dynamicImporter.PreprocessDataAsync(_parsedImportData, mappings, _selectedEntityType);

                // 3. 显示预览
                dgvFinalPreview.DataSource = _finalPreviewData;

                // 4. 切换到最终预览页面
                kryptonNavigatorDynamic.SelectedIndex = 2;

                // 5. 启用批量导入按钮
                kbtnDynamicImport.Enabled = true;

                MainForm.Instance.ShowStatusText($"预处理完成，共 {_finalPreviewData.Rows.Count} 行数据，可以点击\"批量导入\"按钮保存到数据库");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预处理失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 【修改】不再恢复按钮状态，因为从未禁用过
                // kbtnGeneratePreview.Enabled = true;
            }
        }

        /// <summary>
        /// 批量导入按钮点击事件
        /// 仅负责将预处理阶段生成的数据集合批量保存至数据库
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void kbtnDynamicImport_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已完成预处理
                if (_finalPreviewData == null || _finalPreviewData.Rows.Count == 0)
                {
                    MessageBox.Show("请先点击\"预处理\"按钮生成最终数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 确认批量导入
                var confirmResult = MessageBox.Show(
                    $"确定要批量导入 {_finalPreviewData.Rows.Count} 条数据到 {_selectedEntityType?.Name} 吗？\n\n" +
                    $"数据已在预处理阶段完成处理，现在将直接保存到数据库。",
                    "确认批量导入",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult != DialogResult.Yes)
                {
                    return;
                }

                // 执行批量导入（异步）
                await ExecuteBatchImport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"批量导入失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行批量导入任务
        /// 仅负责将预处理阶段生成的数据集合批量保存至数据库
        /// </summary>
        private async Task ExecuteBatchImport()
        {
            try
            {
                // 检查是否有预处理数据
                if (_finalPreviewData == null || _finalPreviewData.Rows.Count == 0)
                {
                    MessageBox.Show("没有可导入的数据，请先点击\"预处理\"按钮", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查是否有勾选的行
                var selectedRows = GetSelectedRows();
                if (selectedRows.Rows.Count == 0)
                {
                    if (_finalPreviewData.Rows.Count > 0)
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

                // 数据验证（可选：预处理时已验证，这里再次验证确保数据完整性）
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
                    MessageBox.Show($"没有有效数据可导入", "导入取消", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 开始批量导入
                // 【修改】不再禁用按钮，允许用户多次操作
                // kbtnDynamicImport.Enabled = false;
                // kbtnDynamicBrowse.Enabled = false;
                // kbtnDynamicMap.Enabled = false;

                MainForm.Instance.ShowStatusText("正在批量导入数据到数据库...");

                // 初始化导入器，传入共享的ForeignKeyService实例
                _dynamicImporter = new DynamicImporter(_db, _unitOfWorkManage, _foreignKeyService);
                
                // 设置当前配置，以便DynamicImporter读取业务键等信息
                _dynamicImporter.SetCurrentConfiguration(_currentConfig);

                // 获取导入类型标识（用于区分客户和供应商等使用相同表的情况）
                string importType = GetImportType();

                var mappings = new ColumnMappingCollection(_currentConfig?.ColumnMappings ?? new List<ColumnMapping>());

                // ✅ 执行批量导入，数据已在预处理阶段处理（isPreprocessed = true）
                DynamicImporter.ImportResult importResult = await _dynamicImporter.ImportAsync(
                    importData, 
                    mappings, 
                    _selectedEntityType, 
                    importType, 
                    isPreprocessed: true  // 标记数据已预处理
                );

                // 显示导入结果
                DisplayImportResult(importResult);

                // 重置状态
                kbtnDynamicImport.Enabled = true;
                kbtnDynamicBrowse.Enabled = true;
                kbtnDynamicMap.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"批量导入失败，请检查数据格式和映射配置。{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 【修改】不再恢复按钮状态，因为从未禁用过
                // kbtnDynamicImport.Enabled = true;
                // kbtnDynamicBrowse.Enabled = true;
                // kbtnDynamicMap.Enabled = true;
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
                                    // 【内存优化】使用 using 确保原始图片及时释放
                                    using (Image img = Image.FromFile(imagePath))
                                    {
                                        // 缩放图片以适应单元格
                                        int maxSize = Math.Min(dgvRawExcelData.Rows[e.RowIndex].Height - 4, 80);
                                        // 【内存优化】创建缩略图副本，DataGridView需要独立实例
                                        Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                        e.Value = scaledImg;
                                        // 注意：scaledImg 由 DataGridView 管理，在 CellFormatting 中无法主动释放
                                        // 通过缓存键追踪，后续可通过 ClearImageCache() 清理
                                        RegisterImageCache(scaledImg, cellValue);
                                    }
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
                                    using (Image img = Image.FromStream(ms))
                                    {
                                        int maxSize = Math.Min(dgvRawExcelData.Rows[e.RowIndex].Height - 4, 80);
                                        Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                        e.Value = scaledImg;
                                        RegisterImageCache(scaledImg, $"binary_{e.RowIndex}_{e.ColumnIndex}");
                                    }
                                }
                                else
                                {
                                    // 如果是路径，尝试加载
                                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages", cellValue);
                                    if (File.Exists(imagePath))
                                    {
                                        using (Image img = Image.FromFile(imagePath))
                                        {
                                            int maxSize = Math.Min(dgvRawExcelData.Rows[e.RowIndex].Height - 4, 80);
                                            Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                            e.Value = scaledImg;
                                            RegisterImageCache(scaledImg, cellValue);
                                        }
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
                                    // 【内存优化】使用 using 确保原始图片及时释放
                                    using (Image img = Image.FromFile(imagePath))
                                    {
                                        // 缩放图片以适应单元格
                                        int maxSize = Math.Min(dgvParsedImportData.Rows[e.RowIndex].Height - 4, 80);
                                        Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                        e.Value = scaledImg;
                                        RegisterImageCache(scaledImg, cellValue);
                                    }
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
                                    using (Image img = Image.FromStream(ms))
                                    {
                                        int maxSize = Math.Min(dgvParsedImportData.Rows[e.RowIndex].Height - 4, 80);
                                        Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                        e.Value = scaledImg;
                                        RegisterImageCache(scaledImg, $"binary_{e.RowIndex}_{e.ColumnIndex}");
                                    }
                                }
                                else
                                {
                                    // 如果是路径，尝试加载
                                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages", cellValue);
                                    if (File.Exists(imagePath))
                                    {
                                        using (Image img = Image.FromFile(imagePath))
                                        {
                                            int maxSize = Math.Min(dgvParsedImportData.Rows[e.RowIndex].Height - 4, 80);
                                            Image scaledImg = new Bitmap(img, new Size(maxSize, maxSize));
                                            e.Value = scaledImg;
                                            RegisterImageCache(scaledImg, cellValue);
                                        }
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
