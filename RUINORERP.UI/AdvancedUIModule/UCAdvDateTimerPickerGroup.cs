using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace RUINORERP.UI.AdvancedUIModule
{
    public partial class UCAdvDateTimerPickerGroup : UserControl
    {
        public UCAdvDateTimerPickerGroup()
        {
            InitializeComponent();
        }

        private void UCAdvDateTimerPickerGroup_Load(object sender, EventArgs e)
        {
            // 初始化日期控件格式
            dtp1.Format = DateTimePickerFormat.Custom;
            dtp1.CustomFormat = "yyyy-MM-dd";
            dtp2.Format = DateTimePickerFormat.Custom;
            dtp2.CustomFormat = "yyyy-MM-dd";
            
            // 确保控件可见
            this.Visible = true;
            dtp1.Visible = true;
            dtp2.Visible = true;
            kryptonLabel1.Visible = true;
            
            // 确保控件位置正确
            dtp1.Location = new Point(2, 2);
            kryptonLabel1.Location = new Point(122, 2);
            dtp2.Location = new Point(136, 2);
        }
    }
}