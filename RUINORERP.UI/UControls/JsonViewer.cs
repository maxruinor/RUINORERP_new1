using Krypton.Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Monitoring.Auditing
{
    /// <summary>
    /// Json查看器
    /// </summary>
    public partial class JsonViewer : KryptonPanel
    {
        private KryptonTreeView treeView;
        private KryptonButton btnExpandAll;
        private KryptonButton btnCollapseAll;
        private KryptonButton btnFormatJson;

        public JsonViewer()
        {
            InitializeComponent();
            // 初始化控件
            treeView = new KryptonTreeView();
            btnExpandAll = new KryptonButton { Text = "展开全部" };
            btnCollapseAll = new KryptonButton { Text = "收起全部" };
            btnFormatJson = new KryptonButton { Text = "格式化JSON" };

            // 设置树视图样式
            treeView.Dock = DockStyle.Fill;
            treeView.StateCommon.Node.Content.ShortText.Font = new Font("微软雅黑", 9F);
            //treeView.StateCommon.Node.Label.Width = 20;

            // 设置按钮面板
            var btnPanel = new KryptonPanel { Dock = DockStyle.Top, Height = 35 };
            btnPanel.Controls.Add(btnExpandAll);
            btnPanel.Controls.Add(btnCollapseAll);
            btnPanel.Controls.Add(btnFormatJson);

            btnExpandAll.Location = new Point(5, 5);
            btnCollapseAll.Location = new Point(100, 5);
            btnFormatJson.Location = new Point(195, 5);

            // 添加控件到主面板
            Controls.Add(treeView);
            Controls.Add(btnPanel);

            // 注册事件
            btnExpandAll.Click += (s, e) => treeView.ExpandAll();
            btnCollapseAll.Click += (s, e) =>
            {
                treeView.CollapseAll();
                if (treeView.Nodes.Count > 0)
                    treeView.Nodes[0].Expand();
            };
            btnFormatJson.Click += (s, e) => FormatJson();
        }

        /// <summary>
        /// 加载审计日志数据
        /// </summary>
        public void LoadAuditData(string jsonData)
        {
            treeView.Nodes.Clear();

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                treeView.Nodes.Add(new TreeNode("无审计数据"));
                return;
            }

            try
            {
                // 尝试解析为JSON对象
                var jsonObject = JObject.Parse(jsonData);
                AddJsonObjectToTree(jsonObject, treeView.Nodes);
            }
            catch (JsonException)
            {
                // 如果不是有效的JSON对象，尝试作为普通文本显示
                treeView.Nodes.Add(new TreeNode($"原始数据: {jsonData}"));
            }

            // 展开根节点
            if (treeView.Nodes.Count > 0)
                treeView.Nodes[0].Expand();
        }

        private void AddJsonObjectToTree(JObject json, TreeNodeCollection nodes)
        {
            foreach (var property in json.Properties())
            {
                var node = new TreeNode(GetDisplayText(property.Name, property.Value));

                if (property.Value is JObject obj)
                {
                    AddJsonObjectToTree(obj, node.Nodes);
                }
                else if (property.Value is JArray array)
                {
                    AddJsonArrayToTree(array, node.Nodes);
                }

                nodes.Add(node);
            }
        }

        private void AddJsonArrayToTree(JArray array, TreeNodeCollection nodes)
        {
            for (int i = 0; i < array.Count; i++)
            {
                var element = array[i];
                var node = new TreeNode($"[{i}] {GetDisplayText(null, element)}");

                if (element is JObject obj)
                {
                    AddJsonObjectToTree(obj, node.Nodes);
                }
                else if (element is JArray subArray)
                {
                    AddJsonArrayToTree(subArray, node.Nodes);
                }

                nodes.Add(node);
            }
        }

        private string GetDisplayText(string name, JToken value)
        {
            if (value.Type == JTokenType.Null)
                return $"{name}: null";

            if (value.Type == JTokenType.String)
            {
                string strValue = value.ToString();
                if (strValue.Length > 50)
                    strValue = strValue.Substring(0, 47) + "...";
                return $"{name}: \"{strValue}\"";
            }

            if (value.Type == JTokenType.Object || value.Type == JTokenType.Array)
                return name ?? value.Type.ToString();

            return $"{name}: {value}";
        }

        private void FormatJson()
        {
            if (treeView.Nodes.Count == 0) return;

            try
            {
                // 从树视图重新构建JSON
                var jsonString = RebuildJsonFromTree(treeView.Nodes);
                var formattedJson = JToken.Parse(jsonString).ToString(Formatting.Indented);

                // 显示格式化的JSON
                using (var form = new KryptonForm())
                {
                    form.Text = "格式化JSON";
                    form.Size = new Size(800, 600);

                    var textBox = new KryptonRichTextBox();
                    textBox.Dock = DockStyle.Fill;
                    textBox.Text = formattedJson;
                    textBox.ReadOnly = true;

                    form.Controls.Add(textBox);
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"格式化JSON失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string RebuildJsonFromTree(TreeNodeCollection nodes)
        {
            var jsonObj = new JObject();

            foreach (TreeNode node in nodes)
            {
                string[] parts = node.Text.Split(new[] { ": " }, 2, StringSplitOptions.None);
                string key = parts[0].Trim();

                if (node.Nodes.Count > 0)
                {
                    if (key.StartsWith("[")) // 数组元素
                    {
                        // 略过，数组处理在父节点
                    }
                    else if (node.Nodes[0].Text.StartsWith("[")) // 是数组
                    {
                        var array = new JArray();
                        foreach (TreeNode itemNode in node.Nodes)
                        {
                            if (itemNode.Nodes.Count > 0)
                            {
                                var itemObj = new JObject();
                                foreach (TreeNode propNode in itemNode.Nodes)
                                {
                                    string[] itemParts = propNode.Text.Split(new[] { ": " }, 2, StringSplitOptions.None);
                                    itemObj[itemParts[0].Trim()] = itemParts[1].Trim();
                                }
                                array.Add(itemObj);
                            }
                            else
                            {
                                array.Add(itemNode.Text.Split(new[] { ": " }, 2, StringSplitOptions.None)[1].Trim());
                            }
                        }
                        jsonObj[key] = array;
                    }
                    else // 是对象
                    {
                        jsonObj[key] = JObject.Parse(RebuildJsonFromTree(node.Nodes));
                    }
                }
                else
                {
                    string value = parts.Length > 1 ? parts[1].Trim() : "";
                    jsonObj[key] = value;
                }
            }

            return jsonObj.ToString();
        }
    }
}