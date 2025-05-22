
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/27/2024 15:06:03
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    [SugarTable("Proc_InventoryTracking")]
    public class Proc_InventoryTracking : BaseEntity, ICloneable
    {
        public Proc_InventoryTracking()
        {
          
            if (!PK_FK_ID_Check())
            {
                throw new Exception("Proc_InventoryTracking" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

        private string _经营历程;


        /// <summary>
        /// 经营历程
        /// </summary>

        [AdvQueryAttribute(ColName = "经营历程", ColDesc = "经营历程")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "经营历程", Length = 40, IsNullable = false, ColumnDescription = "经营历程")]
        [Display(Name = "")]
        public string 经营历程
        {
            get { return _经营历程; }
            set
            {
                SetProperty(ref _经营历程, value);
            }
        }

        private long _ProdDetailID;


        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID", ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProdDetailID", DecimalDigits = 255, Length = 8, IsNullable = false, ColumnDescription = "")]
        [Display(Name = "")]
        public long ProdDetailID
        {
            get { return _ProdDetailID; }
            set
            {
                SetProperty(ref _ProdDetailID, value);
            }
        }

        private string _ProductNo;


        /// <summary>
        /// 产品编码
        /// </summary>

        [AdvQueryAttribute(ColName = "ProductNo", ColDesc = "产品编码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ProductNo", DecimalDigits = 255, Length = 40, IsNullable = false, ColumnDescription = "产品编码")]
        [Display(Name = "")]
        public string ProductNo
        {
            get { return _ProductNo; }
            set
            {
                SetProperty(ref _ProductNo, value);
            }
        }

        private string _SKU;


        /// <summary>
        /// SKU
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU", ColDesc = "SKU")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SKU", DecimalDigits = 255, Length = 80, IsNullable = true, ColumnDescription = "SKU")]
        [Display(Name = "")]
        public string SKU
        {
            get { return _SKU; }
            set
            {
                SetProperty(ref _SKU, value);
            }
        }

        private string _CNName;


        /// <summary>
        /// 产品名称
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName", ColDesc = "产品名称")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "CNName", DecimalDigits = 255, Length = 255, IsNullable = false, ColumnDescription = "产品名称")]
        [Display(Name = "")]
        public string CNName
        {
            get { return _CNName; }
            set
            {
                SetProperty(ref _CNName, value);
            }
        }

        private string _Specifications;


        /// <summary>
        /// 规格
        /// </summary>

        [AdvQueryAttribute(ColName = "Specifications", ColDesc = "规格")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Specifications", DecimalDigits = 255, Length = 1000, IsNullable = true, ColumnDescription = "规格")]
        [Display(Name = "")]
        public string Specifications
        {
            get { return _Specifications; }
            set
            {
                SetProperty(ref _Specifications, value);
            }
        }

        private string _prop;


        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "prop", ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "prop", DecimalDigits = 255, Length = 2147483647, IsNullable = true, ColumnDescription = "属性")]
        [Display(Name = "")]
        public string prop
        {
            get { return _prop; }
            set
            {
                SetProperty(ref _prop, value);
            }
        }

        private string _业务类型;


        /// <summary>
        /// 业务类型
        /// </summary>

        [AdvQueryAttribute(ColName = "业务类型", ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "业务类型", DecimalDigits = 255, Length = 8, IsNullable = false, ColumnDescription = "业务类型")]
        [Display(Name = "")]
        public string 业务类型
        {
            get { return _业务类型; }
            set
            {
                SetProperty(ref _业务类型, value);
            }
        }

        private string _单据编号;


        /// <summary>
        /// 单据编号
        /// </summary>

        [AdvQueryAttribute(ColName = "单据编号", ColDesc = "单据编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "单据编号", DecimalDigits = 255, Length = 1, IsNullable = false, ColumnDescription = "单据编号")]
        [Display(Name = "")]
        public string 单据编号
        {
            get { return _单据编号; }
            set
            {
                SetProperty(ref _单据编号, value);
            }
        }

        private long _库位;


        /// <summary>
        /// 库位
        /// </summary>

        [AdvQueryAttribute(ColName = "库位", ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "库位", DecimalDigits = 255, Length = 8, IsNullable = false, ColumnDescription = "库位")]
        [Display(Name = "")]
        public long Location_ID
        {
            get { return _库位; }
            set
            {
                SetProperty(ref _库位, value);
            }
        }

        private int? _数量;


        /// <summary>
        /// 数量
        /// </summary>

        [AdvQueryAttribute(ColName = "数量", ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "数量", DecimalDigits = 255, Length = 4, IsNullable = true, ColumnDescription = "数量")]
        [Display(Name = "")]
        public int? 数量
        {
            get { return _数量; }
            set
            {
                SetProperty(ref _数量, value);
            }
        }


        private string _进出方向;


        /// <summary>
        /// 进出方向
        /// </summary>

        [AdvQueryAttribute(ColName = "进出方向", ColDesc = "进出方向")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "进出方向",  Length = 10, IsNullable = false, ColumnDescription = "进出方向")]
        [Display(Name = "")]
        public string 进出方向
        {
            get { return _进出方向; }
            set
            {
                SetProperty(ref _进出方向, value);
            }
        }

        private DateTime? _日期;


        /// <summary>
        /// 日期
        /// </summary>

        [AdvQueryAttribute(ColName = "日期", ColDesc = "日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "日期", DecimalDigits = 3, Length = 8, IsNullable = true, ColumnDescription = "发生日期")]
        [Display(Name = "")]
        public DateTime? 日期
        {
            get { return _日期; }
            set
            {
                SetProperty(ref _日期, value);
            }
        }







        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
            return rs;
        }

 

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

