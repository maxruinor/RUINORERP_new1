using Newtonsoft.Json.Linq;
using RUINORERP.Server.Commands;
using RUINORERP.Server.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using Newtonsoft.Json;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Extensions.ServiceExtensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static RUINORERP.Extensions.ServiceExtensions.EditConfigCommand;
using Button = System.Windows.Forms.Button;
using static RUINORERP.Server.Controls.GlobalConfigControl.ConfigHistoryManager;
using RUINORERP.IServices;
using TextBox = System.Windows.Forms.TextBox;
using System.ComponentModel.DataAnnotations;

namespace RUINORERP.Server.Controls
{
    /// <summary>
    /// 全局配置管理控件
    /// 负责系统全局配置的显示、编辑、验证和历史记录管理功能
    /// </summary>
    public partial class GlobalConfigControl : UserControl
    {
        private readonly CommandManager _commandManager;
        private readonly ConfigFileReceiver _configFileReceiver;
        private readonly ILogger<GlobalConfigControl> _logger;
        private List<ConfigHistoryEntry> _configHistory;
        
        // 新增服务依赖
        private readonly IConfigVersionService _versionService;
        private readonly IConfigEncryptionService _encryptionService;
        private readonly IConfigManagerService _configManagerService;
        private readonly IConfigValidationService _configValidationService;
        
        // 移除审计日志服务依赖
        
        private readonly string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
        private ConfigHistoryManager _historyManager = new ConfigHistoryManager();
        
        /// <summary>
        /// 配置元数据字典 - 用于缓存配置类型和文件信息
        /// </summary>
        private readonly Dictionary<string, ConfigMetadata> _configMetadataCache = new Dictionary<string, ConfigMetadata>();

        /// <summary>
        /// 配置实体类型列表
        /// </summary>
        private readonly List<Type> _configEntityTypes = new List<Type>();
        
        /// <summary>
        /// 配置元数据类
        /// </summary>
        private class ConfigMetadata
        {
            public Type ConfigType { get; set; }
            public string FileName { get; set; }
            public string RootNode { get; set; }
        }

        private BaseConfig _currentConfig; // 当前配置对象
        private string _currentConfigFileName; // 当前配置文件名
        private string _currentConfigRootNode; // 当前配置根节点

        public GlobalConfigControl()
        {
            InitializeComponent();

            // 从依赖注入容器获取服务
            _logger = Startup.GetFromFac<ILogger<GlobalConfigControl>>();
            _versionService = Startup.GetFromFac<IConfigVersionService>();
            _encryptionService = Startup.GetFromFac<IConfigEncryptionService>();
            _configManagerService = Startup.GetFromFac<IConfigManagerService>();
            _configValidationService = Startup.GetFromFac<IConfigValidationService>();
            
            // 默认使用系统全局配置文件初始化，后续操作会根据选择的配置类型使用相应的文件
            _commandManager = new CommandManager();
            _configHistory = new List<ConfigHistoryEntry>();

            // 订阅命令管理器事件
            _commandManager.CommandExecuted += (s, e) => UpdateButtonStates();
            _commandManager.CommandUndone += (s, e) => UpdateButtonStates();
            _commandManager.CommandRedone += (s, e) => UpdateButtonStates();
            
            // 初始化版本管理工具栏按钮
            InitializeVersionManagementButtons();
            
            // 注册所有配置类型
            RegisterConfigTypes();
        }
        
        /// <summary>
        /// 注册所有配置类型
        /// </summary>
        private void RegisterConfigTypes()
        {
            try
            {
                // 注册已知的配置类型
                RegisterConfigType(typeof(ServerConfig), "ServerConfig.json", "ServerConfig");
                RegisterConfigType(typeof(SystemGlobalconfig), "SystemGlobalConfig.json", "SystemGlobalconfig");
                RegisterConfigType(typeof(GlobalValidatorConfig), "GlobalValidatorConfig.json", "GlobalValidatorConfig");
                
                // 可以在这里添加对配置目录的扫描，自动发现新的配置类型
                ScanConfigDirectoryForNewTypes();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "注册配置类型失败");
            }
        }
        
        /// <summary>
        /// 注册单个配置类型
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="fileName">配置文件名</param>
        /// <param name="rootNode">配置根节点名称</param>
        private void RegisterConfigType(Type configType, string fileName, string rootNode)
        {
            if (!typeof(BaseConfig).IsAssignableFrom(configType))
            {
                _logger?.LogWarning("尝试注册非BaseConfig类型: {TypeName}", configType.Name);
                return;
            }
            
            _configMetadataCache[configType.Name] = new ConfigMetadata
            {
                ConfigType = configType,
                FileName = fileName,
                RootNode = rootNode
            };
            
            if (!_configEntityTypes.Contains(configType))
            {
                _configEntityTypes.Add(configType);
            }
        }
        
