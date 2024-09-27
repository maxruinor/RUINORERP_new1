using HLH.WinControl.MyTypeConverter;
using Krypton.Toolkit;
using RUINORERP.Model.Models;
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
    public partial class frmMenuPersonalization : KryptonForm
    {
        public frmMenuPersonalization()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (mp == null)
            {
                mp = new MenuPersonalization();
              
            }
            mp.QueryConditionShowColsQty = QueryShowColQty.Value;
            UserGlobalConfig.Instance.MenuPersonalizationlist[MenuPathKey] = mp;
            //保存
            UserGlobalConfig.Instance.Serialize();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string MenuPathKey { get; set; }
        MenuPersonalization mp = new MenuPersonalization();
        private void frmMenuPersonalization_Load(object sender, EventArgs e)
        {
            UserGlobalConfig.Instance.MenuPersonalizationlist.TryGetValue(MenuPathKey, out mp);
            if (mp == null)
            {
                mp = new MenuPersonalization();
                mp.QueryConditionShowColsQty = QueryShowColQty.Value;
            }
            else
            {
                QueryShowColQty.Value = mp.QueryConditionShowColsQty;
            }
        }
    }
}
