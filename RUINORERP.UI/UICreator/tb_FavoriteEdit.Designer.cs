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
    partial class tb_FavoriteEdit
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
     this.lblReferenceID = new Krypton.Toolkit.KryptonLabel();
this.txtReferenceID = new Krypton.Toolkit.KryptonTextBox();

this.lblRef_Table_Name = new Krypton.Toolkit.KryptonLabel();
this.txtRef_Table_Name = new Krypton.Toolkit.KryptonTextBox();

this.lblModuleName = new Krypton.Toolkit.KryptonLabel();
this.txtModuleName = new Krypton.Toolkit.KryptonTextBox();
this.txtModuleName.Multiline = true;

this.lblBusinessType = new Krypton.Toolkit.KryptonLabel();
this.txtBusinessType = new Krypton.Toolkit.KryptonTextBox();
this.txtBusinessType.Multiline = true;

this.lblPublic_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkPublic_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkPublic_enabled.Values.Text ="";

this.lblis_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkis_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkis_enabled.Values.Text ="";

this.lblis_available = new Krypton.Toolkit.KryptonLabel();
this.chkis_available = new Krypton.Toolkit.KryptonCheckBox();
this.chkis_available.Values.Text ="";

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblOwner_by = new Krypton.Toolkit.KryptonLabel();
this.txtOwner_by = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####ReferenceID###Int64
this.lblReferenceID.AutoSize = true;
this.lblReferenceID.Location = new System.Drawing.Point(100,25);
this.lblReferenceID.Name = "lblReferenceID";
this.lblReferenceID.Size = new System.Drawing.Size(41, 12);
this.lblReferenceID.TabIndex = 1;
this.lblReferenceID.Text = "引用ID";
this.txtReferenceID.Location = new System.Drawing.Point(173,21);
this.txtReferenceID.Name = "txtReferenceID";
this.txtReferenceID.Size = new System.Drawing.Size(100, 21);
this.txtReferenceID.TabIndex = 1;
this.Controls.Add(this.lblReferenceID);
this.Controls.Add(this.txtReferenceID);

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
this.lblOwner_by.AutoSize = true;
this.lblOwner_by.Location = new System.Drawing.Point(100,250);
this.lblOwner_by.Name = "lblOwner_by";
this.lblOwner_by.Size = new System.Drawing.Size(41, 12);
this.lblOwner_by.TabIndex = 10;
this.lblOwner_by.Text = "创建人";
this.txtOwner_by.Location = new System.Drawing.Point(173,246);
this.txtOwner_by.Name = "txtOwner_by";
this.txtOwner_by.Size = new System.Drawing.Size(100, 21);
this.txtOwner_by.TabIndex = 10;
this.Controls.Add(this.lblOwner_by);
this.Controls.Add(this.txtOwner_by);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,275);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 11;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,271);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 11;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,325);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 13;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,321);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 13;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 13;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblReferenceID );
this.Controls.Add(this.txtReferenceID );

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

                this.Controls.Add(this.lblOwner_by );
this.Controls.Add(this.txtOwner_by );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_FavoriteEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FavoriteEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblReferenceID;
private Krypton.Toolkit.KryptonTextBox txtReferenceID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRef_Table_Name;
private Krypton.Toolkit.KryptonTextBox txtRef_Table_Name;

    
        
              private Krypton.Toolkit.KryptonLabel lblModuleName;
private Krypton.Toolkit.KryptonTextBox txtModuleName;

    
        
              private Krypton.Toolkit.KryptonLabel lblBusinessType;
private Krypton.Toolkit.KryptonTextBox txtBusinessType;

    
        
              private Krypton.Toolkit.KryptonLabel lblPublic_enabled;
private Krypton.Toolkit.KryptonCheckBox chkPublic_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblis_enabled;
private Krypton.Toolkit.KryptonCheckBox chkis_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblis_available;
private Krypton.Toolkit.KryptonCheckBox chkis_available;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblOwner_by;
private Krypton.Toolkit.KryptonTextBox txtOwner_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

