// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:30:05
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 库存流水表
    /// </summary>
    partial class tb_InventoryTransactionEdit
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
     this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblBizType = new Krypton.Toolkit.KryptonLabel();
this.txtBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblReferenceId = new Krypton.Toolkit.KryptonLabel();
this.txtReferenceId = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantityChange = new Krypton.Toolkit.KryptonLabel();
this.txtQuantityChange = new Krypton.Toolkit.KryptonTextBox();

this.lblAfterQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtAfterQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblBatchNumber = new Krypton.Toolkit.KryptonLabel();
this.txtBatchNumber = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTransactionTime = new Krypton.Toolkit.KryptonLabel();
this.dtpTransactionTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblOperatorId = new Krypton.Toolkit.KryptonLabel();
this.txtOperatorId = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    
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
     
            //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,25);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 1;
this.lblProdDetailID.Text = "产品详情";
this.txtProdDetailID.Location = new System.Drawing.Point(173,21);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 1;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####Location_ID###Int64
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,50);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 2;
this.lblLocation_ID.Text = "库位";
this.txtLocation_ID.Location = new System.Drawing.Point(173,46);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 2;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####BizType###Int32
this.lblBizType.AutoSize = true;
this.lblBizType.Location = new System.Drawing.Point(100,75);
this.lblBizType.Name = "lblBizType";
this.lblBizType.Size = new System.Drawing.Size(41, 12);
this.lblBizType.TabIndex = 3;
this.lblBizType.Text = "业务类型";
this.txtBizType.Location = new System.Drawing.Point(173,71);
this.txtBizType.Name = "txtBizType";
this.txtBizType.Size = new System.Drawing.Size(100, 21);
this.txtBizType.TabIndex = 3;
this.Controls.Add(this.lblBizType);
this.Controls.Add(this.txtBizType);

           //#####ReferenceId###Int64
this.lblReferenceId.AutoSize = true;
this.lblReferenceId.Location = new System.Drawing.Point(100,100);
this.lblReferenceId.Name = "lblReferenceId";
this.lblReferenceId.Size = new System.Drawing.Size(41, 12);
this.lblReferenceId.TabIndex = 4;
this.lblReferenceId.Text = "业务单据";
this.txtReferenceId.Location = new System.Drawing.Point(173,96);
this.txtReferenceId.Name = "txtReferenceId";
this.txtReferenceId.Size = new System.Drawing.Size(100, 21);
this.txtReferenceId.TabIndex = 4;
this.Controls.Add(this.lblReferenceId);
this.Controls.Add(this.txtReferenceId);

           //#####QuantityChange###Int32
this.lblQuantityChange.AutoSize = true;
this.lblQuantityChange.Location = new System.Drawing.Point(100,125);
this.lblQuantityChange.Name = "lblQuantityChange";
this.lblQuantityChange.Size = new System.Drawing.Size(41, 12);
this.lblQuantityChange.TabIndex = 5;
this.lblQuantityChange.Text = "变动数量";
this.txtQuantityChange.Location = new System.Drawing.Point(173,121);
this.txtQuantityChange.Name = "txtQuantityChange";
this.txtQuantityChange.Size = new System.Drawing.Size(100, 21);
this.txtQuantityChange.TabIndex = 5;
this.Controls.Add(this.lblQuantityChange);
this.Controls.Add(this.txtQuantityChange);

           //#####AfterQuantity###Int32
this.lblAfterQuantity.AutoSize = true;
this.lblAfterQuantity.Location = new System.Drawing.Point(100,150);
this.lblAfterQuantity.Name = "lblAfterQuantity";
this.lblAfterQuantity.Size = new System.Drawing.Size(41, 12);
this.lblAfterQuantity.TabIndex = 6;
this.lblAfterQuantity.Text = "变后数量";
this.txtAfterQuantity.Location = new System.Drawing.Point(173,146);
this.txtAfterQuantity.Name = "txtAfterQuantity";
this.txtAfterQuantity.Size = new System.Drawing.Size(100, 21);
this.txtAfterQuantity.TabIndex = 6;
this.Controls.Add(this.lblAfterQuantity);
this.Controls.Add(this.txtAfterQuantity);

           //#####BatchNumber###Int32
