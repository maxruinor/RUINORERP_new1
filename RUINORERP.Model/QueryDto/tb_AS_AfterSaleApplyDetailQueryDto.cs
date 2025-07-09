
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/09/2025 13:49:58
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
    /// 售后申请单明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_AS_AfterSaleApplyDetail")]
    public partial class tb_AS_AfterSaleApplyDetailQueryDto:BaseEntityDto
    {
        public tb_AS_AfterSaleApplyDetailQueryDto()
        {

        }

    
     

        private long _ASApplyID;
        /// <summary>
        /// 售后申请单
        /// </summary>
        [AdvQueryAttribute(ColName = "ASApplyID",ColDesc = "售后申请单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ASApplyID",IsNullable = false,ColumnDescription = "售后申请单" )]
        [FKRelationAttribute("tb_AS_AfterSaleApply","ASApplyID")]
        public long ASApplyID 
        { 
            get{return _ASApplyID;}
            set{SetProperty(ref _ASApplyID, value);}
        }
     

        private long _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "产品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
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
     

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
        }
     

        private string _FaultDescription;
        /// <summary>
        /// 问题描述
        /// </summary>
        [AdvQueryAttribute(ColName = "FaultDescription",ColDesc = "问题描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "FaultDescription",Length=500,IsNullable = true,ColumnDescription = "问题描述" )]
        public string FaultDescription 
        { 
            get{return _FaultDescription;}
            set{SetProperty(ref _FaultDescription, value);}
        }
     

        private int _InitialQuantity= ((0));
        /// <summary>
        /// 客户申报数量
        /// </summary>
        [AdvQueryAttribute(ColName = "InitialQuantity",ColDesc = "客户申报数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "InitialQuantity",IsNullable = false,ColumnDescription = "客户申报数量" )]
        public int InitialQuantity 
        { 
            get{return _InitialQuantity;}
            set{SetProperty(ref _InitialQuantity, value);}
        }
     

        private int _ConfirmedQuantity= ((0));
        /// <summary>
        /// 复核数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ConfirmedQuantity",ColDesc = "复核数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ConfirmedQuantity",IsNullable = false,ColumnDescription = "复核数量" )]
        public int ConfirmedQuantity 
        { 
            get{return _ConfirmedQuantity;}
            set{SetProperty(ref _ConfirmedQuantity, value);}
        }
     

        private string _CustomerPartNo;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CustomerPartNo",Length=100,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomerPartNo 
        { 
            get{return _CustomerPartNo;}
            set{SetProperty(ref _CustomerPartNo, value);}
        }
     

        private int _DeliveredQty= ((0));
        /// <summary>
        /// 交付数量
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveredQty",ColDesc = "交付数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DeliveredQty",IsNullable = false,ColumnDescription = "交付数量" )]
        public int DeliveredQty 
        { 
            get{return _DeliveredQty;}
            set{SetProperty(ref _DeliveredQty, value);}
        }
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
        }


       
    }
}



