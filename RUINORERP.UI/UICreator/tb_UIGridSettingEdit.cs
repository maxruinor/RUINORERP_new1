
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2025 15:31:54
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
    /// UI表格设置数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_UIGridSettingEdit:UserControl
    {
     public tb_UIGridSettingEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_UIGridSetting UIToEntity()
        {
        tb_UIGridSetting entity = new tb_UIGridSetting();
                     entity.UIMenuPID = Int64.Parse(txtUIMenuPID.Text);
                        entity.GridKeyName = txtGridKeyName.Text ;
                       entity.ColsSetting = txtColsSetting.Text ;
                               return entity;
}
        */

        
        private tb_UIGridSetting _EditEntity;
        public tb_UIGridSetting EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_UIGridSetting entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_UIMenuPersonalization>(entity, k => k.UIMenuPID, v=>v.XXNAME, cmbUIMenuPID);
           DataBindingHelper.BindData4TextBox<tb_UIGridSetting>(entity, t => t.GridKeyName, txtGridKeyName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_UIGridSetting>(entity, t => t.ColsSetting, txtColsSetting, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



