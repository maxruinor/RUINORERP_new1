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
using static RUINORERP.Extensions.ServiceExtensions.EditConfigCommand;

namespace RUINORERP.Server
{
    public partial class frmGlobalConfig : Form
    {
        private readonly CommandManager _commandManager;
        private readonly ConfigFileReceiver _configFileReceiver;
        private readonly ILogger<frmGlobalConfig> _logger;
        private List<ConfigHistoryEntry> _configHistory;

        private readonly string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
        private const string CONFIG_FILE_NAME = "SystemGlobalConfig.json";
        private const string CONFIG_ROOT_NODE = "SystemGlobalConfig";

        public frmGlobalConfig(ILogger<frmGlobalConfig> logger = null)
        {
            InitializeComponent();
            _logger = logger;
            _configFileReceiver = new ConfigFileReceiver(System.IO.Path.Combine(basePath, CONFIG_FILE_NAME));
            _commandManager = new CommandManager();
            _configHistory = new List<ConfigHistoryEntry>();
            
            // 订阅命令管理器事件
            _commandManager.CommandExecuted += (s, e) => UpdateButtonStates();
            _commandManager.CommandUndone += (s, e) => UpdateButtonStates();
            _commandManager.CommandRedone += (s, e) => UpdateButtonStates();
            
            // 初始化UI状态
            InitializeUI();
        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

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

        private void tsbtnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void frmGlobalConfig_Load(object sender, EventArgs e)
        {
            LoadData();
        }

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

        private void RefreshData()
        {

        }

        private void tsbtnUndoButton_Click(object sender, EventArgs e)
        {
            _commandManager.UndoCommand();
        }

        private void tsbtnRedoButton_Click(object sender, EventArgs e)
        {
            _commandManager.RedoCommand();
        }

        private JObject ParseConfigFromUI()
        {

            // 将更改保存到App.config
            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // config.Save();
            JObject jsonConfig = (JObject)treeView1.SelectedNode.Tag;
            return jsonConfig;
            // 从UI控件解析配置到JObject
            // 这里需要根据实际UI控件来实现解析逻辑
            //return new JObject();
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            if (propertyGrid1.SelectedObjects != null && propertyGrid1.SelectedObjects.Length > 0)
            {
                var selectedObject = propertyGrid1.SelectedObjects[0];
                var properties = TypeDescriptor.GetProperties(selectedObject);
                foreach (PropertyDescriptor property in properties)
                {
                    // 这里我们只处理字符串类型的属性，你可以根据需要扩展
                    if (property.PropertyType == typeof(string))
                    {
                        string propertyName = property.Name;
                        string propertyValue = (string)property.GetValue(selectedObject);
                        textBox1.Text = propertyValue;
                        break; // 假设只有一个属性会被编辑
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (propertyGrid1.SelectedObjects != null && propertyGrid1.SelectedObjects.Length > 0)
            {
                var selectedObject = propertyGrid1.SelectedObjects[0];
                var properties = TypeDescriptor.GetProperties(selectedObject);
                foreach (PropertyDescriptor property in properties)
                {
                    if (property.PropertyType == typeof(string) && property.Name == "SomeSetting")
                    {
                        property.SetValue(selectedObject, textBox1.Text);
                        break; // 假设只有一个属性会被编辑
                    }
                }
            }
        }

        #region 辅助方法

        /// <summary>
        /// 初始化UI状态
        /// </summary>
        private void InitializeUI()
        {
            UpdateButtonStates();
            Text = "全局配置中心 - " + CONFIG_FILE_NAME;
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStates()
        {
            tsbtnUndoButton.Enabled = _commandManager.CanUndo;
            tsbtnRedoButton.Enabled = _commandManager.CanRedo;
        }

        /// <summary>
        /// 显示配置历史记录
        /// </summary>
        private void ShowConfigHistory()
        {
            if (_configHistory.Count == 0)
            {
                MessageBox.Show("暂无配置历史记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var historyForm = new Form
            {
                Text = "配置历史记录",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
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
            listView.Columns.Add("用户", 100);

            foreach (var entry in _configHistory.OrderByDescending(h => h.Timestamp))
            {
                var item = new ListViewItem(entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(entry.Operation);
                item.SubItems.Add(entry.Description);
                item.SubItems.Add(entry.User);
                listView.Items.Add(item);
            }

            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            var btnClose = new System.Windows.Forms.Button
            {
                Text = "关闭",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Right,
                Width = 80
            };

            var btnExport = new System.Windows.Forms.Button
            {
                Text = "导出历史",
                Dock = DockStyle.Left,
                Width = 80
            };

            btnExport.Click += (s, e) =>
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "JSON文件 (*.json)|*.json|所有文件 (*.*)|*.*",
                    DefaultExt = "json",
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
                            h.User
                        }).ToList();

                        var json = JsonConvert.SerializeObject(historyData, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(saveDialog.FileName, json);
                        MessageBox.Show("历史记录导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            panel.Controls.Add(btnClose);
            panel.Controls.Add(btnExport);

            historyForm.Controls.Add(listView);
            historyForm.Controls.Add(panel);

            historyForm.CancelButton = btnClose;
            historyForm.ShowDialog(this);
        }

        /// <summary>
        /// 验证配置有效性
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <returns>验证是否通过</returns>
        private bool ValidateConfiguration(SystemGlobalconfig config)
        {
            if (config == null)
            {
                MessageBox.Show("配置对象不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var validationResults = new List<string>();

            // 验证服务器端口
            if (config.ServerPort < 1 || config.ServerPort > 65535)
            {
                validationResults.Add("服务器端口必须在1-65535范围内");
            }

            // 验证最大连接数
            if (config.MaxConnections < 1 || config.MaxConnections > 10000)
            {
                validationResults.Add("最大连接数必须在1-10000范围内");
            }

            // 验证心跳间隔
            if (config.HeartbeatInterval < 1000 || config.HeartbeatInterval > 60000)
            {
                validationResults.Add("心跳间隔必须在1000-60000毫秒范围内");
            }

            // 验证业务逻辑配置
            if (config.采购日期必填 && string.IsNullOrWhiteSpace(config.SomeSetting))
            {
                validationResults.Add("启用预交日期必填时，相关设置不能为空");
            }

            // 验证共享打印机配置
            if (config.UseSharedPrinter && string.IsNullOrWhiteSpace(config.SomeSetting))
            {
                validationResults.Add("启用共享打印机时，相关设置不能为空");
            }

            if (validationResults.Count > 0)
            {
                var errorMessage = "配置验证失败：\n\n" + string.Join("\n", validationResults);
                MessageBox.Show(errorMessage, "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 绑定配置到UI
        /// </summary>
        /// <param name="configObject">配置对象</param>
        private void BindConfigurationToUI(SystemGlobalconfig configObject)
        {
            // 清空现有节点
            treeView1.Nodes.Clear();
            
            // 创建根节点
            TreeNode root = new TreeNode("全局配置")
            {
                Tag = configObject,
                ImageKey = "config",
                SelectedImageKey = "config"
            };
            
            // 根据配置属性创建分类节点
            CreateCategoryNodes(root, configObject);
            
            treeView1.Nodes.Add(root);
            treeView1.ExpandAll();
            
            // 绑定到PropertyGrid
            propertyGrid1.SelectedObject = configObject;
        }

        /// <summary>
        /// 创建分类节点
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="configObject">配置对象</param>
        private void CreateCategoryNodes(TreeNode root, SystemGlobalconfig configObject)
        {
            var properties = TypeDescriptor.GetProperties(configObject);
            var categories = properties.Cast<PropertyDescriptor>()
                .Select(p => p.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .OrderBy(c => c);

            foreach (var category in categories)
            {
                TreeNode categoryNode = new TreeNode(category)
                {
                    ImageKey = "folder",
                    SelectedImageKey = "folder"
                };
                root.Nodes.Add(categoryNode);
            }
        }

        /// <summary>
        /// 创建默认配置文件
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        private void CreateDefaultConfiguration(string configFilePath)
        {
            try
            {
                // 确保目录存在
                string directory = System.IO.Path.GetDirectoryName(configFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 创建默认配置
                var defaultConfig = new SystemGlobalconfig
                {
                    ServerName = "localhost",
                    ServerPort = 8080,
                    MaxConnections = 100,
                    HeartbeatInterval = 30,
                    DbType = "MySql",
                    CacheType = "Redis",
                    EnableLogging = true,
                    LogLevel = "Information",
                    DirectPrinting = true,
                    UseSharedPrinter = false,
                    OpenProdTypeForSaleCheck = true
                };

                // 序列化为JSON
                JObject configJson = JObject.FromObject(defaultConfig);
                JObject rootJson = new JObject(new JProperty(CONFIG_ROOT_NODE, configJson));
                
                // 保存到文件
                File.WriteAllText(configFilePath, rootJson.ToString());
                
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建默认配置文件失败");
                throw;
            }
        }

        /// <summary>
        /// 创建后备配置（当加载失败时）
        /// </summary>
        private void CreateFallbackConfiguration()
        {
            try
            {
                var fallbackConfig = new SystemGlobalconfig
                {
                    ServerName = "localhost",
                    ServerPort = 8080,
                    MaxConnections = 50,
                    HeartbeatInterval = 30
                };

                BindConfigurationToUI(fallbackConfig);
                propertyGrid1.SelectedObject = fallbackConfig;
                
                MessageBox.Show("已加载默认配置，请检查配置文件后重新保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建后备配置失败");
                MessageBox.Show("无法加载配置，系统将使用最小配置", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }

    /// <summary>
    /// 命令管理器扩展
    /// </summary>
    public static class CommandManagerExtensions
    {
        public static bool CanUndo(this CommandManager manager)
        {
            return manager != null && manager.GetType().GetProperty("CanUndo")?.GetValue(manager) as bool? == true;
        }

        public static bool CanRedo(this CommandManager manager)
        {
            return manager != null && manager.GetType().GetProperty("CanRedo")?.GetValue(manager) as bool? == true;
        }
    }

    /// <summary>
    /// 配置历史记录条目
    /// </summary>
    public class ConfigHistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public string Operation { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
        public JObject ConfigSnapshot { get; set; }

        public ConfigHistoryEntry()
        {
            Timestamp = DateTime.Now;
        }
    }

    /// <summary>
    /// 配置历史管理器
    /// </summary>
    public class ConfigHistoryManager
    {
        private readonly List<ConfigHistoryEntry> _history;
        private readonly int _maxHistorySize;

        public ConfigHistoryManager(int maxHistorySize = 50)
        {
            _history = new List<ConfigHistoryEntry>();
            _maxHistorySize = maxHistorySize;
        }

        public IReadOnlyList<ConfigHistoryEntry> GetHistory()
        {
            return _history.AsReadOnly();
        }

        public void AddHistoryEntry(ConfigHistoryEntry entry)
        {
            if (entry == null) return;

            _history.Add(entry);

            // 限制历史记录大小
            if (_history.Count > _maxHistorySize)
            {
                _history.RemoveAt(0);
            }
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        public ConfigHistoryEntry GetLastEntry()
        {
            return _history.LastOrDefault();
        }

        public List<ConfigHistoryEntry> GetHistoryByDateRange(DateTime startDate, DateTime endDate)
        {
            return _history.Where(h => h.Timestamp >= startDate && h.Timestamp <= endDate).ToList();
        }
    }
}
