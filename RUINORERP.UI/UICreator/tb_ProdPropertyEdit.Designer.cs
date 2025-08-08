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
    /// 产品属性表
    /// </summary>
    partial class tb_ProdPropertyEdit
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
     this.lblPropertyName = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyName = new Krypton.Toolkit.KryptonTextBox();

this.lblPropertyDesc = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyDesc = new Krypton.Toolkit.KryptonTextBox();

this.lblSortOrder = new Krypton.Toolkit.KryptonLabel();
this.txtSortOrder = new Krypton.Toolkit.KryptonTextBox();

this.lblInputType = new Krypton.Toolkit.KryptonLabel();
this.txtInputType = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####20PropertyName###String
this.lblPropertyName.AutoSize = true;
this.lblPropertyName.Location = new System.Drawing.Point(100,25);
this.lblPropertyName.Name = "lblPropertyName";
this.lblPropertyName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyName.TabIndex = 1;
this.lblPropertyName.Text = "属性名称";
this.txtPropertyName.Location = new System.Drawing.Point(173,21);
this.txtPropertyName.Name = "txtPropertyName";
this.txtPropertyName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyName.TabIndex = 1;
this.Controls.Add(this.lblPropertyName);
this.Controls.Add(this.txtPropertyName);

           //#####50PropertyDesc###String
this.lblPropertyDesc.AutoSize = true;
this.lblPropertyDesc.Location = new System.Drawing.Point(100,50);
this.lblPropertyDesc.Name = "lblPropertyDesc";
this.lblPropertyDesc.Size = new System.Drawing.Size(41, 12);
this.lblPropertyDesc.TabIndex = 2;
this.lblPropertyDesc.Text = "属性描述";
this.txtPropertyDesc.Location = new System.Drawing.Point(173,46);
this.txtPropertyDesc.Name = "txtPropertyDesc";
this.txtPropertyDesc.Size = new System.Drawing.Size(100, 21);
this.txtPropertyDesc.TabIndex = 2;
this.Controls.Add(this.lblPropertyDesc);
this.Controls.Add(this.txtPropertyDesc);

           //#####SortOrder###Int32
this.lblSortOrder.AutoSize = true;
this.lblSortOrder.Location = new System.Drawing.Point(100,75);
this.lblSortOrder.Name = "lblSortOrder";
this.lblSortOrder.Size = new System.Drawing.Size(41, 12);
this.lblSortOrder.TabIndex = 3;
this.lblSortOrder.Text = "排序";
this.txtSortOrder.Location = new System.Drawing.Point(173,71);
this.txtSortOrder.Name = "txtSortOrder";
this.txtSortOrder.Size = new System.Drawing.Size(100, 21);
this.txtSortOrder.TabIndex = 3;
this.Controls.Add(this.lblSortOrder);
this.Controls.Add(this.txtSortOrder);

           //#####50InputType###String
this.lblInputType.AutoSize = true;
this.lblInputType.Location = new System.Drawing.Point(100,100);
this.lblInputType.Name = "lblInputType";
this.lblInputType.Size = new System.Drawing.Size(41, 12);
this.lblInputType.TabIndex = 4;
this.lblInputType.Text = "输入类型";
this.txtInputType.Location = new System.Drawing.Point(173,96);
this.txtInputType.Name = "txtInputType";
this.txtInputType.Size = new System.Drawing.Size(100, 21);
this.txtInputType.TabIndex = 4;
this.Controls.Add(this.lblInputType);
this.Controls.Add(this.txtInputType);

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
                this.Controls.Add(this.lblPropertyName );
this.Controls.Add(this.txtPropertyName );

                this.Controls.Add(this.lblPropertyDesc );
this.Controls.Add(this.txtPropertyDesc );

                this.Controls.Add(this.lblSortOrder );
this.Controls.Add(this.txtSortOrder );

                this.Controls.Add(this.lblInputType );
this.Controls.Add(this.txtInputType );

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
            // "tb_ProdPropertyEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProdPropertyEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPropertyName;
private Krypton.Toolkit.KryptonTextBox txtPropertyName;

    
        
              private Krypton.Toolkit.KryptonLabel lblPropertyDesc;
private Krypton.Toolkit.KryptonTextBox txtPropertyDesc;

    
        
              private Krypton.Toolkit.KryptonLabel lblSortOrder;
private Krypton.Toolkit.KryptonTextBox txtSortOrder;

    
        
              private Krypton.Toolkit.KryptonLabel lblInputType;
private Krypton.Toolkit.KryptonTextBox txtInputType;

    
        
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

