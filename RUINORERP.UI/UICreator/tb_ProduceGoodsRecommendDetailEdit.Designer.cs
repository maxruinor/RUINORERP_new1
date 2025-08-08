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
    partial class tb_ProduceGoodsRecommendDetailEdit
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
     this.lblPDID = new Krypton.Toolkit.KryptonLabel();
this.cmbPDID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblID = new Krypton.Toolkit.KryptonLabel();
this.txtID = new Krypton.Toolkit.KryptonTextBox();

this.lblParentId = new Krypton.Toolkit.KryptonLabel();
this.txtParentId = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblBOM_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblSubtotalCostAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new Krypton.Toolkit.KryptonTextBox();

this.lblRequirementQty = new Krypton.Toolkit.KryptonLabel();
this.txtRequirementQty = new Krypton.Toolkit.KryptonTextBox();

this.lblRecommendQty = new Krypton.Toolkit.KryptonLabel();
this.txtRecommendQty = new Krypton.Toolkit.KryptonTextBox();

this.lblPlanNeedQty = new Krypton.Toolkit.KryptonLabel();
this.txtPlanNeedQty = new Krypton.Toolkit.KryptonTextBox();

this.lblPreStartDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPreStartDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPreEndDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPreEndDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblRefBillNO = new Krypton.Toolkit.KryptonLabel();
this.txtRefBillNO = new Krypton.Toolkit.KryptonTextBox();

this.lblRefBillType = new Krypton.Toolkit.KryptonLabel();
this.txtRefBillType = new Krypton.Toolkit.KryptonTextBox();

this.lblRefBillID = new Krypton.Toolkit.KryptonLabel();
this.txtRefBillID = new Krypton.Toolkit.KryptonTextBox();

    
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
this.lblID.AutoSize = true;
this.lblID.Location = new System.Drawing.Point(100,100);
this.lblID.Name = "lblID";
this.lblID.Size = new System.Drawing.Size(41, 12);
this.lblID.TabIndex = 4;
this.lblID.Text = "";
this.txtID.Location = new System.Drawing.Point(173,96);
this.txtID.Name = "txtID";
this.txtID.Size = new System.Drawing.Size(100, 21);
this.txtID.TabIndex = 4;
this.Controls.Add(this.lblID);
this.Controls.Add(this.txtID);

           //#####ParentId###Int64
//属性测试125ParentId
//属性测试125ParentId
//属性测试125ParentId
//属性测试125ParentId
this.lblParentId.AutoSize = true;
this.lblParentId.Location = new System.Drawing.Point(100,125);
this.lblParentId.Name = "lblParentId";
this.lblParentId.Size = new System.Drawing.Size(41, 12);
this.lblParentId.TabIndex = 5;
this.lblParentId.Text = "";
this.txtParentId.Location = new System.Drawing.Point(173,121);
this.txtParentId.Name = "txtParentId";
this.txtParentId.Size = new System.Drawing.Size(100, 21);
this.txtParentId.TabIndex = 5;
this.Controls.Add(this.lblParentId);
this.Controls.Add(this.txtParentId);

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
this.lblRequirementQty.AutoSize = true;
this.lblRequirementQty.Location = new System.Drawing.Point(100,275);
this.lblRequirementQty.Name = "lblRequirementQty";
this.lblRequirementQty.Size = new System.Drawing.Size(41, 12);
this.lblRequirementQty.TabIndex = 11;
this.lblRequirementQty.Text = "请制量";
this.txtRequirementQty.Location = new System.Drawing.Point(173,271);
this.txtRequirementQty.Name = "txtRequirementQty";
this.txtRequirementQty.Size = new System.Drawing.Size(100, 21);
this.txtRequirementQty.TabIndex = 11;
this.Controls.Add(this.lblRequirementQty);
this.Controls.Add(this.txtRequirementQty);

           //#####RecommendQty###Int32
//属性测试300RecommendQty
//属性测试300RecommendQty
//属性测试300RecommendQty
//属性测试300RecommendQty
this.lblRecommendQty.AutoSize = true;
this.lblRecommendQty.Location = new System.Drawing.Point(100,300);
this.lblRecommendQty.Name = "lblRecommendQty";
this.lblRecommendQty.Size = new System.Drawing.Size(41, 12);
this.lblRecommendQty.TabIndex = 12;
this.lblRecommendQty.Text = "建议量";
this.txtRecommendQty.Location = new System.Drawing.Point(173,296);
this.txtRecommendQty.Name = "txtRecommendQty";
this.txtRecommendQty.Size = new System.Drawing.Size(100, 21);
this.txtRecommendQty.TabIndex = 12;
this.Controls.Add(this.lblRecommendQty);
this.Controls.Add(this.txtRecommendQty);

           //#####PlanNeedQty###Int32
