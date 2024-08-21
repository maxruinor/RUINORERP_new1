using System;
using System.Windows.Forms;
namespace SourceGrid2
{
	/// <summary>
	/// CurTextBox 的摘要说明。
	/// </summary>
	public class CurTextBox:TextBox
	{
		public CurTextBox()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData==Keys.Up ||keyData==Keys.Down) 
			{
				if (this.OnCursorKey!=null) this.OnCursorKey(keyData,EventArgs.Empty);
					return true;
			}
			return base.ProcessCmdKey (ref msg, keyData);
		}
		public event EventHandler OnCursorKey;

	}
}
