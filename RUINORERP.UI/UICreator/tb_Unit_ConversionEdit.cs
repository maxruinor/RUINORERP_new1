
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:24
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
    /// 单位换算表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_Unit_ConversionEdit:UserControl
    {
     public tb_Unit_ConversionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_Unit_Conversion UIToEntity()
        {
        tb_Unit_Conversion entity = new tb_Unit_Conversion();
                     entity.UnitConversion_Name = txtUnitConversion_Name.Text ;
                       entity.Source_unit_id = Int64.Parse(txtSource_unit_id.Text);
                        entity.Target_unit_id = Int64.Parse(txtTarget_unit_id.Text);
                        entity.Conversion_ratio = Decimal.Parse(txtConversion_ratio.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_Unit_Conversion _EditEntity;
        public tb_Unit_Conversion EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Unit_Conversion entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Unit_Conversion>(entity, t => t.UnitConversion_Name, txtUnitConversion_Name, BindDataType4TextBox.Text,false);
          Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_Unit_Conversion>(entity, t => t.Source_unit_id, txtSource_unit_id, BindDataType4TextBox.Qty,false);
          Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_Unit_Conversion>(entity, t => t.Target_unit_id, txtTarget_unit_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Unit_Conversion>(entity, t => t.Conversion_ratio.ToString(), txtConversion_ratio, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Unit_Conversion>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



