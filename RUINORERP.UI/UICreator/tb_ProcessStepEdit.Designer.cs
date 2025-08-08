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
    /// 流程步骤
    /// </summary>
    partial class tb_ProcessStepEdit
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
     this.lblStepBodyld = new Krypton.Toolkit.KryptonLabel();
this.cmbStepBodyld = new Krypton.Toolkit.KryptonComboBox();

this.lblPosition_Id = new Krypton.Toolkit.KryptonLabel();
this.cmbPosition_Id = new Krypton.Toolkit.KryptonComboBox();

this.lblNextNode_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbNextNode_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblVersion = new Krypton.Toolkit.KryptonLabel();
this.txtVersion = new Krypton.Toolkit.KryptonTextBox();

this.lblName = new Krypton.Toolkit.KryptonLabel();
this.txtName = new Krypton.Toolkit.KryptonTextBox();

this.lblDisplayName = new Krypton.Toolkit.KryptonLabel();
this.txtDisplayName = new Krypton.Toolkit.KryptonTextBox();

this.lblStepNodeType = new Krypton.Toolkit.KryptonLabel();
this.txtStepNodeType = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####StepBodyld###Int64
//属性测试25StepBodyld
//属性测试25StepBodyld
//属性测试25StepBodyld
this.lblStepBodyld.AutoSize = true;
this.lblStepBodyld.Location = new System.Drawing.Point(100,25);
this.lblStepBodyld.Name = "lblStepBodyld";
this.lblStepBodyld.Size = new System.Drawing.Size(41, 12);
this.lblStepBodyld.TabIndex = 1;
this.lblStepBodyld.Text = "流程定义";
//111======25
this.cmbStepBodyld.Location = new System.Drawing.Point(173,21);
this.cmbStepBodyld.Name ="cmbStepBodyld";
this.cmbStepBodyld.Size = new System.Drawing.Size(100, 21);
this.cmbStepBodyld.TabIndex = 1;
this.Controls.Add(this.lblStepBodyld);
this.Controls.Add(this.cmbStepBodyld);

           //#####Position_Id###Int64
//属性测试50Position_Id
//属性测试50Position_Id
this.lblPosition_Id.AutoSize = true;
this.lblPosition_Id.Location = new System.Drawing.Point(100,50);
this.lblPosition_Id.Name = "lblPosition_Id";
this.lblPosition_Id.Size = new System.Drawing.Size(41, 12);
this.lblPosition_Id.TabIndex = 2;
this.lblPosition_Id.Text = "位置信息";
//111======50
this.cmbPosition_Id.Location = new System.Drawing.Point(173,46);
this.cmbPosition_Id.Name ="cmbPosition_Id";
this.cmbPosition_Id.Size = new System.Drawing.Size(100, 21);
this.cmbPosition_Id.TabIndex = 2;
this.Controls.Add(this.lblPosition_Id);
this.Controls.Add(this.cmbPosition_Id);

           //#####NextNode_ID###Int64
//属性测试75NextNode_ID
this.lblNextNode_ID.AutoSize = true;
this.lblNextNode_ID.Location = new System.Drawing.Point(100,75);
this.lblNextNode_ID.Name = "lblNextNode_ID";
this.lblNextNode_ID.Size = new System.Drawing.Size(41, 12);
this.lblNextNode_ID.TabIndex = 3;
this.lblNextNode_ID.Text = "";
//111======75
this.cmbNextNode_ID.Location = new System.Drawing.Point(173,71);
this.cmbNextNode_ID.Name ="cmbNextNode_ID";
this.cmbNextNode_ID.Size = new System.Drawing.Size(100, 21);
this.cmbNextNode_ID.TabIndex = 3;
this.Controls.Add(this.lblNextNode_ID);
this.Controls.Add(this.cmbNextNode_ID);

           //#####50Version###String
this.lblVersion.AutoSize = true;
this.lblVersion.Location = new System.Drawing.Point(100,100);
this.lblVersion.Name = "lblVersion";
this.lblVersion.Size = new System.Drawing.Size(41, 12);
this.lblVersion.TabIndex = 4;
this.lblVersion.Text = "版本";
this.txtVersion.Location = new System.Drawing.Point(173,96);
this.txtVersion.Name = "txtVersion";
this.txtVersion.Size = new System.Drawing.Size(100, 21);
this.txtVersion.TabIndex = 4;
this.Controls.Add(this.lblVersion);
this.Controls.Add(this.txtVersion);

           //#####50Name###String
