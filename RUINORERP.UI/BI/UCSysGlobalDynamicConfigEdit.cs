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
using NPOI.OpenXmlFormats.Dml.Diagram;
using Mysqlx.Crud;
using RUINORERP.Global;
using RUINORERP.UI.SysConfig;


namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("动态参数配置", true, UIType.单表数据)]
    public partial class UCSysGlobalDynamicConfigEdit : BaseEditGeneric<tb_SysGlobalDynamicConfig>
    {
        public UCSysGlobalDynamicConfigEdit()
        {
            InitializeComponent();
        }



        private tb_SysGlobalDynamicConfig _EditEntity;
        public tb_SysGlobalDynamicConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity)
        {
            tb_SysGlobalDynamicConfig _EditEntity = entity as tb_SysGlobalDynamicConfig;
            if (_EditEntity.ConfigID == 0)
            {
                _EditEntity.Created_at = System.DateTime.Now;
            }

            DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.ConfigKey, txtConfigKey, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.ConfigValue, txtConfigValue, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_SysGlobalDynamicConfig>(entity, k => k.ValueType, typeof(ConfigValueType), cmbValueType, false);
            DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.ConfigType, txtConfigType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_SysGlobalDynamicConfig>(entity, t => t.IsActive, chkIsActive, false);
            base.BindData(entity);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        Business.LogicaService.UnitController mc = Startup.GetFromFac<UnitController>();


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
