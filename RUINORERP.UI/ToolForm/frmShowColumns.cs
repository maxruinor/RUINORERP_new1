using Krypton.Toolkit;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UCSourceGrid;
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
using System.Xml.Serialization;

namespace RUINORERP.UI.ToolForm
{

    /// <summary>
    /// https://www.cnblogs.com/unicornsir/p/16186537.html
    /// </summary>
    public partial class frmShowColumns : KryptonForm
    {
        public frmShowColumns()
        {
            InitializeComponent();
        }
        private SerializableDictionary<string, bool> _ConfigItems = new SerializableDictionary<string, bool>();
        public SerializableDictionary<string, bool> ConfigItems { get => _ConfigItems; set => _ConfigItems = value; }

        private List<KeyValuePair<string, SourceGridDefineColumnItem>> items = new List<KeyValuePair<string, SourceGridDefineColumnItem>>();
        public List<KeyValuePair<string, SourceGridDefineColumnItem>> Items { get => items; set => items = value; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //如果全不选中。不可以
            int total = 0;
            for (int i = 0; i < chkListBox.Items.Count; i++)
            {
                if (chkListBox.GetItemChecked(i))
                {
                    total++;
                }
            }
            if (total == 0)
            {
                MessageBox.Show("必须要有选择一个！");
                return;
            }
            //if (chkListBox.GetItemText(chkListBox.Items) == "你得到的值")
            //{
            //    chkListBox.SetItemChecked(i, true);
            //}

            for (int i = 0; i < chkListBox.Items.Count; i++)
            {
                if (chkListBox.GetItemChecked(i))
                {
                    KeyValuePair<string, SourceGridDefineColumnItem> kvitem = Items.Find(kv => kv.Key == chkListBox.Items[i].ToString());
                    kvitem.Value.Visible = true;

                }
                else
                {
                    KeyValuePair<string, SourceGridDefineColumnItem> kvitem = Items.Find(kv => kv.Key == chkListBox.Items[i].ToString());
                    kvitem.Value.Visible = false;
                }

                //上面也可以优化为一行
                //同时也变更配置中的值
                ConfigItems[chkListBox.Items[i].ToString()] = chkListBox.GetItemChecked(i);
            }
            //保存
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmShowColumns_Load(object sender, EventArgs e)
        {
            chkListBox.Items.Clear();
            foreach (var item in Items)
            {
                chkListBox.Items.Add(item.Key);
                chkListBox.SetItemChecked(chkListBox.Items.Count - 1, item.Value.Visible); //true改为false为没有选中。
                //保存一份到配置集合中
                //ConfigItems.Add(item.Key, item.Value.Visible);

            }



        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListBox.Items.Count; i++)
            {
                chkListBox.SetItemChecked(i, chkAll.Checked);
            }
        }

        private void chkReverseSelection_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListBox.Items.Count; i++)
            {
                chkListBox.SetItemChecked(i, !chkListBox.GetItemChecked(i));
            }
        }


        private string xmlFileName = string.Empty;

        /// <summary>
        ///用来保存配置自定义列
        /// </summary>
        [Browsable(false)]
        public string XmlFileName
        {
            get;
            set;
        }


    }
}
