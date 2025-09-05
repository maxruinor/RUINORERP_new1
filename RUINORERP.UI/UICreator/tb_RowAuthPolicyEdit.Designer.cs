// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/04/2025 14:48:20
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 行级权限规则
    /// </summary>
    partial class tb_RowAuthPolicyEdit
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
     this.lblPolicyName = new Krypton.Toolkit.KryptonLabel();
this.txtPolicyName = new Krypton.Toolkit.KryptonTextBox();

this.lblTargetTable = new Krypton.Toolkit.KryptonLabel();
this.txtTargetTable = new Krypton.Toolkit.KryptonTextBox();

this.lblTargetEntity = new Krypton.Toolkit.KryptonLabel();
this.txtTargetEntity = new Krypton.Toolkit.KryptonTextBox();

this.lblIsJoinRequired = new Krypton.Toolkit.KryptonLabel();
this.chkIsJoinRequired = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsJoinRequired.Values.Text ="";

this.lblTargetTableJoinField = new Krypton.Toolkit.KryptonLabel();
this.txtTargetTableJoinField = new Krypton.Toolkit.KryptonTextBox();

this.lblJoinTableJoinField = new Krypton.Toolkit.KryptonLabel();
this.txtJoinTableJoinField = new Krypton.Toolkit.KryptonTextBox();

this.lblJoinTable = new Krypton.Toolkit.KryptonLabel();
this.txtJoinTable = new Krypton.Toolkit.KryptonTextBox();

this.lblJoinType = new Krypton.Toolkit.KryptonLabel();
this.txtJoinType = new Krypton.Toolkit.KryptonTextBox();

this.lblJoinOnClause = new Krypton.Toolkit.KryptonLabel();
this.txtJoinOnClause = new Krypton.Toolkit.KryptonTextBox();
this.txtJoinOnClause.Multiline = true;

this.lblFilterClause = new Krypton.Toolkit.KryptonLabel();
this.txtFilterClause = new Krypton.Toolkit.KryptonTextBox();
this.txtFilterClause.Multiline = true;

this.lblEntityType = new Krypton.Toolkit.KryptonLabel();
this.txtEntityType = new Krypton.Toolkit.KryptonTextBox();

this.lblIsEnabled = new Krypton.Toolkit.KryptonLabel();
this.chkIsEnabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsEnabled.Values.Text ="";
this.chkIsEnabled.Checked = true;
this.chkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblPolicyDescription = new Krypton.Toolkit.KryptonLabel();
this.txtPolicyDescription = new Krypton.Toolkit.KryptonTextBox();
this.txtPolicyDescription.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblDefaultRuleEnum = new Krypton.Toolkit.KryptonLabel();
this.txtDefaultRuleEnum = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####100PolicyName###String
this.lblPolicyName.AutoSize = true;
this.lblPolicyName.Location = new System.Drawing.Point(100,25);
this.lblPolicyName.Name = "lblPolicyName";
this.lblPolicyName.Size = new System.Drawing.Size(41, 12);
this.lblPolicyName.TabIndex = 1;
this.lblPolicyName.Text = "规则名称";
this.txtPolicyName.Location = new System.Drawing.Point(173,21);
this.txtPolicyName.Name = "txtPolicyName";
this.txtPolicyName.Size = new System.Drawing.Size(100, 21);
this.txtPolicyName.TabIndex = 1;
this.Controls.Add(this.lblPolicyName);
this.Controls.Add(this.txtPolicyName);

           //#####100TargetTable###String
this.lblTargetTable.AutoSize = true;
this.lblTargetTable.Location = new System.Drawing.Point(100,50);
this.lblTargetTable.Name = "lblTargetTable";
this.lblTargetTable.Size = new System.Drawing.Size(41, 12);
this.lblTargetTable.TabIndex = 2;
this.lblTargetTable.Text = "查询主表";
this.txtTargetTable.Location = new System.Drawing.Point(173,46);
this.txtTargetTable.Name = "txtTargetTable";
this.txtTargetTable.Size = new System.Drawing.Size(100, 21);
this.txtTargetTable.TabIndex = 2;
this.Controls.Add(this.lblTargetTable);
this.Controls.Add(this.txtTargetTable);

           //#####100TargetEntity###String
