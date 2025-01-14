using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.PSI.SAL;
using RUINORERP.UI.UserCenter;

namespace RUINORERP.UI.Common
{
    public class MenuPowerHelper
    {
        #region 添加一个事件来将查询参数设置的过程在这里实现

        //sgd 要提供当前操作的行 列 值

        public delegate void SetQueryConditionsDelegate(object QueryDto, QueryParameter nodeParameter);

        /// <summary>
        /// 设置查询参数
        /// </summary>
        public event SetQueryConditionsDelegate OnSetQueryConditionsDelegate;

        #endregion


        public RUINORERP.Model.Context.ApplicationContext appContext;

        /// <summary>
        /// 应该有基类，以后对  角色组，用户 各种方式的权限做不同处理 多态等
        /// </summary>
        /// 

        public MenuPowerHelper(RUINORERP.Model.Context.ApplicationContext _appContext = null)
        {
            appContext = _appContext;
        }


        public System.Windows.Forms.MenuStrip MainMenu { get; set; }

        List<tb_MenuInfo> MenuList = new List<tb_MenuInfo>();

        #region 添加事件功能

        public delegate void RelatedHandler(MenuPowerHelper power, object obj);

        public event RelatedHandler RelatedEvent;

        #endregion

        #region IPower 成员

        tb_MenuInfoController<tb_MenuInfo> mc = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
        /// <summary>
        /// 加载菜单明码模式 20160823优化
        /// </summary>
        /// <param name="ms"></param>
        public List<tb_MenuInfo> AddMenu(System.Windows.Forms.MenuStrip ms)
        {
            //菜单列表
            List<tb_MenuInfo> resourceList = new List<tb_MenuInfo>();
            //临时创一个菜单组为空
            ToolStripMenuItem[] fixedItems = new ToolStripMenuItem[ms.Items.Count];
            //将窗口，文件等 菜单 写死的 保存到上面那个临时的组中，清空主菜单下所有子菜单。
            ms.Items.CopyTo(fixedItems, 0);
            ms.Items.Clear();
            //两套逻辑 要区分处理
            if (appContext.IsSuperUser)
            {
                foreach (var item in appContext.CurUserInfo.UserModList)
                {
                    if (item.tb_MenuInfos != null)
                    {
                        var modmenus = item.tb_MenuInfos;
                        resourceList.AddRange(modmenus);
                        LoadMenu(ms.Items, modmenus, 0);
                    }
                }
            }
            else
            {
                if (appContext.CurUserInfo == null)
                {
                    return new();
                }
                foreach (var item in appContext.CurUserInfo.UserModList)
                {
                    if (item.tb_P4Menus != null)
                    {
                        List<tb_MenuInfo> tempList = new List<tb_MenuInfo>();
                        foreach (tb_P4Menu P4Menu in item.tb_P4Menus.Where(c => c.IsVisble).ToList())
                        {
                            //不重复
                            if (!tempList.Contains(P4Menu.tb_menuinfo))
                            {
                                tempList.Add(P4Menu.tb_menuinfo);
                            }

                        }
                        ////加入权限，有权限才加载，除初次管理员加载
                        LoadMenu(ms.Items, tempList, 0);
                        resourceList.AddRange(tempList);
                    }
                }
            }

            //最后加上窗口菜单
            ms.Items.AddRange(fixedItems);
            AddwindowsCloseMenu(fixedItems);
            //注册菜单  可以多级
            RegisterMySubMenu(ms.Items);
            return resourceList;
        }


        private void AddwindowsCloseMenu(ToolStripMenuItem[] fixedItems)
        {
            //这个时候临时的不再为空，实际为【窗口】
            foreach (ToolStripMenuItem item in fixedItems)
            {
                //暂时写死
                if (item.Text.Contains("窗口"))
                {
                    ToolStripMenuItem windowsCloseMenu = new ToolStripMenuItem();
                    // windowsMenu.Tag = var;
                    windowsCloseMenu.Text = "【关闭所有窗口(&C)】";
                    windowsCloseMenu.Click += windowsCloseMenu_Click;
                    item.DropDownItems.Add(windowsCloseMenu);
                }
            }
        }



        void windowsCloseMenu_Click(object sender, EventArgs e)
        {
            foreach (Form var in MainForm.Instance.MdiChildren)
            {
                var.Close();
            }
            ControlWindowsMenu(this.MainMenu.Items);
        }






