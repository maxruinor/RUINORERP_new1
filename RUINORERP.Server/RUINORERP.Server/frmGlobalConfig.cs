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
using static RUINORERP.Extensions.ServiceExtensions.EditConfigCommand;
using RUINORERP.Extensions.ServiceExtensions;

namespace RUINORERP.Server
{
    public partial class frmGlobalConfig : Form
    {

        private readonly CommandManager _commandManager;
        private readonly ConfigFileReceiver _configFileReceiver;

        private readonly string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");

        public frmGlobalConfig()
        {
            InitializeComponent();
            _configFileReceiver = new ConfigFileReceiver(basePath + "/" + nameof(SystemGlobalconfig) + ".json");
            _commandManager = new CommandManager();
        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig()
        {

            var configObject = propertyGrid1.SelectedObject as SystemGlobalconfig;
            if (configObject != null)
            {
                // 将configObject转换为JObject
                JObject configJson = JObject.FromObject(configObject);

                // 创建一个新的JObject，并将SystemGlobalConfig作为根节点
                JObject LastConfigJson = new JObject(new JProperty("SystemGlobalConfig", JObject.FromObject(configObject)));


                //JObject newConfig = ParseConfigFromUI(); // 从UI解析新的配置
                EditConfigCommand command = new EditConfigCommand(_configFileReceiver, LastConfigJson);
                _commandManager.ExecuteCommand(command);

                //直接保存
                // string json = JsonConvert.SerializeObject(configObject, Newtonsoft.Json.Formatting.Indented);



                // 将JObject转换为字符串，包含顶级节点
                // string json = configJson.ToString(Newtonsoft.Json.Formatting.Indented);

                // 保存到文件
                // File.WriteAllText("config.json", json);


                // File.WriteAllText("config.json", json);
            }



            //// 保存到JSON或XML
            //File.WriteAllText("config.json", jsonConfig.ToString());
            ////xmlConfig.Save("config.xml");
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
            // 读取App.config
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //XmlDocument xmlConfig = new XmlDocument();
            //xmlConfig.Load("config.xml");

            //直接读取JSON或XML
            //JObject jsonConfig = JObject.Parse(File.ReadAllText("config.json"));

            // 读取文件内容
            string json = File.ReadAllText(basePath + "/" + nameof(SystemGlobalconfig) + ".json");

            // 解析JSON，首先获取SystemGlobalConfig节点
            JObject configJson = JObject.Parse(json);
            JObject systemGlobalConfigJson = configJson["SystemGlobalConfig"] as JObject;

            // 将SystemGlobalConfig节点转换为SystemGlobalConfig对象
            SystemGlobalconfig configObject = systemGlobalConfigJson.ToObject<SystemGlobalconfig>();


            // 假设config.json已经被反序列化为Config对象
            // SystemGlobalconfig configObject = JsonConvert.DeserializeObject<SystemGlobalconfig>(File.ReadAllText("config.json"));
            // 绑定到TreeView
            TreeNode root = new TreeNode("全局配置");
            root.Tag = configObject;
            // 根据配置文件结构添加节点
            treeView1.Nodes.Add(root);
            propertyGrid1.SelectedObject = configObject;

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
    }
}
