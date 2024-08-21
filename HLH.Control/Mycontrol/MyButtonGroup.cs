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




        #region ����


        [Browsable(true), System.ComponentModel.Description("�޸��޴��־")]
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


        [Browsable(true), System.ComponentModel.Description("�����޴��־")]
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


        [Browsable(true), System.ComponentModel.Description("ɾ���޴��־")]
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


        [Browsable(true), System.ComponentModel.Description("�����޴��־")]
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

        [Browsable(true), System.ComponentModel.Description("�Ƿ�༭�ı�־")]
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

        #region �¼�

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
            if (MessageBox.Show("ȷ��Ҫɾ����", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (delNoError)
                {
                    this.OnCDelete(sender, e);
                    MessageBox.Show("ɾ���ɹ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ɾ��ʧ��!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("�޸�ʽ����ɹ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("����ʽ����ɹ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                w_EidtFlag = false;

            }

        }

        #endregion

        #region �¼�����

        #region "�ҵ��˳��¼�"


        [System.ComponentModel.Description("�ҵĲ�ѯ�¼�")]
        public event EventHandler CQueryClick;
        protected void OnQueryClick(object sender, EventArgs e)
        {
            if (CQueryClick != null)
                CQueryClick(sender, e);
        }


        [System.ComponentModel.Description("�ҵ��˳��¼�")]
        public event EventHandler CQuitClick;
        protected void OnCQuit(object sender, EventArgs e)
        {
            if (CQuitClick != null)
                CQuitClick(sender, e);
        }

        #endregion


        #region "�ҵ�����¼�"

        [System.ComponentModel.Description("�ҵ�����¼�")]
        public event EventHandler CInsertClick;
        protected void OnCInsert(object sender, EventArgs e)
        {
            if (CInsertClick != null)
                CInsertClick(sender, e);
        }

        #endregion


        #region "�ҵ��޸��¼�"

        [System.ComponentModel.Description("�ҵ��޸��¼�")]
        public event EventHandler CModifyClick;
        protected void OnCModify(object sender, EventArgs e)
        {
            if (CModifyClick != null)
                CModifyClick(sender, e);
        }

        #endregion

        #region "�ҵ�ɾ���¼�"

        [System.ComponentModel.Description("�ҵ�ɾ���¼�")]
        public event EventHandler CDeleteClick;
        protected void OnCDelete(object sender, EventArgs e)
        {
            if (CDeleteClick != null)
                CDeleteClick(sender, e);
        }

        #endregion


        #region "�ҵı����¼�"

        [System.ComponentModel.Description("�ҵı����¼�")]
        public event EventHandler CSaveClick;
        protected void OnCSave(object sender, EventArgs e)
        {
            if (CSaveClick != null)
                CSaveClick(sender, e);
        }
        #endregion

        #region "�ҵ�ȡ���¼�"

        [System.ComponentModel.Description("�ҵ�ȡ���¼�")]
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
