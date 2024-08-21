
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 14:54:18
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
    /// 卡通箱规格表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CartoonBoxEdit:UserControl
    {
     public tb_CartoonBoxEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CartoonBox UIToEntity()
        {
        tb_CartoonBox entity = new tb_CartoonBox();
                     entity.CartonName = txtCartonName.Text ;
                       entity.Color = txtColor.Text ;
                       entity.Material = txtMaterial.Text ;
                       entity.EmptyBoxWeight = Decimal.Parse(txtEmptyBoxWeight.Text);
                        entity.MaxLoad = Decimal.Parse(txtMaxLoad.Text);
                        entity.Thickness = Decimal.Parse(txtThickness.Text);
                        entity.Length = Decimal.Parse(txtLength.Text);
                        entity.Width = Decimal.Parse(txtWidth.Text);
                        entity.Height = Decimal.Parse(txtHeight.Text);
                        entity.Volume = Decimal.Parse(txtVolume.Text);
                        entity.FluteType = txtFluteType.Text ;
                       entity.PrintType = txtPrintType.Text ;
                       entity.CustomPrint = txtCustomPrint.Text ;
                       entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Description = txtDescription.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_CartoonBox _EditEntity;
        public tb_CartoonBox EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CartoonBox entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.CartonName, txtCartonName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Color, txtColor, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Material, txtMaterial, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.EmptyBoxWeight.ToString(), txtEmptyBoxWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.MaxLoad.ToString(), txtMaxLoad, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Thickness.ToString(), txtThickness, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Length.ToString(), txtLength, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Width.ToString(), txtWidth, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Height.ToString(), txtHeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Volume.ToString(), txtVolume, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.FluteType, txtFluteType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.PrintType, txtPrintType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.CustomPrint, txtCustomPrint, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_CartoonBox>(entity, t => t.Is_enabled, chkIs_enabled, false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_CartoonBox>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_CartoonBox>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CartoonBox>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CartoonBox>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



