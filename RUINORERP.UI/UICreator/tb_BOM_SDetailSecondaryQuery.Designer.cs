
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:13
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 标准物料表次级产出明细
    /// </summary>
    partial class tb_BOM_SDetailSecondaryQuery
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
     
     this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblScale = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtScale = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRemarks = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemarks = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdDetailID###Int64
//属性测试25ProdDetailID
//属性测试25ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,25);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 1;
this.lblProdDetailID.Text = "产品详情";
//111======25
this.cmbProdDetailID.Location = new System.Drawing.Point(173,21);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 1;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####BOM_ID###Int64
//属性测试50BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,50);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 2;
this.lblBOM_ID.Text = "BOM";
//111======50
this.cmbBOM_ID.Location = new System.Drawing.Point(173,46);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 2;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####Qty###Decimal
this.lblQty.AutoSize = true;
this.lblQty.Location = new System.Drawing.Point(100,75);
this.lblQty.Name = "lblQty";
this.lblQty.Size = new System.Drawing.Size(41, 12);
this.lblQty.TabIndex = 3;
this.lblQty.Text = "数量";
//111======75
this.txtQty.Location = new System.Drawing.Point(173,71);
this.txtQty.Name ="txtQty";
this.txtQty.Size = new System.Drawing.Size(100, 21);
this.txtQty.TabIndex = 3;
this.Controls.Add(this.lblQty);
this.Controls.Add(this.txtQty);

           //#####Scale###Decimal
this.lblScale.AutoSize = true;
this.lblScale.Location = new System.Drawing.Point(100,100);
this.lblScale.Name = "lblScale";
this.lblScale.Size = new System.Drawing.Size(41, 12);
this.lblScale.TabIndex = 4;
this.lblScale.Text = "比例";
//111======100
this.txtScale.Location = new System.Drawing.Point(173,96);
this.txtScale.Name ="txtScale";
this.txtScale.Size = new System.Drawing.Size(100, 21);
this.txtScale.TabIndex = 4;
this.Controls.Add(this.lblScale);
this.Controls.Add(this.txtScale);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,125);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 5;
this.lblproperty.Text = "副产品属性";
this.txtproperty.Location = new System.Drawing.Point(173,121);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 5;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,150);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 6;
this.lblUnitCost.Text = "单位成本";
//111======150
this.txtUnitCost.Location = new System.Drawing.Point(173,146);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 6;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####SubtotalCost###Decimal
this.lblSubtotalCost.AutoSize = true;
this.lblSubtotalCost.Location = new System.Drawing.Point(100,175);
this.lblSubtotalCost.Name = "lblSubtotalCost";
this.lblSubtotalCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCost.TabIndex = 7;
this.lblSubtotalCost.Text = "成本小计";
//111======175
this.txtSubtotalCost.Location = new System.Drawing.Point(173,171);
this.txtSubtotalCost.Name ="txtSubtotalCost";
this.txtSubtotalCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCost.TabIndex = 7;
this.Controls.Add(this.lblSubtotalCost);
this.Controls.Add(this.txtSubtotalCost);

           //#####200Remarks###String
this.lblRemarks.AutoSize = true;
this.lblRemarks.Location = new System.Drawing.Point(100,200);
this.lblRemarks.Name = "lblRemarks";
this.lblRemarks.Size = new System.Drawing.Size(41, 12);
this.lblRemarks.TabIndex = 8;
this.lblRemarks.Text = "备注说明";
this.txtRemarks.Location = new System.Drawing.Point(173,196);
this.txtRemarks.Name = "txtRemarks";
this.txtRemarks.Size = new System.Drawing.Size(100, 21);
this.txtRemarks.TabIndex = 8;
this.Controls.Add(this.lblRemarks);
this.Controls.Add(this.txtRemarks);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblQty );
this.Controls.Add(this.txtQty );

                this.Controls.Add(this.lblScale );
this.Controls.Add(this.txtScale );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                this.Controls.Add(this.lblSubtotalCost );
this.Controls.Add(this.txtSubtotalCost );

                this.Controls.Add(this.lblRemarks );
this.Controls.Add(this.txtRemarks );

                    
            this.Name = "tb_BOM_SDetailSecondaryQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblScale;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtScale;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemarks;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemarks;

    
    
   
 





    }
}


