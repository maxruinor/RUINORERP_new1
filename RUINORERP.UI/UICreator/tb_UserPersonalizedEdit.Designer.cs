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
    /// 用户角色个性化设置表
    /// </summary>
    partial class tb_UserPersonalizedEdit
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
     this.lblWorkCellSettings = new Krypton.Toolkit.KryptonLabel();
this.txtWorkCellSettings = new Krypton.Toolkit.KryptonTextBox();
this.txtWorkCellSettings.Multiline = true;

this.lblWorkCellLayout = new Krypton.Toolkit.KryptonLabel();
this.txtWorkCellLayout = new Krypton.Toolkit.KryptonTextBox();
this.txtWorkCellLayout.Multiline = true;

this.lblID = new Krypton.Toolkit.KryptonLabel();
this.cmbID = new Krypton.Toolkit.KryptonComboBox();

this.lblUseUserOwnPrinter = new Krypton.Toolkit.KryptonLabel();
this.chkUseUserOwnPrinter = new Krypton.Toolkit.KryptonCheckBox();
this.chkUseUserOwnPrinter.Values.Text ="";

this.lblPrinterName = new Krypton.Toolkit.KryptonLabel();
this.txtPrinterName = new Krypton.Toolkit.KryptonTextBox();

this.lblSelectTemplatePrint = new Krypton.Toolkit.KryptonLabel();
this.chkSelectTemplatePrint = new Krypton.Toolkit.KryptonCheckBox();
this.chkSelectTemplatePrint.Values.Text ="";

this.lblUserFavoriteMenu = new Krypton.Toolkit.KryptonLabel();
this.txtUserFavoriteMenu = new Krypton.Toolkit.KryptonTextBox();
this.txtUserFavoriteMenu.Multiline = true;

    
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
     
            //#####2147483647WorkCellSettings###String
this.lblWorkCellSettings.AutoSize = true;
this.lblWorkCellSettings.Location = new System.Drawing.Point(100,25);
this.lblWorkCellSettings.Name = "lblWorkCellSettings";
this.lblWorkCellSettings.Size = new System.Drawing.Size(41, 12);
this.lblWorkCellSettings.TabIndex = 1;
this.lblWorkCellSettings.Text = "工作单元设置";
this.txtWorkCellSettings.Location = new System.Drawing.Point(173,21);
this.txtWorkCellSettings.Name = "txtWorkCellSettings";
this.txtWorkCellSettings.Size = new System.Drawing.Size(100, 21);
this.txtWorkCellSettings.TabIndex = 1;
this.txtWorkCellSettings.Multiline = true;
this.Controls.Add(this.lblWorkCellSettings);
this.Controls.Add(this.txtWorkCellSettings);

           //#####2147483647WorkCellLayout###String
this.lblWorkCellLayout.AutoSize = true;
this.lblWorkCellLayout.Location = new System.Drawing.Point(100,50);
this.lblWorkCellLayout.Name = "lblWorkCellLayout";
this.lblWorkCellLayout.Size = new System.Drawing.Size(41, 12);
this.lblWorkCellLayout.TabIndex = 2;
this.lblWorkCellLayout.Text = "工作台布局";
this.txtWorkCellLayout.Location = new System.Drawing.Point(173,46);
this.txtWorkCellLayout.Name = "txtWorkCellLayout";
this.txtWorkCellLayout.Size = new System.Drawing.Size(100, 21);
this.txtWorkCellLayout.TabIndex = 2;
this.txtWorkCellLayout.Multiline = true;
this.Controls.Add(this.lblWorkCellLayout);
this.Controls.Add(this.txtWorkCellLayout);

           //#####ID###Int64
//属性测试75ID
this.lblID.AutoSize = true;
this.lblID.Location = new System.Drawing.Point(100,75);
this.lblID.Name = "lblID";
this.lblID.Size = new System.Drawing.Size(41, 12);
this.lblID.TabIndex = 3;
this.lblID.Text = "用户角色";
//111======75
this.cmbID.Location = new System.Drawing.Point(173,71);
this.cmbID.Name ="cmbID";
this.cmbID.Size = new System.Drawing.Size(100, 21);
this.cmbID.TabIndex = 3;
this.Controls.Add(this.lblID);
this.Controls.Add(this.cmbID);

           //#####UseUserOwnPrinter###Boolean
this.lblUseUserOwnPrinter.AutoSize = true;
this.lblUseUserOwnPrinter.Location = new System.Drawing.Point(100,100);
this.lblUseUserOwnPrinter.Name = "lblUseUserOwnPrinter";
this.lblUseUserOwnPrinter.Size = new System.Drawing.Size(41, 12);
this.lblUseUserOwnPrinter.TabIndex = 4;
this.lblUseUserOwnPrinter.Text = "";
this.chkUseUserOwnPrinter.Location = new System.Drawing.Point(173,96);
this.chkUseUserOwnPrinter.Name = "chkUseUserOwnPrinter";
this.chkUseUserOwnPrinter.Size = new System.Drawing.Size(100, 21);
this.chkUseUserOwnPrinter.TabIndex = 4;
this.Controls.Add(this.lblUseUserOwnPrinter);
this.Controls.Add(this.chkUseUserOwnPrinter);

           //#####200PrinterName###String
