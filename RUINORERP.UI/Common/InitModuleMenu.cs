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
        /// </summary>
        public void InitModuleAndMenu()
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
            foreach (var item in modules)
            {
                tb_ModuleDefinition mod = new tb_ModuleDefinition();
                mod.ModuleName = item.Name;
                mod.ModuleNo = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.ModuleDefinition);
                tb_ModuleDefinition isExistt = mdctr.IsExistEntity(e => e.ModuleName == mod.ModuleName);
                if (isExistt == null)
                {
                    mod = mdctr.AddReEntity(mod);
                }
                else
                {
                    mod = isExistt;
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
                menuInfoparent.ModuleID = mod.ModuleID;
                menuInfoparent.Created_at = System.DateTime.Now;
                tb_MenuInfo MenuInfo = mc.IsExistEntity(e => e.MenuName == menuInfoparent.MenuName && e.Parent_id == 0);
                if (MenuInfo == null)
                {
                    menuInfoparent = mc.AddReEntity(menuInfoparent);
                }
                else
                {
                    menuInfoparent = MenuInfo;
                }
                List<MenuAttrAssemblyInfo> menulist = MenuAssemblylist.Where(it => it.MenuPath.Split('|')[0] == item.Name).ToList();

                模块定义 module = (模块定义)Enum.Parse(typeof(模块定义), item.Name);
                switch (module)
                {
                    case 模块定义.生产管理:
                        InitNavMenu<生产管理>(menuInfoparent, menulist);
                        break;
                    case 模块定义.进销存管理:
                        InitNavMenu<供应链管理>(menuInfoparent, menulist);
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
                    default:
                        break;
                }

            }


        }


        private void InitNavMenu<T>(tb_MenuInfo menuParent, List<MenuAttrAssemblyInfo> list)
        {
            string parentName = typeof(T).Name;
            tb_MenuInfoController<tb_MenuInfo> mc = _appContext.GetRequiredService<tb_MenuInfoController<tb_MenuInfo>>();
            List<EnumDto> modules = new List<EnumDto>();
            modules = typeof(T).EnumToList();

            foreach (var item in modules)
            {
                tb_MenuInfo menuInfoParent = new tb_MenuInfo();
                menuInfoParent.ModuleID = menuParent.ModuleID;
                menuInfoParent.MenuName = item.Name;
                menuInfoParent.IsVisble = true;
                menuInfoParent.IsEnabled = true;
                menuInfoParent.CaptionCN = item.Name;
                menuInfoParent.MenuType = "导航菜单";
                menuInfoParent.Parent_id = menuParent.MenuID;
                menuInfoParent.Created_at = System.DateTime.Now;

                tb_MenuInfo minfo = mc.IsExistEntity(e => e.MenuName == item.Name && e.Parent_id == menuParent.MenuID);
                if (minfo == null)
                {
                    minfo = mc.AddReEntity(menuInfoParent);
                }
                else
                {
                    menuInfoParent = minfo;
                }

                // var arrs = list.GroupBy(x => x.MenuPath.Split('|')[0]);
                var menulist = list.Where(it => it.MenuPath.Split('|')[0] == parentName && it.MenuPath.Split('|')[1] == item.Name).ToList();
                foreach (var menuinfo in menulist)
                {
                    AddMenuItem(menuinfo, menuInfoParent, mc);
                }


            }
        }



        private void AddMenuItem(MenuAttrAssemblyInfo info, tb_MenuInfo ParentMenuInfo, tb_MenuInfoController<tb_MenuInfo> mc)
        {
            tb_MenuInfo isexist = mc.IsExistEntity(e => e.MenuName == info.Caption && e.Parent_id == ParentMenuInfo.MenuID);
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
                menu.Created_at = System.DateTime.Now;
                menu = mc.AddReEntity(menu);
                isexist = menu;
            }


            //第一次添加时才添加相应的按钮
            InitToolStripItem(info, isexist);


            InitFieldIno(info, isexist);



        }





        public void InitToolStripItem(MenuAttrAssemblyInfo info, tb_MenuInfo menuInfo)
        {
            Control c = Startup.ServiceProvider.GetService(info.ClassType) as Control;
            tb_ButtonInfoController<tb_ButtonInfo> BtnController = Startup.GetFromFac<tb_ButtonInfoController<tb_ButtonInfo>>();
            var btns = FindControls<ToolStrip>(c);
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
                                tb_ButtonInfo ExistBtnInfo = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == btn.Text && it.MenuID == menuInfo.MenuID);
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
                        else
                        {   //暂时只有二级。不用循环了
                            if (btnItem is ToolStripDropDownButton btnddb)
                            {
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
                                    tb_ButtonInfo ExistBtnInfo = BtnController.IsExistEntity(it => it.ClassPath == info.ClassPath && it.BtnText == tsi.Text && it.MenuID == menuInfo.MenuID);
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
            if (tb_ButtonInfos.Count > 0)
            {
                _appContext.Db.CopyNew().Insertable<tb_ButtonInfo>(tb_ButtonInfos).ExecuteReturnSnowflakeIdListAsync();
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

        public void InitFieldInoMainAndSub(Type type, tb_MenuInfo menuInfo, bool isChild, string childType)
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

                        info.FieldName = kv.Key;
                        info.FieldText = kv.Value;
                        info.MenuID = menuInfo.MenuID;
                        info.IsChild = isChild;
                        info.ChildEntityName = childType;
                        BusinessHelper.Instance.InitEntity(info);
                        tb_FieldInfo isexist = fieldController.IsExistEntity(e => e.EntityName == type.Name && e.FieldName == kv.Key && e.MenuID == menuInfo.MenuID && e.IsChild == isChild);
                        if (isexist == null)
                        {
                            // fieldController.AddReEntity(info);
                            tb_FieldInfos.Add(info);
                        }
                    }
                    if (tb_FieldInfos.Count > 0)
                    {
                        _appContext.Db.CopyNew().Insertable<tb_FieldInfo>(tb_FieldInfos).ExecuteReturnSnowflakeIdListAsync();
                    }

                    //应该要优化为批量新增
                }
            }
        }


        /*
        /// <summary>
        /// 初始化字段
        /// </summary>
        public void InitFieldIno(tb_MenuInfo menuInfo, Type type)
        {
            try
            {
                List<tb_FieldInfo> fields = new List<tb_FieldInfo>();
                var attrs = type.GetCustomAttributes<SugarTable>();
                foreach (var attr in attrs)
                {
                    if (attr is SugarTable)
                    {
                        var t = Startup.ServiceProvider.GetService(type);//SugarColumn 或进一步取字段特性也可以
                        ConcurrentDictionary<string, string> cd = ReflectionHelper.GetPropertyValue(t, "FieldNameList") as ConcurrentDictionary<string, string>;
                        List<tb_FieldInfo> tb_FieldInfos = new List<tb_FieldInfo>();
                        foreach (KeyValuePair<string, string> kv in cd)
                        {
                            tb_FieldInfo info = Startup.ServiceProvider.CreateInstance<tb_FieldInfo>();
                            info.actionStatus = ActionStatus.新增;
                            info.ClassPath = type.FullName;
                            info.EntityName = type.Name;
                            info.FieldName = kv.Key;
                            info.FieldText = kv.Value;
                            info.MenuID = menuInfo.MenuID;
                            info.ChildEntityName = string.Empty;
                            //fields.Add(info);
                            tb_FieldInfo isexist = fieldController.IsExistEntity(e => e.EntityName == type.Name && e.FieldName == kv.Key && e.MenuID == menuInfo.MenuID);
                            if (isexist == null)
                            {
                                //fieldController.AddReEntity(info);
                                tb_FieldInfos.Add(info);
                            }
                        }

                        //fieldController.AddAsync(fields);
                        //应该要优化为批量新增
                        if (tb_FieldInfos.Count > 0)
                        {
                            _appContext.Db.CopyNew().Insertable<tb_FieldInfo>(tb_FieldInfos).ExecuteReturnSnowflakeIdListAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog(ex.Message, UILogType.错误);
            }
        }
        */

        #endregion



    }
}
