﻿using LightTalkChatBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.PacketSpec.Legacy;
using RUINORERP.PacketSpec.Legacy;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.ClientCmdService;
using RUINORERP.Model.TransModel;
using System.Threading;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.IM
{
    public partial class UCChatBox : UserControl
    {
        public UserInfo Sender { get; set; }
        public UserInfo Receiver { get; set; }

        public UCChatBox()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            SendMessger();

        }

        private void UCChatBox_Load(object sender, EventArgs e)
        {
            Sender = MainForm.Instance.AppContext.CurrentUser;
        }

        private void SendMessger()
        {
            if (this.txtSender.Text.Trim().Length == 0)
            {
                return;
            }

            Sender = MainForm.Instance.AppContext.CurrentUser;
            if (Receiver != null)
            {
                //如果删除了。服务器上的工作流就可以删除了。
                RequestReceiveMessageCmd request = new RequestReceiveMessageCmd(CmdOperation.Send);
                request.MessageType = MessageType.IM;
                request.MessageContent = txtSender.Text;
                request.nextProcesszStep = RUINORERP.PacketSpec.Commands.NextProcesszStep.转发;
                request.ReceiverSessionID = Receiver.SessionId;
                MainForm.Instance.dispatcher.DispatchAsync(request, CancellationToken.None);

                // OriginalData od = ActionForClient.SendMessage(txtSender.Text, Receiver.SessionId, Receiver.EmpName);
                // MainForm.Instance.ecs.AddSendData(od);
                chatBox.addChatBubble(ChatBox.BubbleSide.RIGHT, txtSender.Text, Sender.姓名, Sender.SessionId, @"IMResources\Profiles\face_default.jpg");
                txtSender.Text = "";
                request.MessageReceived += (sender, e) =>
                {
                    // 处理或显示消息
                    Console.WriteLine($"Message in other component: {e.Message}");

                    chatBox.addChatBubble(ChatBox.BubbleSide.LEFT, e.Message, e.SenderName, e.SenderSessionID, @"IMResources\Profiles\face_default.jpg");
                    //MainForm.Instance.uclog.AddLog($"收到消息了" + Message);

                };
            }
            else
            {
                MainForm.Instance.PrintInfoLog("请选择要发送的对象。");
            }

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
