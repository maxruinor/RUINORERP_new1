using SourceGrid;
using SourceGrid.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public class SourceGridDefine : ArrayList
    {
        private SourceGrid.Grid _grid;
        public Color HeadForecolor = Color.Black;
        public Color HeadBackColor = Color.FromArgb(152, 152, 200);
        public Color SummaryColor = Color.FromArgb(217, 217, 255);

        public SourceGridDefine(SourceGridDefineColumnItem[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {

                //按标题数字计算列宽
                if (cols[i].name.Length > 0)
                {
                    cols[i].width = cols[i].name.Length * 40;
                }
                this.Add(cols[i]);
            }

            //for (int i = 0; i < names.Length; i++)
            //{
            //    GridDefineColumnItem item = new GridDefineColumnItem(names[i], 0, false, null);
            //    item.name = names[i];
            //    if (currencys != null) item.currency = currencys[i];
            //    if (sobjects != null) item.selectobject = sobjects[i];
            //    if (width != null) item.width = width[i];

            //}
            //this.linecontrol = linecontrol;
        }
        new public SourceGridDefineColumnItem this[int index]
        {
            get
            {
                return (SourceGridDefineColumnItem)base[index];
            }
        }
        public SourceGridDefineColumnItem this[string name]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].name == name) return this[i];
                }
                return null;
            }
        }
        public SourceGridDefine(SourceGrid.Grid grid)
        {
            _grid = grid;
            _grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Selection = grid.Selection as SelectionBase;
            SetSelection();
            Selection.BindToGrid(_grid);

            //Border
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);

            //Views
            CellBackColorAlternate viewNormal = new CellBackColorAlternate(Color.Khaki, Color.DarkKhaki);
            viewNormal.Border = cellBorder;

            CheckBoxBackColorAlternate viewCheckBox = new CheckBoxBackColorAlternate(Color.Khaki, Color.DarkKhaki);
            viewCheckBox.Border = cellBorder;

            //ColumnHeader view
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            backHeader.BackColor = Color.Maroon;
            backHeader.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            viewColumnHeader.Background = backHeader;
            viewColumnHeader.ForeColor = Color.White;
            viewColumnHeader.Font = new Font("Comic Sans MS", 10, FontStyle.Underline);


            foreach (ColumnInfo item in grid.Columns)
            {

            }

        }

        /// <summary>
        /// 设置选中边框
        /// </summary>
        private void SetSelection()
        {
            grid.SelectionMode = SourceGrid.GridSelectionMode.Cell;

            //启用选多行
            //Grid.Selection.EnableMultiSelection = true;

            grid.TabStop = true;
            DevAge.Drawing.RectangleBorder border = Selection.Border;
            border.SetColor(_SelectionBorderColor); //边框颜色
            border.SetWidth(1);//边框
            Selection.Border.SetDashStyle(System.Drawing.Drawing2D.DashStyle.Custom);
            Selection.Border = border;

            //焦点背景色
            Selection.FocusBackColor = _FocusBackColor;
            //焦点背景色透明值
            Selection.FocusBackColor = Color.FromArgb(60, _FocusBackColor);
        }


        /// <summary>
        /// 选中边框色
        /// </summary>
        private Color _SelectionBorderColor = Color.FromArgb(255, 128, 128, 192);

        private Color _FocusBackColor = Color.FromArgb(255, 0, 128, 255);

        private SelectionBase _selection;

        public SelectionBase Selection { get => _selection; set => _selection = value; }
        public Grid grid { get => _grid; set => _grid = value; }
    }


    internal class CellBackColorAlternate : SourceGrid.Cells.Views.Cell
    {
        public CellBackColorAlternate(Color firstColor, Color secondColor)
        {
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(firstColor);
            SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(secondColor);
        }

        private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
        public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
        {
            get { return mFirstBackground; }
            set { mFirstBackground = value; }
        }

        private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
        public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
        {
            get { return mSecondBackground; }
            set { mSecondBackground = value; }
        }

        protected override void PrepareView(SourceGrid.CellContext context)
        {
            base.PrepareView(context);

            if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                Background = FirstBackground;
            else
                Background = SecondBackground;
        }
    }

    internal class CheckBoxBackColorAlternate : SourceGrid.Cells.Views.CheckBox
    {
        public CheckBoxBackColorAlternate(Color firstColor, Color secondColor)
        {
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(firstColor);
            SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(secondColor);
        }

        private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
        public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
        {
            get { return mFirstBackground; }
            set { mFirstBackground = value; }
        }

        private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
        public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
        {
            get { return mSecondBackground; }
            set { mSecondBackground = value; }
        }

        protected override void PrepareView(SourceGrid.CellContext context)
        {
            base.PrepareView(context);

            if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                Background = FirstBackground;
            else
                Background = SecondBackground;
        }

    }


}
