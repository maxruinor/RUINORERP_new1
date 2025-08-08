
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:52
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
    /// 产品主次及属性关系表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_Prod_Attr_RelationEdit:UserControl
    {
     public tb_Prod_Attr_RelationEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_Prod_Attr_Relation UIToEntity()
        {
        tb_Prod_Attr_Relation entity = new tb_Prod_Attr_Relation();
                     entity.PropertyValueID = Int64.Parse(txtPropertyValueID.Text);
                        entity.Property_ID = Int64.Parse(txtProperty_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_Prod_Attr_Relation _EditEntity;
        public tb_Prod_Attr_Relation EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Prod_Attr_Relation entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProdPropertyValue>(entity, k => k.PropertyValueID, v=>v.XXNAME, cmbPropertyValueID);
          // DataBindingHelper.BindData4Cmb<tb_ProdProperty>(entity, k => k.Property_ID, v=>v.XXNAME, cmbProperty_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Prod>(entity, k => k.ProdBaseID, v=>v.XXNAME, cmbProdBaseID);
           DataBindingHelper.BindData4CheckBox<tb_Prod_Attr_Relation>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



