
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:24
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 基本单位
    /// </summary>
    partial class tb_UnitQuery
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
     
     this.lblUnitName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtUnitName.Multiline = true;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblis_measurement_unit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkis_measurement_unit = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkis_measurement_unit.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####255UnitName###String
this.lblUnitName.AutoSize = true;
this.lblUnitName.Location = new System.Drawing.Point(100,25);
this.lblUnitName.Name = "lblUnitName";
this.lblUnitName.Size = new System.Drawing.Size(41, 12);
this.lblUnitName.TabIndex = 1;
this.lblUnitName.Text = "单位名称";
this.txtUnitName.Location = new System.Drawing.Point(173,21);
this.txtUnitName.Name = "txtUnitName";
this.txtUnitName.Size = new System.Drawing.Size(100, 21);
this.txtUnitName.TabIndex = 1;
this.Controls.Add(this.lblUnitName);
this.Controls.Add(this.txtUnitName);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,50);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 2;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,46);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 2;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####is_measurement_unit###Boolean
this.lblis_measurement_unit.AutoSize = true;
this.lblis_measurement_unit.Location = new System.Drawing.Point(100,75);
this.lblis_measurement_unit.Name = "lblis_measurement_unit";
this.lblis_measurement_unit.Size = new System.Drawing.Size(41, 12);
this.lblis_measurement_unit.TabIndex = 3;
this.lblis_measurement_unit.Text = "是否可换算";
this.chkis_measurement_unit.Location = new System.Drawing.Point(173,71);
this.chkis_measurement_unit.Name = "chkis_measurement_unit";
this.chkis_measurement_unit.Size = new System.Drawing.Size(100, 21);
this.chkis_measurement_unit.TabIndex = 3;
this.Controls.Add(this.lblis_measurement_unit);
this.Controls.Add(this.chkis_measurement_unit);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUnitName );
this.Controls.Add(this.txtUnitName );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblis_measurement_unit );
this.Controls.Add(this.chkis_measurement_unit );

                    
            this.Name = "tb_UnitQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblis_measurement_unit;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkis_measurement_unit;

    
    
   
 





    }
}


