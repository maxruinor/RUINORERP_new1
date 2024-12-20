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


    [MenuAttrAssemblyInfo( "收藏信息编辑", true, UIType.单表数据)]
    public partial class UCFavoriteEdit : BaseEditGeneric<tb_Favorite>
    {
        public UCFavoriteEdit()
        {
            InitializeComponent();
            BindData();
        }

        public override void BindData(BaseEntity entity)
        {
        }

        private tb_Favorite _EditEntity;
        public tb_Favorite EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData()
        {
            DataBindingHelper.BindData4TextBox<tb_Favorite>(base.bindingSourceEdit, t => t.Ref_Table_Name, txtRef_Table_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Favorite>(base.bindingSourceEdit, t => t.ModuleName, txtModuleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Favorite>(base.bindingSourceEdit, t => t.BusinessType, txtBusinessType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_Favorite>(base.bindingSourceEdit, t => t.Public_enabled, chkPubli_enabled, false);
            DataBindingHelper.BindData4CheckBox<tb_Favorite>(base.bindingSourceEdit, t => t.is_enabled, chkis_enabled, false);
            DataBindingHelper.BindData4CheckBox<tb_Favorite>(base.bindingSourceEdit, t => t.is_available, chkis_available, false);
            DataBindingHelper.BindData4TextBox<tb_Favorite>(base.bindingSourceEdit, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
          //  base.BindData(BASE.bindingSourceEdit);
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
