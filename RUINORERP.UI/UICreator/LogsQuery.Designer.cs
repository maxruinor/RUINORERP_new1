﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:31
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
    partial class LogsQuery
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
     
     this.lblDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblLevel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLevel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLogger = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLogger = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMessage = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMessage = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtMessage.Multiline = true;

this.lblException = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtException = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtException.Multiline = true;

this.lblOperator = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOperator = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblModName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblActionName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtActionName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIP = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtIP = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMAC = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMAC = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMachineName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMachineName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUser_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUser_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Date###DateTime
this.lblDate.AutoSize = true;
this.lblDate.Location = new System.Drawing.Point(100,25);
this.lblDate.Name = "lblDate";
this.lblDate.Size = new System.Drawing.Size(41, 12);
this.lblDate.TabIndex = 1;
this.lblDate.Text = "时间";
//111======25
this.dtpDate.Location = new System.Drawing.Point(173,21);
this.dtpDate.Name ="dtpDate";
this.dtpDate.ShowCheckBox =true;
this.dtpDate.Size = new System.Drawing.Size(100, 21);
this.dtpDate.TabIndex = 1;
this.Controls.Add(this.lblDate);
this.Controls.Add(this.dtpDate);

           //#####10Level###String
this.lblLevel.AutoSize = true;
this.lblLevel.Location = new System.Drawing.Point(100,50);
this.lblLevel.Name = "lblLevel";
this.lblLevel.Size = new System.Drawing.Size(41, 12);
this.lblLevel.TabIndex = 2;
this.lblLevel.Text = "级别";
this.txtLevel.Location = new System.Drawing.Point(173,46);
this.txtLevel.Name = "txtLevel";
this.txtLevel.Size = new System.Drawing.Size(100, 21);
this.txtLevel.TabIndex = 2;
this.Controls.Add(this.lblLevel);
this.Controls.Add(this.txtLevel);

           //#####50Logger###String
this.lblLogger.AutoSize = true;
this.lblLogger.Location = new System.Drawing.Point(100,75);
this.lblLogger.Name = "lblLogger";
this.lblLogger.Size = new System.Drawing.Size(41, 12);
this.lblLogger.TabIndex = 3;
this.lblLogger.Text = "记录器";
this.txtLogger.Location = new System.Drawing.Point(173,71);
this.txtLogger.Name = "txtLogger";
this.txtLogger.Size = new System.Drawing.Size(100, 21);
this.txtLogger.TabIndex = 3;
this.Controls.Add(this.lblLogger);
this.Controls.Add(this.txtLogger);

           //#####3000Message###String
this.lblMessage.AutoSize = true;
this.lblMessage.Location = new System.Drawing.Point(100,100);
this.lblMessage.Name = "lblMessage";
this.lblMessage.Size = new System.Drawing.Size(41, 12);
this.lblMessage.TabIndex = 4;
this.lblMessage.Text = "消息";
this.txtMessage.Location = new System.Drawing.Point(173,96);
this.txtMessage.Name = "txtMessage";
this.txtMessage.Size = new System.Drawing.Size(100, 21);
this.txtMessage.TabIndex = 4;
this.Controls.Add(this.lblMessage);
this.Controls.Add(this.txtMessage);

           //#####2147483647Exception###String
this.lblException.AutoSize = true;
this.lblException.Location = new System.Drawing.Point(100,125);
this.lblException.Name = "lblException";
this.lblException.Size = new System.Drawing.Size(41, 12);
this.lblException.TabIndex = 5;
this.lblException.Text = "异常";
this.txtException.Location = new System.Drawing.Point(173,121);
this.txtException.Name = "txtException";
this.txtException.Size = new System.Drawing.Size(100, 21);
this.txtException.TabIndex = 5;
this.txtException.Multiline = true;
this.Controls.Add(this.lblException);
this.Controls.Add(this.txtException);

           //#####50Operator###String
this.lblOperator.AutoSize = true;
this.lblOperator.Location = new System.Drawing.Point(100,150);
this.lblOperator.Name = "lblOperator";
this.lblOperator.Size = new System.Drawing.Size(41, 12);
this.lblOperator.TabIndex = 6;
this.lblOperator.Text = "操作者";
this.txtOperator.Location = new System.Drawing.Point(173,146);
this.txtOperator.Name = "txtOperator";
this.txtOperator.Size = new System.Drawing.Size(100, 21);
this.txtOperator.TabIndex = 6;
this.Controls.Add(this.lblOperator);
this.Controls.Add(this.txtOperator);

           //#####50ModName###String
this.lblModName.AutoSize = true;
this.lblModName.Location = new System.Drawing.Point(100,175);
this.lblModName.Name = "lblModName";
this.lblModName.Size = new System.Drawing.Size(41, 12);
this.lblModName.TabIndex = 7;
this.lblModName.Text = "模块名";
this.txtModName.Location = new System.Drawing.Point(173,171);
this.txtModName.Name = "txtModName";
this.txtModName.Size = new System.Drawing.Size(100, 21);
this.txtModName.TabIndex = 7;
this.Controls.Add(this.lblModName);
this.Controls.Add(this.txtModName);

           //#####100Path###String
