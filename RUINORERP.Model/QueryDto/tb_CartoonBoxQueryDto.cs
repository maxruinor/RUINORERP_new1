
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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 卡通箱规格表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_CartoonBox")]
    public partial class tb_CartoonBoxQueryDto:BaseEntityDto
    {
        public tb_CartoonBoxQueryDto()
        {

        }

    
     

        private string _CartonName;
        /// <summary>
        /// 纸箱名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CartonName",ColDesc = "纸箱名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CartonName",Length=100,IsNullable = true,ColumnDescription = "纸箱名称" )]
        public string CartonName 
        { 
            get{return _CartonName;}
            set{SetProperty(ref _CartonName, value);}
        }
     

        private string _Color;
        /// <summary>
        /// 颜色
        /// </summary>
        [AdvQueryAttribute(ColName = "Color",ColDesc = "颜色")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Color",Length=100,IsNullable = true,ColumnDescription = "颜色" )]
        public string Color 
        { 
            get{return _Color;}
            set{SetProperty(ref _Color, value);}
        }
     

        private string _Material;
        /// <summary>
        /// 材质
        /// </summary>
        [AdvQueryAttribute(ColName = "Material",ColDesc = "材质")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Material",Length=100,IsNullable = true,ColumnDescription = "材质" )]
        public string Material 
        { 
            get{return _Material;}
            set{SetProperty(ref _Material, value);}
        }
     

        private decimal _EmptyBoxWeight= ((0));
        /// <summary>
        /// 空箱重(kg)
        /// </summary>
        [AdvQueryAttribute(ColName = "EmptyBoxWeight",ColDesc = "空箱重(kg)")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "EmptyBoxWeight",IsNullable = false,ColumnDescription = "空箱重(kg)" )]
        public decimal EmptyBoxWeight 
        { 
            get{return _EmptyBoxWeight;}
            set{SetProperty(ref _EmptyBoxWeight, value);}
        }
     

        private decimal _MaxLoad= ((1));
        /// <summary>
        /// 最大承重(kg)
        /// </summary>
        [AdvQueryAttribute(ColName = "MaxLoad",ColDesc = "最大承重(kg)")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "MaxLoad",IsNullable = false,ColumnDescription = "最大承重(kg)" )]
        public decimal MaxLoad 
        { 
            get{return _MaxLoad;}
            set{SetProperty(ref _MaxLoad, value);}
        }
     

        private decimal _Thickness= ((0.1m));
        /// <summary>
        /// 纸板厚度(cm)
        /// </summary>
        [AdvQueryAttribute(ColName = "Thickness",ColDesc = "纸板厚度(cm)")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Thickness",IsNullable = false,ColumnDescription = "纸板厚度(cm)" )]
        public decimal Thickness 
        { 
            get{return _Thickness;}
            set{SetProperty(ref _Thickness, value);}
        }
     

        private decimal _Length= ((1));
        /// <summary>
        /// 长度(cm)
        /// </summary>
        [AdvQueryAttribute(ColName = "Length",ColDesc = "长度(cm)")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Length",IsNullable = false,ColumnDescription = "长度(cm)" )]
        public decimal Length 
        { 
            get{return _Length;}
            set{SetProperty(ref _Length, value);}
        }
     

        private decimal _Width= ((1));
        /// <summary>
        /// 宽度(cm)
        /// </summary>
        [AdvQueryAttribute(ColName = "Width",ColDesc = "宽度(cm)")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Width",IsNullable = false,ColumnDescription = "宽度(cm)" )]
        public decimal Width 
        { 
            get{return _Width;}
            set{SetProperty(ref _Width, value);}
        }
     

        private decimal _Height= ((1));
        /// <summary>
        /// 高度(cm)
        /// </summary>
        [AdvQueryAttribute(ColName = "Height",ColDesc = "高度(cm)")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Height",IsNullable = false,ColumnDescription = "高度(cm)" )]
        public decimal Height 
        { 
            get{return _Height;}
            set{SetProperty(ref _Height, value);}
        }
     

        private decimal _Volume= ((1));
        /// <summary>
        /// 体积Vol(cm³)
        /// </summary>
        [AdvQueryAttribute(ColName = "Volume",ColDesc = "体积Vol(cm³)")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Volume",IsNullable = false,ColumnDescription = "体积Vol(cm³)" )]
        public decimal Volume 
        { 
            get{return _Volume;}
            set{SetProperty(ref _Volume, value);}
        }
     

        private string _FluteType;
        /// <summary>
        /// 瓦楞类型
        /// </summary>
        [AdvQueryAttribute(ColName = "FluteType",ColDesc = "瓦楞类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "FluteType",Length=100,IsNullable = true,ColumnDescription = "瓦楞类型" )]
        public string FluteType 
        { 
            get{return _FluteType;}
            set{SetProperty(ref _FluteType, value);}
        }
     

        private string _PrintType;
        /// <summary>
        /// 印刷类型
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintType",ColDesc = "印刷类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PrintType",Length=100,IsNullable = true,ColumnDescription = "印刷类型" )]
        public string PrintType 
        { 
            get{return _PrintType;}
            set{SetProperty(ref _PrintType, value);}
        }
     

        private string _CustomPrint;
        /// <summary>
        /// 定制印刷
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomPrint",ColDesc = "定制印刷")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CustomPrint",Length=100,IsNullable = true,ColumnDescription = "定制印刷" )]
        public string CustomPrint 
        { 
            get{return _CustomPrint;}
            set{SetProperty(ref _CustomPrint, value);}
        }
     

        private bool? _Is_enabled;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }
     

        private string _Description;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Description",Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Description 
        { 
            get{return _Description;}
            set{SetProperty(ref _Description, value);}
        }
     

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "isdeleted",IsNullable = false,ColumnDescription = "逻辑删除" )]
        public bool isdeleted 
        { 
            get{return _isdeleted;}
            set{SetProperty(ref _isdeleted, value);}
        }
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }


       
    }
}



