using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsApplication23
{
    [ToolboxBitmap(typeof(DataGridViewTextBoxColumn), "DataGridViewTextBoxColumn.bmp")]
    public class DataGridViewMaskedTextBoxColumn : DataGridViewColumn
    {
        private string mask;
        private char promptChar;
        private bool includePrompt;
        private bool includeLiterals;
        private Type validatingType;
        /// <summary>
        /// 实粒化一个Masked对象
        /// </summary>
        public DataGridViewMaskedTextBoxColumn()
            : base(new DataGridViewMaskedTextBoxCell())
        {
        }
        private static DataGridViewTriState TriBool(bool value)
        {
            return value ? DataGridViewTriState.True
                         : DataGridViewTriState.False;
        }
        /// <summary>
        /// Cell模版
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }

            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewMaskedTextBoxCell)))
                {
                    throw new InvalidCastException("类型不属于DataGridViewMaskedTextBoxCell");
                }

                base.CellTemplate = value;
            }
        }
        /// <summary>
        /// 设置正则
        /// </summary>
        public string Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                if (this.mask != value)
                {
                    this.mask = value;
                    DataGridViewMaskedTextBoxCell maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    maskedTextBoxCell.Mask = value;

                    //
                    // 把其他行的Cell的Mask属性更新
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        int rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            DataGridViewCell dataGridViewCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dataGridViewCell is DataGridViewMaskedTextBoxCell)
                            {
                                maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)dataGridViewCell;
                                maskedTextBoxCell.Mask = value;
                            }
                        }
                    }
                }
            }
        }
        public virtual char PromptChar
        {
            get
            {
                return this.promptChar;
            }
            set
            {
                if (this.promptChar != value)
                {
                    this.promptChar = value;

                    DataGridViewMaskedTextBoxCell maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    maskedTextBoxCell.PromptChar = value;
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        int rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            DataGridViewCell dataGridViewCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dataGridViewCell is DataGridViewMaskedTextBoxCell)
                            {
                                maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)dataGridViewCell;
                                maskedTextBoxCell.PromptChar = value;
                            }
                        }
                    }
                }
            }
        }
        public virtual bool IncludePrompt
        {
            get
            {
                return this.includePrompt;
            }
            set
            {
                if (this.includePrompt != value)
                {
                    this.includePrompt = value;
                    DataGridViewMaskedTextBoxCell maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    maskedTextBoxCell.IncludePrompt = TriBool(value);
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        int rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            DataGridViewCell dataGridViewCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dataGridViewCell is DataGridViewMaskedTextBoxCell)
                            {
                                maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)dataGridViewCell;
                                maskedTextBoxCell.IncludePrompt = TriBool(value);
                            }
                        }
                    }
                }
            }
        }
        public virtual bool IncludeLiterals
        {
            get
            {
                return this.includeLiterals;
            }
            set
            {
                if (this.includeLiterals != value)
                {
                    this.includeLiterals = value;

                    DataGridViewMaskedTextBoxCell maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    maskedTextBoxCell.IncludeLiterals = TriBool(value);
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {

                        int rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            DataGridViewCell dataGridViewCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dataGridViewCell is DataGridViewMaskedTextBoxCell)
                            {
                                maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)dataGridViewCell;
                                maskedTextBoxCell.IncludeLiterals = TriBool(value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证类型
        /// </summary>
        public virtual Type ValidatingType
        {
            get
            {
                return this.validatingType;
            }
            set
            {
                if (this.validatingType != value)
                {
                    this.validatingType = value;

                    DataGridViewMaskedTextBoxCell maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                    maskedTextBoxCell.ValidatingType = value;

                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        int rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            DataGridViewCell dataGridViewCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dataGridViewCell is DataGridViewMaskedTextBoxCell)
                            {
                                maskedTextBoxCell = (DataGridViewMaskedTextBoxCell)dataGridViewCell;
                                maskedTextBoxCell.ValidatingType = value;
                            }
                        }
                    }
                }
            }
        }

    }
}
