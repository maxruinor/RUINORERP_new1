
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:41
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
    /// 协作人记录表-记录内部人员介绍客户的情况数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CRM_CollaboratorEdit:UserControl
    {
     public tb_CRM_CollaboratorEdit() {
     
                         InitializeComponent();
      
        
        
        

         }
/*
        
        tb_CRM_Collaborator UIToEntity()
        {
        tb_CRM_Collaborator entity = new tb_CRM_Collaborator();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Customer_id = Int64.Parse(txtCustomer_id.Text);
                                return entity;
}
        */

        
        private tb_CRM_Collaborator _EditEntity;
        public tb_CRM_Collaborator EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRM_Collaborator entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v=>v.XXNAME, cmbCustomer_id);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