        private void LoadMenu(ToolStripItemCollection ms, List<tb_MenuInfo> resourceList, long ID)
        {
            List<tb_MenuInfo> mlist = resourceList.FindAll(delegate (tb_MenuInfo p) { return p.Parent_id == ID; });
            var sortlist = mlist.OrderBy(t => t.Sort);

            //var sortlist = mlist.OrderBy(o => o.CaptionCN).ThenByDescending(t => t.Sort).ToList();
            //mlist = mlist.Sort(new MenuNameComparer());
            foreach (tb_MenuInfo var in sortlist)
            {
                if (var.CaptionCN.Contains("异常"))
                {

                }

                if (var.CaptionCN.Contains("审计"))
                {

                }

                if (!var.IsEnabled || !var.IsVisble)
                {
                    continue;
                }

                ToolStripMenuItem item = new ToolStripMenuItem();
                if (!string.IsNullOrEmpty(var.HotKey))
                {
                    item.Text = "" + var.MenuName + "(&" + var.HotKey + ")";
                }
                else
                {
                    item.Text = "【" + var.MenuName + "】";
                }
                //将资源类型传入，可以对具体的菜单 进行特殊处理
                item.Tag = var;
                //if (var.ResourceType == "操作")
                //{
                //    continue;
                //}
                //item.Enabled = var.IsEnabled;
                ms.Add(item);
                LoadMenu(item.DropDownItems, resourceList, var.MenuID);
            }
        }





        public delegate void OtherHandler(object obj);

        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;


        private void ControlWindowsMenu(ToolStripItemCollection menus)
        {
            //是否显示关闭窗口菜单 
            foreach (ToolStripItem item in menus)
            {
                //暂时写死
                if (item.Text.Contains("窗口"))
                {
                    if (MainForm.Instance.MdiChildren.Length > 0)
                    {
                        item.Visible = true;
                    }
                    else
                    {
                        item.Visible = false;
                    }
                }
                //暂时写死
                if (item.Text.Contains("关闭所有窗口"))
                {
                    if (MainForm.Instance.MdiChildren.Length > 0)
                    {
                        item.Visible = true;
                    }
                    else
                    {
                        item.Visible = false;
                    }
                }
                if (item is ToolStripMenuItem)
                {
                    ToolStripMenuItem subitem = item as ToolStripMenuItem;
                    if (subitem.HasDropDownItems)
                    {
                        ControlWindowsMenu(subitem.DropDownItems);
                    }
                }

            }
        }




        #region  处理菜单事件

        public void doWithMenu(object sender, EventArgs e)
        {
            if (sender.ToString() != "窗口")
            {
                if (sender is ToolStripDropDownItem)
                {
                    ToolStripDropDownItem menuitem = sender as ToolStripDropDownItem;

                    if (menuitem.Tag != null)
                    {
                        if (menuitem.Tag is tb_MenuInfo)
                        {
                            try
                            {
                                tb_MenuInfo pr = menuitem.Tag as tb_MenuInfo;
                                ///这判断 不太正确，需要完善
                                if (!string.IsNullOrEmpty(pr.ClassPath))
                                {
                                    //执行委托事件  使用这种方式是为了 在项目中不引用插件API
                                    if (OtherEvent != null)
                                    {
                                        OtherEvent(pr);
                                    }
                                    ExecuteEvents(pr, null);
                                }
                                else
                                {
                                    ExecuteEvents(pr, null);
                                }


                                ControlWindowsMenu(this.MainMenu.Items);

                                #region 新方法

                                #endregion
                            }
                            catch (Exception exx)
                            {
                                MainForm.Instance.logger.LogError("上线后完善.", exx);
                                MainForm.Instance.PrintInfoLog("上线后完善." + exx.Message + "\r\n" + exx.StackTrace);
                                MessageBox.Show("上线后完善." + exx.Message + "\r\n" + exx.StackTrace);
                            }
                        }
                        else
                        {

                            #region 老方法
                            /*

                            if (!string.IsNullOrEmpty(menuitem.Tag.ToString()))
                            {
                                Form frm = (Form)PowerGlobal.Instance.Assemblyobj.CreateInstance(menuitem.Tag.ToString());

                                if (frm == null)
                                {
                                    MessageBox.Show("类型" + menuitem.Tag.ToString() + "不存在!");
                                    return;
                                }

                                // frm.Text = frm.Text.Replace("【", "").Replace("】", "");
                                // frm.Text = "【" + frm.Text + "】";
                                frm.Text = sender.ToString();
                                foreach (Form var in PowerGlobal.Instance.Frmmain.MdiChildren)
                                {
                                    if (var.ToString() == frm.ToString())
                                    {
                                        return;
                                    }
                                }

                                frm.MdiParent = PowerGlobal.Instance.Frmmain;
                                frm.Show();
                            }
                             * */

                            #endregion

                        }

                    }
                }
            }

        }


