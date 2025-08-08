
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:10
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
    /// 质检表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_QualityInspectionEdit:UserControl
    {
     public tb_QualityInspectionEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_QualityInspection UIToEntity()
        {
        tb_QualityInspection entity = new tb_QualityInspection();
                     entity.InspectionDate = DateTime.Parse(txtInspectionDate.Text);
                        entity.InspectionResult = txtInspectionResult.Text ;
                       entity.ProductID = Int32.Parse(txtProductID.Text);
                                return entity;
}
        */

        
        private tb_QualityInspection _EditEntity;
        public tb_QualityInspection EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_QualityInspection entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4DataTime<tb_QualityInspection>(entity, t => t.InspectionDate, dtpInspectionDate,false);
           DataBindingHelper.BindData4TextBox<tb_QualityInspection>(entity, t => t.InspectionResult, txtInspectionResult, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_QualityInspection>(entity, t => t.ProductID, txtProductID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



