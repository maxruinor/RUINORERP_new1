
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
    partial class tb_ProvincesQuery
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
     
     this.lblProvinceCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProvinceCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblProvinceENName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProvinceENName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProvinceCNName );
this.Controls.Add(this.txtProvinceCNName );

                
                this.Controls.Add(this.lblProvinceENName );
this.Controls.Add(this.txtProvinceENName );

                    
            this.Name = "tb_ProvincesQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProvinceCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProvinceCNName;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProvinceENName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProvinceENName;

    
    
   
 





    }
}


