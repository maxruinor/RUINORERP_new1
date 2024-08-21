using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceGrid;
using SourceGrid.Cells.Views;

namespace RUINORERP.UI.UCSourceGrid
{
    public class SummaryCellView : CellBackColorAlternateView
    {
        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            /*
            // 根据列的 Tag 值设置背景色
            if (context.Cell.Row.Tag == "SummaryRow")
            {
                var columnTag = context.Cell.Column.Tag;
                if (columnTag != null)
                {
                    if (columnTag.ToString() == "价格")
                    {
                        BackColor = Color.LightGray;
                    }
                    else if (columnTag.ToString() == "数量")
                    {
                        BackColor = Color.LightBlue;
                    }
                }
            }
            
             */
        }
    }

    public class CellBackColorAlternateView: SourceGrid.Cells.Views.Cell
    {
        protected override void PrepareView(SourceGrid.CellContext context)
        {
            base.PrepareView(context);
        }
    }
}