this.lblTargetEntity.AutoSize = true;
this.lblTargetEntity.Location = new System.Drawing.Point(100,75);
this.lblTargetEntity.Name = "lblTargetEntity";
this.lblTargetEntity.Size = new System.Drawing.Size(41, 12);
this.lblTargetEntity.TabIndex = 3;
this.lblTargetEntity.Text = "查询实体";
this.txtTargetEntity.Location = new System.Drawing.Point(173,71);
this.txtTargetEntity.Name = "txtTargetEntity";
this.txtTargetEntity.Size = new System.Drawing.Size(100, 21);
this.txtTargetEntity.TabIndex = 3;
this.Controls.Add(this.lblTargetEntity);
this.Controls.Add(this.txtTargetEntity);

           //#####IsJoinRequired###Boolean
this.lblIsJoinRequired.AutoSize = true;
this.lblIsJoinRequired.Location = new System.Drawing.Point(100,100);
this.lblIsJoinRequired.Name = "lblIsJoinRequired";
this.lblIsJoinRequired.Size = new System.Drawing.Size(41, 12);
this.lblIsJoinRequired.TabIndex = 4;
this.lblIsJoinRequired.Text = "是否需要联表";
this.chkIsJoinRequired.Location = new System.Drawing.Point(173,96);
this.chkIsJoinRequired.Name = "chkIsJoinRequired";
this.chkIsJoinRequired.Size = new System.Drawing.Size(100, 21);
this.chkIsJoinRequired.TabIndex = 4;
this.Controls.Add(this.lblIsJoinRequired);
this.Controls.Add(this.chkIsJoinRequired);

           //#####50TargetTableJoinField###String
this.lblTargetTableJoinField.AutoSize = true;
this.lblTargetTableJoinField.Location = new System.Drawing.Point(100,125);
this.lblTargetTableJoinField.Name = "lblTargetTableJoinField";
this.lblTargetTableJoinField.Size = new System.Drawing.Size(41, 12);
this.lblTargetTableJoinField.TabIndex = 5;
this.lblTargetTableJoinField.Text = "目标表关联字段";
this.txtTargetTableJoinField.Location = new System.Drawing.Point(173,121);
this.txtTargetTableJoinField.Name = "txtTargetTableJoinField";
this.txtTargetTableJoinField.Size = new System.Drawing.Size(100, 21);
this.txtTargetTableJoinField.TabIndex = 5;
this.Controls.Add(this.lblTargetTableJoinField);
this.Controls.Add(this.txtTargetTableJoinField);

           //#####50JoinTableJoinField###String
this.lblJoinTableJoinField.AutoSize = true;
this.lblJoinTableJoinField.Location = new System.Drawing.Point(100,150);
this.lblJoinTableJoinField.Name = "lblJoinTableJoinField";
this.lblJoinTableJoinField.Size = new System.Drawing.Size(41, 12);
this.lblJoinTableJoinField.TabIndex = 6;
this.lblJoinTableJoinField.Text = "关联表关联字段";
this.txtJoinTableJoinField.Location = new System.Drawing.Point(173,146);
this.txtJoinTableJoinField.Name = "txtJoinTableJoinField";
this.txtJoinTableJoinField.Size = new System.Drawing.Size(100, 21);
this.txtJoinTableJoinField.TabIndex = 6;
this.Controls.Add(this.lblJoinTableJoinField);
this.Controls.Add(this.txtJoinTableJoinField);

           //#####100JoinTable###String
this.lblJoinTable.AutoSize = true;
this.lblJoinTable.Location = new System.Drawing.Point(100,175);
this.lblJoinTable.Name = "lblJoinTable";
this.lblJoinTable.Size = new System.Drawing.Size(41, 12);
this.lblJoinTable.TabIndex = 7;
this.lblJoinTable.Text = "需要关联的表名";
this.txtJoinTable.Location = new System.Drawing.Point(173,171);
this.txtJoinTable.Name = "txtJoinTable";
this.txtJoinTable.Size = new System.Drawing.Size(100, 21);
this.txtJoinTable.TabIndex = 7;
this.Controls.Add(this.lblJoinTable);
this.Controls.Add(this.txtJoinTable);

           //#####10JoinType###String
this.lblJoinType.AutoSize = true;
this.lblJoinType.Location = new System.Drawing.Point(100,200);
this.lblJoinType.Name = "lblJoinType";
this.lblJoinType.Size = new System.Drawing.Size(41, 12);
this.lblJoinType.TabIndex = 8;
this.lblJoinType.Text = "关联类型";
this.txtJoinType.Location = new System.Drawing.Point(173,196);
this.txtJoinType.Name = "txtJoinType";
this.txtJoinType.Size = new System.Drawing.Size(100, 21);
this.txtJoinType.TabIndex = 8;
this.Controls.Add(this.lblJoinType);
this.Controls.Add(this.txtJoinType);

           //#####500JoinOnClause###String