        /// <summary>
        /// 执行事件  加载一个窗体
        /// </summary>
        /// <param name="pr">窗体菜单的信息</param>
        /// <param name="EntityDto">带查询条件或值的实体可能是查询对象本身或是代理类</param>
        /// <param name="nodeParameter">工作台待办事项点击过来带的条件</param>
        /// <param name="LoadItem">加载单实体</param>
        /// <param name="LoadItems">加载实体集合</param>
        public async void ExecuteEvents(tb_MenuInfo pr, object LoadItem = null, QueryParameter nodeParameter = null, object LoadItems = null)
        {
            if (pr == null)
            {
                return;
            }
            if (appContext.log == null)
            {
                appContext.log = new Logs();
            }
            appContext.log.ModName = pr.MenuType + "=>" + pr.CaptionCN;
            appContext.log.Path = pr.ClassPath;
            appContext.log.ActionName = "可取按钮动作等，或注入？";

            switch (pr.MenuType)
            {
                case "行为菜单":
                case "":
                    if (string.IsNullOrEmpty(pr.ClassPath) || string.IsNullOrEmpty(pr.FormName))
                    {
                        break;
                    }

                    using (StatusBusy busy = new StatusBusy("菜单功能加载中.... 请稍候"))
                    {
                        KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
                        if (cell == null)
                        {
                            cell = new KryptonWorkspaceCell();
                            MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
                        }

                        KryptonPage page;
                        page = MainForm.Instance.kryptonDockingManager1.Pages.ToList().Find(p => p.Text == pr.CaptionCN);
                        if (page == null)
                        {
                            // Create new document to be added into workspace
                            if (pr.IsVisble && pr.IsEnabled)
                            {
                                if (pr.BIBaseForm.Trim().Length == 0)
                                {
                                    MainForm.Instance.uclog.AddLog("请检查菜单基类名的配置是否为空！", Global.UILogType.错误);
                                }

                                if (pr.BIBaseForm.Contains("BaseBillEditGeneric"))
                                {
                                    //var ss = Startup.GetFromFac<RUINORERP.UI.PSI.INV.UCStocktake>();
                                    var menu = Startup.GetFromFacByName<BaseBillEdit>(pr.FormName);
                                    if (menu is BaseBillEdit bbe)
                                    {
                                        menu.CurMenuInfo = pr;

                                    }
                                    page = NewPage(pr.CaptionCN, 1, menu);
                                }
                                else

                                if (pr.BIBaseForm.Contains("BaseEditGeneric"))
                                {
                                    var menu = Startup.GetFromFacByName<Krypton.Toolkit.KryptonForm>(pr.FormName);
                                    page = NewPage(pr.CaptionCN, 1, menu);
                                }
                                else
                                if (pr.BIBaseForm.Contains("BaseListGeneric"))
                                {
                                    var menu = Startup.GetFromFacByName<BaseUControl>(pr.FormName);
                                    if (menu is BaseUControl baseListGeneric)
                                    {
                                        menu.CurMenuInfo = pr;
                                    }
                                    page = NewPage(pr.CaptionCN, 1, menu);
                                }
                                else
                                if (pr.BIBaseForm.Contains("BaseBillQueryMC"))
                                {
                                    var menu = Startup.GetFromFacByName<BaseQuery>(pr.FormName);
                                    page = NewPage(pr.CaptionCN, 1, menu);
                                }
                                else
                                if (pr.BIBaseForm.Contains("UCBaseClass"))
                                {
                                    var menu = Startup.GetFromFacByName<UCBaseClass>(pr.FormName);
                                    menu.CurMenuInfo = pr;
                                    page = NewPage(pr.CaptionCN, 1, menu);
                                }
                                else
                                if (!pr.BIBaseForm.Contains("BaseBillEditGeneric") && !pr.BIBaseForm.Contains("BaseEditGeneric") && !pr.BIBaseForm.Contains("BaseListGeneric"))
                                {
                                    var menu = Startup.GetFromFacByName<UserControl>(pr.FormName);
                                    page = NewPage(pr.CaptionCN, 1, menu);
                                }

                            }

                            page.ClearFlags(KryptonPageFlags.DockingAllowFloating);//控制托出的单独窗体是否能关掉

                        }
                        if (cell.Pages.ToList().Find(p => p.Text == pr.CaptionCN) == null)
                        {
                            cell.AllowPageDrag = true;//控制是不是能托成流体
                            cell.Bar.TabBorderStyle = Krypton.Toolkit.TabBorderStyle.OneNote;
                            cell.Pages.Add(page);
                        }

                        //传实体进去,具体在窗体那边判断    单据实体数据传入加载用
                        //if (page.Controls[0].GetType().BaseType.Name.Contains("BaseBillEditGeneric"))
                        //{
                        //    var billEditGeneric = Startup.GetFromFacByName<BaseBillEdit>(pr.FormName);
                        //    //billEdit.LoadDataToUI(entity);
                        //    // 延迟后在 UI 线程上执行 BindData
                        //    await Task.Delay(500);
                        //    billEditGeneric.Invoke(new Action(() => billEditGeneric.LoadDataToUI(entity)));
                        //    /* LoadDataToUI只能在UI线程中调用，所以需要使用Task.Run来切换到UI线程
                        //    await Task.Delay(1000); // 2000 表示2秒，单位为毫秒

                        //    // 延迟完成后执行 BindData 方法
                        //    await Task.Run(() => billEdit.LoadDataToUI(entity));*/
                        //}

                        //传实体进去,具体在窗体那边判断    单据实体数据传入加载用
                        if (page.Controls[0] is BaseBillEdit billEdit)
                        {
                            //billEdit.LoadDataToUI(entity);
                            // 延迟后在 UI 线程上执行 BindData
                            if (LoadItem != null)
                            {
                                await Task.Delay(400);
                                billEdit.Invoke(new Action(() => billEdit.LoadDataToUI(LoadItem)));
                            }

                            /* LoadDataToUI只能在UI线程中调用，所以需要使用Task.Run来切换到UI线程
                            await Task.Delay(1000); // 2000 表示2秒，单位为毫秒
                            // 延迟完成后执行 BindData 方法
                            await Task.Run(() => billEdit.LoadDataToUI(entity));*/
                        }

                        //传实体进去,具体在窗体那边判断    单据实体数据传入加载用
                        //传实体进去,具体在窗体那边判断    单据实体数据传入加载用
                        if (page.Controls[0] is BaseQuery baseQuery && nodeParameter != null)
                        {
                            //set value这里设置属性？
                            if (OnSetQueryConditionsDelegate != null)
                            {
                                OnSetQueryConditionsDelegate(baseQuery.QueryDtoProxy, nodeParameter);
                            }
                            baseQuery.LoadQueryParametersToUI(baseQuery.QueryDtoProxy, nodeParameter);
                        }

                        //单表列表查询参数导入
                        //传实体进去,具体在窗体那边判断    单据实体数据传入加载用
                        if (page.Controls[0] is BaseUControl listQuery && nodeParameter != null)
                        {
                            //set value这里设置属性？
                            if (OnSetQueryConditionsDelegate != null)
                            {
                                OnSetQueryConditionsDelegate(listQuery.QueryDtoProxy, nodeParameter);
                            }
                            listQuery.LoadQueryParametersToUI(listQuery.QueryDtoProxy, nodeParameter);
                        }
                        else
                        {
                            if (page.Controls[0] is BaseUControl baseUControl)
                            {
                                //加载默认的
                                baseUControl.LoadQueryParametersToUI(null, null);
                            }
                        }



                        //生产工作台 区别上面。为了不影响上面。 
                        if (page.Controls[0] is BaseQuery ucbaseQuery && LoadItems != null)
                        {
                            ucbaseQuery.LoadQueryParametersToUI(LoadItems);
                        }

                        if (page.ButtonSpecs.Count == 0)
                        {
                            ButtonSpecAny bs = new ButtonSpecAny();
                            bs.Type = PaletteButtonSpecStyle.Close;
                            bs.Click += Bs_Click;
                            page.ButtonSpecs.Add(bs);
                        }

                        //下面的代码没效果 哪里有问题？
                        //page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
                        //page.ClearFlags(KryptonPageFlags.All);
                        //KryptonWorkspaceCell activeCcell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
                        //if (activeCcell != null)
                        //{
                        //    activeCcell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
                        //}
                        //cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
                        cell.SelectedPage = page;
                    }

                    /*
                    foreach (var item in cell.Pages)
                    {
                        if (item.Text == pr.CaptionCN)
                        {
                            cell.SelectedPage = item;
                            return;
                        }
                    }
                    */




                    break;
                case "导航菜单":
                    //只是显示作用，不用触发动作
                    break;
                case "操作":
                    try
                    {
                        #region 按类型处理不同的菜单
                        // object obj = PowerGlobal.Instance.Assemblyobj.CreateInstance(pr.ClassPath);
                        // MethodInfo mi = obj.GetType().GetMethod(pr.MethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { }, null);
                        // mi.Invoke(obj, null);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                    break;
                default:
                    break;
            }


        }

        private void Bs_Click(object sender, EventArgs e)
        {
            if (sender is ButtonSpecAny btn && btn.Owner is KryptonPage kpage)
            {
                if (kpage.Controls.Count == 1)
                {
                    var control = kpage.Controls[0];

                    if (control.GetType() != null && control.GetType().BaseType.Name == "BaseListGeneric`1")
                    {
                        // 获取泛型参数类型
                        Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
                        if (genericArguments.Length > 0)
                        {
                            Type genericParameterType = genericArguments[0];
                            var baseUControl = (BaseUControl)control;

                            UIBizSrvice.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseDataGridView1, genericParameterType);

                            // 使用反射创建泛型类型的实例
                            //  Type genericListType = typeof(BaseListGeneric<>).MakeGenericType(genericParameterType);
                            //object listInstance = Activator.CreateInstance(genericListType);
                            //第二个参数 换为名称，第三个是不是 将他放到上层基类中保存。意思是窗体保存对应的这个列的个性化设置？
                            ;
                        }
                    }

                    if (control.GetType() != null && control.GetType().BaseType.Name.Contains("BaseBillQueryMC"))
                    {
                        // 获取泛型参数类型
                        Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
                        if (genericArguments.Length > 0)
                        {
                            Type genericParameterType = genericArguments[0];
                            var baseUControl = (BaseQuery)control;

                            UIBizSrvice.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericParameterType);

                            if (genericArguments.Length == 2 && !genericArguments[0].Name.Equals(genericArguments[1].Name))
                            {
                                UIBizSrvice.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseSubDataGridView, genericArguments[1]);

                            }

                        }
                    }
                    
                    if (control.GetType() != null && control.GetType().BaseType.Name.Contains("BaseNavigatorGeneric"))
                    {
                        // 获取泛型参数类型
                        Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
                        if (genericArguments.Length > 0)
                        {
                            Type genericParameterType = genericArguments[0];
                            var baseUControl = (BaseNavigator)control;
                            UIBizSrvice.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericArguments[0]);
                        }
                    }

