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
    /// 单位换算表
    /// </summary>
    partial class tb_Unit_ConversionEdit
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
     this.lblUnitConversion_Name = new Krypton.Toolkit.KryptonLabel();
this.txtUnitConversion_Name = new Krypton.Toolkit.KryptonTextBox();

Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。this.lblSource_unit_id = new Krypton.Toolkit.KryptonLabel();
this.txtSource_unit_id = new Krypton.Toolkit.KryptonTextBox();

Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。this.lblTarget_unit_id = new Krypton.Toolkit.KryptonLabel();
this.txtTarget_unit_id = new Krypton.Toolkit.KryptonTextBox();

this.lblConversion_ratio = new Krypton.Toolkit.KryptonLabel();
this.txtConversion_ratio = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

    
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
Source_unit_id主外字段不一致。this.lblSource_unit_id.AutoSize = true;
this.lblSource_unit_id.Location = new System.Drawing.Point(100,50);
this.lblSource_unit_id.Name = "lblSource_unit_id";
this.lblSource_unit_id.Size = new System.Drawing.Size(41, 12);
this.lblSource_unit_id.TabIndex = 2;
this.lblSource_unit_id.Text = "来源单位";
this.txtSource_unit_id.Location = new System.Drawing.Point(173,46);
this.txtSource_unit_id.Name = "txtSource_unit_id";
this.txtSource_unit_id.Size = new System.Drawing.Size(100, 21);
this.txtSource_unit_id.TabIndex = 2;
this.Controls.Add(this.lblSource_unit_id);
this.Controls.Add(this.txtSource_unit_id);

           //#####Target_unit_id###Int64
//属性测试75Target_unit_id
Target_unit_id主外字段不一致。//属性测试75Target_unit_id
Source_unit_id主外字段不一致。this.lblTarget_unit_id.AutoSize = true;
this.lblTarget_unit_id.Location = new System.Drawing.Point(100,75);
this.lblTarget_unit_id.Name = "lblTarget_unit_id";
this.lblTarget_unit_id.Size = new System.Drawing.Size(41, 12);
this.lblTarget_unit_id.TabIndex = 3;
this.lblTarget_unit_id.Text = "目标单位";
this.txtTarget_unit_id.Location = new System.Drawing.Point(173,71);
this.txtTarget_unit_id.Name = "txtTarget_unit_id";
this.txtTarget_unit_id.Size = new System.Drawing.Size(100, 21);
this.txtTarget_unit_id.TabIndex = 3;
this.Controls.Add(this.lblTarget_unit_id);
this.Controls.Add(this.txtTarget_unit_id);

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
           // this.kryptonPanel1.TabIndex = 5;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUnitConversion_Name );
this.Controls.Add(this.txtUnitConversion_Name );

                Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。this.Controls.Add(this.lblSource_unit_id );
this.Controls.Add(this.txtSource_unit_id );

                Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。this.Controls.Add(this.lblTarget_unit_id );
this.Controls.Add(this.txtTarget_unit_id );

                this.Controls.Add(this.lblConversion_ratio );
this.Controls.Add(this.txtConversion_ratio );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_Unit_ConversionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_Unit_ConversionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblUnitConversion_Name;
private Krypton.Toolkit.KryptonTextBox txtUnitConversion_Name;

    
        
              Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSource_unit_id;
private Krypton.Toolkit.KryptonTextBox txtSource_unit_id;

    
        
              Target_unit_id主外字段不一致。Source_unit_id主外字段不一致。private Krypton.Toolkit.KryptonLabel lblTarget_unit_id;
private Krypton.Toolkit.KryptonTextBox txtTarget_unit_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblConversion_ratio;
private Krypton.Toolkit.KryptonTextBox txtConversion_ratio;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

