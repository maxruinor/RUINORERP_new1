using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    public class DataGridViewMaskedTextBoxCell : DataGridViewTextBoxCell
    {
        private string mask;
        private char promptChar;
        private DataGridViewTriState includePrompt;
        private DataGridViewTriState includeLiterals;
        private Type validatingType;

        public DataGridViewMaskedTextBoxCell()
            : base()
        {
            this.mask = "";
            this.promptChar = '_';
            this.includePrompt = DataGridViewTriState.NotSet;
            this.includeLiterals = DataGridViewTriState.NotSet;
            this.validatingType = typeof(string);
        }
        /// <summary>
        /// �༭���û��༭��Ԫ��ʱ
        /// </summary>
        /// <param name="rowIndex">��ǰ��</param>
        /// <param name="initialFormattedValue">ֵ</param>
        /// <param name="dataGridViewCellStyle">Cell��ʽ</param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DataGridViewMaskedTextBoxEditingControl maskedTextBoxEditing = DataGridView.EditingControl as DataGridViewMaskedTextBoxEditingControl;

            //
            // ����MaskedTextBox����
            //
            DataGridViewColumn dgvColumn = this.OwningColumn;
            if (dgvColumn is DataGridViewMaskedTextBoxColumn)
            {
                DataGridViewMaskedTextBoxColumn maskedTextBoxColumn = dgvColumn as DataGridViewMaskedTextBoxColumn;

                if (string.IsNullOrEmpty(this.mask))
                {
                    maskedTextBoxEditing.Mask = maskedTextBoxColumn.Mask;
                }
                else
                {
                    maskedTextBoxEditing.Mask = this.mask;
                }

                //
                //����ָ��
                //
                maskedTextBoxEditing.PromptChar = this.PromptChar;

                if (this.ValidatingType == null)
                {
                    maskedTextBoxEditing.ValidatingType = maskedTextBoxColumn.ValidatingType;
                }
                else
                {
                    maskedTextBoxEditing.ValidatingType = this.ValidatingType;
                }

                maskedTextBoxEditing.Text = (string)this.Value;
            }
        }
        /// <summary>
        /// ���ñ༭״̬��Ԫ������
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewMaskedTextBoxEditingControl);
            }
        }

        /// <summary>
        /// ������ʽ��
        /// </summary>
        public virtual string Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                this.mask = value;
            }
        }

        /// <summary>
        /// �û�����ָ��
        /// </summary>
        public virtual char PromptChar
        {
            get
            {
                return this.promptChar;
            }
            set
            {
                this.promptChar = value;
            }
        }
        public virtual DataGridViewTriState IncludePrompt
        {
            get
            {
                return this.includePrompt;
            }
            set
            {
                this.includePrompt = value;
            }
        }
        public virtual DataGridViewTriState IncludeLiterals
        {
            get
            {
                return this.includeLiterals;
            }
            set
            {
                this.includeLiterals = value;
            }
        }

        public virtual Type ValidatingType
        {
            get
            {
                return this.validatingType;
            }
            set
            {
                this.validatingType = value;
            }
        }
        protected static bool BoolFromTri(DataGridViewTriState tri)
        {
            return (tri == DataGridViewTriState.True) ? true : false;
        }
    }
}
