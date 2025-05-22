
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:10
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
    /// 价格记录表
    /// </summary>
    [Serializable()]
    [Description("价格记录表")]
    [SugarTable("tb_PriceRecord")]
    public partial class tb_PriceRecord: BaseEntity, ICloneable
    {
        public tb_PriceRecord()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("价格记录表tb_PriceRecord" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RecordID;
        /// <summary>
        /// 记录ID
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RecordID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "记录ID" , IsPrimaryKey = true)]
        public long RecordID
        { 
            get{return _RecordID;}
            set{
            SetProperty(ref _RecordID, value);
                base.PrimaryKeyID = _RecordID;
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private DateTime? _PurDate;
        /// <summary>
        /// 采购日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PurDate",ColDesc = "采购日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PurDate" ,IsNullable = true,ColumnDescription = "采购日期" )]
        public DateTime? PurDate
        { 
            get{return _PurDate;}
            set{
            SetProperty(ref _PurDate, value);
                        }
        }

        private DateTime? _SaleDate;
        /// <summary>
        /// 销售日期
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleDate",ColDesc = "销售日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "SaleDate" ,IsNullable = true,ColumnDescription = "销售日期" )]
        public DateTime? SaleDate
        { 
            get{return _SaleDate;}
            set{
            SetProperty(ref _SaleDate, value);
                        }
        }

        private decimal _PurPrice= ((0));
        /// <summary>
        /// 采购价
        /// </summary>
        [AdvQueryAttribute(ColName = "PurPrice",ColDesc = "采购价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "PurPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "采购价" )]
        public decimal PurPrice
        { 
            get{return _PurPrice;}
            set{
            SetProperty(ref _PurPrice, value);
                        }
        }

        private decimal _SalePrice= ((0));
        /// <summary>
        /// 销售价
        /// </summary>
        [AdvQueryAttribute(ColName = "SalePrice",ColDesc = "销售价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SalePrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "销售价" )]
        public decimal SalePrice
        { 
            get{return _SalePrice;}
            set{
            SetProperty(ref _SalePrice, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




        public override object Clone()
        {
            tb_PriceRecord loctype = (tb_PriceRecord)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

