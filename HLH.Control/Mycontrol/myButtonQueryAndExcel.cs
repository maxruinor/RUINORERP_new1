using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SHControls.Mycontrol
{
    public partial class myButtonQueryAndExcel : UserControl
    {
        public myButtonQueryAndExcel()
        {
            InitializeComponent();
        }



        static bool P_QueryNoError = true;
        static bool P_OutExcelNoError = true;
        static bool P_PrintNoError = true;


        #region 属性


        [Browsable(true), System.ComponentModel.Description("查询无错标志")]
        public bool QueryNoError
        {
            set
            {
                P_QueryNoError = value;
            }
            get
            {
                return P_QueryNoError;
            }
        }

        [Browsable(true), System.ComponentModel.Description("导出Excel无错标志")]
        public bool OutExcelNoError
        {
            set
            {
                P_OutExcelNoError = value;
            }
            get
            {
                return P_OutExcelNoError;
            }
        }


        [Browsable(true), System.ComponentModel.Description("打印无错标志")]
        public bool PrintNoError
        {
            set
            {
                P_PrintNoError = value;
            }
            get
            {
                return P_PrintNoError;
            }
        }

        #endregion



        [System.ComponentModel.Description("我的查询事件")]
        public event EventHandler CQueryClick;
        protected void OnQueryClick(object sender, EventArgs e)
        {
            if (CQueryClick != null)
                CQueryClick(sender, e);
        }


        [System.ComponentModel.Description("导出excel事件")]
        public event EventHandler COutExcelClick;
        protected void OnOutExcelClick(object sender, EventArgs e)
        {
            if (COutExcelClick != null)
                COutExcelClick(sender, e);
        }



        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.OnQueryClick(sender, e);
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            this.OnOutExcelClick(sender, e);

        }


        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
    }
}
