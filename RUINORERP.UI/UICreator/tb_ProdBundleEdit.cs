
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:53
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
    /// 产品套装表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdBundleEdit:UserControl
    {
     public tb_ProdBundleEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProdBundle UIToEntity()
        {
        tb_ProdBundle entity = new tb_ProdBundle();
                     entity.BundleName = txtBundleName.Text ;
                       entity.Description = txtDescription.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.TargetQty = Int32.Parse(txtTargetQty.Text);
                        entity.ImagesPath = txtImagesPath.Text ;
                       entity.BundleImage = Binary.Parse(txtBundleImage.Text);
                        entity.Weight = Decimal.Parse(txtWeight.Text);
                        entity.Market_Price = Decimal.Parse(txtMarket_Price.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Is_available = Boolean.Parse(txtIs_available.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_ProdBundle _EditEntity;
        public tb_ProdBundle EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdBundle entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.BundleName, txtBundleName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.TargetQty, txtTargetQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.ImagesPath, txtImagesPath, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.BundleImage.ToString(), txtBundleImage, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Weight.ToString(), txtWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Market_Price.ToString(), txtMarket_Price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_ProdBundle>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_ProdBundle>(entity, t => t.Is_available, chkIs_available, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_ProdBundle>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProdBundle>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ProdBundle>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProdBundle>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_ProdBundle>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_ProdBundle>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



