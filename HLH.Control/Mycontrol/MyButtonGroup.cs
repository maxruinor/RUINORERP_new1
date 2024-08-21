using System;
using System.ComponentModel;
using System.Windows.Forms;




namespace SHControls.Mycontrol
{
    public partial class MyButtonGroup : UserControl
    {
        bool w_EidtFlag = false;



        static bool P_saveNoError = true;
        static bool P_delNoError = false;
        static bool P_InserNoError = true;
        static bool P_ModiyNoError = true;




        #region 属性


        [Browsable(true), System.ComponentModel.Description("修改无错标志")]
        public bool ModiyNoError
        {
            set
            {
                P_ModiyNoError = value;
            }
            get
            {
                return P_ModiyNoError;
            }
        }


        [Browsable(true), System.ComponentModel.Description("新增无错标志")]
        public bool InserNoError
        {
            set
            {
                P_InserNoError = value;
            }
            get
            {
                return P_InserNoError;
            }
        }


        [Browsable(true), System.ComponentModel.Description("删除无错标志")]
        public bool delNoError
        {
            set
            {
                P_delNoError = value;
            }
            get
            {
                return P_delNoError;
            }
        }


        [Browsable(true), System.ComponentModel.Description("保存无错标志")]
        public bool saveNoError
        {
            set
            {
                P_saveNoError = value;
            }
            get
            {
                return P_saveNoError;
            }
        }

        [Browsable(true), System.ComponentModel.Description("是否编辑的标志")]
        public bool IsEditFlag
        {
            set
            {
                w_EidtFlag = value;
            }
            get
            {
                return w_EidtFlag;
            }
        }
        #endregion


        public MyButtonGroup()
        {

            InitializeComponent();
        }

        #region 事件

        private void btn_quit_Click(object sender, EventArgs e)
        {
            this.OnCQuit(sender, e);
            GC.Collect();
        }


        private void btn_Query_Click(object sender, EventArgs e)
        {
            this.OnQueryClick(sender, e);
        }



        private void CmdEnable(bool p_YesNo)
        {
            this.btn_add.Enabled = p_YesNo;
            this.btn_modify.Enabled = p_YesNo;
            btn_save.Enabled = !p_YesNo;
            btn_del.Enabled = p_YesNo;
            btn_cancel.Enabled = !p_YesNo;
            this.btn_Query.Enabled = p_YesNo;
            this.btn_quit.Enabled = p_YesNo;

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.OnCCancel(sender, e);
            w_EidtFlag = false;
            CmdEnable(true);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {

            this.OnCInsert(sender, e);
            if (InserNoError)
            {

                CmdEnable(false);
                w_EidtFlag = false;

            }

        }

        private void btn_modify_Click(object sender, EventArgs e)
        {

            OnCModify(sender, e);
            if (ModiyNoError)
            {
                w_EidtFlag = true;
                CmdEnable(false);

            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (delNoError)
                {
                    this.OnCDelete(sender, e);
                    MessageBox.Show("删除成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("删除失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            OnCSave(sender, e);
            if (saveNoError)
            {
                CmdEnable(true);
                if (this.IsEditFlag)
                {
                    MessageBox.Show("修改式保存成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("新增式保存成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                w_EidtFlag = false;

            }

        }

        #endregion

        #region 事件属性

        #region "我的退出事件"


        [System.ComponentModel.Description("我的查询事件")]
        public event EventHandler CQueryClick;
        protected void OnQueryClick(object sender, EventArgs e)
        {
            if (CQueryClick != null)
                CQueryClick(sender, e);
        }


        [System.ComponentModel.Description("我的退出事件")]
        public event EventHandler CQuitClick;
        protected void OnCQuit(object sender, EventArgs e)
        {
            if (CQuitClick != null)
                CQuitClick(sender, e);
        }

        #endregion


        #region "我的添加事件"

        [System.ComponentModel.Description("我的添加事件")]
        public event EventHandler CInsertClick;
        protected void OnCInsert(object sender, EventArgs e)
        {
            if (CInsertClick != null)
                CInsertClick(sender, e);
        }

        #endregion


        #region "我的修改事件"

        [System.ComponentModel.Description("我的修改事件")]
        public event EventHandler CModifyClick;
        protected void OnCModify(object sender, EventArgs e)
        {
            if (CModifyClick != null)
                CModifyClick(sender, e);
        }

        #endregion

        #region "我的删除事件"

        [System.ComponentModel.Description("我的删除事件")]
        public event EventHandler CDeleteClick;
        protected void OnCDelete(object sender, EventArgs e)
        {
            if (CDeleteClick != null)
                CDeleteClick(sender, e);
        }

        #endregion


        #region "我的保存事件"

        [System.ComponentModel.Description("我的保存事件")]
        public event EventHandler CSaveClick;
        protected void OnCSave(object sender, EventArgs e)
        {
            if (CSaveClick != null)
                CSaveClick(sender, e);
        }
        #endregion

        #region "我的取消事件"

        [System.ComponentModel.Description("我的取消事件")]
        public event EventHandler CCancelClick;
        protected void OnCCancel(object sender, EventArgs e)
        {
            if (CCancelClick != null)
                CCancelClick(sender, e);
        }

        #endregion
        #endregion

    }
}
