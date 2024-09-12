using System;

namespace SourceGrid.Cells.Controllers
{
    /// <summary>
    /// ʵ���޷����ս������Ϊ��������Ϊ�����ڶ����Ԫ��֮�乲��
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
