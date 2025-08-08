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
    /// 财务审计日志
    /// </summary>
    partial class tb_FM_AuditLogsEdit
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
     this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblUserName = new Krypton.Toolkit.KryptonLabel();
this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
this.txtUserName.Multiline = true;

this.lblActionTime = new Krypton.Toolkit.KryptonLabel();
this.dtpActionTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblActionType = new Krypton.Toolkit.KryptonLabel();
this.txtActionType = new Krypton.Toolkit.KryptonTextBox();

this.lblObjectType = new Krypton.Toolkit.KryptonLabel();
this.txtObjectType = new Krypton.Toolkit.KryptonTextBox();

this.lblObjectId = new Krypton.Toolkit.KryptonLabel();
this.txtObjectId = new Krypton.Toolkit.KryptonTextBox();

this.lblObjectNo = new Krypton.Toolkit.KryptonLabel();
this.txtObjectNo = new Krypton.Toolkit.KryptonTextBox();

this.lblOldState = new Krypton.Toolkit.KryptonLabel();
this.txtOldState = new Krypton.Toolkit.KryptonTextBox();

this.lblNewState = new Krypton.Toolkit.KryptonLabel();
this.txtNewState = new Krypton.Toolkit.KryptonTextBox();

this.lblDataContent = new Krypton.Toolkit.KryptonLabel();
this.txtDataContent = new Krypton.Toolkit.KryptonTextBox();
this.txtDataContent.Multiline = true;

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
     
            //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,25);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 1;
this.lblEmployee_ID.Text = "员工信息";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,21);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 1;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####255UserName###String
this.lblUserName.AutoSize = true;
this.lblUserName.Location = new System.Drawing.Point(100,50);
this.lblUserName.Name = "lblUserName";
this.lblUserName.Size = new System.Drawing.Size(41, 12);
this.lblUserName.TabIndex = 2;
this.lblUserName.Text = "用户名";
this.txtUserName.Location = new System.Drawing.Point(173,46);
this.txtUserName.Name = "txtUserName";
this.txtUserName.Size = new System.Drawing.Size(100, 21);
this.txtUserName.TabIndex = 2;
this.Controls.Add(this.lblUserName);
this.Controls.Add(this.txtUserName);

           //#####ActionTime###DateTime
this.lblActionTime.AutoSize = true;
this.lblActionTime.Location = new System.Drawing.Point(100,75);
this.lblActionTime.Name = "lblActionTime";
this.lblActionTime.Size = new System.Drawing.Size(41, 12);
this.lblActionTime.TabIndex = 3;
this.lblActionTime.Text = "发生时间";
//111======75
this.dtpActionTime.Location = new System.Drawing.Point(173,71);
this.dtpActionTime.Name ="dtpActionTime";
this.dtpActionTime.ShowCheckBox =true;
this.dtpActionTime.Size = new System.Drawing.Size(100, 21);
this.dtpActionTime.TabIndex = 3;
this.Controls.Add(this.lblActionTime);
this.Controls.Add(this.dtpActionTime);

           //#####50ActionType###String
this.lblActionType.AutoSize = true;
this.lblActionType.Location = new System.Drawing.Point(100,100);
this.lblActionType.Name = "lblActionType";
this.lblActionType.Size = new System.Drawing.Size(41, 12);
this.lblActionType.TabIndex = 4;
this.lblActionType.Text = "动作";
this.txtActionType.Location = new System.Drawing.Point(173,96);
this.txtActionType.Name = "txtActionType";
this.txtActionType.Size = new System.Drawing.Size(100, 21);
this.txtActionType.TabIndex = 4;
this.Controls.Add(this.lblActionType);
this.Controls.Add(this.txtActionType);

           //#####ObjectType###Int32
this.lblObjectType.AutoSize = true;
this.lblObjectType.Location = new System.Drawing.Point(100,125);
this.lblObjectType.Name = "lblObjectType";
this.lblObjectType.Size = new System.Drawing.Size(41, 12);
this.lblObjectType.TabIndex = 5;
this.lblObjectType.Text = "单据类型";
this.txtObjectType.Location = new System.Drawing.Point(173,121);
this.txtObjectType.Name = "txtObjectType";
this.txtObjectType.Size = new System.Drawing.Size(100, 21);
this.txtObjectType.TabIndex = 5;
this.Controls.Add(this.lblObjectType);
this.Controls.Add(this.txtObjectType);

           //#####ObjectId###Int64
this.lblObjectId.AutoSize = true;
this.lblObjectId.Location = new System.Drawing.Point(100,150);
this.lblObjectId.Name = "lblObjectId";
this.lblObjectId.Size = new System.Drawing.Size(41, 12);
this.lblObjectId.TabIndex = 6;
this.lblObjectId.Text = "单据ID";
this.txtObjectId.Location = new System.Drawing.Point(173,146);
this.txtObjectId.Name = "txtObjectId";
this.txtObjectId.Size = new System.Drawing.Size(100, 21);
this.txtObjectId.TabIndex = 6;
this.Controls.Add(this.lblObjectId);
this.Controls.Add(this.txtObjectId);

           //#####50ObjectNo###String
