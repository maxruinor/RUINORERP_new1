// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 流程图子项
    /// </summary>
    partial class tb_FlowchartItemEdit
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
     this.lblIconFile_Path = new Krypton.Toolkit.KryptonLabel();
this.txtIconFile_Path = new Krypton.Toolkit.KryptonTextBox();
this.txtIconFile_Path.Multiline = true;

this.lblTitle = new Krypton.Toolkit.KryptonLabel();
this.txtTitle = new Krypton.Toolkit.KryptonTextBox();

this.lblSizeString = new Krypton.Toolkit.KryptonLabel();
this.txtSizeString = new Krypton.Toolkit.KryptonTextBox();

this.lblPointToString = new Krypton.Toolkit.KryptonLabel();
this.txtPointToString = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####500IconFile_Path###String
this.lblIconFile_Path.AutoSize = true;
this.lblIconFile_Path.Location = new System.Drawing.Point(100,25);
this.lblIconFile_Path.Name = "lblIconFile_Path";
this.lblIconFile_Path.Size = new System.Drawing.Size(41, 12);
this.lblIconFile_Path.TabIndex = 1;
this.lblIconFile_Path.Text = "";
this.txtIconFile_Path.Location = new System.Drawing.Point(173,21);
this.txtIconFile_Path.Name = "txtIconFile_Path";
this.txtIconFile_Path.Size = new System.Drawing.Size(100, 21);
this.txtIconFile_Path.TabIndex = 1;
this.Controls.Add(this.lblIconFile_Path);
this.Controls.Add(this.txtIconFile_Path);

           //#####100Title###String
this.lblTitle.AutoSize = true;
this.lblTitle.Location = new System.Drawing.Point(100,50);
this.lblTitle.Name = "lblTitle";
this.lblTitle.Size = new System.Drawing.Size(41, 12);
this.lblTitle.TabIndex = 2;
this.lblTitle.Text = "标题";
this.txtTitle.Location = new System.Drawing.Point(173,46);
this.txtTitle.Name = "txtTitle";
this.txtTitle.Size = new System.Drawing.Size(100, 21);
this.txtTitle.TabIndex = 2;
this.Controls.Add(this.lblTitle);
this.Controls.Add(this.txtTitle);

           //#####100SizeString###String
this.lblSizeString.AutoSize = true;
this.lblSizeString.Location = new System.Drawing.Point(100,75);
this.lblSizeString.Name = "lblSizeString";
this.lblSizeString.Size = new System.Drawing.Size(41, 12);
this.lblSizeString.TabIndex = 3;
this.lblSizeString.Text = "大小";
this.txtSizeString.Location = new System.Drawing.Point(173,71);
this.txtSizeString.Name = "txtSizeString";
this.txtSizeString.Size = new System.Drawing.Size(100, 21);
this.txtSizeString.TabIndex = 3;
this.Controls.Add(this.lblSizeString);
this.Controls.Add(this.txtSizeString);

           //#####100PointToString###String
this.lblPointToString.AutoSize = true;
this.lblPointToString.Location = new System.Drawing.Point(100,100);
this.lblPointToString.Name = "lblPointToString";
this.lblPointToString.Size = new System.Drawing.Size(41, 12);
this.lblPointToString.TabIndex = 4;
this.lblPointToString.Text = "位置";
this.txtPointToString.Location = new System.Drawing.Point(173,96);
this.txtPointToString.Name = "txtPointToString";
this.txtPointToString.Size = new System.Drawing.Size(100, 21);
this.txtPointToString.TabIndex = 4;
this.Controls.Add(this.lblPointToString);
this.Controls.Add(this.txtPointToString);

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
           // this.kryptonPanel1.TabIndex = 4;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblIconFile_Path );
this.Controls.Add(this.txtIconFile_Path );

                this.Controls.Add(this.lblTitle );
this.Controls.Add(this.txtTitle );

                this.Controls.Add(this.lblSizeString );
this.Controls.Add(this.txtSizeString );

                this.Controls.Add(this.lblPointToString );
this.Controls.Add(this.txtPointToString );

                            // 
            // "tb_FlowchartItemEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FlowchartItemEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblIconFile_Path;
private Krypton.Toolkit.KryptonTextBox txtIconFile_Path;

    
        
              private Krypton.Toolkit.KryptonLabel lblTitle;
private Krypton.Toolkit.KryptonTextBox txtTitle;

    
        
              private Krypton.Toolkit.KryptonLabel lblSizeString;
private Krypton.Toolkit.KryptonTextBox txtSizeString;

    
        
              private Krypton.Toolkit.KryptonLabel lblPointToString;
private Krypton.Toolkit.KryptonTextBox txtPointToString;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

