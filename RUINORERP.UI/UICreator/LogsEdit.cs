
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/20/2024 20:29:59
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
    /// 数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class LogsEdit:UserControl
    {
     public LogsEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        Logs UIToEntity()
        {
        Logs entity = new Logs();
                     entity.Date = DateTime.Parse(txtDate.Text);
                        entity.Level = txtLevel.Text ;
                       entity.Logger = txtLogger.Text ;
                       entity.Message = txtMessage.Text ;
                       entity.Exception = txtException.Text ;
                       entity.Operator = txtOperator.Text ;
                       entity.ModName = txtModName.Text ;
                       entity.Path = txtPath.Text ;
                       entity.ActionName = txtActionName.Text ;
                       entity.IP = txtIP.Text ;
                       entity.MAC = txtMAC.Text ;
                       entity.MachineName = txtMachineName.Text ;
                       entity.User_ID = Int64.Parse(txtUser_ID.Text);
                                return entity;
}
        */

        
        private Logs _EditEntity;
        public Logs EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(Logs entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4DataTime<Logs>(entity, t => t.Date, dtpDate,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Level, txtLevel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Logger, txtLogger, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Message, txtMessage, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Exception, txtException, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Operator, txtOperator, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.ModName, txtModName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.Path, txtPath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.ActionName, txtActionName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.IP, txtIP, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.MAC, txtMAC, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<Logs>(entity, t => t.MachineName, txtMachineName, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_UserInfo>(entity, k => k.User_ID, v=>v.XXNAME, cmbUser_ID);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



