using System.ComponentModel;
using System.Windows.Forms;

namespace HLH.WinControl.DateTimePickers
{
    /// <summary>
    /// 查询条件时间组控件
    /// </summary>
    public partial class UCDTPickers : UserControl
    {
        public UCDTPickers()
        {
            InitializeComponent();
        }

        private string _StrTimeText = string.Empty;
        /// <summary>
        /// 获取或设置是否显示合计行
        /// </summary>
        [Description("查询条件描述：下单时间"), Category("自定义属性")]
        public string StrTimeText
        {
            get { return _StrTimeText; }
            set
            {
                _StrTimeText = value;
                label1.Text = value;
            }
        }

        private bool showCheckBox1 = false;
        private bool showCheckBox2 = false;

        [Description("dt1是否显示勾选"), Category("自定义属性")]
        public bool ShowCheckBox1
        {
            get
            {
                showCheckBox1 = dt1.ShowCheckBox;
                return showCheckBox1;
            }
            set
            {
                showCheckBox1 = value;
                dt1.ShowCheckBox = value;
            }
        }

        [Description("dt2是否显示勾选"), Category("自定义属性")]
        public bool ShowCheckBox2
        {
            get
            {
                showCheckBox2 = dt2.ShowCheckBox;
                return showCheckBox2;
            }
            set
            {
                showCheckBox2 = value;
                dt2.ShowCheckBox = value;
            }

        }


        private bool dtChecked1 = false;
        private bool dtChecked2 = false;

        [Description("dt1是否勾选"), Category("自定义属性")]
        public bool DtChecked1
        {
            get
            {
                dtChecked1 = dt1.Checked;
                return dtChecked1;
            }
            set
            {
                dtChecked1 = value;
                dt1.Checked = value;
            }
        }

        [Description("dt2是否勾选"), Category("自定义属性")]
        public bool DtChecked2
        {
            get
            {
                dtChecked2 = dt2.Checked;
                return dtChecked2;
            }
            set
            {
                dtChecked2 = value;
                dt2.Checked = value;
            }

        }

    }
}
