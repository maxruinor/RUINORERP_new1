
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
    partial class tb_ProductionPlanDetailQuery
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
     
     this.lblPPID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPPID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;



this.lblIsAnalyzed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsAnalyzed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsAnalyzed.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####AnalyzedQuantity###Int32
//属性测试275AnalyzedQuantity
//属性测试275AnalyzedQuantity
//属性测试275AnalyzedQuantity
//属性测试275AnalyzedQuantity

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                
                
                this.Controls.Add(this.lblIsAnalyzed );
this.Controls.Add(this.chkIsAnalyzed );

                    
            this.Name = "tb_ProductionPlanDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPPID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPPID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRequirementDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsAnalyzed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsAnalyzed;

    
    
   
 





    }
}


