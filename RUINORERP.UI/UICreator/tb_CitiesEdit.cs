
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:16
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
    /// 城市表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CitiesEdit:UserControl
    {
     public tb_CitiesEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_Cities UIToEntity()
        {
        tb_Cities entity = new tb_Cities();
                     entity.ProvinceID = Int64.Parse(txtProvinceID.Text);
                        entity.CityCNName = txtCityCNName.Text ;
                       entity.CityENName = txtCityENName.Text ;
                               return entity;
}
        */

        
        private tb_Cities _EditEntity;
        public tb_Cities EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Cities entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Provinces>(entity, k => k.ProvinceID, v=>v.XXNAME, cmbProvinceID);
           DataBindingHelper.BindData4TextBox<tb_Cities>(entity, t => t.CityCNName, txtCityCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Cities>(entity, t => t.CityENName, txtCityENName, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



