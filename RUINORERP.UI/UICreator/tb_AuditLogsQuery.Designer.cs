﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2024 18:45:27
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 审计日志表
    /// </summary>
    partial class tb_AuditLogsQuery
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
     
     this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblUserName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUserName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtUserName.Multiline = true;

this.lblActionTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpActionTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblActionType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtActionType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblObjectNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtObjectNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOldState = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOldState = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNewState = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNewState = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Employee_ID###Int64
//属性测试25Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,25);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 1;
this.lblEmployee_ID.Text = "员工信息";
//111======25
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,21);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 1;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

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
//属性测试125ObjectType

           //#####ObjectId###Int64
//属性测试150ObjectId

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

           //#####100Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,250);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 10;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,246);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 10;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblUserName );
this.Controls.Add(this.txtUserName );

                this.Controls.Add(this.lblActionTime );
this.Controls.Add(this.dtpActionTime );

                this.Controls.Add(this.lblActionType );
this.Controls.Add(this.txtActionType );

                
                
                this.Controls.Add(this.lblObjectNo );
this.Controls.Add(this.txtObjectNo );

                this.Controls.Add(this.lblOldState );
this.Controls.Add(this.txtOldState );

                this.Controls.Add(this.lblNewState );
this.Controls.Add(this.txtNewState );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_AuditLogsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUserName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUserName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblActionTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpActionTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblActionType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtActionType;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblObjectNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtObjectNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOldState;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOldState;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNewState;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNewState;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}

