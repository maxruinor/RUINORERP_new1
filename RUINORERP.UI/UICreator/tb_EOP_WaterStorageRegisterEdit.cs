
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/17/2025 16:59:38
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
    /// 蓄水登记表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_EOP_WaterStorageRegisterEdit:UserControl
    {
     public tb_EOP_WaterStorageRegisterEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_EOP_WaterStorageRegister UIToEntity()
        {
        tb_EOP_WaterStorageRegister entity = new tb_EOP_WaterStorageRegister();
                     entity.WSRNo = txtWSRNo.Text ;
                       entity.PlatformOrderNo = txtPlatformOrderNo.Text ;
                       entity.PlatformType = Int32.Parse(txtPlatformType.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.PlatformFeeAmount = Decimal.Parse(txtPlatformFeeAmount.Text);
                        entity.OrderDate = DateTime.Parse(txtOrderDate.Text);
                        entity.ShippingAddress = txtShippingAddress.Text ;
                       entity.ShippingWay = txtShippingWay.Text ;
                       entity.TrackNo = txtTrackNo.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_EOP_WaterStorageRegister _EditEntity;
        public tb_EOP_WaterStorageRegister EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_EOP_WaterStorageRegister entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.WSRNo, txtWSRNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.PlatformType, txtPlatformType, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.PlatformFeeAmount.ToString(), txtPlatformFeeAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_EOP_WaterStorageRegister>(entity, t => t.OrderDate, dtpOrderDate,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_EOP_WaterStorageRegister>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_EOP_WaterStorageRegister>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_EOP_WaterStorageRegister>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_EOP_WaterStorageRegister>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



