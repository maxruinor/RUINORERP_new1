// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:34
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 
    /// </summary>
    partial class View_ProdPropertyEdit
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
     this.lblProdBaseID = new Krypton.Toolkit.KryptonLabel();
this.txtProdBaseID = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblProperty_ID = new Krypton.Toolkit.KryptonLabel();
this.txtProperty_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPropertyName = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyName = new Krypton.Toolkit.KryptonTextBox();

this.lblPropertyValueID = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyValueID = new Krypton.Toolkit.KryptonTextBox();

this.lblPropertyValueName = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyValueName = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####ProdBaseID###Int64
this.lblProdBaseID.AutoSize = true;
this.lblProdBaseID.Location = new System.Drawing.Point(100,25);
this.lblProdBaseID.Name = "lblProdBaseID";
this.lblProdBaseID.Size = new System.Drawing.Size(41, 12);
this.lblProdBaseID.TabIndex = 1;
this.lblProdBaseID.Text = "";
this.txtProdBaseID.Location = new System.Drawing.Point(173,21);
this.txtProdBaseID.Name = "txtProdBaseID";
this.txtProdBaseID.Size = new System.Drawing.Size(100, 21);
this.txtProdBaseID.TabIndex = 1;
this.Controls.Add(this.lblProdBaseID);
this.Controls.Add(this.txtProdBaseID);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,46);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####Property_ID###Int64
this.lblProperty_ID.AutoSize = true;
this.lblProperty_ID.Location = new System.Drawing.Point(100,75);
this.lblProperty_ID.Name = "lblProperty_ID";
this.lblProperty_ID.Size = new System.Drawing.Size(41, 12);
this.lblProperty_ID.TabIndex = 3;
this.lblProperty_ID.Text = "";
this.txtProperty_ID.Location = new System.Drawing.Point(173,71);
this.txtProperty_ID.Name = "txtProperty_ID";
this.txtProperty_ID.Size = new System.Drawing.Size(100, 21);
this.txtProperty_ID.TabIndex = 3;
this.Controls.Add(this.lblProperty_ID);
this.Controls.Add(this.txtProperty_ID);

           //#####20PropertyName###String
this.lblPropertyName.AutoSize = true;
this.lblPropertyName.Location = new System.Drawing.Point(100,100);
this.lblPropertyName.Name = "lblPropertyName";
this.lblPropertyName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyName.TabIndex = 4;
this.lblPropertyName.Text = "";
this.txtPropertyName.Location = new System.Drawing.Point(173,96);
this.txtPropertyName.Name = "txtPropertyName";
this.txtPropertyName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyName.TabIndex = 4;
this.Controls.Add(this.lblPropertyName);
this.Controls.Add(this.txtPropertyName);

           //#####PropertyValueID###Int64
this.lblPropertyValueID.AutoSize = true;
this.lblPropertyValueID.Location = new System.Drawing.Point(100,125);
this.lblPropertyValueID.Name = "lblPropertyValueID";
this.lblPropertyValueID.Size = new System.Drawing.Size(41, 12);
this.lblPropertyValueID.TabIndex = 5;
this.lblPropertyValueID.Text = "";
this.txtPropertyValueID.Location = new System.Drawing.Point(173,121);
this.txtPropertyValueID.Name = "txtPropertyValueID";
this.txtPropertyValueID.Size = new System.Drawing.Size(100, 21);
this.txtPropertyValueID.TabIndex = 5;
this.Controls.Add(this.lblPropertyValueID);
this.Controls.Add(this.txtPropertyValueID);

           //#####20PropertyValueName###String
this.lblPropertyValueName.AutoSize = true;
this.lblPropertyValueName.Location = new System.Drawing.Point(100,150);
this.lblPropertyValueName.Name = "lblPropertyValueName";
this.lblPropertyValueName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyValueName.TabIndex = 6;
this.lblPropertyValueName.Text = "";
this.txtPropertyValueName.Location = new System.Drawing.Point(173,146);
this.txtPropertyValueName.Name = "txtPropertyValueName";
this.txtPropertyValueName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyValueName.TabIndex = 6;
this.Controls.Add(this.lblPropertyValueName);
this.Controls.Add(this.txtPropertyValueName);

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
           // this.kryptonPanel1.TabIndex = 6;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdBaseID );
this.Controls.Add(this.txtProdBaseID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblProperty_ID );
this.Controls.Add(this.txtProperty_ID );

                this.Controls.Add(this.lblPropertyName );
this.Controls.Add(this.txtPropertyName );

                this.Controls.Add(this.lblPropertyValueID );
this.Controls.Add(this.txtPropertyValueID );

                this.Controls.Add(this.lblPropertyValueName );
this.Controls.Add(this.txtPropertyValueName );

                            // 
            // "View_ProdPropertyEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_ProdPropertyEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProdBaseID;
private Krypton.Toolkit.KryptonTextBox txtProdBaseID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProperty_ID;
private Krypton.Toolkit.KryptonTextBox txtProperty_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPropertyName;
private Krypton.Toolkit.KryptonTextBox txtPropertyName;

    
        
              private Krypton.Toolkit.KryptonLabel lblPropertyValueID;
private Krypton.Toolkit.KryptonTextBox txtPropertyValueID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPropertyValueName;
private Krypton.Toolkit.KryptonTextBox txtPropertyValueName;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

