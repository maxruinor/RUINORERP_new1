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
using RUINORERP.IServices;
using TextBox = System.Windows.Forms.TextBox;
using System.ComponentModel.DataAnnotations;
using RUINORERP.Business.Config;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Services;
using static RUINORERP.Server.Controls.GlobalConfigControl.ConfigHistoryManager;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.Models;
using Formatting = Newtonsoft.Json.Formatting;
using RUINORERP.PacketSpec.Models.Requests;

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
        private readonly IGeneralBroadcastService _generalBroadcastService;

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
            _generalBroadcastService = Startup.GetFromFac<IGeneralBroadcastService>();

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
                RegisterConfigType(typeof(SystemGlobalConfig), "SystemGlobalConfig.json", "SystemGlobalConfig");
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
                    _logger?.LogWarning("没有可保存的配置对象");
                    // 获取主窗体实例并打印信息日志
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintInfoLog("没有可保存的配置对象");
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

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "配置保存失败");
                _logger?.LogError(ex, "配置保存失败");
                // 获取主窗体实例并打印错误日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintErrorLog($"保存配置时发生错误: {ex.Message}");
            }
        }


        /// <summary>
        /// 获取配置描述
        /// </summary>
        private string GetConfigDescription(BaseConfig config)
        {
            if (config is ServerConfig)
                return "服务器配置更新";
            else if (config is SystemGlobalConfig)
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
                _logger?.LogWarning($"全局验证配置验证失败: {errorMessage}");
                // 获取主窗体实例并打印信息日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintInfoLog($"全局验证配置验证失败: {errorMessage}");
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
                    node.Tag = new ConfigInfo
                    {
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
                _logger?.LogError(ex, "初始化JSON文件TreeView失败");
                // 获取主窗体实例并打印错误日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintErrorLog($"初始化JSON文件树时发生错误: {ex.Message}");
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
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理TreeView选择事件失败");
                _logger?.LogError(ex, "处理TreeView选择事件失败");
                // 获取主窗体实例并打印错误日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintErrorLog($"处理配置选择时发生错误: {ex.Message}");
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
        /// <summary>
        /// 广播配置变更给所有客户端
        /// </summary>
        private bool BroadcastConfigChange(BaseConfig config)
        {
            try
            {
                string configData = JsonConvert.SerializeObject(config, Formatting.Indented);
                // 序列化当前配置为JSON
                string configType = _currentConfig.GetType().Name;

                // 创建通用请求
                var request = new GeneralRequest
                {
                    Data = new
                    {
                        ConfigType = configType,
                        ConfigData = configData,
                        Version = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        ForceApply = true
                    }
                };

                // 调用通用广播服务
                _generalBroadcastService?.BroadcastToAllClients(GeneralCommands.ConfigSync, request);

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "广播数据失败");
                // 广播失败不影响配置保存，只记录错误
                return false;
            }
        }




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
                .GroupBy(h => new
                {
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
                        _logger?.LogInformation("历史记录导出成功");
                        // 获取主窗体实例并打印信息日志
                        var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                        mainForm?.PrintInfoLog("历史记录导出成功");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "导出历史记录失败");
                        // 获取主窗体实例并打印错误日志
                        var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                        mainForm?.PrintErrorLog($"导出失败: {ex.Message}");
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
        private bool ValidateSystemGlobalConfiguration(SystemGlobalConfig configObject)
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
                _logger?.LogWarning($"系统全局配置验证失败: {errorMessage}");
                // 获取主窗体实例并打印信息日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintInfoLog($"系统全局配置验证失败: {errorMessage}");
                return false;
            }

            return true;
        }

        // 注意：以下特定类型的验证方法已被移除，因为所有验证逻辑现在都在FluentValidation验证器中实现
        // 保留此注释以表明这些方法不再使用，并且验证已移至ServerConfigValidator、SystemGlobalconfigValidator和GlobalValidatorConfigValidator中

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
                var propertiesByCategory = properties.GroupBy(prop =>
                {
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
        /// 使用基于FluentValidation的验证方法
        /// 所有验证逻辑已移至FluentValidation验证器中实现
        /// </summary>
        private bool ValidateConfiguration(BaseConfig config)
        {
            try
            {
                // 完全依赖配置验证服务进行验证（现在使用FluentValidation）
                var validationResult = _configValidationService.ValidateConfig(config);

                // 检查验证结果
                if (!validationResult.IsValid)
                {
                    // 获取完整的错误信息
                    string errorMessage = validationResult.GetErrorMessage();
                    _logger?.LogWarning($"配置验证失败: {errorMessage}");
                    // 获取主窗体实例并打印信息日志
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintInfoLog($"配置验证失败: {errorMessage}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证配置失败");
                _logger?.LogError(ex, "验证配置失败");
                // 获取主窗体实例并打印错误日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintErrorLog($"验证配置时发生错误: {ex.Message}");
                return false;
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
                else if (configType == typeof(SystemGlobalConfig))
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
                case nameof(SystemGlobalConfig.UseSharedPrinter):
                case "采购日期必填":
                case nameof(SystemGlobalConfig.IsFromPlatform):
                    return false;
                case nameof(SystemGlobalConfig.SomeSetting):
                    return "";
                case nameof(SystemGlobalConfig.OpenProdTypeForSaleCheck):
                case nameof(SystemGlobalConfig.DirectPrinting):
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
                var fallbackConfig = new SystemGlobalConfig
                {
                    UseSharedPrinter = false,
                    SomeSetting = "",
                    采购日期必填 = false,
                    IsFromPlatform = false,
                    OpenProdTypeForSaleCheck = true,
                    DirectPrinting = true
                };

                BindConfigurationToUI(fallbackConfig);
                _logger?.LogWarning("使用默认系统全局配置启动，请检查配置文件后重新加载");
                // 获取主窗体实例并打印信息日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintInfoLog("使用默认系统全局配置启动，请检查配置文件后重新加载");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建后备配置失败");
                // 获取主窗体实例并打印错误日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintErrorLog("无法加载配置，系统将使用最小化配置运行");
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
                // 获取主窗体实例并打印错误日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintErrorLog($"处理属性更改时发生错误: {ex.Message}");
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
                _logger?.LogInformation("请先选择要管理版本的配置文件");
                // 获取主窗体实例并打印信息日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintInfoLog("请先选择要管理版本的配置文件");
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
                // 获取主窗体实例并打印错误日志
                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                mainForm?.PrintErrorLog($"加载配置版本失败: {ex.Message}");
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

                                _logger?.LogInformation("配置版本创建成功");
                                // 获取主窗体实例并打印信息日志
                                var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                                mainForm?.PrintInfoLog("配置版本创建成功");
                                descriptionForm.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "创建配置版本失败");
                            // 获取主窗体实例并打印错误日志
                            var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                            mainForm?.PrintErrorLog($"创建配置版本失败: {ex.Message}");
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
                    _logger?.LogInformation("请选择要回滚的版本");
                    // 使用单例方式获取主窗体并打印信息日志
                    frmMainNew.Instance?.PrintInfoLog("请选择要回滚的版本");
                    return;
                }

                var selectedVersion = listView.SelectedItems[0].Tag as ConfigVersion;
                if (selectedVersion == null)
                    return;

                if (selectedVersion.IsActive)
                {
                    _logger?.LogInformation("当前已是选中的版本，无需回滚");
                    // 获取主窗体实例并打印信息日志
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintInfoLog("当前已是选中的版本，无需回滚");
                    return;
                }

                // 使用单例方式获取主窗体并打印确认信息
                frmMainNew.Instance?.PrintInfoLog($"用户正在考虑回滚到版本 {selectedVersion.VersionNumber}，此操作将覆盖当前配置。");

                // 注意：此处保留MessageBox用于确认操作，因为这是需要用户显式确认的关键操作
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

                            _logger?.LogInformation("配置回滚成功");
                            // 使用单例方式获取主窗体并打印信息日志
                            frmMainNew.Instance?.PrintInfoLog("配置回滚成功");
                        }
                        else
                        {
                            _logger?.LogError("配置回滚失败");
                            // 使用单例方式获取主窗体并打印错误日志
                            frmMainNew.Instance?.PrintErrorLog("配置回滚失败");
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

                        _logger?.LogInformation("配置已成功回滚");
                        // 使用单例方式获取主窗体并打印信息日志
                        frmMainNew.Instance?.PrintInfoLog("配置已成功回滚");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "回滚配置版本失败");
                        // 使用单例方式获取主窗体并打印错误日志
                        frmMainNew.Instance?.PrintErrorLog($"回滚配置版本失败: {ex.Message}");
                    }
                }
            };

            // 比较版本按钮点击事件
            btnCompare.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count < 2)
                {
                    _logger?.LogInformation("请选择两个版本进行比较");
                    // 获取主窗体实例并打印信息日志
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintInfoLog("请选择两个版本进行比较");
                    return;
                }

                if (listView.SelectedItems.Count > 2)
                {
                    _logger?.LogInformation("一次只能比较两个版本");
                    // 获取主窗体实例并打印信息日志
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintInfoLog("一次只能比较两个版本");
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
                    // 获取主窗体实例并打印错误日志
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintErrorLog($"比较配置版本失败: {ex.Message}");
                }
            };

            // 删除版本按钮点击事件
            btnDelete.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count == 0)
                {
                    _logger?.LogInformation("请选择要删除的版本");
                    // 获取主窗体实例并打印信息日志
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintInfoLog("请选择要删除的版本");
                    return;
                }

                var selectedVersion = listView.SelectedItems[0].Tag as ConfigVersion;
                if (selectedVersion == null)
                    return;

                if (selectedVersion.IsActive)
                {
                    _logger?.LogInformation("不能删除当前活动的版本");
                    // 获取主窗体实例并打印信息日志
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintInfoLog("不能删除当前活动的版本");
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
        /// 发布按钮点击事件
        /// </summary>
        private void tsbtnPublish_Click(object sender, EventArgs e)
        {
            if (_currentConfig == null)
            {
                MessageBox.Show("请先选择要发布的配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 验证当前配置
                if (!ValidateConfiguration(_currentConfig))
                {
                    MessageBox.Show("配置验证失败，请检查配置项后重试", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 发布配置前先保存
                SaveConfig();

                // 通过通讯模块发布配置到客户端
                if (BroadcastConfigChange(_currentConfig))
                {
                    // 记录发布历史
                    var historyEntry = new ConfigHistoryEntry(_currentConfig, $"发布配置到客户端")
                    {
                        Operation = "发布配置",
                        ConfigSnapshot = JObject.FromObject(_currentConfig)
                    };
                    _configHistory.Add(historyEntry);
                    //MessageBox.Show("配置已成功发布到所有客户端", "发布成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var mainForm = Application.OpenForms.OfType<frmMainNew>().FirstOrDefault();
                    mainForm?.PrintInfoLog($"配置已成功发布到所有客户端");
                }
                else
                {
                    throw new Exception("发布配置到客户端失败");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发布配置失败");
                MessageBox.Show($"发布配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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