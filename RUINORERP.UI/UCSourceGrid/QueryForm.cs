using Krypton.Navigator;
using Krypton.Toolkit;
using RUINORERP.Business;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UCSourceGrid
{
    public partial class QueryForm : KryptonForm
    {

       
        public QueryForm()
        {
            InitializeComponent();
          
        }

 
        private void btnOk_Click(object sender, EventArgs e)
        {
       
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
