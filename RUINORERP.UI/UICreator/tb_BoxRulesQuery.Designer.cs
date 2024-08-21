
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 17:26:40
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 箱规表
    /// </summary>
    partial class tb_BoxRulesQuery
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
     
     this.lblPack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCartonID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCartonID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBoxRuleName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBoxRuleName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtBoxRuleName.Multiline = true;


this.lblPackingMethod = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPackingMethod = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLength = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLength = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWidth = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWidth = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblHeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtHeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblVolume = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVolume = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGrossWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGrossWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNetWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNetWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Pack_ID###Int64
//属性测试25Pack_ID
//属性测试25Pack_ID
this.lblPack_ID.AutoSize = true;
this.lblPack_ID.Location = new System.Drawing.Point(100,25);
this.lblPack_ID.Name = "lblPack_ID";
this.lblPack_ID.Size = new System.Drawing.Size(41, 12);
this.lblPack_ID.TabIndex = 1;
this.lblPack_ID.Text = "包装信息";
//111======25
this.cmbPack_ID.Location = new System.Drawing.Point(173,21);
this.cmbPack_ID.Name ="cmbPack_ID";
this.cmbPack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPack_ID.TabIndex = 1;
this.Controls.Add(this.lblPack_ID);
this.Controls.Add(this.cmbPack_ID);

           //#####CartonID###Int64
//属性测试50CartonID
this.lblCartonID.AutoSize = true;
this.lblCartonID.Location = new System.Drawing.Point(100,50);
this.lblCartonID.Name = "lblCartonID";
this.lblCartonID.Size = new System.Drawing.Size(41, 12);
this.lblCartonID.TabIndex = 2;
this.lblCartonID.Text = "纸箱规格";
//111======50
this.cmbCartonID.Location = new System.Drawing.Point(173,46);
this.cmbCartonID.Name ="cmbCartonID";
this.cmbCartonID.Size = new System.Drawing.Size(100, 21);
this.cmbCartonID.TabIndex = 2;
this.Controls.Add(this.lblCartonID);
this.Controls.Add(this.cmbCartonID);

           //#####255BoxRuleName###String
this.lblBoxRuleName.AutoSize = true;
this.lblBoxRuleName.Location = new System.Drawing.Point(100,75);
this.lblBoxRuleName.Name = "lblBoxRuleName";
this.lblBoxRuleName.Size = new System.Drawing.Size(41, 12);
this.lblBoxRuleName.TabIndex = 3;
this.lblBoxRuleName.Text = "箱规名称";
this.txtBoxRuleName.Location = new System.Drawing.Point(173,71);
this.txtBoxRuleName.Name = "txtBoxRuleName";
this.txtBoxRuleName.Size = new System.Drawing.Size(100, 21);
this.txtBoxRuleName.TabIndex = 3;
this.Controls.Add(this.lblBoxRuleName);
this.Controls.Add(this.txtBoxRuleName);

           //#####QuantityPerBox###Int32
//属性测试100QuantityPerBox
//属性测试100QuantityPerBox

           //#####100PackingMethod###String
this.lblPackingMethod.AutoSize = true;
this.lblPackingMethod.Location = new System.Drawing.Point(100,125);
this.lblPackingMethod.Name = "lblPackingMethod";
this.lblPackingMethod.Size = new System.Drawing.Size(41, 12);
this.lblPackingMethod.TabIndex = 5;
this.lblPackingMethod.Text = "装箱方式";
this.txtPackingMethod.Location = new System.Drawing.Point(173,121);
this.txtPackingMethod.Name = "txtPackingMethod";
this.txtPackingMethod.Size = new System.Drawing.Size(100, 21);
this.txtPackingMethod.TabIndex = 5;
this.Controls.Add(this.lblPackingMethod);
this.Controls.Add(this.txtPackingMethod);

           //#####Length###Decimal
this.lblLength.AutoSize = true;
this.lblLength.Location = new System.Drawing.Point(100,150);
this.lblLength.Name = "lblLength";
this.lblLength.Size = new System.Drawing.Size(41, 12);
this.lblLength.TabIndex = 6;
this.lblLength.Text = "长度(cm)";
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
this.lblWidth.Text = "宽度(cm)";
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
this.lblHeight.Text = "高度(cm)";
//111======200
this.txtHeight.Location = new System.Drawing.Point(173,196);
this.txtHeight.Name ="txtHeight";
this.txtHeight.Size = new System.Drawing.Size(100, 21);
this.txtHeight.TabIndex = 8;
this.Controls.Add(this.lblHeight);
this.Controls.Add(this.txtHeight);

           //#####Volume###Decimal
