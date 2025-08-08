
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:23
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收藏表 收藏订单 产品 库存报警等
    /// </summary>
    partial class tb_FavoriteQuery
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
     
     
this.lblRef_Table_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRef_Table_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblModuleName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModuleName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtModuleName.Multiline = true;

this.lblBusinessType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBusinessType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtBusinessType.Multiline = true;

this.lblPublic_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkPublic_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkPublic_enabled.Values.Text ="";

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
                 //#####ReferenceID###Int64

           //#####100Ref_Table_Name###String
this.lblRef_Table_Name.AutoSize = true;
this.lblRef_Table_Name.Location = new System.Drawing.Point(100,50);
this.lblRef_Table_Name.Name = "lblRef_Table_Name";
this.lblRef_Table_Name.Size = new System.Drawing.Size(41, 12);
this.lblRef_Table_Name.TabIndex = 2;
this.lblRef_Table_Name.Text = "引用表名";
this.txtRef_Table_Name.Location = new System.Drawing.Point(173,46);
this.txtRef_Table_Name.Name = "txtRef_Table_Name";
this.txtRef_Table_Name.Size = new System.Drawing.Size(100, 21);
this.txtRef_Table_Name.TabIndex = 2;
this.Controls.Add(this.lblRef_Table_Name);
this.Controls.Add(this.txtRef_Table_Name);

           //#####255ModuleName###String
this.lblModuleName.AutoSize = true;
this.lblModuleName.Location = new System.Drawing.Point(100,75);
this.lblModuleName.Name = "lblModuleName";
this.lblModuleName.Size = new System.Drawing.Size(41, 12);
this.lblModuleName.TabIndex = 3;
this.lblModuleName.Text = "模块名";
this.txtModuleName.Location = new System.Drawing.Point(173,71);
this.txtModuleName.Name = "txtModuleName";
this.txtModuleName.Size = new System.Drawing.Size(100, 21);
this.txtModuleName.TabIndex = 3;
this.Controls.Add(this.lblModuleName);
this.Controls.Add(this.txtModuleName);

           //#####255BusinessType###String
this.lblBusinessType.AutoSize = true;
this.lblBusinessType.Location = new System.Drawing.Point(100,100);
this.lblBusinessType.Name = "lblBusinessType";
this.lblBusinessType.Size = new System.Drawing.Size(41, 12);
this.lblBusinessType.TabIndex = 4;
this.lblBusinessType.Text = "业务类型";
this.txtBusinessType.Location = new System.Drawing.Point(173,96);
this.txtBusinessType.Name = "txtBusinessType";
this.txtBusinessType.Size = new System.Drawing.Size(100, 21);
this.txtBusinessType.TabIndex = 4;
this.Controls.Add(this.lblBusinessType);
this.Controls.Add(this.txtBusinessType);

           //#####Public_enabled###Boolean
this.lblPublic_enabled.AutoSize = true;
this.lblPublic_enabled.Location = new System.Drawing.Point(100,125);
this.lblPublic_enabled.Name = "lblPublic_enabled";
this.lblPublic_enabled.Size = new System.Drawing.Size(41, 12);
this.lblPublic_enabled.TabIndex = 5;
this.lblPublic_enabled.Text = "是否公开";
this.chkPublic_enabled.Location = new System.Drawing.Point(173,121);
this.chkPublic_enabled.Name = "chkPublic_enabled";
this.chkPublic_enabled.Size = new System.Drawing.Size(100, 21);
this.chkPublic_enabled.TabIndex = 5;
this.Controls.Add(this.lblPublic_enabled);
this.Controls.Add(this.chkPublic_enabled);

           //#####is_enabled###Boolean
this.lblis_enabled.AutoSize = true;
this.lblis_enabled.Location = new System.Drawing.Point(100,150);
this.lblis_enabled.Name = "lblis_enabled";
this.lblis_enabled.Size = new System.Drawing.Size(41, 12);
this.lblis_enabled.TabIndex = 6;
this.lblis_enabled.Text = "是否启用";
this.chkis_enabled.Location = new System.Drawing.Point(173,146);
this.chkis_enabled.Name = "chkis_enabled";
this.chkis_enabled.Size = new System.Drawing.Size(100, 21);
this.chkis_enabled.TabIndex = 6;
this.Controls.Add(this.lblis_enabled);
this.Controls.Add(this.chkis_enabled);

           //#####is_available###Boolean
this.lblis_available.AutoSize = true;
this.lblis_available.Location = new System.Drawing.Point(100,175);
this.lblis_available.Name = "lblis_available";
this.lblis_available.Size = new System.Drawing.Size(41, 12);
this.lblis_available.TabIndex = 7;
this.lblis_available.Text = "是否可用";
this.chkis_available.Location = new System.Drawing.Point(173,171);
this.chkis_available.Name = "chkis_available";
this.chkis_available.Size = new System.Drawing.Size(100, 21);
this.chkis_available.TabIndex = 7;
this.Controls.Add(this.lblis_available);
this.Controls.Add(this.chkis_available);

           //#####500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,200);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 8;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,196);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 8;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,225);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 9;
this.lblCreated_at.Text = "创建时间";
//111======225
this.dtpCreated_at.Location = new System.Drawing.Point(173,221);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 9;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Owner_by###Int64

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,300);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 12;
this.lblModified_at.Text = "修改时间";
//111======300
this.dtpModified_at.Location = new System.Drawing.Point(173,296);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 12;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblRef_Table_Name );
this.Controls.Add(this.txtRef_Table_Name );

                this.Controls.Add(this.lblModuleName );
this.Controls.Add(this.txtModuleName );

                this.Controls.Add(this.lblBusinessType );
this.Controls.Add(this.txtBusinessType );

                this.Controls.Add(this.lblPublic_enabled );
this.Controls.Add(this.chkPublic_enabled );

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

                
                    
            this.Name = "tb_FavoriteQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRef_Table_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRef_Table_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModuleName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModuleName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBusinessType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBusinessType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPublic_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkPublic_enabled;

    
        
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


