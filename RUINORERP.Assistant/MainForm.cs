using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Assistant.UC;

namespace RUINORERP.Assistant
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnPermissionInfoInit_Click(object sender, EventArgs e)
        {
            UCPermissionInfoInit uc = new UCPermissionInfoInit();
            if (!panel1.Controls.Contains(uc))
            {
                UCMenuEdit menu = new UCMenuEdit();
                menu.Dock = DockStyle.Fill;
                uc.MenuPanel.Controls.Add(menu);
                uc.Dock = DockStyle.Fill;
                panel1.Controls.Add(uc);


            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            int a = 4;


            int b = a % 2;

            int counter = 0;
            int tabindex = 0;
            int counterFlag = 0;
            // 坐标X=初始坐标+(index%行显示个数-1)*适应宽度--取余数
            int LocationX = 50;
            int Hy = 0;
            int row = 0;

            // 坐标Y=初始坐标+（index/行显示个数）*适应宽度--取整除数
            int LocationY = 50;

            for (int i = 0; i < 10; i++)
            {
                counter++;
                tabindex++;
                counterFlag = counter % 2;

                if (counterFlag == 0)
                {
                    row++;
                    Hy = Hy + 150;
                    //定义位置
                    LocationX = 50 * counterFlag + 100;
                    //10行距
                    LocationY = row * 50 + 30 + Hy;

                }
                else
                {

                    //定义位置
                    LocationX = 50 * counterFlag + 100;
                    //10行距
                    LocationY = row * 50 + 30 + Hy;

                }

            }
        }
    }
}
