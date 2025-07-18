
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/18/2025 10:33:40
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
    /// 蓄水登记表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_EOP_WaterStorage")]
    public partial class tb_EOP_WaterStorageQueryDto:BaseEntityDto
    {
        public tb_EOP_WaterStorageQueryDto()
        {

        }

    
     

        private string _WSRNo;
        /// <summary>
        /// 蓄水编号
        /// </summary>
        [AdvQueryAttribute(ColName = "WSRNo",ColDesc = "蓄水编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "WSRNo",Length=50,IsNullable = false,ColumnDescription = "蓄水编号" )]
        public string WSRNo 
        { 
            get{return _WSRNo;}
            set{SetProperty(ref _WSRNo, value);}
        }
     

        private string _PlatformOrderNo;
        /// <summary>
        /// 平台单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PlatformOrderNo",ColDesc = "平台单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PlatformOrderNo",Length=100,IsNullable = false,ColumnDescription = "平台单号" )]
        public string PlatformOrderNo 
        { 
            get{return _PlatformOrderNo;}
            set{SetProperty(ref _PlatformOrderNo, value);}
        }
     

        private int _PlatformType;
        /// <summary>
        /// 平台类型
        /// </summary>
        [AdvQueryAttribute(ColName = "PlatformType",ColDesc = "平台类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "PlatformType",IsNullable = false,ColumnDescription = "平台类型" )]
        public int PlatformType 
        { 
            get{return _PlatformType;}
            set{SetProperty(ref _PlatformType, value);}
        }
     

        private long _Employee_ID;
        /// <summary>
        /// 业务员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "业务员" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProjectGroup_ID",IsNullable = true,ColumnDescription = "项目组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}
            set{SetProperty(ref _ProjectGroup_ID, value);}
        }
     

        private decimal _TotalAmount= ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount",IsNullable = false,ColumnDescription = "总金额" )]
        public decimal TotalAmount 
        { 
            get{return _TotalAmount;}
            set{SetProperty(ref _TotalAmount, value);}
        }
     

        private decimal _PlatformFeeAmount= ((0));
        /// <summary>
        /// 平台费用
        /// </summary>
        [AdvQueryAttribute(ColName = "PlatformFeeAmount",ColDesc = "平台费用")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "PlatformFeeAmount",IsNullable = false,ColumnDescription = "平台费用" )]
        public decimal PlatformFeeAmount 
        { 
            get{return _PlatformFeeAmount;}
            set{SetProperty(ref _PlatformFeeAmount, value);}
        }
     

        private DateTime _OrderDate;
        /// <summary>
        /// 订单日期
        /// </summary>
        [AdvQueryAttribute(ColName = "OrderDate",ColDesc = "订单日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "OrderDate",IsNullable = false,ColumnDescription = "订单日期" )]
        public DateTime OrderDate 
        { 
            get{return _OrderDate;}
            set{SetProperty(ref _OrderDate, value);}
        }
     

        private string _ShippingAddress;
        /// <summary>
        /// 收货地址
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingAddress",ColDesc = "收货地址")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ShippingAddress",Length=500,IsNullable = true,ColumnDescription = "收货地址" )]
        public string ShippingAddress 
        { 
            get{return _ShippingAddress;}
            set{SetProperty(ref _ShippingAddress, value);}
        }
     

        private string _ShippingWay;
        /// <summary>
        /// 发货方式
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingWay",ColDesc = "发货方式")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ShippingWay",Length=50,IsNullable = true,ColumnDescription = "发货方式" )]
        public string ShippingWay 
        { 
            get{return _ShippingWay;}
            set{SetProperty(ref _ShippingWay, value);}
        }
     

        private string _TrackNo;
        /// <summary>
        /// 物流单号
        /// </summary>
        [AdvQueryAttribute(ColName = "TrackNo",ColDesc = "物流单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "TrackNo",Length=50,IsNullable = true,ColumnDescription = "物流单号" )]
        public string TrackNo 
        { 
            get{return _TrackNo;}
            set{SetProperty(ref _TrackNo, value);}
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
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=1500,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
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


       
    }
}



