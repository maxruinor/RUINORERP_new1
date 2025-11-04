using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business;
using RUINORERP.UI.Common;
using RUINORERP.Global;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class UCLocationEdit : BaseEditGeneric<tb_Location>
    {
        public UCLocationEdit()
        {
            InitializeComponent();
        }

        private tb_Location _EditEntity;

        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_Location;
            if (_EditEntity.Location_ID == 0)
            {
                _EditEntity.LocationCode = BizCodeService.GetBaseInfoNo(BaseInfoType.Location.ToString());
            }
            DataBindingHelper.BindData4Cmb<tb_LocationType>(entity, k => k.LocationType_ID, v=>v.TypeName, txtLocationType_ID);
            DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Name, txtName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, CmbContactPerson);
            DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.LocationCode, txtLocationCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Tel, txtTel, BindDataType4TextBox.Text, false);

            //有默认值
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_Employee>(entity, t => t.Is_enabled, rdbis_enabledYes, rdbis_enabledNo);


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_LocationValidator> (), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            base.BindData(entity);
        }

 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

         

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
