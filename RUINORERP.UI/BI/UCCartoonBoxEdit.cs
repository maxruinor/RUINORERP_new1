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

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("产品类型编辑", true, UIType.单表数据)]
    public partial class UCCartoonBoxEdit : BaseEditGeneric<tb_CartoonBox>
    {
        public UCCartoonBoxEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity baseEntity)
        {
            tb_CartoonBox entity = baseEntity as tb_CartoonBox;
            entity.Material = "卡通箱 (CARTON)";

            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.CartonName, txtCartonName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Color, txtColor, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Material, txtMaterial, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.EmptyBoxWeight.ToString(), txtEmptyBoxWeight, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.MaxLoad.ToString(), txtMaxLoad, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Thickness.ToString(), txtThickness, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Length.ToString(), txtLength, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Width.ToString(), txtWidth, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Height.ToString(), txtHeight, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Volume.ToString(), txtVolume, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.FluteType, txtFluteType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.PrintType, txtPrintType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.CustomPrint, txtCustomPrint, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_CartoonBox>(entity, t => t.Is_enabled, chkIs_enabled, false);
            DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {


                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_CartoonBox>(c => c.Width) || s2.PropertyName == entity.GetPropertyName<tb_CartoonBox>(c => c.Length)
                  || s2.PropertyName == entity.GetPropertyName<tb_CartoonBox>(c => c.Height))
                    {
                        entity.Volume = entity.Width * entity.Length * entity.Height;
                    }

        


                }





                

            };

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
