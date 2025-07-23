using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("模块定义", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCModuleDefinitionList : BaseForm.BaseListGeneric<tb_ModuleDefinition>
    {
        public UCModuleDefinitionList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCModuleDefinitionEdit);
        }


        /// <summary>
        /// 模块。一般会在初始化时自动添加，并且要添加对应的菜单。中途添加时。就要检测是否有对应的菜单
        /// </summary>
        /// <returns></returns>
        public override async Task<List<tb_ModuleDefinition>> Save()
        {
            List<tb_ModuleDefinition> list = new List<tb_ModuleDefinition>();
            list = await base.Save();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    //if (item.ModuleName.HasValue && item.Converted.Value)
                    //{
                    //    //对象的目标客户设置为已转换
                    //    var result = await MainForm.Instance.AppContext.Db.Queryable<tb_CustomerVendor>()
                    //        .Where(it => it.Customer_id == item.Customer_id)
                    //        .SingleAsync();

                    //    //如果修改了客户名称，则和销售客户同步
                    //    if (result != null && result.CVName != item.CustomerName)
                    //    {
                    //        result.CVName = item.CustomerName;
                    //        result.Phone = item.Contact_Phone;
                    //        await MainForm.Instance.AppContext.Db.Updateable<tb_CustomerVendor>(result)
                    //            .UpdateColumns(it => new { it.CVName, it.Phone })
                    //        //.SetColumns(it => it.CVName == item.CustomerName)//SetColumns是可以叠加的 写2个就2个字段赋值
                    //        .Where(it => it.Customer_id == item.Customer_id)
                    //        .ExecuteCommandAsync();
                    //    }
                    //}
                }
            }
            return list;
        }
    }
}
