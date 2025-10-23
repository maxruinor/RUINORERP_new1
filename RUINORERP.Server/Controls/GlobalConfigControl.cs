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
        private const string SYSTEM_GLOBAL_CONFIG_FILE_NAME = "SystemGlobalConfig.json";
        private const string SYSTEM_GLOBAL_CONFIG_ROOT_NODE = "SystemGlobalConfig";
        private const string SERVER_CONFIG_FILE_NAME = "ServerConfig.json";
        private const string SERVER_CONFIG_ROOT_NODE = "ServerConfig";
        private const string GLOBAL_VALIDATOR_CONFIG_FILE_NAME = "GlobalValidatorConfig.json";
        private const string GLOBAL_VALIDATOR_CONFIG_ROOT_NODE = "GlobalValidatorConfig";

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
            // 初始化TreeView，按文件显示配置
            InitializeConfigTreeView();
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
        /// 根据配置类型进行验证
        /// </summary>
        private bool ValidateConfiguration(BaseConfig config)
        {
            if (config is ServerConfig serverConfig)
            {
                return ValidateServerConfiguration(serverConfig);
            }
            else if (config is SystemGlobalconfig systemConfig)
            {
                return ValidateSystemGlobalConfiguration(systemConfig);
            }
            else if (config is GlobalValidatorConfig validatorConfig)
            {
                return ValidateGlobalValidatorConfiguration(validatorConfig);
            }
            
            MessageBox.Show("未知的配置类型", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
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
        /// 初始化配置TreeView（右侧树）
        /// </summary>
        private void InitializeConfigTreeView()
        {
            try
            {
                treeView1.Nodes.Clear();

                // 添加服务器配置节点
                TreeNode serverNode = new TreeNode("服务器配置");
                serverNode.Tag = new ConfigInfo { FileName = SERVER_CONFIG_FILE_NAME, RootNode = SERVER_CONFIG_ROOT_NODE, ConfigType = typeof(ServerConfig) };
                treeView1.Nodes.Add(serverNode);

                // 添加系统全局配置节点
                TreeNode systemNode = new TreeNode("系统全局配置");
                systemNode.Tag = new ConfigInfo { FileName = SYSTEM_GLOBAL_CONFIG_FILE_NAME, RootNode = SYSTEM_GLOBAL_CONFIG_ROOT_NODE, ConfigType = typeof(SystemGlobalconfig) };
                treeView1.Nodes.Add(systemNode);

                // 添加全局验证配置节点
                TreeNode validatorNode = new TreeNode("全局验证配置");
                validatorNode.Tag = new ConfigInfo { FileName = GLOBAL_VALIDATOR_CONFIG_FILE_NAME, RootNode = GLOBAL_VALIDATOR_CONFIG_ROOT_NODE, ConfigType = typeof(GlobalValidatorConfig) };
                treeView1.Nodes.Add(validatorNode);

                treeView1.ExpandAll();
                
                // 订阅TreeView选择事件
                treeView1.AfterSelect -= treeView1_AfterSelect;
                treeView1.AfterSelect += treeView1_AfterSelect;

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化配置TreeView失败");
                MessageBox.Show($"初始化配置树时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化JSON文件TreeView（左侧树）
        /// </summary>
        private void InitializeJsonFilesTreeView()
        {
            try
            {
                treeView2.Nodes.Clear();

                // 添加服务器配置JSON文件节点
                TreeNode serverJsonNode = new TreeNode("ServerConfig.json");
                serverJsonNode.Tag = new ConfigInfo { FileName = SERVER_CONFIG_FILE_NAME, RootNode = SERVER_CONFIG_ROOT_NODE, ConfigType = typeof(ServerConfig) };
                treeView2.Nodes.Add(serverJsonNode);

                // 添加系统全局配置JSON文件节点
                TreeNode systemJsonNode = new TreeNode("SystemGlobalConfig.json");
                systemJsonNode.Tag = new ConfigInfo { FileName = SYSTEM_GLOBAL_CONFIG_FILE_NAME, RootNode = SYSTEM_GLOBAL_CONFIG_ROOT_NODE, ConfigType = typeof(SystemGlobalconfig) };
                treeView2.Nodes.Add(systemJsonNode);

                // 添加全局验证配置JSON文件节点
                TreeNode validatorJsonNode = new TreeNode("GlobalValidatorConfig.json");
                validatorJsonNode.Tag = new ConfigInfo { FileName = GLOBAL_VALIDATOR_CONFIG_FILE_NAME, RootNode = GLOBAL_VALIDATOR_CONFIG_ROOT_NODE, ConfigType = typeof(GlobalValidatorConfig) };
                treeView2.Nodes.Add(validatorJsonNode);

                treeView2.ExpandAll();

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化JSON文件TreeView失败");
                MessageBox.Show($"初始化JSON文件树时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 左侧TreeView选择事件处理
        /// </summary>
        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node?.Tag is ConfigInfo configInfo)
                {
                    // 加载配置文件
                    LoadConfigurationFile(configInfo.FileName, configInfo.RootNode, configInfo.ConfigType);
                    
                    // 在右侧TreeView中展开并选中对应配置文件的节点
                    ExpandAndSelectTreeNode(treeView1, configInfo.FileName);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理TreeView2选择事件失败");
                MessageBox.Show($"加载配置文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    treeView.SelectedNode = node;
                    node.EnsureVisible();
                    // 触发选择事件处理
                    treeView1_AfterSelect(treeView, new TreeViewEventArgs(node));
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
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            // 此方法保留以兼容现有调用，但主要功能已移至InitializeConfigTreeView
            InitializeConfigTreeView();
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
        /// 加载系统全局配置
        /// </summary>
        private void LoadSystemGlobalConfiguration()
        {
            LoadConfigurationFile(SYSTEM_GLOBAL_CONFIG_FILE_NAME, SYSTEM_GLOBAL_CONFIG_ROOT_NODE, typeof(SystemGlobalconfig));
        }

        /// <summary>
        /// 加载服务器配置
        /// </summary>
        private void LoadServerConfiguration()
        {
            LoadConfigurationFile(SERVER_CONFIG_FILE_NAME, SERVER_CONFIG_ROOT_NODE, typeof(ServerConfig));
        }

        /// <summary>
        /// 加载全局验证配置
        /// </summary>
        private void LoadGlobalValidatorConfiguration()
        {
            LoadConfigurationFile(GLOBAL_VALIDATOR_CONFIG_FILE_NAME, GLOBAL_VALIDATOR_CONFIG_ROOT_NODE, typeof(GlobalValidatorConfig));
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

            // 绑定到TreeView，显示当前配置的属性分类
            if (configObject is SystemGlobalconfig systemConfig)
            {
                CreateSystemGlobalCategoryNodes(systemConfig);
            }
            else if (configObject is ServerConfig serverConfig)
            {
                CreateServerCategoryNodes(serverConfig);
            }
            else if (configObject is GlobalValidatorConfig validatorConfig)
            {
                CreateGlobalValidatorCategoryNodes(validatorConfig);
            }
        }

        /// <summary>
        /// 创建全局验证配置分类节点
        /// </summary>
        private void CreateGlobalValidatorCategoryNodes(GlobalValidatorConfig configObject)
        {
            // 保存当前选中的配置文件节点
            TreeNode selectedConfigNode = null;
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Tag is ConfigInfo info && info.ConfigType == typeof(GlobalValidatorConfig))
                {
                    selectedConfigNode = node;
                    break;
                }
            }

            // 清空并重新创建子节点
            if (selectedConfigNode != null)
            {
                selectedConfigNode.Nodes.Clear();

                // 采购模块节点
                var purchaseNode = new TreeNode("采购模块");
                purchaseNode.Nodes.Add("预交日期必填").Tag = new { Property = nameof(configObject.预交日期必填), Category = "采购模块" };
                selectedConfigNode.Nodes.Add(purchaseNode);

                // 生产模块节点
                var productionNode = new TreeNode("生产模块");
                productionNode.Nodes.Add("预开工日期必填").Tag = new { Property = nameof(configObject.预开工日期必填), Category = "生产模块" };
                productionNode.Nodes.Add("预完工日期必填").Tag = new { Property = nameof(configObject.预完工日期必填), Category = "生产模块" };
                productionNode.Nodes.Add("返工提醒天数").Tag = new { Property = nameof(configObject.ReworkTipDays), Category = "生产模块" };
                selectedConfigNode.Nodes.Add(productionNode);

                // 客户关系节点
                var customerNode = new TreeNode("客户关系");
                customerNode.Nodes.Add("计划提前提示天数").Tag = new { Property = nameof(configObject.计划提前提示天数), Category = "客户关系" };
                selectedConfigNode.Nodes.Add(customerNode);

                // 销售模块节点
                var salesNode = new TreeNode("销售模块");
                salesNode.Nodes.Add("销售金额精度").Tag = new { Property = nameof(configObject.MoneyDataPrecision), Category = "销售模块" };
                salesNode.Nodes.Add("是否来自平台").Tag = new { Property = nameof(configObject.IsFromPlatform), Category = "销售模块" };
                salesNode.Nodes.Add("项目组必填").Tag = new { Property = nameof(configObject.NeedInputProjectGroup), Category = "销售模块" };
                selectedConfigNode.Nodes.Add(salesNode);

                // 借出模块节点
                var lendNode = new TreeNode("借出模块");
                lendNode.Nodes.Add("借出单的接收单位必填").Tag = new { Property = nameof(configObject.借出单的接收单位必填), Category = "借出" };
                selectedConfigNode.Nodes.Add(lendNode);

                selectedConfigNode.ExpandAll();
            }
        }

        /// <summary>
        /// 创建系统全局配置分类节点
        /// </summary>
        /// <param name="configObject">系统全局配置对象</param>
        private void CreateSystemGlobalCategoryNodes(SystemGlobalconfig configObject)
        {
            treeView1.Nodes.Clear();

            // 采购模块配置节点
            var purchaseNode = new TreeNode("采购模块配置");
            purchaseNode.Nodes.Add("采购日期必填").Tag = new { Property = "采购日期必填", Category = "Purchase" };
            purchaseNode.Nodes.Add("是否来自平台").Tag = new { Property = nameof(configObject.IsFromPlatform), Category = "Purchase" };
            treeView1.Nodes.Add(purchaseNode);

            // 销售模块配置节点
            var salesNode = new TreeNode("销售模块配置");
            salesNode.Nodes.Add("是否来自平台").Tag = new { Property = nameof(configObject.IsFromPlatform), Category = "Sales" };
            salesNode.Nodes.Add("开启产品类型检查").Tag = new { Property = nameof(configObject.OpenProdTypeForSaleCheck), Category = "Sales" };
            treeView1.Nodes.Add(salesNode);

            // 打印设置节点
            var printNode = new TreeNode("打印设置");
            printNode.Nodes.Add("直接打印").Tag = new { Property = nameof(configObject.DirectPrinting), Category = "Printing" };
            printNode.Nodes.Add("使用共享打印机").Tag = new { Property = nameof(configObject.UseSharedPrinter), Category = "Printing" };
            treeView1.Nodes.Add(printNode);

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 创建服务器配置分类节点
        /// </summary>
        /// <param name="configObject">服务器配置对象</param>
        private void CreateServerCategoryNodes(ServerConfig configObject)
        {
            treeView1.Nodes.Clear();

            // 服务器基础设置节点
            var serverNode = new TreeNode("服务器基础设置");
            serverNode.Nodes.Add("服务器名称").Tag = new { Property = "ServerName", Category = "ServerSettings" };
            serverNode.Nodes.Add("服务器端口").Tag = new { Property = "ServerPort", Category = "ServerSettings" };
            serverNode.Nodes.Add("最大连接数").Tag = new { Property = "MaxConnections", Category = "ServerSettings" };
            serverNode.Nodes.Add("心跳间隔").Tag = new { Property = "HeartbeatInterval", Category = "ServerSettings" };
            treeView1.Nodes.Add(serverNode);

            // 数据库配置节点
            var dbNode = new TreeNode("数据库配置");
            dbNode.Nodes.Add("数据库类型").Tag = new { Property = "DbType", Category = "DatabaseSettings" };
            dbNode.Nodes.Add("连接字符串").Tag = new { Property = "DbConnectionString", Category = "DatabaseSettings" };
            treeView1.Nodes.Add(dbNode);

            // 缓存配置节点
            var cacheNode = new TreeNode("缓存配置");
            cacheNode.Nodes.Add("缓存类型").Tag = new { Property = "CacheType", Category = "CacheSettings" };
            cacheNode.Nodes.Add("缓存连接字符串").Tag = new { Property = "CacheConnectionString", Category = "CacheSettings" };
            treeView1.Nodes.Add(cacheNode);

            // 日志配置节点
            var logNode = new TreeNode("日志配置");
            logNode.Nodes.Add("日志级别").Tag = new { Property = "LogLevel", Category = "LoggingSettings" };
            logNode.Nodes.Add("启用日志").Tag = new { Property = "EnableLogging", Category = "LoggingSettings" };
            treeView1.Nodes.Add(logNode);

            // 文件存储配置节点
            var fileNode = new TreeNode("文件存储配置");
            fileNode.Nodes.Add("文件存储路径").Tag = new { Property = "FileStoragePath", Category = "FileStorageSettings" };
            fileNode.Nodes.Add("最大文件大小(MB)").Tag = new { Property = "MaxFileSizeMB", Category = "FileStorageSettings" };
            treeView1.Nodes.Add(fileNode);

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 创建默认配置
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

            JObject configJson;

            if (configType == typeof(ServerConfig))
            {
                // 创建服务器默认配置
                var defaultServerConfig = new ServerConfig
                {
                    ServerName = "RUINORERP Server",
                    ServerPort = 8080,
                    MaxConnections = 100,
                    HeartbeatInterval = 30000,
                    DbType = "MySql",
                    DbConnectionString = "Server=localhost;Database=RUINORERP;Uid=root;Pwd=123456;",
                    CacheType = "Memory",
                    CacheConnectionString = "",
                    LogLevel = "Info",
                    EnableLogging = true,
                    FileStoragePath = "D:\\RUINORERP\\FileStorage",
                    MaxFileSizeMB = 10
                };

                configJson = JObject.FromObject(defaultServerConfig);
            }
            else if (configType == typeof(SystemGlobalconfig))
            {
                // 创建系统全局默认配置
                var defaultSystemConfig = new SystemGlobalconfig
                {
                    UseSharedPrinter = false,
                    SomeSetting = "",
                    采购日期必填 = false,
                    IsFromPlatform = false,
                    OpenProdTypeForSaleCheck = true,
                    DirectPrinting = true
                };

                configJson = JObject.FromObject(defaultSystemConfig);
            }
            else if (configType == typeof(GlobalValidatorConfig))
            {
                // 创建全局验证默认配置
                var defaultValidatorConfig = new GlobalValidatorConfig
                {
                    预交日期必填 = false,
                    预开工日期必填 = false,
                    预完工日期必填 = false,
                    ReworkTipDays = 3,
                    计划提前提示天数 = 1,
                    MoneyDataPrecision = 4,
                    IsFromPlatform = true,
                    NeedInputProjectGroup = true,
                    借出单的接收单位必填 = false,
                    SomeSetting = ""
                };

                configJson = JObject.FromObject(defaultValidatorConfig);
            }
            else
            {
                throw new ArgumentException($"未知的配置类型: {configType.Name}");
            }

            var fullConfig = new JObject(new JProperty(rootNode, configJson));
            File.WriteAllText(filePath, fullConfig.ToString(Newtonsoft.Json.Formatting.Indented));
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




        /// <summary>
        /// TreeView选择改变事件
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                // 处理配置文件节点的选择
                if (e.Node?.Tag is ConfigInfo configInfo)
                {
                    // 加载配置文件
                    LoadConfigurationFile(configInfo.FileName, configInfo.RootNode, configInfo.ConfigType);
                    
                    // 在左侧TreeView中展开并选中相应的节点
                    ExpandAndSelectTreeNode((System.Windows.Forms.TreeView)treeView2, configInfo.FileName);
                }
                // 处理原有属性节点的选择，保留兼容
                else if (e.Node.Tag != null && propertyGrid1.SelectedObject != null)
                {
                    // 尝试获取属性信息
                    var tagType = e.Node.Tag.GetType();
                    var propertyInfo = tagType.GetProperty("Property");
                    
                    if (propertyInfo != null)
                    {
                        var propertyName = propertyInfo.GetValue(e.Node.Tag)?.ToString();
                        if (!string.IsNullOrEmpty(propertyName))
                        {
                            var configType = _currentConfig?.GetType();
                            if (configType != null)
                            {
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
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理TreeView1选择事件失败");
                MessageBox.Show($"处理TreeView1选择事件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
            if (treeView1.SelectedNode?.Tag != null && _currentConfig != null)
            {
                // 尝试获取属性信息
                var tagType = treeView1.SelectedNode.Tag.GetType();
                var propertyInfo = tagType.GetProperty("Property");
                
                if (propertyInfo != null)
                {
                    var propertyName = propertyInfo.GetValue(treeView1.SelectedNode.Tag)?.ToString();
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
            LoadData();
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