// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 工作台配置表
    /// </summary>
    partial class tb_WorkCenterConfigEdit
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
     this.lblRoleID = new Krypton.Toolkit.KryptonLabel();
this.txtRoleID = new Krypton.Toolkit.KryptonTextBox();

this.lblUser_ID = new Krypton.Toolkit.KryptonLabel();
this.txtUser_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblOperable = new Krypton.Toolkit.KryptonLabel();
this.chkOperable = new Krypton.Toolkit.KryptonCheckBox();
this.chkOperable.Values.Text ="";

this.lblOnlyDisplay = new Krypton.Toolkit.KryptonLabel();
this.chkOnlyDisplay = new Krypton.Toolkit.KryptonCheckBox();
this.chkOnlyDisplay.Values.Text ="";

this.lblToDoList = new Krypton.Toolkit.KryptonLabel();
this.txtToDoList = new Krypton.Toolkit.KryptonTextBox();
this.txtToDoList.Multiline = true;

this.lblFrequentlyMenus = new Krypton.Toolkit.KryptonLabel();
this.txtFrequentlyMenus = new Krypton.Toolkit.KryptonTextBox();

this.lblDataOverview = new Krypton.Toolkit.KryptonLabel();
this.txtDataOverview = new Krypton.Toolkit.KryptonTextBox();
this.txtDataOverview.Multiline = true;

    
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
     
            //#####RoleID###Int64
this.lblRoleID.AutoSize = true;
this.lblRoleID.Location = new System.Drawing.Point(100,25);
this.lblRoleID.Name = "lblRoleID";
this.lblRoleID.Size = new System.Drawing.Size(41, 12);
this.lblRoleID.TabIndex = 1;
this.lblRoleID.Text = "角色";
this.txtRoleID.Location = new System.Drawing.Point(173,21);
this.txtRoleID.Name = "txtRoleID";
this.txtRoleID.Size = new System.Drawing.Size(100, 21);
this.txtRoleID.TabIndex = 1;
this.Controls.Add(this.lblRoleID);
this.Controls.Add(this.txtRoleID);

           //#####User_ID###Int64
this.lblUser_ID.AutoSize = true;
this.lblUser_ID.Location = new System.Drawing.Point(100,50);
this.lblUser_ID.Name = "lblUser_ID";
this.lblUser_ID.Size = new System.Drawing.Size(41, 12);
this.lblUser_ID.TabIndex = 2;
this.lblUser_ID.Text = "用户";
this.txtUser_ID.Location = new System.Drawing.Point(173,46);
this.txtUser_ID.Name = "txtUser_ID";
this.txtUser_ID.Size = new System.Drawing.Size(100, 21);
this.txtUser_ID.TabIndex = 2;
this.Controls.Add(this.lblUser_ID);
this.Controls.Add(this.txtUser_ID);

           //#####Operable###Boolean
this.lblOperable.AutoSize = true;
this.lblOperable.Location = new System.Drawing.Point(100,75);
this.lblOperable.Name = "lblOperable";
this.lblOperable.Size = new System.Drawing.Size(41, 12);
this.lblOperable.TabIndex = 3;
this.lblOperable.Text = "可操作";
this.chkOperable.Location = new System.Drawing.Point(173,71);
this.chkOperable.Name = "chkOperable";
this.chkOperable.Size = new System.Drawing.Size(100, 21);
this.chkOperable.TabIndex = 3;
this.Controls.Add(this.lblOperable);
this.Controls.Add(this.chkOperable);

           //#####OnlyDisplay###Boolean
this.lblOnlyDisplay.AutoSize = true;
this.lblOnlyDisplay.Location = new System.Drawing.Point(100,100);
this.lblOnlyDisplay.Name = "lblOnlyDisplay";
this.lblOnlyDisplay.Size = new System.Drawing.Size(41, 12);
this.lblOnlyDisplay.TabIndex = 4;
this.lblOnlyDisplay.Text = "仅展示";
this.chkOnlyDisplay.Location = new System.Drawing.Point(173,96);
this.chkOnlyDisplay.Name = "chkOnlyDisplay";
this.chkOnlyDisplay.Size = new System.Drawing.Size(100, 21);
this.chkOnlyDisplay.TabIndex = 4;
this.Controls.Add(this.lblOnlyDisplay);
this.Controls.Add(this.chkOnlyDisplay);

           //#####500ToDoList###String
