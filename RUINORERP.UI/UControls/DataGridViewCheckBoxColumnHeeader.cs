using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace RUINORERP.UI.UControls
{
    /*
     * 名称：带CheckBox的DataGridViewColumnHeaderCell
     *
     * 作者：www.cnblogs.com/edzjx
     * 
     * 时间：2009-11-29
     * 
     * 备注：cellhead用Checkbox来代替，采用的方法是checkbox
     *  
     * 申明：不足之处可以跟作者联系，任何人可以使用此源码，出于尊重作者的劳动，请保留作者信息。
     */
 
        //定义触发单击事件的委托
        public delegate void datagridviewcheckboxHeaderEventHander(object sender, datagridviewCheckboxHeaderEventArgs e);


        //定义包含列头checkbox选择状态的参数类
        public class datagridviewCheckboxHeaderEventArgs : EventArgs
        {
            private bool checkedState = false;

            public bool CheckedState
            {
                get { return checkedState; }
                set { checkedState = value; }
            }
        }



        //定义继承于DataGridViewColumnHeaderCell的类，用于绘制checkbox，定义checkbox鼠标单击事件
        public class DataGridViewCheckBoxColumnHeeaderCell : DataGridViewColumnHeaderCell
        {

            static int counts = 0;

            Point _cellLocation = new Point();



            public DataGridViewCheckBoxColumnHeeaderCell()
            {
                counts++;
                ch.CheckedChanged += new EventHandler(ch_CheckedChanged);
                ch.MouseClick += new MouseEventHandler(ch_MouseClick);
            }

            void ch_MouseClick(object sender, MouseEventArgs e)
            {
                DataGridViewCellMouseEventArgs ex = new DataGridViewCellMouseEventArgs(this.ColumnIndex, -1, this._cellLocation.X, this._cellLocation.Y, e);

                base.OnMouseClick(ex);

            }

            void ch_CheckedChanged(object sender, EventArgs e)
            {
                if (this.EnableSelectAll)
                {
                    for (int i = 0; i < this.DataGridView.Rows.Count; i++)
                    {

                        this.DataGridView[this.ColumnIndex, i].Value = this.ch.Checked;
                    }
                }
            }

            bool _EnableSelectAll = true;
            /// <summary>
            /// Gets or sets EnableSelectAll
            /// </summary>
            [DefaultValue(true)]
            public bool EnableSelectAll
            {
                get
                {

                    return _EnableSelectAll;

                }
                set
                {

                    _EnableSelectAll = value;
                }
            }


            System.Windows.Forms.CheckBox ch = new System.Windows.Forms.CheckBox();
            /// <summary>
            /// Gets or sets this's checkbox.
            /// </summary>
            [DefaultValue(true)]
            public System.Windows.Forms.CheckBox CheckBox
            {
                get
                {

                    return ch;

                }
                set
                {

                    ch = value;
                }
            }


            //绘制列头checkbox
            protected override void Paint(System.Drawing.Graphics graphics,
               System.Drawing.Rectangle clipBounds,
               System.Drawing.Rectangle cellBounds,
               int rowIndex,
               DataGridViewElementStates dataGridViewElementState,
               object value,
               object formattedValue,
               string errorText,
               DataGridViewCellStyle cellStyle,
               DataGridViewAdvancedBorderStyle advancedBorderStyle,
               DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    dataGridViewElementState, value,
                    formattedValue, errorText, cellStyle,
                    advancedBorderStyle, paintParts);

                //checkbox的方位配置。
                Rectangle checkbounds = new Rectangle();
                checkbounds.X = cellBounds.X + 1; checkbounds.Y = cellBounds.Y + 2;
                checkbounds.Height = cellBounds.Height - 3;
                if ((cellBounds.Width + cellBounds.X) > this.DataGridView.Width)
                {
                    checkbounds.Width = this.DataGridView.Width - checkbounds.X - 10;
                }
                else
                {
                    checkbounds.Width = cellBounds.Width - 10;
                }

                _cellLocation = cellBounds.Location;



                Debug.WriteLine(counts.ToString());

                //配置Check
                ch.Name = "Chbox" + counts.ToString();
                ch.Text = this.Value.ToString();
                ch.FlatStyle = System.Windows.Forms.FlatStyle.System;
                ch.UseVisualStyleBackColor = true;
                ch.Margin = new System.Windows.Forms.Padding(0);
                ch.TextAlign = ContentAlignment.TopLeft;
                ch.Bounds = checkbounds;

                this.DataGridView.Controls.Add(ch);

                Debug.WriteLine(this.DataGridView.Controls.Count + " " + this.DataGridView.Controls[ch.Name]);
                ch.BringToFront();

                this.DataGridView.CellValueChanged += new DataGridViewCellEventHandler(DataGridView_CellValueChanged);
            }

            //对于单元格单元格内容改变时，需要重绘checkbox。
            void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                if (e.ColumnIndex == this.ColumnIndex && e.RowIndex == -1)
                {
                    this.CheckBox.Text = this.Value.ToString();
                    this.CheckBox.BringToFront();
                    ((DataGridView)sender).InvalidateCell(this);
                }
                if (e.ColumnIndex == -1 && e.RowIndex == -1)
                {
                    ((DataGridView)sender).InvalidateCell(this);
                }
            }

        }

    }
 