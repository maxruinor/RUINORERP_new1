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
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("收付款账号配置", true, UIType.单表数据)]
    public partial class UCPayMethodAccountMapperEdit : BaseEditGeneric<tb_PayMethodAccountMapper>
    {
        public UCPayMethodAccountMapperEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity baseEntity)
        {
            tb_PayMethodAccountMapper entity = baseEntity as tb_PayMethodAccountMapper;
            if (entity==null)
            {
                return;
            }
            if (entity.PAMID>0)
            {

            }
            else
            {
                entity.EffectiveDate = System.DateTime.Now;
                entity.ExpiryDate = System.DateTime.Now.AddYears(10);
            }
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);
            DataBindingHelper.BindData4TextBox<tb_PayMethodAccountMapper>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_PayMethodAccountMapper>(entity, t => t.EffectiveDate, dtpEffectiveDate, false);
            DataBindingHelper.BindData4DataTime<tb_PayMethodAccountMapper>(entity, t => t.ExpiryDate, dtpExpiryDate, false);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_PayMethodAccountMapperValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
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
