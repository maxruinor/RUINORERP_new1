
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 位置信息
    /// </summary>
    partial class tb_PositionQuery
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
     
     this.lblLeft = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLeft = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBottom = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBottom = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTop = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTop = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50Left###String
this.lblLeft.AutoSize = true;
this.lblLeft.Location = new System.Drawing.Point(100,25);
this.lblLeft.Name = "lblLeft";
this.lblLeft.Size = new System.Drawing.Size(41, 12);
this.lblLeft.TabIndex = 1;
this.lblLeft.Text = "左边距";
this.txtLeft.Location = new System.Drawing.Point(173,21);
this.txtLeft.Name = "txtLeft";
this.txtLeft.Size = new System.Drawing.Size(100, 21);
this.txtLeft.TabIndex = 1;
this.Controls.Add(this.lblLeft);
this.Controls.Add(this.txtLeft);

           //#####50Right###String
this.lblRight.AutoSize = true;
this.lblRight.Location = new System.Drawing.Point(100,50);
this.lblRight.Name = "lblRight";
this.lblRight.Size = new System.Drawing.Size(41, 12);
this.lblRight.TabIndex = 2;
this.lblRight.Text = "右边距";
this.txtRight.Location = new System.Drawing.Point(173,46);
this.txtRight.Name = "txtRight";
this.txtRight.Size = new System.Drawing.Size(100, 21);
this.txtRight.TabIndex = 2;
this.Controls.Add(this.lblRight);
this.Controls.Add(this.txtRight);

           //#####50Bottom###String
this.lblBottom.AutoSize = true;
this.lblBottom.Location = new System.Drawing.Point(100,75);
this.lblBottom.Name = "lblBottom";
this.lblBottom.Size = new System.Drawing.Size(41, 12);
this.lblBottom.TabIndex = 3;
this.lblBottom.Text = "下边距";
this.txtBottom.Location = new System.Drawing.Point(173,71);
this.txtBottom.Name = "txtBottom";
this.txtBottom.Size = new System.Drawing.Size(100, 21);
this.txtBottom.TabIndex = 3;
this.Controls.Add(this.lblBottom);
this.Controls.Add(this.txtBottom);

           //#####50Top###String
this.lblTop.AutoSize = true;
this.lblTop.Location = new System.Drawing.Point(100,100);
this.lblTop.Name = "lblTop";
this.lblTop.Size = new System.Drawing.Size(41, 12);
this.lblTop.TabIndex = 4;
this.lblTop.Text = "上边距";
this.txtTop.Location = new System.Drawing.Point(173,96);
this.txtTop.Name = "txtTop";
this.txtTop.Size = new System.Drawing.Size(100, 21);
this.txtTop.TabIndex = 4;
this.Controls.Add(this.lblTop);
this.Controls.Add(this.txtTop);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblLeft );
this.Controls.Add(this.txtLeft );

                this.Controls.Add(this.lblRight );
this.Controls.Add(this.txtRight );

                this.Controls.Add(this.lblBottom );
this.Controls.Add(this.txtBottom );

                this.Controls.Add(this.lblTop );
this.Controls.Add(this.txtTop );

                    
            this.Name = "tb_PositionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLeft;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLeft;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBottom;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBottom;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTop;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTop;

    
    
   
 





    }
}


