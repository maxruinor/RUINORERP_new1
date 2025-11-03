using RUINOR.WinFormsUI.Demo.ChkComboBoxDemo;
using RUINOR.WinFormsUI.TileListView;
using RUINORERP.Extensions.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINOR.WinFormsUI.Demo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //RedisHelper.DbNum = 1;
            //BNRFactory.Default.Register("redis", new RedisSequenceParameter(RedisHelper.Db));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmChkComboBoxDemo frmChkComboBoxDemo = new frmChkComboBoxDemo();
            frmChkComboBoxDemo.Show();
            OutlookGridApp.Form1 form = new OutlookGridApp.Form1();
            form.Show();

            RUINOR.WinFormsUI.Demo.TreeGridView.Form1 treeform = new RUINOR.WinFormsUI.Demo.TreeGridView.Form1();
            treeform.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RUINOR.WinFormsUI.TreeViewColumns.TreeViewColumnsDemo.Form1 treeform = new TreeViewColumns.TreeViewColumnsDemo.Form1();
            treeform.Show();

            TryTreeListView.TryTreeListView tryTree = new TryTreeListView.TryTreeListView();
            tryTree.Show();
        }

        private void btnTest平铺chk_Click(object sender, EventArgs e)
        {

            tileListView.Clear();
            tileListView.AddGroup("1", "颜色");
            for (int i = 0; i < 15; i++)
            {
                //tileListView.AddItemToGroup("颜色", i + "Item属",true);
                tileListView.AddItemToGroup("1", i + "Item属性值很长呢？ ", true);
            }

            tileListView.AddItemToGroup("1", "Item 2", false);

            tileListView.AddGroup("2","Group 2");
            tileListView.AddItemToGroup("2","Group 2", true);
            tileListView.AddItemToGroup("2","Group 2",  false);

            tileListView.UpdateUI();


        }
    }
}
