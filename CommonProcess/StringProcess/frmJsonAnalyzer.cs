using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using ICSharpCode.TextEditor;
using HLH.Lib.Helper;

namespace CommonProcess.StringProcess
{

    /// <summary>
    /// 经常页码数据 会放到一个大的json数据体中，假期能采集到了一个完整的json数据。只是需要处理其中的单个数据
    /// </summary>
    public partial class frmJsonAnalyzer : Form
    {
        public frmJsonAnalyzer()
        {
            InitializeComponent();
        }

        private string _jsonTextInput = string.Empty;

        public string jsonTextInput
        {
            get { return _jsonTextInput; }
            set { _jsonTextInput = value; }
        }

        private void frmJsonAnalyzer_Load(object sender, EventArgs e)
        {
            richTextBoxinput.Text = jsonTextInput;
        }




        private void btn_Click(object sender, EventArgs e)
        {
            /*
           //创建JObject
           //textfile1.txt存储着需要解析的JSON数据
           var jobj = Newtonsoft.Json.Linq.JObject.Parse(richTextBoxinput.Text);

           //创建TreeView的数据源
           object obj = jobj.Children().Select(c => Lib.JsonHeaderLogic.FromJToken(c));
           treeView1.Nodes.Clear();
           loadtree(treeView1, obj);
           */

            try
            {
                richTextBoxinput.Text = HLH.Lib.Helper.JsonHelper.ConvertJsonString(richTextBoxinput.Text);
                var root = JToken.Parse(richTextBoxinput.Text);
                DisplayTreeView(root, "测试");

            }
            catch (Exception ex)
            {
                rtxt提取Para.Text = "异常" + ex.Message + ex.StackTrace;

            }




        }



        public void loadtree(TreeView treeView1, object obj)
        {
            IEnumerable<JsonHeaderLogic> list = (IEnumerable<JsonHeaderLogic>)obj;
            if (list != null)
            {
                foreach (var item in list)
                {
                    TreeNode boot = new TreeNode();
                    boot.Tag = item.Header;
                    boot.Text = item.Header;
                    boot.Name = item.Header;
                    boot.ToolTipText = item.ToString();
                    treeView1.Nodes.Add(boot);
                    if (item.Token != null)
                    {
                        if (item.Token.HasValues)
                        {
                            AddTree(item.Token, boot);
                        }

                    }

                }

            }





        }

        /// <summary>
        /// 递归添加树的节点
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="pNode"></param>
        public void AddTree(object obj, TreeNode pNode)
        {
            Newtonsoft.Json.Linq.JToken data = obj as Newtonsoft.Json.Linq.JToken;
            //Lib.JsonHeaderLogic data = obj as Lib.JsonHeaderLogic;
            if (data.HasValues)
            {

                foreach (var item in data.Children())
                {
                    TreeNode boot = new TreeNode();
                    boot.Text = item.ToString();
                    //  boot.Name = dr.Id;
                    //  boot.ToolTipText = boot.Name + "-" + dr["isleaf"].ToString();  //保存是否为叶子
                    if (data.Count() != 1 || data.Children().Count() == 0)//一个的话。就是重复的？不对。
                    {
                        pNode.Nodes.Add(boot);
                        if (item.HasValues)
                        {
                            AddTree(item, boot);
                        }
                    }
                    else
                    {
                        if (item.HasValues)
                        {
                            AddTree(item, pNode);
                        }
                    }

                }







            }

        }


        private void btnGetValue_Click(object sender, EventArgs e)
        {
            string lastRs = string.Empty;
            if (rtxtLastValue.Tag != null)
            {
                rtxt提取Para.Clear();

                string path = rtxtLastValue.Tag.ToString();

                UCJson路径提取Para ucpara = new UCJson路径提取Para();
                ucpara.jsonPath = path;
                if (treeView1.Nodes.Count > 2)
                {
                    ucpara.isJson格式化 = true;
                }
                else
                {
                    ucpara.isJson格式化 = false;
                }

                lastRs = ucpara.ProcessDo(richTextBoxinput.Text);

                #region 
                /*
                string[] sz = path.Split('.');
                var root = JToken.Parse(richTextBoxinput.Text);
                for (int i = 0; i < sz.Length; i++)
                {
                    if (i == sz.Length - 1)
                    {
                        lastRs = "结果：\r\n" + root[sz[i]].ToString() + "\r\n";
                        rtxt提取Para.AppendText("结果：\r\n" + root[sz[i]].ToString());
                    }
                    else
                    {
                        root = Newtonsoft.Json.Linq.JObject.Parse(root[sz[i]].ToString());
                        rtxt提取Para.AppendText("过程：\r\n" + root.ToString());
                    }
                }*/
                #endregion
                rtxtLastValue.AppendText("\r\n");
                rtxtLastValue.AppendText(lastRs);
            }

        }

