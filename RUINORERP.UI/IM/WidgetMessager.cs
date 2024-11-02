using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 提示窗体  苏飞
    /// </summary>
    public partial class Messager : Krypton.Toolkit.KryptonForm
    {
        public Messager()
        {
            InitializeComponent();

        }

        public string Content { get; set; } = string.Empty;

        #region //私有变量和方法

        //显示登录用户的计算机信息
        public void ShowComptureInfo()
        {
            //CUP
            //label9.Text = ComputerInfo.GetCpuID();

            //硬盘
            //label26.Text = ComputerInfo.GetDiskID();

            //IP
            //  lblIP.Text = ComputerInfo.GetIPAddress();

            //上次登录IP
            // lbloldIP.Text = ComputerInfo.GetIPAddress();

            //用户名
            // lblUser.Text = OfficeInfo.ofLogin + " 商户欢迎您";

            //计算机名称
            //label21.Text = ComputerInfo.GetComputerName();

            //操作系统
            //label23.Text = ComputerInfo.GetSystemType();

            //当前用户
            //label25.Text = ComputerInfo.GetUserName();
        }

        #endregion

        //界面加载
        private void Messages_Load(object sender, EventArgs e)
        {
            try
            {
                //让窗体加载时显示到右下角
                int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - 255;
                int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - 161;
                this.SetDesktopLocation(x, y);

                //加载显示信息
                ShowComptureInfo();
                //如果我们想不显示，就直接在窗体的load事件中加上如下语句：
                this.ShowInTaskbar = false;
                //渐变显示这里表示加载
                caozuo = "load";
                //this.Opacity = 0;
            }
            catch (Exception)
            {

            }
        }

        //关闭按钮
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //图片离开事件
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            // pictureBox1.BackgroundImage = ClientSystem.Properties.Resources.lgintop;
        }

        //图片进入事件
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            //  pictureBox1.BackgroundImage = ClientSystem.Properties.Resources.lgintop1;
        }

        //修改密码
        private void label6_Click(object sender, EventArgs e)
        {
            //ChangePwd frm = new ChangePwd();
            // frm.OfficeInfo = this.OfficeInfo;
            // frm.Show();
        }

        //系统官网
        private void label7_Click(object sender, EventArgs e)
        {
            // Process.Start("http://www.smxzc.com/");
        }

        #region//拖动无标题窗体

        private bool isMouseDown = false;
        private Point FormLocation;     //form的location
        private Point mouseOffset;      //鼠标的按下位置

        //鼠标安下
        private void Messages_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    isMouseDown = true;
                    FormLocation = this.Location;
                    mouseOffset = Control.MousePosition;
                }
            }
            catch (Exception)
            {

            }
        }

        //鼠标移动
        private void Messages_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                int _x = 0;
                int _y = 0;
                if (isMouseDown)
                {
                    Point pt = Control.MousePosition;
                    _x = mouseOffset.X - pt.X;
                    _y = mouseOffset.Y - pt.Y;

                    this.Location = new Point(FormLocation.X - _x, FormLocation.Y - _y);
                }
            }
            catch (Exception)
            {

            }
        }

        //鼠标松开
        private void Messages_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                isMouseDown = false;
            }
            catch (Exception)
            {

            }
        }
        #endregion

        //定时关闭窗体
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = true;
            caozuo = "close";//关闭窗体
        }

        //进入窗体事件
        private void Messages_MouseEnter(object sender, EventArgs e)
        {
            //停止定时关闭
            timer1.Enabled = false;
            //开始渐变加载
            caozuo = "load";
        }

        //窗体离开事件
        private void Messages_MouseLeave(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        string caozuo = "";

        //定时处理渐变的效果
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (caozuo == "load")
            {
                this.Opacity += 0.09;
            }
            else if (caozuo == "close")
            {
                this.Opacity = this.Opacity - 0.09;
                if (this.Opacity == 0)
                    this.Close();
            }
        }
    }

}