this.lblPrinterName.AutoSize = true;
this.lblPrinterName.Location = new System.Drawing.Point(100,125);
this.lblPrinterName.Name = "lblPrinterName";
this.lblPrinterName.Size = new System.Drawing.Size(41, 12);
this.lblPrinterName.TabIndex = 5;
this.lblPrinterName.Text = "";
this.txtPrinterName.Location = new System.Drawing.Point(173,121);
this.txtPrinterName.Name = "txtPrinterName";
this.txtPrinterName.Size = new System.Drawing.Size(100, 21);
this.txtPrinterName.TabIndex = 5;
this.Controls.Add(this.lblPrinterName);
this.Controls.Add(this.txtPrinterName);

           //#####SelectTemplatePrint###Boolean
this.lblSelectTemplatePrint.AutoSize = true;
this.lblSelectTemplatePrint.Location = new System.Drawing.Point(100,150);
this.lblSelectTemplatePrint.Name = "lblSelectTemplatePrint";
this.lblSelectTemplatePrint.Size = new System.Drawing.Size(41, 12);
this.lblSelectTemplatePrint.TabIndex = 6;
this.lblSelectTemplatePrint.Text = "选择模板打印";
this.chkSelectTemplatePrint.Location = new System.Drawing.Point(173,146);
this.chkSelectTemplatePrint.Name = "chkSelectTemplatePrint";
this.chkSelectTemplatePrint.Size = new System.Drawing.Size(100, 21);
this.chkSelectTemplatePrint.TabIndex = 6;
this.Controls.Add(this.lblSelectTemplatePrint);
this.Controls.Add(this.chkSelectTemplatePrint);

           //#####2147483647UserFavoriteMenu###String
this.lblUserFavoriteMenu.AutoSize = true;
this.lblUserFavoriteMenu.Location = new System.Drawing.Point(100,175);
this.lblUserFavoriteMenu.Name = "lblUserFavoriteMenu";
this.lblUserFavoriteMenu.Size = new System.Drawing.Size(41, 12);
this.lblUserFavoriteMenu.TabIndex = 7;
this.lblUserFavoriteMenu.Text = "用户工具栏";
this.txtUserFavoriteMenu.Location = new System.Drawing.Point(173,171);
this.txtUserFavoriteMenu.Name = "txtUserFavoriteMenu";
this.txtUserFavoriteMenu.Size = new System.Drawing.Size(100, 21);
this.txtUserFavoriteMenu.TabIndex = 7;
this.txtUserFavoriteMenu.Multiline = true;
this.Controls.Add(this.lblUserFavoriteMenu);
this.Controls.Add(this.txtUserFavoriteMenu);

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
                this.Controls.Add(this.lblWorkCellSettings );
this.Controls.Add(this.txtWorkCellSettings );

                this.Controls.Add(this.lblWorkCellLayout );
this.Controls.Add(this.txtWorkCellLayout );

                this.Controls.Add(this.lblID );
this.Controls.Add(this.cmbID );

                this.Controls.Add(this.lblUseUserOwnPrinter );
this.Controls.Add(this.chkUseUserOwnPrinter );

                this.Controls.Add(this.lblPrinterName );
this.Controls.Add(this.txtPrinterName );

                this.Controls.Add(this.lblSelectTemplatePrint );
this.Controls.Add(this.chkSelectTemplatePrint );

                this.Controls.Add(this.lblUserFavoriteMenu );
this.Controls.Add(this.txtUserFavoriteMenu );

                            // 
            // "tb_UserPersonalizedEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_UserPersonalizedEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblWorkCellSettings;
private Krypton.Toolkit.KryptonTextBox txtWorkCellSettings;

    
        
              private Krypton.Toolkit.KryptonLabel lblWorkCellLayout;
private Krypton.Toolkit.KryptonTextBox txtWorkCellLayout;

    
        
              private Krypton.Toolkit.KryptonLabel lblID;
private Krypton.Toolkit.KryptonComboBox cmbID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUseUserOwnPrinter;
private Krypton.Toolkit.KryptonCheckBox chkUseUserOwnPrinter;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrinterName;
private Krypton.Toolkit.KryptonTextBox txtPrinterName;

    
        
              private Krypton.Toolkit.KryptonLabel lblSelectTemplatePrint;
private Krypton.Toolkit.KryptonCheckBox chkSelectTemplatePrint;

    
        
              private Krypton.Toolkit.KryptonLabel lblUserFavoriteMenu;
private Krypton.Toolkit.KryptonTextBox txtUserFavoriteMenu;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