this.lblName.AutoSize = true;
this.lblName.Location = new System.Drawing.Point(100,125);
this.lblName.Name = "lblName";
this.lblName.Size = new System.Drawing.Size(41, 12);
this.lblName.TabIndex = 5;
this.lblName.Text = "标题";
this.txtName.Location = new System.Drawing.Point(173,121);
this.txtName.Name = "txtName";
this.txtName.Size = new System.Drawing.Size(100, 21);
this.txtName.TabIndex = 5;
this.Controls.Add(this.lblName);
this.Controls.Add(this.txtName);

           //#####50DisplayName###String
this.lblDisplayName.AutoSize = true;
this.lblDisplayName.Location = new System.Drawing.Point(100,150);
this.lblDisplayName.Name = "lblDisplayName";
this.lblDisplayName.Size = new System.Drawing.Size(41, 12);
this.lblDisplayName.TabIndex = 6;
this.lblDisplayName.Text = "显示名称";
this.txtDisplayName.Location = new System.Drawing.Point(173,146);
this.txtDisplayName.Name = "txtDisplayName";
this.txtDisplayName.Size = new System.Drawing.Size(100, 21);
this.txtDisplayName.TabIndex = 6;
this.Controls.Add(this.lblDisplayName);
this.Controls.Add(this.txtDisplayName);

           //#####50StepNodeType###String
this.lblStepNodeType.AutoSize = true;
this.lblStepNodeType.Location = new System.Drawing.Point(100,175);
this.lblStepNodeType.Name = "lblStepNodeType";
this.lblStepNodeType.Size = new System.Drawing.Size(41, 12);
this.lblStepNodeType.TabIndex = 7;
this.lblStepNodeType.Text = "节点类型";
this.txtStepNodeType.Location = new System.Drawing.Point(173,171);
this.txtStepNodeType.Name = "txtStepNodeType";
this.txtStepNodeType.Size = new System.Drawing.Size(100, 21);
this.txtStepNodeType.TabIndex = 7;
this.Controls.Add(this.lblStepNodeType);
this.Controls.Add(this.txtStepNodeType);

           //#####255Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,200);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 8;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,196);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 8;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
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
           // this.kryptonPanel1.TabIndex = 9;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblStepBodyld );
this.Controls.Add(this.cmbStepBodyld );

                this.Controls.Add(this.lblPosition_Id );
this.Controls.Add(this.cmbPosition_Id );

                this.Controls.Add(this.lblNextNode_ID );
this.Controls.Add(this.cmbNextNode_ID );

                this.Controls.Add(this.lblVersion );
this.Controls.Add(this.txtVersion );

                this.Controls.Add(this.lblName );
this.Controls.Add(this.txtName );

                this.Controls.Add(this.lblDisplayName );
this.Controls.Add(this.txtDisplayName );

                this.Controls.Add(this.lblStepNodeType );
this.Controls.Add(this.txtStepNodeType );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_ProcessStepEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProcessStepEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblStepBodyld;
private Krypton.Toolkit.KryptonComboBox cmbStepBodyld;

    
        
              private Krypton.Toolkit.KryptonLabel lblPosition_Id;
private Krypton.Toolkit.KryptonComboBox cmbPosition_Id;

    
        
              private Krypton.Toolkit.KryptonLabel lblNextNode_ID;
private Krypton.Toolkit.KryptonComboBox cmbNextNode_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblVersion;
private Krypton.Toolkit.KryptonTextBox txtVersion;

    
        
              private Krypton.Toolkit.KryptonLabel lblName;
private Krypton.Toolkit.KryptonTextBox txtName;

    
        
              private Krypton.Toolkit.KryptonLabel lblDisplayName;
private Krypton.Toolkit.KryptonTextBox txtDisplayName;

    
        
              private Krypton.Toolkit.KryptonLabel lblStepNodeType;
private Krypton.Toolkit.KryptonTextBox txtStepNodeType;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

