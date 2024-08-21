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
    partial class tb_CartoonBoxEdit
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
            this.lblCartonName = new Krypton.Toolkit.KryptonLabel();
            this.txtCartonName = new Krypton.Toolkit.KryptonTextBox();
            this.lblColor = new Krypton.Toolkit.KryptonLabel();
            this.txtColor = new Krypton.Toolkit.KryptonTextBox();
            this.lblMaterial = new Krypton.Toolkit.KryptonLabel();
            this.txtMaterial = new Krypton.Toolkit.KryptonTextBox();
            this.lblEmptyBoxWeight = new Krypton.Toolkit.KryptonLabel();
            this.txtEmptyBoxWeight = new Krypton.Toolkit.KryptonTextBox();
            this.lblMaxLoad = new Krypton.Toolkit.KryptonLabel();
            this.txtMaxLoad = new Krypton.Toolkit.KryptonTextBox();
            this.lblThickness = new Krypton.Toolkit.KryptonLabel();
            this.txtThickness = new Krypton.Toolkit.KryptonTextBox();
            this.lblLength = new Krypton.Toolkit.KryptonLabel();
            this.txtLength = new Krypton.Toolkit.KryptonTextBox();
            this.lblWidth = new Krypton.Toolkit.KryptonLabel();
            this.txtWidth = new Krypton.Toolkit.KryptonTextBox();
            this.lblHeight = new Krypton.Toolkit.KryptonLabel();
            this.txtHeight = new Krypton.Toolkit.KryptonTextBox();
            this.lblVolume = new Krypton.Toolkit.KryptonLabel();
            this.txtVolume = new Krypton.Toolkit.KryptonTextBox();
            this.lblFluteType = new Krypton.Toolkit.KryptonLabel();
            this.txtFluteType = new Krypton.Toolkit.KryptonTextBox();
            this.lblPrintType = new Krypton.Toolkit.KryptonLabel();
            this.txtPrintType = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomPrint = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomPrint = new Krypton.Toolkit.KryptonTextBox();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblDescription = new Krypton.Toolkit.KryptonLabel();
            this.txtDescription = new Krypton.Toolkit.KryptonTextBox();
            this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
            this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
            this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
            this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
            this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();
            this.SuspendLayout();
            // 
            // lblCartonName
            // 
            this.lblCartonName.Location = new System.Drawing.Point(100, 25);
            this.lblCartonName.Name = "lblCartonName";
            this.lblCartonName.Size = new System.Drawing.Size(62, 20);
            this.lblCartonName.TabIndex = 1;
            this.lblCartonName.Values.Text = "纸箱名称";
            // 
            // txtCartonName
            // 
            this.txtCartonName.Location = new System.Drawing.Point(173, 21);
            this.txtCartonName.Name = "txtCartonName";
            this.txtCartonName.Size = new System.Drawing.Size(100, 23);
            this.txtCartonName.TabIndex = 1;
            // 
            // lblColor
            // 
            this.lblColor.Location = new System.Drawing.Point(100, 50);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(36, 20);
            this.lblColor.TabIndex = 2;
            this.lblColor.Values.Text = "颜色";
            // 
            // txtColor
            // 
            this.txtColor.Location = new System.Drawing.Point(173, 46);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(100, 23);
            this.txtColor.TabIndex = 2;
            // 
            // lblMaterial
            // 
            this.lblMaterial.Location = new System.Drawing.Point(100, 75);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(36, 20);
            this.lblMaterial.TabIndex = 3;
            this.lblMaterial.Values.Text = "材质";
            // 
            // txtMaterial
            // 
            this.txtMaterial.Location = new System.Drawing.Point(173, 71);
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Size = new System.Drawing.Size(100, 23);
            this.txtMaterial.TabIndex = 3;
            // 
            // lblEmptyBoxWeight
            // 
            this.lblEmptyBoxWeight.Location = new System.Drawing.Point(100, 100);
            this.lblEmptyBoxWeight.Name = "lblEmptyBoxWeight";
            this.lblEmptyBoxWeight.Size = new System.Drawing.Size(70, 20);
            this.lblEmptyBoxWeight.TabIndex = 4;
            this.lblEmptyBoxWeight.Values.Text = "空箱重(kg)";
            // 
            // txtEmptyBoxWeight
            // 
            this.txtEmptyBoxWeight.Location = new System.Drawing.Point(173, 96);
            this.txtEmptyBoxWeight.Name = "txtEmptyBoxWeight";
            this.txtEmptyBoxWeight.Size = new System.Drawing.Size(100, 23);
            this.txtEmptyBoxWeight.TabIndex = 4;
            // 
            // lblMaxLoad
            // 
            this.lblMaxLoad.Location = new System.Drawing.Point(100, 125);
            this.lblMaxLoad.Name = "lblMaxLoad";
            this.lblMaxLoad.Size = new System.Drawing.Size(83, 20);
            this.lblMaxLoad.TabIndex = 5;
            this.lblMaxLoad.Values.Text = "最大承重(kg)";
            // 
            // txtMaxLoad
            // 
            this.txtMaxLoad.Location = new System.Drawing.Point(173, 121);
            this.txtMaxLoad.Name = "txtMaxLoad";
            this.txtMaxLoad.Size = new System.Drawing.Size(100, 23);
            this.txtMaxLoad.TabIndex = 5;
            // 
            // lblThickness
            // 
            this.lblThickness.Location = new System.Drawing.Point(100, 150);
            this.lblThickness.Name = "lblThickness";
            this.lblThickness.Size = new System.Drawing.Size(86, 20);
            this.lblThickness.TabIndex = 6;
            this.lblThickness.Values.Text = "纸板厚度(cm)";
            // 
            // txtThickness
            // 
            this.txtThickness.Location = new System.Drawing.Point(173, 146);
            this.txtThickness.Name = "txtThickness";
            this.txtThickness.Size = new System.Drawing.Size(100, 23);
            this.txtThickness.TabIndex = 6;
            // 
            // lblLength
            // 
            this.lblLength.Location = new System.Drawing.Point(100, 175);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(60, 20);
            this.lblLength.TabIndex = 7;
            this.lblLength.Values.Text = "长度(cm)";
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(173, 171);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(100, 23);
            this.txtLength.TabIndex = 7;
            // 
            // lblWidth
            // 
            this.lblWidth.Location = new System.Drawing.Point(100, 200);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(60, 20);
            this.lblWidth.TabIndex = 8;
            this.lblWidth.Values.Text = "宽度(cm)";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(173, 196);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(100, 23);
            this.txtWidth.TabIndex = 8;
            // 
            // lblHeight
            // 
            this.lblHeight.Location = new System.Drawing.Point(100, 225);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(60, 20);
            this.lblHeight.TabIndex = 9;
            this.lblHeight.Values.Text = "高度(cm)";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(173, 221);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(100, 23);
            this.txtHeight.TabIndex = 9;
            // 
            // lblVolume
            // 
            this.lblVolume.Location = new System.Drawing.Point(100, 250);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(83, 20);
            this.lblVolume.TabIndex = 10;
            this.lblVolume.Values.Text = "体积Vol(cm³)";
            // 
            // txtVolume
            // 
            this.txtVolume.Location = new System.Drawing.Point(173, 246);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(100, 23);
            this.txtVolume.TabIndex = 10;
            // 
            // lblFluteType
            // 
            this.lblFluteType.Location = new System.Drawing.Point(100, 275);
            this.lblFluteType.Name = "lblFluteType";
            this.lblFluteType.Size = new System.Drawing.Size(62, 20);
            this.lblFluteType.TabIndex = 11;
            this.lblFluteType.Values.Text = "瓦楞类型";
            // 
            // txtFluteType
            // 
            this.txtFluteType.Location = new System.Drawing.Point(173, 271);
            this.txtFluteType.Name = "txtFluteType";
            this.txtFluteType.Size = new System.Drawing.Size(100, 23);
            this.txtFluteType.TabIndex = 11;
            // 
            // lblPrintType
            // 
            this.lblPrintType.Location = new System.Drawing.Point(100, 300);
            this.lblPrintType.Name = "lblPrintType";
            this.lblPrintType.Size = new System.Drawing.Size(62, 20);
            this.lblPrintType.TabIndex = 12;
            this.lblPrintType.Values.Text = "印刷类型";
            // 
            // txtPrintType
            // 
            this.txtPrintType.Location = new System.Drawing.Point(173, 296);
            this.txtPrintType.Name = "txtPrintType";
            this.txtPrintType.Size = new System.Drawing.Size(100, 23);
            this.txtPrintType.TabIndex = 12;
            // 
            // lblCustomPrint
            // 
            this.lblCustomPrint.Location = new System.Drawing.Point(100, 325);
            this.lblCustomPrint.Name = "lblCustomPrint";
            this.lblCustomPrint.Size = new System.Drawing.Size(62, 20);
            this.lblCustomPrint.TabIndex = 13;
            this.lblCustomPrint.Values.Text = "定制印刷";
            // 
            // txtCustomPrint
            // 
            this.txtCustomPrint.Location = new System.Drawing.Point(173, 321);
            this.txtCustomPrint.Name = "txtCustomPrint";
            this.txtCustomPrint.Size = new System.Drawing.Size(100, 23);
            this.txtCustomPrint.TabIndex = 13;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(100, 350);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 14;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Location = new System.Drawing.Point(173, 346);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 14;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(100, 375);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(36, 20);
            this.lblDescription.TabIndex = 15;
            this.lblDescription.Values.Text = "备注";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(173, 371);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(100, 21);
            this.txtDescription.TabIndex = 15;
            // 
            // lblisdeleted
            // 
            this.lblisdeleted.Location = new System.Drawing.Point(100, 400);
            this.lblisdeleted.Name = "lblisdeleted";
            this.lblisdeleted.Size = new System.Drawing.Size(62, 20);
            this.lblisdeleted.TabIndex = 16;
            this.lblisdeleted.Values.Text = "逻辑删除";
            // 
            // chkisdeleted
            // 
            this.chkisdeleted.Location = new System.Drawing.Point(173, 396);
            this.chkisdeleted.Name = "chkisdeleted";
            this.chkisdeleted.Size = new System.Drawing.Size(19, 13);
            this.chkisdeleted.TabIndex = 16;
            this.chkisdeleted.Values.Text = "";
            // 
            // lblCreated_at
            // 
            this.lblCreated_at.Location = new System.Drawing.Point(100, 425);
            this.lblCreated_at.Name = "lblCreated_at";
            this.lblCreated_at.Size = new System.Drawing.Size(62, 20);
            this.lblCreated_at.TabIndex = 17;
            this.lblCreated_at.Values.Text = "创建时间";
            // 
            // dtpCreated_at
            // 
            this.dtpCreated_at.Location = new System.Drawing.Point(173, 421);
            this.dtpCreated_at.Name = "dtpCreated_at";
            this.dtpCreated_at.ShowCheckBox = true;
            this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
            this.dtpCreated_at.TabIndex = 17;
            // 
            // lblCreated_by
            // 
            this.lblCreated_by.Location = new System.Drawing.Point(100, 450);
            this.lblCreated_by.Name = "lblCreated_by";
            this.lblCreated_by.Size = new System.Drawing.Size(49, 20);
            this.lblCreated_by.TabIndex = 18;
            this.lblCreated_by.Values.Text = "创建人";
            // 
            // txtCreated_by
            // 
            this.txtCreated_by.Location = new System.Drawing.Point(173, 446);
            this.txtCreated_by.Name = "txtCreated_by";
            this.txtCreated_by.Size = new System.Drawing.Size(100, 23);
            this.txtCreated_by.TabIndex = 18;
            // 
            // lblModified_at
            // 
            this.lblModified_at.Location = new System.Drawing.Point(100, 475);
            this.lblModified_at.Name = "lblModified_at";
            this.lblModified_at.Size = new System.Drawing.Size(62, 20);
            this.lblModified_at.TabIndex = 19;
            this.lblModified_at.Values.Text = "修改时间";
            // 
            // dtpModified_at
            // 
            this.dtpModified_at.Location = new System.Drawing.Point(173, 471);
            this.dtpModified_at.Name = "dtpModified_at";
            this.dtpModified_at.ShowCheckBox = true;
            this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
            this.dtpModified_at.TabIndex = 19;
            // 
            // lblModified_by
            // 
            this.lblModified_by.Location = new System.Drawing.Point(100, 500);
            this.lblModified_by.Name = "lblModified_by";
            this.lblModified_by.Size = new System.Drawing.Size(49, 20);
            this.lblModified_by.TabIndex = 20;
            this.lblModified_by.Values.Text = "修改人";
            // 
            // txtModified_by
            // 
            this.txtModified_by.Location = new System.Drawing.Point(173, 496);
            this.txtModified_by.Name = "txtModified_by";
            this.txtModified_by.Size = new System.Drawing.Size(100, 23);
            this.txtModified_by.TabIndex = 20;
            // 
            // tb_CartoonBoxEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCartonName);
            this.Controls.Add(this.txtCartonName);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.lblMaterial);
            this.Controls.Add(this.txtMaterial);
            this.Controls.Add(this.lblEmptyBoxWeight);
            this.Controls.Add(this.txtEmptyBoxWeight);
            this.Controls.Add(this.lblMaxLoad);
            this.Controls.Add(this.txtMaxLoad);
            this.Controls.Add(this.lblThickness);
            this.Controls.Add(this.txtThickness);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.txtLength);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.txtVolume);
            this.Controls.Add(this.lblFluteType);
            this.Controls.Add(this.txtFluteType);
            this.Controls.Add(this.lblPrintType);
            this.Controls.Add(this.txtPrintType);
            this.Controls.Add(this.lblCustomPrint);
            this.Controls.Add(this.txtCustomPrint);
            this.Controls.Add(this.lblIs_enabled);
            this.Controls.Add(this.chkIs_enabled);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblisdeleted);
            this.Controls.Add(this.chkisdeleted);
            this.Controls.Add(this.lblCreated_at);
            this.Controls.Add(this.dtpCreated_at);
            this.Controls.Add(this.lblCreated_by);
            this.Controls.Add(this.txtCreated_by);
            this.Controls.Add(this.lblModified_at);
            this.Controls.Add(this.dtpModified_at);
            this.Controls.Add(this.lblModified_by);
            this.Controls.Add(this.txtModified_by);
            this.Name = "tb_CartoonBoxEdit";
            this.Size = new System.Drawing.Size(911, 596);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCartonName;
