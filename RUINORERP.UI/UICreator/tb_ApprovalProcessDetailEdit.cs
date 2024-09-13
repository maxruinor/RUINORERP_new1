
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:32
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
    /// 审核流程明细表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ApprovalProcessDetailEdit:UserControl
    {
     public tb_ApprovalProcessDetailEdit() {
     
             
                    InitializeComponent();
      
        
        
        

         }
/*
        
        tb_ApprovalProcessDetail UIToEntity()
        {
        tb_ApprovalProcessDetail entity = new tb_ApprovalProcessDetail();
                     entity.ApprovalID = Int64.Parse(txtApprovalID.Text);
                        entity.ApprovalResults = Int32.Parse(txtApprovalResults.Text);
                        entity.ApprovalOrder = Int32.Parse(txtApprovalOrder.Text);
                                return entity;
}
        */

        
        private tb_ApprovalProcessDetail _EditEntity;
        public tb_ApprovalProcessDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ApprovalProcessDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Approval>(entity, k => k.ApprovalID, v=>v.XXNAME, cmbApprovalID);
           DataBindingHelper.BindData4TextBox<tb_ApprovalProcessDetail>(entity, t => t.ApprovalResults, txtApprovalResults, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ApprovalProcessDetail>(entity, t => t.ApprovalOrder, txtApprovalOrder, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



