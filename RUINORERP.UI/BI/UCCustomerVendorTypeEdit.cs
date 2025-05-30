﻿using System;
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
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo( "往来单位等级编辑", true, UIType.单表数据)]
    public partial class UCCustomerVendorTypeEdit : BaseEditGeneric<tb_CustomerVendorType>
    {
        public UCCustomerVendorTypeEdit()
        {
            InitializeComponent();
        }

 

        public override void BindData(BaseEntity entity)
        {
           
            DataBindingHelper.BindData4TextBox<tb_CustomerVendorType>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CustomerVendorType>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_CustomerVendorType>(entity, t => t.BusinessPartnerType, cmbCustomerVendorType, BindDataType4Enum.EnumName, typeof(BusinessPartnerType));
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService <tb_CustomerVendorTypeValidator> (), kryptonPanel1.Controls);
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
