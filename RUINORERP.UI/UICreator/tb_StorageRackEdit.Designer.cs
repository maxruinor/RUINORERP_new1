// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:22
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 货架信息表
    /// </summary>
    partial class tb_StorageRackEdit
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
     this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblRackNO = new Krypton.Toolkit.KryptonLabel();
this.txtRackNO = new Krypton.Toolkit.KryptonTextBox();

this.lblRackName = new Krypton.Toolkit.KryptonLabel();
this.txtRackName = new Krypton.Toolkit.KryptonTextBox();

this.lblRackLocation = new Krypton.Toolkit.KryptonLabel();
this.txtRackLocation = new Krypton.Toolkit.KryptonTextBox();

this.lblDesc = new Krypton.Toolkit.KryptonLabel();
this.txtDesc = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####Location_ID###Int64
//属性测试25Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,25);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 1;
this.lblLocation_ID.Text = "所属仓库";
//111======25
this.cmbLocation_ID.Location = new System.Drawing.Point(173,21);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 1;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####50RackNO###String
this.lblRackNO.AutoSize = true;
this.lblRackNO.Location = new System.Drawing.Point(100,50);
this.lblRackNO.Name = "lblRackNO";
this.lblRackNO.Size = new System.Drawing.Size(41, 12);
this.lblRackNO.TabIndex = 2;
this.lblRackNO.Text = "货架编号";
this.txtRackNO.Location = new System.Drawing.Point(173,46);
this.txtRackNO.Name = "txtRackNO";
this.txtRackNO.Size = new System.Drawing.Size(100, 21);
this.txtRackNO.TabIndex = 2;
this.Controls.Add(this.lblRackNO);
this.Controls.Add(this.txtRackNO);

           //#####50RackName###String
this.lblRackName.AutoSize = true;
this.lblRackName.Location = new System.Drawing.Point(100,75);
this.lblRackName.Name = "lblRackName";
this.lblRackName.Size = new System.Drawing.Size(41, 12);
this.lblRackName.TabIndex = 3;
this.lblRackName.Text = "货架名称";
this.txtRackName.Location = new System.Drawing.Point(173,71);
this.txtRackName.Name = "txtRackName";
this.txtRackName.Size = new System.Drawing.Size(100, 21);
this.txtRackName.TabIndex = 3;
this.Controls.Add(this.lblRackName);
this.Controls.Add(this.txtRackName);

           //#####100RackLocation###String
this.lblRackLocation.AutoSize = true;
this.lblRackLocation.Location = new System.Drawing.Point(100,100);
this.lblRackLocation.Name = "lblRackLocation";
this.lblRackLocation.Size = new System.Drawing.Size(41, 12);
this.lblRackLocation.TabIndex = 4;
this.lblRackLocation.Text = "货架位置";
this.txtRackLocation.Location = new System.Drawing.Point(173,96);
this.txtRackLocation.Name = "txtRackLocation";
this.txtRackLocation.Size = new System.Drawing.Size(100, 21);
this.txtRackLocation.TabIndex = 4;
this.Controls.Add(this.lblRackLocation);
this.Controls.Add(this.txtRackLocation);

           //#####100Desc###String
this.lblDesc.AutoSize = true;
this.lblDesc.Location = new System.Drawing.Point(100,125);
this.lblDesc.Name = "lblDesc";
this.lblDesc.Size = new System.Drawing.Size(41, 12);
this.lblDesc.TabIndex = 5;
this.lblDesc.Text = "描述";
this.txtDesc.Location = new System.Drawing.Point(173,121);
this.txtDesc.Name = "txtDesc";
this.txtDesc.Size = new System.Drawing.Size(100, 21);
this.txtDesc.TabIndex = 5;
this.Controls.Add(this.lblDesc);
this.Controls.Add(this.txtDesc);

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
                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblRackNO );
this.Controls.Add(this.txtRackNO );

                this.Controls.Add(this.lblRackName );
this.Controls.Add(this.txtRackName );

                this.Controls.Add(this.lblRackLocation );
this.Controls.Add(this.txtRackLocation );

                this.Controls.Add(this.lblDesc );
this.Controls.Add(this.txtDesc );

                            // 
            // "tb_StorageRackEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_StorageRackEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRackNO;
private Krypton.Toolkit.KryptonTextBox txtRackNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblRackName;
private Krypton.Toolkit.KryptonTextBox txtRackName;

    
        
              private Krypton.Toolkit.KryptonLabel lblRackLocation;
private Krypton.Toolkit.KryptonTextBox txtRackLocation;

    
        
              private Krypton.Toolkit.KryptonLabel lblDesc;
private Krypton.Toolkit.KryptonTextBox txtDesc;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

