using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 所有用户控件基类
    /// </summary>
    public partial class UCBaseClass : UserControl
    {
        public UCBaseClass()
        {
            InitializeComponent();
        }

        public tb_MenuInfo CurMenuInfo { get; set; }
    }
}
