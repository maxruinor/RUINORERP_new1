
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:00
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 可视化排程
    /// </summary>
    partial class tb_ProduceViewScheduleQuery
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
     
     

this.lblstart_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpstart_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblend_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpend_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####product_id###Int32

           //#####quantity###Int32

           //#####start_date###DateTime
this.lblstart_date.AutoSize = true;
this.lblstart_date.Location = new System.Drawing.Point(100,75);
this.lblstart_date.Name = "lblstart_date";
this.lblstart_date.Size = new System.Drawing.Size(41, 12);
this.lblstart_date.TabIndex = 3;
this.lblstart_date.Text = "计划开始日期";
//111======75
this.dtpstart_date.Location = new System.Drawing.Point(173,71);
this.dtpstart_date.Name ="dtpstart_date";
this.dtpstart_date.ShowCheckBox =true;
this.dtpstart_date.Size = new System.Drawing.Size(100, 21);
this.dtpstart_date.TabIndex = 3;
this.Controls.Add(this.lblstart_date);
this.Controls.Add(this.dtpstart_date);

           //#####end_date###DateTime
this.lblend_date.AutoSize = true;
this.lblend_date.Location = new System.Drawing.Point(100,100);
this.lblend_date.Name = "lblend_date";
this.lblend_date.Size = new System.Drawing.Size(41, 12);
this.lblend_date.TabIndex = 4;
this.lblend_date.Text = "计划完成日期";
//111======100
this.dtpend_date.Location = new System.Drawing.Point(173,96);
this.dtpend_date.Name ="dtpend_date";
this.dtpend_date.ShowCheckBox =true;
this.dtpend_date.Size = new System.Drawing.Size(100, 21);
this.dtpend_date.TabIndex = 4;
this.Controls.Add(this.lblend_date);
this.Controls.Add(this.dtpend_date);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                this.Controls.Add(this.lblstart_date );
this.Controls.Add(this.dtpstart_date );

                this.Controls.Add(this.lblend_date );
this.Controls.Add(this.dtpend_date );

                    
            this.Name = "tb_ProduceViewScheduleQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblstart_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpstart_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblend_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpend_date;

    
    
   
 





    }
}