        private void btnSearchKeyValue_Click(object sender, EventArgs e)
        {
            //richTextBoxinput.Text = ConvertJsonString(richTextBoxinput.Text);
            /*
            //创建JObject
            //textfile1.txt存储着需要解析的JSON数据
            Newtonsoft.Json.Linq.JObject jobj = Newtonsoft.Json.Linq.JObject.Parse(richTextBoxinput.Text);
            string rs = GetValuebyKey(richTextBoxinput.Text, txtKey.Text);
            rtxtLastValue.Text = rs;*/


            var root = JToken.Parse(richTextBoxinput.Text);
            TreeNode tnfind = GetNode(root, "测试", txtKey.Text.Trim(), txtValue.Text.Trim(), chk值包含.Checked);
            if (tnfind != null)
            {
                if (tnfind.Text.Trim().Length == 0)
                {
                    MessageBox.Show("没有找到对应的节点。请重新设置查找条件，注意大小写和特殊符号！");
                }
                else
                {
                    TreeNode[] tn = treeView1.Nodes.Find(tnfind.Text, true);
                    if (tn.Length > 0)
                    {
                        rtxtLastValue.Clear();
                        /*
                        if ("" is JObject)
                        {

                        }
                        if ("" is JValue)
                        {
                        }
                        if (JArray)
                        {

                        }*/
                        //由查找的递归方法中看。也是由 JProperty 时才返回值
                        Newtonsoft.Json.Linq.JProperty jp = null;
                        if (tnfind.Tag.GetType().FullName.Contains("JProperty"))
                        {
                            jp = tnfind.Tag as Newtonsoft.Json.Linq.JProperty;
                        }
                        else
                        {

                        }


                        treeView1.SelectedNode = tn[0];
                        treeView1.SelectedNode.Checked = true;
                        treeView1.SelectedNode.BackColor = Color.SkyBlue;
                        rtxtLastValue.AppendText("节点：");
                        rtxtLastValue.AppendText("\r\n");
                        rtxtLastValue.AppendText(tnfind.Tag.ToString());
                        rtxtLastValue.AppendText("\r\n");
                        rtxtLastValue.AppendText("路径：");
                        rtxtLastValue.AppendText("\r\n");
                        rtxtLastValue.AppendText(jp.Path);
                        rtxtLastValue.Tag = jp.Path;//方便临时测试
                        jsonPickPath = jp.Path;
                        int offset = 0;
                        int length = 0;
                        length = tnfind.Tag.ToString().Length;
                        if (richTextBoxinput.Text.Contains(tnfind.Tag.ToString()))
                        {
                            offset = richTextBoxinput.Text.IndexOf(tnfind.Tag.ToString());
                        }
                        //richTextBoxinput.ActiveTextAreaControl.SelectionManager.SelectedText = tn[0].Text;


                        richTextBoxinput.Select(offset, length);
                        richTextBoxinput.SelectionBackColor = Color.LightBlue;
                        richTextBoxinput.ScrollToCaret();

                    }
                }
            }

        }

