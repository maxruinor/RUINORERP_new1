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
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Global;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("店铺编辑", true, UIType.单表数据)]
    public partial class UCStoreEdit : BaseEditGeneric<tb_OnlineStoreInfo>
    {
        public UCStoreEdit()
        {
            InitializeComponent();
        }


        private tb_OnlineStoreInfo _EditEntity;
        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_OnlineStoreInfo;
            if (_EditEntity.Store_ID == 0)
            {
                _EditEntity.StoreCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.StoreCode);
            }
            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.StoreCode, txtStoreCode, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.StoreName, txtStoreName, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.PlatformName, txtPlatformName, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Contact, txtContact, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Phone, txtPhone, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.ResponsiblePerson, txtResponsiblePerson, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

 
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_OnlineStoreInfoValidator>  (), kryptonPanel1.Controls);
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
