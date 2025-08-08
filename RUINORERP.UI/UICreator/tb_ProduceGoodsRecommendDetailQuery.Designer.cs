
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:59
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 自制成品建议
    /// </summary>
    partial class tb_ProduceGoodsRecommendDetailQuery
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



this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblPreStartDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPreStartDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPreEndDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPreEndDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblRefBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRefBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



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

           //#####ID###Int64
//属性测试100ID
//属性测试100ID
//属性测试100ID
//属性测试100ID

           //#####ParentId###Int64
//属性测试125ParentId
//属性测试125ParentId
//属性测试125ParentId
//属性测试125ParentId

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,150);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 6;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,146);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 6;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,175);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 7;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,171);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 7;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####BOM_ID###Int64
//属性测试200BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,200);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 8;
this.lblBOM_ID.Text = "标准配方";
//111======200
this.cmbBOM_ID.Location = new System.Drawing.Point(173,196);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 8;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,225);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 9;
this.lblSubtotalCostAmount.Text = "成本小计";
//111======225
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,221);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 9;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,250);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 10;
this.lblUnitCost.Text = "单位成本";
//111======250
this.txtUnitCost.Location = new System.Drawing.Point(173,246);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 10;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####RequirementQty###Int32
//属性测试275RequirementQty
//属性测试275RequirementQty
//属性测试275RequirementQty
//属性测试275RequirementQty

           //#####RecommendQty###Int32
//属性测试300RecommendQty
//属性测试300RecommendQty
//属性测试300RecommendQty
//属性测试300RecommendQty

           //#####PlanNeedQty###Int32
//属性测试325PlanNeedQty
//属性测试325PlanNeedQty
//属性测试325PlanNeedQty
//属性测试325PlanNeedQty

           //#####PreStartDate###DateTime
this.lblPreStartDate.AutoSize = true;
this.lblPreStartDate.Location = new System.Drawing.Point(100,350);
this.lblPreStartDate.Name = "lblPreStartDate";
this.lblPreStartDate.Size = new System.Drawing.Size(41, 12);
this.lblPreStartDate.TabIndex = 14;
this.lblPreStartDate.Text = "预开工日";
//111======350
this.dtpPreStartDate.Location = new System.Drawing.Point(173,346);
this.dtpPreStartDate.Name ="dtpPreStartDate";
this.dtpPreStartDate.ShowCheckBox =true;
this.dtpPreStartDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreStartDate.TabIndex = 14;
this.Controls.Add(this.lblPreStartDate);
this.Controls.Add(this.dtpPreStartDate);

           //#####PreEndDate###DateTime
this.lblPreEndDate.AutoSize = true;
this.lblPreEndDate.Location = new System.Drawing.Point(100,375);
this.lblPreEndDate.Name = "lblPreEndDate";
this.lblPreEndDate.Size = new System.Drawing.Size(41, 12);
this.lblPreEndDate.TabIndex = 15;
this.lblPreEndDate.Text = "预完工日";
//111======375
this.dtpPreEndDate.Location = new System.Drawing.Point(173,371);
this.dtpPreEndDate.Name ="dtpPreEndDate";
this.dtpPreEndDate.ShowCheckBox =true;
this.dtpPreEndDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreEndDate.TabIndex = 15;
this.Controls.Add(this.lblPreEndDate);
this.Controls.Add(this.dtpPreEndDate);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,400);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 16;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,396);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 16;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####100RefBillNO###String
this.lblRefBillNO.AutoSize = true;
this.lblRefBillNO.Location = new System.Drawing.Point(100,425);
this.lblRefBillNO.Name = "lblRefBillNO";
this.lblRefBillNO.Size = new System.Drawing.Size(41, 12);
this.lblRefBillNO.TabIndex = 17;
this.lblRefBillNO.Text = "生成单号";
this.txtRefBillNO.Location = new System.Drawing.Point(173,421);
this.txtRefBillNO.Name = "txtRefBillNO";
this.txtRefBillNO.Size = new System.Drawing.Size(100, 21);
this.txtRefBillNO.TabIndex = 17;
this.Controls.Add(this.lblRefBillNO);
this.Controls.Add(this.txtRefBillNO);

           //#####RefBillType###Int32
//属性测试450RefBillType
//属性测试450RefBillType
//属性测试450RefBillType
//属性测试450RefBillType

           //#####RefBillID###Int64
//属性测试475RefBillID
//属性测试475RefBillID
//属性测试475RefBillID
//属性测试475RefBillID

          
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

                
                
                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                
                
                
                this.Controls.Add(this.lblPreStartDate );
this.Controls.Add(this.dtpPreStartDate );

                this.Controls.Add(this.lblPreEndDate );
this.Controls.Add(this.dtpPreEndDate );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblRefBillNO );
this.Controls.Add(this.txtRefBillNO );

                
                
                    
            this.Name = "tb_ProduceGoodsRecommendDetailQuery";
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

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreStartDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPreStartDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreEndDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPreEndDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRefBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRefBillNO;

    
        
              
    
        
              
    
    
   
 





    }
}


