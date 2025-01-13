
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:23
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
    /// 采购退回单 当采购发生退回时，您可以将它记录在本系统的采购退回作业中。在实际业务中虽然发生了采购但实际货物却还未入库，采购退回作业可退回订金、退回数量处理。采购退回单可以由采购订单转入，也可以手动录入新增单据,一般没有金额变化的，可以直接作废采购单。有订单等才需要做退回
    /// </summary>
    [Serializable()]
    [Description("tb_PurOrderRe")]
    [SugarTable("tb_PurOrderRe")]
    public partial class tb_PurOrderRe: BaseEntity, ICloneable
    {
        public tb_PurOrderRe()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_PurOrderRe" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PurRetrunID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurRetrunID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PurRetrunID
        { 
            get{return _PurRetrunID;}
            set{
            base.PrimaryKeyID = _PurRetrunID;
            SetProperty(ref _PurRetrunID, value);
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

        private long? _PurOrder_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PurOrder_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurOrder_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_PurOrder","PurOrder_ID")]
        public long? PurOrder_ID
        { 
            get{return _PurOrder_ID;}
            set{
            SetProperty(ref _PurOrder_ID, value);
            }
        }

        private string _PurOrderNo;
        /// <summary>
        /// 采购单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PurOrderNo",ColDesc = "采购单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PurOrderNo" ,Length=100,IsNullable = false,ColumnDescription = "采购单号" )]
        public string PurOrderNo
        { 
            get{return _PurOrderNo;}
            set{
            SetProperty(ref _PurOrderNo, value);
            }
        }

        private DateTime? _ReturnDate;
        /// <summary>
        /// 退回日期
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnDate",ColDesc = "退回日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ReturnDate" ,IsNullable = true,ColumnDescription = "退回日期" )]
        public DateTime? ReturnDate
        { 
            get{return _ReturnDate;}
            set{
            SetProperty(ref _ReturnDate, value);
            }
        }

        private string _PurReturnNo;
        /// <summary>
        /// 退回单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PurReturnNo",ColDesc = "退回单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PurReturnNo" ,Length=50,IsNullable = true,ColumnDescription = "退回单号" )]
        public string PurReturnNo
        { 
            get{return _PurReturnNo;}
            set{
            SetProperty(ref _PurReturnNo, value);
            }
        }

        private decimal _GetPayment;
        /// <summary>
        /// 实际收回订金货款
        /// </summary>
        [AdvQueryAttribute(ColName = "GetPayment",ColDesc = "实际收回订金货款")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "GetPayment" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "实际收回订金货款" )]
        public decimal GetPayment
        { 
            get{return _GetPayment;}
            set{
            SetProperty(ref _GetPayment, value);
            }
        }

        private decimal _TotalTaxAmount;
        /// <summary>
        /// 总计税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalTaxAmount",ColDesc = "总计税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalTaxAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "总计税额" )]
        public decimal TotalTaxAmount
        { 
            get{return _TotalTaxAmount;}
            set{
            SetProperty(ref _TotalTaxAmount, value);
            }
        }

        private decimal _TotalAmount;
        /// <summary>
        /// 总计金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总计金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "总计金额" )]
        public decimal TotalAmount
        { 
            get{return _TotalAmount;}
            set{
            SetProperty(ref _TotalAmount, value);
            }
        }

        private DateTime? _DeliveryDate;
        /// <summary>
        /// 发货日期
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveryDate",ColDesc = "发货日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DeliveryDate" ,IsNullable = true,ColumnDescription = "发货日期" )]
        public DateTime? DeliveryDate
        { 
            get{return _DeliveryDate;}
            set{
            SetProperty(ref _DeliveryDate, value);
            }
        }

        private string _ReturnAddress;
        /// <summary>
        /// 退回地址
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnAddress",ColDesc = "退回地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReturnAddress" ,Length=255,IsNullable = true,ColumnDescription = "退回地址" )]
        public string ReturnAddress
        { 
            get{return _ReturnAddress;}
            set{
            SetProperty(ref _ReturnAddress, value);
            }
        }

        private string _ShippingWay;
        /// <summary>
        /// 发货方式
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingWay",ColDesc = "发货方式")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ShippingWay" ,Length=50,IsNullable = true,ColumnDescription = "发货方式" )]
        public string ShippingWay
        { 
            get{return _ShippingWay;}
            set{
            SetProperty(ref _ShippingWay, value);
            }
        }

        private string _TrackNo;
        /// <summary>
        /// 物流单号
        /// </summary>
        [AdvQueryAttribute(ColName = "TrackNo",ColDesc = "物流单号")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "TrackNo" ,Length=50,IsNullable = true,ColumnDescription = "物流单号" )]
        public string TrackNo
        { 
            get{return _TrackNo;}
            set{
            SetProperty(ref _TrackNo, value);
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

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions
        { 
            get{return _ApprovalOpinions;}
            set{
            SetProperty(ref _ApprovalOpinions, value);
            }
        }

        private int? _ApprovalStatus;
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")] 
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="SByte",  ColumnName = "ApprovalStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审批状态" )]
        public int? ApprovalStatus
        { 
            get{return _ApprovalStatus;}
            set{
            SetProperty(ref _ApprovalStatus, value);
            }
        }

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ApprovalResults" ,IsNullable = true,ColumnDescription = "审批结果" )]
        public bool? ApprovalResults
        { 
            get{return _ApprovalResults;}
            set{
            SetProperty(ref _ApprovalResults, value);
            }
        }

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数据状态" )]
        public int DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
            }
        }

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Approver_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审批人" )]
        public long? Approver_by
        { 
            get{return _Approver_by;}
            set{
            SetProperty(ref _Approver_by, value);
            }
        }

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Approver_at" ,IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? Approver_at
        { 
            get{return _Approver_at;}
            set{
            SetProperty(ref _Approver_at, value);
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

        private bool? _GenerateVouchers;
        /// <summary>
        /// 生成凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "GenerateVouchers",ColDesc = "生成凭证")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "GenerateVouchers" ,IsNullable = true,ColumnDescription = "生成凭证" )]
        public bool? GenerateVouchers
        { 
            get{return _GenerateVouchers;}
            set{
            SetProperty(ref _GenerateVouchers, value);
            }
        }

        private decimal? _TotalQty;
        /// <summary>
        /// 合计数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "合计数量")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalQty" , DecimalDigits = 6,IsNullable = true,ColumnDescription = "合计数量" )]
        public decimal? TotalQty
        { 
            get{return _TotalQty;}
            set{
            SetProperty(ref _TotalQty, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(PurOrder_ID))]
        public virtual tb_PurOrder tb_purorder { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }


        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrderReDetail.PurRetrunID))]
        public virtual List<tb_PurOrderReDetail> tb_PurOrderReDetails { get; set; }
        //tb_PurOrderReDetail.PurRetrunID)
        //PurRetrunID.FK_TB_PURRE_de_TB_PURORD)
        //tb_PurOrderRe.PurRetrunID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_PurOrderRe);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_PurOrderRe loctype = (tb_PurOrderRe)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

