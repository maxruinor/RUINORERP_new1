using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.Business;
using System.Linq.Expressions;
using SqlSugar;
using RUINORERP.Common.Helper;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using System.Reflection;
using RUINORERP.UI.Common;
using System.Collections.Concurrent;
using RUINORERP.Model.Dto;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINOR.WinFormsUI.TreeViewThreeState;
using RUINORERP.UI.UControls;
using Newtonsoft.Json.Linq;
using RUINORERP.Model.ConfigModel;
using System.IO;
using RUINORERP.Extensions.ServiceExtensions;
using static RUINORERP.Extensions.ServiceExtensions.EditConfigCommand;
using TransInstruction;
using RUINORERP.UI.ClientCmdService;
using TransInstruction.CommandService;
using System.Threading;
using RUINORERP.Global;

namespace RUINORERP.UI.SysConfig
{

    /// <summary>
    /// 比用户授权角色简单，那个是行记录存在性控制， 这里是默认每个角色都有。通过关系表中的字段来控制的
    /// </summary>
    [MenuAttrAssemblyInfo("动态参数配置", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.动态参数)]
    public partial class UCGlobalDynamicConfig : UserControl
    {
        private readonly CommandManager _commandManager;
        private readonly string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.DynamicConfigFileDirectory);

        private ConfigFileReceiver _configFileReceiver;

        public UCGlobalDynamicConfig()
        {
            InitializeComponent();
            _commandManager = new CommandManager();
        }

        //private void LoadData()
        //{

        //    #region 系统配置
        //    // 解析JSON，首先获取SystemGlobalConfig节点
        //    JObject sGconfigJson = JObject.Parse(File.ReadAllText(File.ReadAllText(basePath + "/" + nameof(SystemGlobalconfig) + ".json")));
        //    JObject sgConfigJson = sGconfigJson["SystemGlobalconfig"] as JObject;

        //    // 将SystemGlobalConfig节点转换为SystemGlobalConfig对象
        //    SystemGlobalconfig SgconfigObject = sgConfigJson.ToObject<SystemGlobalconfig>();


        //    //ThreeStateTreeNode nd = new ThreeStateTreeNode();
        //    //nd.Text = "配置参数";
        //    //treeView1.Nodes.Add(nd);

        //    // 假设config.json已经被反序列化为Config对象
        //    // SystemGlobalconfig configObject = JsonConvert.DeserializeObject<SystemGlobalconfig>(File.ReadAllText("config.json"));
        //    // 绑定到TreeView
        //    TreeNode sgConfigNode = new TreeNode("全局配置");
        //    sgConfigNode.Tag = SgconfigObject;
        //    // 根据配置文件结构添加节点
        //    treeView1.Nodes.Add(sgConfigNode);

        //    #endregion




        //    // treeView1.Nodes[0].Expand();
        //}



        private void GetConfig<T>(string nodeText)
        {
            string className = typeof(T).Name;
            #region 验证配置
            // 读取App.config
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //XmlDocument xmlConfig = new XmlDocument();
            //xmlConfig.Load("config.xml");

            //直接读取JSON或XML
            //JObject jsonConfig = JObject.Parse(File.ReadAllText("config.json"));

            // 读取文件内容
            // string json = File.ReadAllText("GlobalValidator.json");
            string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.DynamicConfigFileDirectory);
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            string json = File.ReadAllText(basePath + "/" + className + ".json");

            // 解析JSON，首先获取SystemGlobalConfig节点
            JObject configJson = JObject.Parse(json);
            JObject systemGlobalConfigJson = null;

            if (configJson.TryGetValue(className, out JToken token))
            {
                systemGlobalConfigJson = token as JObject;

                // 将SystemGlobalConfig节点转换为SystemGlobalConfig对象
                object configObject = systemGlobalConfigJson.ToObject(typeof(T));

                //ThreeStateTreeNode nd = new ThreeStateTreeNode();
                //nd.Text = "配置参数";
                //treeView1.Nodes.Add(nd);

                // 假设config.json已经被反序列化为Config对象
                // SystemGlobalconfig configObject = JsonConvert.DeserializeObject<SystemGlobalconfig>(File.ReadAllText("config.json"));
                // 绑定到TreeView
                TreeNode ValidatorConfigNode = new TreeNode(nodeText);
                ValidatorConfigNode.Tag = configObject;
                // 根据配置文件结构添加节点
                treeView1.Nodes.Add(ValidatorConfigNode);

            }
            else
            {
                // 处理节点不存在的情况
                // 将SystemGlobalConfig节点转换为SystemGlobalConfig对象
                object configObject = configJson.ToObject(typeof(T));

                //ThreeStateTreeNode nd = new ThreeStateTreeNode();
                //nd.Text = "配置参数";
                //treeView1.Nodes.Add(nd);

                // 假设config.json已经被反序列化为Config对象
                // SystemGlobalconfig configObject = JsonConvert.DeserializeObject<SystemGlobalconfig>(File.ReadAllText("config.json"));
                // 绑定到TreeView
                TreeNode ValidatorConfigNode = new TreeNode(nodeText);
                ValidatorConfigNode.Tag = configObject;
                // 根据配置文件结构添加节点
                treeView1.Nodes.Add(ValidatorConfigNode);
            }



