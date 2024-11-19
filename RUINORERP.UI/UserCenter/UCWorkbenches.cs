using Krypton.Docking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserCenter
{
    public partial class UCWorkbenches : FrmBase
    {
        public UCWorkbenches()
        {
            InitializeComponent();
        }

        private void UCWorkbenches_Load(object sender, EventArgs e)
        {
            // Setup docking functionality
            KryptonDockingWorkspace w = kryptonDockingManager1.ManageWorkspace(kryptonDockableWorkspaceQuery);
            kryptonDockingManager1.ManageControl(kryptonPanelMainBig, w);
            kryptonDockingManager1.ManageFloating(this);
        }
    }
}
