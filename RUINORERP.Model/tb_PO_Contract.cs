
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:01
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
    /// 先采购合同再订单,条款内容后面补充 注意一个合同可以多个发票一个发票也可以多个合同
    /// </summary>
    [Serializable()]
    [Description("先采购合同再订单,条款内容后面补充 注意一个合同可以多个发票一个发票也可以多个合同")]
    [SugarTable("tb_PO_Contract")]
    public partial class tb_PO_Contract: BaseEntity, ICloneable
    {
        public tb_PO_Contract()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("先采购合同再订单,条款内容后面补充 注意一个合同可以多个发票一个发票也可以多个合同tb_PO_Contract" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _POContractID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "POContractID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long POContractID
        { 
            get{return _POContractID;}
            set{
            SetProperty(ref _POContractID, value);
                base.PrimaryKeyID = _POContractID;
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                        }
        }

        private long? _ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Company","ID")]
        public long? ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                        }
        }

        private long? _TemplateId;
        /// <summary>
        /// 明细
        /// </summary>
        [AdvQueryAttribute(ColName = "TemplateId",ColDesc = "明细")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "TemplateId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "明细" )]
        [FKRelationAttribute("tb_ContractTemplate","TemplateId")]
        public long? TemplateId
        { 
            get{return _TemplateId;}
            set{
            SetProperty(ref _TemplateId, value);
                        }
        }

        private string _POContractNo;
        /// <summary>
        /// 合同编号
        /// </summary>
        [AdvQueryAttribute(ColName = "POContractNo",ColDesc = "合同编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "POContractNo" ,Length=50,IsNullable = true,ColumnDescription = "合同编号" )]
        public string POContractNo
        { 
            get{return _POContractNo;}
            set{
            SetProperty(ref _POContractNo, value);
                        }
        }

        private long? _Buyer;
        /// <summary>
        /// 采购方
        /// </summary>
        [AdvQueryAttribute(ColName = "Buyer",ColDesc = "采购方")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Buyer" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "采购方" )]
        public long? Buyer
        { 
            get{return _Buyer;}
            set{
            SetProperty(ref _Buyer, value);
                        }
        }

        private long? _Seller;
        /// <summary>
        /// 销售方
        /// </summary>
        [AdvQueryAttribute(ColName = "Seller",ColDesc = "销售方")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Seller" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "销售方" )]
        public long? Seller
        { 
            get{return _Seller;}
            set{
            SetProperty(ref _Seller, value);
                        }
        }

        private DateTime? _Contract_Date;
        /// <summary>
        /// 签署日期
        /// </summary>
        [AdvQueryAttribute(ColName = "Contract_Date",ColDesc = "签署日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Contract_Date" ,IsNullable = true,ColumnDescription = "签署日期" )]
        public DateTime? Contract_Date
        { 
            get{return _Contract_Date;}
            set{
            SetProperty(ref _Contract_Date, value);
                        }
        }

        private bool? _ContractType;
        /// <summary>
        /// 合同类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ContractType",ColDesc = "合同类型")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ContractType" ,IsNullable = true,ColumnDescription = "合同类型" )]
        public bool? ContractType
        { 
            get{return _ContractType;}
            set{
            SetProperty(ref _ContractType, value);
                        }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 业务员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "业务员" )]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private int? _TotalQty;
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "总数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalQty" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "总数量" )]
        public int? TotalQty
        { 
            get{return _TotalQty;}
            set{
            SetProperty(ref _TotalQty, value);
                        }
        }

        private decimal? _TotalAmount;
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "总金额" )]
        public decimal? TotalAmount
        { 
            get{return _TotalAmount;}
            set{
            SetProperty(ref _TotalAmount, value);
                        }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
                        }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
                        }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
                        }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
                        }
        }

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
                        }
        }

        private int _PrintStatus= ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus",ColDesc = "打印状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PrintStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "打印状态" )]
        public int PrintStatus
        { 
            get{return _PrintStatus;}
            set{
            SetProperty(ref _PrintStatus, value);
                        }
        }

        private string _ClauseContent;
        /// <summary>
        /// 条款内容
        /// </summary>
        [AdvQueryAttribute(ColName = "ClauseContent",ColDesc = "条款内容")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "ClauseContent" ,Length=2147483647,IsNullable = true,ColumnDescription = "条款内容" )]
        public string ClauseContent
        { 
            get{return _ClauseContent;}
            set{
            SetProperty(ref _ClauseContent, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ID))]
        public virtual tb_Company tb_company { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(TemplateId))]
        public virtual tb_ContractTemplate tb_contracttemplate { get; set; }



        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_PO_Contract loctype = (tb_PO_Contract)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

