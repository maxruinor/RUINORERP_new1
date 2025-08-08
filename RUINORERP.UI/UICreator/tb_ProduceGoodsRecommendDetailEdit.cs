
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:59
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;

namespace RUINORERP.UI
{
    /// <summary>
    /// 自制成品建议数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProduceGoodsRecommendDetailEdit:UserControl
    {
     public tb_ProduceGoodsRecommendDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProduceGoodsRecommendDetail UIToEntity()
        {
        tb_ProduceGoodsRecommendDetail entity = new tb_ProduceGoodsRecommendDetail();
                     entity.PDID = Int64.Parse(txtPDID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ID = Int64.Parse(txtID.Text);
                        entity.ParentId = Int64.Parse(txtParentId.Text);
                        entity.Specifications = txtSpecifications.Text ;
                       entity.property = txtproperty.Text ;
                       entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.RequirementQty = Int32.Parse(txtRequirementQty.Text);
                        entity.RecommendQty = Int32.Parse(txtRecommendQty.Text);
                        entity.PlanNeedQty = Int32.Parse(txtPlanNeedQty.Text);
                        entity.PreStartDate = DateTime.Parse(txtPreStartDate.Text);
                        entity.PreEndDate = DateTime.Parse(txtPreEndDate.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.RefBillNO = txtRefBillNO.Text ;
                       entity.RefBillType = Int32.Parse(txtRefBillType.Text);
                        entity.RefBillID = Int64.Parse(txtRefBillID.Text);
                                return entity;
}
        */

        
        private tb_ProduceGoodsRecommendDetail _EditEntity;
        public tb_ProduceGoodsRecommendDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProduceGoodsRecommendDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProductionDemand>(entity, k => k.PDID, v=>v.XXNAME, cmbPDID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.ID, txtID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.ParentId, txtParentId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_BOM_S>(entity, k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.RequirementQty, txtRequirementQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.RecommendQty, txtRecommendQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.PlanNeedQty, txtPlanNeedQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProduceGoodsRecommendDetail>(entity, t => t.PreStartDate, dtpPreStartDate,false);
           DataBindingHelper.BindData4DataTime<tb_ProduceGoodsRecommendDetail>(entity, t => t.PreEndDate, dtpPreEndDate,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.RefBillNO, txtRefBillNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.RefBillType, txtRefBillType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceGoodsRecommendDetail>(entity, t => t.RefBillID, txtRefBillID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



