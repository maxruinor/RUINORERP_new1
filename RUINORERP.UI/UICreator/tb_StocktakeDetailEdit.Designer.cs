// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:41
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
    partial class tb_StocktakeDetailEdit
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
     this.lblMainID = new Krypton.Toolkit.KryptonLabel();
this.cmbMainID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCarryinglQty = new Krypton.Toolkit.KryptonLabel();
this.txtCarryinglQty = new Krypton.Toolkit.KryptonTextBox();

this.lblDiffQty = new Krypton.Toolkit.KryptonLabel();
this.txtDiffQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckQty = new Krypton.Toolkit.KryptonLabel();
this.txtCheckQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCost = new Krypton.Toolkit.KryptonLabel();
this.txtCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedCost = new Krypton.Toolkit.KryptonLabel();
this.txtUntaxedCost = new Krypton.Toolkit.KryptonTextBox();

this.lblCarryingSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCarryingSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblDiffSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtDiffSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCheckSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

    
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
this.lblProdDetailID.Text = "";
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

           //#####CarryinglQty###Int32
//属性测试100CarryinglQty
//属性测试100CarryinglQty
//属性测试100CarryinglQty
this.lblCarryinglQty.AutoSize = true;
this.lblCarryinglQty.Location = new System.Drawing.Point(100,100);
this.lblCarryinglQty.Name = "lblCarryinglQty";
this.lblCarryinglQty.Size = new System.Drawing.Size(41, 12);
this.lblCarryinglQty.TabIndex = 4;
this.lblCarryinglQty.Text = "载账数量";
this.txtCarryinglQty.Location = new System.Drawing.Point(173,96);
this.txtCarryinglQty.Name = "txtCarryinglQty";
this.txtCarryinglQty.Size = new System.Drawing.Size(100, 21);
this.txtCarryinglQty.TabIndex = 4;
this.Controls.Add(this.lblCarryinglQty);
this.Controls.Add(this.txtCarryinglQty);

           //#####DiffQty###Int32
//属性测试125DiffQty
//属性测试125DiffQty
//属性测试125DiffQty
this.lblDiffQty.AutoSize = true;
this.lblDiffQty.Location = new System.Drawing.Point(100,125);
this.lblDiffQty.Name = "lblDiffQty";
this.lblDiffQty.Size = new System.Drawing.Size(41, 12);
this.lblDiffQty.TabIndex = 5;
this.lblDiffQty.Text = "差异数量";
this.txtDiffQty.Location = new System.Drawing.Point(173,121);
this.txtDiffQty.Name = "txtDiffQty";
this.txtDiffQty.Size = new System.Drawing.Size(100, 21);
this.txtDiffQty.TabIndex = 5;
this.Controls.Add(this.lblDiffQty);
this.Controls.Add(this.txtDiffQty);

           //#####CheckQty###Int32
//属性测试150CheckQty
//属性测试150CheckQty
//属性测试150CheckQty
this.lblCheckQty.AutoSize = true;
this.lblCheckQty.Location = new System.Drawing.Point(100,150);
this.lblCheckQty.Name = "lblCheckQty";
this.lblCheckQty.Size = new System.Drawing.Size(41, 12);
this.lblCheckQty.TabIndex = 6;
this.lblCheckQty.Text = "盘点数量";
this.txtCheckQty.Location = new System.Drawing.Point(173,146);
this.txtCheckQty.Name = "txtCheckQty";
this.txtCheckQty.Size = new System.Drawing.Size(100, 21);
this.txtCheckQty.TabIndex = 6;
this.Controls.Add(this.lblCheckQty);
this.Controls.Add(this.txtCheckQty);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,175);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 7;
this.lblCost.Text = "含税单位成本";
//111======175
this.txtCost.Location = new System.Drawing.Point(173,171);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 7;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,200);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 8;
this.lblTaxRate.Text = "税率";
//111======200
this.txtTaxRate.Location = new System.Drawing.Point(173,196);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 8;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####UntaxedCost###Decimal
this.lblUntaxedCost.AutoSize = true;
this.lblUntaxedCost.Location = new System.Drawing.Point(100,225);
this.lblUntaxedCost.Name = "lblUntaxedCost";
this.lblUntaxedCost.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedCost.TabIndex = 9;
this.lblUntaxedCost.Text = "未税单位成本";
//111======225
this.txtUntaxedCost.Location = new System.Drawing.Point(173,221);
this.txtUntaxedCost.Name ="txtUntaxedCost";
this.txtUntaxedCost.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedCost.TabIndex = 9;
this.Controls.Add(this.lblUntaxedCost);
this.Controls.Add(this.txtUntaxedCost);

           //#####CarryingSubtotalAmount###Decimal
this.lblCarryingSubtotalAmount.AutoSize = true;
this.lblCarryingSubtotalAmount.Location = new System.Drawing.Point(100,250);
this.lblCarryingSubtotalAmount.Name = "lblCarryingSubtotalAmount";
this.lblCarryingSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblCarryingSubtotalAmount.TabIndex = 10;
this.lblCarryingSubtotalAmount.Text = "载账小计";
//111======250
this.txtCarryingSubtotalAmount.Location = new System.Drawing.Point(173,246);
this.txtCarryingSubtotalAmount.Name ="txtCarryingSubtotalAmount";
this.txtCarryingSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtCarryingSubtotalAmount.TabIndex = 10;
this.Controls.Add(this.lblCarryingSubtotalAmount);
this.Controls.Add(this.txtCarryingSubtotalAmount);

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

           //#####CheckSubtotalAmount###Decimal
this.lblCheckSubtotalAmount.AutoSize = true;
this.lblCheckSubtotalAmount.Location = new System.Drawing.Point(100,300);
this.lblCheckSubtotalAmount.Name = "lblCheckSubtotalAmount";
this.lblCheckSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblCheckSubtotalAmount.TabIndex = 12;
this.lblCheckSubtotalAmount.Text = "盘点小计";
//111======300
this.txtCheckSubtotalAmount.Location = new System.Drawing.Point(173,296);
this.txtCheckSubtotalAmount.Name ="txtCheckSubtotalAmount";
this.txtCheckSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtCheckSubtotalAmount.TabIndex = 12;
this.Controls.Add(this.lblCheckSubtotalAmount);
this.Controls.Add(this.txtCheckSubtotalAmount);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,325);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 13;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,321);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 13;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,350);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 14;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,346);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 14;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

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
           // this.kryptonPanel1.TabIndex = 14;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMainID );
this.Controls.Add(this.cmbMainID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                this.Controls.Add(this.lblCarryinglQty );
this.Controls.Add(this.txtCarryinglQty );

                this.Controls.Add(this.lblDiffQty );
this.Controls.Add(this.txtDiffQty );

                this.Controls.Add(this.lblCheckQty );
this.Controls.Add(this.txtCheckQty );

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

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                            // 
            // "tb_StocktakeDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_StocktakeDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblMainID;
private Krypton.Toolkit.KryptonComboBox cmbMainID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRack_ID;
private Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCarryinglQty;
private Krypton.Toolkit.KryptonTextBox txtCarryinglQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffQty;
private Krypton.Toolkit.KryptonTextBox txtDiffQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckQty;
private Krypton.Toolkit.KryptonTextBox txtCheckQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCost;
private Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblUntaxedCost;
private Krypton.Toolkit.KryptonTextBox txtUntaxedCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblCarryingSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtCarryingSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtDiffSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtCheckSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

