using FastReport.DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.IM
{
   

    public partial class PromptMessager : Office2007Form
    {
        [DllImport("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dateTime, int dwFlags);//以动画效果绘制窗体

        #region 窗体显示参数
        private int currentX;//横坐标        

        private int currentY;//纵坐标        

        private int screenHeight;//屏幕高度        

        private int screenWidth;//屏幕宽度   

        int AW_ACTIVE = 0x20000; //激活窗口，在使用了AW_HIDE标志后不要使用这个标志     

        int AW_HIDE = 0x10000;//隐藏窗口     

        int AW_BLEND = 0x80000;// 使用淡入淡出效果     

        int AW_SLIDE = 0x40000;//使用滑动类型动画效果，默认为滚动动画类型，当使用AW_CENTER标志时，这个标志就被忽略     

        int AW_CENTER = 0x0010;//若使用了AW_HIDE标志，则使窗口向内重叠；否则向外扩展     

        int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志     

        int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志     

        int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志     

        int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志  
        #endregion
        public PromptMessager()
        {
            InitializeComponent();
        }
        public PromptMessager(string message)
        {
            InitializeComponent();
            label1.Text = message;
        }
        private void Message_Load(object sender, EventArgs e)
        {

            Rectangle rect = Screen.PrimaryScreen.WorkingArea;//获取工作区

            screenHeight = rect.Height;//获取工作区的高度

            screenWidth = rect.Width;//获取工作区的宽度

            currentX = screenWidth - this.Width; //获取绘制窗体的横坐标

            currentY = screenHeight - this.Height;//获取绘制窗体的纵坐标

            this.Location = new System.Drawing.Point(currentX, currentY);

            PDLL.AnimateWindow(this.Handle, 1000, AW_SLIDE | AW_VER_NEGATIVE);
            timer1.Enabled = true;
        }
        //这里我加了Timer控件，控制提示框的显示时间，提示窗显示一定时间就隐藏
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label1.Text = "";
            if (this.WindowState == FormWindowState.Normal)
            {
                this.Hide();
                timer1.Enabled = false;
            }


        }

    }
}
