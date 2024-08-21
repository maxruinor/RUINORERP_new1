using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using TransInstruction;
using TransInstruction.DataPortal;
using TransInstruction.DataModel;
using LightTalkChatBox;
using Krypton.Navigator;
using Krypton.Toolkit;
using SourceGrid2.Win32;
using System.Web.UI.WebControls;

namespace RUINORERP.UI.IM
{
    public partial class UCMessager : UserControl
    {
        private static UCMessager _main;
        internal static UCMessager Instance
        {
            get { return _main; }
        }
        public UCMessager()
        {
            InitializeComponent();
            _main = this;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }



        private void LoadUserOnline()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(1000);

                        listboxForUsers.BeginUpdate();
                        kryptonGroupBox1.Text = "用户列表【" + listboxForUsers.Items.Count + "】";
                        if (MainForm.Instance.ecs.UserInfos.Count == 0)
                        {
                            listboxForUsers.Items.Clear();
                        }
                        foreach (var item in MainForm.Instance.ecs.UserInfos)
                        {
                            //跳过自己
                            if (item.SessionId == MainForm.Instance.ecs.CurrentUser.SessionId || item.EmpName == MainForm.Instance.ecs.CurrentUser.EmpName)
                            {
                                continue;
                            }
                            bool exist;
                            if (item != null && item.EmpName != null)
                            {
                                if (listboxForUsers.Items.Contains(item))
                                {
                                    exist = true;
                                    //更新状态 ？
                                }
                                else
                                {
                                    exist = false;
                                    //add
                                    listboxForUsers.Items.Add(item);

                                }
                            }
                        }