this.lblBatchNumber.AutoSize = true;
this.lblBatchNumber.Location = new System.Drawing.Point(100,175);
this.lblBatchNumber.Name = "lblBatchNumber";
this.lblBatchNumber.Size = new System.Drawing.Size(41, 12);
this.lblBatchNumber.TabIndex = 7;
this.lblBatchNumber.Text = "批号";
this.txtBatchNumber.Location = new System.Drawing.Point(173,171);
this.txtBatchNumber.Name = "txtBatchNumber";
this.txtBatchNumber.Size = new System.Drawing.Size(100, 21);
this.txtBatchNumber.TabIndex = 7;
this.Controls.Add(this.lblBatchNumber);
this.Controls.Add(this.txtBatchNumber);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,200);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 8;
this.lblUnitCost.Text = "单位成本";
//111======200
this.txtUnitCost.Location = new System.Drawing.Point(173,196);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 8;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####TransactionTime###DateTime
this.lblTransactionTime.AutoSize = true;
this.lblTransactionTime.Location = new System.Drawing.Point(100,225);
this.lblTransactionTime.Name = "lblTransactionTime";
this.lblTransactionTime.Size = new System.Drawing.Size(41, 12);
this.lblTransactionTime.TabIndex = 9;
this.lblTransactionTime.Text = "操作时间";
//111======225
this.dtpTransactionTime.Location = new System.Drawing.Point(173,221);
this.dtpTransactionTime.Name ="dtpTransactionTime";
this.dtpTransactionTime.Size = new System.Drawing.Size(100, 21);
this.dtpTransactionTime.TabIndex = 9;
this.Controls.Add(this.lblTransactionTime);
this.Controls.Add(this.dtpTransactionTime);

           //#####OperatorId###Int64
this.lblOperatorId.AutoSize = true;
this.lblOperatorId.Location = new System.Drawing.Point(100,250);
this.lblOperatorId.Name = "lblOperatorId";
this.lblOperatorId.Size = new System.Drawing.Size(41, 12);
this.lblOperatorId.TabIndex = 10;
this.lblOperatorId.Text = "操作人";
this.txtOperatorId.Location = new System.Drawing.Point(173,246);
this.txtOperatorId.Name = "txtOperatorId";
this.txtOperatorId.Size = new System.Drawing.Size(100, 21);
this.txtOperatorId.TabIndex = 10;
this.Controls.Add(this.lblOperatorId);
this.Controls.Add(this.txtOperatorId);

           //#####250Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,275);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 11;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,271);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 11;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

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
           // this.kryptonPanel1.TabIndex = 11;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblBizType );
this.Controls.Add(this.txtBizType );

                this.Controls.Add(this.lblReferenceId );
this.Controls.Add(this.txtReferenceId );

                this.Controls.Add(this.lblQuantityChange );
this.Controls.Add(this.txtQuantityChange );

                this.Controls.Add(this.lblAfterQuantity );
this.Controls.Add(this.txtAfterQuantity );

                this.Controls.Add(this.lblBatchNumber );
this.Controls.Add(this.txtBatchNumber );

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                this.Controls.Add(this.lblTransactionTime );
this.Controls.Add(this.dtpTransactionTime );

                this.Controls.Add(this.lblOperatorId );
this.Controls.Add(this.txtOperatorId );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_InventoryTransactionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_InventoryTransactionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBizType;
private Krypton.Toolkit.KryptonTextBox txtBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblReferenceId;
private Krypton.Toolkit.KryptonTextBox txtReferenceId;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantityChange;
private Krypton.Toolkit.KryptonTextBox txtQuantityChange;

    
        
              private Krypton.Toolkit.KryptonLabel lblAfterQuantity;
private Krypton.Toolkit.KryptonTextBox txtAfterQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblBatchNumber;
private Krypton.Toolkit.KryptonTextBox txtBatchNumber;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitCost;
private Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTransactionTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpTransactionTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblOperatorId;
private Krypton.Toolkit.KryptonTextBox txtOperatorId;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

