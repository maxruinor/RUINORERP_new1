using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonProcess.QueryProcess
{
    public class ConsumablesButtonDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        public override void OnSetComponentDefaults()
        {
            base.OnSetComponentDefaults();
            Control.Text = "...";
        }
    }
}