this.lblJoinOnClause.AutoSize = true;
this.lblJoinOnClause.Location = new System.Drawing.Point(100,225);
this.lblJoinOnClause.Name = "lblJoinOnClause";
this.lblJoinOnClause.Size = new System.Drawing.Size(41, 12);
this.lblJoinOnClause.TabIndex = 9;
this.lblJoinOnClause.Text = "关联条件";
this.txtJoinOnClause.Location = new System.Drawing.Point(173,221);
this.txtJoinOnClause.Name = "txtJoinOnClause";
this.txtJoinOnClause.Size = new System.Drawing.Size(100, 21);
this.txtJoinOnClause.TabIndex = 9;
this.Controls.Add(this.lblJoinOnClause);
this.Controls.Add(this.txtJoinOnClause);

           //#####1000FilterClause###String
this.lblFilterClause.AutoSize = true;
this.lblFilterClause.Location = new System.Drawing.Point(100,250);
this.lblFilterClause.Name = "lblFilterClause";
this.lblFilterClause.Size = new System.Drawing.Size(41, 12);
this.lblFilterClause.TabIndex = 10;
this.lblFilterClause.Text = "过滤条件";
this.txtFilterClause.Location = new System.Drawing.Point(173,246);
this.txtFilterClause.Name = "txtFilterClause";
this.txtFilterClause.Size = new System.Drawing.Size(100, 21);
this.txtFilterClause.TabIndex = 10;
this.Controls.Add(this.lblFilterClause);
this.Controls.Add(this.txtFilterClause);

           //#####200EntityType###String
this.lblEntityType.AutoSize = true;
this.lblEntityType.Location = new System.Drawing.Point(100,275);
this.lblEntityType.Name = "lblEntityType";
this.lblEntityType.Size = new System.Drawing.Size(41, 12);
this.lblEntityType.TabIndex = 11;
this.lblEntityType.Text = "实体的全限定类名";
this.txtEntityType.Location = new System.Drawing.Point(173,271);
this.txtEntityType.Name = "txtEntityType";
this.txtEntityType.Size = new System.Drawing.Size(100, 21);
this.txtEntityType.TabIndex = 11;
this.Controls.Add(this.lblEntityType);
this.Controls.Add(this.txtEntityType);

           //#####IsEnabled###Boolean
this.lblIsEnabled.AutoSize = true;
this.lblIsEnabled.Location = new System.Drawing.Point(100,300);
this.lblIsEnabled.Name = "lblIsEnabled";
this.lblIsEnabled.Size = new System.Drawing.Size(41, 12);
this.lblIsEnabled.TabIndex = 12;
this.lblIsEnabled.Text = "是否启用";
this.chkIsEnabled.Location = new System.Drawing.Point(173,296);
this.chkIsEnabled.Name = "chkIsEnabled";
this.chkIsEnabled.Size = new System.Drawing.Size(100, 21);
this.chkIsEnabled.TabIndex = 12;
this.Controls.Add(this.lblIsEnabled);
this.Controls.Add(this.chkIsEnabled);

           //#####500PolicyDescription###String
this.lblPolicyDescription.AutoSize = true;
this.lblPolicyDescription.Location = new System.Drawing.Point(100,325);
this.lblPolicyDescription.Name = "lblPolicyDescription";
this.lblPolicyDescription.Size = new System.Drawing.Size(41, 12);
this.lblPolicyDescription.TabIndex = 13;
this.lblPolicyDescription.Text = "规则描述";
this.txtPolicyDescription.Location = new System.Drawing.Point(173,321);
this.txtPolicyDescription.Name = "txtPolicyDescription";
this.txtPolicyDescription.Size = new System.Drawing.Size(100, 21);
this.txtPolicyDescription.TabIndex = 13;
this.Controls.Add(this.lblPolicyDescription);
this.Controls.Add(this.txtPolicyDescription);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,350);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 14;
this.lblCreated_at.Text = "创建时间";
//111======350
this.dtpCreated_at.Location = new System.Drawing.Point(173,346);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 14;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,375);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 15;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,371);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 15;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,400);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 16;
this.lblModified_at.Text = "修改时间";
//111======400
this.dtpModified_at.Location = new System.Drawing.Point(173,396);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 16;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,425);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 17;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,421);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 17;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####DefaultRuleEnum###Int32
this.lblDefaultRuleEnum.AutoSize = true;
this.lblDefaultRuleEnum.Location = new System.Drawing.Point(100,450);
this.lblDefaultRuleEnum.Name = "lblDefaultRuleEnum";
this.lblDefaultRuleEnum.Size = new System.Drawing.Size(41, 12);
this.lblDefaultRuleEnum.TabIndex = 18;
this.lblDefaultRuleEnum.Text = "默认规则";
this.txtDefaultRuleEnum.Location = new System.Drawing.Point(173,446);
this.txtDefaultRuleEnum.Name = "txtDefaultRuleEnum";
this.txtDefaultRuleEnum.Size = new System.Drawing.Size(100, 21);
this.txtDefaultRuleEnum.TabIndex = 18;
this.Controls.Add(this.lblDefaultRuleEnum);
this.Controls.Add(this.txtDefaultRuleEnum);

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
           // this.kryptonPanel1.TabIndex = 18;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPolicyName );
