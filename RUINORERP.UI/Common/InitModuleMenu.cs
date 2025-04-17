using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Global;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using static RUINORERP.Model.ModuleMenuDefine;
using RUINORERP.Business;
using System.Reflection;
using SqlSugar;
using System.Collections.Concurrent;
using RUINORERP.Common.Helper;
using System.Windows.Forms;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINORERP.Common;
using RUINORERP.Common.SnowflakeIdHelper;
using RUINORERP.UI.AdvancedUIModule;
using Castle.Components.DictionaryAdapter.Xml;
using System.Web.WebSockets;
using SourceGrid2.Win32;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// TODO  要完善的 这里多个地方可以批量新增 需要优化
    /// 因为与UI紧密才放到这个层
    /// </summary>
    public class InitModuleMenu
    {
        public ApplicationContext _appContext;

        public InitModuleMenu(ApplicationContext apeContext)
        {
            _appContext = apeContext;
        }

        /// <summary>
        /// 为了查找明细表名类型，保存所有类型名称方便查找
        /// </summary>
        List<string> typeNames = new List<string>();

        #region 系统级初始化菜单
        /// <summary>
        /// 定义模块 模块下定义好了对应枚举再对应上了UI
        /// 可以多次执行，但是发布后不需要每次执行
        /// 2025-1-16优化执行为批量添加 可以多次执行
        /// </summary>
        public async void InitModuleAndMenu()
        {
            //这里先提取要找到实体的类型，执行一次
            Assembly dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            ModelTypes = dalAssemble.GetExportedTypes();

            typeNames = ModelTypes.Select(m => m.Name).ToList();

            //提取到UI的类相关信息
            List<MenuAttrAssemblyInfo> MenuAssemblylist = UIHelper.RegisterForm();

            tb_ModuleDefinitionController<tb_ModuleDefinition> mdctr = _appContext.GetRequiredService<tb_ModuleDefinitionController<tb_ModuleDefinition>>();
            tb_MenuInfoController<tb_MenuInfo> mc = _appContext.GetRequiredService<tb_MenuInfoController<tb_MenuInfo>>();
            List<EnumDto> modules = new List<EnumDto>();
            modules = typeof(ModuleMenuDefine.模块定义).EnumToList();

            //检测CRM如果没有购买则不会显示
            if (!MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM))
            {
                modules = modules.Where(m => m.Name != ModuleMenuDefine.模块定义.客户关系.ToString()).ToList();
            }
            //先把相关的查出来，内存中去判断是否存在。这样可以多次执行（只能是在超级管理员或特殊用户下多次执行）普通用户是不能执行这个的
            List<tb_ModuleDefinition> ExistModuleList = new List<tb_ModuleDefinition>();
            //ExistModuleList = await mdctr.QueryByNavAsync();
            ExistModuleList = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_ModuleDefinition>()
                .Includes(c => c.tb_MenuInfos, b => b.tb_moduledefinition)
                .Includes(c => c.tb_MenuInfos, b => b.tb_ButtonInfos)
                .Includes(c => c.tb_MenuInfos, b => b.tb_FieldInfos)
                .Includes(c => c.tb_MenuInfos, b => b.tb_UIMenuPersonalizations)
                .ToListAsync();
            if (ExistModuleList == null)
            {
                ExistModuleList = new List<tb_ModuleDefinition>();
            }

            // List<tb_ModuleDefinition> WantAddModuleList = new List<tb_ModuleDefinition>();
            foreach (var item in modules)
            {
                //定义模块
                tb_ModuleDefinition mod = new tb_ModuleDefinition();
                mod.ModuleName = item.Name;
                mod.ModuleNo = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.ModuleDefinition);
                mod.Available = true;
                mod.Visible = true;
                tb_ModuleDefinition isExistt = ExistModuleList.FirstOrDefault(e => e.ModuleName == mod.ModuleName);
                if (isExistt == null)
                {
                    //mod = mdctr.AddReEntity(mod);
                    ExistModuleList.Add(mod);
                }
                else
                {
                    mod = isExistt;
                }
                if (mod.tb_MenuInfos == null)
                {
                    mod.tb_MenuInfos = new List<tb_MenuInfo>();
                }
                tb_MenuInfo menuInfoparent = new tb_MenuInfo();
                // menuInfoparent.MenuID = IdHelper.GetLongId(); //会自动生成ID 第一次这样运行出错，可能没有初始化暂时不管
                menuInfoparent.MenuName = item.Name;
                menuInfoparent.IsVisble = true;
                menuInfoparent.ModuleID = mod.ModuleID;
                menuInfoparent.IsEnabled = true;
                menuInfoparent.CaptionCN = item.Name;
                menuInfoparent.MenuType = "导航菜单";
                menuInfoparent.Parent_id = 0;

                menuInfoparent.Created_at = System.DateTime.Now;
                //tb_MenuInfo MenuInfo = mc.IsExistEntity(e => e.MenuName == menuInfoparent.MenuName && e.Parent_id == 0);
                tb_MenuInfo MenuInfo = mod.tb_MenuInfos.FirstOrDefault(e => e.MenuName == menuInfoparent.MenuName && e.Parent_id == 0);
                if (MenuInfo == null)
                {
                    //menuInfoparent = mc.AddReEntity(menuInfoparent);
                    mod.tb_MenuInfos.Add(menuInfoparent);
                }
                //else
                //{
                //    menuInfoparent = MenuInfo;
                //}

            }
            //对象中的主键会返回到本身实体中
            if (ExistModuleList.Where(c => c.ModuleID == 0).ToList().Count > 0)
            {
                List<long> ids = await MainForm.Instance.AppContext.Db.Insertable(ExistModuleList.Where(c => c.ModuleID == 0).ToList()).ExecuteReturnSnowflakeIdListAsync();
            }

            //这里是默认的第一级菜单来源于模块，默认Parent_id == 0了
            ExistModuleList.ForEach(c => c.tb_MenuInfos.ForEach(
               e =>
               {
                   e.ModuleID = c.ModuleID;
                   e.tb_moduledefinition = c;
               }
                )
            );

            List<tb_MenuInfo> WantAddMenuList = new List<tb_MenuInfo>();

            //WantAddModuleList.ForEach(c => c.tb_MenuInfos.Where(me => me.MenuID == 0).ToList().ForEach(e=>StartInitNavMenu(e, MenuAssemblylist, modules[0])));
            ExistModuleList.ForEach(c => WantAddMenuList.AddRange(c.tb_MenuInfos.Where(me => me.MenuID == 0).ToList()));

            //var menuInfos = from tb_MenuInfo menuInfo in WantAddMenuList
            //                where menuInfo.MenuID == 0
            //                select menuInfo;
            if (WantAddMenuList.Count > 0)
            {
                await MainForm.Instance.AppContext.Db.Insertable(WantAddMenuList).ExecuteReturnSnowflakeIdListAsync();
            }

            List<tb_MenuInfo> needProcessTopMenulist = new List<tb_MenuInfo>();
            //这里先保存。再循环去处理下级的每个菜单,因为下面的有菜单的上下级关联关系了,这里只处理顶级菜单
            ExistModuleList.ForEach(c => needProcessTopMenulist.AddRange(c.tb_MenuInfos.Where(c => c.Parent_id == 0).ToList()));
            foreach (var newItem in needProcessTopMenulist)
            {
                StartInitNavMenu(newItem, MenuAssemblylist, newItem.tb_moduledefinition.ModuleName);
            }

            //批量添加的参考代码  
            //if (GridSetting.UIGID == 0)
            //{

            //    await MainForm.Instance.AppContext.Db.Insertable(GridSetting).ExecuteReturnSnowflakeIdAsync();
            //}
            //else
            //{
            //    await MainForm.Instance.AppContext.Db.Updateable(GridSetting).ExecuteCommandAsync();
            //}
        }


        private void StartInitNavMenu(tb_MenuInfo menuInfoparent, List<MenuAttrAssemblyInfo> MenuAssemblylist, string modelName)
        {

            List<MenuAttrAssemblyInfo> menulist = MenuAssemblylist.Where(it => it.MenuPath.Split('|')[0] == modelName).ToList();

            模块定义 module = (模块定义)Enum.Parse(typeof(模块定义), modelName);
            switch (module)
            {
                case 模块定义.生产管理:
                    InitNavMenu<生产管理>(menuInfoparent, menulist);
                    break;
                case 模块定义.进销存管理:
                    InitNavMenu<进销存管理>(menuInfoparent, menulist);
                    break;
                case 模块定义.客户关系:
                    InitNavMenu<客户关系>(menuInfoparent, menulist);
                    break;
                case 模块定义.财务管理:
                    InitNavMenu<财务管理>(menuInfoparent, menulist);
                    break;
                case 模块定义.行政管理:
                    InitNavMenu<行政管理>(menuInfoparent, menulist);
                    break;
                case 模块定义.报表管理:
                    InitNavMenu<报表管理>(menuInfoparent, menulist);
                    break;
                case 模块定义.基础资料:
                    InitNavMenu<基础资料>(menuInfoparent, menulist);
                    break;
                case 模块定义.系统设置:
                    InitNavMenu<系统设置>(menuInfoparent, menulist);
                    break;
                default:
                    break;
            }
        }


        private async void InitNavMenu<T>(tb_MenuInfo menuParent, List<MenuAttrAssemblyInfo> list)
        {
            string parentName = typeof(T).Name;
            tb_MenuInfoController<tb_MenuInfo> mc = _appContext.GetRequiredService<tb_MenuInfoController<tb_MenuInfo>>();
            List<EnumDto> modules = new List<EnumDto>();
            modules = typeof(T).EnumToList();

            List<tb_MenuInfo> ExistMenuInfoList = new List<tb_MenuInfo>();
            if (menuParent.tb_moduledefinition.tb_MenuInfos == null)
            {
                menuParent.tb_moduledefinition.tb_MenuInfos = new List<tb_MenuInfo>();
            }
            ExistMenuInfoList = menuParent.tb_moduledefinition.tb_MenuInfos.Where(c => c.Parent_id == menuParent.MenuID).ToList();

            foreach (var item in modules)
            {
                tb_MenuInfo menuInfoNextParent = new tb_MenuInfo();
                menuInfoNextParent.ModuleID = menuParent.ModuleID;
                menuInfoNextParent.MenuName = item.Name;
                menuInfoNextParent.IsVisble = true;
                menuInfoNextParent.IsEnabled = true;
                menuInfoNextParent.CaptionCN = item.Name;
                menuInfoNextParent.MenuType = "导航菜单";
                menuInfoNextParent.Parent_id = menuParent.MenuID;
                menuInfoNextParent.Created_at = System.DateTime.Now;

                //tb_MenuInfo minfo = mc.IsExistEntity(e => e.MenuName == item.Name && e.Parent_id == menuParent.MenuID);
                tb_MenuInfo minfo = ExistMenuInfoList.FirstOrDefault(e => e.MenuName == item.Name && e.Parent_id == menuParent.MenuID);
                if (minfo == null)
                {
                    //minfo = mc.AddReEntity(menuInfoParent);
                    ExistMenuInfoList.Add(menuInfoNextParent);
                }
                //else
                //{
                //    menuInfoNextParent = minfo;
                //}

            }


            ExistMenuInfoList.ForEach(
            e =>
            {
                e.ModuleID = menuParent.tb_moduledefinition.ModuleID;
                e.Parent_id = menuParent.MenuID;
                e.tb_moduledefinition = menuParent.tb_moduledefinition;
            }
             );

            //对象中的主键会返回到本身实体中
            if (ExistMenuInfoList.Where(c => c.MenuID == 0).ToList().Count > 0)
            {
                await MainForm.Instance.AppContext.Db.Insertable(ExistMenuInfoList.Where(c => c.MenuID == 0).ToList()).ExecuteReturnSnowflakeIdListAsync();
            }


            //上面统一添加后再添加一级菜单

            foreach (tb_MenuInfo NextMenuInfo in ExistMenuInfoList)
            {
                var menulist = list.Where(it => it.MenuPath.Split('|')[0] == parentName && it.MenuPath.Split('|')[1] == NextMenuInfo.MenuName).ToList();
                foreach (var menuinfo in menulist)
                {
                    AddMenuItem(menuinfo, NextMenuInfo, mc);
                }
            }

        }



        private void AddMenuItem(MenuAttrAssemblyInfo info, tb_MenuInfo ParentMenuInfo, tb_MenuInfoController<tb_MenuInfo> mc)
        {
            List<tb_MenuInfo> ExistMenuInfoList = new List<tb_MenuInfo>();
            if (ParentMenuInfo.tb_moduledefinition.tb_MenuInfos == null)
            {
                ParentMenuInfo.tb_moduledefinition.tb_MenuInfos = new List<tb_MenuInfo>();
            }
            ExistMenuInfoList = ParentMenuInfo.tb_moduledefinition.tb_MenuInfos.Where(c => c.Parent_id == ParentMenuInfo.MenuID).ToList();


            // List<tb_MenuInfo> WantAddMenuList = new List<tb_MenuInfo>();

            //tb_MenuInfo isexist = mc.IsExistEntity(e => e.MenuName == info.Caption && e.Parent_id == ParentMenuInfo.MenuID);
            tb_MenuInfo isexist = ExistMenuInfoList.FirstOrDefault(e => e.MenuName == info.Caption && e.Parent_id == ParentMenuInfo.MenuID);
            if (isexist == null)
            {
                Model.tb_MenuInfo menu = new tb_MenuInfo();
                menu.MenuName = info.Caption;
                menu.ModuleID = ParentMenuInfo.ModuleID;
                menu.IsVisble = true;
                menu.IsEnabled = true;
                menu.CaptionCN = info.Caption;
                menu.ClassPath = info.ClassPath;
                menu.FormName = info.ClassName;
                menu.Parent_id = ParentMenuInfo.MenuID;
                menu.BIBaseForm = info.BIBaseForm;
                if (info.MenuBizType.HasValue)
                {
                    menu.BizType = (int)info.MenuBizType;
                }
                menu.MenuType = "行为菜单";
                menu.EntityName = info.EntityName;
                if (menu.BIBaseForm.Contains("BaseBillQueryMC"))
                {
                    //如果是查询式 ，添加默认的布局
                    menu.DefaultLayout = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><DW N=\"Workspace\" O=\"0\" S=\"1715, 693\"><KW V=\"1\" A=\"明细信息\"><CGD /><WS UN=\"1E691A582DF14E8443907289238B58BD\" S=\"T,0,50:T,0,50\" D=\"Horizontal\"><WS UN=\"e3f23429df664a559f8a64593e8e2221\" S=\"T,0,50:T,0,50\" D=\"Vertical\"><WC UN=\"b84cdfb9b68c401a865ecba6ce06f11a\" S=\"T,0,50:T,0,50\" NM=\"BarTabGroup\" AR=\"True\" MINS=\"150, 87\" SP=\"单据信息\"><KP UN=\"单据信息\" S=\"False\"><CPD /></KP></WC><WC UN=\"35e3228682914082bbb1a275ee166a95\" S=\"T,0,50:T,0,50\" NM=\"BarTabGroup\" AR=\"True\" MINS=\"150, 110\" SP=\"明细信息\"><KP UN=\"明细信息\" S=\"False\"><CPD /></KP></WC></WS></WS></KW></DW>";
                }
                menu.Created_at = System.DateTime.Now;
                menu = mc.AddReEntity(menu);
                // WantAddMenuList.Add(menu);
                isexist = menu;
            }
            //if (WantAddMenuList.Where(c => c.ModuleID == 0).ToList().Count > 0)
            //{
            //    await MainForm.Instance.AppContext.Db.Insertable(WantAddMenuList.Where(c => c.ModuleID == 0).ToList()).ExecuteReturnSnowflakeIdListAsync();
            //}

            //第一次添加时才添加相应的按钮
            InitToolStripItem(info, isexist);


            InitFieldIno(info, isexist);



        }





        public async void InitToolStripItem(MenuAttrAssemblyInfo info, tb_MenuInfo menuInfo)
        {
            Control c = Startup.ServiceProvider.GetService(info.ClassType) as Control;
            tb_ButtonInfoController<tb_ButtonInfo> BtnController = Startup.GetFromFac<tb_ButtonInfoController<tb_ButtonInfo>>();
            var btns = FindControls<ToolStrip>(c);

            if (menuInfo.tb_ButtonInfos == null)
            {
                menuInfo.tb_ButtonInfos = new List<tb_ButtonInfo>();
            }


            List<tb_ButtonInfo> tb_ButtonInfos = new List<tb_ButtonInfo>();
            foreach (var item in btns)
            {
                if (item.GetType().Name == "ToolStrip")
                {

                    foreach (var btnItem in item.Items)
                    {
                        if (btnItem is ToolStripButton btn)
                        {
                            //按钮名至少大于1
                            if (btn.Text.Trim().Length > 1)
                            {
                                //添加
                                tb_ButtonInfo btnInfo = new tb_ButtonInfo();
                                btnInfo.BtnName = btn.Name;
                                btnInfo.BtnText = btn.Text;
                                btnInfo.FormName = info.ClassName;
                                btnInfo.ClassPath = info.ClassPath;
                                btnInfo.MenuID = menuInfo.MenuID;
                                btnInfo.IsEnabled = true;

                                //tb_ButtonInfo ExistBtnInfo = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == btn.Text && it.MenuID == menuInfo.MenuID);
                                tb_ButtonInfo ExistBtnInfo = menuInfo.tb_ButtonInfos.FirstOrDefault(it => it.ClassPath == info.ClassPath && it.BtnText == btn.Text && it.MenuID == menuInfo.MenuID);
                                if (ExistBtnInfo == null)
                                {
                                    //tnController.AddReEntity(btnInfo);
                                    tb_ButtonInfos.Add(btnInfo);
                                }
                                //else
                                //{
                                //    btnInfo = ExistBtnInfo;
                                //}
                            }
                        }

                        else if (btnItem is ToolStripSplitButton btnSplit)
                        {
                            //按钮名至少大于1
                            if (btnSplit.Text.Trim().Length > 1)
                            {
                                //添加
                                tb_ButtonInfo btnInfo = new tb_ButtonInfo();
                                btnInfo.BtnName = btnSplit.Name;
                                btnInfo.BtnText = btnSplit.Text;
                                btnInfo.FormName = info.ClassName;
                                btnInfo.ClassPath = info.ClassPath;
                                btnInfo.MenuID = menuInfo.MenuID;
                                btnInfo.IsEnabled = true;
                                //tb_ButtonInfo ExistBtnInfo = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == btnSplit.Text && it.MenuID == menuInfo.MenuID);
                                tb_ButtonInfo ExistBtnInfo = menuInfo.tb_ButtonInfos.FirstOrDefault(it => it.ClassPath == info.ClassPath && it.BtnText == btnSplit.Text && it.MenuID == menuInfo.MenuID);
                                if (ExistBtnInfo == null)
                                {
                                    tb_ButtonInfos.Add(btnInfo);
                                }
                                foreach (ToolStripItem tsi in btnSplit.DropDownItems)
                                {
                                    //添加
                                    tb_ButtonInfo btnInfoSub = new tb_ButtonInfo();
                                    btnInfoSub.BtnName = tsi.Name;
                                    btnInfoSub.BtnText = tsi.Text;
                                    btnInfoSub.FormName = info.ClassName;
                                    btnInfoSub.ClassPath = info.ClassPath;
                                    btnInfoSub.MenuID = menuInfo.MenuID;
                                    btnInfoSub.IsEnabled = true;
                                    //tb_ButtonInfo ExistBtnInfoSub = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == btnInfoSub.BtnText && it.MenuID == menuInfo.MenuID);
                                    tb_ButtonInfo ExistBtnInfoSub = menuInfo.tb_ButtonInfos.FirstOrDefault(it => it.ClassPath == info.ClassPath && it.BtnText == btnInfoSub.BtnText && it.MenuID == menuInfo.MenuID);
                                    if (ExistBtnInfoSub == null)
                                    {
                                        tb_ButtonInfos.Add(btnInfoSub);
                                    }
                                }
                            }
                        }
                        else
                        {   //暂时只有二级。不用循环了
                            if (btnItem is ToolStripDropDownButton btnddb)
                            {
                                //添加
                                tb_ButtonInfo btnInfoDrop = new tb_ButtonInfo();
                                btnInfoDrop.BtnName = btnddb.Name;
                                btnInfoDrop.BtnText = btnddb.Text;
                                btnInfoDrop.FormName = info.ClassName;
                                btnInfoDrop.ClassPath = info.ClassPath;
                                btnInfoDrop.MenuID = menuInfo.MenuID;
                                btnInfoDrop.IsEnabled = true;
                                //tb_ButtonInfo ExistBtnInfoDrop = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == btnddb.Text && it.MenuID == menuInfo.MenuID);
                                tb_ButtonInfo ExistBtnInfoDrop = menuInfo.tb_ButtonInfos.FirstOrDefault(it => it.ClassPath == info.ClassPath && it.BtnText == btnddb.Text && it.MenuID == menuInfo.MenuID);
                                if (ExistBtnInfoDrop == null)
                                {
                                    //tnController.AddReEntity(btnInfo);
                                    tb_ButtonInfos.Add(btnInfoDrop);
                                }

                                foreach (ToolStripItem tsi in btnddb.DropDownItems)
                                {
                                    //添加
                                    tb_ButtonInfo btnInfo = new tb_ButtonInfo();
                                    btnInfo.BtnName = tsi.Name;
                                    btnInfo.BtnText = tsi.Text;
                                    btnInfo.FormName = info.ClassName;
                                    btnInfo.ClassPath = info.ClassPath;
                                    btnInfo.MenuID = menuInfo.MenuID;
                                    btnInfo.IsEnabled = true;
                                    //tb_ButtonInfo ExistBtnInfo = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == tsi.Text && it.MenuID == menuInfo.MenuID);
                                    tb_ButtonInfo ExistBtnInfo = menuInfo.tb_ButtonInfos.FirstOrDefault(it => it.ClassPath == info.ClassPath && it.BtnText == tsi.Text && it.MenuID == menuInfo.MenuID);
                                    if (ExistBtnInfo == null)
                                    {
                                        //tnController.AddReEntity(btnInfo);
                                        tb_ButtonInfos.Add(btnInfo);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            if (c is IToolStripMenuInfoAuth)
            {
                IToolStripMenuInfoAuth formAuth = c as IToolStripMenuInfoAuth;
                if (formAuth != null)
                {
                    ToolStripItem[] stripItems = formAuth.AddExtendButton(menuInfo);
                    foreach (var tsitem in stripItems)
                    {
                        //添加
                        tb_ButtonInfo btnInfoSub = new tb_ButtonInfo();
                        btnInfoSub.BtnName = tsitem.Name;
                        btnInfoSub.BtnText = tsitem.Text;
                        btnInfoSub.FormName = info.ClassName;
                        btnInfoSub.ClassPath = info.ClassPath;
                        btnInfoSub.MenuID = menuInfo.MenuID;
                        btnInfoSub.IsEnabled = true;
                        //tb_ButtonInfo ExistBtnInfoSub = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == btnInfoSub.BtnText && it.MenuID == menuInfo.MenuID);
                        tb_ButtonInfo ExistBtnInfoSub = menuInfo.tb_ButtonInfos.FirstOrDefault(it => it.ClassPath == info.ClassPath && it.BtnText == btnInfoSub.BtnText && it.MenuID == menuInfo.MenuID);
                        if (ExistBtnInfoSub == null)
                        {
                            tb_ButtonInfos.Add(btnInfoSub);
                        }
                    }
                }
            }

            if (c is IContextMenuInfoAuth)
            {
                IContextMenuInfoAuth formAuth = c as IContextMenuInfoAuth;
                if (formAuth != null)
                {
                    List<UI.UControls.ContextMenuController> ContextMenuItems = formAuth.AddContextMenu();
                    foreach (var tsitem in ContextMenuItems)
                    {
                        //添加
                        tb_ButtonInfo btnInfoSub = new tb_ButtonInfo();
                        btnInfoSub.BtnName = tsitem.MenuText;
                        btnInfoSub.BtnText = tsitem.MenuText;
                        btnInfoSub.FormName = info.ClassName;
                        btnInfoSub.ClassPath = info.ClassPath;
                        btnInfoSub.MenuID = menuInfo.MenuID;
                        btnInfoSub.ButtonType = ButtonType.ContextMenu.ToString();
                        btnInfoSub.IsEnabled = true;
                        //tb_ButtonInfo ExistBtnInfoSub = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == btnInfoSub.BtnText && it.MenuID == menuInfo.MenuID);
                        tb_ButtonInfo ExistBtnInfoSub = menuInfo.tb_ButtonInfos.FirstOrDefault(it => it.ClassPath == info.ClassPath
                        && it.ButtonType == ButtonType.ContextMenu.ToString()
                        && it.BtnText == btnInfoSub.BtnText && it.MenuID == menuInfo.MenuID);
                        if (ExistBtnInfoSub == null)
                        {
                            tb_ButtonInfos.Add(btnInfoSub);
                        }
                    }
                }
            }

            if (tb_ButtonInfos.Count > 0)
            {
                List<long> idsbtn = await _appContext.Db.CopyNew().Insertable<tb_ButtonInfo>(tb_ButtonInfos).ExecuteReturnSnowflakeIdListAsync();
                if (idsbtn.Count == tb_ButtonInfos.Count)
                {
                    //添加后才不会重复添加
                    menuInfo.tb_ButtonInfos.AddRange(tb_ButtonInfos);
                }

            }

        }

        /// <summary>
        /// 遍历所有控件树递,归所有子控件，查找所有包含指定控件类型的控件，并返回所有包含指定控件类型的控件的列表。调用该方法，获取所有按钮控件：var buttons = FindControls<Button>(this);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public List<T> FindControls<T>(Control control) where T : ToolStrip //Control
        {
            List<T> controls = new List<T>();

            // 遍历控件集合
            foreach (Control ctrl in control.Controls)
            {
                // 判断控件类型是否为指定类型
                if (ctrl is T tControl)
                {
                    // 添加符合条件的控件
                    controls.Add(tControl);
                }

                // 如果控件还有子控件，则递归调用此方法
                if (ctrl.HasChildren)
                {
                    controls.AddRange(FindControls<T>(ctrl));
                }
            }

            // 返回符合条件的控件列表
            return controls;
        }


        tb_FieldInfoController<tb_FieldInfo> fieldController = Startup.GetFromFac<tb_FieldInfoController<tb_FieldInfo>>();


        Type[] ModelTypes;

        /// <summary>
        /// 初始化字段
        /// </summary>
        private void InitFieldIno(MenuAttrAssemblyInfo Info, tb_MenuInfo menuInfo)
        {
            try
            {
                // Type[] ModelTypes
                foreach (Type type in ModelTypes)
                {
                    //如果不是指定菜单对应的实体就不加入字段
                    if (type.FullName.Contains("QueryDto"))
                    {
                        continue;
                    }

                    //如果不是指定菜单对应的实体就不加入字段
                    if (type.Name != Info.EntityName)
                    {
                        continue;
                    }

                    InitFieldInoMainAndSub(type, menuInfo, false, "");
                    //尝试找子表类型
                    string childType = typeNames.FirstOrDefault(s => s.Contains(type.Name + "Detail"));
                    if (!string.IsNullOrEmpty(childType))
                    {
                        // type.FullName + "Detail";
                        //Type cType = System.Reflection.Assembly.Load("RUINORERP.Model.dll").GetType(type.FullName + "Detail");
                        Type cType = ModelTypes.FirstOrDefault(t => t.FullName == type.FullName + "Detail");
                        if (cType != null)
                        {
                            InitFieldInoMainAndSub(cType, menuInfo, true, childType);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog(ex.Message, UILogType.错误);
            }
        }

        public async void InitFieldInoMainAndSub(Type type, tb_MenuInfo menuInfo, bool isChild, string childType)
        {
            List<tb_FieldInfo> fields = new List<tb_FieldInfo>();
            var attrs = type.GetCustomAttributes<SugarTable>();
            foreach (var attr in attrs)
            {
                if (attr is SugarTable)
                {
                    //var t = Startup.ServiceProvider.GetService(type);//SugarColumn 或进一步取字段特性也可以
                    var t = Startup.ServiceProvider.CreateInstance(type);//SugarColumn 或进一步取字段特性也可以
                    ConcurrentDictionary<string, string> cd = ReflectionHelper.GetPropertyValue(t, "FieldNameList") as ConcurrentDictionary<string, string>;
                    if (cd == null)
                    {
                        cd = new ConcurrentDictionary<string, string>();
                        SugarColumn entityAttr;
                        foreach (PropertyInfo field in type.GetProperties())
                        {
                            foreach (Attribute attrField in field.GetCustomAttributes(true))
                            {
                                entityAttr = attrField as SugarColumn;
                                if (null != entityAttr)
                                {
                                    if (entityAttr.ColumnDescription == null)
                                    {
                                        continue;
                                    }
                                    if (entityAttr.IsIdentity)
                                    {
                                        continue;
                                    }
                                    if (entityAttr.IsPrimaryKey)
                                    {
                                        continue;
                                    }
                                    if (entityAttr.ColumnDescription.Trim().Length > 0)
                                    {
                                        cd.TryAdd(field.Name, entityAttr.ColumnDescription);
                                    }
                                }
                            }
                        }
                    }

                    List<tb_FieldInfo> tb_FieldInfos = new List<tb_FieldInfo>();
                    foreach (KeyValuePair<string, string> kv in cd)
                    {
                        tb_FieldInfo info = Startup.ServiceProvider.CreateInstance<tb_FieldInfo>();
                        info.ActionStatus = ActionStatus.新增;
                        info.ClassPath = type.FullName;
                        if (isChild)
                        {
                            info.EntityName = type.Name.Replace("Detail", "");
                        }
                        else
                        {
                            info.EntityName = type.Name;
                        }
                        info.IsEnabled = true;
                        info.FieldName = kv.Key;
                        info.FieldText = kv.Value;
                        info.MenuID = menuInfo.MenuID;
                        info.IsChild = isChild;
                        info.ChildEntityName = childType;
                        BusinessHelper.Instance.InitEntity(info);
                        //bool isexist = fieldController.IsExist(e => e.EntityName == info.EntityName && e.FieldName == kv.Key && e.MenuID == menuInfo.MenuID && e.IsChild == isChild);
                        var existFieldInfo = menuInfo.tb_FieldInfos.FirstOrDefault(e => e.EntityName == info.EntityName && e.FieldName == kv.Key && e.MenuID == menuInfo.MenuID && e.IsChild == isChild);
                        if (existFieldInfo == null)
                        {
                            tb_FieldInfos.Add(info);
                        }
                    }
                    if (tb_FieldInfos.Count > 0)
                    {
                        List<long> idsbtn = await _appContext.Db.CopyNew().Insertable<tb_FieldInfo>(tb_FieldInfos).ExecuteReturnSnowflakeIdListAsync();
                        if (idsbtn.Count == tb_FieldInfos.Count)
                        {
                            //添加后才不会重复添加
                            menuInfo.tb_FieldInfos.AddRange(tb_FieldInfos);
                        }
                    }


                }
            }
        }

        #endregion


    }
}
