using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model;
using RUINORERP.Common;

namespace RUINORERP.UI.UControls
{
    [FormMark(typeof(UCLocList), "菜单初始11化", true)]
    public partial class UCLocList : UserControl
    {
        public UCLocList()
        {
            InitializeComponent();
        }

        private void UCLocList_Load(object sender, EventArgs e)
        {
            //Tb_Unit unit = new Tb_Unit();
           // kryptonDataGridView1.DataSource = unit.FindAllList();
        }
    }
}
