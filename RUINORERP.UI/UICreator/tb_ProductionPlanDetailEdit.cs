
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:02
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
    /// 生产计划明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProductionPlanDetailEdit:UserControl
    {
     public tb_ProductionPlanDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProductionPlanDetail UIToEntity()
        {
        tb_ProductionPlanDetail entity = new tb_ProductionPlanDetail();
                     entity.PPID = Int64.Parse(txtPPID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Specifications = txtSpecifications.Text ;
                       entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.RequirementDate = DateTime.Parse(txtRequirementDate.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.CompletedQuantity = Int32.Parse(txtCompletedQuantity.Text);
                        entity.AnalyzedQuantity = Int32.Parse(txtAnalyzedQuantity.Text);
                        entity.IsAnalyzed = Boolean.Parse(txtIsAnalyzed.Text);
                                return entity;
}
        */

        
        private tb_ProductionPlanDetail _EditEntity;
        public tb_ProductionPlanDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProductionPlanDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProductionPlan>(entity, k => k.PPID, v=>v.XXNAME, cmbPPID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlanDetail>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlanDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlanDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionPlanDetail>(entity, t => t.RequirementDate, dtpRequirementDate,false);
          // DataBindingHelper.BindData4Cmb<tb_BOM_S>(entity, k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlanDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlanDetail>(entity, t => t.CompletedQuantity, txtCompletedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlanDetail>(entity, t => t.AnalyzedQuantity, txtAnalyzedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ProductionPlanDetail>(entity, t => t.IsAnalyzed, chkIsAnalyzed, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



