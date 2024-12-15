
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/14/2024 16:49:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 包装规格表
    /// </summary>
    partial class tb_PackingQuery
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
     
     this.lblPackagingName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPackagingName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtPackagingName.Multiline = true;

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBundleID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBundleID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdBaseID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdBaseID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblBoxMaterial = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBoxMaterial = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLength = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLength = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWidth = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWidth = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblHeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtHeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblVolume = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVolume = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNetWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNetWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGrossWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGrossWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

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
                 //#####255PackagingName###String
this.lblPackagingName.AutoSize = true;
this.lblPackagingName.Location = new System.Drawing.Point(100,25);
this.lblPackagingName.Name = "lblPackagingName";
this.lblPackagingName.Size = new System.Drawing.Size(41, 12);
this.lblPackagingName.TabIndex = 1;
this.lblPackagingName.Text = "包装名称";
this.txtPackagingName.Location = new System.Drawing.Point(173,21);
this.txtPackagingName.Name = "txtPackagingName";
this.txtPackagingName.Size = new System.Drawing.Size(100, 21);
this.txtPackagingName.TabIndex = 1;
this.Controls.Add(this.lblPackagingName);
this.Controls.Add(this.txtPackagingName);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "产品详情";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####BundleID###Int64
//属性测试75BundleID
this.lblBundleID.AutoSize = true;
this.lblBundleID.Location = new System.Drawing.Point(100,75);
this.lblBundleID.Name = "lblBundleID";
this.lblBundleID.Size = new System.Drawing.Size(41, 12);
this.lblBundleID.TabIndex = 3;
this.lblBundleID.Text = "套装组合";
//111======75
this.cmbBundleID.Location = new System.Drawing.Point(173,71);
this.cmbBundleID.Name ="cmbBundleID";
this.cmbBundleID.Size = new System.Drawing.Size(100, 21);
this.cmbBundleID.TabIndex = 3;
this.Controls.Add(this.lblBundleID);
this.Controls.Add(this.cmbBundleID);

           //#####ProdBaseID###Int64
//属性测试100ProdBaseID
//属性测试100ProdBaseID
//属性测试100ProdBaseID
this.lblProdBaseID.AutoSize = true;
this.lblProdBaseID.Location = new System.Drawing.Point(100,100);
this.lblProdBaseID.Name = "lblProdBaseID";
this.lblProdBaseID.Size = new System.Drawing.Size(41, 12);
this.lblProdBaseID.TabIndex = 4;
this.lblProdBaseID.Text = "产品";
//111======100
this.cmbProdBaseID.Location = new System.Drawing.Point(173,96);
this.cmbProdBaseID.Name ="cmbProdBaseID";
this.cmbProdBaseID.Size = new System.Drawing.Size(100, 21);
this.cmbProdBaseID.TabIndex = 4;
this.Controls.Add(this.lblProdBaseID);
this.Controls.Add(this.cmbProdBaseID);

           //#####Unit_ID###Int64
//属性测试125Unit_ID
//属性测试125Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,125);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 5;
this.lblUnit_ID.Text = "包装单位";
//111======125
this.cmbUnit_ID.Location = new System.Drawing.Point(173,121);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 5;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,150);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 6;
this.lblSKU.Text = "SKU码";
this.txtSKU.Location = new System.Drawing.Point(173,146);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 6;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,175);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 7;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,171);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 7;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####2147483647PackImage###Binary

           //#####200BoxMaterial###String
this.lblBoxMaterial.AutoSize = true;
this.lblBoxMaterial.Location = new System.Drawing.Point(100,225);
this.lblBoxMaterial.Name = "lblBoxMaterial";
this.lblBoxMaterial.Size = new System.Drawing.Size(41, 12);
this.lblBoxMaterial.TabIndex = 9;
this.lblBoxMaterial.Text = "盒子材质";
this.txtBoxMaterial.Location = new System.Drawing.Point(173,221);
this.txtBoxMaterial.Name = "txtBoxMaterial";
this.txtBoxMaterial.Size = new System.Drawing.Size(100, 21);
this.txtBoxMaterial.TabIndex = 9;
this.Controls.Add(this.lblBoxMaterial);
this.Controls.Add(this.txtBoxMaterial);

           //#####Length###Decimal
this.lblLength.AutoSize = true;
this.lblLength.Location = new System.Drawing.Point(100,250);
this.lblLength.Name = "lblLength";
this.lblLength.Size = new System.Drawing.Size(41, 12);
this.lblLength.TabIndex = 10;
this.lblLength.Text = "长度(cm)";
//111======250
this.txtLength.Location = new System.Drawing.Point(173,246);
this.txtLength.Name ="txtLength";
this.txtLength.Size = new System.Drawing.Size(100, 21);
this.txtLength.TabIndex = 10;
this.Controls.Add(this.lblLength);
this.Controls.Add(this.txtLength);

           //#####Width###Decimal
