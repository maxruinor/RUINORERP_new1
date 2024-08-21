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
        /// 编辑在用户编辑单元格时
        /// </summary>
        /// <param name="rowIndex">当前行</param>
        /// <param name="initialFormattedValue">值</param>
        /// <param name="dataGridViewCellStyle">Cell样式</param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DataGridViewMaskedTextBoxEditingControl maskedTextBoxEditing = DataGridView.EditingControl as DataGridViewMaskedTextBoxEditingControl;

            //
            // 设置MaskedTextBox特性
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
                //输入分割符
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
        /// 设置编辑状态单元格类型
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewMaskedTextBoxEditingControl);
            }
        }

        /// <summary>
        /// 正则表达式子
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
        /// 用户输入分割符
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
