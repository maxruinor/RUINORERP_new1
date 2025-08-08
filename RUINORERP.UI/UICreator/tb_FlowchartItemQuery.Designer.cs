
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 流程图子项
    /// </summary>
    partial class tb_FlowchartItemQuery
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
     
     this.lblIconFile_Path = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtIconFile_Path = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtIconFile_Path.Multiline = true;

this.lblTitle = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTitle = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSizeString = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSizeString = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPointToString = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPointToString = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####500IconFile_Path###String
this.lblIconFile_Path.AutoSize = true;
this.lblIconFile_Path.Location = new System.Drawing.Point(100,25);
this.lblIconFile_Path.Name = "lblIconFile_Path";
this.lblIconFile_Path.Size = new System.Drawing.Size(41, 12);
this.lblIconFile_Path.TabIndex = 1;
this.lblIconFile_Path.Text = "";
this.txtIconFile_Path.Location = new System.Drawing.Point(173,21);
this.txtIconFile_Path.Name = "txtIconFile_Path";
this.txtIconFile_Path.Size = new System.Drawing.Size(100, 21);
this.txtIconFile_Path.TabIndex = 1;
this.Controls.Add(this.lblIconFile_Path);
this.Controls.Add(this.txtIconFile_Path);

           //#####100Title###String
this.lblTitle.AutoSize = true;
this.lblTitle.Location = new System.Drawing.Point(100,50);
this.lblTitle.Name = "lblTitle";
this.lblTitle.Size = new System.Drawing.Size(41, 12);
this.lblTitle.TabIndex = 2;
this.lblTitle.Text = "标题";
this.txtTitle.Location = new System.Drawing.Point(173,46);
this.txtTitle.Name = "txtTitle";
this.txtTitle.Size = new System.Drawing.Size(100, 21);
this.txtTitle.TabIndex = 2;
this.Controls.Add(this.lblTitle);
this.Controls.Add(this.txtTitle);

           //#####100SizeString###String
this.lblSizeString.AutoSize = true;
this.lblSizeString.Location = new System.Drawing.Point(100,75);
this.lblSizeString.Name = "lblSizeString";
this.lblSizeString.Size = new System.Drawing.Size(41, 12);
this.lblSizeString.TabIndex = 3;
this.lblSizeString.Text = "大小";
this.txtSizeString.Location = new System.Drawing.Point(173,71);
this.txtSizeString.Name = "txtSizeString";
this.txtSizeString.Size = new System.Drawing.Size(100, 21);
this.txtSizeString.TabIndex = 3;
this.Controls.Add(this.lblSizeString);
this.Controls.Add(this.txtSizeString);

           //#####100PointToString###String
this.lblPointToString.AutoSize = true;
this.lblPointToString.Location = new System.Drawing.Point(100,100);
this.lblPointToString.Name = "lblPointToString";
this.lblPointToString.Size = new System.Drawing.Size(41, 12);
this.lblPointToString.TabIndex = 4;
this.lblPointToString.Text = "位置";
this.txtPointToString.Location = new System.Drawing.Point(173,96);
this.txtPointToString.Name = "txtPointToString";
this.txtPointToString.Size = new System.Drawing.Size(100, 21);
this.txtPointToString.TabIndex = 4;
this.Controls.Add(this.lblPointToString);
this.Controls.Add(this.txtPointToString);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblIconFile_Path );
this.Controls.Add(this.txtIconFile_Path );

                this.Controls.Add(this.lblTitle );
this.Controls.Add(this.txtTitle );

                this.Controls.Add(this.lblSizeString );
this.Controls.Add(this.txtSizeString );

                this.Controls.Add(this.lblPointToString );
this.Controls.Add(this.txtPointToString );

                    
            this.Name = "tb_FlowchartItemQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIconFile_Path;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtIconFile_Path;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTitle;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTitle;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSizeString;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSizeString;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPointToString;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPointToString;

    
    
   
 





    }
}


