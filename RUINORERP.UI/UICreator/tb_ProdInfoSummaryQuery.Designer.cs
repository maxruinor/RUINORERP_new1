
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:56
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 商品信息汇总
    /// </summary>
    partial class tb_ProdInfoSummaryQuery
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
     
     this.lbl平均价格 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt平均价格 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



    //for end
    this.SuspendLayout();
    
         //for start
                 //#####平均价格###Decimal
this.lbl平均价格.AutoSize = true;
this.lbl平均价格.Location = new System.Drawing.Point(100,25);
this.lbl平均价格.Name = "lbl平均价格";
this.lbl平均价格.Size = new System.Drawing.Size(41, 12);
this.lbl平均价格.TabIndex = 1;
this.lbl平均价格.Text = "平均价格";
//111======25
this.txt平均价格.Location = new System.Drawing.Point(173,21);
this.txt平均价格.Name ="txt平均价格";
this.txt平均价格.Size = new System.Drawing.Size(100, 21);
this.txt平均价格.TabIndex = 1;
this.Controls.Add(this.lbl平均价格);
this.Controls.Add(this.txt平均价格);

           //#####总销售量###Int32

           //#####库存总量###Int32

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lbl平均价格 );
this.Controls.Add(this.txt平均价格 );

                
                
                    
            this.Name = "tb_ProdInfoSummaryQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl平均价格;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt平均价格;

    
        
              
    
        
              
    
    
   
 





    }
}


