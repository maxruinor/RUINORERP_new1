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
using RUINORERP.Business.Processor;
using SqlSugar;
using RUINORERP.Business.Security;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("箱规编辑", true, UIType.单表数据)]
    public partial class UCBoxRulesEdit : BaseEditGeneric<tb_BoxRules>
    {
        public UCBoxRulesEdit()
        {
            InitializeComponent();
        }


        public override void BindData(BaseEntity BrEntity)
        {
            tb_BoxRules entity = BrEntity as tb_BoxRules;

            if (entity.BoxRules_ID == 0)
            {
                entity.ActionStatus= ActionStatus.新增;
                entity.Is_enabled = true;
                entity.Created_at = DateTime.Now;
                entity.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            }
            else
            {
                entity.ActionStatus = ActionStatus.修改;
                entity.Modified_at = DateTime.Now;
                entity.Modified_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            }

            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.BoxRuleName, txtBoxRuleName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.QuantityPerBox, txtQuantityPerBox, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4Cmb<tb_CartoonBox>(entity, t => t.CartonID, t => t.CartonName, cmbCartonID);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Volume.ToString(), txtVolume, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.GrossWeight.ToString(), txtGrossWeight, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.NetWeight.ToString(), txtNetWeight, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.PackingMethod, txtPackingMethod, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Length.ToString(), txtLength, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Width.ToString(), txtWidth, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Height.ToString(), txtHeight, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_BoxRules>(entity, t => t.Is_enabled, chkIs_enabled, false);

            //先绑定这个。InitFilterForControl 这个才生效, 一共三个来控制，这里分别是绑定ID和SKU。下面InitFilterForControlByExp 是生成快捷按钮
            // DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, k => k.su txtProdDetailSKU, BindDataType4TextBox.Text, true);
            //DataBindingHelper.BindData4TextBoxWithTagQuery<tb_BoxRules>(entity, v => v.ProdDetailID, txtProdDetailSKU, true);



            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_BoxRulesValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);

                var lambda = Expressionable.Create<tb_Packing>()
                   .And(t => t.Is_enabled == true)
                   .ToExpression();
                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Packing).Name + "Processor");
                QueryFilter queryFilter = baseProcessor.GetQueryFilter();
                queryFilter.FilterLimitExpressions.Add(lambda);
                DataBindingHelper.BindData4Cmb<tb_Packing>(entity, k => k.Pack_ID, v => v.PackagingName, cmbPack_ID, queryFilter.GetFilterExpression<tb_Packing>(), true);
                DataBindingHelper.InitFilterForControlByExp<tb_Packing>(entity, cmbPack_ID, c => c.PackagingName, queryFilter);
            }
            else
            {
                DataBindingHelper.BindData4Cmb<tb_Packing>(entity, k => k.Pack_ID, v => v.PackagingName, cmbPack_ID, true);
            }
            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {

                //权限允许
                if ((true && entity.ActionStatus == ActionStatus.修改) || (true && entity.ActionStatus == ActionStatus.新增))
                {

                    if (entity.QuantityPerBox > 0 && s2.PropertyName == entity.GetPropertyName<tb_BoxRules>(c => c.QuantityPerBox))
                    {
                        //entity.tb_packing.tb_prod.tb_unit
                        entity.PackingMethod = $"每{entity.QuantityPerBox}一箱";
                    }

                    if (entity.CartonID > 0 && s2.PropertyName == entity.GetPropertyName<tb_BoxRules>(c => c.CartonID))
                    {
                        //entity.Length=0;
                      
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_BoxRules>(c => c.Length) ||
                       s2.PropertyName == entity.GetPropertyName<tb_BoxRules>(c => c.Width) ||
                       s2.PropertyName == entity.GetPropertyName<tb_BoxRules>(c => c.Height))
                    {
                        entity.Volume = entity.Length * entity.Width * entity.Height;
                    }

                    //if (entity.QuantityPerBox > 0 && s2.PropertyName == entity.GetPropertyName<tb_BoxRules>(c => c.Unit_ID))
                    //{
                    //    entity.PackingMethod = $"每{entity.QuantityPerBox}{cmbUnit_ID.SelectedText}一箱";
                    //}

                }

            };




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

        private void UCBoxRulesEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
