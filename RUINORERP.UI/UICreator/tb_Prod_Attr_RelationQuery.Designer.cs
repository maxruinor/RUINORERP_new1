
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


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品主次及属性关系表
    /// </summary>
    partial class tb_Prod_Attr_RelationQuery
    {
    
    
            /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
                /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        
     //for start
     
     this.lblPropertyValueID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPropertyValueID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProperty_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProperty_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdBaseID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdBaseID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####PropertyValueID###Int64
//属性测试25PropertyValueID
//属性测试25PropertyValueID
//属性测试25PropertyValueID
this.lblPropertyValueID.AutoSize = true;
this.lblPropertyValueID.Location = new System.Drawing.Point(100,25);
this.lblPropertyValueID.Name = "lblPropertyValueID";
this.lblPropertyValueID.Size = new System.Drawing.Size(41, 12);
this.lblPropertyValueID.TabIndex = 1;
this.lblPropertyValueID.Text = "属性值";
//111======25
this.cmbPropertyValueID.Location = new System.Drawing.Point(173,21);
this.cmbPropertyValueID.Name ="cmbPropertyValueID";
this.cmbPropertyValueID.Size = new System.Drawing.Size(100, 21);
this.cmbPropertyValueID.TabIndex = 1;
this.Controls.Add(this.lblPropertyValueID);
this.Controls.Add(this.cmbPropertyValueID);

           //#####Property_ID###Int64
//属性测试50Property_ID
//属性测试50Property_ID
//属性测试50Property_ID
//属性测试50Property_ID
this.lblProperty_ID.AutoSize = true;
this.lblProperty_ID.Location = new System.Drawing.Point(100,50);
this.lblProperty_ID.Name = "lblProperty_ID";
this.lblProperty_ID.Size = new System.Drawing.Size(41, 12);
this.lblProperty_ID.TabIndex = 2;
this.lblProperty_ID.Text = "属性";
//111======50
this.cmbProperty_ID.Location = new System.Drawing.Point(173,46);
this.cmbProperty_ID.Name ="cmbProperty_ID";
this.cmbProperty_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProperty_ID.TabIndex = 2;
this.Controls.Add(this.lblProperty_ID);
this.Controls.Add(this.cmbProperty_ID);

           //#####ProdDetailID###Int64
//属性测试75ProdDetailID
//属性测试75ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,75);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 3;
this.lblProdDetailID.Text = "货品详情";
//111======75
this.cmbProdDetailID.Location = new System.Drawing.Point(173,71);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 3;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####ProdBaseID###Int64
//属性测试100ProdBaseID
this.lblProdBaseID.AutoSize = true;
this.lblProdBaseID.Location = new System.Drawing.Point(100,100);
this.lblProdBaseID.Name = "lblProdBaseID";
this.lblProdBaseID.Size = new System.Drawing.Size(41, 12);
this.lblProdBaseID.TabIndex = 4;
this.lblProdBaseID.Text = "货品";
//111======100
this.cmbProdBaseID.Location = new System.Drawing.Point(173,96);
this.cmbProdBaseID.Name ="cmbProdBaseID";
this.cmbProdBaseID.Size = new System.Drawing.Size(100, 21);
this.cmbProdBaseID.TabIndex = 4;
this.Controls.Add(this.lblProdBaseID);
this.Controls.Add(this.cmbProdBaseID);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,125);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 5;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,121);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 5;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPropertyValueID );
this.Controls.Add(this.cmbPropertyValueID );

                this.Controls.Add(this.lblProperty_ID );
this.Controls.Add(this.cmbProperty_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblProdBaseID );
this.Controls.Add(this.cmbProdBaseID );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_Prod_Attr_RelationQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPropertyValueID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPropertyValueID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProperty_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProperty_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdBaseID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdBaseID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