this.lblWidth.AutoSize = true;
this.lblWidth.Location = new System.Drawing.Point(100,275);
this.lblWidth.Name = "lblWidth";
this.lblWidth.Size = new System.Drawing.Size(41, 12);
this.lblWidth.TabIndex = 11;
this.lblWidth.Text = "宽度(cm)";
//111======275
this.txtWidth.Location = new System.Drawing.Point(173,271);
this.txtWidth.Name ="txtWidth";
this.txtWidth.Size = new System.Drawing.Size(100, 21);
this.txtWidth.TabIndex = 11;
this.Controls.Add(this.lblWidth);
this.Controls.Add(this.txtWidth);

           //#####Height###Decimal
this.lblHeight.AutoSize = true;
this.lblHeight.Location = new System.Drawing.Point(100,300);
this.lblHeight.Name = "lblHeight";
this.lblHeight.Size = new System.Drawing.Size(41, 12);
this.lblHeight.TabIndex = 12;
this.lblHeight.Text = "高度(cm)";
//111======300
this.txtHeight.Location = new System.Drawing.Point(173,296);
this.txtHeight.Name ="txtHeight";
this.txtHeight.Size = new System.Drawing.Size(100, 21);
this.txtHeight.TabIndex = 12;
this.Controls.Add(this.lblHeight);
this.Controls.Add(this.txtHeight);

           //#####Volume###Decimal
this.lblVolume.AutoSize = true;
this.lblVolume.Location = new System.Drawing.Point(100,325);
this.lblVolume.Name = "lblVolume";
this.lblVolume.Size = new System.Drawing.Size(41, 12);
this.lblVolume.TabIndex = 13;
this.lblVolume.Text = "体积Vol(cm³)";
//111======325
this.txtVolume.Location = new System.Drawing.Point(173,321);
this.txtVolume.Name ="txtVolume";
this.txtVolume.Size = new System.Drawing.Size(100, 21);
this.txtVolume.TabIndex = 13;
this.Controls.Add(this.lblVolume);
this.Controls.Add(this.txtVolume);

           //#####NetWeight###Decimal
this.lblNetWeight.AutoSize = true;
this.lblNetWeight.Location = new System.Drawing.Point(100,350);
this.lblNetWeight.Name = "lblNetWeight";
this.lblNetWeight.Size = new System.Drawing.Size(41, 12);
this.lblNetWeight.TabIndex = 14;
this.lblNetWeight.Text = "净重N.Wt.(g)";
//111======350
this.txtNetWeight.Location = new System.Drawing.Point(173,346);
this.txtNetWeight.Name ="txtNetWeight";
this.txtNetWeight.Size = new System.Drawing.Size(100, 21);
this.txtNetWeight.TabIndex = 14;
this.Controls.Add(this.lblNetWeight);
this.Controls.Add(this.txtNetWeight);

           //#####GrossWeight###Decimal
this.lblGrossWeight.AutoSize = true;
this.lblGrossWeight.Location = new System.Drawing.Point(100,375);
this.lblGrossWeight.Name = "lblGrossWeight";
this.lblGrossWeight.Size = new System.Drawing.Size(41, 12);
this.lblGrossWeight.TabIndex = 15;
this.lblGrossWeight.Text = "毛重G.Wt.(g)";
//111======375
this.txtGrossWeight.Location = new System.Drawing.Point(173,371);
this.txtGrossWeight.Name ="txtGrossWeight";
this.txtGrossWeight.Size = new System.Drawing.Size(100, 21);
this.txtGrossWeight.TabIndex = 15;
this.Controls.Add(this.lblGrossWeight);
this.Controls.Add(this.txtGrossWeight);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,400);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 16;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,396);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 16;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,425);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 17;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,421);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 17;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,450);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 18;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,446);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 18;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,475);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 19;
this.lblCreated_at.Text = "创建时间";
//111======475
this.dtpCreated_at.Location = new System.Drawing.Point(173,471);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 19;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试500Created_by
//属性测试500Created_by
//属性测试500Created_by
//属性测试500Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,525);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 21;
this.lblModified_at.Text = "修改时间";
//111======525
this.dtpModified_at.Location = new System.Drawing.Point(173,521);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 21;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试550Modified_by
//属性测试550Modified_by
//属性测试550Modified_by
//属性测试550Modified_by

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPackagingName );
this.Controls.Add(this.txtPackagingName );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblBundleID );
this.Controls.Add(this.cmbBundleID );

                this.Controls.Add(this.lblProdBaseID );
this.Controls.Add(this.cmbProdBaseID );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblBoxMaterial );
this.Controls.Add(this.txtBoxMaterial );

                this.Controls.Add(this.lblLength );
this.Controls.Add(this.txtLength );

                this.Controls.Add(this.lblWidth );
this.Controls.Add(this.txtWidth );

                this.Controls.Add(this.lblHeight );
this.Controls.Add(this.txtHeight );

                this.Controls.Add(this.lblVolume );
this.Controls.Add(this.txtVolume );

                this.Controls.Add(this.lblNetWeight );
this.Controls.Add(this.txtNetWeight );

                this.Controls.Add(this.lblGrossWeight );
this.Controls.Add(this.txtGrossWeight );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_PackingQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPackagingName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPackagingName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBundleID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBundleID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdBaseID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdBaseID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBoxMaterial;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBoxMaterial;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLength;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLength;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWidth;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWidth;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblHeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtHeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVolume;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVolume;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNetWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNetWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGrossWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGrossWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


