using LightTalkChatBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransInstruction.DataPortal;
using TransInstruction;
using TransInstruction.DataModel;

namespace RUINORERP.UI.IM
{
    public partial class UCChatBox : UserControl
    {

        public OnlineUserInfo Sender { get; set; }
        public OnlineUserInfo Receiver { get; set; }


        public UCChatBox()
        {
            InitializeComponent();
        }


        public bool 发送消息(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                TransInstruction.DataModel.OnlineUserInfo userinfo = new TransInstruction.DataModel.OnlineUserInfo();
                string sender = ByteDataAnalysis.GetString(gd.Two, ref index);
                string Message = ByteDataAnalysis.GetString(gd.Two, ref index);
                // OnlineUserInfo MainForm.Instance.ecs.UserInfos.FindLast(t => t.SessionId == sender);
                //  UCMessager.Instance.txtMessageHistory.AppendText("@" + sender + "  " + Message);
                chatBox.addChatBubble(ChatBox.BubbleSide.RIGHT, Message, sender, "110", @"temp\testProfile2.png");
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("用户登陆:" + ex.Message);
            }
            return rs;

        }



        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessger();
        }

        private void UCChatBox_Load(object sender, EventArgs e)
        {
            Sender = MainForm.Instance.ecs.CurrentUser;
        }


        private void SendMessger()
        {
            // MainForm.Instance.LoginServer();
            if (Receiver != null)
            {
                OriginalData od = ActionForClient.SendMessage(txtSender.Text, Receiver.SessionId, Receiver.EmpName);
                MainForm.Instance.ecs.AddSendData(od);
                chatBox.addChatBubble(ChatBox.BubbleSide.RIGHT, txtSender.Text, Sender.EmpName, Sender.SessionId, @"IMResources\Profiles\face_default.jpg");
                txtSender.Text = "";
            }
            else
            {
                MainForm.Instance.PrintInfoLog("请选择要发送的对象。");
            }
            //OriginalData od1 = ActionForClient.SendMessage(txtSender.Text, "传到服务器的测试", "");
            //MainForm.Instance.ecs.AddSendData(od1);
            //return;
            //
        }

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:

                        break;
                    case Keys.F1:

                        break;
                    case Keys.Enter:
                        SendMessger();
                        break;
                }

            }
            return false;
        }

    }
}
