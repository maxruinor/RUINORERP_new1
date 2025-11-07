
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:17
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
    /// 合同明细
    /// </summary>
    [Serializable()]
    [Description("合同明细")]
    [SugarTable("tb_SO_ContractDetail")]
    public partial class tb_SO_ContractDetail: BaseEntity, ICloneable
    {
        public tb_SO_ContractDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("合同明细tb_SO_ContractDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SOContractSub_ID;
        /// <summary>
        /// 明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SOContractSub_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "明细" , IsPrimaryKey = true)]
        public long SOContractSub_ID
        { 
            get{return _SOContractSub_ID;}
            set{
            SetProperty(ref _SOContractSub_ID, value);
                base.PrimaryKeyID = _SOContractSub_ID;
            }
        }

        private long? _SOContractID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SOContractID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SOContractID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_SO_Contract","SOContractID")]
        public long? SOContractID
        { 
            get{return _SOContractID;}
            set{
            SetProperty(ref _SOContractID, value);
                        }
        }

        private long? _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "产品" )]
        public long? ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private string _ItemName;
        /// <summary>
        /// 项目名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ItemName",ColDesc = "项目名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ItemName" ,Length=100,IsNullable = true,ColumnDescription = "项目名称" )]
        public string ItemName
        { 
            get{return _ItemName;}
            set{
            SetProperty(ref _ItemName, value);
                        }
        }

        private string _ItemNumber;
        /// <summary>
        /// 项目编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ItemNumber",ColDesc = "项目编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ItemNumber" ,Length=50,IsNullable = true,ColumnDescription = "项目编号" )]
        public string ItemNumber
        { 
            get{return _ItemNumber;}
            set{
            SetProperty(ref _ItemNumber, value);
                        }
        }

        private string _Specification;
        /// <summary>
        /// 规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specification",ColDesc = "规格")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "Specification" ,Length=100,IsNullable = true,ColumnDescription = "规格" )]
        public string Specification
        { 
            get{return _Specification;}
            set{
            SetProperty(ref _Specification, value);
                        }
        }

        private string _Unit;
        /// <summary>
        /// 单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit",ColDesc = "单位")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Unit" ,Length=20,IsNullable = true,ColumnDescription = "单位" )]
        public string Unit
        { 
            get{return _Unit;}
            set{
            SetProperty(ref _Unit, value);
                        }
        }

        private int? _Quantity;
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数量" )]
        public int? Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
                        }
        }

        private decimal? _UnitPrice;
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitPrice" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "单价" )]
        public decimal? UnitPrice
        { 
            get{return _UnitPrice;}
            set{
            SetProperty(ref _UnitPrice, value);
                        }
        }

        private decimal _SubtotalAmount;
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalAmount",ColDesc = "金额小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal SubtotalAmount
        { 
            get{return _SubtotalAmount;}
            set{
            SetProperty(ref _SubtotalAmount, value);
                        }
        }

        private bool _IsIncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IsIncludeTax",ColDesc = "含税")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsIncludeTax" ,IsNullable = false,ColumnDescription = "含税" )]
        public bool IsIncludeTax
        { 
            get{return _IsIncludeTax;}
            set{
            SetProperty(ref _IsIncludeTax, value);
                        }
        }

        private decimal? _TaxRate;
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 2,IsNullable = true,ColumnDescription = "税率" )]
        public decimal? TaxRate
        { 
            get{return _TaxRate;}
            set{
            SetProperty(ref _TaxRate, value);
                        }
        }

        private decimal? _TaxAmount;
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "税额" )]
        public decimal? TaxAmount
        { 
            get{return _TaxAmount;}
            set{
            SetProperty(ref _TaxAmount, value);
                        }
        }

        private string _Remarks;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remarks",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Remarks" ,Length=500,IsNullable = true,ColumnDescription = "备注" )]
        public string Remarks
        { 
            get{return _Remarks;}
            set{
            SetProperty(ref _Remarks, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(SOContractID))]
        public virtual tb_SO_Contract tb_so_contract { get; set; }



        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_SO_ContractDetail loctype = (tb_SO_ContractDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