        public string GetValuebyKey(string jsonTxt, string key)
        {
            string rs = string.Empty;

            if (jsonTxt.Contains("{"))
            {
                #region json
                Newtonsoft.Json.Linq.JObject jobj = Newtonsoft.Json.Linq.JObject.Parse(jsonTxt);

                if (jobj.HasValues)
                {
                    int total = jobj.Values().Count();
                    int i = 0;
                    foreach (Newtonsoft.Json.Linq.JToken item in jobj.Values())
                    {
                        i++;
                        if (item.Path.ToLower() == key.ToLower())
                        {
                            rs = item.ToString();
                            break;
                        }
                        else
                        {
                            var tmpMsg = "";
                            int ckResult = ChkJson(item, out tmpMsg);

                            if (string.IsNullOrEmpty(item.Path))
                            {

                            }
                            if (item.HasValues)
                            {
                                foreach (var sub in item.Children())
                                {
                                    if (sub.HasValues)
                                    {
                                        if (sub.Count() == 1)
                                        {

                                        }


                                        rs = GetValuebyKey(sub.ToString(), key);
                                    }

                                }
                                //rs= GetValuebyKey(item.ToString(), key);
                            }
                            else
                            {
                                if (item.Path.ToLower() == key.ToLower())
                                {
                                    rs = item.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {

                }

                #endregion
            }
            else
            {
                #region json1
                #endregion
            }







            return rs;
        }

        protected new int ChkJson(JToken jo, out string msg)
        {
            msg = "";
            if (jo == null) return 0;
            if (jo.HasValues && jo.Values().Count() > 0)
            {
                foreach (JToken item in jo.Values())
                {
                    var result = ChkJson(item, out msg);
                    if (result != 0)
                        return result;
                }
            }
            else
            {
                try
                {
                    var obj = JToken.Parse(jo.ToString());

                }
                catch (Exception ex)
                {


                }

                return 801;

            }

            return 0;
        }





        #region 绑定到树 并且 查找KEY or value

        /// <summary>
        /// 查找指定的key 或value
        /// </summary>
        /// <param name="root"></param>
        /// <param name="rootName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ContainsValue">值是否可以包含</param>
        /// <returns></returns>
        private TreeNode GetNode(JToken root, string rootName, string key, string value, bool ContainsValue)
        {
            TreeNode tnfind = new TreeNode();

            try
            {

                var tNode = new TreeNode(rootName);
                tNode.Tag = root;

                tnfind = FindNode(root, tNode, key, value, ContainsValue);

            }
            finally
            {
            }
            return tnfind;
        }

        private TreeNode FindNode(JToken token, TreeNode inTreeNode, string key, string value, bool ContainsValue)
        {
            TreeNode tnfind = new TreeNode();
            if (!string.IsNullOrEmpty(tnfind.Text))
            {
                return tnfind;
            }
            if (token == null)
                return tnfind;
            if (token is JValue)
            {
                var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
                childNode.Name = token.ToString();
                childNode.Tag = token;
                if (token.Path.Contains(key))
                {

                }

            }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    if (!string.IsNullOrEmpty(tnfind.Text))
                    {
                        return tnfind;
                    }
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                    childNode.Name = property.Name;
                    childNode.Tag = property;
                    if (property.Name == key && !string.IsNullOrEmpty(key))
                    {
                        childNode.BackColor = Color.LightBlue;

                        tnfind = childNode;

                        return tnfind;
                    }

                    if (ContainsValue)
                    {
                        if (property.Value.ToString().Contains(value) && !string.IsNullOrEmpty(value))
                        {
                            childNode.BackColor = Color.LightBlue;
                            tnfind = childNode;

                            return tnfind;
                        }
                    }
                    else
                    {
                        //==时，
                        if (property.Value.ToString() == value && !string.IsNullOrEmpty(value))
                        {
                            childNode.BackColor = Color.LightBlue;
                            tnfind = childNode;

                            return tnfind;
                        }
                    }



                    tnfind = FindNode(property.Value, childNode, key, value, ContainsValue);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
                    childNode.Name = i.ToString();
                    childNode.Tag = array[i];
                    tnfind = FindNode(array[i], childNode, key, value, ContainsValue);
                    return tnfind;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }
            return tnfind;
        }

        #endregion



        #region 绑定到树

        private void DisplayTreeView(JToken root, string rootName)
        {
            treeView1.BeginUpdate();
            try
            {
                treeView1.Nodes.Clear();
                var tNode = treeView1.Nodes[treeView1.Nodes.Add(new TreeNode(rootName))];
                tNode.Tag = root;

                AddNode(root, tNode);

                treeView1.ExpandAll();
            }
            finally
            {
                treeView1.EndUpdate();
            }
        }

        private void AddNode(JToken token, TreeNode inTreeNode)
        {
            if (token == null)
                return;
            if (token is JValue)
            {
                var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
                childNode.Name = token.ToString();
                childNode.Tag = token;
            }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                    childNode.Name = property.Name;
                    childNode.Tag = property;
                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
                    childNode.Name = i.ToString();
                    childNode.Tag = array[i];
                    AddNode(array[i], childNode);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }
        }

        #endregion


        private string _jsonPickPath = string.Empty;
        /// <summary>
        /// path格式为  aa.bb.cc;  提取时是  obj[aa][bb][cc].tostring()
        /// </summary>
        public string jsonPickPath
        {
            get { return _jsonPickPath; }
            set { _jsonPickPath = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (jsonPickPath.Trim().Length > 0)
            {
                this.DialogResult = DialogResult.Yes;
            }
            else
            {
                this.DialogResult = DialogResult.No;
            }
            this.Close();
        }
    }
}
