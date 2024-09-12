using System;

namespace SourceGrid.Cells.Controllers
{
    /// <summary>
    /// 实现无法接收焦点的行为。这种行为可以在多个单元格之间共享。
    /// </summary>
    public class Unselectable : ControllerBase
	{
		public readonly static Unselectable Default = new Unselectable();

		public override void OnFocusEntering(CellContext sender, System.ComponentModel.CancelEventArgs e)
		{
			base.OnFocusEntering (sender, e);

			e.Cancel = !CanReceiveFocus(sender, e);
		}
		public override bool CanReceiveFocus(CellContext sender, EventArgs e)
		{
			return false;
		}
	}
}
