
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 14:54:18
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 卡通箱规格表
    /// </summary>
    partial class tb_CartoonBoxQuery
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
     
     this.lblCartonName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCartonName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblColor = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtColor = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMaterial = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaterial = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmptyBoxWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEmptyBoxWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMaxLoad = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaxLoad = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblThickness = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtThickness = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLength = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLength = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWidth = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWidth = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblHeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtHeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblVolume = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVolume = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFluteType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFluteType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrintType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrintType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomPrint = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomPrint = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDescription.Multiline = true;

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100CartonName###String
this.lblCartonName.AutoSize = true;
this.lblCartonName.Location = new System.Drawing.Point(100,25);
this.lblCartonName.Name = "lblCartonName";
this.lblCartonName.Size = new System.Drawing.Size(41, 12);
this.lblCartonName.TabIndex = 1;
this.lblCartonName.Text = "纸箱名称";
this.txtCartonName.Location = new System.Drawing.Point(173,21);
this.txtCartonName.Name = "txtCartonName";
this.txtCartonName.Size = new System.Drawing.Size(100, 21);
this.txtCartonName.TabIndex = 1;
this.Controls.Add(this.lblCartonName);
this.Controls.Add(this.txtCartonName);

           //#####100Color###String
this.lblColor.AutoSize = true;
this.lblColor.Location = new System.Drawing.Point(100,50);
this.lblColor.Name = "lblColor";
this.lblColor.Size = new System.Drawing.Size(41, 12);
this.lblColor.TabIndex = 2;
this.lblColor.Text = "颜色";
this.txtColor.Location = new System.Drawing.Point(173,46);
this.txtColor.Name = "txtColor";
this.txtColor.Size = new System.Drawing.Size(100, 21);
this.txtColor.TabIndex = 2;
this.Controls.Add(this.lblColor);
this.Controls.Add(this.txtColor);

           //#####100Material###String
this.lblMaterial.AutoSize = true;
this.lblMaterial.Location = new System.Drawing.Point(100,75);
this.lblMaterial.Name = "lblMaterial";
this.lblMaterial.Size = new System.Drawing.Size(41, 12);
this.lblMaterial.TabIndex = 3;
this.lblMaterial.Text = "材质";
this.txtMaterial.Location = new System.Drawing.Point(173,71);
this.txtMaterial.Name = "txtMaterial";
this.txtMaterial.Size = new System.Drawing.Size(100, 21);
this.txtMaterial.TabIndex = 3;
this.Controls.Add(this.lblMaterial);
this.Controls.Add(this.txtMaterial);

           //#####EmptyBoxWeight###Decimal
this.lblEmptyBoxWeight.AutoSize = true;
this.lblEmptyBoxWeight.Location = new System.Drawing.Point(100,100);
this.lblEmptyBoxWeight.Name = "lblEmptyBoxWeight";
this.lblEmptyBoxWeight.Size = new System.Drawing.Size(41, 12);
this.lblEmptyBoxWeight.TabIndex = 4;
this.lblEmptyBoxWeight.Text = "空箱重(kg)";
//111======100
this.txtEmptyBoxWeight.Location = new System.Drawing.Point(173,96);
this.txtEmptyBoxWeight.Name ="txtEmptyBoxWeight";
this.txtEmptyBoxWeight.Size = new System.Drawing.Size(100, 21);
this.txtEmptyBoxWeight.TabIndex = 4;
this.Controls.Add(this.lblEmptyBoxWeight);
this.Controls.Add(this.txtEmptyBoxWeight);

           //#####MaxLoad###Decimal
this.lblMaxLoad.AutoSize = true;
this.lblMaxLoad.Location = new System.Drawing.Point(100,125);
this.lblMaxLoad.Name = "lblMaxLoad";
this.lblMaxLoad.Size = new System.Drawing.Size(41, 12);
this.lblMaxLoad.TabIndex = 5;
this.lblMaxLoad.Text = "最大承重(kg)";
//111======125
this.txtMaxLoad.Location = new System.Drawing.Point(173,121);
this.txtMaxLoad.Name ="txtMaxLoad";
this.txtMaxLoad.Size = new System.Drawing.Size(100, 21);
this.txtMaxLoad.TabIndex = 5;
this.Controls.Add(this.lblMaxLoad);
this.Controls.Add(this.txtMaxLoad);

           //#####Thickness###Decimal
