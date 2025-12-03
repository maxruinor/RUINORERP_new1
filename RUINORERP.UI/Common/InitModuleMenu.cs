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
using RUINORERP.UI.BaseForm;
using RUINORERP.Global.EnumExt;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Dto;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 初始化模块菜单和权限的服务类
    /// </summary>
    public class InitModuleMenu
    {
        private readonly ApplicationContext _appContext;
        private readonly ILogger<InitModuleMenu> _logger;
        private List<MenuAttrAssemblyInfo> _menuAssemblyList = new List<MenuAttrAssemblyInfo>();
        private List<string> _typeNames = new List<string>();
        private Type[] _modelTypes;

        public InitModuleMenu(ApplicationContext appContext, ILogger<InitModuleMenu> logger)
        {
            _appContext = appContext;
            _logger = logger;
            // 加载菜单程序集信息
            _menuAssemblyList = UIHelper.RegisterForm();
        }

        #region 系统级初始化菜单
        /// <summary>
        /// 初始化模块和菜单结构
        /// 支持多次执行，采用批量操作优化性能
        /// </summary>
        public async Task InitModuleAndMenuAsync()
        {
            try
            {


                // 加载模型类型信息
                var dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
                _modelTypes = dalAssemble.GetExportedTypes();
                _typeNames = _modelTypes.Select(m => m.Name).ToList();

                // 获取控制器实例
                var mdctr = _appContext.GetRequiredService<tb_ModuleDefinitionController<tb_ModuleDefinition>>();
                var mc = _appContext.GetRequiredService<tb_MenuInfoController<tb_MenuInfo>>();

                // 获取模块枚举列表并过滤不可用模块
                var modules = typeof(ModuleMenuDefine.模块定义).EnumToList();
                if (!MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM))
                {
                    modules = modules.Where(m => m.Name != ModuleMenuDefine.模块定义.客户关系.ToString()).ToList();
                }
                if (!MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.财务模块))
                {
                    modules = modules.Where(m => m.Name != ModuleMenuDefine.模块定义.财务管理.ToString()).ToList();
                }
                if (!MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.生产模块))
                {
                    modules = modules.Where(m => m.Name != ModuleMenuDefine.模块定义.生产管理.ToString()).ToList();
                }

                // 从数据库加载现有模块数据（包含关联的菜单、按钮等信息）
                var existModuleList = await MainForm.Instance.AppContext.Db.CopyNew()
                    .Queryable<tb_ModuleDefinition>()
                    .Includes(c => c.tb_MenuInfos, b => b.tb_moduledefinition)
                    .Includes(c => c.tb_MenuInfos, b => b.tb_ButtonInfos)
                    .Includes(c => c.tb_MenuInfos, b => b.tb_FieldInfos)
                    .Includes(c => c.tb_MenuInfos, b => b.tb_UIMenuPersonalizations)
                    .ToListAsync() ?? new List<tb_ModuleDefinition>();

                // 处理模块和顶级菜单
                foreach (var moduleDto in modules)
                {
                    // 查找或创建模块定义
                    var module = existModuleList.FirstOrDefault(e => e.ModuleName == moduleDto.Name)
                        ?? new tb_ModuleDefinition
                        {
                            ModuleName = moduleDto.Name,
                            ModuleNo = ClientBizCodeService.GetBaseInfoNo(BaseInfoType.ModuleDefinition),
                            Available = true,
                            Visible = true
                        };

                    if (module.ModuleID == 0)
                    {
                        existModuleList.Add(module);
                    }

                    // 确保模块的菜单集合已初始化
                    if (module.tb_MenuInfos == null)
                    {
                        module.tb_MenuInfos = new List<tb_MenuInfo>();
                    }

                    // 查找或创建顶级菜单
                    var topMenu = module.tb_MenuInfos.FirstOrDefault(e => e.MenuName == moduleDto.Name && e.Parent_id == 0)
                        ?? new tb_MenuInfo
                        {
                            MenuName = moduleDto.Name,
                            IsVisble = true,
                            ModuleID = module.ModuleID,
                            IsEnabled = true,
                            CaptionCN = moduleDto.Name,
                            MenuType = "导航菜单",
                            Parent_id = 0,
                            Created_at = System.DateTime.Now
                        };

                    if (topMenu.MenuID == 0)
                    {
                        module.tb_MenuInfos.Add(topMenu);
                    }
                }

                // 批量插入新模块
                var newModules = existModuleList.Where(c => c.ModuleID == 0).ToList();
                if (newModules.Any())
                {
                    var moduleIds = await MainForm.Instance.AppContext.Db
                        .Insertable(newModules)
                        .ExecuteReturnSnowflakeIdListAsync();

                    // 确保ID正确分配给实体
                    for (int i = 0; i < newModules.Count; i++)
                    {
                        newModules[i].ModuleID = moduleIds[i];
                    }
                }

                // 关联菜单与模块
                foreach (var module in existModuleList)
                {
                    foreach (var menu in module.tb_MenuInfos)
                    {
                        menu.ModuleID = module.ModuleID;
                        menu.tb_moduledefinition = module;
                    }
                }

                // 批量插入新菜单
                var newMenus = existModuleList
                    .SelectMany(m => m.tb_MenuInfos)
                    .Where(m => m.MenuID == 0)
                    .ToList();

                if (newMenus.Any())
                {
                    var menuIds = await MainForm.Instance.AppContext.Db
                        .Insertable(newMenus)
                        .ExecuteReturnSnowflakeIdListAsync();

                    // 确保ID正确分配给实体
                    for (int i = 0; i < newMenus.Count; i++)
                    {
                        newMenus[i].MenuID = menuIds[i];
                    }
                }

                // 处理顶级菜单的子菜单
                var topMenus = existModuleList
                    .SelectMany(m => m.tb_MenuInfos)
                    .Where(m => m.Parent_id == 0)
                    .ToList();

                foreach (var topMenu in topMenus)
                {
                    await StartInitNavMenuAsync(topMenu, _menuAssemblyList, topMenu.tb_moduledefinition.ModuleName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化模块和菜单时发生错误");
                MainForm.Instance.uclog.AddLog($"初始化模块和菜单失败: {ex.Message}", UILogType.错误);
            }
        }

        /// <summary>
        /// 初始化指定模块的导航菜单
        /// </summary>
        private async Task StartInitNavMenuAsync(tb_MenuInfo menuInfoparent, List<MenuAttrAssemblyInfo> menuAssemblyList, string modelName)
        {
            try
            {
                var menulist = menuAssemblyList.Where(it => it.MenuPath.Split('|')[0] == modelName).ToList();
                模块定义 module = (模块定义)Enum.Parse(typeof(模块定义), modelName);

                switch (module)
                {
                    case 模块定义.生产管理:
                        await InitNavMenuAsync<生产管理>(menuInfoparent, menulist);
                        break;
                    case 模块定义.进销存管理:
                        await InitNavMenuAsync<进销存管理>(menuInfoparent, menulist);
                        break;
                    case 模块定义.客户关系:
                        await InitNavMenuAsync<客户关系>(menuInfoparent, menulist);
                        break;
                    case 模块定义.财务管理:
                        await InitNavMenuAsync<财务管理>(menuInfoparent, menulist);
                        break;
                    case 模块定义.行政管理:
                        await InitNavMenuAsync<行政管理>(menuInfoparent, menulist);
                        break;
                    case 模块定义.报表管理:
                        await InitNavMenuAsync<报表管理>(menuInfoparent, menulist);
                        break;
                    case 模块定义.基础资料:
                        await InitNavMenuAsync<基础资料>(menuInfoparent, menulist);
                        break;
                    case 模块定义.系统设置:
                        await InitNavMenuAsync<系统设置>(menuInfoparent, menulist);
                        break;
                    default:
                        _logger.LogWarning($"未处理的模块类型: {modelName}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化导航菜单时发生错误: {modelName}");
                MainForm.Instance.uclog.AddLog($"初始化导航菜单失败: {ex.Message}", UILogType.错误);
            }
        }

        /// <summary>
        /// 初始化指定类型的导航菜单
        /// </summary>
        private async Task InitNavMenuAsync<T>(tb_MenuInfo menuParent, List<MenuAttrAssemblyInfo> list)
        {
            try
            {
                string parentName = typeof(T).Name;
                var mc = _appContext.GetRequiredService<tb_MenuInfoController<tb_MenuInfo>>();
                var modules = typeof(T).EnumToList();

                // 确保父菜单的模块已初始化菜单集合
                if (menuParent.tb_moduledefinition.tb_MenuInfos == null)
                {
                    menuParent.tb_moduledefinition.tb_MenuInfos = new List<tb_MenuInfo>();
                }

                // 获取现有子菜单
                var existMenuInfoList = menuParent.tb_moduledefinition.tb_MenuInfos
                    .Where(c => c.Parent_id == menuParent.MenuID)
                    .ToList();

                // 处理子菜单
                foreach (var item in modules)
                {
                    var menuInfo = existMenuInfoList.FirstOrDefault(e => e.MenuName == item.Name && e.Parent_id == menuParent.MenuID)
                        ?? new tb_MenuInfo
                        {
                            ModuleID = menuParent.ModuleID,
                            MenuName = item.Name,
                            IsVisble = true,
                            IsEnabled = true,
                            CaptionCN = item.Name,
                            MenuType = "导航菜单",
                            Parent_id = menuParent.MenuID,
                            Created_at = System.DateTime.Now
                        };

                    if (menuInfo.MenuID == 0)
                    {
                        existMenuInfoList.Add(menuInfo);
                    }
                }

                // 设置菜单关联信息
                foreach (var menuInfo in existMenuInfoList)
                {
                    menuInfo.ModuleID = menuParent.tb_moduledefinition.ModuleID;
                    menuInfo.Parent_id = menuParent.MenuID;
                    menuInfo.tb_moduledefinition = menuParent.tb_moduledefinition;
                }

                // 批量插入新菜单
                var newMenus = existMenuInfoList.Where(c => c.MenuID == 0).ToList();
                if (newMenus.Any())
                {
                    var menuIds = await MainForm.Instance.AppContext.Db
                        .Insertable(newMenus)
                        .ExecuteReturnSnowflakeIdListAsync();

                    // 确保ID正确分配给实体
                    for (int i = 0; i < newMenus.Count; i++)
                    {
                        newMenus[i].MenuID = menuIds[i];
                    }
                }

                // 处理子菜单的下级菜单项
                foreach (var nextMenuInfo in existMenuInfoList)
                {
                    var menulist = list.Where(it =>
                        it.MenuPath.Split('|')[0] == parentName &&
                        it.MenuPath.Split('|')[1] == nextMenuInfo.MenuName).ToList();

                    foreach (var menuinfo in menulist)
                    {
                        await AddMenuItemAsync(menuinfo, nextMenuInfo, mc);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化导航菜单类型 {typeof(T).Name} 时发生错误");
                MainForm.Instance.uclog.AddLog($"初始化导航菜单类型失败: {ex.Message}", UILogType.错误);
            }
        }

        /// <summary>
        /// 添加菜单项及其相关的按钮和字段信息
        /// </summary>
        private async Task AddMenuItemAsync(MenuAttrAssemblyInfo info, tb_MenuInfo parentMenuInfo, tb_MenuInfoController<tb_MenuInfo> mc)
        {
            try
            {
                // 确保父菜单的模块已初始化菜单集合
                if (parentMenuInfo.tb_moduledefinition.tb_MenuInfos == null)
                {
                    parentMenuInfo.tb_moduledefinition.tb_MenuInfos = new List<tb_MenuInfo>();
                }

                // 获取现有菜单项
                var existMenuInfoList = parentMenuInfo.tb_moduledefinition.tb_MenuInfos
                    .Where(c => c.Parent_id == parentMenuInfo.MenuID)
                    .ToList();

                // 查找或创建菜单项
                var menu = existMenuInfoList.FirstOrDefault(e => e.MenuName == info.Caption && e.Parent_id == parentMenuInfo.MenuID)
                    ?? new tb_MenuInfo
                    {
                        MenuName = info.Caption,
                        ModuleID = parentMenuInfo.ModuleID,
                        IsVisble = true,
                        IsEnabled = true,
                        CaptionCN = info.Caption,
                        ClassPath = info.ClassPath,
                        FormName = info.ClassName,
                        Parent_id = parentMenuInfo.MenuID,
                        BIBaseForm = info.BIBaseForm,
                        BIBizBaseForm = info.BIBizBaseForm,
                        BizInterface = info.BizInterface,
                        BizType = info.MenuBizType.HasValue ? (int)info.MenuBizType : 0,
                        MenuType = "行为菜单",
                        EntityName = info.EntityName,
                        Created_at = System.DateTime.Now
                    };

                if (menu.MenuID == 0)
                {
                    menu = mc.AddReEntity(menu); // 单个添加，因为可能有复杂业务逻辑
                    existMenuInfoList.Add(menu);
                }

                // 初始化菜单项的按钮和字段信息
                await InitToolStripItemAsync(info, menu);
                await InitFieldInoAsync(info, menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"添加菜单项 {info.Caption} 时发生错误");
                MainForm.Instance.uclog.AddLog($"添加菜单项失败: {ex.Message}", UILogType.错误);
            }
        }

        /// <summary>
        /// 初始化工具栏按钮和右键菜单按钮信息
        /// </summary>
        /// <param name="InsertToDb">是否保存到数据库。有时只想检测是否属于指定窗体。因为编程中有变动。后期固定了才不会。</param>
        public async Task<List<tb_ButtonInfo>> InitToolStripItemAsync(MenuAttrAssemblyInfo info, tb_MenuInfo menuInfo, bool InsertToDb = true)
        {
            try
            {
                // 创建并获取窗体实例
                var c = Startup.ServiceProvider.GetService(info.ClassType) as Control;
                if (c == null)
                {
                    _logger.LogWarning($"无法获取类型 {info.ClassType} 的实例");
                    return null;
                }

                // 获取按钮控制器
                var btnController = Startup.GetFromFac<tb_ButtonInfoController<tb_ButtonInfo>>();

                // 确保菜单的按钮集合已初始化
                menuInfo.tb_ButtonInfos ??= new List<tb_ButtonInfo>();

                // 查找窗体上的所有工具栏
                var toolStrips = FindControls<ToolStrip>(c);

                // 用于存储待添加的新按钮
                var newButtonInfos = new List<tb_ButtonInfo>();

                // 处理工具栏按钮
                foreach (var toolStrip in toolStrips)
                {
                    foreach (var item in toolStrip.Items)
                    {
                        if (item is ToolStripButton btn && !string.IsNullOrWhiteSpace(btn.Text))
                        {
                            AddButtonIfNotExists(btn.Text, btn.Name, ButtonType.Toolbar);
                        }
                        else if (item is ToolStripSplitButton splitBtn && !string.IsNullOrWhiteSpace(splitBtn.Text))
                        {
                            AddButtonIfNotExists(splitBtn.Text, splitBtn.Name, ButtonType.Toolbar);

                            // 处理下拉项
                            foreach (ToolStripItem dropDownItem in splitBtn.DropDownItems)
                            {
                                if (!string.IsNullOrWhiteSpace(dropDownItem.Text))
                                {
                                    AddButtonIfNotExists(dropDownItem.Text, dropDownItem.Name, ButtonType.Toolbar);
                                }
                            }
                        }
                        else if (item is ToolStripDropDownButton dropDownBtn && !string.IsNullOrWhiteSpace(dropDownBtn.Text))
                        {
                            AddButtonIfNotExists(dropDownBtn.Text, dropDownBtn.Name, ButtonType.Toolbar);

                            // 处理下拉项
                            foreach (ToolStripItem dropDownItem in dropDownBtn.DropDownItems)
                            {
                                if (!string.IsNullOrWhiteSpace(dropDownItem.Text))
                                {
                                    AddButtonIfNotExists(dropDownItem.Text, dropDownItem.Name, ButtonType.Toolbar);
                                }
                            }
                        }
                    }
                }

                // 处理扩展按钮
                if (c is IToolStripMenuInfoAuth formAuth)
                {
                    var stripItems = formAuth.AddExtendButton(menuInfo);
                    foreach (var tsitem in stripItems)
                    {
                        if (!string.IsNullOrWhiteSpace(tsitem.Text))
                        {
                            AddButtonIfNotExists(tsitem.Text, tsitem.Name, ButtonType.Toolbar);
                        }
                    }
                }

                // 处理右键菜单
                if (c is IContextMenuInfoAuth contextMenuAuth)
                {
                    var contextMenuItems = contextMenuAuth.AddContextMenu();
                    foreach (var menuItem in contextMenuItems)
                    {
                        if (!string.IsNullOrWhiteSpace(menuItem.MenuText))
                        {
                            AddButtonIfNotExists(
                                menuItem.MenuText,
                                menuItem.MenuText,
                                ButtonType.ContextMenu);
                        }
                    }
                }

                // 处理特殊窗体类型的按钮过滤
                if (c is BaseQuery baseQuery)
                {
                    ApplyMenuFilters(baseQuery, ref newButtonInfos);
                }
                else if (c is BaseBillEdit baseBillEdit)
                {
                    ApplyMenuFilters(baseBillEdit, ref newButtonInfos);
                }

                // 批量插入新按钮
                if (newButtonInfos.Any())
                {
                    if (InsertToDb)
                    {
                        var buttonIds = await _appContext.Db
                                             .Insertable(newButtonInfos)
                                             .ExecuteReturnSnowflakeIdListAsync();
                    }
                    // 确保ID正确分配给实体
                    //for (int i = 0; i < newButtonInfos.Count; i++)
                    //{
                    //    newButtonInfos[i].ButtonInfo_ID = buttonIds[i];
                    //}
                    // 添加到菜单的按钮集合中
                    menuInfo.tb_ButtonInfos.AddRange(newButtonInfos);
                }

                // 局部函数：添加按钮（如果不存在）
                void AddButtonIfNotExists(string text, string name, ButtonType buttonType)
                {
                    var buttonTypeStr = buttonType.ToString();
                    var exists = menuInfo.tb_ButtonInfos.Any(
                        it => it.ClassPath == info.ClassPath &&
                              it.BtnText == text &&
                              it.MenuID == menuInfo.MenuID &&
                              it.ButtonType == buttonTypeStr);

                    if (!exists)
                    {
                        if (!newButtonInfos.Exists(c => c.BtnName == name && c.MenuID == menuInfo.MenuID && c.ButtonType == buttonTypeStr))
                        {
                            newButtonInfos.Add(new tb_ButtonInfo
                            {
                                BtnName = name,
                                BtnText = text,
                                FormName = info.ClassName,
                                ClassPath = info.ClassPath,
                                MenuID = menuInfo.MenuID,
                                IsEnabled = true,
                                ButtonType = buttonTypeStr,
                                Created_at = System.DateTime.Now,
                                Created_by = _appContext.CurUserInfo?.Id
                            });
                        }

                    }
                }

                // 局部函数：应用菜单过滤规则
                void ApplyMenuFilters(dynamic form, ref List<tb_ButtonInfo> buttons)
                {
                    if (form.IncludedMenuList?.Count > 0)
                    {
                        form.AddIncludedMenuList();
                        var includeList = ((IEnumerable<MenuItemEnums>)form.IncludedMenuList).Select(it => it.ToString()).ToList();
                        buttons = buttons.Where(it => includeList.Contains(it.BtnText)).ToList();
                    }
                    else
                    {
                        form.AddExcludeMenuList();
                        var excludeList = ((IEnumerable<MenuItemEnums>)form.ExcludeMenuList).Select(it => it.ToString()).ToList();
                        buttons = buttons.Where(it => !excludeList.Contains(it.BtnText)).ToList();

                        if (form.ExcludeMenuTextList?.Count > 0)
                        {
                            buttons = buttons.Where(it => !form.ExcludeMenuTextList.Contains(it.BtnText)).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化工具栏按钮时发生错误: {info.ClassPath}");
                MainForm.Instance.uclog.AddLog($"初始化工具栏按钮失败: {ex.Message}", UILogType.错误);
            }
            return menuInfo.tb_ButtonInfos;
        }

        /// <summary>
        /// 遍历控件树，查找所有指定类型的子控件
        /// </summary>
        public List<T> FindControls<T>(Control control) where T : Control
        {
            var controls = new List<T>();

            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is T tControl)
                {
                    controls.Add(tControl);
                }

                if (ctrl.HasChildren)
                {
                    controls.AddRange(FindControls<T>(ctrl));
                }
            }

            return controls;
        }

        /// <summary>
        /// 初始化菜单关联的字段信息
        /// </summary>
        private async Task InitFieldInoAsync(MenuAttrAssemblyInfo info, tb_MenuInfo menuInfo)
        {
            try
            {
                if (_modelTypes == null || _modelTypes.Length == 0)
                {
                    _logger.LogWarning("模型类型集合为空，无法初始化字段信息");
                    return;
                }

                foreach (Type type in _modelTypes)
                {
                    // 排除查询DTO类型
                    if (type.FullName.Contains("QueryDto"))
                    {
                        continue;
                    }

                    // 只处理与菜单关联的实体类型
                    if (type.Name != info.EntityName)
                    {
                        continue;
                    }

                    // 初始化主表字段
                    await InitFieldInoMainAndSubAsync(type, menuInfo, false, "");

                    // 尝试查找并初始化子表字段
                    string childTypeName = _typeNames.FirstOrDefault(s => s.Contains(type.Name + "Detail"));
                    if (!string.IsNullOrEmpty(childTypeName))
                    {
                        Type childType = _modelTypes.FirstOrDefault(t => t.FullName == type.FullName + "Detail");
                        if (childType != null)
                        {
                            await InitFieldInoMainAndSubAsync(childType, menuInfo, true, childTypeName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化字段信息时发生错误: {info.EntityName}");
                MainForm.Instance.uclog.AddLog($"初始化字段信息失败: {ex.Message}", UILogType.错误);
            }
        }

        /// <summary>
        /// 初始化主表和子表的字段信息
        /// 注册一个规律： 在一个菜单名下。可能有主子表和公共表。他们的实体名：都统一成主表
        /// </summary>
        public async Task InitFieldInoMainAndSubAsync(Type EntityType, tb_MenuInfo menuInfo, bool isChild, string childType)
        {
            try
            {
                // 确保菜单的字段集合已初始化
                menuInfo.tb_FieldInfos ??= new List<tb_FieldInfo>();

                //// 获取实体的SugarTable特性
                //var tableAttrs = type.GetCustomAttributes<SugarTable>().ToList();
                List<Type> entityTypes = new List<Type>();
                entityTypes.Add(EntityType);
                //明细才处理
                if (EntityType.Name.Contains("Detail"))
                {
                    #region 取明细中的公共产品类，或其它类。
                    try
                    {
                        MenuAttrAssemblyInfo mai = _menuAssemblyList.FirstOrDefault(e => e.ClassPath == menuInfo.ClassPath);
                        if (mai != null)
                        {
                            // 创建并获取窗体实例
                            object formType = Startup.ServiceProvider.GetService(mai.ClassType);

                            // 使用上面的方法获取公共实体类型
                            List<Type> publicEntityTypes = GetPublicEntityTypes(formType);

                            if (publicEntityTypes != null)
                            {
                                entityTypes.AddRange(publicEntityTypes);
                            }
                        }

                        // 在处理方法中直接通过类型访问
                        //var publicEntityObjects = formType.GetProperty("PublicEntityObjects",
                        //                                  BindingFlags.Static | BindingFlags.Public)
                        //                     ?.GetValue(null) as List<Type>;


                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion
                }
                foreach (var type in entityTypes)
                {
                    // 获取实体的字段元数据
                    var entityInstance = Startup.ServiceProvider.CreateInstance(type);
                    var fieldNameList = ReflectionHelper.GetPropertyValue(entityInstance, "FieldNameList") as ConcurrentDictionary<string, string>;

                    // 如果无法通过反射获取字段列表，则直接从属性特性中提取
                    if (fieldNameList == null)
                    {
                        fieldNameList = new ConcurrentDictionary<string, string>();
                        foreach (PropertyInfo property in type.GetProperties())
                        {
                            foreach (Attribute attrField in property.GetCustomAttributes(true))
                            {
                                if (attrField is SugarColumn columnAttr &&
                                    !string.IsNullOrWhiteSpace(columnAttr.ColumnDescription) &&
                                    !columnAttr.IsIdentity &&
                                    !columnAttr.IsPrimaryKey)
                                {
                                    fieldNameList.TryAdd(property.Name, columnAttr.ColumnDescription);
                                }
                            }
                        }
                    }

                    //目前添加的是明细公共部分的。暂时也认为是子表中的一部分
                    if ((type.Name.Contains("ProductSharePart")) || EntityType.Name.Contains("Detail"))
                    {
                        isChild = true;
                    }
                    // 创建待添加的字段信息列表
                    var newFieldInfos = new List<tb_FieldInfo>();
                    foreach (var kv in fieldNameList)
                    {
                        // 检查字段是否已存在
                        var exists = menuInfo.tb_FieldInfos.Any(
                            e => e.EntityName == type.Name &&
                                 e.FieldName == kv.Key &&
                                 e.MenuID == menuInfo.MenuID);

                        if (!exists)
                        {
                            newFieldInfos.Add(new tb_FieldInfo
                            {
                                ClassPath = type.FullName,
                                EntityName = type.Name,
                                IsEnabled = true,
                                FieldName = kv.Key,
                                FieldText = kv.Value,
                                MenuID = menuInfo.MenuID,
                                IsChild = isChild,
                                ChildEntityName = childType,
                                ActionStatus = ActionStatus.新增
                            });
                        }
                    }

                    // 批量插入新字段
                    if (newFieldInfos.Any())
                    {
                        // 初始化实体（设置默认值等）
                        foreach (var fieldInfo in newFieldInfos)
                        {
                            BusinessHelper.Instance.InitEntity(fieldInfo);
                        }

                        var fieldIds = await _appContext.Db
                            .Insertable(newFieldInfos)
                            .ExecuteReturnSnowflakeIdListAsync();

                        // 确保ID正确分配给实体
                        //for (int i = 0; i < newFieldInfos.Count; i++)
                        //{
                        //    newFieldInfos[i].FieldInfo_ID = fieldIds[i];
                        //}

                        // 添加到菜单的字段集合中
                        menuInfo.tb_FieldInfos.AddRange(newFieldInfos);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"初始化字段信息（主表/子表）时发生错误: {EntityType?.Name}");
                MainForm.Instance.uclog.AddLog($"初始化字段信息失败: {ex.Message}", UILogType.错误);
            }
        }

        /// <summary>
        /// 优先接口
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private List<Type> ProcessEntityWithInterface(object entity)
        {
            List<Type> publicEntityTypes = new List<Type>();
            if (entity is IPublicEntityObject publicEntity)
            {
                publicEntityTypes = publicEntity.PublicEntityObjects;
            }
            //else
            //{
            //    //上一级
            //    ProcessEntityWithInterface(entity.GetType().GetBaseType());
            //}
            return publicEntityTypes;
        }


        private List<Type> GetPublicEntityTypes(object entity)
        {
            return ProcessEntityWithInterface(entity);

            // 优先尝试通过接口获取
            if (entity is IPublicEntityObject publicEntity)
            {
                return publicEntity.PublicEntityObjects;
            }

            // 否则使用反射
            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty("PublicEntityObjects",
                BindingFlags.Public | BindingFlags.Instance);

            if (property != null &&
                property.PropertyType == typeof(List<Type>) &&
                property.CanRead)
            {
                return property.GetValue(entity) as List<Type>;
            }

            return null;
        }
        #endregion
    }


    /// <summary>
    /// TODO  要完善的 这里多个地方可以批量新增 需要优化
    /// 因为与UI紧密才放到这个层
    /// </summary>
    public class InitModuleMenu_old
    {

        private readonly ApplicationContext _appContext;
        private readonly ILogger<InitModuleMenu> _logger;
        //提取到UI的类相关信息
        private List<MenuAttrAssemblyInfo> _menuAssemblyList = new List<MenuAttrAssemblyInfo>();

        /// <summary>
        /// 为了查找明细表名类型，保存所有类型名称方便查找
        /// </summary>
        private List<string> _typeNames = new List<string>();
        private Type[] _modelTypes;

        public InitModuleMenu_old(ApplicationContext appContext, ILogger<InitModuleMenu> logger)
        {
            _appContext = appContext;
            _logger = logger;
        }


        #region 系统级初始化菜单
        /// <summary>
        /// 定义模块 模块下定义好了对应枚举再对应上了UI
        /// 可以多次执行，但是发布后不需要每次执行
        /// 2025-1-16优化执行为批量添加 可以多次执行
        /// </summary>
        public async Task InitModuleAndMenu()
        {
            _menuAssemblyList = UIHelper.RegisterForm();

            //这里先提取要找到实体的类型，执行一次
            Assembly dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            _modelTypes = dalAssemble.GetExportedTypes();

            _typeNames = _modelTypes.Select(m => m.Name).ToList();



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
                mod.ModuleNo = ClientBizCodeService.GetBaseInfoNo(BaseInfoType.ModuleDefinition);
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
                StartInitNavMenu(newItem, _menuAssemblyList, newItem.tb_moduledefinition.ModuleName);
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


        private async Task InitNavMenu<T>(tb_MenuInfo menuParent, List<MenuAttrAssemblyInfo> list)
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
                menu.BIBizBaseForm = info.BIBizBaseForm;
                menu.BizInterface = info.BizInterface;
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


        /// <summary>
        /// 初始化按钮 包含工具栏和右键菜单按钮
        /// </summary>
        /// <param name="info"></param>
        /// <param name="menuInfo"></param>
        public async Task InitToolStripItem(MenuAttrAssemblyInfo info, tb_MenuInfo menuInfo)
        {
            Control c = Startup.ServiceProvider.GetService(info.ClassType) as Control;
            tb_ButtonInfoController<tb_ButtonInfo> BtnController = Startup.GetFromFac<tb_ButtonInfoController<tb_ButtonInfo>>();
            var btns = FindControls<ToolStrip>(c);

            if (menuInfo.tb_ButtonInfos == null)
            {
                menuInfo.tb_ButtonInfos = new List<tb_ButtonInfo>();
            }

            //保存按钮信息，后面批量保存到数据库再添加了对应菜单的按钮信息集合中

            List<tb_ButtonInfo> NewButtonInfos = new List<tb_ButtonInfo>();

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
                                    NewButtonInfos.Add(btnInfo);
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
                                    NewButtonInfos.Add(btnInfo);
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
                                        NewButtonInfos.Add(btnInfoSub);
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
                                    NewButtonInfos.Add(btnInfoDrop);
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
                                        NewButtonInfos.Add(btnInfo);
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
                            NewButtonInfos.Add(btnInfoSub);
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
                            NewButtonInfos.Add(btnInfoSub);
                        }
                    }
                }
            }

            //通用情况时 排除窗体上的一些指定的按钮，如：核销查询。只会有查询。没有审核 删除这些
            if (c is BaseQuery)
            {
                BaseQuery baseQuery = c as BaseQuery;
                if (baseQuery != null)
                {
                    //优先保留指定设置的菜单集合，其它的排除
                    if (baseQuery.IncludedMenuList.Count > 0)
                    {
                        baseQuery.AddIncludedMenuList();
                        List<MenuItemEnums> stripItems = baseQuery.IncludedMenuList;
                        List<string> excludemenuTextList = stripItems.Select(it => it.ToString()).ToList();
                        NewButtonInfos = NewButtonInfos.Where(it => excludemenuTextList.Contains(it.BtnText)).ToList();

                    }
                    else
                    {
                        baseQuery.AddExcludeMenuList();
                        //其它的排除
                        List<MenuItemEnums> stripItems = baseQuery.ExcludeMenuList;
                        //意思是stripItems这个公共按钮是和枚举命名是对应的。但是 特殊窗体中 添加了命名不一样的菜单
                        List<string> excludemenuTextList = stripItems.Select(it => it.ToString()).ToList();
                        NewButtonInfos = NewButtonInfos.Where(it => !excludemenuTextList.Contains(it.BtnText)).ToList();
                        //其它的排除
                        List<string> MenuTextItems = baseQuery.ExcludeMenuTextList;
                        NewButtonInfos = NewButtonInfos.Where(it => !MenuTextItems.Contains(it.BtnText)).ToList();
                    }
                }
            }

            if (c is BaseBillEdit)
            {
                BaseBillEdit baseBillEdit = c as BaseBillEdit;
                if (baseBillEdit != null)
                {
                    //优先保留指定设置的菜单集合，其它的排除
                    if (baseBillEdit.IncludedMenuList.Count > 0)
                    {
                        baseBillEdit.AddIncludedMenuList();
                        List<MenuItemEnums> stripItems = baseBillEdit.IncludedMenuList;
                        List<string> excludemenuTextList = stripItems.Select(it => it.ToString()).ToList();
                        NewButtonInfos = NewButtonInfos.Where(it => excludemenuTextList.Contains(it.BtnText)).ToList();

                    }
                    else
                    {
                        baseBillEdit.AddExcludeMenuList();
                        //其它的排除
                        List<MenuItemEnums> stripItems = baseBillEdit.ExcludeMenuList;
                        List<string> excludemenuTextList = stripItems.Select(it => it.ToString()).ToList();
                        NewButtonInfos = NewButtonInfos.Where(it => !excludemenuTextList.Contains(it.BtnText)).ToList();

                        //其它的排除
                        List<string> MenuTextItems = baseBillEdit.ExcludeMenuTextList;
                        NewButtonInfos = NewButtonInfos.Where(it => !MenuTextItems.Contains(it.BtnText)).ToList();
                    }
                }
            }

            if (NewButtonInfos.Count > 0)
            {
                List<long> idsbtn = await _appContext.Db.CopyNew().Insertable<tb_ButtonInfo>(NewButtonInfos).ExecuteReturnSnowflakeIdListAsync();
                if (idsbtn.Count > 0)
                {
                    //不会重复添加
                    for (int i = 0; i < NewButtonInfos.Count; i++)
                    {
                        if (!menuInfo.tb_ButtonInfos.Any(c => c.BtnText == NewButtonInfos[i].BtnText))
                        {
                            menuInfo.tb_ButtonInfos.Add(NewButtonInfos[i]);
                        }
                    }
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

        /// <summary>
        /// 初始化字段
        /// </summary>
        private void InitFieldIno(MenuAttrAssemblyInfo Info, tb_MenuInfo menuInfo)
        {
            try
            {
                // Type[] ModelTypes
                foreach (Type type in _modelTypes)
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
                    string childType = _typeNames.FirstOrDefault(s => s.Contains(type.Name + "Detail"));
                    if (!string.IsNullOrEmpty(childType))
                    {
                        // type.FullName + "Detail";
                        //Type cType = System.Reflection.Assembly.Load("RUINORERP.Model.dll").GetType(type.FullName + "Detail");
                        Type cType = _modelTypes.FirstOrDefault(t => t.FullName == type.FullName + "Detail");
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

        public async Task InitFieldInoMainAndSub(Type type, tb_MenuInfo menuInfo, bool isChild, string childType)
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
                        var existFieldInfo = menuInfo.tb_FieldInfos.FirstOrDefault(e => e.EntityName == info.EntityName && e.FieldName == kv.Key && e.MenuID == menuInfo.MenuID && e.IsChild == isChild);
                        if (existFieldInfo == null)
                        {
                            tb_FieldInfos.Add(info);
                        }
                    }
                    if (tb_FieldInfos.Count > 0)
                    {
                        List<long> idsbtn = await _appContext.Db.CopyNew().Insertable<tb_FieldInfo>(tb_FieldInfos).ExecuteReturnSnowflakeIdListAsync();
                        if (idsbtn.Count > 0)
                        {
                            //添加后才不会重复添加
                            menuInfo.tb_FieldInfos.AddRange(tb_FieldInfos);
                        }
                    }

                    //不会重复添加
                    //for (int i = 0; i < tb_FieldInfos.Count; i++)
                    //{
                    //    if (!menuInfo.tb_FieldInfos.Any(c => c.FieldName == tb_FieldInfos[i].FieldName && c.ClassPath == tb_FieldInfos[i].ClassPath))
                    //    {
                    //        menuInfo.tb_FieldInfos.Add(tb_FieldInfos[i]);
                    //    }
                    //}
                }
            }
        }

        #endregion


    }
}
