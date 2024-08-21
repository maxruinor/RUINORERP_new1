
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
    /// 产品属性组合
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProdPropGoup")]
    public class View_ProdPropGoupQueryDto:BaseEntity, ICloneable
    {
        public View_ProdPropGoupQueryDto()
        {

        }

    
        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "产品")]
        [Display(Name = "产品")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private string _prop;
        
        
        /// <summary>
        /// 属性组合
        /// </summary>
        [SugarColumn(ColumnName = "prop",Length=200,IsNullable = true,ColumnDescription = "属性组合")]
        [Display(Name = "属性组合")]
        public string prop 
        { 
            get{return _prop;}            set{                SetProperty(ref _prop, value);                }
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





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

