using System;
using System.Windows.Forms;
namespace SourceGrid2
{
	/// <summary>
	/// CurTextBox ��ժҪ˵����
	/// </summary>
	public class CurTextBox:TextBox
	{
		public CurTextBox()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
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