this.lblPath.AutoSize = true;
this.lblPath.Location = new System.Drawing.Point(100,200);
this.lblPath.Name = "lblPath";
this.lblPath.Size = new System.Drawing.Size(41, 12);
this.lblPath.TabIndex = 8;
this.lblPath.Text = "路径";
this.txtPath.Location = new System.Drawing.Point(173,196);
this.txtPath.Name = "txtPath";
this.txtPath.Size = new System.Drawing.Size(100, 21);
this.txtPath.TabIndex = 8;
this.Controls.Add(this.lblPath);
this.Controls.Add(this.txtPath);

           //#####50ActionName###String
this.lblActionName.AutoSize = true;
this.lblActionName.Location = new System.Drawing.Point(100,225);
this.lblActionName.Name = "lblActionName";
this.lblActionName.Size = new System.Drawing.Size(41, 12);
this.lblActionName.TabIndex = 9;
this.lblActionName.Text = "动作";
this.txtActionName.Location = new System.Drawing.Point(173,221);
this.txtActionName.Name = "txtActionName";
this.txtActionName.Size = new System.Drawing.Size(100, 21);
this.txtActionName.TabIndex = 9;
this.Controls.Add(this.lblActionName);
this.Controls.Add(this.txtActionName);

           //#####20IP###String
this.lblIP.AutoSize = true;
this.lblIP.Location = new System.Drawing.Point(100,250);
this.lblIP.Name = "lblIP";
this.lblIP.Size = new System.Drawing.Size(41, 12);
this.lblIP.TabIndex = 10;
this.lblIP.Text = "网络地址";
this.txtIP.Location = new System.Drawing.Point(173,246);
this.txtIP.Name = "txtIP";
this.txtIP.Size = new System.Drawing.Size(100, 21);
this.txtIP.TabIndex = 10;
this.Controls.Add(this.lblIP);
this.Controls.Add(this.txtIP);

           //#####30MAC###String
this.lblMAC.AutoSize = true;
this.lblMAC.Location = new System.Drawing.Point(100,275);
this.lblMAC.Name = "lblMAC";
this.lblMAC.Size = new System.Drawing.Size(41, 12);
this.lblMAC.TabIndex = 11;
this.lblMAC.Text = "物理地址";
this.txtMAC.Location = new System.Drawing.Point(173,271);
this.txtMAC.Name = "txtMAC";
this.txtMAC.Size = new System.Drawing.Size(100, 21);
this.txtMAC.TabIndex = 11;
this.Controls.Add(this.lblMAC);
this.Controls.Add(this.txtMAC);

           //#####50MachineName###String
this.lblMachineName.AutoSize = true;
this.lblMachineName.Location = new System.Drawing.Point(100,300);
this.lblMachineName.Name = "lblMachineName";
this.lblMachineName.Size = new System.Drawing.Size(41, 12);
this.lblMachineName.TabIndex = 12;
this.lblMachineName.Text = "电脑名";
this.txtMachineName.Location = new System.Drawing.Point(173,296);
this.txtMachineName.Name = "txtMachineName";
this.txtMachineName.Size = new System.Drawing.Size(100, 21);
this.txtMachineName.TabIndex = 12;
this.Controls.Add(this.lblMachineName);
this.Controls.Add(this.txtMachineName);

           //#####User_ID###Int64
//属性测试325User_ID
this.lblUser_ID.AutoSize = true;
this.lblUser_ID.Location = new System.Drawing.Point(100,325);
this.lblUser_ID.Name = "lblUser_ID";
this.lblUser_ID.Size = new System.Drawing.Size(41, 12);
this.lblUser_ID.TabIndex = 13;
this.lblUser_ID.Text = "用户";
//111======325
this.cmbUser_ID.Location = new System.Drawing.Point(173,321);
this.cmbUser_ID.Name ="cmbUser_ID";
this.cmbUser_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUser_ID.TabIndex = 13;
this.Controls.Add(this.lblUser_ID);
this.Controls.Add(this.cmbUser_ID);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblDate );
this.Controls.Add(this.dtpDate );

                this.Controls.Add(this.lblLevel );
this.Controls.Add(this.txtLevel );

                this.Controls.Add(this.lblLogger );
this.Controls.Add(this.txtLogger );

                this.Controls.Add(this.lblMessage );
this.Controls.Add(this.txtMessage );

                this.Controls.Add(this.lblException );
this.Controls.Add(this.txtException );

                this.Controls.Add(this.lblOperator );
this.Controls.Add(this.txtOperator );

                this.Controls.Add(this.lblModName );
this.Controls.Add(this.txtModName );

                this.Controls.Add(this.lblPath );
this.Controls.Add(this.txtPath );

                this.Controls.Add(this.lblActionName );
this.Controls.Add(this.txtActionName );

                this.Controls.Add(this.lblIP );
this.Controls.Add(this.txtIP );

                this.Controls.Add(this.lblMAC );
this.Controls.Add(this.txtMAC );

                this.Controls.Add(this.lblMachineName );
this.Controls.Add(this.txtMachineName );

                this.Controls.Add(this.lblUser_ID );
this.Controls.Add(this.cmbUser_ID );

                    
            this.Name = "LogsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLevel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLevel;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLogger;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLogger;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMessage;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMessage;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblException;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtException;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOperator;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOperator;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblActionName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtActionName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIP;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtIP;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMAC;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMAC;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMachineName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMachineName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUser_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUser_ID;

    
    
   
 





    }
}


