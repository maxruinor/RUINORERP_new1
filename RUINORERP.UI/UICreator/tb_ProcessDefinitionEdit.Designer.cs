// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:51
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 流程定义 http://www.phpheidong.com/blog/article/68471/a3129f742e5e396e3d1e/
    /// </summary>
    partial class tb_ProcessDefinitionEdit
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
     this.lblStep_Id = new Krypton.Toolkit.KryptonLabel();
this.cmbStep_Id = new Krypton.Toolkit.KryptonComboBox();

this.lblVersion = new Krypton.Toolkit.KryptonLabel();
this.txtVersion = new Krypton.Toolkit.KryptonTextBox();

this.lblTitle = new Krypton.Toolkit.KryptonLabel();
this.txtTitle = new Krypton.Toolkit.KryptonTextBox();

this.lblColor = new Krypton.Toolkit.KryptonLabel();
this.txtColor = new Krypton.Toolkit.KryptonTextBox();

this.lblIcon = new Krypton.Toolkit.KryptonLabel();
this.txtIcon = new Krypton.Toolkit.KryptonTextBox();
this.txtIcon.Multiline = true;

this.lblDescription = new Krypton.Toolkit.KryptonLabel();
this.txtDescription = new Krypton.Toolkit.KryptonTextBox();
this.txtDescription.Multiline = true;

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    
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
     
            //#####Step_Id###Int64
//属性测试25Step_Id
this.lblStep_Id.AutoSize = true;
this.lblStep_Id.Location = new System.Drawing.Point(100,25);
this.lblStep_Id.Name = "lblStep_Id";
this.lblStep_Id.Size = new System.Drawing.Size(41, 12);
this.lblStep_Id.TabIndex = 1;
this.lblStep_Id.Text = "流程定义";
//111======25
this.cmbStep_Id.Location = new System.Drawing.Point(173,21);
this.cmbStep_Id.Name ="cmbStep_Id";
this.cmbStep_Id.Size = new System.Drawing.Size(100, 21);
this.cmbStep_Id.TabIndex = 1;
this.Controls.Add(this.lblStep_Id);
this.Controls.Add(this.cmbStep_Id);

           //#####50Version###String
this.lblVersion.AutoSize = true;
this.lblVersion.Location = new System.Drawing.Point(100,50);
this.lblVersion.Name = "lblVersion";
this.lblVersion.Size = new System.Drawing.Size(41, 12);
this.lblVersion.TabIndex = 2;
this.lblVersion.Text = "版本";
this.txtVersion.Location = new System.Drawing.Point(173,46);
this.txtVersion.Name = "txtVersion";
this.txtVersion.Size = new System.Drawing.Size(100, 21);
this.txtVersion.TabIndex = 2;
this.Controls.Add(this.lblVersion);
this.Controls.Add(this.txtVersion);

           //#####50Title###String
this.lblTitle.AutoSize = true;
this.lblTitle.Location = new System.Drawing.Point(100,75);
this.lblTitle.Name = "lblTitle";
this.lblTitle.Size = new System.Drawing.Size(41, 12);
this.lblTitle.TabIndex = 3;
this.lblTitle.Text = "标题";
this.txtTitle.Location = new System.Drawing.Point(173,71);
this.txtTitle.Name = "txtTitle";
this.txtTitle.Size = new System.Drawing.Size(100, 21);
this.txtTitle.TabIndex = 3;
this.Controls.Add(this.lblTitle);
this.Controls.Add(this.txtTitle);

           //#####50Color###String
this.lblColor.AutoSize = true;
this.lblColor.Location = new System.Drawing.Point(100,100);
this.lblColor.Name = "lblColor";
this.lblColor.Size = new System.Drawing.Size(41, 12);
this.lblColor.TabIndex = 4;
this.lblColor.Text = "颜色";
this.txtColor.Location = new System.Drawing.Point(173,96);
this.txtColor.Name = "txtColor";
this.txtColor.Size = new System.Drawing.Size(100, 21);
this.txtColor.TabIndex = 4;
this.Controls.Add(this.lblColor);
this.Controls.Add(this.txtColor);

           //#####250Icon###String
this.lblIcon.AutoSize = true;
this.lblIcon.Location = new System.Drawing.Point(100,125);
this.lblIcon.Name = "lblIcon";
this.lblIcon.Size = new System.Drawing.Size(41, 12);
this.lblIcon.TabIndex = 5;
this.lblIcon.Text = "图标";
this.txtIcon.Location = new System.Drawing.Point(173,121);
this.txtIcon.Name = "txtIcon";
this.txtIcon.Size = new System.Drawing.Size(100, 21);
this.txtIcon.TabIndex = 5;
this.Controls.Add(this.lblIcon);
this.Controls.Add(this.txtIcon);

           //#####255Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,150);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 6;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,146);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 6;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,175);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 7;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,171);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 7;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

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
           // this.kryptonPanel1.TabIndex = 7;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblStep_Id );
this.Controls.Add(this.cmbStep_Id );

                this.Controls.Add(this.lblVersion );
this.Controls.Add(this.txtVersion );

                this.Controls.Add(this.lblTitle );
this.Controls.Add(this.txtTitle );

                this.Controls.Add(this.lblColor );
this.Controls.Add(this.txtColor );

                this.Controls.Add(this.lblIcon );
this.Controls.Add(this.txtIcon );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_ProcessDefinitionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProcessDefinitionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblStep_Id;
private Krypton.Toolkit.KryptonComboBox cmbStep_Id;

    
        
              private Krypton.Toolkit.KryptonLabel lblVersion;
private Krypton.Toolkit.KryptonTextBox txtVersion;

    
        
              private Krypton.Toolkit.KryptonLabel lblTitle;
private Krypton.Toolkit.KryptonTextBox txtTitle;

    
        
              private Krypton.Toolkit.KryptonLabel lblColor;
private Krypton.Toolkit.KryptonTextBox txtColor;

    
        
              private Krypton.Toolkit.KryptonLabel lblIcon;
private Krypton.Toolkit.KryptonTextBox txtIcon;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