this.Controls.Add(this.txtPolicyName );

                this.Controls.Add(this.lblTargetTable );
this.Controls.Add(this.txtTargetTable );

                this.Controls.Add(this.lblTargetEntity );
this.Controls.Add(this.txtTargetEntity );

                this.Controls.Add(this.lblIsJoinRequired );
this.Controls.Add(this.chkIsJoinRequired );

                this.Controls.Add(this.lblTargetTableJoinField );
this.Controls.Add(this.txtTargetTableJoinField );

                this.Controls.Add(this.lblJoinTableJoinField );
this.Controls.Add(this.txtJoinTableJoinField );

                this.Controls.Add(this.lblJoinTable );
this.Controls.Add(this.txtJoinTable );

                this.Controls.Add(this.lblJoinType );
this.Controls.Add(this.txtJoinType );

                this.Controls.Add(this.lblJoinOnClause );
this.Controls.Add(this.txtJoinOnClause );

                this.Controls.Add(this.lblFilterClause );
this.Controls.Add(this.txtFilterClause );

                this.Controls.Add(this.lblEntityType );
this.Controls.Add(this.txtEntityType );

                this.Controls.Add(this.lblIsEnabled );
this.Controls.Add(this.chkIsEnabled );

                this.Controls.Add(this.lblPolicyDescription );
this.Controls.Add(this.txtPolicyDescription );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblDefaultRuleEnum );
this.Controls.Add(this.txtDefaultRuleEnum );

                            // 
            // "tb_RowAuthPolicyEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_RowAuthPolicyEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPolicyName;
private Krypton.Toolkit.KryptonTextBox txtPolicyName;

    
        
              private Krypton.Toolkit.KryptonLabel lblTargetTable;
private Krypton.Toolkit.KryptonTextBox txtTargetTable;

    
        
              private Krypton.Toolkit.KryptonLabel lblTargetEntity;
private Krypton.Toolkit.KryptonTextBox txtTargetEntity;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsJoinRequired;
private Krypton.Toolkit.KryptonCheckBox chkIsJoinRequired;

    
        
              private Krypton.Toolkit.KryptonLabel lblTargetTableJoinField;
private Krypton.Toolkit.KryptonTextBox txtTargetTableJoinField;

    
        
              private Krypton.Toolkit.KryptonLabel lblJoinTableJoinField;
private Krypton.Toolkit.KryptonTextBox txtJoinTableJoinField;

    
        
              private Krypton.Toolkit.KryptonLabel lblJoinTable;
private Krypton.Toolkit.KryptonTextBox txtJoinTable;

    
        
              private Krypton.Toolkit.KryptonLabel lblJoinType;
private Krypton.Toolkit.KryptonTextBox txtJoinType;

    
        
              private Krypton.Toolkit.KryptonLabel lblJoinOnClause;
private Krypton.Toolkit.KryptonTextBox txtJoinOnClause;

    
        
              private Krypton.Toolkit.KryptonLabel lblFilterClause;
private Krypton.Toolkit.KryptonTextBox txtFilterClause;

    
        
              private Krypton.Toolkit.KryptonLabel lblEntityType;
private Krypton.Toolkit.KryptonTextBox txtEntityType;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsEnabled;
private Krypton.Toolkit.KryptonCheckBox chkIsEnabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblPolicyDescription;
private Krypton.Toolkit.KryptonTextBox txtPolicyDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblDefaultRuleEnum;
private Krypton.Toolkit.KryptonTextBox txtDefaultRuleEnum;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