            #endregion
        }

        private void UCGlobalDynamicConfig_Load(object sender, EventArgs e)
        {
            treeView1.HideSelection = false;
            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView1.CheckBoxes = false;
            treeView1.Nodes.Clear();
            GetConfig<GlobalValidatorConfig>("验证配置");
            GetConfig<SystemGlobalconfig>("系统配置");
            //LoadData();
        }








        private void tVtypeList_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Node.Bounds);
            if (e.State == TreeNodeStates.Selected)//做判断
            {
                e.Graphics.FillRectangle(Brushes.CornflowerBlue, new Rectangle(e.Node.Bounds.Left, e.Node.Bounds.Top, e.Node.Bounds.Width, e.Node.Bounds.Height));//背景色为蓝色
                RectangleF drawRect = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 10, e.Bounds.Height);
                e.Graphics.DrawString(e.Node.Text, treeView1.Font, Brushes.White, drawRect);//字体为白色
            }
            else
            {
                e.DrawDefault = true;
            }
        }





        #region 基础声明

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        Exit(this);//csc关闭窗体
                        break;
                }

            }
            //return false;
            var key = keyData & Keys.KeyCode;
            var modeifierKey = keyData & Keys.Modifiers;
            if (modeifierKey == Keys.Control && key == Keys.F)
            {
                // MessageBox.Show("Control+F is pressed");
                return true;

            }

            var otherkey = keyData & Keys.KeyCode;
            var othermodeifierKey = keyData & Keys.Modifiers;
            if (othermodeifierKey == Keys.Control && otherkey == Keys.F)
            {
                MessageBox.Show("Control+F is pressed");
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }

        bool Edited = false;
        protected virtual void Exit(object thisform)
        {
            if (!Edited)
            {
                //退出
                CloseTheForm(thisform);
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗?   这里是不是可以进一步提示 哪些内容没有保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //退出
                    CloseTheForm(thisform);
                }
            }
        }

        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            if ((thisform as Control).Parent is KryptonPage)
            {
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
            else
            {
                Form frm = (thisform as Control).Parent.Parent as Form;
                frm.Close();
            }

        }




        #endregion


        private void btnSave_Click(object sender, EventArgs e)
        {
            //ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
            //configManager.GetValue("GlobalDynamicConfig");
            SaveConfig();
        }

        private void SaveConfig<T>()
        {
            var configObject = propertyGrid1.SelectedObject;
            if (propertyGrid1.SelectedObject != null)
            {
                // 将configObject转换为JObject
                JObject configJson = JObject.FromObject(configObject);

                // 创建一个新的JObject，并将GlobalValidatorConfig作为根节点
                JObject LastConfigJson = new JObject(new JProperty(typeof(T).Name, JObject.FromObject(configObject)));


                //JObject newConfig = ParseConfigFromUI(); // 从UI解析新的配置
                EditConfigCommand command = new EditConfigCommand(_configFileReceiver, LastConfigJson);
                _commandManager.ExecuteCommand(command);

                //直接保存
                // string json = JsonConvert.SerializeObject(configObject, Newtonsoft.Json.Formatting.Indented);

                // 将JObject转换为字符串，包含顶级节点
                // string json = configJson.ToString(Newtonsoft.Json.Formatting.Indented);

                // 保存到文件
                // File.WriteAllText("config.json", json);


                //如果有更新变动就上传到服务器再分发到所有客户端
                OriginalData odforCache = ActionForClient.更新动态配置<T>(configObject);
                byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                MainForm.Instance.ecs.client.Send(buffer);
            }

            //// 保存到JSON或XML
            //File.WriteAllText("config.json", jsonConfig.ToString());
            ////xmlConfig.Save("config.xml");
        }
        private void SaveConfig()
        {
            if (propertyGrid1.SelectedObject != null)
            {
                // 将configObject转换为JObject
                // JObject configJson = JObject.FromObject(propertyGrid1.SelectedObject);

                // 创建一个新的JObject，并将GlobalValidatorConfig作为根节点
                JObject LastConfigJson = new JObject(new JProperty(propertyGrid1.SelectedObject.GetType().Name, JObject.FromObject(propertyGrid1.SelectedObject)));


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

            //如果有更新变动就上传到服务器再分发到所有客户端

            //SystemGlobalconfig 下面类型有问题应该要根据选择的节点来配置
            OriginalData odforCache = ActionForClient.更新动态配置<SystemGlobalconfig>(propertyGrid1.SelectedObject);
            byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
            MainForm.Instance.ecs.client.Send(buffer);

            RequestReceiveEntityCmd request = new RequestReceiveEntityCmd(CmdOperation.Send);
            request.requestType = EntityTransferCmdType.处理动态配置;
            request.nextProcesszStep = NextProcesszStep.转发;
            request.SendObject = propertyGrid1.SelectedObject;
            MainForm.Instance.dispatcher.DispatchAsync(request, CancellationToken.None);


            //// 保存到JSON或XML
            //File.WriteAllText("config.json", jsonConfig.ToString());
            ////xmlConfig.Save("config.xml");
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseTheForm(this);
        }


        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                return;
            }


            if (!(treeView1.SelectedNode.Tag is BaseConfig))
            {
                return;
            }
            propertyGrid1.SelectedObject = treeView1.SelectedNode.Tag;

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            _configFileReceiver = new ConfigFileReceiver(basePath + "/" + propertyGrid1.SelectedObject.GetType().Name + ".json");
        }


        private void UpdateSaveEnabled(BaseEntity entity)
        {
            entity.PropertyChanged += (sender, s2) =>
            {
                //如果客户有变化，带出对应有业务员
                //if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.CustomerVendor_ID))
                //{

                //}

                toolStripButtonSave.Enabled = true;

            };
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








        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            toolStripButtonSave.Enabled = true;
        }

        private void tsbtnUndoButton_Click(object sender, EventArgs e)
        {
            _commandManager.UndoCommand();
        }

        private void tsbtnRedoButton_Click(object sender, EventArgs e)
        {
            _commandManager.RedoCommand();
        }

        private void kryptonPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