                    MainForm.Instance.kryptonDockingManager1.RemovePage(kpage.UniqueName, true);
                    kpage.Dispose();
                }
            }
        }


        private KryptonPage NewPage(string name, int image, Control content)
        {
            // Create new page with title and image
            KryptonPage p = new KryptonPage();
            p.Text = name;// + 1.ToString();
            p.TextTitle = name;
            //  p.TextDescription = name + _count.ToString();
            //  p.ImageSmall = imageListSmall.Images[image];

            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);

            //  _count++;
            return p;
        }

        #endregion

        /*

         *  //通过反射来执行类的静态方法
            Type tx = typeof(ConsoleApplication1.Program);
            MethodInfo mf = tx.GetMethod("Display", BindingFlags.NonPublic|BindingFlags.Static, null, new Type[] { }, null);
            string saf = (string)mf.Invoke(null,null);

            Console.WriteLine(saf);

            Console.ReadKey();


            //通过反射来执行类的实例方法
            //string[] str = new string[2];
            Program p1 = new Program();
            Type t = p1.GetType();
             //因为此句我分析好久
            MethodInfo mi = t.GetMethod("Spec", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null);

            //通过反射执行ReturnAutoID方法，返回AutoID值
            string strx = mi.Invoke(p1, null).ToString();
            Console.WriteLine(strx);
            Console.ReadKey();
         * 
         */

        #region  注册或者注销 菜单事件

        /// <summary>
        /// 递归注册菜单事件 20160824
        /// </summary>
        /// <param name="menus"></param>
        void RegisterMySubMenu(ToolStripItemCollection menus)
        {
            foreach (ToolStripMenuItem m in menus)
            {
                m.Click += new EventHandler(doWithMenu);

                m.Visible = true;
                if (m.HasDropDownItems)
                {
                    RegisterMySubMenu(m.DropDownItems);
                }
            }
        }



        void RegisterMenu(System.Windows.Forms.ToolStripItemCollection bar)
        {
            if ("admin" == "admin")
            {
                for (int j = 0; j < bar.Count; j++)
                {
                    if (bar[j].GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                    {
                        bar[j].Click += new EventHandler(doWithMenu);
                        bar[j].Visible = true;

                    }
                }
            }

        }



        void UnRegisterMenu(System.Windows.Forms.ToolStripItemCollection bar)
        {
            if ("admin" == "admin")
            {
                for (int j = 0; j < bar.Count; j++)
                {
                    if (bar[j].GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                    {
                        bar[j].Click -= new EventHandler(doWithMenu);
                        bar[j].Visible = true;
                    }
                }
            }
        }

        #endregion

        private static List<tb_MenuInfo> roleResourceList = new List<tb_MenuInfo>();

        /// <summary>
        /// 每个用户登陆时，取他的所有的权限资源关系。缓存起来
        /// </summary>
        public static List<tb_MenuInfo> RoleResourceList
        {
            get { return roleResourceList; }
            set { roleResourceList = value; }
        }


        /// <summary>
        /// 判断是否拥有权限
        /// </summary>
        /// <param name="ownerID">所属id，包括用户id，和角色组id</param>
        /// <param name="Resource">资源id</param>
        /// <returns></returns>
        public static bool HasRights(int ownerID, int ResourceID)
        {
            /*
            RoleResourceRelationEntity Relation = new RoleResourceRelationEntity();
            if (RoleResourceList.Count == 0)
            {

                RoleResourceList = Relation.GetAllByQuery(" RoleID =" + ownerID);
            }

            Relation = RoleResourceList.Find(delegate (RoleResourceRelationEntity r) { return r.ResourceID == ResourceID && r.RoleID == ownerID; });
            if (Relation != null)
            {
                return Relation.Authorized;
            }
            else
            {
                MessageBox.Show("您没有权限执行此项操作。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }*/
            return true;
        }

        /// <summary>
        /// 判断是否拥有权限
        /// </summary>
        /// <param name="ownerID">所属id，包括用户id，和角色组id</param>
        /// <param name="Resource">资源id</param>
        /// <returns></returns>
        public static bool HasRights(int ownerID, int ResourceID, bool showNotice)
        {
            /*
            RoleResourceRelationEntity Relation = new RoleResourceRelationEntity();
            if (RoleResourceList.Count == 0)
            {

                RoleResourceList = Relation.GetAllByQuery(" RoleID =" + ownerID);
            }

            Relation = RoleResourceList.Find(delegate (RoleResourceRelationEntity r) { return r.ResourceID == ResourceID && r.RoleID == ownerID; });
            if (Relation != null)
            {
                return Relation.Authorized;
            }
            else
            {
                if (showNotice)
                {
                    MessageBox.Show("您没有权限执行此项操作。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }*/
            return true;
        }


        /// <summary>
        /// 初始化角色设置
        /// </summary>
        /// <returns></returns>
        public static bool initPower()
        {
            //删除角色
            // PowerRoleEntity entity = new PowerRoleEntity();
            // entity.Delete(" delete from PowerRole  ");
            return true;
        }


        /// <summary>
        /// 初始化角色资源关系设置
        /// </summary>
        /// <returns></returns>
        public static bool initRoleResourceRelation()
        {
            // PowerRoleEntity entity = new PowerRoleEntity();
            // entity.Delete(" delete from RoleResourceRelation  ");
            return true;

        }


        /// <summary>
        /// 初始化角色资源关系设置
        /// </summary>
        /// <returns></returns>
        public static bool initUserResourceRelation()
        {
            if (MessageBox.Show("本删除操作是不能恢复的\r你确定要删除吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                //删除角色
                //  PowerRoleEntity entity = new PowerRoleEntity();
                // entity.Delete(" delete from UserResourceRelation  ");
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
    // 自定义的比较器，按照字符串的长度进行比较
    public sealed class MenuNameComparer : IComparer<tb_MenuInfo>
    {
        public int Compare(tb_MenuInfo x, tb_MenuInfo y)
        {
            if (object.ReferenceEquals(x, y))
                return 0;
            else if (null == x)
                return -1;
            else if (null == y)
                return 1;
            else
                return string.Compare(x.CaptionCN, y.CaptionCN);
        }
    }

}
