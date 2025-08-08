// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:02
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 生产计划明细
    /// </summary>
    partial class tb_ProductionPlanDetailEdit
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
     this.lblPPID = new Krypton.Toolkit.KryptonLabel();
this.cmbPPID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblRequirementDate = new Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblBOM_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCompletedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtCompletedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblAnalyzedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtAnalyzedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblIsAnalyzed = new Krypton.Toolkit.KryptonLabel();
this.chkIsAnalyzed = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsAnalyzed.Values.Text ="";

    
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
     
            //#####PPID###Int64
//属性测试25PPID
//属性测试25PPID
this.lblPPID.AutoSize = true;
this.lblPPID.Location = new System.Drawing.Point(100,25);
this.lblPPID.Name = "lblPPID";
this.lblPPID.Size = new System.Drawing.Size(41, 12);
this.lblPPID.TabIndex = 1;
this.lblPPID.Text = "";
//111======25
this.cmbPPID.Location = new System.Drawing.Point(173,21);
this.cmbPPID.Name ="cmbPPID";
this.cmbPPID.Size = new System.Drawing.Size(100, 21);
this.cmbPPID.TabIndex = 1;
this.Controls.Add(this.lblPPID);
this.Controls.Add(this.cmbPPID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "货品";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,75);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 3;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,71);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 3;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,100);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 4;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,96);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 4;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64
//属性测试125Location_ID
//属性测试125Location_ID
//属性测试125Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,125);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 5;
this.lblLocation_ID.Text = "库位";
//111======125
this.cmbLocation_ID.Location = new System.Drawing.Point(173,121);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 5;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####Quantity###Int32
//属性测试150Quantity
//属性测试150Quantity
//属性测试150Quantity
//属性测试150Quantity
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,150);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 6;
this.lblQuantity.Text = "计划数量";
this.txtQuantity.Location = new System.Drawing.Point(173,146);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 6;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,175);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 7;
this.lblRequirementDate.Text = "需求日期";
//111======175
this.dtpRequirementDate.Location = new System.Drawing.Point(173,171);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.ShowCheckBox =true;
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 7;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

           //#####BOM_ID###Int64
//属性测试200BOM_ID
//属性测试200BOM_ID
//属性测试200BOM_ID
//属性测试200BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,200);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 8;
this.lblBOM_ID.Text = "配方名称";
//111======200
this.cmbBOM_ID.Location = new System.Drawing.Point(173,196);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 8;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,225);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 9;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,221);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 9;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####CompletedQuantity###Int32
//属性测试250CompletedQuantity
//属性测试250CompletedQuantity
//属性测试250CompletedQuantity
//属性测试250CompletedQuantity
this.lblCompletedQuantity.AutoSize = true;
this.lblCompletedQuantity.Location = new System.Drawing.Point(100,250);
this.lblCompletedQuantity.Name = "lblCompletedQuantity";
this.lblCompletedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblCompletedQuantity.TabIndex = 10;
this.lblCompletedQuantity.Text = "完成数量";
this.txtCompletedQuantity.Location = new System.Drawing.Point(173,246);
this.txtCompletedQuantity.Name = "txtCompletedQuantity";
this.txtCompletedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtCompletedQuantity.TabIndex = 10;
this.Controls.Add(this.lblCompletedQuantity);
this.Controls.Add(this.txtCompletedQuantity);

           //#####AnalyzedQuantity###Int32
//属性测试275AnalyzedQuantity
//属性测试275AnalyzedQuantity
//属性测试275AnalyzedQuantity
//属性测试275AnalyzedQuantity
this.lblAnalyzedQuantity.AutoSize = true;
this.lblAnalyzedQuantity.Location = new System.Drawing.Point(100,275);
this.lblAnalyzedQuantity.Name = "lblAnalyzedQuantity";
this.lblAnalyzedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblAnalyzedQuantity.TabIndex = 11;
this.lblAnalyzedQuantity.Text = "已分析数量";
this.txtAnalyzedQuantity.Location = new System.Drawing.Point(173,271);
this.txtAnalyzedQuantity.Name = "txtAnalyzedQuantity";
this.txtAnalyzedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtAnalyzedQuantity.TabIndex = 11;
this.Controls.Add(this.lblAnalyzedQuantity);
this.Controls.Add(this.txtAnalyzedQuantity);

           //#####IsAnalyzed###Boolean
this.lblIsAnalyzed.AutoSize = true;
this.lblIsAnalyzed.Location = new System.Drawing.Point(100,300);
this.lblIsAnalyzed.Name = "lblIsAnalyzed";
this.lblIsAnalyzed.Size = new System.Drawing.Size(41, 12);
this.lblIsAnalyzed.TabIndex = 12;
this.lblIsAnalyzed.Text = "已分析";
this.chkIsAnalyzed.Location = new System.Drawing.Point(173,296);
this.chkIsAnalyzed.Name = "chkIsAnalyzed";
this.chkIsAnalyzed.Size = new System.Drawing.Size(100, 21);
this.chkIsAnalyzed.TabIndex = 12;
this.Controls.Add(this.lblIsAnalyzed);
this.Controls.Add(this.chkIsAnalyzed);

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
           // this.kryptonPanel1.TabIndex = 12;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPPID );
this.Controls.Add(this.cmbPPID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCompletedQuantity );
this.Controls.Add(this.txtCompletedQuantity );

                this.Controls.Add(this.lblAnalyzedQuantity );
this.Controls.Add(this.txtAnalyzedQuantity );

                this.Controls.Add(this.lblIsAnalyzed );
this.Controls.Add(this.chkIsAnalyzed );

                            // 
            // "tb_ProductionPlanDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProductionPlanDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPPID;
private Krypton.Toolkit.KryptonComboBox cmbPPID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblRequirementDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_ID;
private Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblCompletedQuantity;
private Krypton.Toolkit.KryptonTextBox txtCompletedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblAnalyzedQuantity;
private Krypton.Toolkit.KryptonTextBox txtAnalyzedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsAnalyzed;
private Krypton.Toolkit.KryptonCheckBox chkIsAnalyzed;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

