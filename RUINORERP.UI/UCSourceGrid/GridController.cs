using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UCSourceGrid
{

    public class KeyDeleteController : SourceGrid.Cells.Controllers.ControllerBase
    {
        public override void OnKeyDown(SourceGrid.CellContext sender, KeyEventArgs e)
        {
            base.OnKeyDown(sender, e);

            if (e.KeyCode == Keys.Delete)
            {
                SourceGrid.Grid grid = (SourceGrid.Grid)sender.Grid;
                grid.Rows.Remove(sender.Position.Row);
            }
        }
    }
}
