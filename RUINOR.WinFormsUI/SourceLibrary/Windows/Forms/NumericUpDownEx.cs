using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace SourceLibrary.Windows.Forms
{
	public class NumericUpDownEx : NumericUpDown
	{
		private System.ComponentModel.IContainer components = null;

		public NumericUpDownEx()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			UserEdit = true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		protected override void OnValidated(EventArgs e)
		{
			base.OnValidated(e);
			base.ParseEditText();
		}
	}
}