this.lblVolume.AutoSize = true;
this.lblVolume.Location = new System.Drawing.Point(100,225);
this.lblVolume.Name = "lblVolume";
this.lblVolume.Size = new System.Drawing.Size(41, 12);
this.lblVolume.TabIndex = 9;
this.lblVolume.Text = "体积Vol(cm³)";
//111======225
this.txtVolume.Location = new System.Drawing.Point(173,221);
this.txtVolume.Name ="txtVolume";
this.txtVolume.Size = new System.Drawing.Size(100, 21);
this.txtVolume.TabIndex = 9;
this.Controls.Add(this.lblVolume);
this.Controls.Add(this.txtVolume);

           //#####GrossWeight###Decimal
this.lblGrossWeight.AutoSize = true;
this.lblGrossWeight.Location = new System.Drawing.Point(100,250);
this.lblGrossWeight.Name = "lblGrossWeight";
this.lblGrossWeight.Size = new System.Drawing.Size(41, 12);
this.lblGrossWeight.TabIndex = 10;
this.lblGrossWeight.Text = "毛重G.Wt.(kg)";
//111======250
this.txtGrossWeight.Location = new System.Drawing.Point(173,246);
this.txtGrossWeight.Name ="txtGrossWeight";
this.txtGrossWeight.Size = new System.Drawing.Size(100, 21);
this.txtGrossWeight.TabIndex = 10;
this.Controls.Add(this.lblGrossWeight);
this.Controls.Add(this.txtGrossWeight);

           //#####NetWeight###Decimal
this.lblNetWeight.AutoSize = true;
this.lblNetWeight.Location = new System.Drawing.Point(100,275);
this.lblNetWeight.Name = "lblNetWeight";
this.lblNetWeight.Size = new System.Drawing.Size(41, 12);
this.lblNetWeight.TabIndex = 11;
this.lblNetWeight.Text = "净重N.Wt.(kg)";
//111======275
this.txtNetWeight.Location = new System.Drawing.Point(173,271);
this.txtNetWeight.Name ="txtNetWeight";
this.txtNetWeight.Size = new System.Drawing.Size(100, 21);
this.txtNetWeight.TabIndex = 11;
this.Controls.Add(this.lblNetWeight);
this.Controls.Add(this.txtNetWeight);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,300);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 12;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,296);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 12;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,325);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 13;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,321);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 13;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,350);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 14;
this.lblCreated_at.Text = "创建时间";
//111======350
this.dtpCreated_at.Location = new System.Drawing.Point(173,346);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 14;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试375Created_by
//属性测试375Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,400);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 16;
this.lblModified_at.Text = "修改时间";
//111======400
this.dtpModified_at.Location = new System.Drawing.Point(173,396);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 16;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试425Modified_by
//属性测试425Modified_by

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPack_ID );
this.Controls.Add(this.cmbPack_ID );

                this.Controls.Add(this.lblCartonID );
this.Controls.Add(this.cmbCartonID );

                this.Controls.Add(this.lblBoxRuleName );
this.Controls.Add(this.txtBoxRuleName );

                
                this.Controls.Add(this.lblPackingMethod );
this.Controls.Add(this.txtPackingMethod );

                this.Controls.Add(this.lblLength );
this.Controls.Add(this.txtLength );

                this.Controls.Add(this.lblWidth );
this.Controls.Add(this.txtWidth );

                this.Controls.Add(this.lblHeight );
this.Controls.Add(this.txtHeight );

                this.Controls.Add(this.lblVolume );
this.Controls.Add(this.txtVolume );

                this.Controls.Add(this.lblGrossWeight );
this.Controls.Add(this.txtGrossWeight );

                this.Controls.Add(this.lblNetWeight );
this.Controls.Add(this.txtNetWeight );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_BoxRulesQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPack_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCartonID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCartonID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBoxRuleName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBoxRuleName;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPackingMethod;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPackingMethod;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLength;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLength;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWidth;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWidth;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblHeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtHeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVolume;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVolume;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGrossWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGrossWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNetWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNetWeight;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


