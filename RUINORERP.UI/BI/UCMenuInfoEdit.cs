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


namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("菜单编辑", true, UIType.单表数据)]
    public partial class UCMenuInfoEdit : BaseEditGeneric<tb_MenuInfo>
    {
        public UCMenuInfoEdit()
        {
            InitializeComponent();
        }

        private tb_MenuInfo _EditEntity;
        public tb_MenuInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity)
        {
            // DataBindingHelper.BindData4Cmb<tb_ModuleDefinition>(entity, k => k.ModuleID, v=>v.XXNAME, cmbModuleID);
            _EditEntity = entity as tb_MenuInfo;
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuName, txtMenuName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuType, txtMenuType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BIBaseForm, txtBIBaseForm, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.UIType, txtUIType, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.CaptionCN, txtCaptionCN, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.CaptionEN, txtCaptionEN, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.FormName, txtFormName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.ClassPath, txtClassPath, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.EntityName, txtEntityName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_MenuInfo>(entity, t => t.IsVisble, chkIsVisble, false);
            DataBindingHelper.BindData4CheckBox<tb_MenuInfo>(entity, t => t.IsEnabled, chkIsEnabled, false);
            //有默认值

            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Parent_id, txtParent_id, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Discription, txtDiscription, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuNo, txtMenuNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuLevel, txtMenuLevel, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.HotKey, txtHotKey, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.DefaultLayout, txtDefaultLayout, BindDataType4TextBox.Text, false);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        Business.LogicaService.UnitController mc = Startup.GetFromFac<UnitController>();
        /*
        private bool RebindUI(bool saveObject, bool rebind)
        {
            // disable events
            this.ResourceBindingSource.RaiseListChangedEvents = false;
            this.AssignmentsBindingSource.RaiseListChangedEvents = false;
            try
            {
                // unbind the UI
                UnbindBindingSource(this.AssignmentsBindingSource, saveObject, false);
                UnbindBindingSource(this.ResourceBindingSource, saveObject, true);
                this.AssignmentsBindingSource.DataSource = this.ResourceBindingSource;

                // save or cancel changes
                if (saveObject)
                {
                    Resource.ApplyEdit();
                    try
                    {
                        Resource = Resource.Save();
                    }
                    catch (Csla.DataPortalException ex)
                    {
                        MessageBox.Show(ex.BusinessException.ToString(),
                          "Error saving", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(),
                          "Error Saving", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                else
                    Resource.CancelEdit();

                return true;
            }
            finally
            {
                // rebind UI if requested
                if (rebind)
                    BindUI();

                // restore events
                this.ResourceBindingSource.RaiseListChangedEvents = true;
                this.AssignmentsBindingSource.RaiseListChangedEvents = true;

                if (rebind)
                {
                    // refresh the UI if rebinding
                    this.ResourceBindingSource.ResetBindings(false);
                    this.AssignmentsBindingSource.ResetBindings(false);
                }
            }
        }
        */

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
