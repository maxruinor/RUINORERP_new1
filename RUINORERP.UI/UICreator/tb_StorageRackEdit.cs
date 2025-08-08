
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:21
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
    /// 货架信息表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_StorageRackEdit:UserControl
    {
     public tb_StorageRackEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_StorageRack UIToEntity()
        {
        tb_StorageRack entity = new tb_StorageRack();
                     entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.RackNO = txtRackNO.Text ;
                       entity.RackName = txtRackName.Text ;
                       entity.RackLocation = txtRackLocation.Text ;
                       entity.Desc = txtDesc.Text ;
                               return entity;
}
        */

        
        private tb_StorageRack _EditEntity;
        public tb_StorageRack EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_StorageRack entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_StorageRack>(entity, t => t.RackNO, txtRackNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StorageRack>(entity, t => t.RackName, txtRackName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StorageRack>(entity, t => t.RackLocation, txtRackLocation, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StorageRack>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



