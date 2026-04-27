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

        #region 图片缓存管理
        // 【内存优化】图片缓存，防止重复加载同一图片
        private readonly Dictionary<string, Image> _imageCache = new Dictionary<string, Image>();
        private readonly object _imageCacheLock = new object();
        private const int MaxImageCacheSize = 100; // 最多缓存100张图片
        
        // 【P0修复】跟踪当前显示的Bitmap对象，用于及时释放
        private readonly Dictionary<string, Image> _currentDisplayImages = new Dictionary<string, Image>();
        private readonly object _displayImagesLock = new object();

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
                        if (_imageCache.TryGetValue(k, out var oldImg))
                        {
                            oldImg?.Dispose();
                        }
                        _imageCache.Remove(k);
                    }
                }

                // 避免重复添加相同键
                if (!_imageCache.ContainsKey(key))
                {
                    _imageCache[key] = image;
                }
            }
            
            // 【P0修复】同时注册到当前显示图片追踪字典
            RegisterDisplayImage(image, key);
        }
        
        /// <summary>
        /// 【P0修复】注册当前显示的图片，用于及时释放旧图片
        /// </summary>
        private void RegisterDisplayImage(Image image, string key)
        {
            if (image == null || string.IsNullOrEmpty(key)) return;
            
            lock (_displayImagesLock)
            {
                // 如果该key已有显示的图片，先释放旧图片
                if (_currentDisplayImages.TryGetValue(key, out var oldImage))
                {
                    // 只有当旧图片不在缓存中时才释放（缓存中的图片可能被复用）
                    if (!_imageCache.ContainsKey(key))
                    {
                        oldImage?.Dispose();
                    }
                }
                
                _currentDisplayImages[key] = image;
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
            
            // 【P0修复】同时清理当前显示的图片追踪
            lock (_displayImagesLock)
            {
                foreach (var img in _currentDisplayImages.Values)
                {
                    // 只释放不在缓存中的图片（缓存中的已在上面释放）
                    if (!_imageCache.ContainsValue(img))
                    {
                        img?.Dispose();
                    }
                }
                _currentDisplayImages.Clear();
            }
        }
        #endregion
        
        // 宽表导入相关字段
        private RUINORERP.Business.ImportEngine.SmartImportEngine _wideTableEngine;
        private bool _isWideTableMode = false;  // 是否启用宽表模式

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
            
            // 初始化宽表导入引擎
            _wideTableEngine = new RUINORERP.Business.ImportEngine.SmartImportEngine(_db);

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
        /// 添加导入模式选择面板
        /// </summary>
        /// <summary>
        /// 单表导入单选按钮改变事件
        /// </summary>
        private void KradioSingleTable_CheckedChanged(object sender, EventArgs e)
        {
            if (kradioSingleTable.Checked)
            {
                SwitchToSingleTableMode();
            }
        }
        
        /// <summary>
        /// 宽表导入单选按钮改变事件
        /// </summary>
        private void KradioWideTable_CheckedChanged(object sender, EventArgs e)
        {
            if (kradioWideTable.Checked)
            {
                SwitchToWideTableMode();
            }
        }
        
        /// <summary>
        /// 切换到单表导入模式
        /// </summary>
        private void SwitchToSingleTableMode()
        {
            _isWideTableMode = false;
            klblModeDescription.Values.Text = "单表导入：适合单个Excel表导入一个数据库表，需要配置列映射关系";
            MainForm.Instance.ShowStatusText("✓ 已切换到单表导入模式");
            
            // 启用实体类型选择
            kcmbDynamicEntityType.Enabled = true;
            
            // 重置下拉框提示文本
            if (kcmbDynamicMappingName.Items.Count > 0 && kcmbDynamicMappingName.SelectedIndex == 0)
            {
                kcmbDynamicMappingName.Items[0] = "-- 请选择导入配置 --";
            }
            
            // 重新加载单表配置
            if (_selectedEntityType != null)
            {
                LoadMappingConfigsForEntityType();
            }
            else
            {
                kcmbDynamicMappingName.Items.Clear();
                kcmbDynamicMappingName.Items.Add("-- 请先选择实体类型 --");
                kcmbDynamicMappingName.SelectedIndex = 0;
            }
            
            UpdateMappingControlStates();
        }
        
        /// <summary>
        /// 切换到宽表导入模式
        /// </summary>
        private void SwitchToWideTableMode()
        {
            _isWideTableMode = true;
            klblModeDescription.Values.Text = "宽表导入：适合一个Excel表包含主表和多个依赖表的数据，使用预定义的Profile配置";
            MainForm.Instance.ShowStatusText("✓ 已切换到宽表导入模式");
            
            // 禁用实体类型选择（宽表不需要）
            kcmbDynamicEntityType.Enabled = false;
            
            // 清空并重新加载宽表配置
            LoadWideTableProfilesOnly();
            
            // 宽表模式按钮状态
            kbtnDynamicParse.Enabled = false;
            kbtnDynamicMap.Enabled = false;
            kbtnGeneratePreview.Enabled = false;
            kbtnDynamicImport.Enabled = true;
        }
        
        /// <summary>
        /// 仅加载宽表Profile配置
        /// </summary>
        private void LoadWideTableProfilesOnly()
        {
            try
            {
                kcmbDynamicMappingName.Items.Clear();
                kcmbDynamicMappingName.Items.Add("-- 请选择宽表Profile --");
                
                var profileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles");
                if (!Directory.Exists(profileDirectory))
                {
                    MainForm.Instance.ShowStatusText("未找到宽表Profile配置目录");
                    return;
                }

                // 查找所有宽表Profile文件
                var wideTableProfiles = new List<string>();
                foreach (var jsonFile in Directory.GetFiles(profileDirectory, "*.json"))
                {
                    try
                    {
                        var content = File.ReadAllText(jsonFile);
                        if (content.Contains("\"MasterTable\"") || content.Contains("\"DependencyTables\""))
                        {
                            var profileName = Path.GetFileNameWithoutExtension(jsonFile);
                            wideTableProfiles.Add(profileName);
                        }
                    }
                    catch
                    {
                        // 忽略无法读取的文件
                    }
                }

                // 添加宽表Profile
                foreach (var profile in wideTableProfiles.OrderBy(p => p))
                {
                    kcmbDynamicMappingName.Items.Add($"[宽表] {profile}");
                }
                
                MainForm.Instance.ShowStatusText($"已加载 {wideTableProfiles.Count} 个宽表Profile配置");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowStatusText($"加载宽表Profile失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加宽表Profile管理按钮和生成结果预览按钮
        /// </summary>
        /// <summary>
        /// 管理宽表Profile按钮点击事件
        /// </summary>
        private async void kbtnManageProfiles_Click(object sender, EventArgs e)
        {
            await OpenWideTableProfileEditor();
        }
        
        /// <summary>
        /// 打开宽表Profile编辑器
        /// </summary>
        private async Task OpenWideTableProfileEditor()
        {
            try
            {
                var frm = new FrmWideTableProfileEditor(_db);
                var result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // 重新加载Profile列表
                    LoadWideTableProfiles();
                    MessageBox.Show("Profile已保存,请从下拉框中选择", "提示", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开Profile编辑器失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    _isWideTableMode = false;
                    UpdateMappingControlStates();
                    return;
                }

                string selectedText = kcmbDynamicMappingName.SelectedItem.ToString();
                
                // 检查是否为宽表Profile
                if (selectedText.StartsWith("[宽表] "))
                {
                    _isWideTableMode = true;
                    MainForm.Instance.ShowStatusText($"✓ 已切换到宽表导入模式: {selectedText}");
                    
                    // 宽表模式不需要选择实体类型和映射配置
                    kbtnDynamicParse.Enabled = false;  // 禁用解析按钮(宽表不需要预览解析)
                    kbtnDynamicMap.Enabled = false;    // 禁用映射配置按钮
                    kbtnGeneratePreview.Enabled = false; // 禁用预览按钮
                    kbtnDynamicImport.Enabled = true;  // 宽表可以直接导入
                    
                    return;
                }
                else if (selectedText.StartsWith("[配置] "))
                {
                    // 单表配置模式
                    _isWideTableMode = false;
                    MainForm.Instance.ShowStatusText($"✓ 已切换到单表导入模式: {selectedText}");
                }
                else
                {
                    // 旧格式配置（兼容）
                    _isWideTableMode = false;
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
            bool isWideTable = _isWideTableMode;

            // 根据当前模式更新控件状态
            if (isWideTable)
            {
                // 宽表模式：禁用解析和映射配置按钮
                kbtnDynamicParse.Enabled = false;  // 宽表不需要解析
                kbtnDynamicMap.Enabled = false;    // 宽表不需要映射配置
                kbtnGeneratePreview.Enabled = false; // 宽表不需要预览
                kbtnDynamicImport.Enabled = true;   // 宽表可以直接导入
                
                // 禁用实体类型选择（宽表不需要）
                if (kcmbDynamicEntityType != null)
                {
                    kcmbDynamicEntityType.Enabled = false;
                }
            }
            else
            {
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
        }

        /// <summary>
        /// 加载映射配置列表（根据当前模式）
        /// </summary>
        private void LoadMappingConfigs()
        {
            if (_isWideTableMode)
            {
                // 宽表模式：只加载宽表Profile
                LoadWideTableProfilesOnly();
            }
            else
            {
                // 单表模式：只加载单表配置
                LoadMappingConfigsForEntityType();
            }
        }

        /// <summary>
        /// 加载宽表Profile配置列表
        /// </summary>
        private void LoadWideTableProfiles()
        {
            try
            {
                var profileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles");
                if (!Directory.Exists(profileDirectory))
                {
                    return;
                }

                // 查找所有宽表Profile文件(包含MasterTable字段的JSON)
                var wideTableProfiles = new List<string>();
                foreach (var jsonFile in Directory.GetFiles(profileDirectory, "*.json"))
                {
                    try
                    {
                        var content = File.ReadAllText(jsonFile);
                        if (content.Contains("\"MasterTable\"") || content.Contains("\"DependencyTables\""))
                        {
                            var profileName = Path.GetFileNameWithoutExtension(jsonFile);
                            wideTableProfiles.Add(profileName);
                        }
                    }
                    catch
                    {
                        // 忽略无法读取的文件
                    }
                }

                // 将宽表Profile添加到下拉框(用特殊前缀标识)
                foreach (var profile in wideTableProfiles.OrderBy(p => p))
                {
                    // 检查是否已存在
                    if (!kcmbDynamicMappingName.Items.Contains($"[宽表] {profile}"))
                    {
                        kcmbDynamicMappingName.Items.Add($"[宽表] {profile}");
                    }
                }
                
                MainForm.Instance.ShowStatusText($"已加载 {wideTableProfiles.Count} 个宽表Profile配置");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowStatusText($"加载宽表Profile失败: {ex.Message}");
            }
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
        /// 根据映射配置解析Excel数据，仅做数据转换不查询外键
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
                // 根据映射配置转换数据（不包含外键查询）
                _parsedImportData = ApplyColumnMapping(fullData, _currentConfig.ColumnMappings);

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
                        !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn.Key))
                    {
                        string sourceColumnName = mapping.ForeignConfig.ForeignKeySourceColumn.Key;

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
                                            // 尝试处理图片并保存
                                            string imagePath = cellValue?.ToString();
                                            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                                            {
                                                try
                                                {
                                                    // 获取产品编码作为文件名前缀
                                                    string productCode = sourceRow["ProductCode"]?.ToString() ?? "IMG";
                                                    string savedPath = _imageProcessor.ProcessAndSaveImage(imagePath, productCode);
                                                    targetRow[mapping.SystemField?.Value] = savedPath;
                                                }
                                                catch (Exception ex)
                                                {
                                                    System.Diagnostics.Debug.WriteLine($"图片处理失败: {ex.Message}");
                                                    targetRow[mapping.SystemField?.Value] = imagePath; // 失败则保留原路径
                                                }
                                            }
                                            else
                                            {
                                                targetRow[mapping.SystemField?.Value] = imagePath ?? "";
                                            }
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
                                string sourceColumn = mapping.ForeignConfig?.ForeignKeySourceColumn?.Key ?? mapping.ExcelColumn;
                                // DisplayName = 显示名称
                                string sourceColumnDisplay = mapping.ForeignConfig?.ForeignKeySourceColumn?.Value ?? sourceColumn;

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
                                    targetRow[mapping.SystemField?.Key] = $"[通过关联外键:{sourceColumnDisplay}:{foreignKeySourceValue}->找{mapping.ForeignConfig?.ForeignKeyTable?.Value}.{mapping.ForeignConfig?.ForeignKeyField?.Value}]";
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Key] = $"[外键关联:{mapping.ForeignConfig?.ForeignKeyTable?.Value}.{mapping.ForeignConfig?.ForeignKeyField?.Value}]";
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
                string mappingName = string.Empty;
                
                if (isEditMode)
                {
                    // 【修复】从下拉框获取配置名时需要去除"[配置] "前缀
                    string selectedText = kcmbDynamicMappingName.SelectedItem.ToString();
                    if (selectedText.StartsWith("[配置] "))
                    {
                        mappingName = selectedText.Substring(5); // 移除 "[配置] " 前缀
                    }
                    else
                    {
                        mappingName = selectedText;
                    }
                }

                // 如果是编辑模式，先加载选中的配置
                if (isEditMode)
                {
                    LoadSelectedMapping();
                }

                // 使用原始Excel数据进行映射配置
                using (var frmMapping = new frmColumnMappingConfig(_selectedEntityType, _rawExcelData))
                {
                    // 设置参数
                    frmMapping.ImportConfig = _currentConfig;
                    frmMapping.IsEditMode = isEditMode;
                    frmMapping.OriginalMappingName = mappingName;

                    // 订阅映射配置保存成功事件
                    frmMapping.MappingSaved += (s, args) =>
                    {
                        // 刷新映射配置下拉列表
                        LoadMappingConfigsForEntityType();

                        // 【修复】自动选中刚保存的配置 - 需要匹配带前缀的格式
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
                _dynamicImporter = new DynamicImporter(_db, _foreignKeyService);

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

                // 移除不必要的Application.DoEvents()，后续的await会让UI线程有机会处理消息

                // 初始化导入器，传入共享的ForeignKeyService实例
                _dynamicImporter = new DynamicImporter(_db,  _foreignKeyService);

                // 获取导入类型标识（用于区分客户和供应商等使用相同表的情况）
                string importType = GetImportType();

                // 检查是否有图片字段映射
                var mappings = new ColumnMappingCollection(_currentConfig?.ColumnMappings ?? new List<ColumnMapping>());
                bool hasImageFields = mappings.Any(m => m.DataSourceType == DataSourceType.ExcelImage || m.IsImageColumn);

                DynamicImporter.ImportResult importResult;

                // 获取Excel文件路径
                string excelFilePath = ktxtDynamicFilePath?.Text;

                if (hasImageFields && !string.IsNullOrEmpty(excelFilePath) && File.Exists(excelFilePath))
                {
                    // 使用支持图片导入的方法
                    int sheetIndex = kcmbDynamicSheetName.SelectedIndex;
                    int headerRowIndex = 0; // 默认第一行为标题行
                    importResult = await _dynamicImporter.ImportFromExcelAsync(
                        excelFilePath, 
                        mappings, 
                        _selectedEntityType, 
                        sheetIndex, 
                        headerRowIndex,
                        importType);
                }
                else
                {
                    // 执行普通导入（异步）
                    importResult = await _dynamicImporter.ImportAsync(importData, mappings, _selectedEntityType, importType);
                }

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
        /// 预加载外键数据并解析为真实值，供用户确认后再导入
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

                MainForm.Instance.ShowStatusText("正在生成结果预览...");
                kbtnGeneratePreview.Enabled = false;

                // 1. 预加载所有外键数据到缓存
                PreloadForeignKeyData();

                // 2. 创建带外键解析的数据副本
                DataTable previewData = _parsedImportData.Copy();
                
                // 3. 解析外键字段为真实值
                ResolveForeignKeysInPreview(previewData);

                // 4. 显示预览
                dgvParsedImportData.DataSource = previewData;

                // 5. 启用导入按钮
                kbtnDynamicImport.Enabled = true;
                
                MainForm.Instance.ShowStatusText($"结果预览生成完成，共 {previewData.Rows.Count} 行数据，可以点击\"导入\"按钮保存到数据库（可选，已经可以直接导入）");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成结果预览失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                kbtnGeneratePreview.Enabled = true;
            }
        }

        /// <summary>
        /// 在预览数据中解析外键为真实值
        /// </summary>
        private void ResolveForeignKeysInPreview(DataTable previewData)
        {
            try
            {
                var foreignKeyMappings = _currentConfig.ColumnMappings
                    .Where(m => m.DataSourceType == DataSourceType.ForeignKey && m.ForeignConfig != null)
                    .ToList();

                if (!foreignKeyMappings.Any())
                {
                    return;
                }

                int resolvedCount = 0;

                foreach (var mapping in foreignKeyMappings)
                {
                    string systemFieldName = mapping.SystemField?.Key;
                    if (string.IsNullOrEmpty(systemFieldName) || !previewData.Columns.Contains(systemFieldName))
                    {
                        continue;
                    }

                    string sourceColumnName = mapping.ForeignConfig.ForeignKeySourceColumn?.Key;
                    if (string.IsNullOrEmpty(sourceColumnName) || !previewData.Columns.Contains(sourceColumnName))
                    {
                        continue;
                    }

                    string targetTable = mapping.ForeignConfig.ForeignKeyTable?.Value;
                    string targetField = mapping.ForeignConfig.ForeignKeyField?.Value;

                    if (string.IsNullOrEmpty(targetTable) || string.IsNullOrEmpty(targetField))
                    {
                        continue;
                    }

                    // 遍历每一行，解析外键
                    foreach (DataRow row in previewData.Rows)
                    {
                        object sourceValue = row[sourceColumnName];
                        if (sourceValue == null || sourceValue == DBNull.Value || string.IsNullOrEmpty(sourceValue.ToString()))
                        {
                            continue;
                        }

                        // 从缓存中获取外键ID
                        object foreignKeyId = _foreignKeyService.GetForeignKeyId(
                            sourceValue.ToString(),
                            targetTable,
                            targetField
                        );

                        if (foreignKeyId != null)
                        {
                            // 更新系统字段的值为真实的ID
                            row[systemFieldName] = foreignKeyId;
                            
                            // 添加一列显示关联信息（可选，用于调试）
                            string displayColName = $"{systemFieldName}_关联信息";
                            if (!previewData.Columns.Contains(displayColName))
                            {
                                previewData.Columns.Add(displayColName, typeof(string));
                            }
                            row[displayColName] = $"{sourceValue} → ID:{foreignKeyId}";
                            
                            resolvedCount++;
                        }
                        else
                        {
                            // 外键未找到，标记为错误
                            string errorColName = $"{systemFieldName}_错误";
                            if (!previewData.Columns.Contains(errorColName))
                            {
                                previewData.Columns.Add(errorColName, typeof(string));
                            }
                            row[errorColName] = $"未找到: {sourceValue}";
                        }
                    }
                }

                MainForm.Instance.ShowStatusText($"已解析 {resolvedCount} 个外键关联");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowStatusText($"解析外键失败: {ex.Message}");
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
                // 检查是否为宽表导入模式
                if (_isWideTableMode)
                {
                    // 宽表导入模式
                    if (kcmbDynamicMappingName.SelectedIndex <= 0)
                    {
                        MessageBox.Show("请先选择宽表导入Profile配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string profileName = kcmbDynamicMappingName.SelectedItem.ToString();
                    
                    // 显示导入策略选择对话框
                    var result = MessageBox.Show(
                        $"请选择导入策略:\n\n" +
                        $"【是】一键导入 - 自动处理所有表(推荐新手)\n" +
                        $"【否】分步导入 - 先导入基础表,再导入主表(推荐高级用户)\n\n" +
                        $"当前Profile: {profileName}",
                        "宽表导入策略选择",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        // 一键导入
                        await ExecuteWideTableImportOneClick(profileName);
                    }
                    else if (result == DialogResult.No)
                    {
                        // 分步导入 - 显示步骤选择
                        var stepResult = MessageBox.Show(
                            "分步导入:\n\n" +
                            "【是】步骤1: 仅导入基础表(供应商/类目)\n" +
                            "【否】步骤2: 仅导入主表(产品)\n\n" +
                            "注意: 执行步骤2前必须先完成步骤1",
                            "选择导入步骤",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (stepResult == DialogResult.Yes)
                        {
                            await ExecuteWideTableImportStep1_DependencyTables(profileName);
                        }
                        else
                        {
                            await ExecuteWideTableImportStep2_MasterTable(profileName);
                        }
                    }
                    // Cancel则不执行任何操作
                    return;
                }

                // 原有单表导入逻辑
                // 确认导入
                var confirmResult = MessageBox.Show(
                    $"确定要导入 {_parsedImportData?.Rows.Count ?? 0} 条数据到 {_selectedEntityType?.Name} 吗？\n\n" +
                    $"请确保已点击\"生成结果预览\"并检查数据无误。",
                    "确认导入",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult != DialogResult.Yes)
                {
                    return;
                }

                // 预加载外键数据（确保缓存最新）
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

        #region 宽表导入功能

        /// <summary>
        /// 执行宽表导入(一键式)
        /// </summary>
        private async Task ExecuteWideTableImportOneClick(string profileName)
        {
            try
            {
                MainForm.Instance.ShowStatusText("开始宽表导入...");
                kbtnDynamicImport.Enabled = false;

                var report = await _wideTableEngine.ExecuteWideTableImportAsync(
                    ktxtDynamicFilePath.Text,
                    profileName
                );

                if (report.IsSuccess)
                {
                    MessageBox.Show(
                        $"宽表导入成功!\n\n总记录数: {report.TotalRows}\n成功记录: {report.SuccessRows}\n\n{report.Message}",
                        "导入成功",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    MainForm.Instance.ShowStatusText($"宽表导入完成: {report.SuccessRows} 条记录");
                }
                else
                {
                    MessageBox.Show(
                        $"宽表导入失败:\n\n{report.Message}",
                        "导入失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    MainForm.Instance.ShowStatusText($"宽表导入失败: {report.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"宽表导入异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.Instance.ShowStatusText($"宽表导入异常: {ex.Message}");
            }
            finally
            {
                kbtnDynamicImport.Enabled = true;
            }
        }

        /// <summary>
        /// 执行分步导入 - 仅导入依赖表
        /// </summary>
        private async Task ExecuteWideTableImportStep1_DependencyTables(string profileName)
        {
            try
            {
                MainForm.Instance.ShowStatusText("步骤1: 导入基础表...");
                
                var report = await _wideTableEngine.ImportDependencyTablesOnlyAsync(
                    ktxtDynamicFilePath.Text,
                    profileName
                );

                if (report.IsSuccess)
                {
                    MessageBox.Show(
                        $"基础表导入成功!\n\n处理记录: {report.SuccessRows}\n\n{report.Message}\n\n接下来可以执行步骤2: 导入主表",
                        "步骤1完成",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    MainForm.Instance.ShowStatusText($"基础表导入完成: {report.SuccessRows} 条记录");
                }
                else
                {
                    MessageBox.Show($"基础表导入失败:\n\n{report.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"基础表导入异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行分步导入 - 仅导入主表
        /// </summary>
        private async Task ExecuteWideTableImportStep2_MasterTable(string profileName)
        {
            try
            {
                MainForm.Instance.ShowStatusText("步骤2: 导入主表...");
                
                var report = await _wideTableEngine.ImportMasterTableOnlyAsync(
                    ktxtDynamicFilePath.Text,
                    profileName
                );

                if (report.IsSuccess)
                {
                    MessageBox.Show(
                        $"主表导入成功!\n\n处理记录: {report.SuccessRows}\n\n{report.Message}",
                        "步骤2完成",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    MainForm.Instance.ShowStatusText($"主表导入完成: {report.SuccessRows} 条记录");
                }
                else
                {
                    MessageBox.Show($"主表导入失败:\n\n{report.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"主表导入异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

    }
}
