
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
    /// 基本单位数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_UnitEdit:UserControl
    {
     public tb_UnitEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_Unit UIToEntity()
        {
        tb_Unit entity = new tb_Unit();
                     entity.UnitName = txtUnitName.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.is_measurement_unit = Boolean.Parse(txtis_measurement_unit.Text);
                                return entity;
}
        */

        
        private tb_Unit _EditEntity;
        public tb_Unit EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Unit entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Unit>(entity, t => t.UnitName, txtUnitName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Unit>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_Unit>(entity, t => t.is_measurement_unit, chkis_measurement_unit, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



