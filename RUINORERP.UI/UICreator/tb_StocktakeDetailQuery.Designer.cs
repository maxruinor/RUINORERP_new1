
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/14/2025 11:15:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 盘点明细表
    /// </summary>
    partial class tb_StocktakeDetailQuery
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
     
     this.lblMainID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMainID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCarryingSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCarryingSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblDiffSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiffSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCheckSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCheckSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####MainID###Int64
//属性测试25MainID
//属性测试25MainID
this.lblMainID.AutoSize = true;
this.lblMainID.Location = new System.Drawing.Point(100,25);
this.lblMainID.Name = "lblMainID";
this.lblMainID.Size = new System.Drawing.Size(41, 12);
this.lblMainID.TabIndex = 1;
this.lblMainID.Text = "";
//111======25
this.cmbMainID.Location = new System.Drawing.Point(173,21);
this.cmbMainID.Name ="cmbMainID";
this.cmbMainID.Size = new System.Drawing.Size(100, 21);
this.cmbMainID.TabIndex = 1;
this.Controls.Add(this.lblMainID);
this.Controls.Add(this.cmbMainID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "产品";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####Rack_ID###Int64
//属性测试75Rack_ID
//属性测试75Rack_ID
//属性测试75Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,75);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 3;
this.lblRack_ID.Text = "货架";
//111======75
this.cmbRack_ID.Location = new System.Drawing.Point(173,71);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 3;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

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

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,125);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 5;
this.lblCost.Text = "成本";
//111======125
this.txtCost.Location = new System.Drawing.Point(173,121);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 5;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,150);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 6;
this.lblTaxRate.Text = "税率";
//111======150
this.txtTaxRate.Location = new System.Drawing.Point(173,146);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 6;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####UntaxedCost###Decimal
this.lblUntaxedCost.AutoSize = true;
this.lblUntaxedCost.Location = new System.Drawing.Point(100,175);
this.lblUntaxedCost.Name = "lblUntaxedCost";
this.lblUntaxedCost.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedCost.TabIndex = 7;
this.lblUntaxedCost.Text = "未税单价";
//111======175
this.txtUntaxedCost.Location = new System.Drawing.Point(173,171);
this.txtUntaxedCost.Name ="txtUntaxedCost";
this.txtUntaxedCost.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedCost.TabIndex = 7;
this.Controls.Add(this.lblUntaxedCost);
this.Controls.Add(this.txtUntaxedCost);

           //#####CarryinglQty###Int32
//属性测试200CarryinglQty
//属性测试200CarryinglQty
//属性测试200CarryinglQty

           //#####CarryingSubtotalAmount###Decimal
this.lblCarryingSubtotalAmount.AutoSize = true;
this.lblCarryingSubtotalAmount.Location = new System.Drawing.Point(100,225);
this.lblCarryingSubtotalAmount.Name = "lblCarryingSubtotalAmount";
this.lblCarryingSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblCarryingSubtotalAmount.TabIndex = 9;
this.lblCarryingSubtotalAmount.Text = "载账小计";
//111======225
this.txtCarryingSubtotalAmount.Location = new System.Drawing.Point(173,221);
this.txtCarryingSubtotalAmount.Name ="txtCarryingSubtotalAmount";
this.txtCarryingSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtCarryingSubtotalAmount.TabIndex = 9;
this.Controls.Add(this.lblCarryingSubtotalAmount);
this.Controls.Add(this.txtCarryingSubtotalAmount);

           //#####DiffQty###Int32
//属性测试250DiffQty
//属性测试250DiffQty
//属性测试250DiffQty

           //#####DiffSubtotalAmount###Decimal
this.lblDiffSubtotalAmount.AutoSize = true;
this.lblDiffSubtotalAmount.Location = new System.Drawing.Point(100,275);
this.lblDiffSubtotalAmount.Name = "lblDiffSubtotalAmount";
this.lblDiffSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblDiffSubtotalAmount.TabIndex = 11;
this.lblDiffSubtotalAmount.Text = "差异小计";
//111======275
this.txtDiffSubtotalAmount.Location = new System.Drawing.Point(173,271);
this.txtDiffSubtotalAmount.Name ="txtDiffSubtotalAmount";
this.txtDiffSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtDiffSubtotalAmount.TabIndex = 11;
this.Controls.Add(this.lblDiffSubtotalAmount);
this.Controls.Add(this.txtDiffSubtotalAmount);

           //#####CheckQty###Int32
//属性测试300CheckQty
//属性测试300CheckQty
//属性测试300CheckQty

           //#####CheckSubtotalAmount###Decimal
this.lblCheckSubtotalAmount.AutoSize = true;
this.lblCheckSubtotalAmount.Location = new System.Drawing.Point(100,325);
this.lblCheckSubtotalAmount.Name = "lblCheckSubtotalAmount";
this.lblCheckSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblCheckSubtotalAmount.TabIndex = 13;
this.lblCheckSubtotalAmount.Text = "盘点小计";
//111======325
this.txtCheckSubtotalAmount.Location = new System.Drawing.Point(173,321);
this.txtCheckSubtotalAmount.Name ="txtCheckSubtotalAmount";
this.txtCheckSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtCheckSubtotalAmount.TabIndex = 13;
this.Controls.Add(this.lblCheckSubtotalAmount);
this.Controls.Add(this.txtCheckSubtotalAmount);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,350);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 14;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,346);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 14;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMainID );
this.Controls.Add(this.cmbMainID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblUntaxedCost );
this.Controls.Add(this.txtUntaxedCost );

                
                this.Controls.Add(this.lblCarryingSubtotalAmount );
this.Controls.Add(this.txtCarryingSubtotalAmount );

                
                this.Controls.Add(this.lblDiffSubtotalAmount );
this.Controls.Add(this.txtDiffSubtotalAmount );

                
                this.Controls.Add(this.lblCheckSubtotalAmount );
this.Controls.Add(this.txtCheckSubtotalAmount );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_StocktakeDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMainID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMainID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedCost;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCarryingSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCarryingSubtotalAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiffSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiffSubtotalAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheckSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCheckSubtotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


