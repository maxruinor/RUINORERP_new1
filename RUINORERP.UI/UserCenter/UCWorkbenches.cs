using FastReport.DevComponents.AdvTree;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Security;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.UI.UserCenter.DataParts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RUINORERP.UI.UserCenter
{
    public partial class UCWorkbenches : UserControl
    {
        public UCWorkbenches()
        {
            InitializeComponent();
        }

       public  KryptonDockingWorkspace ws = null;
        private NavigatorMode _mode = NavigatorMode.HeaderBarCheckButtonHeaderGroup;
        private void UCWorkbenches_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            //月销售，依客戶，依业务
            tb_RoleInfo CurrentRole = MainForm.Instance.AppContext.CurrentRole;
            tb_UserInfo CurrentUser = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
            //先取人，无人再取角色。
            tb_WorkCenterConfig centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID && c.User_ID == CurrentUser.User_ID);
            if (centerConfig == null)
            {
                centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID);
            }


            #region 请求缓存
            //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
            UIBizSrvice.RequestCache<tb_Prod>();
            UIBizSrvice.RequestCache<tb_Employee>();
            #endregion

            // Setup docking functionality
            ws = kryptonDockingManager1.ManageWorkspace(kryptonDockableWorkspaceQuery);
            kryptonDockingManager1.ManageControl(kryptonPanelMainBig, ws);
            kryptonDockingManager1.ManageFloating(MainForm.Instance);

            UCTodoList todoList = Startup.GetFromFac<UCTodoList>();

            var todoPage = UIForKryptonHelper.NewPage("代办事项", todoList);

            // Add initial docking pages
            //kryptonDockingManager1.AddToWorkspace("Workspace", new KryptonPage[] { NewDocument(), NewDocument() });
            kryptonDockingManager1.AddDockspace("Control", DockingEdge.Left, new KryptonPage[] { todoPage });
            //kryptonDockingManager1.AddDockspace("Control", DockingEdge.Bottom, new KryptonPage[] { NewInput(), NewPropertyGrid(), NewInput(), NewPropertyGrid() });

            //UpdateModeButtons();

            //创建面板并加入
            KryptonPageCollection Kpages = new KryptonPageCollection();
            if (Kpages.Count == 0)
            {
                BuilderDataOverview(Kpages, centerConfig);
            }

            //加载布局
            try
            {
                //Location of XML file
                string xmlFilePath = "UCWorkbenchesPersistence.xml";
                if (System.IO.File.Exists(xmlFilePath) && AuthorizeController.GetQueryPageLayoutCustomize(MainForm.Instance.AppContext))
                {
                    #region load
                    // Create the XmlNodeReader object.
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlFilePath);
                    XmlNodeReader nodeReader = new XmlNodeReader(doc);
                    // Set the validation settings.
                    XmlReaderSettings settings = new XmlReaderSettings();
                    //settings.ValidationType = ValidationType.Schema;
                    //settings.Schemas.Add("urn:bookstore-schema", "books.xsd");
                    //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                    //settings.NewLineChars = Environment.NewLine;
                    //settings.Indent = true;

                    using (XmlReader reader = XmlReader.Create(nodeReader, settings))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
                            {
                                //加载停靠信息
                                ws.LoadElementFromXml(reader, Kpages);
                            }
                        }

                    }
                    #endregion
                }
                else
                {
                    //没有个性化文件时用默认的
                    if (!string.IsNullOrEmpty(MainForm.Instance.AppContext.CurrentUser_Role.WorkDefaultLayout))
                    {
                        #region load
                        //加载XML文件
                        XmlDocument xmldoc = new XmlDocument();
                        //获取XML字符串
                        string xmlStr = xmldoc.InnerXml;
                        //字符串转XML
                        xmldoc.LoadXml(MainForm.Instance.AppContext.CurrentUser_Role.WorkDefaultLayout);

                        XmlNodeReader nodeReader = new XmlNodeReader(xmldoc);
                        XmlReaderSettings settings = new XmlReaderSettings();
                        using (XmlReader reader = XmlReader.Create(nodeReader, settings))
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
                                {
                                    //加载停靠信息
                                    ws.LoadElementFromXml(reader, Kpages);
                                }
                            }

                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog("加载查询页布局配置文件出错。" + ex.Message, Global.UILogType.错误);
                MainForm.Instance.logger.LogError(ex, "加载查询页布局配置文件出错。");
            }

            //如果加载过的停靠信息中不正常。就手动初始化
            foreach (KryptonPage page in Kpages)
            {
                if (!(page is KryptonStorePage) && !kryptonDockingManager1.ContainsPage(page.UniqueName))
                {
                    switch (page.UniqueName)
                    {
                        case "查询条件":
                            //kryptonDockingManager1.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());
                            kryptonDockingManager1.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());

                            break;
                        case "明细信息":
                            // kryptonDockingManager1.AddDockspace("Control", DockingEdge.Left, Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                            break;
                        case "单据信息":

                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "生产").ToArray());


                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "单据信息").ToArray());
                            break;
                        case "关联信息":

                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "关联信息").ToArray());

                            break;
                        default:
                            kryptonDockingManager1.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == page.UniqueName).ToArray());
                            break;

                    }
                }
            }


            kryptonDockingManager1.ShowPageContextMenu += new System.EventHandler<Krypton.Docking.ContextPageEventArgs>(this.kryptonDockingManager_ShowPageContextMenu);
            kryptonDockingManager1.ShowWorkspacePageContextMenu += new System.EventHandler<Krypton.Docking.ContextPageEventArgs>(this.kryptonDockingManager_ShowWorkspacePageContextMenu);
        }



        private void kryptonDockingManager_ShowPageContextMenu(object sender, ContextPageEventArgs e)
        {
            // Create a set of custom menu items
            KryptonContextMenuItems customItems = new KryptonContextMenuItems();
            KryptonContextMenuSeparator customSeparator = new KryptonContextMenuSeparator();
            KryptonContextMenuItem customItem1 = new KryptonContextMenuItem("Custom Item 1擦22", new EventHandler(OnCustomMenuItem));
            KryptonContextMenuItem customItem2 = new KryptonContextMenuItem("Custom Item 1擦2", new EventHandler(OnCustomMenuItem));
            customItem1.Tag = e.Page;
            customItem2.Tag = e.Page;
            customItems.Items.AddRange(new KryptonContextMenuItemBase[] { customSeparator, customItem1, customItem2 });

            // Add set of custom items into the provided menu
            e.KryptonContextMenu.Items.Add(customItems);
        }

        private void kryptonDockingManager_ShowWorkspacePageContextMenu(object sender, ContextPageEventArgs e)
        {
            // Create a set of custom menu items
            KryptonContextMenuItems customItems = new KryptonContextMenuItems();
            KryptonContextMenuSeparator customSeparator = new KryptonContextMenuSeparator();
            KryptonContextMenuItem customItem1 = new KryptonContextMenuItem("Custom Item 1擦3", new EventHandler(OnCustomMenuItem));
            KryptonContextMenuItem customItem2 = new KryptonContextMenuItem("Custom Item 1擦4", new EventHandler(OnCustomMenuItem));
            customItem1.Tag = e.Page;
            customItem2.Tag = e.Page;
            customItems.Items.AddRange(new KryptonContextMenuItemBase[] { customSeparator, customItem1, customItem2 });

            // Add set of custom items into the provided menu
            e.KryptonContextMenu.Items.Add(customItems);
        }

        private void OnCustomMenuItem(object sender, EventArgs e)
        {
            KryptonContextMenuItem menuItem = (KryptonContextMenuItem)sender;
            KryptonPage page = (KryptonPage)menuItem.Tag;
            MessageBox.Show("Clicked menu option '" + menuItem.Text + "' for the page '" + page.Text + "'.", "Page Context Menu");
        }


        /// <summary>
        /// 数据概览
        /// </summary>
        private void BuilderDataOverview(KryptonPageCollection Kpages, tb_WorkCenterConfig centerConfig)
        {
            if (centerConfig != null)
            {
                List<string> DataOverviewItems = centerConfig.DataOverview.Split(',').ToList();
                foreach (var item in DataOverviewItems)
                {
                    if (item.IsNullOrEmpty())
                    {
                        continue;
                    }
                    数据概览 DataOverview = (数据概览)Enum.Parse(typeof(数据概览), item);
                    switch (DataOverview)
                    {
                        case 数据概览.销售单元:
                            UCSaleCell uCSaleCell = new UCSaleCell();
                            Kpages.Add(UIForKryptonHelper.NewPage("销售单元", uCSaleCell));
                            break;
                        case 数据概览.采购单元:
                            UCPURCell uCPURCell = new UCPURCell();
                            Kpages.Add(UIForKryptonHelper.NewPage("采购单元", uCPURCell));
                            break;
                        case 数据概览.库存单元:
                            UCStockCell uCStockCell = new UCStockCell();
                            Kpages.Add(UIForKryptonHelper.NewPage("库存单元", uCStockCell));
                            break;
                        case 数据概览.生产单元:
                            UCMRPCell uCProduceCell = new UCMRPCell();
                            Kpages.Add(UIForKryptonHelper.NewPage("生产单元", uCProduceCell));
                            break;
                        default:
                            break;
                    }

                }
            }



        }
    }
}
