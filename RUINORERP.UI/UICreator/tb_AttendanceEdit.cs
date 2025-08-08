
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:10
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
    /// 考勤表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AttendanceEdit:UserControl
    {
     public tb_AttendanceEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Attendance UIToEntity()
        {
        tb_Attendance entity = new tb_Attendance();
                     entity.badgenumber = txtbadgenumber.Text ;
                       entity.username = txtusername.Text ;
                       entity.deptname = txtdeptname.Text ;
                       entity.sDate = txtsDate.Text ;
                       entity.stime = txtstime.Text ;
                       entity.eDate = DateTime.Parse(txteDate.Text);
                        entity.t1 = DateTime.Parse(txtt1.Text);
                        entity.t2 = DateTime.Parse(txtt2.Text);
                        entity.t3 = DateTime.Parse(txtt3.Text);
                        entity.t4 = DateTime.Parse(txtt4.Text);
                                return entity;
}
        */

        
        private tb_Attendance _EditEntity;
        public tb_Attendance EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Attendance entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Attendance>(entity, t => t.badgenumber, txtbadgenumber, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Attendance>(entity, t => t.username, txtusername, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Attendance>(entity, t => t.deptname, txtdeptname, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Attendance>(entity, t => t.sDate, txtsDate, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Attendance>(entity, t => t.stime, txtstime, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_Attendance>(entity, t => t.eDate, dtpeDate,false);
           DataBindingHelper.BindData4DataTime<tb_Attendance>(entity, t => t.t1, dtpt1,false);
           DataBindingHelper.BindData4DataTime<tb_Attendance>(entity, t => t.t2, dtpt2,false);
           DataBindingHelper.BindData4DataTime<tb_Attendance>(entity, t => t.t3, dtpt3,false);
           DataBindingHelper.BindData4DataTime<tb_Attendance>(entity, t => t.t4, dtpt4,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



