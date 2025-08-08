
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:14
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识
    /// </summary>
    partial class tb_BOMConfigHistoryQuery
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
     
     this.lblVerNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVerNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEffective_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEffective_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblis_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkis_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkis_enabled.Values.Text ="";

this.lblis_available = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkis_available = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkis_available.Values.Text ="";

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50VerNo###String
this.lblVerNo.AutoSize = true;
this.lblVerNo.Location = new System.Drawing.Point(100,25);
this.lblVerNo.Name = "lblVerNo";
this.lblVerNo.Size = new System.Drawing.Size(41, 12);
this.lblVerNo.TabIndex = 1;
this.lblVerNo.Text = "版本号";
this.txtVerNo.Location = new System.Drawing.Point(173,21);
this.txtVerNo.Name = "txtVerNo";
this.txtVerNo.Size = new System.Drawing.Size(100, 21);
this.txtVerNo.TabIndex = 1;
this.Controls.Add(this.lblVerNo);
this.Controls.Add(this.txtVerNo);

           //#####Effective_at###DateTime
this.lblEffective_at.AutoSize = true;
this.lblEffective_at.Location = new System.Drawing.Point(100,50);
this.lblEffective_at.Name = "lblEffective_at";
this.lblEffective_at.Size = new System.Drawing.Size(41, 12);
this.lblEffective_at.TabIndex = 2;
this.lblEffective_at.Text = "生效时间";
//111======50
this.dtpEffective_at.Location = new System.Drawing.Point(173,46);
this.dtpEffective_at.Name ="dtpEffective_at";
this.dtpEffective_at.ShowCheckBox =true;
this.dtpEffective_at.Size = new System.Drawing.Size(100, 21);
this.dtpEffective_at.TabIndex = 2;
this.Controls.Add(this.lblEffective_at);
this.Controls.Add(this.dtpEffective_at);

           //#####is_enabled###Boolean
this.lblis_enabled.AutoSize = true;
this.lblis_enabled.Location = new System.Drawing.Point(100,75);
this.lblis_enabled.Name = "lblis_enabled";
this.lblis_enabled.Size = new System.Drawing.Size(41, 12);
this.lblis_enabled.TabIndex = 3;
this.lblis_enabled.Text = "是否启用";
this.chkis_enabled.Location = new System.Drawing.Point(173,71);
this.chkis_enabled.Name = "chkis_enabled";
this.chkis_enabled.Size = new System.Drawing.Size(100, 21);
this.chkis_enabled.TabIndex = 3;
this.Controls.Add(this.lblis_enabled);
this.Controls.Add(this.chkis_enabled);

           //#####is_available###Boolean
this.lblis_available.AutoSize = true;
this.lblis_available.Location = new System.Drawing.Point(100,100);
this.lblis_available.Name = "lblis_available";
this.lblis_available.Size = new System.Drawing.Size(41, 12);
this.lblis_available.TabIndex = 4;
this.lblis_available.Text = "是否可用";
this.chkis_available.Location = new System.Drawing.Point(173,96);
this.chkis_available.Name = "chkis_available";
this.chkis_available.Size = new System.Drawing.Size(100, 21);
this.chkis_available.TabIndex = 4;
this.Controls.Add(this.lblis_available);
this.Controls.Add(this.chkis_available);

           //#####500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,125);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 5;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,121);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 5;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "创建时间";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,200);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 8;
this.lblModified_at.Text = "修改时间";
//111======200
this.dtpModified_at.Location = new System.Drawing.Point(173,196);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 8;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblVerNo );
this.Controls.Add(this.txtVerNo );

                this.Controls.Add(this.lblEffective_at );
this.Controls.Add(this.dtpEffective_at );

                this.Controls.Add(this.lblis_enabled );
this.Controls.Add(this.chkis_enabled );

                this.Controls.Add(this.lblis_available );
this.Controls.Add(this.chkis_available );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_BOMConfigHistoryQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVerNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVerNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEffective_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEffective_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblis_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkis_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblis_available;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkis_available;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


