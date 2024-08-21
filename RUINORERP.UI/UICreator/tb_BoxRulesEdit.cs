
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 17:26:39
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
    /// 箱规表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BoxRulesEdit:UserControl
    {
     public tb_BoxRulesEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BoxRules UIToEntity()
        {
        tb_BoxRules entity = new tb_BoxRules();
                     entity.Pack_ID = Int64.Parse(txtPack_ID.Text);
                        entity.CartonID = Int64.Parse(txtCartonID.Text);
                        entity.BoxRuleName = txtBoxRuleName.Text ;
                       entity.QuantityPerBox = Int32.Parse(txtQuantityPerBox.Text);
                        entity.PackingMethod = txtPackingMethod.Text ;
                       entity.Length = Decimal.Parse(txtLength.Text);
                        entity.Width = Decimal.Parse(txtWidth.Text);
                        entity.Height = Decimal.Parse(txtHeight.Text);
                        entity.Volume = Decimal.Parse(txtVolume.Text);
                        entity.GrossWeight = Decimal.Parse(txtGrossWeight.Text);
                        entity.NetWeight = Decimal.Parse(txtNetWeight.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_BoxRules _EditEntity;
        public tb_BoxRules EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BoxRules entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Packing>(entity, k => k.Pack_ID, v=>v.XXNAME, cmbPack_ID);
          // DataBindingHelper.BindData4Cmb<tb_CartoonBox>(entity, k => k.CartonID, v=>v.XXNAME, cmbCartonID);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.BoxRuleName, txtBoxRuleName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.QuantityPerBox, txtQuantityPerBox, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.PackingMethod, txtPackingMethod, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Length.ToString(), txtLength, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Width.ToString(), txtWidth, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Height.ToString(), txtHeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Volume.ToString(), txtVolume, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.GrossWeight.ToString(), txtGrossWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.NetWeight.ToString(), txtNetWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_BoxRules>(entity, t => t.Is_enabled, chkIs_enabled, false);
           DataBindingHelper.BindData4DataTime<tb_BoxRules>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_BoxRules>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_BoxRules>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



