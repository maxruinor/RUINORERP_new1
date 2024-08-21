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


namespace RUINORERP.Business.CommService
{
    public class InitDBData
    {

        public ApplicationContext _appContext;

        public InitDBData(ApplicationContext apeContext)
        {
            _appContext = apeContext;
        }


        /// <summary>
        /// 定义模块 模块下定义好了对应枚举再对应上了UI
        /// 可以多次执行，但是发布后不需要每次执行
        /// </summary>
        public async void InitModuleAndMenu()
        {

            tb_ModuleDefinitionController<tb_ModuleDefinition> mdctr = _appContext.GetRequiredService<tb_ModuleDefinitionController<tb_ModuleDefinition>>();
            tb_MenuInfoController<tb_MenuInfo> mc = _appContext.GetRequiredService<tb_MenuInfoController<tb_MenuInfo>>();
            List<EnumDto> modules = new List<EnumDto>();
            modules = typeof(ModuleMenuDefine.模块定义).EnumToList();
            foreach (var item in modules)
            {
                tb_ModuleDefinition mod = new tb_ModuleDefinition();
                mod.ModeuleName = item.Name;
                mod.ModeuleNo = BizCodeGenerationHelper.GetBaseInfoNo(BaseInfoType.ModuleDefinition);
                bool isexit = await mdctr.ExistFieldValueAsync(e => e.ModeuleName, mod.ModeuleName);
                if (isexit)
                {
                    continue;
                }

                mod = await mdctr.AddReEntityAsync(mod);


                tb_MenuInfo menuInfoparent = new tb_MenuInfo();
                // menuInfoparent.MenuID = IdHelper.GetLongId(); //会自动生成ID 第一次这样运行出错，可能没有初始化暂时不管
                menuInfoparent.MenuName = item.Name;
                menuInfoparent.IsVisble = true;
                menuInfoparent.IsEnabled = true;
                menuInfoparent.CaptionCN = item.Name;
                menuInfoparent.MenuType = "导航菜单";
                menuInfoparent.parent_id = 0;
                menuInfoparent.Created_at = System.DateTime.Now;
                menuInfoparent = await mc.AddReEntityAsync(menuInfoparent);
                模块定义 module = (模块定义)Enum.Parse(typeof(模块定义), item.Name);
                switch (module)
                {
                    case 模块定义.生产管理:
                        InitNavMenu<生产管理>(menuInfoparent.MenuID);
                        break;
                    case 模块定义.进销存管理:
                        InitNavMenu<进销存管理>(menuInfoparent.MenuID);
                        break;
                    case 模块定义.客户关系:
                        InitNavMenu<客户关系>(menuInfoparent.MenuID);
                        break;
                    case 模块定义.财务管理:
                        InitNavMenu<财务管理>(menuInfoparent.MenuID);
                        break;
                    case 模块定义.行政管理:
                        InitNavMenu<行政管理>(menuInfoparent.MenuID);
                        break;
                    case 模块定义.报表管理:
                        InitNavMenu<报表管理>(menuInfoparent.MenuID);
                        break;
                    case 模块定义.基础资料:
                        InitNavMenu<基础资料>(menuInfoparent.MenuID);
                        break;
                    default:
                        break;
                }

            }

        }


        private async void InitNavMenu<T>(long parent_id)
        {
            tb_MenuInfoController<tb_MenuInfo> mc = _appContext.GetRequiredService<tb_MenuInfoController<tb_MenuInfo>>();
            List<EnumDto> modules = new List<EnumDto>();
            modules = typeof(T).EnumToList();
            foreach (var item in modules)
            {
                tb_MenuInfo menuInfoparent = new tb_MenuInfo();
                // menuInfoparent.MenuID = IdHelper.GetLongId(); //会自动生成ID 第一次这样运行出错，可能没有初始化暂时不管
                menuInfoparent.MenuName = item.Name;
                menuInfoparent.IsVisble = true;
                menuInfoparent.IsEnabled = true;
                menuInfoparent.CaptionCN = item.Name;
                menuInfoparent.MenuType = "导航菜单";
                menuInfoparent.parent_id = parent_id;
                menuInfoparent.Created_at = System.DateTime.Now;
     
                bool isexit = await mc.ExistFieldValueAsync(e => e.MenuName, item.Name);
                if (isexit)
                {
                    continue;
                }
                var newMenuItem = await mc.AddReEntityAsync(menuInfoparent);
                List<MenuAssemblyInfo> list = UIHelper.RegisterForm();
                var arrs = list.GroupBy(x => x.MenuPath.Split('|')[0]);
            }
        }








    }
}
