
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:05
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 付款方式 交易方式，后面扩展有关账期 账龄分析的字段
    /// </summary>
    [Serializable()]
    [SugarTable("tb_PaymentMethod")]
    public partial class tb_PaymentMethodQueryDto:BaseEntityDto
    {
        public tb_PaymentMethodQueryDto()
        {

        }

    
     

        private string _Paytype_Name;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_Name",ColDesc = "付款方式")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Paytype_Name",Length=10,IsNullable = true,ColumnDescription = "付款方式" )]
        public string Paytype_Name 
        { 
            get{return _Paytype_Name;}
            set{SetProperty(ref _Paytype_Name, value);}
        }
     

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Desc",Length=50,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc 
        { 
            get{return _Desc;}
            set{SetProperty(ref _Desc, value);}
        }
     

        private bool? _Cash;
        /// <summary>
        /// 现金
        /// </summary>
        [AdvQueryAttribute(ColName = "Cash",ColDesc = "现金")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Cash",IsNullable = true,ColumnDescription = "现金" )]
        public bool? Cash 
        { 
            get{return _Cash;}
            set{SetProperty(ref _Cash, value);}
        }


       
    }
}



