
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:16
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 城市表
    /// </summary>
    partial class tb_CitiesQuery
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
     
     this.lblProvinceID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProvinceID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCityCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCityCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCityENName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCityENName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProvinceID###Int64
//属性测试25ProvinceID
this.lblProvinceID.AutoSize = true;
this.lblProvinceID.Location = new System.Drawing.Point(100,25);
this.lblProvinceID.Name = "lblProvinceID";
this.lblProvinceID.Size = new System.Drawing.Size(41, 12);
this.lblProvinceID.TabIndex = 1;
this.lblProvinceID.Text = "省";
//111======25
this.cmbProvinceID.Location = new System.Drawing.Point(173,21);
this.cmbProvinceID.Name ="cmbProvinceID";
this.cmbProvinceID.Size = new System.Drawing.Size(100, 21);
this.cmbProvinceID.TabIndex = 1;
this.Controls.Add(this.lblProvinceID);
this.Controls.Add(this.cmbProvinceID);

           //#####80CityCNName###String
this.lblCityCNName.AutoSize = true;
this.lblCityCNName.Location = new System.Drawing.Point(100,50);
this.lblCityCNName.Name = "lblCityCNName";
this.lblCityCNName.Size = new System.Drawing.Size(41, 12);
this.lblCityCNName.TabIndex = 2;
this.lblCityCNName.Text = "城市中文名";
this.txtCityCNName.Location = new System.Drawing.Point(173,46);
this.txtCityCNName.Name = "txtCityCNName";
this.txtCityCNName.Size = new System.Drawing.Size(100, 21);
this.txtCityCNName.TabIndex = 2;
this.Controls.Add(this.lblCityCNName);
this.Controls.Add(this.txtCityCNName);

           //#####80CityENName###String
this.lblCityENName.AutoSize = true;
this.lblCityENName.Location = new System.Drawing.Point(100,75);
this.lblCityENName.Name = "lblCityENName";
this.lblCityENName.Size = new System.Drawing.Size(41, 12);
this.lblCityENName.TabIndex = 3;
this.lblCityENName.Text = "城市英文名";
this.txtCityENName.Location = new System.Drawing.Point(173,71);
this.txtCityENName.Name = "txtCityENName";
this.txtCityENName.Size = new System.Drawing.Size(100, 21);
this.txtCityENName.TabIndex = 3;
this.Controls.Add(this.lblCityENName);
this.Controls.Add(this.txtCityENName);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProvinceID );
this.Controls.Add(this.cmbProvinceID );

                this.Controls.Add(this.lblCityCNName );
this.Controls.Add(this.txtCityCNName );

                this.Controls.Add(this.lblCityENName );
this.Controls.Add(this.txtCityENName );

                    
            this.Name = "tb_CitiesQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProvinceID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProvinceID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCityCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCityCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCityENName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCityENName;

    
    
   
 





    }
}


