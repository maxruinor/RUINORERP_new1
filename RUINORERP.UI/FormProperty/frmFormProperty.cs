using HLH.Lib.Helper;
using Netron.GraphLib;
using Netron.Xeon;
using Newtonsoft.Json;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RUINORERP.UI.FormProperty
{
    public partial class frmFormProperty : frmBase
    {
       
        public frmFormProperty()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
             
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmApproval_Load(object sender, EventArgs e)
        {

        }

 

        


       
    }
}
