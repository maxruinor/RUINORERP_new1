using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
	public class CellClickEvent : SourceGrid.Cells.Controllers.ControllerBase
	{
		public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
		{
			base.OnClick(sender, e);

			//MessageBox.Show(sender.Grid, sender.DisplayText);
		}
	}
}
