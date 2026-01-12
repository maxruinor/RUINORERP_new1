using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using LiveChartsCore.Geo;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.BusinessService.SmartMenuService;
using RUINORERP.UI.UserCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    public class MenuPowerHelper
    {
        #region 事件定义

        public delegate void SetQueryConditionsDelegate(object QueryDto, QueryParameter nodeParameter);
        /// <summary>
        /// 设置查询参数
        /// </summary>
        public event SetQueryConditionsDelegate OnSetQueryConditionsDelegate;

        public delegate void RelatedHandler(MenuPowerHelper power, object obj);
        public event RelatedHandler RelatedEvent;

        public delegate void OtherHandler(object obj);
        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;

        #endregion

        #region 属性和字段

        public RUINORERP.Model.Context.ApplicationContext appContext;
        private readonly MenuTracker _menuTracker;
        public readonly ILogger<MenuPowerHelper> _logger;

        /// <summary>
        /// 获取所有已加载的菜单列表，方便外部使用
        /// </summary>
        public List<tb_MenuInfo> MenuList { get; private set; }

        public System.Windows.Forms.MenuStrip MainMenu { get; set; }

        #endregion

        #region 构造函数

        public MenuPowerHelper(MenuTracker menuTracker, ILogger<MenuPowerHelper> logger, RUINORERP.Model.Context.ApplicationContext _appContext = null)
        {
            appContext = _appContext;
            _logger = logger;
            _menuTracker = menuTracker;
            MenuList = new List<tb_MenuInfo>();
        }

        #endregion

        #region 菜单管理

        tb_MenuInfoController<tb_MenuInfo> mc = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();

        /// <summary>
        /// 加载菜单
        /// </summary>
        /// <param name="ms">菜单条控件</param>
        /// <returns>加载的菜单列表</returns>
        public List<tb_MenuInfo> AddMenu(System.Windows.Forms.MenuStrip ms)
        {
            //清空当前菜单列表并重新加载
            MenuList.Clear();
            //临时创一个菜单组为空
            ToolStripMenuItem[] fixedItems = new ToolStripMenuItem[ms.Items.Count];
            //将窗口，文件等 菜单 写死的 保存到上面那个临时的组中，清空主菜单下所有子菜单。
            ms.Items.CopyTo(fixedItems, 0);
            ms.Items.Clear();
            //两套逻辑 要区分处理
            if (appContext.CurUserInfo.UserInfo.UserName == "超级管理员")
            {
                if (appContext.CurUserInfo.UserModList.Count == 0)
                {
                    var modlist = appContext.Db.CopyNew().Queryable<tb_ModuleDefinition>()
                 .Includes(a => a.tb_MenuInfos, b => b.tb_ButtonInfos)
                 .Includes(a => a.tb_MenuInfos, b => b.tb_FieldInfos)
                 .ToList();
                    appContext.CurUserInfo.UserModList = modlist;
                }
                // 没有配置时按默认的加载
                foreach (var item in appContext.CurUserInfo.UserModList)
                {
                    if (item.tb_MenuInfos != null)
                    {
                        var modmenus = item.tb_MenuInfos;
                        modmenus = modmenus.OrderBy(t => t.Sort).ToList();
                        MenuList.AddRange(modmenus);
                        LoadMenu(ms.Items, modmenus, 0);
                    }
                }
            }
            else
            {
                List<tb_MenuInfo> tempList = new List<tb_MenuInfo>();

                if (appContext.CurrentRole.tb_P4Menus == null)
                {
                    appContext.CurrentRole.tb_P4Menus = new List<tb_P4Menu>();
                }
                foreach (tb_P4Menu P4Menu in appContext.CurrentRole.tb_P4Menus)
                {
                    if (P4Menu.tb_menuinfo.IsVisble && P4Menu.IsVisble)
                    {
                        //不重复
                        if (!tempList.Contains(P4Menu.tb_menuinfo))
                        {
                            P4Menu.tb_menuinfo.tb_P4Buttons = new List<tb_P4Button>();
                            P4Menu.tb_menuinfo.tb_P4Fields = new List<tb_P4Field>();
                            P4Menu.tb_menuinfo.tb_P4Buttons = appContext.CurrentRole.tb_P4Buttons.Where(c => c.MenuID == P4Menu.MenuID).ToList();
                            P4Menu.tb_menuinfo.tb_P4Fields = appContext.CurrentRole.tb_P4Fields.Where(c => c.MenuID == P4Menu.MenuID).ToList();
                            tempList.Add(P4Menu.tb_menuinfo);
                        }
                    }
                }

                tempList = tempList.OrderBy(t => t.Sort).ToList();

                ////加入权限，有权限才加载，除初次管理员加载
                LoadMenu(ms.Items, tempList, 0);
                MenuList.AddRange(tempList);
            }

            //最后加上窗口菜单
            ms.Items.AddRange(fixedItems);
            AddwindowsCloseMenu(fixedItems);
            //注册菜单  可以多级
            RegisterMySubMenu(ms.Items);

            //返回完整的菜单列表
            return MenuList;
        }


        private void AddwindowsCloseMenu(ToolStripMenuItem[] fixedItems)
        {
            foreach (ToolStripMenuItem item in fixedItems)
            {
                if (item.Text.Contains("窗口"))
                {
                    ToolStripMenuItem windowsCloseMenu = new ToolStripMenuItem();
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
            // 使用LINQ替代委托，提高可读性和性能
            List<tb_MenuInfo> mlist = resourceList.Where(p => p.Parent_id == ID).ToList();
            var sortlist = mlist.OrderBy(t => t.Sort);

            foreach (tb_MenuInfo var in sortlist)
            {
                if (var.CaptionCN.IsNullOrEmpty())
                {
                    _logger.LogError($"{var.MenuID}{var.MenuName}菜单名称为空，请检查数据");
                    continue;
                }

                //如果是普通用户 不启用就直接返回
                if (!Program.AppContextData.IsSuperUser && !var.IsVisble)
                {
                    continue;
                }

                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Enabled = var.IsEnabled;
                if (Program.AppContextData.IsSuperUser)
                {
                    item.Enabled = true;
                }
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
                                //if (this.MainMenu == null)
                                //{
                                //    this.MainMenu = MainForm.Instance.menuStripMain;
                                //}
                                if (this.MainMenu != null)
                                {
                                    ControlWindowsMenu(this.MainMenu.Items);
                                }

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
        public async Task ExecuteEvents(tb_MenuInfo pr, object LoadItem = null, QueryParameter nodeParameter = null, object LoadItems = null)
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
                    _menuTracker.RecordMenuUsage(pr.MenuID);
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
                            if ((pr.IsVisble && pr.IsEnabled) || Program.AppContextData.IsSuperUser)
                            {
                                if (pr.BIBaseForm.IsNullOrEmpty())
                                {
                                    MainForm.Instance.uclog.AddLog("请检查菜单基类名的配置是否为空！", Global.UILogType.错误);
                                    return;
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
                                if (pr.BIBaseForm.Contains("BaseListWithTree`1"))
                                {
                                    var menu = Startup.GetFromFacByName<BaseListWithTree>(pr.FormName);
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
                                if (pr.BIBaseForm.Contains("BaseNavigatorGeneric"))
                                {
                                    //这里要区别处理两种情况。一种是普通的。一种是 财务共用表两层业务类的情况
                                    if (string.IsNullOrEmpty(pr.BizInterface))
                                    {
                                        var menu = Startup.GetFromFacByName<UserControl>(pr.FormName);
                                        page = NewPage(pr.CaptionCN, 1, menu);
                                    }
                                    else
                                    {
                                        var menu = Startup.GetFromFacByName<BaseNavigator>(pr.FormName);
                                        page = NewPage(pr.CaptionCN, 1, menu);
                                    }

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
                                    if (menu == null)
                                    {
                                        MessageBox.Show("未找到窗体:" + pr.FormName + " 请配置正确的菜单 ");
                                        return;
                                    }
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
                        if (page.Controls[0] is BaseBillEdit billEdit)
                        {
                            //billEdit.LoadDataToUI(entity);
                            // 延迟后在 UI 线程上执行 BindData
                            if (LoadItem != null)
                            {
                                await Task.Delay(600);
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
                            //确保在UI线程上执行条件设置和UI加载
                            baseQuery.BeginInvoke(new Action(() =>
                            {
                                //if (baseQuery.QueryDtoProxy==null)
                                //{
                                //    baseQuery.QueryDtoProxy = baseQuery.LoadQueryConditionToUI(5);
                                //}

                                //先设置查询条件
                                if (OnSetQueryConditionsDelegate != null)
                                {
                                    OnSetQueryConditionsDelegate(baseQuery.QueryDtoProxy, nodeParameter);
                                }
                                //然后加载UI和执行查询 上在委托将查询条件传过去 。会默认执行一个查询。
                                baseQuery.LoadQueryParametersToUI(baseQuery.QueryDtoProxy, nodeParameter);
                            }));
                        }

                        //单表列表查询参数导入
                        //传实体进去,具体在窗体那边判断    单据实体数据传入加载用
                        if (page.Controls[0] is BaseUControl listQuery && nodeParameter != null)
                        {
                            //确保在UI线程上执行条件设置和UI加载，与BaseQuery类型使用相同的优化方式
                            listQuery.BeginInvoke(new Action(() =>
                            {
                                //先设置查询条件
                                if (OnSetQueryConditionsDelegate != null)
                                {
                                    OnSetQueryConditionsDelegate(listQuery.QueryDtoProxy, nodeParameter);
                                }
                                //然后加载UI和执行查询
                                listQuery.LoadQueryParametersToUI(listQuery.QueryDtoProxy, nodeParameter);
                            }));
                        }
                        else
                        {
                            if (page.Controls[0] is BaseUControl baseUControl)
                            {
                                //确保在UI线程上加载默认参数
                                if (baseUControl.IsHandleCreated)
                                {
                                    baseUControl.BeginInvoke(new Action(() =>
                                    {
                                        //加载默认的
                                        baseUControl.LoadQueryParametersToUI(null, null);
                                    }));
                                }
                                else
                                {
                                    // 如果句柄未创建，直接调用方法（假设当前线程是UI线程）
                                    // 或者可以添加HandleCreated事件处理器，但这里简单处理
                                    baseUControl.LoadQueryParametersToUI(null, null);
                                }
                            }
                        }



                        //生产工作台 区别上面。为了不影响上面。 
                        if (page.Controls[0] is BaseQuery ucbaseQuery && LoadItems != null)
                        {
                            ucbaseQuery.LoadQueryParametersToUI(LoadItems);
                        }

                        if (page.ButtonSpecs.All(bs => bs.Type != PaletteButtonSpecStyle.Close))
                        {
                            ButtonSpecAny bs = new ButtonSpecAny();
                            bs.Type = PaletteButtonSpecStyle.Close;
                            //bs.Image=
                            bs.Click += Bs_Click;
                            page.ButtonSpecs.Add(bs);
                        }
                        cell.SelectedPage = page;
                    }

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



        /// <summary>
        /// 关闭的一个方法
        /// 关闭事件 由tab左上的小x决定的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bs_Click_old(object sender, EventArgs e)
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

                            UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseDataGridView1, genericParameterType);

                            // 使用反射创建泛型类型的实例
                            //  Type genericListType = typeof(BaseListGeneric<>).MakeGenericType(genericParameterType);
                            //object listInstance = Activator.CreateInstance(genericListType);
                            //第二个参数 换为名称，第三个是不是 将他放到上层基类中保存。意思是窗体保存对应的这个列的个性化设置？
                            ;
                        }
                    }

                    if (control.GetType() != null &&
                        (control.GetType().BaseType.Name == "BaseBillEditGeneric`2" ||
                        control.GetType().BaseType.BaseType.Name == "BaseBillEditGeneric`2"))
                    {
                        var baseBillEdit = (BaseBillEdit)control;
                        baseBillEdit.UNLock();
                    }

                    if (control.GetType() != null && (control.GetType().BaseType.Name.Contains("BaseBillQueryMC") ||
                    control.GetType().BaseType.BaseType.Name.Contains("BaseBillQueryMC")
                        ))
                    {
                        // 获取泛型参数类型
                        Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
                        if (genericArguments.Length > 0)
                        {
                            Type genericParameterType = genericArguments[0];
                            var baseUControl = (BaseQuery)control;

                            UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericParameterType);

                            if (genericArguments.Length == 2 && !genericArguments[0].Name.Equals(genericArguments[1].Name))
                            {
                                UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseSubDataGridView, genericArguments[1]);

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
                            UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericArguments[0]);
                        }
                    }

                    MainForm.Instance.kryptonDockingManager1.RemovePage(kpage.UniqueName, true);
                    kpage.Dispose();
                }
            }
        }

        TabCloseHandler closeHandler = new TabCloseHandler();

        /// <summary>
        /// 关闭的一个方法
        /// 关闭事件 由tab左上的小x决定的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bs_Click(object sender, EventArgs e)
        {
            if (sender is ButtonSpecAny btn && btn.Owner is KryptonPage kpage)
            {
                if (kpage.Controls.Count == 1)
                {
                    var control = kpage.Controls[0];
                    closeHandler.ProcessControlOnClose(control);
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



        #region 菜单事件注册

        /// <summary>
        /// 递归注册菜单事件
        /// </summary>
        /// <param name="menus">菜单项集合</param>
        void RegisterMySubMenu(ToolStripItemCollection menus)
        {
            foreach (ToolStripMenuItem m in menus)
            {
                m.Click += doWithMenu;
                m.Visible = true;
                if (m.HasDropDownItems)
                {
                    RegisterMySubMenu(m.DropDownItems);
                }
            }
        }

        #endregion

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
