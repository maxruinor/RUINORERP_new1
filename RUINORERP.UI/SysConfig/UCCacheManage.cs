using FastReport.DevComponents.DotNetBar.Controls;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("缓存管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCCacheManage : UserControl
    {
        public UCCacheManage()
        {
            InitializeComponent();
        }

        private void 请求缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void UCCacheManage_Load(object sender, EventArgs e)
        {
            //这里添加所有缓存
            LoadCacheToUI();
        }

        private void LoadCacheToUI()
        {
            //加载所有缓存的表
            listBoxTableList.Items.Clear();
            foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
            {
                listBoxTableList.Items.Add(tableName);
            }
        }

        private void btnRefreshCache_Click(object sender, EventArgs e)
        {
            LoadCacheToUI();
        }

        /// <summary>
        /// 所有实体表都在这个命名空间下，不需要每次都反射
        /// </summary>
        Assembly assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
        private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tableName = listBoxTableList.SelectedItem.ToString();
            var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
            if (CacheList == null)
            {
                dataGridView1.DataSource = null;
                return;
            }
            // 使用 Assembly.Load 加载包含 PrintHelper<T> 类的程序集

            // 使用 GetType 方法获取 PrintHelper<T> 的类型
            Type type = assembly.GetType("RUINORERP.Model." + tableName);
            dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(type);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.XmlFileName = "UCCacheManage" + tableName;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = CacheList;
        }
    }
}