this.lblThickness.AutoSize = true;
this.lblThickness.Location = new System.Drawing.Point(100,150);
this.lblThickness.Name = "lblThickness";
this.lblThickness.Size = new System.Drawing.Size(41, 12);
this.lblThickness.TabIndex = 6;
this.lblThickness.Text = "纸板厚度(cm)";
//111======150
this.txtThickness.Location = new System.Drawing.Point(173,146);
this.txtThickness.Name ="txtThickness";
this.txtThickness.Size = new System.Drawing.Size(100, 21);
this.txtThickness.TabIndex = 6;
this.Controls.Add(this.lblThickness);
this.Controls.Add(this.txtThickness);

           //#####Length###Decimal
this.lblLength.AutoSize = true;
this.lblLength.Location = new System.Drawing.Point(100,175);
this.lblLength.Name = "lblLength";
this.lblLength.Size = new System.Drawing.Size(41, 12);
this.lblLength.TabIndex = 7;
this.lblLength.Text = "长度(cm)";
//111======175
this.txtLength.Location = new System.Drawing.Point(173,171);
this.txtLength.Name ="txtLength";
this.txtLength.Size = new System.Drawing.Size(100, 21);
this.txtLength.TabIndex = 7;
this.Controls.Add(this.lblLength);
this.Controls.Add(this.txtLength);

           //#####Width###Decimal
this.lblWidth.AutoSize = true;
this.lblWidth.Location = new System.Drawing.Point(100,200);
this.lblWidth.Name = "lblWidth";
this.lblWidth.Size = new System.Drawing.Size(41, 12);
this.lblWidth.TabIndex = 8;
this.lblWidth.Text = "宽度(cm)";
//111======200
this.txtWidth.Location = new System.Drawing.Point(173,196);
this.txtWidth.Name ="txtWidth";
this.txtWidth.Size = new System.Drawing.Size(100, 21);
this.txtWidth.TabIndex = 8;
this.Controls.Add(this.lblWidth);
this.Controls.Add(this.txtWidth);

           //#####Height###Decimal
this.lblHeight.AutoSize = true;
this.lblHeight.Location = new System.Drawing.Point(100,225);
this.lblHeight.Name = "lblHeight";
this.lblHeight.Size = new System.Drawing.Size(41, 12);
this.lblHeight.TabIndex = 9;
this.lblHeight.Text = "高度(cm)";
//111======225
this.txtHeight.Location = new System.Drawing.Point(173,221);
this.txtHeight.Name ="txtHeight";
this.txtHeight.Size = new System.Drawing.Size(100, 21);
this.txtHeight.TabIndex = 9;
this.Controls.Add(this.lblHeight);
this.Controls.Add(this.txtHeight);

           //#####Volume###Decimal
this.lblVolume.AutoSize = true;
this.lblVolume.Location = new System.Drawing.Point(100,250);
this.lblVolume.Name = "lblVolume";
this.lblVolume.Size = new System.Drawing.Size(41, 12);
this.lblVolume.TabIndex = 10;
this.lblVolume.Text = "体积Vol(cm³)";
//111======250
this.txtVolume.Location = new System.Drawing.Point(173,246);
this.txtVolume.Name ="txtVolume";
this.txtVolume.Size = new System.Drawing.Size(100, 21);
this.txtVolume.TabIndex = 10;
this.Controls.Add(this.lblVolume);
this.Controls.Add(this.txtVolume);

           //#####100FluteType###String
