
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:02
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
    /// 报表打印配置表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_PrintConfig")]
    public partial class tb_PrintConfigQueryDto:BaseEntityDto
    {
        public tb_PrintConfigQueryDto()
        {

        }

    
     

        private string _Config_Name;
        /// <summary>
        /// 配置名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Config_Name",ColDesc = "配置名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Config_Name",Length=100,IsNullable = false,ColumnDescription = "配置名称" )]
        public string Config_Name 
        { 
            get{return _Config_Name;}
            set{SetProperty(ref _Config_Name, value);}
        }
     

        private int _BizType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "业务类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "BizType",IsNullable = false,ColumnDescription = "业务类型" )]
        public int BizType 
        { 
            get{return _BizType;}
            set{SetProperty(ref _BizType, value);}
        }
     

        private string _BizName;
        /// <summary>
        /// 业务名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BizName",ColDesc = "业务名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "BizName",Length=30,IsNullable = false,ColumnDescription = "业务名称" )]
        public string BizName 
        { 
            get{return _BizName;}
            set{SetProperty(ref _BizName, value);}
        }
     

        private string _PrinterName;
        /// <summary>
        /// 打印机名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PrinterName",ColDesc = "打印机名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PrinterName",Length=200,IsNullable = true,ColumnDescription = "打印机名称" )]
        public string PrinterName 
        { 
            get{return _PrinterName;}
            set{SetProperty(ref _PrinterName, value);}
        }
     

        private bool? _PrinterSelected= false;
        /// <summary>
        /// 设置了默认打印机
        /// </summary>
        [AdvQueryAttribute(ColName = "PrinterSelected",ColDesc = "设置了默认打印机")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "PrinterSelected",IsNullable = true,ColumnDescription = "设置了默认打印机" )]
        public bool? PrinterSelected 
        { 
            get{return _PrinterSelected;}
            set{SetProperty(ref _PrinterSelected, value);}
        }
     

        private bool? _Landscape= false;
        /// <summary>
        /// 设置横向打印
        /// </summary>
        [AdvQueryAttribute(ColName = "Landscape",ColDesc = "设置横向打印")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Landscape",IsNullable = true,ColumnDescription = "设置横向打印" )]
        public bool? Landscape 
        { 
            get{return _Landscape;}
            set{SetProperty(ref _Landscape, value);}
        }


       
    }
}



