// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:46
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 省份表
    /// </summary>
    partial class tb_ProvincesEdit
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
     this.lblProvinceCNName = new Krypton.Toolkit.KryptonLabel();
this.txtProvinceCNName = new Krypton.Toolkit.KryptonTextBox();

this.lblCountryID = new Krypton.Toolkit.KryptonLabel();
this.txtCountryID = new Krypton.Toolkit.KryptonTextBox();

this.lblProvinceENName = new Krypton.Toolkit.KryptonLabel();
this.txtProvinceENName = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####80ProvinceCNName###String
this.lblProvinceCNName.AutoSize = true;
this.lblProvinceCNName.Location = new System.Drawing.Point(100,25);
this.lblProvinceCNName.Name = "lblProvinceCNName";
this.lblProvinceCNName.Size = new System.Drawing.Size(41, 12);
this.lblProvinceCNName.TabIndex = 1;
this.lblProvinceCNName.Text = "省份中文名";
this.txtProvinceCNName.Location = new System.Drawing.Point(173,21);
this.txtProvinceCNName.Name = "txtProvinceCNName";
this.txtProvinceCNName.Size = new System.Drawing.Size(100, 21);
this.txtProvinceCNName.TabIndex = 1;
this.Controls.Add(this.lblProvinceCNName);
this.Controls.Add(this.txtProvinceCNName);

           //#####CountryID###Int64
this.lblCountryID.AutoSize = true;
this.lblCountryID.Location = new System.Drawing.Point(100,50);
this.lblCountryID.Name = "lblCountryID";
this.lblCountryID.Size = new System.Drawing.Size(41, 12);
this.lblCountryID.TabIndex = 2;
this.lblCountryID.Text = "国家";
this.txtCountryID.Location = new System.Drawing.Point(173,46);
this.txtCountryID.Name = "txtCountryID";
this.txtCountryID.Size = new System.Drawing.Size(100, 21);
this.txtCountryID.TabIndex = 2;
this.Controls.Add(this.lblCountryID);
this.Controls.Add(this.txtCountryID);

           //#####80ProvinceENName###String
this.lblProvinceENName.AutoSize = true;
this.lblProvinceENName.Location = new System.Drawing.Point(100,75);
this.lblProvinceENName.Name = "lblProvinceENName";
this.lblProvinceENName.Size = new System.Drawing.Size(41, 12);
this.lblProvinceENName.TabIndex = 3;
this.lblProvinceENName.Text = "省份英文名";
this.txtProvinceENName.Location = new System.Drawing.Point(173,71);
this.txtProvinceENName.Name = "txtProvinceENName";
this.txtProvinceENName.Size = new System.Drawing.Size(100, 21);
this.txtProvinceENName.TabIndex = 3;
this.Controls.Add(this.lblProvinceENName);
this.Controls.Add(this.txtProvinceENName);

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
           // this.kryptonPanel1.TabIndex = 3;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProvinceCNName );
this.Controls.Add(this.txtProvinceCNName );

                this.Controls.Add(this.lblCountryID );
this.Controls.Add(this.txtCountryID );

                this.Controls.Add(this.lblProvinceENName );
this.Controls.Add(this.txtProvinceENName );

                            // 
            // "tb_ProvincesEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProvincesEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProvinceCNName;
private Krypton.Toolkit.KryptonTextBox txtProvinceCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblCountryID;
private Krypton.Toolkit.KryptonTextBox txtCountryID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProvinceENName;
private Krypton.Toolkit.KryptonTextBox txtProvinceENName;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

