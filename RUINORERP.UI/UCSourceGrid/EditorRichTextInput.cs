using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UCSourceGrid
{
    public class EditorRichTextInput : SourceGrid.Cells.Editors.TextBoxUITypeEditor
    {
        /// <summary>
        /// Gets the control used for editing the cell.
        /// </summary>
        public new DevAge.Windows.Forms.DevAgeTextBoxButton Control
        {
            get
            {
                return (DevAge.Windows.Forms.DevAgeTextBoxButton)base.Control;
            }
        }

        public EditorRichTextInput(Type p_Type) : base(p_Type)
        {
            Control.DialogOpen += new EventHandler(Control_DialogOpen);
        }

        void Control_DialogOpen(object sender, EventArgs e)
        {

            using (RichTextForm dg = new RichTextForm())
            {
                dg.StartPosition = FormStartPosition.CenterScreen;
                if (Control.Value!=null)
                {
                    dg.InputValue = Control.Value.ToString();
                }
                
                if (dg.ShowDialog() == DialogResult.OK)
                {
                    Control.Value = dg.InputValue;
                }
            }
        }

    }
}
