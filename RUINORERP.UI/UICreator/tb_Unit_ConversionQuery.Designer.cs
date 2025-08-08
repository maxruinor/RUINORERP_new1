
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
    /// 单位换算表
    /// </summary>
    partial class tb_Unit_ConversionQuery
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
     
     this.lblUnitConversion_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitConversion_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。
Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。
this.lblConversion_ratio = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtConversion_ratio = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100UnitConversion_Name###String
this.lblUnitConversion_Name.AutoSize = true;
this.lblUnitConversion_Name.Location = new System.Drawing.Point(100,25);
this.lblUnitConversion_Name.Name = "lblUnitConversion_Name";
this.lblUnitConversion_Name.Size = new System.Drawing.Size(41, 12);
this.lblUnitConversion_Name.TabIndex = 1;
this.lblUnitConversion_Name.Text = "备注";
this.txtUnitConversion_Name.Location = new System.Drawing.Point(173,21);
this.txtUnitConversion_Name.Name = "txtUnitConversion_Name";
this.txtUnitConversion_Name.Size = new System.Drawing.Size(100, 21);
this.txtUnitConversion_Name.TabIndex = 1;
this.Controls.Add(this.lblUnitConversion_Name);
this.Controls.Add(this.txtUnitConversion_Name);

           //#####Source_unit_id###Int64
//属性测试50Source_unit_id
Target_unit_id主外字段不一致。//属性测试50Source_unit_id
Source_unit_id主外字段不一致。
           //#####Target_unit_id###Int64
//属性测试75Target_unit_id
Target_unit_id主外字段不一致。//属性测试75Target_unit_id
Source_unit_id主外字段不一致。
           //#####Conversion_ratio###Decimal
this.lblConversion_ratio.AutoSize = true;
this.lblConversion_ratio.Location = new System.Drawing.Point(100,100);
this.lblConversion_ratio.Name = "lblConversion_ratio";
this.lblConversion_ratio.Size = new System.Drawing.Size(41, 12);
this.lblConversion_ratio.TabIndex = 4;
this.lblConversion_ratio.Text = "换算比例";
//111======100
this.txtConversion_ratio.Location = new System.Drawing.Point(173,96);
this.txtConversion_ratio.Name ="txtConversion_ratio";
this.txtConversion_ratio.Size = new System.Drawing.Size(100, 21);
this.txtConversion_ratio.TabIndex = 4;
this.Controls.Add(this.lblConversion_ratio);
this.Controls.Add(this.txtConversion_ratio);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,125);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 5;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,121);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 5;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUnitConversion_Name );
this.Controls.Add(this.txtUnitConversion_Name );

                Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。
                Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。
                this.Controls.Add(this.lblConversion_ratio );
this.Controls.Add(this.txtConversion_ratio );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_Unit_ConversionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitConversion_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitConversion_Name;

    
        
              Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。
    
        
              Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConversion_ratio;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtConversion_ratio;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