                        listboxForUsers.EndUpdate();
                    }
                    catch (Exception ex)
                    {
                        //Tools.ShowMsg(ex.Message + "StartSendService!#");
                        MainForm.Instance.PrintInfoLog("显示在线人数出错 ", ex);
                    }
                }
            });
        }

        private void UCMessager_Load(object sender, EventArgs e)
        {
            try
            {
                listboxForUsers.Items.Clear();
                this.Invoke(new Action(() =>
                {
                    LoadUserOnline();
                }));

            }
            catch (Exception ex)
            {

            }


        }

        private void listboxUserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //选中
        }



        #region 添加tab


        private int _count = 0;
        // Colors used when hot tracking over tabs
        private Color _hotMain = Color.FromArgb(255, 240, 200);
        private Color _hotEmbedSelected = Color.FromArgb(255, 241, 224);
        private Color _hotEmbedTracking = Color.FromArgb(255, 231, 162);

        // 8 colors for when the tab is not selected
        private Color[] _normal = new Color[]{ Color.FromArgb(156, 193, 182), Color.FromArgb(247, 184, 134),
                                               Color.FromArgb(217, 173, 194), Color.FromArgb(165, 194, 215),
                                               Color.FromArgb(179, 166, 190), Color.FromArgb(234, 214, 163),
                                               Color.FromArgb(246, 250, 125), Color.FromArgb(188, 168, 225) };

        // 8 colors for when the tab is selected
        private Color[] _select = new Color[]{ Color.FromArgb(200, 221, 215), Color.FromArgb(251, 216, 188),
                                               Color.FromArgb(234, 210, 221), Color.FromArgb(205, 221, 233),
                                               Color.FromArgb(213, 206, 219), Color.FromArgb(244, 232, 204),
                                               Color.FromArgb(250, 252, 183), Color.FromArgb(218, 207, 239) };

        private KryptonPage AddTopPage(OnlineUserInfo userInfo)
        {
            // Create a new krypton page to be added
            KryptonPage page = new KryptonPage();

            // Set the page title
            page.Text = "与" + userInfo.EmpName + "的对话";

            // Remove the default image for the page
            page.ImageSmall = null;

            // Set the padding so contained controls are indented
            page.Padding = new Padding(7);

            // Get the colors to use for this new page
            Color normal = _normal[_count % _normal.Length];
            Color select = _select[_count % _select.Length];

            // Set the page colors
            page.StateNormal.Page.Color1 = select;
            page.StateNormal.Page.Color2 = normal;
            page.StateNormal.Tab.Back.Color2 = normal;
            page.StateSelected.Tab.Back.Color2 = select;
            page.StateTracking.Tab.Back.Color2 = _hotMain;
            page.StatePressed.Tab.Back.Color2 = _hotMain;

            // We want the page drawn as a gradient with colors relative to its own area
            page.StateCommon.Page.ColorAlign = PaletteRectangleAlign.Local;
            page.StateCommon.Page.ColorStyle = PaletteColorStyle.Sigma;

            // We add an embedded navigator with its own pages to mimic OneNote operation
            // AddEmbeddedNavigator(page);
            UCChatBox chatBox = new UCChatBox();
            chatBox.Receiver = userInfo;
            chatBox.Dock = DockStyle.Fill;
            page.Controls.Add(chatBox);
            page.Name = userInfo.SessionId;
            // Add page to end of the navigator collection
            kryptonNavigator1.Pages.Add(page);

            // Bump the page index to use next
            _count++;
            return page;
        }

        /*
        private void AddEmbeddedNavigator(KryptonPage page)
        {
            // Create a navigator to embed inside the page
            KryptonNavigator nav = new KryptonNavigator();

            // We want the navigator to fill the entire page area
            nav.Dock = DockStyle.Fill;

            // Remove the close and context buttons
            nav.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            nav.Button.ButtonDisplayLogic = ButtonDisplayLogic.None;

            // Set the required tab and bar settings
            nav.Bar.BarOrientation = VisualOrientation.Right;
            nav.Bar.ItemOrientation = ButtonOrientation.FixedTop;
            nav.Bar.ItemSizing = BarItemSizing.SameWidthAndHeight;
            nav.Bar.TabBorderStyle = TabBorderStyle.RoundedEqualSmall;
            nav.Bar.TabStyle = TabStyle.StandardProfile;

            // Do not draw the bar area background, let parent page show through
            nav.StateCommon.Panel.Draw = InheritBool.False;

            // Use same font for all tab states and we want text aligned to near
            nav.StateCommon.Tab.Content.ShortText.Font = SystemFonts.IconTitleFont;
            nav.StateCommon.Tab.Content.ShortText.TextH = PaletteRelativeAlign.Near;

            // Set the page colors
            nav.StateCommon.Tab.Content.Padding = new Padding(4);
            nav.StateNormal.Tab.Back.ColorStyle = PaletteColorStyle.Linear;
            nav.StateNormal.Tab.Back.Color1 = _select[_count % _select.Length];
            nav.StateNormal.Tab.Back.Color2 = Color.White;
            nav.StateNormal.Tab.Back.ColorAngle = 270;
            nav.StateSelected.Tab.Back.ColorStyle = PaletteColorStyle.Linear;
            nav.StateSelected.Tab.Back.Color2 = _hotEmbedSelected;
            nav.StateSelected.Tab.Back.ColorAngle = 270;
            nav.StateTracking.Tab.Back.ColorStyle = PaletteColorStyle.Solid;
            nav.StateTracking.Tab.Back.Color1 = _hotEmbedTracking;
            nav.StatePressed.Tab.Back.ColorStyle = PaletteColorStyle.Solid;
            nav.StatePressed.Tab.Back.Color1 = _hotEmbedTracking;

            // Add a random number of pages
            Random rand = new Random();
            int numPages = 3 + rand.Next(5);

            for (int i = 0; i < numPages; i++)
                nav.Pages.Add(NewEmbeddedPage(_titleEmbedded[rand.Next(_titleEmbedded.Length - 1)]));

            page.Controls.Add(nav);
        }
        */

        public string 接收消息(OriginalData od)
        {
            string Message = string.Empty;
            try
            {
                try
                {
                    MainForm.Instance.Invoke(new Action(() =>
                    {
                        int index = 0;
                        ByteBuff bg = new ByteBuff(od.Two);
                        TransInstruction.DataModel.OnlineUserInfo userinfo = new TransInstruction.DataModel.OnlineUserInfo();
                        string sendtime = ByteDataAnalysis.GetString(od.Two, ref index);
                        string sessinid = ByteDataAnalysis.GetString(od.Two, ref index);
                        string MsgSender = ByteDataAnalysis.GetString(od.Two, ref index);
                        Message = ByteDataAnalysis.GetString(od.Two, ref index);
                        if (IM.UCMessager.Instance.kryptonNavigator1.Pages.Contains(sessinid))
                        {
                            if (IM.UCMessager.Instance.kryptonNavigator1.Pages[sessinid].Controls[0] is UCChatBox ucchatBox)
                            {
                                ucchatBox.chatBox.addChatBubble(ChatBox.BubbleSide.LEFT, Message, MsgSender, sessinid, @"IMResources\Profiles\face_default.jpg");
                                //MainForm.Instance.uclog.AddLog($"收到消息了" + Message);
                            }
                        }
                        else
                        {
                            //
                            KryptonPage page = AddTopPage(userinfo);
                            if (page.Controls[0] is UCChatBox ucchatBox)
                            {
                                ucchatBox.chatBox.addChatBubble(ChatBox.BubbleSide.LEFT, Message, MsgSender, sessinid, @"IMResources\Profiles\face_default.jpg");
                            }
                        }

                    }));

                }
                catch (Exception ex)
                {

                }


            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("用户登陆:" + ex.Message);
            }
            return Message;

        }




        private KryptonPage NewEmbeddedPage(string title)
        {
            KryptonPage page = new KryptonPage();
            page.Text = title;
            page.ImageSmall = null;
            return page;
        }


        #endregion

        private void listboxForUsers_DoubleClick(object sender, EventArgs e)
        {
            KryptonPage page = kryptonNavigator1.Pages.Where(c => c.Name == (listboxForUsers.SelectedItem as OnlineUserInfo).SessionId).FirstOrDefault();
            if (page != null)
            {
                kryptonNavigator1.SelectedPage = page;
            }
            else
            {
                AddTopPage(listboxForUsers.SelectedItem as OnlineUserInfo);
            }

        }
    }
}
