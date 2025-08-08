
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 报表打印配置表
    /// </summary>
    partial class tb_PrintConfigQuery
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
     
     this.lblConfig_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtConfig_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblBizName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBizName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrinterName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrinterName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrinterSelected = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkPrinterSelected = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkPrinterSelected.Values.Text ="";

this.lblLandscape = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkLandscape = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkLandscape.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100Config_Name###String
this.lblConfig_Name.AutoSize = true;
this.lblConfig_Name.Location = new System.Drawing.Point(100,25);
this.lblConfig_Name.Name = "lblConfig_Name";
this.lblConfig_Name.Size = new System.Drawing.Size(41, 12);
this.lblConfig_Name.TabIndex = 1;
this.lblConfig_Name.Text = "配置名称";
this.txtConfig_Name.Location = new System.Drawing.Point(173,21);
this.txtConfig_Name.Name = "txtConfig_Name";
this.txtConfig_Name.Size = new System.Drawing.Size(100, 21);
this.txtConfig_Name.TabIndex = 1;
this.Controls.Add(this.lblConfig_Name);
this.Controls.Add(this.txtConfig_Name);

           //#####BizType###Int32

           //#####30BizName###String
this.lblBizName.AutoSize = true;
this.lblBizName.Location = new System.Drawing.Point(100,75);
this.lblBizName.Name = "lblBizName";
this.lblBizName.Size = new System.Drawing.Size(41, 12);
this.lblBizName.TabIndex = 3;
this.lblBizName.Text = "业务名称";
this.txtBizName.Location = new System.Drawing.Point(173,71);
this.txtBizName.Name = "txtBizName";
this.txtBizName.Size = new System.Drawing.Size(100, 21);
this.txtBizName.TabIndex = 3;
this.Controls.Add(this.lblBizName);
this.Controls.Add(this.txtBizName);

           //#####200PrinterName###String
this.lblPrinterName.AutoSize = true;
this.lblPrinterName.Location = new System.Drawing.Point(100,100);
this.lblPrinterName.Name = "lblPrinterName";
this.lblPrinterName.Size = new System.Drawing.Size(41, 12);
this.lblPrinterName.TabIndex = 4;
this.lblPrinterName.Text = "打印机名称";
this.txtPrinterName.Location = new System.Drawing.Point(173,96);
this.txtPrinterName.Name = "txtPrinterName";
this.txtPrinterName.Size = new System.Drawing.Size(100, 21);
this.txtPrinterName.TabIndex = 4;
this.Controls.Add(this.lblPrinterName);
this.Controls.Add(this.txtPrinterName);

           //#####PrinterSelected###Boolean
this.lblPrinterSelected.AutoSize = true;
this.lblPrinterSelected.Location = new System.Drawing.Point(100,125);
this.lblPrinterSelected.Name = "lblPrinterSelected";
this.lblPrinterSelected.Size = new System.Drawing.Size(41, 12);
this.lblPrinterSelected.TabIndex = 5;
this.lblPrinterSelected.Text = "设置了默认打印机";
this.chkPrinterSelected.Location = new System.Drawing.Point(173,121);
this.chkPrinterSelected.Name = "chkPrinterSelected";
this.chkPrinterSelected.Size = new System.Drawing.Size(100, 21);
this.chkPrinterSelected.TabIndex = 5;
this.Controls.Add(this.lblPrinterSelected);
this.Controls.Add(this.chkPrinterSelected);

           //#####Landscape###Boolean
this.lblLandscape.AutoSize = true;
this.lblLandscape.Location = new System.Drawing.Point(100,150);
this.lblLandscape.Name = "lblLandscape";
this.lblLandscape.Size = new System.Drawing.Size(41, 12);
this.lblLandscape.TabIndex = 6;
this.lblLandscape.Text = "设置横向打印";
this.chkLandscape.Location = new System.Drawing.Point(173,146);
this.chkLandscape.Name = "chkLandscape";
this.chkLandscape.Size = new System.Drawing.Size(100, 21);
this.chkLandscape.TabIndex = 6;
this.Controls.Add(this.lblLandscape);
this.Controls.Add(this.chkLandscape);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,175);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 7;
this.lblCreated_at.Text = "创建时间";
//111======175
this.dtpCreated_at.Location = new System.Drawing.Point(173,171);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 7;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,225);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 9;
this.lblModified_at.Text = "修改时间";
//111======225
this.dtpModified_at.Location = new System.Drawing.Point(173,221);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 9;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,275);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 11;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,271);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 11;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblConfig_Name );
this.Controls.Add(this.txtConfig_Name );

                
                this.Controls.Add(this.lblBizName );
this.Controls.Add(this.txtBizName );

                this.Controls.Add(this.lblPrinterName );
this.Controls.Add(this.txtPrinterName );

                this.Controls.Add(this.lblPrinterSelected );
this.Controls.Add(this.chkPrinterSelected );

                this.Controls.Add(this.lblLandscape );
this.Controls.Add(this.chkLandscape );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_PrintConfigQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConfig_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtConfig_Name;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBizName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBizName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrinterName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrinterName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrinterSelected;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkPrinterSelected;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLandscape;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkLandscape;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


