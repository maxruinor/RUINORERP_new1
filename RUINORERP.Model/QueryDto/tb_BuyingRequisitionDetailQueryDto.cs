
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/05/2024 17:00:18
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
    /// 请购单明细表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_BuyingRequisitionDetail")]
    public partial class tb_BuyingRequisitionDetailQueryDto:BaseEntityDto
    {
        public tb_BuyingRequisitionDetailQueryDto()
        {

        }

    
     

        private long? _PuRequisition_ID;
        /// <summary>
        /// 请购单
        /// </summary>
        [AdvQueryAttribute(ColName = "PuRequisition_ID",ColDesc = "请购单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PuRequisition_ID",IsNullable = true,ColumnDescription = "请购单" )]
        [FKRelationAttribute("tb_BuyingRequisition","PuRequisition_ID")]
        public long? PuRequisition_ID 
        { 
            get{return _PuRequisition_ID;}
            set{SetProperty(ref _PuRequisition_ID, value);}
        }
     

        private long _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "产品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
        }
     

        private DateTime? _RequirementDate;
        /// <summary>
        /// 需求日期
        /// </summary>
        [AdvQueryAttribute(ColName = "RequirementDate",ColDesc = "需求日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "RequirementDate",IsNullable = true,ColumnDescription = "需求日期" )]
        public DateTime? RequirementDate 
        { 
            get{return _RequirementDate;}
            set{SetProperty(ref _RequirementDate, value);}
        }
     

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "property",Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property 
        { 
            get{return _property;}
            set{SetProperty(ref _property, value);}
        }
     

        private int _ActualRequiredQty;
        /// <summary>
        /// 需求数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualRequiredQty",ColDesc = "需求数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ActualRequiredQty",IsNullable = false,ColumnDescription = "需求数量" )]
        public int ActualRequiredQty 
        { 
            get{return _ActualRequiredQty;}
            set{SetProperty(ref _ActualRequiredQty, value);}
        }
     

        private int _Quantity;
        /// <summary>
        /// 请购数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "请购数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Quantity",IsNullable = false,ColumnDescription = "请购数量" )]
        public int Quantity 
        { 
            get{return _Quantity;}
            set{SetProperty(ref _Quantity, value);}
        }
     

        private int _DeliveredQuantity= ((0));
        /// <summary>
        /// 已交数量
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveredQuantity",ColDesc = "已交数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DeliveredQuantity",IsNullable = false,ColumnDescription = "已交数量" )]
        public int DeliveredQuantity 
        { 
            get{return _DeliveredQuantity;}
            set{SetProperty(ref _DeliveredQuantity, value);}
        }
     

        private string _Purpose;
        /// <summary>
        /// 用途
        /// </summary>
        [AdvQueryAttribute(ColName = "Purpose",ColDesc = "用途")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Purpose",Length=500,IsNullable = false,ColumnDescription = "用途" )]
        public string Purpose 
        { 
            get{return _Purpose;}
            set{SetProperty(ref _Purpose, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=1000,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private bool? _Purchased= false;
        /// <summary>
        /// 已采购
        /// </summary>
        [AdvQueryAttribute(ColName = "Purchased",ColDesc = "已采购")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Purchased",IsNullable = true,ColumnDescription = "已采购" )]
        public bool? Purchased 
        { 
            get{return _Purchased;}
            set{SetProperty(ref _Purchased, value);}
        }


       
    }
}



