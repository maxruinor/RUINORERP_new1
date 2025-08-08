
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:03
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
    /// 项目组与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProjectGroupAccountMapperEdit:UserControl
    {
     public tb_ProjectGroupAccountMapperEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_ProjectGroupAccountMapper UIToEntity()
        {
        tb_ProjectGroupAccountMapper entity = new tb_ProjectGroupAccountMapper();
                     entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.Description = txtDescription.Text ;
                       entity.EffectiveDate = DateTime.Parse(txtEffectiveDate.Text);
                        entity.ExpiryDate = DateTime.Parse(txtExpiryDate.Text);
                                return entity;
}
        */

        
        private tb_ProjectGroupAccountMapper _EditEntity;
        public tb_ProjectGroupAccountMapper EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProjectGroupAccountMapper entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
           DataBindingHelper.BindData4TextBox<tb_ProjectGroupAccountMapper>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ProjectGroupAccountMapper>(entity, t => t.EffectiveDate, dtpEffectiveDate,false);
           DataBindingHelper.BindData4DataTime<tb_ProjectGroupAccountMapper>(entity, t => t.ExpiryDate, dtpExpiryDate,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



