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
using RUINORERP.Global;
using RUINORERP.UI.UCSourceGrid;
using static OpenTK.Graphics.OpenGL.GL;
using RUINORERP.Model.ReminderModel.ReminderRules;

namespace RUINORERP.UI.SmartReminderClient
{
    [MenuAttrAssemblyInfo("安全库存配置编辑", true, UIType.单表数据)]
    public partial class UCSafetyStockConfigEdit : BaseEditGeneric<SafetyStockConfig>
    {
        public UCSafetyStockConfigEdit()
        {
            InitializeComponent();
        }


        public SafetyStockConfig safetyStockConfig { get; set; }

        public void BindData(SafetyStockConfig entity)
        {
            if (entity == null)
            {
                entity = new SafetyStockConfig();
            }
            entity._ProductIds = safetyStockConfig.ProductIds.ToJson();
            DataBindingHelper.BindData4TextBox<SafetyStockConfig>(entity, t => t._ProductIds, txt_ProductIds, BindDataType4TextBox.Text, false);
            txt_ProductIds.ReadOnly = true;//要选取，不能手输入。不然格式错误

            DataBindingHelper.BindData4TextBox<SafetyStockConfig>(entity, t => t.MinStock, txtMinStock, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<SafetyStockConfig>(entity, t => t.MaxStock, txtMaxStock, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<SafetyStockConfig>(entity, t => t.IsEnabled, chkIs_enabled, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.EndEdit();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UCBoxRulesEdit_Load(object sender, EventArgs e)
        {
            BindData(safetyStockConfig);
        }

        private void btnSeleted_Click(object sender, EventArgs e)
        {
            using (QueryFormGeneric dg = new QueryFormGeneric())
            {
                dg.Text = "安全库存检测产品对象导入";
                dg.StartPosition = FormStartPosition.CenterScreen;
                dg.prodQuery.MultipleChoices = true;
                //dg.prodQuery.QueryField = EditEntity.GetPropertyName<tb_Stocktake>(c => c.Location_ID);
                //dg.prodQuery.QueryValue = EditEntity.Location_ID;
                dg.prodQuery.UseType = ProdQueryUseType.盘点导入;

                if (dg.ShowDialog() == DialogResult.OK)
                {
                    /// Control.Tag = dg.QueryObjects;
                    // Control.Value = dg.QueryValue;
                    //将查询到的结果转换为单据明细 ,货加号来自于库存，后面添加查询对象除产品，等还可以依库存查
                    safetyStockConfig.ProductIds.Clear();
                    foreach (View_ProdDetail item in dg.prodQuery.QueryObjects)
                    {
                        safetyStockConfig.ProductIds.Add(item.ProdDetailID);
                    }
                }
            }
            //将集合用,号隔开后拼起来
            txt_ProductIds.Text = safetyStockConfig.ProductIds.ToJson();
        }
    }
}
