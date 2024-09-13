// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:36
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
    partial class tb_BOM_SDetailSecondaryEdit
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
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblBOM_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblQty = new Krypton.Toolkit.KryptonLabel();
this.txtQty = new Krypton.Toolkit.KryptonTextBox();

this.lblScale = new Krypton.Toolkit.KryptonLabel();
this.txtScale = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCost = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCost = new Krypton.Toolkit.KryptonTextBox();

this.lblRemarks = new Krypton.Toolkit.KryptonLabel();
this.txtRemarks = new Krypton.Toolkit.KryptonTextBox();

    
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
//属性测试25ProdDetailID
//属性测试25ProdDetailID
//属性测试25ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,25);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 1;
this.lblProdDetailID.Text = "货品详情";
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

           //#####Location_ID###Int64
//属性测试75Location_ID
//属性测试75Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 3;
this.lblLocation_ID.Text = "仓库";
//111======75
this.cmbLocation_ID.Location = new System.Drawing.Point(173,71);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 3;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

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

           //#####Qty###Decimal
this.lblQty.AutoSize = true;
this.lblQty.Location = new System.Drawing.Point(100,125);
this.lblQty.Name = "lblQty";
this.lblQty.Size = new System.Drawing.Size(41, 12);
this.lblQty.TabIndex = 5;
this.lblQty.Text = "数量";
//111======125
this.txtQty.Location = new System.Drawing.Point(173,121);
this.txtQty.Name ="txtQty";
this.txtQty.Size = new System.Drawing.Size(100, 21);
this.txtQty.TabIndex = 5;
this.Controls.Add(this.lblQty);
this.Controls.Add(this.txtQty);

           //#####Scale###Decimal
this.lblScale.AutoSize = true;
this.lblScale.Location = new System.Drawing.Point(100,150);
this.lblScale.Name = "lblScale";
this.lblScale.Size = new System.Drawing.Size(41, 12);
this.lblScale.TabIndex = 6;
this.lblScale.Text = "比例";
//111======150
this.txtScale.Location = new System.Drawing.Point(173,146);
this.txtScale.Name ="txtScale";
this.txtScale.Size = new System.Drawing.Size(100, 21);
this.txtScale.TabIndex = 6;
this.Controls.Add(this.lblScale);
this.Controls.Add(this.txtScale);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,175);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 7;
this.lblUnitCost.Text = "单位成本";
//111======175
this.txtUnitCost.Location = new System.Drawing.Point(173,171);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 7;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####SubtotalCost###Decimal
this.lblSubtotalCost.AutoSize = true;
this.lblSubtotalCost.Location = new System.Drawing.Point(100,200);
this.lblSubtotalCost.Name = "lblSubtotalCost";
this.lblSubtotalCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCost.TabIndex = 8;
this.lblSubtotalCost.Text = "成本小计";
//111======200
this.txtSubtotalCost.Location = new System.Drawing.Point(173,196);
this.txtSubtotalCost.Name ="txtSubtotalCost";
this.txtSubtotalCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCost.TabIndex = 8;
this.Controls.Add(this.lblSubtotalCost);
this.Controls.Add(this.txtSubtotalCost);

           //#####200Remarks###String
this.lblRemarks.AutoSize = true;
this.lblRemarks.Location = new System.Drawing.Point(100,225);
this.lblRemarks.Name = "lblRemarks";
this.lblRemarks.Size = new System.Drawing.Size(41, 12);
this.lblRemarks.TabIndex = 9;
this.lblRemarks.Text = "备注说明";
this.txtRemarks.Location = new System.Drawing.Point(173,221);
this.txtRemarks.Name = "txtRemarks";
this.txtRemarks.Size = new System.Drawing.Size(100, 21);
this.txtRemarks.TabIndex = 9;
this.Controls.Add(this.lblRemarks);
this.Controls.Add(this.txtRemarks);

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
           // this.kryptonPanel1.TabIndex = 9;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblQty );
this.Controls.Add(this.txtQty );

                this.Controls.Add(this.lblScale );
this.Controls.Add(this.txtScale );

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                this.Controls.Add(this.lblSubtotalCost );
this.Controls.Add(this.txtSubtotalCost );

                this.Controls.Add(this.lblRemarks );
this.Controls.Add(this.txtRemarks );

                            // 
            // "tb_BOM_SDetailSecondaryEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_BOM_SDetailSecondaryEdit";
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
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_ID;
private Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblQty;
private Krypton.Toolkit.KryptonTextBox txtQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblScale;
private Krypton.Toolkit.KryptonTextBox txtScale;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitCost;
private Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalCost;
private Krypton.Toolkit.KryptonTextBox txtSubtotalCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblRemarks;
private Krypton.Toolkit.KryptonTextBox txtRemarks;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

