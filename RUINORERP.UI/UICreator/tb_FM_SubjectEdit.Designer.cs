// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:36
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
    partial class tb_FM_SubjectEdit
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
     this.lblParent_subject_id = new Krypton.Toolkit.KryptonLabel();
this.txtParent_subject_id = new Krypton.Toolkit.KryptonTextBox();

this.lblsubject_code = new Krypton.Toolkit.KryptonLabel();
this.txtsubject_code = new Krypton.Toolkit.KryptonTextBox();

this.lblsubject_name = new Krypton.Toolkit.KryptonLabel();
this.txtsubject_name = new Krypton.Toolkit.KryptonTextBox();

this.lblsubject_en_name = new Krypton.Toolkit.KryptonLabel();
this.txtsubject_en_name = new Krypton.Toolkit.KryptonTextBox();

this.lblSubject_Type = new Krypton.Toolkit.KryptonLabel();
this.txtSubject_Type = new Krypton.Toolkit.KryptonTextBox();

this.lblBalance_direction = new Krypton.Toolkit.KryptonLabel();
this.chkBalance_direction = new Krypton.Toolkit.KryptonCheckBox();
this.chkBalance_direction.Values.Text ="";

this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();


this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    
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
     
            //#####Parent_subject_id###Int64
this.lblParent_subject_id.AutoSize = true;
this.lblParent_subject_id.Location = new System.Drawing.Point(100,25);
this.lblParent_subject_id.Name = "lblParent_subject_id";
this.lblParent_subject_id.Size = new System.Drawing.Size(41, 12);
this.lblParent_subject_id.TabIndex = 1;
this.lblParent_subject_id.Text = "";
this.txtParent_subject_id.Location = new System.Drawing.Point(173,21);
this.txtParent_subject_id.Name = "txtParent_subject_id";
this.txtParent_subject_id.Size = new System.Drawing.Size(100, 21);
this.txtParent_subject_id.TabIndex = 1;
this.Controls.Add(this.lblParent_subject_id);
this.Controls.Add(this.txtParent_subject_id);

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
this.lblSubject_Type.AutoSize = true;
this.lblSubject_Type.Location = new System.Drawing.Point(100,125);
this.lblSubject_Type.Name = "lblSubject_Type";
this.lblSubject_Type.Size = new System.Drawing.Size(41, 12);
this.lblSubject_Type.TabIndex = 5;
this.lblSubject_Type.Text = "科目类型";
this.txtSubject_Type.Location = new System.Drawing.Point(173,121);
this.txtSubject_Type.Name = "txtSubject_Type";
this.txtSubject_Type.Size = new System.Drawing.Size(100, 21);
this.txtSubject_Type.TabIndex = 5;
this.Controls.Add(this.lblSubject_Type);
this.Controls.Add(this.txtSubject_Type);

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

           //#####Sort###Int32
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,200);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 8;
this.lblSort.Text = "排序";
this.txtSort.Location = new System.Drawing.Point(173,196);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 8;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

           //#####2147483647Images###Binary

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

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,275);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 11;
this.lblCreated_at.Text = "创建时间";
//111======275
this.dtpCreated_at.Location = new System.Drawing.Point(173,271);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 11;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,300);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 12;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,296);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 12;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,325);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 13;
this.lblModified_at.Text = "修改时间";
//111======325
this.dtpModified_at.Location = new System.Drawing.Point(173,321);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 13;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,350);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 14;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,346);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 14;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,375);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 15;
this.lblisdeleted.Text = "";
this.chkisdeleted.Location = new System.Drawing.Point(173,371);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 15;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

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
           // this.kryptonPanel1.TabIndex = 15;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblParent_subject_id );
this.Controls.Add(this.txtParent_subject_id );

                this.Controls.Add(this.lblsubject_code );
this.Controls.Add(this.txtsubject_code );

                this.Controls.Add(this.lblsubject_name );
this.Controls.Add(this.txtsubject_name );

                this.Controls.Add(this.lblsubject_en_name );
this.Controls.Add(this.txtsubject_en_name );

                this.Controls.Add(this.lblSubject_Type );
this.Controls.Add(this.txtSubject_Type );

                this.Controls.Add(this.lblBalance_direction );
this.Controls.Add(this.chkBalance_direction );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                            // 
            // "tb_FM_SubjectEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_SubjectEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblParent_subject_id;
private Krypton.Toolkit.KryptonTextBox txtParent_subject_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblsubject_code;
private Krypton.Toolkit.KryptonTextBox txtsubject_code;

    
        
              private Krypton.Toolkit.KryptonLabel lblsubject_name;
private Krypton.Toolkit.KryptonTextBox txtsubject_name;

    
        
              private Krypton.Toolkit.KryptonLabel lblsubject_en_name;
private Krypton.Toolkit.KryptonTextBox txtsubject_en_name;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubject_Type;
private Krypton.Toolkit.KryptonTextBox txtSubject_Type;

    
        
              private Krypton.Toolkit.KryptonLabel lblBalance_direction;
private Krypton.Toolkit.KryptonCheckBox chkBalance_direction;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

