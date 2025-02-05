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
using RUINORERP.Global;
using System.Collections;
using System.Linq.Expressions;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("单位换算列表", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCUnitConversionList : BaseForm.BaseListGeneric<tb_Unit_Conversion>
    {

        public UCUnitConversionList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCUnitConversionEdit);

            //因为生成时   [FKRelationAttribute("tb_Unit","Target_unit_id")] 这里不对。所以用下面代码修正。生成的会被覆盖
            #region 数据源来源单位和目标单位的字段不等于原始单位表中的主键字段，在显示时通过主键无法找到对应显示的名称

            base.SetForeignkeyPointsList<tb_Unit, tb_Unit_Conversion>(s => s.Unit_ID, t => t.Source_unit_id);
            base.SetForeignkeyPointsList<tb_Unit, tb_Unit_Conversion>(s => s.Unit_ID, t => t.Target_unit_id);

            ColDisplayTypes.Add(typeof(tb_Unit));

            #endregion

        }

        //protected override void Modify()
        //{
        //    UCUnitEdit frmadd = new UCUnitEdit();
        //    base.Modify<tb_Unit>(frmadd);
        //    // tb_Unit modifyobj = base.bindingSourceList.Current as tb_Unit;
        //}



        /*
        protected async override void Save()
        {
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_Unit;

                switch (entity.actionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                    case ActionStatus.修改:
                        ReturnResults<tb_Unit> rr = new ReturnResults<tb_Unit>();
                        rr = await ctr.SaveOrUpdate(entity);
                        if (rr.Succeeded)
                        {
                            base.ToolBarEnabledControl(MenuItemEnums.保存);
                        }
                        //tb_Unit Entity = await ctr.AddReEntityAsync(entity);
                        //如果新增 保存后。还是新增加状态，因为增加另一条。所以保存不为灰色。所以会重复增加
                        break;
                    case ActionStatus.删除:
                        break;
                    default:
                        break;
                }
                entity.HasChanged = false;
            }
        }
        */









    }
}
