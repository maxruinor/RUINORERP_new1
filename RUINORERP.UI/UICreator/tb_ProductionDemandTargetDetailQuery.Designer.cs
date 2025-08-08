
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:01
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 生产需求分析目标对象明细
    /// </summary>
    partial class tb_ProductionDemandTargetDetailQuery
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
     
     this.lblPDID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPDID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();







this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;


this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####PDID###Int64
//属性测试25PDID
//属性测试25PDID
//属性测试25PDID
//属性测试25PDID
this.lblPDID.AutoSize = true;
this.lblPDID.Location = new System.Drawing.Point(100,25);
this.lblPDID.Name = "lblPDID";
this.lblPDID.Size = new System.Drawing.Size(41, 12);
this.lblPDID.TabIndex = 1;
this.lblPDID.Text = "";
//111======25
this.cmbPDID.Location = new System.Drawing.Point(173,21);
this.cmbPDID.Name ="cmbPDID";
this.cmbPDID.Size = new System.Drawing.Size(100, 21);
this.cmbPDID.TabIndex = 1;
this.Controls.Add(this.lblPDID);
this.Controls.Add(this.cmbPDID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
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

           //#####Location_ID###Int64
//属性测试75Location_ID
//属性测试75Location_ID
//属性测试75Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 3;
this.lblLocation_ID.Text = "库位";
//111======75
this.cmbLocation_ID.Location = new System.Drawing.Point(173,71);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 3;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####BOM_ID###Int64
//属性测试100BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,100);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 4;
this.lblBOM_ID.Text = "配方名称";
//111======100
this.cmbBOM_ID.Location = new System.Drawing.Point(173,96);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 4;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####NeedQuantity###Int32
//属性测试125NeedQuantity
//属性测试125NeedQuantity
//属性测试125NeedQuantity
//属性测试125NeedQuantity

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,150);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 6;
this.lblRequirementDate.Text = "需求日期";
//111======150
this.dtpRequirementDate.Location = new System.Drawing.Point(173,146);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 6;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

           //#####BookInventory###Int32
//属性测试175BookInventory
//属性测试175BookInventory
//属性测试175BookInventory
//属性测试175BookInventory

           //#####AvailableStock###Int32
//属性测试200AvailableStock
//属性测试200AvailableStock
//属性测试200AvailableStock
//属性测试200AvailableStock

           //#####InTransitInventory###Int32
//属性测试225InTransitInventory
//属性测试225InTransitInventory
//属性测试225InTransitInventory
//属性测试225InTransitInventory

           //#####MakeProcessInventory###Int32
//属性测试250MakeProcessInventory
//属性测试250MakeProcessInventory
//属性测试250MakeProcessInventory
//属性测试250MakeProcessInventory

           //#####SaleQty###Int32
//属性测试275SaleQty
//属性测试275SaleQty
//属性测试275SaleQty
//属性测试275SaleQty

           //#####NotIssueMaterialQty###Int32
//属性测试300NotIssueMaterialQty
//属性测试300NotIssueMaterialQty
//属性测试300NotIssueMaterialQty
//属性测试300NotIssueMaterialQty

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,325);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 13;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,321);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 13;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####PPCID###Int64
//属性测试350PPCID
//属性测试350PPCID
//属性测试350PPCID
//属性测试350PPCID

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,375);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 15;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,371);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 15;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,400);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 16;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,396);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 16;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPDID );
this.Controls.Add(this.cmbPDID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                
                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                
                
                
                
                
                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                
                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                    
            this.Name = "tb_ProductionDemandTargetDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPDID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPDID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRequirementDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
    
   
 





    }
}


