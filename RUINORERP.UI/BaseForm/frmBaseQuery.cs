using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.BaseForm
{
    public partial class frmBaseQuery : frmBase
    {
        public frmBaseQuery()
        {
            InitializeComponent();

            InitListData();
        }

     

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            //this.dataGridView1.DataSource = null;
            //toolStripButtonSave.Enabled = false;
            ListDataSoure = bindingSourceList;
            //绑定导航
           // this.bindingNavigatorList.BindingSource = ListDataSoure;

           // this.dataGridView1.DataSource = ListDataSoure.DataSource;
        }

        private ConcurrentDictionary<string, KeyValuePair<string, bool>> fieldNameList;

        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("表列名的中文描述集合"), Category("自定属性"), Browsable(true)]
        public ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList
        {
            get
            {
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }

        public System.Windows.Forms.BindingSource _ListDataSoure = null;

        [Description("列表中的要显示的数据来源[BindingSource]"), Category("自定属性"), Browsable(true)]
        /// <summary>
        /// 列表的数据源(实际要显示的)
        /// </summary>
        public System.Windows.Forms.BindingSource ListDataSoure
        {
            get { return _ListDataSoure; }
            set { _ListDataSoure = value; }
        }
    }
}
