
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:09
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
    /// 付款申请单明细-对应的应付单据项目数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_PaymentDetailEdit:UserControl
    {
     public tb_FM_PaymentDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_PaymentDetail UIToEntity()
        {
        tb_FM_PaymentDetail entity = new tb_FM_PaymentDetail();
                     entity.PaymentID = Int64.Parse(txtPaymentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.tb__DepartmentID = Int64.Parse(txttb__DepartmentID.Text);
                        entity.SourceBill_BizType = Int32.Parse(txtSourceBill_BizType.Text);
                        entity.SourceBill_ID = Int64.Parse(txtSourceBill_ID.Text);
                        entity.SourceBillNO = txtSourceBillNO.Text ;
                       entity.IsAdvancePayment = Boolean.Parse(txtIsAdvancePayment.Text);
                        entity.PayReasonItems = txtPayReasonItems.Text ;
                       entity.Summary = txtSummary.Text ;
                       entity.SubAmount = Decimal.Parse(txtSubAmount.Text);
                        entity.SubPamountInWords = txtSubPamountInWords.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_FM_PaymentDetail _EditEntity;
        public tb_FM_PaymentDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PaymentDetail entity)
        {
        _EditEntity = entity;
                       tb__DepartmentID主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_FM_Payment>(entity, k => k.PaymentID, v=>v.XXNAME, cmbPaymentID);
          tb__DepartmentID主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          tb__DepartmentID主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
// DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          tb__DepartmentID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.tb__DepartmentID, txttb__DepartmentID, BindDataType4TextBox.Qty,false);
          tb__DepartmentID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.SourceBill_BizType, txtSourceBill_BizType, BindDataType4TextBox.Qty,false);
          tb__DepartmentID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.SourceBill_ID, txtSourceBill_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.SourceBillNO, txtSourceBillNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentDetail>(entity, t => t.IsAdvancePayment, chkIsAdvancePayment, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.PayReasonItems, txtPayReasonItems, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.SubAmount.ToString(), txtSubAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.SubPamountInWords, txtSubPamountInWords, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentDetail>(entity, t => t.Created_at, dtpCreated_at,false);
          tb__DepartmentID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentDetail>(entity, t => t.Modified_at, dtpModified_at,false);
          tb__DepartmentID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentDetail>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



