
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/29/2025 18:37:27
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 字段信息表
    /// </summary>
    partial class tb_FieldInfoQuery
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
     
     this.lblMenuID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMenuID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEntityName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEntityName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFieldName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFieldName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFieldText = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFieldText = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblClassPath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClassPath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtClassPath.Multiline = true;

this.lblIsForm = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsForm = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsForm.Values.Text ="";

this.lblDefaultHide = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkDefaultHide = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkDefaultHide.Values.Text ="";

this.lblReadOnly = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkReadOnly = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkReadOnly.Values.Text ="";

this.lblIsEnabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsEnabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsEnabled.Values.Text ="";
this.chkIsEnabled.Checked = true;
this.chkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsChild = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsChild = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsChild.Values.Text ="";

this.lblChildEntityName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtChildEntityName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####MenuID###Int64
//属性测试25MenuID
this.lblMenuID.AutoSize = true;
this.lblMenuID.Location = new System.Drawing.Point(100,25);
this.lblMenuID.Name = "lblMenuID";
this.lblMenuID.Size = new System.Drawing.Size(41, 12);
this.lblMenuID.TabIndex = 1;
this.lblMenuID.Text = "菜单";
//111======25
this.cmbMenuID.Location = new System.Drawing.Point(173,21);
this.cmbMenuID.Name ="cmbMenuID";
this.cmbMenuID.Size = new System.Drawing.Size(100, 21);
this.cmbMenuID.TabIndex = 1;
this.Controls.Add(this.lblMenuID);
this.Controls.Add(this.cmbMenuID);

           //#####100EntityName###String
this.lblEntityName.AutoSize = true;
this.lblEntityName.Location = new System.Drawing.Point(100,50);
this.lblEntityName.Name = "lblEntityName";
this.lblEntityName.Size = new System.Drawing.Size(41, 12);
this.lblEntityName.TabIndex = 2;
this.lblEntityName.Text = "实体名称";
this.txtEntityName.Location = new System.Drawing.Point(173,46);
this.txtEntityName.Name = "txtEntityName";
this.txtEntityName.Size = new System.Drawing.Size(100, 21);
this.txtEntityName.TabIndex = 2;
this.Controls.Add(this.lblEntityName);
this.Controls.Add(this.txtEntityName);

           //#####100FieldName###String
this.lblFieldName.AutoSize = true;
this.lblFieldName.Location = new System.Drawing.Point(100,75);
this.lblFieldName.Name = "lblFieldName";
this.lblFieldName.Size = new System.Drawing.Size(41, 12);
this.lblFieldName.TabIndex = 3;
this.lblFieldName.Text = "字段名称";
this.txtFieldName.Location = new System.Drawing.Point(173,71);
this.txtFieldName.Name = "txtFieldName";
this.txtFieldName.Size = new System.Drawing.Size(100, 21);
this.txtFieldName.TabIndex = 3;
this.Controls.Add(this.lblFieldName);
this.Controls.Add(this.txtFieldName);

           //#####100FieldText###String
this.lblFieldText.AutoSize = true;
this.lblFieldText.Location = new System.Drawing.Point(100,100);
this.lblFieldText.Name = "lblFieldText";
this.lblFieldText.Size = new System.Drawing.Size(41, 12);
this.lblFieldText.TabIndex = 4;
this.lblFieldText.Text = "字段显示";
this.txtFieldText.Location = new System.Drawing.Point(173,96);
this.txtFieldText.Name = "txtFieldText";
this.txtFieldText.Size = new System.Drawing.Size(100, 21);
this.txtFieldText.TabIndex = 4;
this.Controls.Add(this.lblFieldText);
this.Controls.Add(this.txtFieldText);

           //#####500ClassPath###String
this.lblClassPath.AutoSize = true;
this.lblClassPath.Location = new System.Drawing.Point(100,125);
this.lblClassPath.Name = "lblClassPath";
this.lblClassPath.Size = new System.Drawing.Size(41, 12);
this.lblClassPath.TabIndex = 5;
this.lblClassPath.Text = "类路径";
this.txtClassPath.Location = new System.Drawing.Point(173,121);
this.txtClassPath.Name = "txtClassPath";
this.txtClassPath.Size = new System.Drawing.Size(100, 21);
this.txtClassPath.TabIndex = 5;
this.Controls.Add(this.lblClassPath);
this.Controls.Add(this.txtClassPath);

           //#####IsForm###Boolean
this.lblIsForm.AutoSize = true;
this.lblIsForm.Location = new System.Drawing.Point(100,150);
this.lblIsForm.Name = "lblIsForm";
this.lblIsForm.Size = new System.Drawing.Size(41, 12);
this.lblIsForm.TabIndex = 6;
this.lblIsForm.Text = "是否为窗体";
this.chkIsForm.Location = new System.Drawing.Point(173,146);
this.chkIsForm.Name = "chkIsForm";
this.chkIsForm.Size = new System.Drawing.Size(100, 21);
this.chkIsForm.TabIndex = 6;
this.Controls.Add(this.lblIsForm);
this.Controls.Add(this.chkIsForm);

           //#####DefaultHide###Boolean
