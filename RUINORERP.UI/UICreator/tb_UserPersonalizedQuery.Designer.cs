
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
    partial class tb_UserPersonalizedQuery
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
     
     this.lblWorkCellSettings = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWorkCellSettings = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtWorkCellSettings.Multiline = true;

this.lblWorkCellLayout = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWorkCellLayout = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtWorkCellLayout.Multiline = true;

this.lblID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblUseUserOwnPrinter = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkUseUserOwnPrinter = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkUseUserOwnPrinter.Values.Text ="";

this.lblPrinterName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrinterName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSelectTemplatePrint = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkSelectTemplatePrint = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkSelectTemplatePrint.Values.Text ="";

this.lblUserFavoriteMenu = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUserFavoriteMenu = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtUserFavoriteMenu.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                    
            this.Name = "tb_UserPersonalizedQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWorkCellSettings;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWorkCellSettings;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWorkCellLayout;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWorkCellLayout;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUseUserOwnPrinter;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkUseUserOwnPrinter;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrinterName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrinterName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSelectTemplatePrint;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkSelectTemplatePrint;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUserFavoriteMenu;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUserFavoriteMenu;

    
    
   
 





    }
}


