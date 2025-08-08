
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:38
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
    /// 开票资料数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_InvoiceInfoEdit:UserControl
    {
     public tb_InvoiceInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_InvoiceInfo UIToEntity()
        {
        tb_InvoiceInfo entity = new tb_InvoiceInfo();
                     entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.PICompanyName = txtPICompanyName.Text ;
                       entity.PITaxID = txtPITaxID.Text ;
                       entity.PIAddress = txtPIAddress.Text ;
                       entity.PITEL = txtPITEL.Text ;
                       entity.PIBankName = txtPIBankName.Text ;
                       entity.PIBankNo = txtPIBankNo.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.信用天数 = txt信用天数.Text ;
                       entity.往来余额 = txt往来余额.Text ;
                       entity.应收款 = txt应收款.Text ;
                       entity.预收款 = txt预收款.Text ;
                       entity.纳税号 = txt纳税号.Text ;
                       entity.开户行 = txt开户行.Text ;
                       entity.银行帐号 = txt银行帐号.Text ;
                               return entity;
}
        */

        
        private tb_InvoiceInfo _EditEntity;
        public tb_InvoiceInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_InvoiceInfo entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.PICompanyName, txtPICompanyName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.PITaxID, txtPITaxID, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.PIAddress, txtPIAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.PITEL, txtPITEL, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.PIBankName, txtPIBankName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.PIBankNo, txtPIBankNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.信用天数, txt信用天数, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.往来余额, txt往来余额, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.应收款, txt应收款, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.预收款, txt预收款, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.纳税号, txt纳税号, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.开户行, txt开户行, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_InvoiceInfo>(entity, t => t.银行帐号, txt银行帐号, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