this.lblToDoList.AutoSize = true;
this.lblToDoList.Location = new System.Drawing.Point(100,125);
this.lblToDoList.Name = "lblToDoList";
this.lblToDoList.Size = new System.Drawing.Size(41, 12);
this.lblToDoList.TabIndex = 5;
this.lblToDoList.Text = "待办事项";
this.txtToDoList.Location = new System.Drawing.Point(173,121);
this.txtToDoList.Name = "txtToDoList";
this.txtToDoList.Size = new System.Drawing.Size(100, 21);
this.txtToDoList.TabIndex = 5;
this.Controls.Add(this.lblToDoList);
this.Controls.Add(this.txtToDoList);

           //#####200FrequentlyMenus###String
this.lblFrequentlyMenus.AutoSize = true;
this.lblFrequentlyMenus.Location = new System.Drawing.Point(100,150);
this.lblFrequentlyMenus.Name = "lblFrequentlyMenus";
this.lblFrequentlyMenus.Size = new System.Drawing.Size(41, 12);
this.lblFrequentlyMenus.TabIndex = 6;
this.lblFrequentlyMenus.Text = "常用菜单";
this.txtFrequentlyMenus.Location = new System.Drawing.Point(173,146);
this.txtFrequentlyMenus.Name = "txtFrequentlyMenus";
this.txtFrequentlyMenus.Size = new System.Drawing.Size(100, 21);
this.txtFrequentlyMenus.TabIndex = 6;
this.Controls.Add(this.lblFrequentlyMenus);
this.Controls.Add(this.txtFrequentlyMenus);

           //#####500DataOverview###String
this.lblDataOverview.AutoSize = true;
this.lblDataOverview.Location = new System.Drawing.Point(100,175);
this.lblDataOverview.Name = "lblDataOverview";
this.lblDataOverview.Size = new System.Drawing.Size(41, 12);
this.lblDataOverview.TabIndex = 7;
this.lblDataOverview.Text = "数据概览";
this.txtDataOverview.Location = new System.Drawing.Point(173,171);
this.txtDataOverview.Name = "txtDataOverview";
this.txtDataOverview.Size = new System.Drawing.Size(100, 21);
this.txtDataOverview.TabIndex = 7;
this.Controls.Add(this.lblDataOverview);
this.Controls.Add(this.txtDataOverview);

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
                this.Controls.Add(this.lblRoleID );
this.Controls.Add(this.txtRoleID );

                this.Controls.Add(this.lblUser_ID );
this.Controls.Add(this.txtUser_ID );

                this.Controls.Add(this.lblOperable );
this.Controls.Add(this.chkOperable );

                this.Controls.Add(this.lblOnlyDisplay );
this.Controls.Add(this.chkOnlyDisplay );

                this.Controls.Add(this.lblToDoList );
this.Controls.Add(this.txtToDoList );

                this.Controls.Add(this.lblFrequentlyMenus );
this.Controls.Add(this.txtFrequentlyMenus );

                this.Controls.Add(this.lblDataOverview );
this.Controls.Add(this.txtDataOverview );

                            // 
            // "tb_WorkCenterConfigEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_WorkCenterConfigEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblRoleID;
private Krypton.Toolkit.KryptonTextBox txtRoleID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUser_ID;
private Krypton.Toolkit.KryptonTextBox txtUser_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblOperable;
private Krypton.Toolkit.KryptonCheckBox chkOperable;

    
        
              private Krypton.Toolkit.KryptonLabel lblOnlyDisplay;
private Krypton.Toolkit.KryptonCheckBox chkOnlyDisplay;

    
        
              private Krypton.Toolkit.KryptonLabel lblToDoList;
private Krypton.Toolkit.KryptonTextBox txtToDoList;

    
        
              private Krypton.Toolkit.KryptonLabel lblFrequentlyMenus;
private Krypton.Toolkit.KryptonTextBox txtFrequentlyMenus;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataOverview;
private Krypton.Toolkit.KryptonTextBox txtDataOverview;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

