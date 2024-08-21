
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/31/2024 20:19:57
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 订单装箱单，针对特别订单详细处理 也用主子表来做。暂时不搞
    /// </summary>
    partial class tb_OrderPackingQuery
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
     
     this.lblSOrder_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbSOrder_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBoxNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBoxNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBoxMark = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBoxMark = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRemarks = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemarks = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRemarks.Multiline = true;


this.lblLength = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLength = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWidth = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWidth = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblHeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtHeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblBoxMaterial = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBoxMaterial = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblVolume = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVolume = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblGrossWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGrossWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNetWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNetWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPackingMethod = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPackingMethod = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####SOrder_ID###Int64
//属性测试25SOrder_ID
this.lblSOrder_ID.AutoSize = true;
this.lblSOrder_ID.Location = new System.Drawing.Point(100,25);
this.lblSOrder_ID.Name = "lblSOrder_ID";
this.lblSOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblSOrder_ID.TabIndex = 1;
this.lblSOrder_ID.Text = "订单";
//111======25
this.cmbSOrder_ID.Location = new System.Drawing.Point(173,21);
this.cmbSOrder_ID.Name ="cmbSOrder_ID";
this.cmbSOrder_ID.Size = new System.Drawing.Size(100, 21);
this.cmbSOrder_ID.TabIndex = 1;
this.Controls.Add(this.lblSOrder_ID);
this.Controls.Add(this.cmbSOrder_ID);

           //#####50BoxNo###String
this.lblBoxNo.AutoSize = true;
this.lblBoxNo.Location = new System.Drawing.Point(100,50);
this.lblBoxNo.Name = "lblBoxNo";
this.lblBoxNo.Size = new System.Drawing.Size(41, 12);
this.lblBoxNo.TabIndex = 2;
this.lblBoxNo.Text = "箱号";
this.txtBoxNo.Location = new System.Drawing.Point(173,46);
this.txtBoxNo.Name = "txtBoxNo";
this.txtBoxNo.Size = new System.Drawing.Size(100, 21);
this.txtBoxNo.TabIndex = 2;
this.Controls.Add(this.lblBoxNo);
this.Controls.Add(this.txtBoxNo);

           //#####100BoxMark###String
this.lblBoxMark.AutoSize = true;
this.lblBoxMark.Location = new System.Drawing.Point(100,75);
this.lblBoxMark.Name = "lblBoxMark";
this.lblBoxMark.Size = new System.Drawing.Size(41, 12);
this.lblBoxMark.TabIndex = 3;
this.lblBoxMark.Text = "箱唛";
this.txtBoxMark.Location = new System.Drawing.Point(173,71);
this.txtBoxMark.Name = "txtBoxMark";
this.txtBoxMark.Size = new System.Drawing.Size(100, 21);
this.txtBoxMark.TabIndex = 3;
this.Controls.Add(this.lblBoxMark);
this.Controls.Add(this.txtBoxMark);

           //#####255Remarks###String
this.lblRemarks.AutoSize = true;
this.lblRemarks.Location = new System.Drawing.Point(100,100);
this.lblRemarks.Name = "lblRemarks";
this.lblRemarks.Size = new System.Drawing.Size(41, 12);
this.lblRemarks.TabIndex = 4;
this.lblRemarks.Text = "备注";
this.txtRemarks.Location = new System.Drawing.Point(173,96);
this.txtRemarks.Name = "txtRemarks";
this.txtRemarks.Size = new System.Drawing.Size(100, 21);
this.txtRemarks.TabIndex = 4;
this.Controls.Add(this.lblRemarks);
this.Controls.Add(this.txtRemarks);

           //#####QuantityPerBox###Int32
//属性测试125QuantityPerBox

           //#####Length###Decimal
