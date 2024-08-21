
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:06
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
    /// 交易方式设定，后面扩展有关账期 账龄分析的字段,暂时保存一个主子关系方便后面扩展
    /// </summary>
    [Serializable()]
    [SugarTable("tb_PaymentMethodDetail")]
    public partial class tb_PaymentMethodDetailQueryDto:BaseEntityDto
    {
        public tb_PaymentMethodDetailQueryDto()
        {

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



