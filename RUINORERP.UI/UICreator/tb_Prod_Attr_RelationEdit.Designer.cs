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
    partial class tb_Prod_Attr_RelationEdit
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblPropertyValueID = new Krypton.Toolkit.KryptonLabel();
this.cmbPropertyValueID = new Krypton.Toolkit.KryptonComboBox();

this.lblProperty_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProperty_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdBaseID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdBaseID = new Krypton.Toolkit.KryptonComboBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    
    //for end
   // ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
   // this.kryptonPanel1.SuspendLayout();
    this.SuspendLayout();
    
            // 
            // btnOk
            // 
            //this.btnOk.Location = new System.Drawing.Point(126, 355);
            //this.btnOk.Name = "btnOk";
            //this.btnOk.Size = new System.Drawing.Size(90, 25);
            //this.btnOk.TabIndex = 0;
           // this.btnOk.Values.Text = "确定";
            //this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
           // this.btnCancel.Location = new System.Drawing.Point(244, 355);
            //this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(90, 25);
            //this.btnCancel.TabIndex = 1;
            //this.btnCancel.Values.Text = "取消";
           // this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
         //for size
     
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

        //for 加入到容器
            //components = new System.ComponentModel.Container();
           
            //this.Controls.Add(this.btnCancel);
            //this.Controls.Add(this.btnOk);
            // 
            // kryptonPanel1
            // 
          //  this.kryptonPanel1.Controls.Add(this.btnCancel);
         //   this.kryptonPanel1.Controls.Add(this.btnOk);
           // this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
           // this.kryptonPanel1.Name = "kryptonPanel1";
           // this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
           // this.kryptonPanel1.TabIndex = 5;

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

                            // 
            // "tb_Prod_Attr_RelationEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_Prod_Attr_RelationEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPropertyValueID;
private Krypton.Toolkit.KryptonComboBox cmbPropertyValueID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProperty_ID;
private Krypton.Toolkit.KryptonComboBox cmbProperty_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdBaseID;
private Krypton.Toolkit.KryptonComboBox cmbProdBaseID;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

