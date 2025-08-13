
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:30:06
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
    /// 库存流水表
    /// </summary>
    [Serializable()]
    [Description("库存流水表")]
    [SugarTable("tb_InventoryTransaction")]
    public partial class tb_InventoryTransaction: BaseEntity, ICloneable
    {
        public tb_InventoryTransaction()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("库存流水表tb_InventoryTransaction" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _TranID;
        /// <summary>
        /// 流水ID
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "TranID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "流水ID" , IsPrimaryKey = true)]
        public long TranID
        { 
            get{return _TranID;}
            set{
            SetProperty(ref _TranID, value);
                base.PrimaryKeyID = _TranID;
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品详情" )]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "库位" )]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
                        }
        }

        private int _BizType= ((0));
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BizType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "业务类型" )]
        public int BizType
        { 
            get{return _BizType;}
            set{
            SetProperty(ref _BizType, value);
                        }
        }

        private long _ReferenceId;
        /// <summary>
        /// 业务单据
        /// </summary>
        [AdvQueryAttribute(ColName = "ReferenceId",ColDesc = "业务单据")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReferenceId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "业务单据" )]
        public long ReferenceId
        { 
            get{return _ReferenceId;}
            set{
            SetProperty(ref _ReferenceId, value);
                        }
        }

        private int _QuantityChange= ((0));
        /// <summary>
        /// 变动数量
        /// </summary>
        [AdvQueryAttribute(ColName = "QuantityChange",ColDesc = "变动数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "QuantityChange" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "变动数量" )]
        public int QuantityChange
        { 
            get{return _QuantityChange;}
            set{
            SetProperty(ref _QuantityChange, value);
                        }
        }

        private int _AfterQuantity= ((0));
        /// <summary>
        /// 变后数量
        /// </summary>
        [AdvQueryAttribute(ColName = "AfterQuantity",ColDesc = "变后数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "AfterQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "变后数量" )]
        public int AfterQuantity
        { 
            get{return _AfterQuantity;}
            set{
            SetProperty(ref _AfterQuantity, value);
                        }
        }

        private int _BatchNumber= ((0));
        /// <summary>
        /// 批号
        /// </summary>
        [AdvQueryAttribute(ColName = "BatchNumber",ColDesc = "批号")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BatchNumber" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "批号" )]
        public int BatchNumber
        { 
            get{return _BatchNumber;}
            set{
            SetProperty(ref _BatchNumber, value);
                        }
        }

        private decimal _UnitCost= ((0));
        /// <summary>
        /// 单位成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "单位成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位成本" )]
        public decimal UnitCost
        { 
            get{return _UnitCost;}
            set{
            SetProperty(ref _UnitCost, value);
                        }
        }

        private DateTime _TransactionTime;
        /// <summary>
        /// 操作时间
        /// </summary>
        [AdvQueryAttribute(ColName = "TransactionTime",ColDesc = "操作时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "TransactionTime" ,IsNullable = false,ColumnDescription = "操作时间" )]
        public DateTime TransactionTime
        { 
            get{return _TransactionTime;}
            set{
            SetProperty(ref _TransactionTime, value);
                        }
        }

        private long? _OperatorId;
        /// <summary>
        /// 操作人
        /// </summary>
        [AdvQueryAttribute(ColName = "OperatorId",ColDesc = "操作人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "OperatorId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "操作人" )]
        public long? OperatorId
        { 
            get{return _OperatorId;}
            set{
            SetProperty(ref _OperatorId, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=250,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        #endregion

        #region 扩展属性


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_InventoryTransaction loctype = (tb_InventoryTransaction)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

