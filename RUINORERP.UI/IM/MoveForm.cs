using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RUINORERP.UI.IM
{
    public partial class MoveForm : Form
    {
        /// <summary>
        /// 滚动开始点
        /// </summary>
        private Point StartPoint;
        /// <summary>
        /// 等待次数
        /// </summary>
        private int waitCount = 50;
        /// <summary>
        /// 已经等待的次数
        /// </summary>
        private int waitedCount = 0;

        /// <summary>
        /// 滚动终点
        /// </summary>
        private Point EndPoint;

        /// <summary>
        /// 当前窗体的纵坐标
        /// </summary>
        public int Y
        {
            get { return this.EndPoint.Y; }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        private FormMoveState State;

        /// <summary>
        /// 下一个状态
        /// </summary>
        private FormMoveState NextState;

        /// <summary>
        /// 窗体距离右边屏幕的距离
        /// </summary>
        private int marginRight = 50;

        /// <summary>
        /// 鼠标是否已经悬停
        /// </summary>
        private bool mouseEnter;

        /// <summary>
        /// 窗口的打开者
        /// </summary>
        public MainForm Opener;

        /// <summary>
        /// 移动终点与初始点的纵坐标的差，一般等于窗体高度
        /// </summary>
        private int endHeight = 200;

        private delegate void SetLocationHandler(Point p);
        private delegate void SetHeightHandler(int height);
        private delegate void CloseWindowHandler();

        private System.Threading.Timer upTimer;
        private System.Threading.Timer waitTimer;
        //private System.Threading.Timer downTimer;
        //private System.Threading.Timer mouseTimer;

        /// <summary>
        /// 是否使用 System.Threading.Timer 控件
        /// </summary>
        private bool useThread = false;

        /// <summary>
        /// 移动次数
        /// </summary>
        private int moveCount = 0;

        private int moveHeight = 1;

        public MoveForm()
        {
            InitializeComponent();
            if (this.useThread)
            {
                this.upTimer = new System.Threading.Timer(new System.Threading.TimerCallback(this.moveUp));
                this.waitTimer = new System.Threading.Timer(new System.Threading.TimerCallback(this.moveWait));
                //this.downTimer = new System.Threading.Timer(new System.Threading.TimerCallback(this.moveDown));
                //this.mouseTimer = new System.Threading.Timer(new System.Threading.TimerCallback(this.mouseState));
            }
        }

        private void MoveForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.label1.Visible = false;
            //置窗口的大小
            //this.Width = this.Width;
            this.Width = this.BackgroundImage.Width;

            this.Height = 200;

            //this.endHeight = this.Height;
            this.endHeight = this.BackgroundImage.Height;

            //this.Height = this.endHeight;
            //return;

            //获取当前屏幕

            Screen currentScreen = Screen.AllScreens[0];

            //设置开始位置

            this.StartPoint = new Point();
            this.StartPoint.X = currentScreen.WorkingArea.Width - this.Width - this.marginRight;
            this.StartPoint.Y = this.Opener.TopPointY;
            //设置终止位置

            this.EndPoint = new Point();
            this.EndPoint.X = this.StartPoint.X;
            this.EndPoint.Y = this.Opener.TopPointY - this.endHeight;

            //设置窗口初始位置
            this.Location = this.StartPoint;

            //设置moveUpTimer;
            if (this.useThread)
            {
                this.upTimer.Change(10, 50);
            }
            else
            {
                this.moveUpTimer.Enabled = true;
            }

            this.NextState = FormMoveState.MoveUp;
        }

        private void moveUpTimer_Tick(object sender, EventArgs e)
        {
            switch (this.NextState)
            {
                case FormMoveState.MoveUp:
                    this.moveUp(new object());
                    break;
                case FormMoveState.Waiting:
                    this.moveWait(new object());
                    break;
                case FormMoveState.MoveDown:
                    this.moveDown(new object());
                    break;
                default:
                    break;
            }

            //int a = this.Height - this.moveCount;
            //this.label1.Text = a.ToString() + " " + this.Size.Height.ToString() + "  " + this.BackgroundImage.Height.ToString();
        }

        private void moveUp(object sender)
        {
            if (this.Location.Y != this.EndPoint.Y)
            {
                moveCount++;
                Point tempPoint = new Point();
                tempPoint.X = this.Location.X;
                tempPoint.Y = this.Location.Y - this.moveHeight;
                if (this.useThread)
                {
                    this.SetLocation(tempPoint);
                    this.SetHeight(this.Height + this.moveHeight);
                }
                else
                {
                    this.Location = tempPoint;
                    //为什么要等待两次也就是两像素？因为Form。Height默认最小值为2
                    if (this.moveCount > 2)
                    {
                        this.Height += this.moveHeight;
                    }
                }
                this.State = FormMoveState.MoveUp;
            }
            else
            {
                this.NextState = FormMoveState.Waiting;
            }
        }

        private void waitingTimer_Tick(object sender, EventArgs e)
        {
            this.moveWait(new object());
        }

        private void moveWait(object sender)
        {
            if (!this.mouseStateTimer.Enabled)
            {
                this.mouseStateTimer.Enabled = true;
            }

            if (this.waitedCount < this.waitCount)
            {
                this.waitedCount++;
                this.State = FormMoveState.Waiting;
            }
            else
            {
                this.NextState = FormMoveState.MoveDown;
            }
        }

        private void moveDownTimer_Tick(object sender, EventArgs e)
        {
            this.moveDown(new object());
        }

        private void moveDown(object sender)
        {
            if (this.mouseStateTimer.Enabled)
            {
                this.mouseStateTimer.Enabled = false;
            }

            if (this.Location != this.StartPoint)
            {
                Point tempPoint = new Point();
                tempPoint.X = this.Location.X;
                tempPoint.Y = this.Location.Y + this.moveHeight;
                if (this.useThread)
                {
                    this.SetLocation(tempPoint);
                    this.SetHeight(this.Height - this.moveHeight);
                }
                else
                {
                    this.Location = tempPoint;
                    this.Height -= this.moveHeight;
                }
                this.State = FormMoveState.MoveDown;
            }
            else
            {
                if (this.useThread)
                {
                    this.upTimer.Change(-1, -1);
                    this.CloseWindow();
                }
                else
                {
                    this.moveUpTimer.Enabled = false;
                    this.Close();
                }

            }
        }

        private void MoveForm_MouseEnter(object sender, EventArgs e)
        {
            this.mouseEnter = true;
        }

        private void MoveForm_MouseLeave(object sender, EventArgs e)
        {
            this.mouseEnter = false;
        }

        private void mouseStateTimer_Tick(object sender, EventArgs e)
        {
            this.mouseState(new object());
        }

        private void mouseState(object sender)
        {
            if (this.State == FormMoveState.Waiting && this.mouseEnter)
            {
                this.moveUpTimer.Enabled = false;
            }
            else
            {
                this.moveUpTimer.Enabled = true;
            }
        }

        private void MoveForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Opener.ListMoveForms.Remove(this);
        }

        private void SetLocation(Point p)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetLocationHandler(this.SetLocation), new object[] { p });
            }
            else
            {
                this.Location = p;
            }
        }

        private void SetHeight(int height)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetHeightHandler(this.SetHeight), new object[] { height });
            }
            else
            {
                this.Height = height;
            }
        }

        private void CloseWindow()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CloseWindowHandler(this.CloseWindow));
            }
            else
            {
                this.Close();
            }
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



    }
}
