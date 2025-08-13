
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:30:06
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
    partial class tb_InventoryTransactionQuery
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
     
     






this.lblUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTransactionTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpTransactionTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdDetailID###Int64

           //#####Location_ID###Int64

           //#####BizType###Int32

           //#####ReferenceId###Int64

           //#####QuantityChange###Int32

           //#####AfterQuantity###Int32

           //#####BatchNumber###Int32

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                
                
                
                
                
                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                this.Controls.Add(this.lblTransactionTime );
this.Controls.Add(this.dtpTransactionTime );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_InventoryTransactionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransactionTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpTransactionTime;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


