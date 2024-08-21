using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Models;
using SourceGrid.Selection;

namespace RUINORERP.UI.UCSourceGrid
{
    // 创建一个继承自 Grid 控件的类
    public class MyGrid : Grid
    {
        private int _statisticsRow = -1;

        public MyGrid() : base()
        {
            // 设置单元格外观
            DefaultCell = new Cell(new CellBackColorAlternateView(Color.LightGray));
            Selection.EnableMultiSelection = false;

            // 添加一行来统计数据
            _statisticsRow = RowsCount;
            Rows.Insert(_statisticsRow);
            Rows[_statisticsRow].Height = 20;
            Rows[_statisticsRow].Tag = "statisticsRow";
            FixedRows = 1; // 固定第一行

            // 将第一行设置为标题行
            SetTitleRow();

            // 设置默认样式
            for (int i = 1; i < ColumnsCount; i++)
            {
                Columns[i].Width = 100;
                Columns[i].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            }

            // 设置最后一列的样式
            Columns[ColumnsCount - 1].Width = 100;
            Columns[ColumnsCount - 1].EditorType = typeof(SourceGrid.Cells.Editors.TextBox);
            Columns[ColumnsCount - 1].Editor.EnableEdit = true;
            Columns[ColumnsCount - 1].Editor.EditableMode = EditableMode.AnyKey;

            // 设置统计行
            for (int i = 1; i < ColumnsCount - 1; i++)
            {
                var sumModel = new SumCellModel();
                sumModel.DisplayFormat = "{0:C}";
                Columns[i].DataCell.Model.AddModel(sumModel);
            }

            FreezeRows = 1;
            FreezeArea = FreezeArea.TopLeft;
            
        }

        // 将第一行设置为标题行
        private void SetTitleRow()
        {
            for (int i = 1; i < ColumnsCount - 1; i++)
            {
                var header = new Header();
                header.Image = null;
                header.Caption = "Column " + i;
                Columns[i].DataCell.Model.AddModel(header);
            }
        }

        // 添加新行
        public void AddNewRow()
        {
            Rows.Insert(RowsCount - 1);
        }

        // 获取指定行中指定列的单元格
        public Cell GetCell(int row, int col)
        {
            return Rows[row][col];
        }

        // 设置指定列的汇总单元格
        public void SetSumRow(int col, object value)
        {
            Rows[_statisticsRow][col] = new Cell(value);
        }
    }
}
