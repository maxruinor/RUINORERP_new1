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
using RUINORERP.Business.Processor;
namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("菜单管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCMenuInfoList : BaseForm.BaseListGeneric<tb_MenuInfo>
    {
        public UCMenuInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCMenuInfoEdit);
            #region 数据源来源单位和目标单位的字段不等于原始单位表中的主键字段，在显示时通过主键无法找到对应显示的名称

            base.SetForeignkeyPointsList<tb_MenuInfo, tb_MenuInfo>(s => s.Parent_id, t => t.MenuID);
            base.SetForeignkeyPointsList<tb_MenuInfo, tb_MenuInfo>(s => s.MenuID, t => t.Parent_id);
            // base.SetForeignkeyPointsList<tb_Unit, tb_Unit_Conversion>(s => s.Unit_ID, t => t.Target_unit_id);

            ColDisplayTypes.Add(typeof(tb_ModuleDefinition));

            #endregion
        }


        //public override void QueryConditionBuilder()
        //{
        //    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_MenuInfo).Name + "Processor");
        //    QueryConditionFilter = baseProcessor.GetQueryFilter();
        //}


        //因为tb_P4FieldInfo中引用了字段表中的信息，所以要使用导航删除。但是一定要细心

        tb_MenuInfoController<tb_MenuInfo> childctr = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
        protected async override Task<bool> Delete()
        {
            bool rs = false;
            foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
            {
                tb_MenuInfo Info = dr.DataBoundItem as tb_MenuInfo;
                 rs = await childctr.BaseDeleteByNavAsync(dr.DataBoundItem as tb_MenuInfo);
                if (rs)
                {
                    //提示
                    MainForm.Instance.PrintInfoLog($"{Info.MenuName},{Info.CaptionCN}删除成功。");
                }
            }
            Query();
            return rs;
        }
    }
}
