
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:03
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
     
     this.lblRegion_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRegion_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProvinceCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProvinceCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblProvinceENName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProvinceENName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Region_ID###Int64
//属性测试25Region_ID
this.lblRegion_ID.AutoSize = true;
this.lblRegion_ID.Location = new System.Drawing.Point(100,25);
this.lblRegion_ID.Name = "lblRegion_ID";
this.lblRegion_ID.Size = new System.Drawing.Size(41, 12);
this.lblRegion_ID.TabIndex = 1;
this.lblRegion_ID.Text = "地区";
//111======25
this.cmbRegion_ID.Location = new System.Drawing.Point(173,21);
this.cmbRegion_ID.Name ="cmbRegion_ID";
this.cmbRegion_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRegion_ID.TabIndex = 1;
this.Controls.Add(this.lblRegion_ID);
this.Controls.Add(this.cmbRegion_ID);

           //#####80ProvinceCNName###String
this.lblProvinceCNName.AutoSize = true;
this.lblProvinceCNName.Location = new System.Drawing.Point(100,50);
this.lblProvinceCNName.Name = "lblProvinceCNName";
this.lblProvinceCNName.Size = new System.Drawing.Size(41, 12);
this.lblProvinceCNName.TabIndex = 2;
this.lblProvinceCNName.Text = "省份中文名";
this.txtProvinceCNName.Location = new System.Drawing.Point(173,46);
this.txtProvinceCNName.Name = "txtProvinceCNName";
this.txtProvinceCNName.Size = new System.Drawing.Size(100, 21);
this.txtProvinceCNName.TabIndex = 2;
this.Controls.Add(this.lblProvinceCNName);
this.Controls.Add(this.txtProvinceCNName);

           //#####CountryID###Int64
//属性测试75CountryID

           //#####80ProvinceENName###String
this.lblProvinceENName.AutoSize = true;
this.lblProvinceENName.Location = new System.Drawing.Point(100,100);
this.lblProvinceENName.Name = "lblProvinceENName";
this.lblProvinceENName.Size = new System.Drawing.Size(41, 12);
this.lblProvinceENName.TabIndex = 4;
this.lblProvinceENName.Text = "省份英文名";
this.txtProvinceENName.Location = new System.Drawing.Point(173,96);
this.txtProvinceENName.Name = "txtProvinceENName";
this.txtProvinceENName.Size = new System.Drawing.Size(100, 21);
this.txtProvinceENName.TabIndex = 4;
this.Controls.Add(this.lblProvinceENName);
this.Controls.Add(this.txtProvinceENName);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRegion_ID );
this.Controls.Add(this.cmbRegion_ID );

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
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRegion_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRegion_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProvinceCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProvinceCNName;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProvinceENName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProvinceENName;

    
    
   
 





    }
}


