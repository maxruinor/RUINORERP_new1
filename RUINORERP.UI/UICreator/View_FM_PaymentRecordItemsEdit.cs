
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:28
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
    /// 收付款单明细统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_FM_PaymentRecordItemsEdit:UserControl
    {
     public View_FM_PaymentRecordItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_FM_PaymentRecordItems UIToEntity()
        {
        View_FM_PaymentRecordItems entity = new View_FM_PaymentRecordItems();
                     entity.PaymentId = Int64.Parse(txtPaymentId.Text);
                        entity.PaymentNo = txtPaymentNo.Text ;
                       entity.SourceBillNo = txtSourceBillNo.Text ;
                       entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.PayeeInfoID = Int64.Parse(txtPayeeInfoID.Text);
                        entity.PayeeAccountNo = txtPayeeAccountNo.Text ;
                       entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.TotalForeignAmount = Decimal.Parse(txtTotalForeignAmount.Text);
                        entity.TotalLocalAmount = Decimal.Parse(txtTotalLocalAmount.Text);
                        entity.PaymentDate = DateTime.Parse(txtPaymentDate.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.PaymentStatus = Int32.Parse(txtPaymentStatus.Text);
                        entity.PaymentImagePath = txtPaymentImagePath.Text ;
                       entity.ReferenceNo = txtReferenceNo.Text ;
                       entity.IsReversed = Boolean.Parse(txtIsReversed.Text);
                        entity.ReversedOriginalId = Int64.Parse(txtReversedOriginalId.Text);
                        entity.ReversedOriginalNo = txtReversedOriginalNo.Text ;
                       entity.ReversedByPaymentId = Int64.Parse(txtReversedByPaymentId.Text);
                        entity.ReversedByPaymentNo = txtReversedByPaymentNo.Text ;
                       entity.PaymentDetailId = Int64.Parse(txtPaymentDetailId.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.ForeignAmount = Decimal.Parse(txtForeignAmount.Text);
                        entity.LocalAmount = Decimal.Parse(txtLocalAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.Remark = txtRemark.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private View_FM_PaymentRecordItems _EditEntity;
        public View_FM_PaymentRecordItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_FM_PaymentRecordItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.PaymentId, txtPaymentId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.PaymentNo, txtPaymentNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.Account_id, txtAccount_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.PayeeInfoID, txtPayeeInfoID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.Currency_ID, txtCurrency_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.TotalForeignAmount.ToString(), txtTotalForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.TotalLocalAmount.ToString(), txtTotalLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<View_FM_PaymentRecordItems>(entity, t => t.PaymentDate, dtpPaymentDate,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.Paytype_ID, txtPaytype_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.PaymentStatus, txtPaymentStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.PaymentImagePath, txtPaymentImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.ReferenceNo, txtReferenceNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_FM_PaymentRecordItems>(entity, t => t.IsReversed, chkIsReversed, false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.ReversedOriginalId, txtReversedOriginalId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.ReversedOriginalNo, txtReversedOriginalNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.ReversedByPaymentId, txtReversedByPaymentId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.ReversedByPaymentNo, txtReversedByPaymentNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.PaymentDetailId, txtPaymentDetailId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.ForeignAmount.ToString(), txtForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.LocalAmount.ToString(), txtLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_FM_PaymentRecordItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_FM_PaymentRecordItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<View_FM_PaymentRecordItems>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



