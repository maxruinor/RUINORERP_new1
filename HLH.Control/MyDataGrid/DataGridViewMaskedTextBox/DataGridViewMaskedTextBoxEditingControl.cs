using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    //Cell�༭�࣬ʵ��IDataGridViewEditingControl�ӿ�,�ɲ���ComboBoxEditingControl��д��
    public class DataGridViewMaskedTextBoxEditingControl : MaskedTextBox, IDataGridViewEditingControl
    {
        protected int rowIndex;
        protected DataGridView dataGridView;
        protected bool valueChanged = false;

        public DataGridViewMaskedTextBoxEditingControl()
        {
            base.TabStop = false;
        }
        //��д����(MakedTextBox)��OnTextChanged����
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            NotifyDataGridViewOfValueChange();
        }
        //  ��textֵ�����仯ʱ��֪ͨDataGridView
        private void NotifyDataGridViewOfValueChange()
        {
            valueChanged = true;
            dataGridView.NotifyCurrentCellDirty(true);
        }
        /// <summary>
        /// ���ö��뷽ʽ
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        private static HorizontalAlignment translateAlignment(DataGridViewContentAlignment align)
        {
            switch (align)
            {
                case DataGridViewContentAlignment.TopLeft:
                case DataGridViewContentAlignment.MiddleLeft:
                case DataGridViewContentAlignment.BottomLeft:
                    return HorizontalAlignment.Left;

                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.BottomCenter:
                    return HorizontalAlignment.Center;

                case DataGridViewContentAlignment.TopRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.BottomRight:
                    return HorizontalAlignment.Right;
            }

            return HorizontalAlignment.Left;
        }

        /// <summary>
        /// ��Cell���༭��ʱ������ʾ
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get
            {
                return Cursors.IBeam;
            }
        }
        /// <summary>
        /// ��ȡ���������ڵ�DataGridView
        /// </summary>
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }

            set
            {
                dataGridView = value;
            }
        }

        /// <summary>
        /// ��ȡ�����ø�ʽ�����ֵ
        /// </summary>
        public object EditingControlFormattedValue
        {
            set
            {
                Text = value.ToString();
                NotifyDataGridViewOfValueChange();
            }
            get
            {
                return this.Text;
            }

        }
        /// <summary>
        /// ��ȡ�ؼ���Textֵ
        /// </summary>
        /// <param name="context">����������</param>
        /// <returns></returns>
        public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return Text;
        }

        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyData"></param>
        /// <param name="dataGridViewWantsInputKey"></param>
        /// <returns></returns>
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Right:
                    if (!(this.SelectionLength == 0
                          && this.SelectionStart == this.ToString().Length))
                    {
                        return true;
                    }
                    break;

                case Keys.Left:
                    if (!(this.SelectionLength == 0
                          && this.SelectionStart == 0))
                    {
                        return true;
                    }
                    break;

                case Keys.Home:
                case Keys.End:
                    if (this.SelectionLength != this.ToString().Length)
                    {
                        return true;
                    }
                    break;

                case Keys.Prior:
                case Keys.Next:
                    if (this.valueChanged)
                    {
                        return true;
                    }
                    break;

                case Keys.Delete:
                    if (this.SelectionLength > 0 || this.SelectionStart < this.ToString().Length)
                    {
                        return true;
                    }
                    break;
            }
            return !dataGridViewWantsInputKey;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                SelectAll();
            }
            else
            {
                this.SelectionStart = this.ToString().Length;
            }
        }
        public virtual bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// �ؼ�������
        /// </summary>
        public int EditingControlRowIndex
        {
            get
            {
                return this.rowIndex;
            }

            set
            {
                this.rowIndex = value;
            }
        }
        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <param name="dataGridViewCellStyle"></param>
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.TextAlign = translateAlignment(dataGridViewCellStyle.Alignment);
        }


        /// <summary>
        /// �Ƿ�ֵ�����˱仯
        /// </summary>
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }

            set
            {
                this.valueChanged = value;
            }
        }
    }
}
