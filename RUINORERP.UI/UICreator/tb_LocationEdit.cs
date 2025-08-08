
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
    /// 库位表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_LocationEdit:UserControl
    {
     public tb_LocationEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Location UIToEntity()
        {
        tb_Location entity = new tb_Location();
                     entity.LocationType_ID = Int64.Parse(txtLocationType_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.LocationCode = txtLocationCode.Text ;
                       entity.Tel = txtTel.Text ;
                       entity.Name = txtName.Text ;
                       entity.Desc = txtDesc.Text ;
                       entity.Sort = Int32.Parse(txtSort.Text);
                                return entity;
}
        */

        
        private tb_Location _EditEntity;
        public tb_Location EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Location entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_LocationType>(entity, k => k.LocationType_ID, v=>v.XXNAME, cmbLocationType_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4CheckBox<tb_Location>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.LocationCode, txtLocationCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Tel, txtTel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Name, txtName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