private Krypton.Toolkit.KryptonTextBox txtCartonName;

    
        
              private Krypton.Toolkit.KryptonLabel lblColor;
private Krypton.Toolkit.KryptonTextBox txtColor;

    
        
              private Krypton.Toolkit.KryptonLabel lblMaterial;
private Krypton.Toolkit.KryptonTextBox txtMaterial;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmptyBoxWeight;
private Krypton.Toolkit.KryptonTextBox txtEmptyBoxWeight;

    
        
              private Krypton.Toolkit.KryptonLabel lblMaxLoad;
private Krypton.Toolkit.KryptonTextBox txtMaxLoad;

    
        
              private Krypton.Toolkit.KryptonLabel lblThickness;
private Krypton.Toolkit.KryptonTextBox txtThickness;

    
        
              private Krypton.Toolkit.KryptonLabel lblLength;
private Krypton.Toolkit.KryptonTextBox txtLength;

    
        
              private Krypton.Toolkit.KryptonLabel lblWidth;
private Krypton.Toolkit.KryptonTextBox txtWidth;

    
        
              private Krypton.Toolkit.KryptonLabel lblHeight;
private Krypton.Toolkit.KryptonTextBox txtHeight;

    
        
              private Krypton.Toolkit.KryptonLabel lblVolume;
private Krypton.Toolkit.KryptonTextBox txtVolume;

    
        
              private Krypton.Toolkit.KryptonLabel lblFluteType;
private Krypton.Toolkit.KryptonTextBox txtFluteType;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintType;
private Krypton.Toolkit.KryptonTextBox txtPrintType;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomPrint;
private Krypton.Toolkit.KryptonTextBox txtCustomPrint;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

