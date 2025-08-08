// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:57
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品属性值表
    /// </summary>
    partial class tb_ProdPropertyValueEdit
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
     this.lblProperty_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProperty_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPropertyValueName = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyValueName = new Krypton.Toolkit.KryptonTextBox();

this.lblPropertyValueDesc = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyValueDesc = new Krypton.Toolkit.KryptonTextBox();

this.lblSortOrder = new Krypton.Toolkit.KryptonLabel();
this.txtSortOrder = new Krypton.Toolkit.KryptonTextBox();

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

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####Property_ID###Int64
//属性测试25Property_ID
this.lblProperty_ID.AutoSize = true;
this.lblProperty_ID.Location = new System.Drawing.Point(100,25);
this.lblProperty_ID.Name = "lblProperty_ID";
this.lblProperty_ID.Size = new System.Drawing.Size(41, 12);
this.lblProperty_ID.TabIndex = 1;
this.lblProperty_ID.Text = "属性";
//111======25
this.cmbProperty_ID.Location = new System.Drawing.Point(173,21);
this.cmbProperty_ID.Name ="cmbProperty_ID";
this.cmbProperty_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProperty_ID.TabIndex = 1;
this.Controls.Add(this.lblProperty_ID);
this.Controls.Add(this.cmbProperty_ID);

           //#####20PropertyValueName###String
this.lblPropertyValueName.AutoSize = true;
this.lblPropertyValueName.Location = new System.Drawing.Point(100,50);
this.lblPropertyValueName.Name = "lblPropertyValueName";
this.lblPropertyValueName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyValueName.TabIndex = 2;
this.lblPropertyValueName.Text = "属性值名称";
this.txtPropertyValueName.Location = new System.Drawing.Point(173,46);
this.txtPropertyValueName.Name = "txtPropertyValueName";
this.txtPropertyValueName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyValueName.TabIndex = 2;
this.Controls.Add(this.lblPropertyValueName);
this.Controls.Add(this.txtPropertyValueName);

           //#####50PropertyValueDesc###String
this.lblPropertyValueDesc.AutoSize = true;
this.lblPropertyValueDesc.Location = new System.Drawing.Point(100,75);
this.lblPropertyValueDesc.Name = "lblPropertyValueDesc";
this.lblPropertyValueDesc.Size = new System.Drawing.Size(41, 12);
this.lblPropertyValueDesc.TabIndex = 3;
this.lblPropertyValueDesc.Text = "属性值描述";
this.txtPropertyValueDesc.Location = new System.Drawing.Point(173,71);
this.txtPropertyValueDesc.Name = "txtPropertyValueDesc";
this.txtPropertyValueDesc.Size = new System.Drawing.Size(100, 21);
this.txtPropertyValueDesc.TabIndex = 3;
this.Controls.Add(this.lblPropertyValueDesc);
this.Controls.Add(this.txtPropertyValueDesc);

           //#####SortOrder###Int32
//属性测试100SortOrder
this.lblSortOrder.AutoSize = true;
this.lblSortOrder.Location = new System.Drawing.Point(100,100);
this.lblSortOrder.Name = "lblSortOrder";
this.lblSortOrder.Size = new System.Drawing.Size(41, 12);
this.lblSortOrder.TabIndex = 4;
this.lblSortOrder.Text = "排序";
this.txtSortOrder.Location = new System.Drawing.Point(173,96);
this.txtSortOrder.Name = "txtSortOrder";
this.txtSortOrder.Size = new System.Drawing.Size(100, 21);
this.txtSortOrder.TabIndex = 4;
this.Controls.Add(this.lblSortOrder);
this.Controls.Add(this.txtSortOrder);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,125);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 5;
this.lblCreated_at.Text = "创建时间";
//111======125
this.dtpCreated_at.Location = new System.Drawing.Point(173,121);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 5;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试150Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,150);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 6;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,146);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 6;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,175);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 7;
this.lblModified_at.Text = "修改时间";
//111======175
this.dtpModified_at.Location = new System.Drawing.Point(173,171);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 7;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试200Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,200);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 8;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,196);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 8;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,225);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 9;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,221);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 9;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试250DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,250);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 10;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,246);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 10;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

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
           // this.kryptonPanel1.TabIndex = 10;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProperty_ID );
this.Controls.Add(this.cmbProperty_ID );

                this.Controls.Add(this.lblPropertyValueName );
this.Controls.Add(this.txtPropertyValueName );

                this.Controls.Add(this.lblPropertyValueDesc );
this.Controls.Add(this.txtPropertyValueDesc );

                this.Controls.Add(this.lblSortOrder );
this.Controls.Add(this.txtSortOrder );

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

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                            // 
            // "tb_ProdPropertyValueEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProdPropertyValueEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProperty_ID;
private Krypton.Toolkit.KryptonComboBox cmbProperty_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPropertyValueName;
private Krypton.Toolkit.KryptonTextBox txtPropertyValueName;

    
        
              private Krypton.Toolkit.KryptonLabel lblPropertyValueDesc;
private Krypton.Toolkit.KryptonTextBox txtPropertyValueDesc;

    
        
              private Krypton.Toolkit.KryptonLabel lblSortOrder;
private Krypton.Toolkit.KryptonTextBox txtSortOrder;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