        /// <summary>
        /// 扫描配置目录，发现新的配置类型
        /// </summary>
        private void ScanConfigDirectoryForNewTypes()
        {
            try
            {
                if (!Directory.Exists(basePath))
                {
                    return;
                }
                
                var jsonFiles = Directory.GetFiles(basePath, "*.json");
                foreach (var filePath in jsonFiles)
                {
                    string fileName = Path.GetFileName(filePath);
                    // 如果文件尚未在缓存中注册，尝试解析它
                    if (!_configMetadataCache.Values.Any(m => m.FileName == fileName))
                    {
                        try
                        {
                            string json = File.ReadAllText(filePath);
                            JObject configJson = JObject.Parse(json);
                            // 获取根节点名称
                            string rootNode = configJson.Properties().FirstOrDefault()?.Name;
                            if (!string.IsNullOrEmpty(rootNode))
                            {
                                // 这里可以尝试通过反射找到对应的类型
                                // 暂时只记录日志
                                _logger?.LogInformation("发现未注册的配置文件: {FileName}, 根节点: {RootNode}", fileName, rootNode);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "解析配置文件失败: {FilePath}", filePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "扫描配置目录失败");
            }
        }
        
        /// <summary>
        /// 初始化版本管理工具栏按钮
        /// </summary>
        private void InitializeVersionManagementButtons()
        {
            // 在实际环境中，这里应该添加版本管理相关的工具栏按钮
            // 由于设计器文件中可能尚未定义这些按钮，此处仅提供初始化逻辑
            if (components == null)
            {
                components = new System.ComponentModel.Container();
            }
        }
        private void GlobalConfigControl_Load(object sender, EventArgs e)
        {
            // 初始化新的TreeView，加载JSON文件
            InitializeJsonFilesTreeView();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                if (_currentConfig == null || string.IsNullOrEmpty(_currentConfigFileName) || string.IsNullOrEmpty(_currentConfigRootNode))
                {
                    MessageBox.Show("没有可保存的配置对象", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 根据配置类型进行验证
                bool isValid = ValidateConfiguration(_currentConfig);
                if (!isValid)
                {
                    return;
                }

                // 确定描述信息
                string description = GetConfigDescription(_currentConfig);
                string configFilePath = System.IO.Path.Combine(basePath, _currentConfigFileName);
                
                // 获取原始配置用于审计比较
                BaseConfig originalConfig = null;
                if (File.Exists(configFilePath))
                {
                    string json = File.ReadAllText(configFilePath);
                    JObject configJson = JObject.Parse(json);
                    if (configJson.TryGetValue(_currentConfigRootNode, out JToken token))
                    {
                        originalConfig = ((JObject)token).ToObject(_currentConfig.GetType()) as BaseConfig;
                        if (originalConfig != null)
                        {
                            // 解密原始配置以便比较
                            originalConfig = _encryptionService.DecryptConfig(originalConfig);
                        }
                    }
                }

                // 对敏感配置进行加密
                var encryptedConfig = _encryptionService.EncryptConfig(_currentConfig);
                
                // 使用配置管理器服务保存配置
                bool saveResult = _configManagerService.SaveConfig(encryptedConfig, configFilePath);
                if (!saveResult)
                {
                    throw new InvalidOperationException("配置保存失败");
                }
                
                // 创建完整的配置JSON对象
                JObject configJsonObj = JObject.FromObject(encryptedConfig);
                JObject fullConfigJson = new JObject(new JProperty(_currentConfigRootNode, configJsonObj));

                // 执行保存命令
                var configFileReceiver = new ConfigFileReceiver(configFilePath);
                EditConfigCommand command = new EditConfigCommand(configFileReceiver, fullConfigJson, description);
                _commandManager.ExecuteCommand(command);

                // 创建配置版本
                string versionDescription = $"{description} - 由{Environment.UserName}修改";
                _versionService.CreateVersion(_currentConfig, _currentConfigFileName, versionDescription);
                
                // 添加到历史记录
                var historyEntry = new ConfigHistoryEntry(_currentConfig, description)
                {
                    Operation = "保存配置",
                    ConfigSnapshot = (JObject)fullConfigJson.DeepClone()
                };
                _configHistory.Add(historyEntry);
                
                // 如果是服务器配置，初始化文件存储路径
                if (_currentConfig is ServerConfig serverConfig)
                {
                    FileStorageHelper.InitializeStoragePath(serverConfig);
                }
                
                // 更新UI状态
                UpdateButtonStates();
                
                MessageBox.Show($"配置已成功保存并创建版本", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "配置保存失败");
                MessageBox.Show($"保存配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
        /// <summary>
        /// 获取配置描述
        /// </summary>
        private string GetConfigDescription(BaseConfig config)
        {
            if (config is ServerConfig)
                return "服务器配置更新";
            else if (config is SystemGlobalconfig)
                return "系统全局配置更新";
            else if (config is GlobalValidatorConfig)
                return "全局验证配置更新";
            
            return "配置更新";
        }

        /// <summary>
        /// 验证全局验证配置
        /// </summary>
        private bool ValidateGlobalValidatorConfiguration(GlobalValidatorConfig configObject)
        {
            var validationResults = new List<string>();

            // 验证返工提醒天数
            if (configObject.ReworkTipDays < 0 || configObject.ReworkTipDays > 365)
            {
                validationResults.Add("返工提醒天数必须在0-365范围内");
            }

            // 验证计划提前提示天数
            if (configObject.计划提前提示天数 < 0 || configObject.计划提前提示天数 > 365)
            {
                validationResults.Add("计划提前提示天数必须在0-365范围内");
            }

            // 验证销售金额精度
            if (configObject.MoneyDataPrecision < 0 || configObject.MoneyDataPrecision > 10)
            {
                validationResults.Add("销售金额精度必须在0-10范围内");
            }

            if (validationResults.Count > 0)
            {
                var errorMessage = string.Join("\n", validationResults);
                MessageBox.Show($"全局验证配置验证失败:\n{errorMessage}", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

 
        /// <summary>
        /// 初始化JSON文件TreeView（左侧树）
        /// </summary>
        private void InitializeJsonFilesTreeView()
        {
            try
            {
                treeView2.Nodes.Clear();

                // 使用配置元数据字典创建所有配置文件节点
                foreach (var metadata in _configMetadataCache.Values)
                {
                    TreeNode node = new TreeNode(GetDisplayName(metadata.ConfigType));
                    node.Tag = new ConfigInfo {
                        FileName = metadata.FileName, 
                        RootNode = metadata.RootNode, 
                        ConfigType = metadata.ConfigType 
                    };
                    treeView2.Nodes.Add(node);
                }

                treeView2.ExpandAll();

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化JSON文件TreeView失败");
                MessageBox.Show($"初始化JSON文件树时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
   

        /// <summary>
        /// 获取类型的DisplayName属性值
        /// </summary>
        /// <param name="type">配置类类型</param>
        /// <returns>DisplayName属性值，如果不存在则返回类型名称</returns>
        private string GetDisplayName(Type type)
        {
            var displayNameAttr = type.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .FirstOrDefault() as DisplayNameAttribute;
            
            return displayNameAttr?.DisplayName ?? type.Name;
        }

        /// <summary>
        /// TreeView选择事件处理
        /// </summary>
        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                // 处理配置文件节点的选择
                if (e.Node?.Tag is ConfigInfo configInfo)
                {
                    // 加载配置文件
                    LoadConfigurationFile(configInfo.FileName, configInfo.RootNode, configInfo.ConfigType);
                }
                // 处理原有属性节点的选择，保留兼容性
                else if (e.Node.Tag != null && _currentConfig != null)
                {
                    // 尝试获取属性信息
                    var tagType = e.Node.Tag.GetType();
                    var propertyInfo = tagType.GetProperty("Property");
                    
                    if (propertyInfo != null)
                    {
                        var propertyName = propertyInfo.GetValue(e.Node.Tag)?.ToString();
                        if (!string.IsNullOrEmpty(propertyName))
                        {
                            var configType = _currentConfig.GetType();
                            var prop = configType.GetProperty(propertyName);
                            if (prop != null)
                            {
                                var value = prop.GetValue(_currentConfig);
                                textBox1.Text = value?.ToString() ?? "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理TreeView选择事件失败");
                MessageBox.Show($"处理配置选择时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 在TreeView中展开并选中对应配置文件的节点
        /// </summary>
        /// <param name="treeView">目标TreeView控件</param>
        /// <param name="fileName">配置文件名</param>
        private void ExpandAndSelectTreeNode(System.Windows.Forms.TreeView treeView, string fileName)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                if (node.Tag is ConfigInfo configInfo && configInfo.FileName == fileName)
                {
                    // 选中节点并确保可见，但不手动触发事件处理程序以避免循环引用
                    treeView.SelectedNode = node;
                    node.EnsureVisible();
                    break;
                }
            }
        }

        /// <summary>
        /// 配置信息类
        /// </summary>
        private class ConfigInfo
        {
            public string FileName { get; set; }
            public string RootNode { get; set; }
            public Type ConfigType { get; set; }
        }

        

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadConfigurationFile(string fileName, string rootNode, Type configType)
        {
            string configFilePath = System.IO.Path.Combine(basePath, fileName);

            if (!File.Exists(configFilePath))
            {
                _logger?.LogWarning("配置文件不存在，创建默认配置: {FilePath}", configFilePath);
                CreateDefaultConfiguration(configType, configFilePath, rootNode);
            }

            // 读取配置文件
            string json = File.ReadAllText(configFilePath);
            JObject configJson = JObject.Parse(json);

            // 解析配置对象
            if (configJson.TryGetValue(rootNode, out JToken token))
            {
                JObject configJsonObj = token as JObject;
                BaseConfig configObject = configJsonObj.ToObject(configType) as BaseConfig;
                
                if (configObject != null)
                {
                    // 解密配置对象中的敏感信息
                    configObject = _encryptionService.DecryptConfig(configObject);
                    
                    _currentConfig = configObject;
                    _currentConfigFileName = fileName;
                    _currentConfigRootNode = rootNode;

                    // 绑定到UI
                    BindConfigurationToUI(configObject);
                }
                else
                {
                    throw new InvalidOperationException($"无法解析配置对象: {configType.Name}");
                }
            }
            else
            {
                throw new InvalidOperationException($"配置文件格式错误，缺少 {rootNode} 节点");
            }
        }


        /// <summary>
        /// 撤销操作
        /// </summary>
        private void Undo()
        {
            _commandManager.UndoCommand();
        }

        /// <summary>
        /// 重做操作
        /// </summary>
        private void Redo()
        {
            _commandManager.RedoCommand();
        }

        /// <summary>
        /// 显示配置历史记录
        /// </summary>
        private void ShowConfigHistory()
        {
            var historyForm = new Form
            {
                Text = "配置历史记录",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            var listView = new System.Windows.Forms.ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            listView.Columns.Add("时间", 150);
            listView.Columns.Add("操作", 100);
            listView.Columns.Add("描述", 200);
            listView.Columns.Add("配置类型", 120);
            listView.Columns.Add("用户", 150);

            // 只使用本地历史记录
            var allHistory = new List<object>();
            
            // 添加本地历史记录
            foreach (var entry in _configHistory.OrderByDescending(h => h.Timestamp))
            {
                allHistory.Add(new
                {
                    Timestamp = entry.Timestamp,
                    Operation = entry.Operation,
                    Description = entry.Description,
                    ConfigType = entry.ConfigType,
                    User = entry.User
                });
            }
            
            // 去重并按时间排序
            var distinctHistory = allHistory
                .GroupBy(h => new { 
                    Timestamp = ((dynamic)h).Timestamp, 
                    Operation = ((dynamic)h).Operation, 
                    Description = ((dynamic)h).Description 
                })
                .Select(g => g.First())
                .OrderByDescending(h => ((dynamic)h).Timestamp)
                .Take(100); // 限制显示数量
            
            // 添加到ListView
            foreach (var entry in distinctHistory)
            {
                dynamic item = entry;
                var listItem = new ListViewItem(item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                listItem.SubItems.Add(item.Operation);
                listItem.SubItems.Add(item.Description);
                listItem.SubItems.Add(item.ConfigType);
                listItem.SubItems.Add(item.User);
                listView.Items.Add(listItem);
            }

            if (listView.Items.Count == 0)
            {
                listView.Items.Add(new ListViewItem("暂无历史记录"));
            }

            var panel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            var btnExport = new Button { Text = "导出历史记录", Left = 10, Top = 8 };
            var btnClose = new Button { Text = "关闭", Left = 120, Top = 8 };
            
            // 添加版本管理按钮
            var btnVersions = new Button { Text = "管理配置版本", Left = 230, Top = 8 };
            panel.Controls.Add(btnVersions);

            btnExport.Click += (s, e) =>
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "JSON文件|*.json",
                    FileName = $"ConfigHistory_{DateTime.Now:yyyyMMdd_HHmmss}.json"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var historyData = distinctHistory.Select(h => new
                        {
                            ((dynamic)h).Timestamp,
                            ((dynamic)h).Operation,
                            ((dynamic)h).Description,
                            ((dynamic)h).User,
                            ((dynamic)h).ConfigType
                        });

                        File.WriteAllText(saveDialog.FileName, JsonConvert.SerializeObject(historyData, Newtonsoft.Json.Formatting.Indented));
                        MessageBox.Show("历史记录导出成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            
            // 版本管理按钮点击事件
            btnVersions.Click += (s, e) =>
            {
                ShowConfigVersionManager();
            };

            btnClose.Click += (s, e) => historyForm.Close();

            panel.Controls.Add(btnExport);
            panel.Controls.Add(btnClose);
            panel.Controls.Add(btnVersions);

            historyForm.Controls.Add(listView);
            historyForm.Controls.Add(panel);

            historyForm.ShowDialog(this);
        }

        /// <summary>
        /// 验证系统全局配置
        /// </summary>
        /// <param name="configObject">系统全局配置对象</param>
        /// <returns>验证结果</returns>
        private bool ValidateSystemGlobalConfiguration(SystemGlobalconfig configObject)
        {
            var validationResults = new List<string>();

            // 业务逻辑验证
            if (configObject.UseSharedPrinter && string.IsNullOrEmpty(configObject.SomeSetting))
            {
                validationResults.Add("启用共享打印机时，相关设置不能为空");
            }

            // 打印设置验证 - 使用DirectPrinting属性和SomeSetting属性
            if (configObject.DirectPrinting && string.IsNullOrEmpty(configObject.SomeSetting))
            {
                validationResults.Add("启用直接打印时，相关设置不能为空");
            }

            if (validationResults.Count > 0)
            {
                var errorMessage = string.Join("\n", validationResults);
                MessageBox.Show($"系统全局配置验证失败:\n{errorMessage}", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证服务器配置
        /// </summary>
        /// <param name="configObject">服务器配置对象</param>
        /// <returns>验证结果</returns>
        private bool ValidateServerConfiguration(ServerConfig configObject)
        {
            var validationResults = new List<string>();

            // 服务器基础设置验证
            if (configObject.ServerPort < 1 || configObject.ServerPort > 65535)
            {
                validationResults.Add("服务器端口必须在1-65535范围内");
            }

            if (configObject.MaxConnections < 1 || configObject.MaxConnections > 10000)
            {
                validationResults.Add("最大连接数必须在1-10000范围内");
            }

            if (configObject.HeartbeatInterval < 1000 || configObject.HeartbeatInterval > 60000)
            {
                validationResults.Add("心跳间隔必须在1000-60000毫秒范围内");
            }

            // 文件存储路径验证
            if (string.IsNullOrEmpty(configObject.FileStoragePath))
            {
                validationResults.Add("文件存储路径不能为空");
            }
            else if (!Directory.Exists(configObject.FileStoragePath))
            {
                try
                {
                    // 尝试创建目录
                    Directory.CreateDirectory(configObject.FileStoragePath);
                }
                catch
                {
                    validationResults.Add("文件存储路径无效或无法访问");
                }
            }

            // 分类路径验证
            if (string.IsNullOrEmpty(configObject.PaymentVoucherPath))
            {
                validationResults.Add("付款凭证路径不能为空");
            }
            
            if (string.IsNullOrEmpty(configObject.ProductImagePath))
            {
                validationResults.Add("产品图片路径不能为空");
            }
            
            if (string.IsNullOrEmpty(configObject.BOMManualPath))
            {
                validationResults.Add("BOM手册路径不能为空");
            }

            // 最大文件大小验证
            if (configObject.MaxFileSizeMB <= 0 || configObject.MaxFileSizeMB > 1000)
            {
                validationResults.Add("单个文件最大上传大小必须在1-1000MB之间");
            }

            // 数据库连接字符串验证
            if (string.IsNullOrEmpty(configObject.DbConnectionString))
            {
                validationResults.Add("数据库连接字符串不能为空");
            }

            // 日志级别验证
            if (string.IsNullOrEmpty(configObject.LogLevel))
            {
                validationResults.Add("日志级别不能为空");
            }

            if (validationResults.Count > 0)
            {
                var errorMessage = string.Join("\n", validationResults);
                MessageBox.Show($"服务器配置验证失败:\n{errorMessage}", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStates()
        {
            // 使用反射获取命令管理器状态
            var canUndo = CommandManagerExtensions.CanUndo(_commandManager);
            var canRedo = CommandManagerExtensions.CanRedo(_commandManager);

            if (tsbtnUndo != null) tsbtnUndo.Enabled = canUndo;
            if (tsbtnRedo != null) tsbtnRedo.Enabled = canRedo;
        }

        /// <summary>
        /// 绑定配置到UI
        /// </summary>
        /// <param name="configObject">配置对象</param>
        private void BindConfigurationToUI(BaseConfig configObject)
        {
            // 绑定到PropertyGrid
            propertyGrid1.SelectedObject = configObject;
            _currentConfig = configObject;

            // 找到当前配置对应的根节点
            TreeNode rootNode = null;
            foreach (TreeNode node in treeView2.Nodes)
            {
                if (node.Tag is ConfigInfo info && info.ConfigType == configObject.GetType())
                {
                    rootNode = node;
                    break;
                }
            }
            
            // 创建配置详情节点结构
            CreateDetailedConfigurationNodes(configObject, rootNode);

            // 使用通用的UI绑定方法
            if (rootNode != null)
            {
                BindConfigurationToUI(configObject, rootNode);
            }
        }

        /// <summary>
        /// 通用的配置UI绑定方法，通过反射处理所有配置类型
        /// </summary>
        /// <param name="configObject">配置对象</param>
        /// <param name="rootNode">根节点</param>
        private void BindConfigurationToUI(BaseConfig configObject, TreeNode rootNode)
        {
            try
            {
                // 清空子节点
                rootNode.Nodes.Clear();
                
                // 创建节点结构
                CreateConfigurationNodes(configObject, rootNode);
                
                // 展开节点
                rootNode.ExpandAll();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "绑定配置到UI失败");
            }
        }
        
        /// <summary>
        /// 根据配置对象的属性分类创建TreeView节点
        /// </summary>
        /// <param name="configObject">配置对象</param>
        /// <param name="parentNode">父节点</param>
        private void CreateConfigurationNodes(BaseConfig configObject, TreeNode parentNode)
        {
            try
            {
                // 获取配置对象的所有公开属性
                var properties = configObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                
                // 按Category分组属性
                var propertiesByCategory = properties.GroupBy(prop => {
                    var categoryAttr = prop.GetCustomAttributes(typeof(CategoryAttribute), false).FirstOrDefault() as CategoryAttribute;
                    return categoryAttr?.Category ?? "通用设置"; // 如果没有Category属性，使用默认分类
                });
                
                // 为每个分类创建节点
                foreach (var categoryGroup in propertiesByCategory)
                {
                    TreeNode categoryNode = new TreeNode(categoryGroup.Key);
                    
                    // 为分类中的每个属性创建子节点
                    foreach (var property in categoryGroup)
                    {
                        // 获取属性的DisplayName或使用属性名
                        var displayName = GetPropertyDisplayName(property);
                        
                        TreeNode propertyNode = new TreeNode(displayName);
                        // 保存属性信息到Tag
                        propertyNode.Tag = new { Property = property.Name, Category = categoryGroup.Key };
                        
                        categoryNode.Nodes.Add(propertyNode);
                    }
                    
                    parentNode.Nodes.Add(categoryNode);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建配置节点失败");
            }
        }
        
        /// <summary>
        /// 获取属性的DisplayName
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <returns>显示名称</returns>
        private string GetPropertyDisplayName(PropertyInfo property)
        {
            var displayNameAttr = property.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .FirstOrDefault() as DisplayNameAttribute;
            
            return displayNameAttr?.DisplayName ?? property.Name;
        }

        /// <summary>
        /// 使用通用验证方法替换类型特定的验证方法
        /// </summary>
        private bool ValidateConfiguration(BaseConfig config)
        {
            return ValidateConfiguration((object)config);
        }
        
        /// <summary>
        /// 通用配置验证方法
        /// </summary>
        /// <param name="configObject">配置对象</param>
        /// <returns>验证结果</returns>
        private bool ValidateConfiguration(object configObject)
        {
            try
            {
                var validationResults = new List<string>();
                
                // 检查是否为BaseConfig类型
                if (configObject is BaseConfig config)
                {
                    // 首先使用配置验证服务进行验证
                    var serviceValidation = _configValidationService.ValidateConfig(config);
                    if (!serviceValidation.IsValid)
                    {
                        // 获取所有属性错误
                        if (serviceValidation.Errors != null && serviceValidation.Errors.Count > 0)
                        {
                            foreach (var error in serviceValidation.Errors)
                            {
                                validationResults.Add($"{error.Key}: {error.Value}");
                            }
                        }
                        
                        // 获取所有全局错误
                        if (serviceValidation.GlobalErrors != null && serviceValidation.GlobalErrors.Count > 0)
                        {
                            validationResults.AddRange(serviceValidation.GlobalErrors);
                        }
                    }
                    
                    // 使用反射获取配置对象的属性并根据特性进行验证
                    var properties = config.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        // 获取属性值
                        var value = property.GetValue(config);
                        
                        // 执行通用验证
                        ValidateProperty(property, value, validationResults);
                    }
                }
                
                // 如果有验证错误，显示错误消息
                if (validationResults.Count > 0)
                {
                    var errorMessage = string.Join("\n", validationResults);
                    MessageBox.Show($"配置验证失败:\n{errorMessage}", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证配置失败");
                MessageBox.Show($"验证配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        /// <summary>
        /// 对单个属性进行验证
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="value">属性值</param>
        /// <param name="validationResults">验证结果列表</param>
        private void ValidateProperty(PropertyInfo property, object value, List<string> validationResults)
        {
            // 检查必填字段
            if (property.IsDefined(typeof(RequiredAttribute), false) && (value == null || (value is string && string.IsNullOrEmpty((string)value))))
            {
                var displayName = GetPropertyDisplayName(property);
                validationResults.Add($"{displayName} 是必填项");
            }
            
            // 检查范围约束
            if (value != null && property.IsDefined(typeof(RangeAttribute), false))
            {
                var rangeAttr = property.GetCustomAttribute<RangeAttribute>();
                if (rangeAttr != null)
                {
                    // 只对数值类型进行范围验证
                    if (value is IConvertible)
                    {
                        try
                        {
                            decimal numericValue = Convert.ToDecimal(value);
                            if (numericValue < Convert.ToDecimal(rangeAttr.Minimum) || numericValue > Convert.ToDecimal(rangeAttr.Maximum))
                            {
                                var displayName = GetPropertyDisplayName(property);
                                validationResults.Add($"{displayName} 必须在 {rangeAttr.Minimum} 到 {rangeAttr.Maximum} 之间");
                            }
                        }
                        catch { }
                    }
                }
            }
            
            // 检查字符串长度
            if (value is string stringValue && property.IsDefined(typeof(StringLengthAttribute), false))
            {
                var lengthAttr = property.GetCustomAttribute<StringLengthAttribute>();
                if (lengthAttr != null && stringValue.Length > lengthAttr.MaximumLength)
                {
                    var displayName = GetPropertyDisplayName(property);
                    validationResults.Add($"{displayName} 的长度不能超过 {lengthAttr.MaximumLength} 个字符");
                }
            }
            
            // 文件路径验证
            if (value is string pathValue && property.Name.Contains("Path"))
            {
                if (!string.IsNullOrEmpty(pathValue))
                {
                    try
                    {
                        // 检查路径是否有效
                        var directoryPath = Path.GetDirectoryName(pathValue);
                        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                        {
                            try
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            catch
                            {
                                var displayName = GetPropertyDisplayName(property);
                                validationResults.Add($"{displayName} 路径无效或无法访问: {pathValue}");
                            }
                        }
                    }
                    catch
                    {
                        var displayName = GetPropertyDisplayName(property);
                        validationResults.Add($"{displayName} 包含无效的路径: {pathValue}");
                    }
                }
            }
        }

        /// <summary>
        /// 创建详细配置节点结构
        /// </summary>
        /// <param name="configObject">配置对象</param>
        /// <param name="parentNode">父节点</param>
        private void CreateDetailedConfigurationNodes(BaseConfig configObject, TreeNode parentNode)
        {
            if (parentNode == null || configObject == null)
                return;

            // 清除现有子节点但保留根节点
            parentNode.Nodes.Clear();

            // 使用通用节点创建方法生成详细配置结构
            CreateConfigurationNodes(configObject, parentNode);

            // 展开节点以显示详细信息
            parentNode.Expand();
        }
        /// <summary>
        /// 创建默认配置 - 使用反射方式
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="filePath">配置文件路径</param>
        /// <param name="rootNode">根节点名称</param>
        private void CreateDefaultConfiguration(Type configType, string filePath, string rootNode)
        {
            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            try
            {
                // 使用反射创建配置实例
                BaseConfig configInstance = CreateDefaultConfigInstance(configType);
                
                // 使用反射设置默认属性值
                SetDefaultPropertyValues(configInstance, configType);
                
                // 转换为JSON
                JObject configJson = JObject.FromObject(configInstance);
                
                var fullConfig = new JObject(new JProperty(rootNode, configJson));
                File.WriteAllText(filePath, fullConfig.ToString(Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"创建默认配置失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 使用反射创建配置实例
        /// </summary>
        private BaseConfig CreateDefaultConfigInstance(Type configType)
        {
            // 验证类型是否为BaseConfig的派生类
            if (!typeof(BaseConfig).IsAssignableFrom(configType))
            {
                throw new ArgumentException($"类型 {configType.Name} 不是 BaseConfig 的有效派生类");
            }
            
            try
            {
                return (BaseConfig)Activator.CreateInstance(configType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"无法创建类型 {configType.Name} 的实例: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 使用反射设置配置对象的默认属性值
        /// </summary>
        /// <param name="configObject">配置对象</param>
        /// <param name="configType">配置类型</param>
        private void SetDefaultPropertyValues(BaseConfig configObject, Type configType)
        {
            // 获取所有公共实例属性
            PropertyInfo[] properties = configType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (PropertyInfo property in properties)
            {
                // 检查是否可写
                if (!property.CanWrite)
                    continue;
                
                // 尝试设置默认值
                object defaultValue = null;
                
                // 对于特定配置类型的特定属性，设置自定义默认值
                if (configType == typeof(ServerConfig))
                {
                    defaultValue = GetServerConfigDefaultValue(property.Name, property.PropertyType);
                }
                else if (configType == typeof(SystemGlobalconfig))
                {
                    defaultValue = GetSystemGlobalConfigDefaultValue(property.Name, property.PropertyType);
                }
                else if (configType == typeof(GlobalValidatorConfig))
                {
                    defaultValue = GetGlobalValidatorConfigDefaultValue(property.Name, property.PropertyType);
                }
                
                // 如果找到了合适的默认值，设置它
                if (defaultValue != null)
                {
                    property.SetValue(configObject, defaultValue);
                }
            }
        }
        
        /// <summary>
        /// 获取服务器配置特定属性的默认值
        /// </summary>
        private object GetServerConfigDefaultValue(string propertyName, Type propertyType)
        {
            switch (propertyName)
            {
                case nameof(ServerConfig.ServerName):
                    return "RUINORERP Server";
                case nameof(ServerConfig.ServerPort):
                    return 8080;
                case nameof(ServerConfig.MaxConnections):
                    return 100;
                case nameof(ServerConfig.HeartbeatInterval):
                    return 30000;
                case nameof(ServerConfig.DbType):
                    return "MySql";
                case nameof(ServerConfig.DbConnectionString):
                    return "Server=localhost;Database=RUINORERP;Uid=root;Pwd=123456;";
                case nameof(ServerConfig.CacheType):
                    return "Memory";
                case nameof(ServerConfig.CacheConnectionString):
                    return "";
                case nameof(ServerConfig.LogLevel):
                    return "Info";
                case nameof(ServerConfig.EnableLogging):
                    return true;
                case nameof(ServerConfig.FileStoragePath):
                    return "D:\\RUINORERP\\FileStorage";
                case nameof(ServerConfig.MaxFileSizeMB):
                    return 10;
                case nameof(ServerConfig.PaymentVoucherPath):
                    return "PaymentVouchers";
                case nameof(ServerConfig.ProductImagePath):
                    return "ProductImages";
                case nameof(ServerConfig.BOMManualPath):
                    return "BOMManuals";
                default:
                    return GetDefaultValueForType(propertyType);
            }
        }
        
        /// <summary>
        /// 获取系统全局配置特定属性的默认值
        /// </summary>
        private object GetSystemGlobalConfigDefaultValue(string propertyName, Type propertyType)
        {
            switch (propertyName)
            {
                case nameof(SystemGlobalconfig.UseSharedPrinter):
                case "采购日期必填":
                case nameof(SystemGlobalconfig.IsFromPlatform):
                    return false;
                case nameof(SystemGlobalconfig.SomeSetting):
                    return "";
                case nameof(SystemGlobalconfig.OpenProdTypeForSaleCheck):
                case nameof(SystemGlobalconfig.DirectPrinting):
                    return true;
                default:
                    return GetDefaultValueForType(propertyType);
            }
        }
        
        /// <summary>
        /// 获取全局验证配置特定属性的默认值
        /// </summary>
        private object GetGlobalValidatorConfigDefaultValue(string propertyName, Type propertyType)
        {
            switch (propertyName)
            {
                case "预交日期必填":
                case "预开工日期必填":
                case "预完工日期必填":
                case "借出单的接收单位必填":
                    return false;
                case nameof(GlobalValidatorConfig.ReworkTipDays):
                    return 3;
                case "计划提前提示天数":
                    return 1;
                case nameof(GlobalValidatorConfig.MoneyDataPrecision):
                    return 4;
                case nameof(GlobalValidatorConfig.IsFromPlatform):
                case nameof(GlobalValidatorConfig.NeedInputProjectGroup):
                    return true;
                case nameof(GlobalValidatorConfig.SomeSetting):
                    return "";
                default:
                    return GetDefaultValueForType(propertyType);
            }
        }
        
        /// <summary>
        /// 获取类型的默认值
        /// </summary>
        private object GetDefaultValueForType(Type propertyType)
        {
            if (propertyType.IsValueType)
            {
                return Activator.CreateInstance(propertyType);
            }
            return null;
        }

        /// <summary>
        /// 创建后备配置
        /// </summary>
        private void CreateFallbackConfiguration()
        {
            try
            {
                // 创建系统全局后备配置
                var fallbackConfig = new SystemGlobalconfig
                {
                    UseSharedPrinter = false,
                    SomeSetting = "",
                    采购日期必填 = false,
                    IsFromPlatform = false,
                    OpenProdTypeForSaleCheck = true,
                    DirectPrinting = true
                };

                BindConfigurationToUI(fallbackConfig);
                MessageBox.Show("使用默认系统全局配置启动，请检查配置文件后重新加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建后备配置失败");
                MessageBox.Show("无法加载配置，系统将使用最小化配置运行", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        // 已移除treeView1_AfterSelect方法，功能已整合到treeView2_AfterSelect

        /// <summary>
        /// PropertyGrid属性改变事件
        /// </summary>
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                // 处理BaseConfig类型及其子类的属性变更
                if (propertyGrid1.SelectedObject is BaseConfig config)
                {
                    // 创建新命令并执行
                    var command = new ChangeConfigCommand(config, e.ChangedItem.Label, e.OldValue, e.ChangedItem.Value);
                    _commandManager.ExecuteCommand(command);
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "属性值更改处理失败");
                MessageBox.Show($"处理属性更改时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 文本框文本改变事件
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (treeView2.SelectedNode?.Tag != null && _currentConfig != null)
            {
                // 尝试获取属性信息
                var tagType = treeView2.SelectedNode.Tag.GetType();
                var propertyInfo = tagType.GetProperty("Property");
                
                if (propertyInfo != null)
                {
                    var propertyName = propertyInfo.GetValue(treeView2.SelectedNode.Tag)?.ToString();
                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        var configType = _currentConfig.GetType();
                        var prop = configType.GetProperty(propertyName);
                        if (prop != null && prop.CanWrite)
                        {
                            try
                            {
                                var convertedValue = Convert.ChangeType(textBox1.Text, prop.PropertyType);
                                prop.SetValue(_currentConfig, convertedValue);
                                propertyGrid1.Refresh();
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogWarning(ex, "属性值转换失败: {PropertyName} = {Value}", propertyName, textBox1.Text);
                                MessageBox.Show($"属性值转换失败: {propertyName} = {textBox1.Text}\n错误: {ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        /// <summary>
        /// 撤销按钮点击事件
        /// </summary>
        private void tsbtnUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        /// <summary>
        /// 重做按钮点击事件
        /// </summary>
        private void tsbtnRedo_Click(object sender, EventArgs e)
        {
            Redo();
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void tsbtnRefresh_Click(object sender, EventArgs e)
        {
            // 刷新TreeView和当前配置
            InitializeJsonFilesTreeView();
            if (!string.IsNullOrEmpty(_currentConfigFileName))
            {
                // 重新加载当前配置文件
                var metadata = _configMetadataCache.Values.FirstOrDefault(m => m.FileName == _currentConfigFileName);
                if (metadata != null)
                {
                    LoadConfigurationFile(metadata.FileName, metadata.RootNode, metadata.ConfigType);
                }
            }
        }

        /// <summary>
        /// 历史记录按钮点击事件
        /// </summary>
        private void tsbtnHistory_Click(object sender, EventArgs e)
        {
            ShowConfigHistory();
        }
        
        /// <summary>
        /// 显示配置版本管理器
        /// </summary>
        private void ShowConfigVersionManager()
        {
            if (string.IsNullOrEmpty(_currentConfigFileName))
            {
                MessageBox.Show("请先选择要管理版本的配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var versionForm = new Form
            {
                Text = $"配置版本管理 - {_currentConfigFileName}",
                Size = new Size(850, 600),
                StartPosition = FormStartPosition.CenterParent
            };
            
            var listView = new System.Windows.Forms.ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            
            listView.Columns.Add("版本号", 80);
            listView.Columns.Add("创建时间", 150);
            listView.Columns.Add("描述", 250);
            listView.Columns.Add("创建者", 120);
            listView.Columns.Add("状态", 80);
            
            // 加载版本列表
            try
            {
                var versions = _versionService.GetVersions(_currentConfigFileName);
                foreach (var version in versions.OrderByDescending(v => v.VersionNumber))
                {
                    var item = new ListViewItem(version.VersionNumber.ToString());
                    item.SubItems.Add(version.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(version.Description);
                    item.SubItems.Add(version.IsActive ? "当前" : "历史");
                    item.Tag = version;
                    listView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "加载配置版本失败");
                MessageBox.Show($"加载配置版本失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            var panel = new Panel { Dock = DockStyle.Bottom, Height = 60 };
            
            var btnCreateVersion = new Button { Text = "创建新版本", Left = 10, Top = 15 };
            var btnRollback = new Button { Text = "回滚到此版本", Left = 120, Top = 15 };
            var btnCompare = new Button { Text = "比较版本", Left = 240, Top = 15 };
            var btnDelete = new Button { Text = "删除版本", Left = 350, Top = 15 };
            var btnClose = new Button { Text = "关闭", Left = 460, Top = 15 };
            
            // 创建新版本按钮点击事件
            btnCreateVersion.Click += (s, e) =>
            {
                try
                {
                    var descriptionForm = new Form
                    {
                        Text = "创建新版本",
                        Size = new Size(400, 200),
                        StartPosition = FormStartPosition.CenterParent
                    };
                    
                    var label = new Label { Text = "版本描述:", Left = 20, Top = 20 };
                    var textBox = new TextBox { Left = 20, Top = 40, Width = 340, Multiline = true, Height = 60 };
                    var okButton = new Button { Text = "确定", Left = 150, Top = 110 };
                    var cancelButton = new Button { Text = "取消", Left = 240, Top = 110 };
                    
                    okButton.Click += (s2, e2) =>
                    {
                        try
                        {
                            if (_currentConfig != null)
                            {
                                string versionDesc = textBox.Text.Trim();
                                if (string.IsNullOrEmpty(versionDesc))
                                {
                                    versionDesc = "手动创建版本";
                                }
                                
                                _versionService.CreateVersion(_currentConfig, _currentConfigFileName, versionDesc);
                                
                                // 刷新版本列表
                                listView.Items.Clear();
                                var versions = _versionService.GetVersions(_currentConfigFileName);
                                foreach (var version in versions.OrderByDescending(v => v.VersionNumber))
                                {
                                    var item = new ListViewItem(version.VersionNumber.ToString());
                                    item.SubItems.Add(version.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                    item.SubItems.Add(version.Description);
                                    item.SubItems.Add(version.IsActive ? "当前" : "历史");
                                    item.Tag = version;
                                    listView.Items.Add(item);
                                }
                                
                                MessageBox.Show("配置版本创建成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                descriptionForm.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "创建配置版本失败");
                            MessageBox.Show($"创建配置版本失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                    
                    cancelButton.Click += (s2, e2) => descriptionForm.Close();
                    
                    descriptionForm.Controls.Add(label);
                    descriptionForm.Controls.Add(textBox);
                    descriptionForm.Controls.Add(okButton);
                    descriptionForm.Controls.Add(cancelButton);
                    
                    descriptionForm.ShowDialog(versionForm);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "创建版本对话框失败");
                }
            };
            
            // 回滚按钮点击事件
            btnRollback.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要回滚的版本", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                var selectedVersion = listView.SelectedItems[0].Tag as ConfigVersion;
                if (selectedVersion == null)
                    return;
                
                if (selectedVersion.IsActive)
                {
                    MessageBox.Show("当前已是选中的版本，无需回滚", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                if (MessageBox.Show($"确定要回滚到版本 {selectedVersion.VersionNumber} 吗？\n此操作将覆盖当前配置。", 
                    "确认回滚", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        // 回滚到选定版本
                        bool rollbackSuccess = _versionService.RollbackToVersion(selectedVersion.VersionId);
                        
                        if (rollbackSuccess)
                        {
                            // 重新加载配置
                            _currentConfig = _configManagerService.GetConfig<BaseConfig>(_currentConfigFileName);
                            
                            // 重新加载配置到UI
                            BindConfigurationToUI(_currentConfig);
                            
                            MessageBox.Show("配置回滚成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("配置回滚失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                        // 添加到本地历史记录，不使用审计日志服务
                        var historyEntry = new ConfigHistoryEntry(_currentConfig, $"回滚到版本 {selectedVersion.VersionNumber}")
                        {
                            Operation = "配置回滚",
                            ConfigSnapshot = JObject.FromObject(_currentConfig)
                        };
                        _configHistory.Add(historyEntry);
                        
                        // 刷新版本列表
                        listView.Items.Clear();
                        var versions = _versionService.GetVersions(_currentConfigFileName);
                        foreach (var version in versions.OrderByDescending(v => v.VersionNumber))
                        {
                            var item = new ListViewItem(version.VersionNumber.ToString());
                            item.SubItems.Add(version.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            item.SubItems.Add(version.Description);
                            item.SubItems.Add(version.IsActive ? "当前" : "历史");
                            item.Tag = version;
                            listView.Items.Add(item);
                        }
                        
                        MessageBox.Show("配置已成功回滚", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "回滚配置版本失败");
                        MessageBox.Show($"回滚配置版本失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            
            // 比较版本按钮点击事件
            btnCompare.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count < 2)
                {
                    MessageBox.Show("请选择两个版本进行比较", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                if (listView.SelectedItems.Count > 2)
                {
                    MessageBox.Show("一次只能比较两个版本", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                var version1 = listView.SelectedItems[0].Tag as ConfigVersion;
                var version2 = listView.SelectedItems[1].Tag as ConfigVersion;
                
                if (version1 == null || version2 == null)
                    return;
                
                try
                {
                    var diffResult = _versionService.CompareVersionsDetailed(version1.VersionId, version2.VersionId);
                    
                    // 显示差异结果
                    var diffForm = new Form
                    {
                        Text = $"版本比较 - v{version1.VersionNumber} vs v{version2.VersionNumber}",
                        Size = new Size(800, 600),
                        StartPosition = FormStartPosition.CenterParent
                    };
                    
                    var diffTextBox = new TextBox
                    {
                        Dock = DockStyle.Fill,
                        Multiline = true,
                        ReadOnly = true,
                        ScrollBars = ScrollBars.Both,
                        Font = new Font("Consolas", 9)
                    };
                    
                    // 格式化差异结果
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"版本比较: v{version1.VersionNumber} ({version1.CreatedTime:yyyy-MM-dd HH:mm:ss}) vs v{version2.VersionNumber} ({version2.CreatedTime:yyyy-MM-dd HH:mm:ss})\n");
                    
                    if (diffResult.AddedProperties.Count > 0)
                    {
                        sb.AppendLine("新增属性:");
                        foreach (var prop in diffResult.AddedProperties)
                        {
                            sb.AppendLine($"  + {prop.Key}: {prop.Value}");
                        }
                        sb.AppendLine();
                    }
                    
                    if (diffResult.RemovedProperties.Count > 0)
                    {
                        sb.AppendLine("删除属性:");
                        foreach (var prop in diffResult.RemovedProperties)
                        {
                            sb.AppendLine($"  - {prop.Key}: {prop.Value}");
                        }
                        sb.AppendLine();
                    }
                    
                    if (diffResult.ModifiedProperties.Count > 0)
                    {
                        sb.AppendLine("修改属性:");
                        foreach (var prop in diffResult.ModifiedProperties)
                        {
                            sb.AppendLine($"  * {prop.Key}:");
                            sb.AppendLine($"    旧值: {prop.Value.OldValue}");
                            sb.AppendLine($"    新值: {prop.Value.NewValue}");
                        }
                        sb.AppendLine();
                    }
                    
                    if (diffResult.AddedProperties.Count == 0 && diffResult.RemovedProperties.Count == 0 && diffResult.ModifiedProperties.Count == 0)
                    {
                        sb.AppendLine("两个版本的配置完全相同");
                    }
                    
                    diffTextBox.Text = sb.ToString();
                    diffForm.Controls.Add(diffTextBox);
                    diffForm.ShowDialog(versionForm);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "比较配置版本失败");
                    MessageBox.Show($"比较配置版本失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            
            // 删除版本按钮点击事件
            btnDelete.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要删除的版本", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                var selectedVersion = listView.SelectedItems[0].Tag as ConfigVersion;
                if (selectedVersion == null)
                    return;
                
                if (selectedVersion.IsActive)
                {
                    MessageBox.Show("不能删除当前活动的版本", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                if (MessageBox.Show($"确定要删除版本 {selectedVersion.VersionNumber} 吗？\n此操作不可撤销。", 
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        _versionService.DeleteVersion(selectedVersion.VersionId);
                        
                        // 从列表中移除
                        listView.Items.Remove(listView.SelectedItems[0]);
                        
                        // 添加到历史记录，不使用审计日志服务
                        var historyEntry = new ConfigHistoryEntry(_currentConfig, $"删除版本 {selectedVersion.VersionNumber}")
                        {
                            Operation = "删除版本",
                            ConfigSnapshot = JObject.FromObject(_currentConfig)
                        };
                        _configHistory.Add(historyEntry);
                        
                        MessageBox.Show("配置版本已成功删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "删除配置版本失败");
                        MessageBox.Show($"删除配置版本失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            
            btnClose.Click += (s, e) => versionForm.Close();
            
            panel.Controls.Add(btnCreateVersion);
            panel.Controls.Add(btnRollback);
            panel.Controls.Add(btnCompare);
            panel.Controls.Add(btnDelete);
            panel.Controls.Add(btnClose);
            
            versionForm.Controls.Add(listView);
            versionForm.Controls.Add(panel);
            
            versionForm.ShowDialog(this);
        }
        
        /// <summary>
        /// 版本管理按钮点击事件
        /// </summary>
        private void tsbtnVersionManager_Click(object sender, EventArgs e)
        {
            ShowConfigVersionManager();
        }

        /// <summary>
        /// 配置历史记录条目
        /// </summary>
        public class ConfigHistoryEntry
        {
            public DateTime Timestamp { get; set; } = DateTime.Now;
            public string Operation { get; set; }
            public string Description { get; set; }
            public string User { get; set; }
            public JObject ConfigSnapshot { get; set; }
            public string ConfigType { get; set; }

            public ConfigHistoryEntry(BaseConfig config, string changeDescription)
            {
                Timestamp = DateTime.Now;
                ConfigSnapshot = JObject.FromObject(config);
                Description = changeDescription;
                ConfigType = config.GetType().Name;
                User = Environment.UserName;
                Operation = "修改配置";
            }
        }

        /// <summary>
        /// 配置历史记录管理器
        /// </summary>
        public class ConfigHistoryManager
        {
            private readonly List<ConfigHistoryEntry> _historyEntries;

            public ConfigHistoryManager()
            {
                _historyEntries = new List<ConfigHistoryEntry>();
            }

            /// <summary>
            /// 添加历史记录
            /// </summary>
            public void AddHistoryEntry(ConfigHistoryEntry entry)
            {
                if (entry != null)
                {
                    _historyEntries.Add(entry);
                }
            }

            /// <summary>
            /// 获取所有历史记录
            /// </summary>
            public IReadOnlyList<ConfigHistoryEntry> GetAllHistoryEntries()
            {
                return _historyEntries.AsReadOnly();
            }

            /// <summary>
            /// 清除历史记录
            /// </summary>
            public void ClearHistory()
            {
                _historyEntries.Clear();
            }

            /// <summary>
            /// 按日期范围获取历史记录
            /// </summary>
            public IReadOnlyList<ConfigHistoryEntry> GetHistoryEntriesByDateRange(DateTime startDate, DateTime endDate)
            {
                return _historyEntries.Where(h => h.Timestamp >= startDate && h.Timestamp <= endDate).ToList().AsReadOnly();
            }

            /// <summary>
            /// 配置更改命令
            /// </summary>
            public class ChangeConfigCommand : ICommand
        {
            private readonly BaseConfig _config;
            private readonly string _propertyName;
            private readonly object _oldValue;
            private readonly object _newValue;

            public ChangeConfigCommand(BaseConfig config, string propertyName, object oldValue, object newValue)
            {
                _config = config;
                _propertyName = propertyName;
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public void Execute()
            {
                var prop = _config.GetType().GetProperty(_propertyName);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(_config, _newValue);
                }
            }

            public void Undo()
            {
                var prop = _config.GetType().GetProperty(_propertyName);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(_config, _oldValue);
                }
            }
        }

            /// <summary>
            /// 命令管理器扩展
            /// </summary>
            public static class CommandManagerExtensions
            {
                /// <summary>
                /// 检查是否可以撤销
                /// </summary>
                public static bool CanUndo(CommandManager commandManager)
                {
                    if (commandManager == null) return false;

                    try
                    {
                        var undoStackField = typeof(CommandManager).GetField("_undoStack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (undoStackField != null)
                        {
                            var undoStack = undoStackField.GetValue(commandManager) as System.Collections.IEnumerable;
                            return undoStack != null && undoStack.Cast<object>().Any();
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录日志但不抛出异常
                        Console.WriteLine($"检查撤销状态失败: {ex.Message}");
                    }

                    return false;
                }

                /// <summary>
                /// 检查是否可以重做
                /// </summary>
                public static bool CanRedo(CommandManager commandManager)
                {
                    if (commandManager == null) return false;

                    try
                    {
                        var redoStackField = typeof(CommandManager).GetField("_redoStack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (redoStackField != null)
                        {
                            var redoStack = redoStackField.GetValue(commandManager) as System.Collections.IEnumerable;
                            return redoStack != null && redoStack.Cast<object>().Any();
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录日志但不抛出异常
                        Console.WriteLine($"检查重做状态失败: {ex.Message}");
                    }

                    return false;
                }
            }
        }
    }
}