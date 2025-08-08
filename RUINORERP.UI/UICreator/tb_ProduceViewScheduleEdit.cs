
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:00
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
    /// 可视化排程数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProduceViewScheduleEdit:UserControl
    {
     public tb_ProduceViewScheduleEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        

         }
/*
        
        tb_ProduceViewSchedule UIToEntity()
        {
        tb_ProduceViewSchedule entity = new tb_ProduceViewSchedule();
                     entity.product_id = Int32.Parse(txtproduct_id.Text);
                        entity.quantity = Int32.Parse(txtquantity.Text);
                        entity.start_date = DateTime.Parse(txtstart_date.Text);
                        entity.end_date = DateTime.Parse(txtend_date.Text);
                                return entity;
}
        */

        
        private tb_ProduceViewSchedule _EditEntity;
        public tb_ProduceViewSchedule EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProduceViewSchedule entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ProduceViewSchedule>(entity, t => t.product_id, txtproduct_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProduceViewSchedule>(entity, t => t.quantity, txtquantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProduceViewSchedule>(entity, t => t.start_date, dtpstart_date,false);
           DataBindingHelper.BindData4DataTime<tb_ProduceViewSchedule>(entity, t => t.end_date, dtpend_date,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