this.lblLength.AutoSize = true;
this.lblLength.Location = new System.Drawing.Point(100,150);
this.lblLength.Name = "lblLength";
this.lblLength.Size = new System.Drawing.Size(41, 12);
this.lblLength.TabIndex = 6;
this.lblLength.Text = "长度(CM)";
//111======150
this.txtLength.Location = new System.Drawing.Point(173,146);
this.txtLength.Name ="txtLength";
this.txtLength.Size = new System.Drawing.Size(100, 21);
this.txtLength.TabIndex = 6;
this.Controls.Add(this.lblLength);
this.Controls.Add(this.txtLength);

           //#####Width###Decimal
this.lblWidth.AutoSize = true;
this.lblWidth.Location = new System.Drawing.Point(100,175);
this.lblWidth.Name = "lblWidth";
this.lblWidth.Size = new System.Drawing.Size(41, 12);
this.lblWidth.TabIndex = 7;
this.lblWidth.Text = "宽度(CM)";
//111======175
this.txtWidth.Location = new System.Drawing.Point(173,171);
this.txtWidth.Name ="txtWidth";
this.txtWidth.Size = new System.Drawing.Size(100, 21);
this.txtWidth.TabIndex = 7;
this.Controls.Add(this.lblWidth);
this.Controls.Add(this.txtWidth);

           //#####Height###Decimal
this.lblHeight.AutoSize = true;
this.lblHeight.Location = new System.Drawing.Point(100,200);
this.lblHeight.Name = "lblHeight";
this.lblHeight.Size = new System.Drawing.Size(41, 12);
this.lblHeight.TabIndex = 8;
this.lblHeight.Text = "高度(CM)";
//111======200
this.txtHeight.Location = new System.Drawing.Point(173,196);
this.txtHeight.Name ="txtHeight";
this.txtHeight.Size = new System.Drawing.Size(100, 21);
this.txtHeight.TabIndex = 8;
this.Controls.Add(this.lblHeight);
this.Controls.Add(this.txtHeight);

           //#####Created_by###Int64
//属性测试225Created_by

           //#####200BoxMaterial###String
this.lblBoxMaterial.AutoSize = true;
this.lblBoxMaterial.Location = new System.Drawing.Point(100,250);
this.lblBoxMaterial.Name = "lblBoxMaterial";
this.lblBoxMaterial.Size = new System.Drawing.Size(41, 12);
this.lblBoxMaterial.TabIndex = 10;
this.lblBoxMaterial.Text = "箱子材质";
this.txtBoxMaterial.Location = new System.Drawing.Point(173,246);
this.txtBoxMaterial.Name = "txtBoxMaterial";
this.txtBoxMaterial.Size = new System.Drawing.Size(100, 21);
this.txtBoxMaterial.TabIndex = 10;
this.Controls.Add(this.lblBoxMaterial);
this.Controls.Add(this.txtBoxMaterial);

           //#####Volume###Decimal
this.lblVolume.AutoSize = true;
this.lblVolume.Location = new System.Drawing.Point(100,275);
this.lblVolume.Name = "lblVolume";
this.lblVolume.Size = new System.Drawing.Size(41, 12);
this.lblVolume.TabIndex = 11;
this.lblVolume.Text = "体积(CM)";
//111======275
this.txtVolume.Location = new System.Drawing.Point(173,271);
this.txtVolume.Name ="txtVolume";
this.txtVolume.Size = new System.Drawing.Size(100, 21);
this.txtVolume.TabIndex = 11;
this.Controls.Add(this.lblVolume);
this.Controls.Add(this.txtVolume);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,300);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 12;
this.lblCreated_at.Text = "创建时间";
//111======300
this.dtpCreated_at.Location = new System.Drawing.Point(173,296);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 12;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,325);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 13;
this.lblModified_at.Text = "修改时间";
//111======325
this.dtpModified_at.Location = new System.Drawing.Point(173,321);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 13;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试350Modified_by

           //#####GrossWeight###Decimal
