
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:06
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
    /// 采购商品建议数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurGoodsRecommendDetailEdit:UserControl
    {
     public tb_PurGoodsRecommendDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurGoodsRecommendDetail UIToEntity()
        {
        tb_PurGoodsRecommendDetail entity = new tb_PurGoodsRecommendDetail();
                     entity.PDID = Int64.Parse(txtPDID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.RecommendPurPrice = Decimal.Parse(txtRecommendPurPrice.Text);
                        entity.ActualRequiredQty = Int32.Parse(txtActualRequiredQty.Text);
                        entity.RecommendQty = Int32.Parse(txtRecommendQty.Text);
                        entity.RequirementQty = Int32.Parse(txtRequirementQty.Text);
                        entity.RequirementDate = DateTime.Parse(txtRequirementDate.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.RefBillNO = txtRefBillNO.Text ;
                       entity.RefBillType = Int64.Parse(txtRefBillType.Text);
                        entity.RefBillID = Int64.Parse(txtRefBillID.Text);
                        entity.PDCID_RowID = Int64.Parse(txtPDCID_RowID.Text);
                                return entity;
}
        */

        
        private tb_PurGoodsRecommendDetail _EditEntity;
        public tb_PurGoodsRecommendDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurGoodsRecommendDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProductionDemand>(entity, k => k.PDID, v=>v.XXNAME, cmbPDID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.RecommendPurPrice.ToString(), txtRecommendPurPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.ActualRequiredQty, txtActualRequiredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.RecommendQty, txtRecommendQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.RequirementQty, txtRequirementQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurGoodsRecommendDetail>(entity, t => t.RequirementDate, dtpRequirementDate,false);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.RefBillNO, txtRefBillNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.RefBillType, txtRefBillType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.RefBillID, txtRefBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurGoodsRecommendDetail>(entity, t => t.PDCID_RowID, txtPDCID_RowID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



