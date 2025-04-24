
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/23/2025 23:27:32
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
    /// 收付款方式与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PayMethodAccountMapperEdit:UserControl
    {
     public tb_PayMethodAccountMapperEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_PayMethodAccountMapper UIToEntity()
        {
        tb_PayMethodAccountMapper entity = new tb_PayMethodAccountMapper();
                     entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.Description = txtDescription.Text ;
                       entity.EffectiveDate = DateTime.Parse(txtEffectiveDate.Text);
                        entity.ExpiryDate = DateTime.Parse(txtExpiryDate.Text);
                                return entity;
}
        */

        
        private tb_PayMethodAccountMapper _EditEntity;
        public tb_PayMethodAccountMapper EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PayMethodAccountMapper entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
           DataBindingHelper.BindData4TextBox<tb_PayMethodAccountMapper>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_PayMethodAccountMapper>(entity, t => t.EffectiveDate, dtpEffectiveDate,false);
           DataBindingHelper.BindData4DataTime<tb_PayMethodAccountMapper>(entity, t => t.ExpiryDate, dtpExpiryDate,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



