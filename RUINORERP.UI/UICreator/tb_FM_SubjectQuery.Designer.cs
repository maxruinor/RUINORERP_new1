
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:39:08
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 会计科目表，财务系统中使用
    /// </summary>
    partial class tb_FM_SubjectQuery
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
     
     
this.lblsubject_code = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtsubject_code = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblsubject_name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtsubject_name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblsubject_en_name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtsubject_en_name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblBalance_direction = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkBalance_direction = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkBalance_direction.Values.Text ="";

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblEndDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEndDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####parent_subject_id###Int64

           //#####50subject_code###String
this.lblsubject_code.AutoSize = true;
this.lblsubject_code.Location = new System.Drawing.Point(100,50);
this.lblsubject_code.Name = "lblsubject_code";
this.lblsubject_code.Size = new System.Drawing.Size(41, 12);
this.lblsubject_code.TabIndex = 2;
this.lblsubject_code.Text = "科目代码";
this.txtsubject_code.Location = new System.Drawing.Point(173,46);
this.txtsubject_code.Name = "txtsubject_code";
this.txtsubject_code.Size = new System.Drawing.Size(100, 21);
this.txtsubject_code.TabIndex = 2;
this.Controls.Add(this.lblsubject_code);
this.Controls.Add(this.txtsubject_code);

           //#####100subject_name###String
this.lblsubject_name.AutoSize = true;
this.lblsubject_name.Location = new System.Drawing.Point(100,75);
this.lblsubject_name.Name = "lblsubject_name";
this.lblsubject_name.Size = new System.Drawing.Size(41, 12);
this.lblsubject_name.TabIndex = 3;
this.lblsubject_name.Text = "科目名称";
this.txtsubject_name.Location = new System.Drawing.Point(173,71);
this.txtsubject_name.Name = "txtsubject_name";
this.txtsubject_name.Size = new System.Drawing.Size(100, 21);
this.txtsubject_name.TabIndex = 3;
this.Controls.Add(this.lblsubject_name);
this.Controls.Add(this.txtsubject_name);

           //#####100subject_en_name###String
this.lblsubject_en_name.AutoSize = true;
this.lblsubject_en_name.Location = new System.Drawing.Point(100,100);
this.lblsubject_en_name.Name = "lblsubject_en_name";
this.lblsubject_en_name.Size = new System.Drawing.Size(41, 12);
this.lblsubject_en_name.TabIndex = 4;
this.lblsubject_en_name.Text = "科目名称";
this.txtsubject_en_name.Location = new System.Drawing.Point(173,96);
this.txtsubject_en_name.Name = "txtsubject_en_name";
this.txtsubject_en_name.Size = new System.Drawing.Size(100, 21);
this.txtsubject_en_name.TabIndex = 4;
this.Controls.Add(this.lblsubject_en_name);
this.Controls.Add(this.txtsubject_en_name);

           //#####Subject_Type###Int32

           //#####Balance_direction###Boolean
this.lblBalance_direction.AutoSize = true;
this.lblBalance_direction.Location = new System.Drawing.Point(100,150);
this.lblBalance_direction.Name = "lblBalance_direction";
this.lblBalance_direction.Size = new System.Drawing.Size(41, 12);
this.lblBalance_direction.TabIndex = 6;
this.lblBalance_direction.Text = "余额方向";
this.chkBalance_direction.Location = new System.Drawing.Point(173,146);
this.chkBalance_direction.Name = "chkBalance_direction";
this.chkBalance_direction.Size = new System.Drawing.Size(100, 21);
this.chkBalance_direction.TabIndex = 6;
this.Controls.Add(this.lblBalance_direction);
this.Controls.Add(this.chkBalance_direction);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,175);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 7;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,171);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 7;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####EndDate###DateTime
this.lblEndDate.AutoSize = true;
this.lblEndDate.Location = new System.Drawing.Point(100,200);
this.lblEndDate.Name = "lblEndDate";
this.lblEndDate.Size = new System.Drawing.Size(41, 12);
this.lblEndDate.TabIndex = 8;
this.lblEndDate.Text = "离职日期";
//111======200
this.dtpEndDate.Location = new System.Drawing.Point(173,196);
this.dtpEndDate.Name ="dtpEndDate";
this.dtpEndDate.ShowCheckBox =true;
this.dtpEndDate.Size = new System.Drawing.Size(100, 21);
this.dtpEndDate.TabIndex = 8;
this.Controls.Add(this.lblEndDate);
this.Controls.Add(this.dtpEndDate);

           //#####Sort###Int32

           //#####2147483647Images###Binary

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,275);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 11;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,271);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 11;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblsubject_code );
this.Controls.Add(this.txtsubject_code );

                this.Controls.Add(this.lblsubject_name );
this.Controls.Add(this.txtsubject_name );

                this.Controls.Add(this.lblsubject_en_name );
this.Controls.Add(this.txtsubject_en_name );

                
                this.Controls.Add(this.lblBalance_direction );
this.Controls.Add(this.chkBalance_direction );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblEndDate );
this.Controls.Add(this.dtpEndDate );

                
                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_FM_SubjectQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblsubject_code;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtsubject_code;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblsubject_name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtsubject_name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblsubject_en_name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtsubject_en_name;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBalance_direction;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkBalance_direction;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEndDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEndDate;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


