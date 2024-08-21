using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
    public class MyToolTipModel : SourceGrid.Cells.Models.IToolTipText
    {
        public static readonly MyToolTipModel Default = new MyToolTipModel();

        public string GetToolTipText(SourceGrid.CellContext cellContext)
        {
            return "Row is selected";
        }
    }
}
