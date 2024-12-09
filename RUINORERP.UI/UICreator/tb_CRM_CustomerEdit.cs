
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:42
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
    /// 目标客户，公海客户 CRM系统中使用，给成交客户作外键引用数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CRM_CustomerEdit:UserControl
    {
     public tb_CRM_CustomerEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CRM_Customer UIToEntity()
        {
        tb_CRM_Customer entity = new tb_CRM_Customer();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.LeadID = Int64.Parse(txtLeadID.Text);
                        entity.Region_ID = Int64.Parse(txtRegion_ID.Text);
                        entity.ProvinceID = Int64.Parse(txtProvinceID.Text);
                        entity.CityID = Int64.Parse(txtCityID.Text);
                        entity.CustomerName = txtCustomerName.Text ;
                       entity.CustomerAddress = txtCustomerAddress.Text ;
                       entity.RepeatCustomer = Boolean.Parse(txtRepeatCustomer.Text);
                        entity.CustomerTags = txtCustomerTags.Text ;
                       entity.CustomerStatus = Int32.Parse(txtCustomerStatus.Text);
                        entity.GetCustomerSource = txtGetCustomerSource.Text ;
                       entity.SalePlatform = txtSalePlatform.Text ;
                       entity.Website = txtWebsite.Text ;
                       entity.CustomerLevel = Int32.Parse(txtCustomerLevel.Text);
                        entity.PurchaseCount = Int32.Parse(txtPurchaseCount.Text);
                        entity.TotalPurchaseAmount = Decimal.Parse(txtTotalPurchaseAmount.Text);
                        entity.DaysSinceLastPurchase = Int32.Parse(txtDaysSinceLastPurchase.Text);
                        entity.LastPurchaseDate = DateTime.Parse(txtLastPurchaseDate.Text);
                        entity.FirstPurchaseDate = DateTime.Parse(txtFirstPurchaseDate.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_CRM_Customer _EditEntity;
        public tb_CRM_Customer EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRM_Customer entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_CRM_Leads>(entity, k => k.LeadID, v=>v.XXNAME, cmbLeadID);
          // DataBindingHelper.BindData4Cmb<tb_CRM_Region>(entity, k => k.Region_ID, v=>v.XXNAME, cmbRegion_ID);
          // DataBindingHelper.BindData4Cmb<tb_Provinces>(entity, k => k.ProvinceID, v=>v.XXNAME, cmbProvinceID);
          // DataBindingHelper.BindData4Cmb<tb_Cities>(entity, k => k.CityID, v=>v.XXNAME, cmbCityID);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerName, txtCustomerName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerAddress, txtCustomerAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_CRM_Customer>(entity, t => t.RepeatCustomer, chkRepeatCustomer, false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerTags, txtCustomerTags, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerStatus, txtCustomerStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.GetCustomerSource, txtGetCustomerSource, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.SalePlatform, txtSalePlatform, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerLevel, txtCustomerLevel, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.PurchaseCount, txtPurchaseCount, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.TotalPurchaseAmount.ToString(), txtTotalPurchaseAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.DaysSinceLastPurchase, txtDaysSinceLastPurchase, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Customer>(entity, t => t.LastPurchaseDate, dtpLastPurchaseDate,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Customer>(entity, t => t.FirstPurchaseDate, dtpFirstPurchaseDate,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Customer>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Customer>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_CRM_Customer>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



