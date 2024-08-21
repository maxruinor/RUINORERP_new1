
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2023 00:48:52
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 产品属性关系
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProdProperty")]
    public class View_ProdPropertyQueryDto:BaseEntity, ICloneable
    {
        public View_ProdPropertyQueryDto()
        {

        }

    
        private long? _ProdBaseID;
        
        
        /// <summary>
        /// 产品
        /// </summary>
        [SugarColumn(ColumnName = "ProdBaseID",IsNullable = true,ColumnDescription = "产品")]
        [Display(Name = "产品")]
        public long? ProdBaseID 
        { 
            get{return _ProdBaseID;}            set{                SetProperty(ref _ProdBaseID, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品详情
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "产品详情")]
        [Display(Name = "产品详情")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private long? _Property_ID;
        
        
        /// <summary>
        /// Property_ID
        /// </summary>
        [SugarColumn(ColumnName = "Property_ID",IsNullable = true,ColumnDescription = "Property_ID")]
        [Display(Name = "Property_ID")]
        public long? Property_ID 
        { 
            get{return _Property_ID;}            set{                SetProperty(ref _Property_ID, value);                }
        }

        private string _PropertyName;
        
        
        /// <summary>
        /// 属性名称
        /// </summary>
        [SugarColumn(ColumnName = "PropertyName",Length=20,IsNullable = true,ColumnDescription = "属性名称")]
        [Display(Name = "属性名称")]
        public string PropertyName 
        { 
            get{return _PropertyName;}            set{                SetProperty(ref _PropertyName, value);                }
        }

        private long? _PropertyValueID;
        
        
        /// <summary>
        /// PropertyValueID
        /// </summary>
        [SugarColumn(ColumnName = "PropertyValueID",IsNullable = true,ColumnDescription = "PropertyValueID")]
        [Display(Name = "PropertyValueID")]
        public long? PropertyValueID 
        { 
            get{return _PropertyValueID;}            set{                SetProperty(ref _PropertyValueID, value);                }
        }

        private string _PropertyValueName;
        
        
        /// <summary>
        /// 属性值名称
        /// </summary>
        [SugarColumn(ColumnName = "PropertyValueName",Length=20,IsNullable = true,ColumnDescription = "属性值名称")]
        [Display(Name = "属性值名称")]
        public string PropertyValueName 
        { 
            get{return _PropertyValueName;}            set{                SetProperty(ref _PropertyValueName, value);                }
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

