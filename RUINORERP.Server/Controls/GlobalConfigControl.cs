using Newtonsoft.Json.Linq;
using RUINORERP.Server.Commands;
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

        private readonly string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
        private const string CONFIG_FILE_NAME = "SystemGlobalConfig.json";
        private const string CONFIG_ROOT_NODE = "SystemGlobalConfig";

        public GlobalConfigControl()
        {
            InitializeComponent();
            
            // 从依赖注入容器获取服务
            _logger = Startup.GetFromFac<ILogger<GlobalConfigControl>>();
            _configFileReceiver = new ConfigFileReceiver(System.IO.Path.Combine(basePath, CONFIG_FILE_NAME));
            _commandManager = new CommandManager();
            _configHistory = new List<ConfigHistoryEntry>();
            
            // 订阅命令管理器事件
            _commandManager.CommandExecuted += (s, e) => UpdateButtonStates();
            _commandManager.CommandUndone += (s, e) => UpdateButtonStates();
            _commandManager.CommandRedone += (s, e) => UpdateButtonStates();
        }

        private void GlobalConfigControl_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                var configObject = propertyGrid1.SelectedObject as SystemGlobalconfig;
                if (configObject == null)
                {
                    MessageBox.Show("没有可保存的配置对象", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 验证配置有效性
                if (!ValidateConfiguration(configObject))
                {
                    return;
                }

                // 创建配置JSON对象
                JObject configJson = JObject.FromObject(configObject);
                JObject LastConfigJson = new JObject(new JProperty(CONFIG_ROOT_NODE, configJson));

                // 执行保存命令
                EditConfigCommand command = new EditConfigCommand(_configFileReceiver, LastConfigJson, "系统全局配置更新");
                _commandManager.ExecuteCommand(command);

                // 添加到历史记录
                var historyEntry = new ConfigHistoryEntry
                {
                    Operation = "保存配置",
                    Description = "系统全局配置更新",
                    User = Environment.UserName,
                    ConfigSnapshot = (JObject)LastConfigJson.DeepClone()
                };
                _configHistory.Add(historyEntry);

                _logger?.LogInformation("配置保存成功: {FileName}", CONFIG_FILE_NAME);
                
                // 更新UI状态
                UpdateButtonStates();
                
                MessageBox.Show("配置已成功保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "配置保存失败");
                MessageBox.Show($"保存配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                string configFilePath = System.IO.Path.Combine(basePath, CONFIG_FILE_NAME);
                
                if (!File.Exists(configFilePath))
                {
                    _logger?.LogWarning("配置文件不存在，创建默认配置: {FilePath}", configFilePath);
                    CreateDefaultConfiguration(configFilePath);
                }

                // 读取配置文件
                string json = File.ReadAllText(configFilePath);
                JObject configJson = JObject.Parse(json);
                
                // 解析配置对象
                if (configJson.TryGetValue(CONFIG_ROOT_NODE, out JToken token))
                {
                    JObject systemGlobalConfigJson = token as JObject;
                    SystemGlobalconfig configObject = systemGlobalConfigJson.ToObject<SystemGlobalconfig>();
                    
                    // 绑定到TreeView和PropertyGrid
                    BindConfigurationToUI(configObject);
                    
                    _logger?.LogInformation("配置加载成功: {FileName}", CONFIG_FILE_NAME);
                }
                else
                {
                    throw new InvalidOperationException($"配置文件格式错误，缺少 {CONFIG_ROOT_NODE} 节点");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "配置加载失败");
                MessageBox.Show($"加载配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // 创建默认配置作为后备方案
                CreateFallbackConfiguration();
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
            listView.Columns.Add("描述", 300);
            listView.Columns.Add("用户", 150);

            // 添加历史记录（按时间倒序）
            foreach (var entry in _configHistory.OrderByDescending(h => h.Timestamp))
            {
                var item = new ListViewItem(entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(entry.Operation);
                item.SubItems.Add(entry.Description);
                item.SubItems.Add(entry.User);
                listView.Items.Add(item);
            }

            if (_configHistory.Count == 0)
            {
                listView.Items.Add(new ListViewItem("暂无历史记录"));
            }

            var panel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            var btnExport = new Button { Text = "导出历史记录", Left = 10, Top = 8 };
            var btnClose = new Button { Text = "关闭", Left = 120, Top = 8 };

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
                        var historyData = _configHistory.Select(h => new
                        {
                            h.Timestamp,
                            h.Operation,
                            h.Description,
                            h.User,
                            ConfigSnapshot = h.ConfigSnapshot?.ToString()
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

            btnClose.Click += (s, e) => historyForm.Close();

            panel.Controls.Add(btnExport);
            panel.Controls.Add(btnClose);

            historyForm.Controls.Add(listView);
            historyForm.Controls.Add(panel);

            historyForm.ShowDialog(this);
        }

        /// <summary>
        /// 验证配置
        /// </summary>
        /// <param name="configObject">配置对象</param>
        /// <returns>验证结果</returns>
        private bool ValidateConfiguration(SystemGlobalconfig configObject)
        {
            var validationResults = new List<string>();

            // 基本验证
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

            // 业务逻辑验证
            if (configObject.UseSharedPrinter && string.IsNullOrEmpty(configObject.SomeSetting))
            {
                validationResults.Add("启用共享打印机时，相关设置不能为空");
            }

            if (validationResults.Count > 0)
            {
                var errorMessage = string.Join("\n", validationResults);
                MessageBox.Show($"配置验证失败:\n{errorMessage}", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private void BindConfigurationToUI(SystemGlobalconfig configObject)
        {
            // 绑定到PropertyGrid
            propertyGrid1.SelectedObject = configObject;

            // 绑定到TreeView
            CreateCategoryNodes(configObject);
        }

        /// <summary>
        /// 创建分类节点
        /// </summary>
        /// <param name="configObject">配置对象</param>
        private void CreateCategoryNodes(SystemGlobalconfig configObject)
        {
            treeView1.Nodes.Clear();

            // 服务器配置节点
            var serverNode = new TreeNode("服务器配置");
            serverNode.Nodes.Add("端口设置").Tag = new { Property = nameof(configObject.ServerPort), Category = "Server" };
            serverNode.Nodes.Add("最大连接数").Tag = new { Property = nameof(configObject.MaxConnections), Category = "Server" };
            serverNode.Nodes.Add("心跳间隔").Tag = new { Property = nameof(configObject.HeartbeatInterval), Category = "Server" };
            treeView1.Nodes.Add(serverNode);

            // 数据库配置节点
            var dbNode = new TreeNode("数据库配置");
            dbNode.Nodes.Add("连接字符串").Tag = new { Property = nameof(configObject.DbConnectionString), Category = "Database" };
            dbNode.Nodes.Add("数据库类型").Tag = new { Property = nameof(configObject.DbType), Category = "Database" };
            treeView1.Nodes.Add(dbNode);

            // 缓存配置节点
            var cacheNode = new TreeNode("缓存配置");
            cacheNode.Nodes.Add("缓存类型").Tag = new { Property = nameof(configObject.CacheType), Category = "Cache" };
            cacheNode.Nodes.Add("缓存连接字符串").Tag = new { Property = nameof(configObject.CacheConnectionString), Category = "Cache" };
            treeView1.Nodes.Add(cacheNode);

            // 日志配置节点
            var logNode = new TreeNode("日志配置");
            logNode.Nodes.Add("日志级别").Tag = new { Property = nameof(configObject.LogLevel), Category = "Logging" };
            logNode.Nodes.Add("启用日志").Tag = new { Property = nameof(configObject.EnableLogging), Category = "Logging" };
            treeView1.Nodes.Add(logNode);

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        private void CreateDefaultConfiguration(string filePath)
        {
            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 创建默认配置
            var defaultConfig = new SystemGlobalconfig
            {
                ServerPort = 8080,
                MaxConnections = 100,
                HeartbeatInterval = 30000,
                DbConnectionString = "Server=localhost;Database=RUINORERP;Uid=root;Pwd=123456;",
                DbType = "MySql",
                LogLevel = "Info",
                EnableLogging = true,
                UseSharedPrinter = false,
                SomeSetting = ""
            };

            var configJson = JObject.FromObject(defaultConfig);
            var fullConfig = new JObject(new JProperty(CONFIG_ROOT_NODE, configJson));

            File.WriteAllText(filePath, fullConfig.ToString(Newtonsoft.Json.Formatting.Indented));
        }

        /// <summary>
        /// 创建后备配置
        /// </summary>
        private void CreateFallbackConfiguration()
        {
            try
            {
                var fallbackConfig = new SystemGlobalconfig
                {
                    ServerPort = 8080,
                    MaxConnections = 50,
                    HeartbeatInterval = 30000,
                    DbConnectionString = "Server=localhost;Database=RUINORERP;Uid=root;Pwd=123456;",
                    DbType = "MySql",
                    LogLevel = "Info",
                EnableLogging = true
                };

                BindConfigurationToUI(fallbackConfig);
                MessageBox.Show("使用默认配置启动，请检查配置文件后重新加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (e.Node.Tag != null && propertyGrid1.SelectedObject != null)
            {
                var configObject = propertyGrid1.SelectedObject as SystemGlobalconfig;
                if (configObject != null)
                {
                    var propertyInfo = e.Node.Tag.GetType().GetProperty("Property");
                    if (propertyInfo != null)
                    {
                        var propertyName = propertyInfo.GetValue(e.Node.Tag)?.ToString();
                        if (!string.IsNullOrEmpty(propertyName))
                        {
                            var prop = typeof(SystemGlobalconfig).GetProperty(propertyName);
                            if (prop != null)
                            {
                                var value = prop.GetValue(configObject);
                                textBox1.Text = value?.ToString() ?? "";
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// PropertyGrid属性改变事件
        /// </summary>
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // 可以在这里添加属性改变时的处理逻辑
            _logger?.LogInformation("配置属性已更改: {PropertyName} = {NewValue}", e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value);
        }

        /// <summary>
        /// 文本框文本改变事件
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag != null && propertyGrid1.SelectedObject != null)
            {
                var configObject = propertyGrid1.SelectedObject as SystemGlobalconfig;
                if (configObject != null)
                {
                    var propertyInfo = treeView1.SelectedNode.Tag.GetType().GetProperty("Property");
                    if (propertyInfo != null)
                    {
                        var propertyName = propertyInfo.GetValue(treeView1.SelectedNode.Tag)?.ToString();
                        if (!string.IsNullOrEmpty(propertyName))
                        {
                            var prop = typeof(SystemGlobalconfig).GetProperty(propertyName);
                            if (prop != null && prop.CanWrite)
                            {
                                try
                                {
                                    var convertedValue = Convert.ChangeType(textBox1.Text, prop.PropertyType);
                                    prop.SetValue(configObject, convertedValue);
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