this.lblObjectNo.AutoSize = true;
this.lblObjectNo.Location = new System.Drawing.Point(100,175);
this.lblObjectNo.Name = "lblObjectNo";
this.lblObjectNo.Size = new System.Drawing.Size(41, 12);
this.lblObjectNo.TabIndex = 7;
this.lblObjectNo.Text = "单据编号";
this.txtObjectNo.Location = new System.Drawing.Point(173,171);
this.txtObjectNo.Name = "txtObjectNo";
this.txtObjectNo.Size = new System.Drawing.Size(100, 21);
this.txtObjectNo.TabIndex = 7;
this.Controls.Add(this.lblObjectNo);
this.Controls.Add(this.txtObjectNo);

           //#####100OldState###String
this.lblOldState.AutoSize = true;
this.lblOldState.Location = new System.Drawing.Point(100,200);
this.lblOldState.Name = "lblOldState";
this.lblOldState.Size = new System.Drawing.Size(41, 12);
this.lblOldState.TabIndex = 8;
this.lblOldState.Text = "操作前状态";
this.txtOldState.Location = new System.Drawing.Point(173,196);
this.txtOldState.Name = "txtOldState";
this.txtOldState.Size = new System.Drawing.Size(100, 21);
this.txtOldState.TabIndex = 8;
this.Controls.Add(this.lblOldState);
this.Controls.Add(this.txtOldState);

           //#####100NewState###String
this.lblNewState.AutoSize = true;
this.lblNewState.Location = new System.Drawing.Point(100,225);
this.lblNewState.Name = "lblNewState";
this.lblNewState.Size = new System.Drawing.Size(41, 12);
this.lblNewState.TabIndex = 9;
this.lblNewState.Text = "操作后状态";
this.txtNewState.Location = new System.Drawing.Point(173,221);
this.txtNewState.Name = "txtNewState";
this.txtNewState.Size = new System.Drawing.Size(100, 21);
this.txtNewState.TabIndex = 9;
this.Controls.Add(this.lblNewState);
this.Controls.Add(this.txtNewState);

           //#####2147483647DataContent###String
this.lblDataContent.AutoSize = true;
this.lblDataContent.Location = new System.Drawing.Point(100,250);
this.lblDataContent.Name = "lblDataContent";
this.lblDataContent.Size = new System.Drawing.Size(41, 12);
this.lblDataContent.TabIndex = 10;
this.lblDataContent.Text = "审计日志";
this.txtDataContent.Location = new System.Drawing.Point(173,246);
this.txtDataContent.Name = "txtDataContent";
this.txtDataContent.Size = new System.Drawing.Size(100, 21);
this.txtDataContent.TabIndex = 10;
this.txtDataContent.Multiline = true;
this.Controls.Add(this.lblDataContent);
this.Controls.Add(this.txtDataContent);

           //#####8000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,275);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 11;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,271);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 11;
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
           // this.kryptonPanel1.TabIndex = 11;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblUserName );
this.Controls.Add(this.txtUserName );

                this.Controls.Add(this.lblActionTime );
this.Controls.Add(this.dtpActionTime );

                this.Controls.Add(this.lblActionType );
this.Controls.Add(this.txtActionType );

                this.Controls.Add(this.lblObjectType );
this.Controls.Add(this.txtObjectType );

                this.Controls.Add(this.lblObjectId );
this.Controls.Add(this.txtObjectId );

                this.Controls.Add(this.lblObjectNo );
this.Controls.Add(this.txtObjectNo );

                this.Controls.Add(this.lblOldState );
this.Controls.Add(this.txtOldState );

                this.Controls.Add(this.lblNewState );
this.Controls.Add(this.txtNewState );

                this.Controls.Add(this.lblDataContent );
this.Controls.Add(this.txtDataContent );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_FM_AuditLogsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_AuditLogsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUserName;
private Krypton.Toolkit.KryptonTextBox txtUserName;

    
        
              private Krypton.Toolkit.KryptonLabel lblActionTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpActionTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblActionType;
private Krypton.Toolkit.KryptonTextBox txtActionType;

    
        
              private Krypton.Toolkit.KryptonLabel lblObjectType;
private Krypton.Toolkit.KryptonTextBox txtObjectType;

    
        
              private Krypton.Toolkit.KryptonLabel lblObjectId;
private Krypton.Toolkit.KryptonTextBox txtObjectId;

    
        
              private Krypton.Toolkit.KryptonLabel lblObjectNo;
private Krypton.Toolkit.KryptonTextBox txtObjectNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblOldState;
private Krypton.Toolkit.KryptonTextBox txtOldState;

    
        
              private Krypton.Toolkit.KryptonLabel lblNewState;
private Krypton.Toolkit.KryptonTextBox txtNewState;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataContent;
private Krypton.Toolkit.KryptonTextBox txtDataContent;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

