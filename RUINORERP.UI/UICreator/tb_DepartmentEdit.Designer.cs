// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/07/2025 21:48:21
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 部门表是否分层
    /// </summary>
    partial class tb_DepartmentEdit
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
     this.lblID = new Krypton.Toolkit.KryptonLabel();
this.cmbID = new Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentCode = new Krypton.Toolkit.KryptonLabel();
this.txtDepartmentCode = new Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentName = new Krypton.Toolkit.KryptonLabel();
this.txtDepartmentName = new Krypton.Toolkit.KryptonTextBox();
this.txtDepartmentName.Multiline = true;

this.lblTEL = new Krypton.Toolkit.KryptonLabel();
this.txtTEL = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

this.lblDirector = new Krypton.Toolkit.KryptonLabel();
this.txtDirector = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####ID###Int64
//属性测试25ID
this.lblID.AutoSize = true;
this.lblID.Location = new System.Drawing.Point(100,25);
this.lblID.Name = "lblID";
this.lblID.Size = new System.Drawing.Size(41, 12);
this.lblID.TabIndex = 1;
this.lblID.Text = "公司";
//111======25
this.cmbID.Location = new System.Drawing.Point(173,21);
this.cmbID.Name ="cmbID";
this.cmbID.Size = new System.Drawing.Size(100, 21);
this.cmbID.TabIndex = 1;
this.Controls.Add(this.lblID);
this.Controls.Add(this.cmbID);

           //#####20DepartmentCode###String
this.lblDepartmentCode.AutoSize = true;
this.lblDepartmentCode.Location = new System.Drawing.Point(100,50);
this.lblDepartmentCode.Name = "lblDepartmentCode";
this.lblDepartmentCode.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentCode.TabIndex = 2;
this.lblDepartmentCode.Text = "部门代号";
this.txtDepartmentCode.Location = new System.Drawing.Point(173,46);
this.txtDepartmentCode.Name = "txtDepartmentCode";
this.txtDepartmentCode.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentCode.TabIndex = 2;
this.Controls.Add(this.lblDepartmentCode);
this.Controls.Add(this.txtDepartmentCode);

           //#####255DepartmentName###String
this.lblDepartmentName.AutoSize = true;
this.lblDepartmentName.Location = new System.Drawing.Point(100,75);
this.lblDepartmentName.Name = "lblDepartmentName";
this.lblDepartmentName.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentName.TabIndex = 3;
this.lblDepartmentName.Text = "部门名称";
this.txtDepartmentName.Location = new System.Drawing.Point(173,71);
this.txtDepartmentName.Name = "txtDepartmentName";
this.txtDepartmentName.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentName.TabIndex = 3;
this.Controls.Add(this.lblDepartmentName);
this.Controls.Add(this.txtDepartmentName);

           //#####20TEL###String
this.lblTEL.AutoSize = true;
this.lblTEL.Location = new System.Drawing.Point(100,100);
this.lblTEL.Name = "lblTEL";
this.lblTEL.Size = new System.Drawing.Size(41, 12);
this.lblTEL.TabIndex = 4;
this.lblTEL.Text = "电话";
this.txtTEL.Location = new System.Drawing.Point(173,96);
this.txtTEL.Name = "txtTEL";
this.txtTEL.Size = new System.Drawing.Size(100, 21);
this.txtTEL.TabIndex = 4;
this.Controls.Add(this.lblTEL);
this.Controls.Add(this.txtTEL);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,125);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 5;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,121);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 5;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####20Director###String
this.lblDirector.AutoSize = true;
this.lblDirector.Location = new System.Drawing.Point(100,150);
this.lblDirector.Name = "lblDirector";
this.lblDirector.Size = new System.Drawing.Size(41, 12);
this.lblDirector.TabIndex = 6;
this.lblDirector.Text = "责任人";
this.txtDirector.Location = new System.Drawing.Point(173,146);
this.txtDirector.Name = "txtDirector";
this.txtDirector.Size = new System.Drawing.Size(100, 21);
this.txtDirector.TabIndex = 6;
this.Controls.Add(this.lblDirector);
this.Controls.Add(this.txtDirector);

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
                this.Controls.Add(this.lblID );
this.Controls.Add(this.cmbID );

                this.Controls.Add(this.lblDepartmentCode );
this.Controls.Add(this.txtDepartmentCode );

                this.Controls.Add(this.lblDepartmentName );
this.Controls.Add(this.txtDepartmentName );

                this.Controls.Add(this.lblTEL );
this.Controls.Add(this.txtTEL );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblDirector );
this.Controls.Add(this.txtDirector );

                            // 
            // "tb_DepartmentEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_DepartmentEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblID;
private Krypton.Toolkit.KryptonComboBox cmbID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentCode;
private Krypton.Toolkit.KryptonTextBox txtDepartmentCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentName;
private Krypton.Toolkit.KryptonTextBox txtDepartmentName;

    
        
              private Krypton.Toolkit.KryptonLabel lblTEL;
private Krypton.Toolkit.KryptonTextBox txtTEL;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblDirector;
private Krypton.Toolkit.KryptonTextBox txtDirector;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