this.lblFluteType.AutoSize = true;
this.lblFluteType.Location = new System.Drawing.Point(100,275);
this.lblFluteType.Name = "lblFluteType";
this.lblFluteType.Size = new System.Drawing.Size(41, 12);
this.lblFluteType.TabIndex = 11;
this.lblFluteType.Text = "瓦楞类型";
this.txtFluteType.Location = new System.Drawing.Point(173,271);
this.txtFluteType.Name = "txtFluteType";
this.txtFluteType.Size = new System.Drawing.Size(100, 21);
this.txtFluteType.TabIndex = 11;
this.Controls.Add(this.lblFluteType);
this.Controls.Add(this.txtFluteType);

           //#####100PrintType###String
this.lblPrintType.AutoSize = true;
this.lblPrintType.Location = new System.Drawing.Point(100,300);
this.lblPrintType.Name = "lblPrintType";
this.lblPrintType.Size = new System.Drawing.Size(41, 12);
this.lblPrintType.TabIndex = 12;
this.lblPrintType.Text = "印刷类型";
this.txtPrintType.Location = new System.Drawing.Point(173,296);
this.txtPrintType.Name = "txtPrintType";
this.txtPrintType.Size = new System.Drawing.Size(100, 21);
this.txtPrintType.TabIndex = 12;
this.Controls.Add(this.lblPrintType);
this.Controls.Add(this.txtPrintType);

           //#####100CustomPrint###String
this.lblCustomPrint.AutoSize = true;
this.lblCustomPrint.Location = new System.Drawing.Point(100,325);
this.lblCustomPrint.Name = "lblCustomPrint";
this.lblCustomPrint.Size = new System.Drawing.Size(41, 12);
this.lblCustomPrint.TabIndex = 13;
this.lblCustomPrint.Text = "定制印刷";
this.txtCustomPrint.Location = new System.Drawing.Point(173,321);
this.txtCustomPrint.Name = "txtCustomPrint";
this.txtCustomPrint.Size = new System.Drawing.Size(100, 21);
this.txtCustomPrint.TabIndex = 13;
this.Controls.Add(this.lblCustomPrint);
this.Controls.Add(this.txtCustomPrint);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,350);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 14;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,346);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 14;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####255Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,375);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 15;
this.lblDescription.Text = "备注";
this.txtDescription.Location = new System.Drawing.Point(173,371);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 15;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,400);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 16;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,396);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 16;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,425);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 17;
this.lblCreated_at.Text = "创建时间";
//111======425
this.dtpCreated_at.Location = new System.Drawing.Point(173,421);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 17;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,475);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 19;
this.lblModified_at.Text = "修改时间";
//111======475
this.dtpModified_at.Location = new System.Drawing.Point(173,471);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 19;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCartonName );
this.Controls.Add(this.txtCartonName );

                this.Controls.Add(this.lblColor );
this.Controls.Add(this.txtColor );

                this.Controls.Add(this.lblMaterial );
this.Controls.Add(this.txtMaterial );

                this.Controls.Add(this.lblEmptyBoxWeight );
this.Controls.Add(this.txtEmptyBoxWeight );

                this.Controls.Add(this.lblMaxLoad );
this.Controls.Add(this.txtMaxLoad );

                this.Controls.Add(this.lblThickness );
this.Controls.Add(this.txtThickness );

                this.Controls.Add(this.lblLength );
this.Controls.Add(this.txtLength );

                this.Controls.Add(this.lblWidth );
this.Controls.Add(this.txtWidth );

                this.Controls.Add(this.lblHeight );
this.Controls.Add(this.txtHeight );

                this.Controls.Add(this.lblVolume );
this.Controls.Add(this.txtVolume );

                this.Controls.Add(this.lblFluteType );
this.Controls.Add(this.txtFluteType );

                this.Controls.Add(this.lblPrintType );
this.Controls.Add(this.txtPrintType );

                this.Controls.Add(this.lblCustomPrint );
this.Controls.Add(this.txtCustomPrint );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_CartoonBoxQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCartonName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCartonName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblColor;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtColor;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterial;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaterial;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmptyBoxWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEmptyBoxWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaxLoad;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaxLoad;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblThickness;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtThickness;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLength;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLength;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWidth;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWidth;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblHeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtHeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVolume;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVolume;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFluteType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFluteType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrintType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrintType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomPrint;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomPrint;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