//属性测试325PlanNeedQty
//属性测试325PlanNeedQty
//属性测试325PlanNeedQty
//属性测试325PlanNeedQty
this.lblPlanNeedQty.AutoSize = true;
this.lblPlanNeedQty.Location = new System.Drawing.Point(100,325);
this.lblPlanNeedQty.Name = "lblPlanNeedQty";
this.lblPlanNeedQty.Size = new System.Drawing.Size(41, 12);
this.lblPlanNeedQty.TabIndex = 13;
this.lblPlanNeedQty.Text = "计划需求数";
this.txtPlanNeedQty.Location = new System.Drawing.Point(173,321);
this.txtPlanNeedQty.Name = "txtPlanNeedQty";
this.txtPlanNeedQty.Size = new System.Drawing.Size(100, 21);
this.txtPlanNeedQty.TabIndex = 13;
this.Controls.Add(this.lblPlanNeedQty);
this.Controls.Add(this.txtPlanNeedQty);

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
this.lblRefBillType.AutoSize = true;
this.lblRefBillType.Location = new System.Drawing.Point(100,450);
this.lblRefBillType.Name = "lblRefBillType";
this.lblRefBillType.Size = new System.Drawing.Size(41, 12);
this.lblRefBillType.TabIndex = 18;
this.lblRefBillType.Text = "单据类型";
this.txtRefBillType.Location = new System.Drawing.Point(173,446);
this.txtRefBillType.Name = "txtRefBillType";
this.txtRefBillType.Size = new System.Drawing.Size(100, 21);
this.txtRefBillType.TabIndex = 18;
this.Controls.Add(this.lblRefBillType);
this.Controls.Add(this.txtRefBillType);

           //#####RefBillID###Int64
//属性测试475RefBillID
//属性测试475RefBillID
//属性测试475RefBillID
//属性测试475RefBillID
this.lblRefBillID.AutoSize = true;
this.lblRefBillID.Location = new System.Drawing.Point(100,475);
this.lblRefBillID.Name = "lblRefBillID";
this.lblRefBillID.Size = new System.Drawing.Size(41, 12);
this.lblRefBillID.TabIndex = 19;
this.lblRefBillID.Text = "生成单据";
this.txtRefBillID.Location = new System.Drawing.Point(173,471);
this.txtRefBillID.Name = "txtRefBillID";
this.txtRefBillID.Size = new System.Drawing.Size(100, 21);
this.txtRefBillID.TabIndex = 19;
this.Controls.Add(this.lblRefBillID);
this.Controls.Add(this.txtRefBillID);

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
           // this.kryptonPanel1.TabIndex = 19;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPDID );
this.Controls.Add(this.cmbPDID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblID );
this.Controls.Add(this.txtID );

                this.Controls.Add(this.lblParentId );
this.Controls.Add(this.txtParentId );

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

                this.Controls.Add(this.lblRequirementQty );
this.Controls.Add(this.txtRequirementQty );

                this.Controls.Add(this.lblRecommendQty );
this.Controls.Add(this.txtRecommendQty );

                this.Controls.Add(this.lblPlanNeedQty );
this.Controls.Add(this.txtPlanNeedQty );

                this.Controls.Add(this.lblPreStartDate );
this.Controls.Add(this.dtpPreStartDate );

                this.Controls.Add(this.lblPreEndDate );
this.Controls.Add(this.dtpPreEndDate );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblRefBillNO );
this.Controls.Add(this.txtRefBillNO );

                this.Controls.Add(this.lblRefBillType );
this.Controls.Add(this.txtRefBillType );

                this.Controls.Add(this.lblRefBillID );
this.Controls.Add(this.txtRefBillID );

                            // 
            // "tb_ProduceGoodsRecommendDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProduceGoodsRecommendDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPDID;
private Krypton.Toolkit.KryptonComboBox cmbPDID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblID;
private Krypton.Toolkit.KryptonTextBox txtID;

    
        
              private Krypton.Toolkit.KryptonLabel lblParentId;
private Krypton.Toolkit.KryptonTextBox txtParentId;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_ID;
private Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitCost;
private Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblRequirementQty;
private Krypton.Toolkit.KryptonTextBox txtRequirementQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblRecommendQty;
private Krypton.Toolkit.KryptonTextBox txtRecommendQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlanNeedQty;
private Krypton.Toolkit.KryptonTextBox txtPlanNeedQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblPreStartDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPreStartDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPreEndDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPreEndDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBillNO;
private Krypton.Toolkit.KryptonTextBox txtRefBillNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBillType;
private Krypton.Toolkit.KryptonTextBox txtRefBillType;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBillID;
private Krypton.Toolkit.KryptonTextBox txtRefBillID;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