this.lblGrossWeight.AutoSize = true;
this.lblGrossWeight.Location = new System.Drawing.Point(100,375);
this.lblGrossWeight.Name = "lblGrossWeight";
this.lblGrossWeight.Size = new System.Drawing.Size(41, 12);
this.lblGrossWeight.TabIndex = 15;
this.lblGrossWeight.Text = "毛重(KG)";
//111======375
this.txtGrossWeight.Location = new System.Drawing.Point(173,371);
this.txtGrossWeight.Name ="txtGrossWeight";
this.txtGrossWeight.Size = new System.Drawing.Size(100, 21);
this.txtGrossWeight.TabIndex = 15;
this.Controls.Add(this.lblGrossWeight);
this.Controls.Add(this.txtGrossWeight);

           //#####NetWeight###Decimal
this.lblNetWeight.AutoSize = true;
this.lblNetWeight.Location = new System.Drawing.Point(100,400);
this.lblNetWeight.Name = "lblNetWeight";
this.lblNetWeight.Size = new System.Drawing.Size(41, 12);
this.lblNetWeight.TabIndex = 16;
this.lblNetWeight.Text = "净重(KG)";
//111======400
this.txtNetWeight.Location = new System.Drawing.Point(173,396);
this.txtNetWeight.Name ="txtNetWeight";
this.txtNetWeight.Size = new System.Drawing.Size(100, 21);
this.txtNetWeight.TabIndex = 16;
this.Controls.Add(this.lblNetWeight);
this.Controls.Add(this.txtNetWeight);

           //#####100PackingMethod###String
this.lblPackingMethod.AutoSize = true;
this.lblPackingMethod.Location = new System.Drawing.Point(100,425);
this.lblPackingMethod.Name = "lblPackingMethod";
this.lblPackingMethod.Size = new System.Drawing.Size(41, 12);
this.lblPackingMethod.TabIndex = 17;
this.lblPackingMethod.Text = "打包方式";
this.txtPackingMethod.Location = new System.Drawing.Point(173,421);
this.txtPackingMethod.Name = "txtPackingMethod";
this.txtPackingMethod.Size = new System.Drawing.Size(100, 21);
this.txtPackingMethod.TabIndex = 17;
this.Controls.Add(this.lblPackingMethod);
this.Controls.Add(this.txtPackingMethod);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSOrder_ID );
this.Controls.Add(this.cmbSOrder_ID );

                this.Controls.Add(this.lblBoxNo );
this.Controls.Add(this.txtBoxNo );

                this.Controls.Add(this.lblBoxMark );
this.Controls.Add(this.txtBoxMark );

                this.Controls.Add(this.lblRemarks );
this.Controls.Add(this.txtRemarks );

                
                this.Controls.Add(this.lblLength );
this.Controls.Add(this.txtLength );

                this.Controls.Add(this.lblWidth );
this.Controls.Add(this.txtWidth );

                this.Controls.Add(this.lblHeight );
this.Controls.Add(this.txtHeight );

                
                this.Controls.Add(this.lblBoxMaterial );
this.Controls.Add(this.txtBoxMaterial );

                this.Controls.Add(this.lblVolume );
this.Controls.Add(this.txtVolume );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblGrossWeight );
this.Controls.Add(this.txtGrossWeight );

                this.Controls.Add(this.lblNetWeight );
this.Controls.Add(this.txtNetWeight );

                this.Controls.Add(this.lblPackingMethod );
this.Controls.Add(this.txtPackingMethod );

                    
            this.Name = "tb_OrderPackingQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSOrder_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSOrder_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBoxNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBoxNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBoxMark;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBoxMark;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemarks;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemarks;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLength;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLength;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWidth;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWidth;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblHeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtHeight;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBoxMaterial;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBoxMaterial;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVolume;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVolume;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGrossWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGrossWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNetWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNetWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPackingMethod;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPackingMethod;

    
    
   
 





    }
}


