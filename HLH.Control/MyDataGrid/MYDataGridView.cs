using System.Drawing;
using System.Windows.Forms;

namespace SHControls.DataGrid
{

    //过时了，不再使用2020by
    //简单式  显示行号。奇偶显示色
    public partial class MYDataGridView : System.Windows.Forms.DataGridView
    {
        public MYDataGridView()
        {
            InitializeComponent();
            SetDataGridview();
        }



        private void SetDataGridview()
        {

            DataGridViewCellStyle c = new DataGridViewCellStyle();
            c.BackColor = Color.Yellow;
            this.AlternatingRowsDefaultCellStyle = c;
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.GridColor = Color.SkyBlue;
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            this.DefaultCellStyle.SelectionBackColor = Color.MistyRose;
            this.DefaultCellStyle.SelectionForeColor = Color.Blue;
            this.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Gold;
            this.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;


            // this.FirstDisplayedScrollingRowIndex = this.Rows[this.Rows.Count - 1].Index;
            this.RowHeadersWidth = 59;
            //this.this.Rows[this.Rows.Count + 1].Selected = true;

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Rows.Count > 0)
            {
                if (this.TopLeftHeaderCell.Value == null)
                {
                    this.TopLeftHeaderCell.Value = this.Rows.Count.ToString();
                }
                if (this.Rows.Count.ToString() != this.TopLeftHeaderCell.Value.ToString())
                {
                    this.TopLeftHeaderCell.Value = this.Rows.Count.ToString();
                }
            }
        }


        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if (Rows.Count > 0)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    DataGridViewPaintParts paintParts =
                        e.PaintParts & ~DataGridViewPaintParts.Focus;

                    e.Paint(e.ClipBounds, paintParts);
                    e.Handled = true;
                }

                if (e.ColumnIndex < 0 && e.RowIndex >= 0)
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                    Rectangle indexRect = e.CellBounds;
                    indexRect.Inflate(-2, -2);

                    TextRenderer.DrawText(e.Graphics,
                        (e.RowIndex + 1).ToString(),
                        e.CellStyle.Font,
                        indexRect,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }
            }
        }
    }
}
