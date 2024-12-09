
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:46
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
    /// 省份表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProvincesEdit:UserControl
    {
     public tb_ProvincesEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_Provinces UIToEntity()
        {
        tb_Provinces entity = new tb_Provinces();
                     entity.ProvinceCNName = txtProvinceCNName.Text ;
                       entity.CountryID = Int64.Parse(txtCountryID.Text);
                        entity.ProvinceENName = txtProvinceENName.Text ;
                               return entity;
}
        */

        
        private tb_Provinces _EditEntity;
        public tb_Provinces EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Provinces entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Provinces>(entity, t => t.ProvinceCNName, txtProvinceCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Provinces>(entity, t => t.CountryID, txtCountryID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Provinces>(entity, t => t.ProvinceENName, txtProvinceENName, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