this.lblDefaultHide.AutoSize = true;
this.lblDefaultHide.Location = new System.Drawing.Point(100,175);
this.lblDefaultHide.Name = "lblDefaultHide";
this.lblDefaultHide.Size = new System.Drawing.Size(41, 12);
this.lblDefaultHide.TabIndex = 7;
this.lblDefaultHide.Text = "默认隐藏";
this.chkDefaultHide.Location = new System.Drawing.Point(173,171);
this.chkDefaultHide.Name = "chkDefaultHide";
this.chkDefaultHide.Size = new System.Drawing.Size(100, 21);
this.chkDefaultHide.TabIndex = 7;
this.Controls.Add(this.lblDefaultHide);
this.Controls.Add(this.chkDefaultHide);

           //#####ReadOnly###Boolean
this.lblReadOnly.AutoSize = true;
this.lblReadOnly.Location = new System.Drawing.Point(100,200);
this.lblReadOnly.Name = "lblReadOnly";
this.lblReadOnly.Size = new System.Drawing.Size(41, 12);
this.lblReadOnly.TabIndex = 8;
this.lblReadOnly.Text = "只读";
this.chkReadOnly.Location = new System.Drawing.Point(173,196);
this.chkReadOnly.Name = "chkReadOnly";
this.chkReadOnly.Size = new System.Drawing.Size(100, 21);
this.chkReadOnly.TabIndex = 8;
this.Controls.Add(this.lblReadOnly);
this.Controls.Add(this.chkReadOnly);

           //#####IsEnabled###Boolean
this.lblIsEnabled.AutoSize = true;
this.lblIsEnabled.Location = new System.Drawing.Point(100,225);
this.lblIsEnabled.Name = "lblIsEnabled";
this.lblIsEnabled.Size = new System.Drawing.Size(41, 12);
this.lblIsEnabled.TabIndex = 9;
this.lblIsEnabled.Text = "是否启用";
this.chkIsEnabled.Location = new System.Drawing.Point(173,221);
this.chkIsEnabled.Name = "chkIsEnabled";
this.chkIsEnabled.Size = new System.Drawing.Size(100, 21);
this.chkIsEnabled.TabIndex = 9;
this.Controls.Add(this.lblIsEnabled);
this.Controls.Add(this.chkIsEnabled);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,250);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 10;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,246);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 10;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####IsChild###Boolean
this.lblIsChild.AutoSize = true;
this.lblIsChild.Location = new System.Drawing.Point(100,275);
this.lblIsChild.Name = "lblIsChild";
this.lblIsChild.Size = new System.Drawing.Size(41, 12);
this.lblIsChild.TabIndex = 11;
this.lblIsChild.Text = "子表字段";
this.chkIsChild.Location = new System.Drawing.Point(173,271);
this.chkIsChild.Name = "chkIsChild";
this.chkIsChild.Size = new System.Drawing.Size(100, 21);
this.chkIsChild.TabIndex = 11;
this.Controls.Add(this.lblIsChild);
this.Controls.Add(this.chkIsChild);

           //#####100ChildEntityName###String
this.lblChildEntityName.AutoSize = true;
this.lblChildEntityName.Location = new System.Drawing.Point(100,300);
this.lblChildEntityName.Name = "lblChildEntityName";
this.lblChildEntityName.Size = new System.Drawing.Size(41, 12);
this.lblChildEntityName.TabIndex = 12;
this.lblChildEntityName.Text = "子表名称";
this.txtChildEntityName.Location = new System.Drawing.Point(173,296);
this.txtChildEntityName.Name = "txtChildEntityName";
this.txtChildEntityName.Size = new System.Drawing.Size(100, 21);
this.txtChildEntityName.TabIndex = 12;
this.Controls.Add(this.lblChildEntityName);
this.Controls.Add(this.txtChildEntityName);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,325);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 13;
this.lblCreated_at.Text = "创建时间";
//111======325
this.dtpCreated_at.Location = new System.Drawing.Point(173,321);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 13;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMenuID );
this.Controls.Add(this.cmbMenuID );

                this.Controls.Add(this.lblEntityName );
this.Controls.Add(this.txtEntityName );

                this.Controls.Add(this.lblFieldName );
this.Controls.Add(this.txtFieldName );

                this.Controls.Add(this.lblFieldText );
this.Controls.Add(this.txtFieldText );

                this.Controls.Add(this.lblClassPath );
this.Controls.Add(this.txtClassPath );

                this.Controls.Add(this.lblIsForm );
this.Controls.Add(this.chkIsForm );

                this.Controls.Add(this.lblDefaultHide );
this.Controls.Add(this.chkDefaultHide );

                this.Controls.Add(this.lblReadOnly );
this.Controls.Add(this.chkReadOnly );

                this.Controls.Add(this.lblIsEnabled );
this.Controls.Add(this.chkIsEnabled );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIsChild );
this.Controls.Add(this.chkIsChild );

                this.Controls.Add(this.lblChildEntityName );
this.Controls.Add(this.txtChildEntityName );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                    
            this.Name = "tb_FieldInfoQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMenuID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMenuID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEntityName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEntityName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFieldName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFieldName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFieldText;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFieldText;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClassPath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClassPath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsForm;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsForm;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefaultHide;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkDefaultHide;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReadOnly;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkReadOnly;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsEnabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsEnabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsChild;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsChild;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblChildEntityName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtChildEntityName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
    
   
 





    }
}


