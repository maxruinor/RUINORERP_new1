
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:17
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
    /// 先销售合同再订单,条款内容后面补充 注意一个合同可以多个发票一个发票也可以多个合同数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SO_ContractEdit:UserControl
    {
     public tb_SO_ContractEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SO_Contract UIToEntity()
        {
        tb_SO_Contract entity = new tb_SO_Contract();
                     entity.ID = Int64.Parse(txtID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.BillingInfo_ID = Int64.Parse(txtBillingInfo_ID.Text);
                        entity.TemplateId = Int64.Parse(txtTemplateId.Text);
                        entity.SOContractNo = txtSOContractNo.Text ;
                       entity.Contract_Date = DateTime.Parse(txtContract_Date.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.EffectiveDate = DateTime.Parse(txtEffectiveDate.Text);
                        entity.ExpireDate = DateTime.Parse(txtExpireDate.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.ClauseContent = txtClauseContent.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_SO_Contract _EditEntity;
        public tb_SO_Contract EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SO_Contract entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Company>(entity, k => k.ID, v=>v.XXNAME, cmbID);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_BillingInformation>(entity, k => k.BillingInfo_ID, v=>v.XXNAME, cmbBillingInfo_ID);
          // DataBindingHelper.BindData4Cmb<tb_ContractTemplate>(entity, k => k.TemplateId, v=>v.XXNAME, cmbTemplateId);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.SOContractNo, txtSOContractNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_SO_Contract>(entity, t => t.Contract_Date, dtpContract_Date,false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SO_Contract>(entity, t => t.EffectiveDate, dtpEffectiveDate,false);
           DataBindingHelper.BindData4DataTime<tb_SO_Contract>(entity, t => t.ExpireDate, dtpExpireDate,false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.ClauseContent, txtClauseContent, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_SO_Contract>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SO_Contract>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_SO_Contract>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SO_Contract>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



