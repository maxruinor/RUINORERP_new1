﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/27/2024 19:26:24
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
    /// 借出单统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProdBorrowing")]
    public class View_ProdBorrowing:BaseEntity, ICloneable
    {
        public View_ProdBorrowing()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_ProdBorrowing" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _BorrowID;
        
        
        /// <summary>
        /// BorrowID
        /// </summary>

        [AdvQueryAttribute(ColName = "BorrowID",ColDesc = "BorrowID")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BorrowID" ,IsNullable = true,ColumnDescription = "BorrowID" )]
        [Display(Name = "BorrowID")]
        public long? BorrowID 
        { 
            get{return _BorrowID;}
        }

        private string _BorrowNo;
        
        
        /// <summary>
        /// 借出单号
        /// </summary>

        [AdvQueryAttribute(ColName = "BorrowNo",ColDesc = "借出单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BorrowNo" ,Length=50,IsNullable = true,ColumnDescription = "借出单号" )]
        [Display(Name = "借出单号")]
        public string BorrowNo 
        { 
            get{return _BorrowNo;}
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 接收单位
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "接收单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "接收单位" )]
        [Display(Name = "接收单位")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 借出人
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "借出人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "借出人" )]
        [Display(Name = "借出人")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
        }

        private DateTime? _Out_date;
        
        
        /// <summary>
        /// 出库日期
        /// </summary>

        [AdvQueryAttribute(ColName = "Out_date",ColDesc = "出库日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Out_date" ,IsNullable = true,ColumnDescription = "出库日期" )]
        [Display(Name = "出库日期")]
        public DateTime? Out_date 
        { 
            get{return _Out_date;}
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1500,IsNullable = true,ColumnDescription = "备注" )]
        [Display(Name = "备注")]
        public string Notes 
        { 
            get{return _Notes;}
        }

        private DateTime? _DueDate;
        
        
        /// <summary>
        /// 到期日期
        /// </summary>

        [AdvQueryAttribute(ColName = "DueDate",ColDesc = "到期日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DueDate" ,IsNullable = true,ColumnDescription = "到期日期" )]
        [Display(Name = "到期日期")]
        public DateTime? DueDate 
        { 
            get{return _DueDate;}
        }

        private string _Reason;
        
        
        /// <summary>
        /// 借出原因
        /// </summary>

        [AdvQueryAttribute(ColName = "Reason",ColDesc = "借出原因")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Reason" ,Length=500,IsNullable = true,ColumnDescription = "借出原因" )]
        [Display(Name = "借出原因")]
        public string Reason 
        { 
            get{return _Reason;}
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 数据状态
        /// </summary>

        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" ,IsNullable = true,ColumnDescription = "数据状态" )]
        [Display(Name = "数据状态")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}
        }

        private string _CloseCaseOpinions;
        
        
        /// <summary>
        /// 结案意见
        /// </summary>

        [AdvQueryAttribute(ColName = "CloseCaseOpinions",ColDesc = "结案意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CloseCaseOpinions" ,Length=200,IsNullable = true,ColumnDescription = "结案意见" )]
        [Display(Name = "结案意见")]
        public string CloseCaseOpinions 
        { 
            get{return _CloseCaseOpinions;}
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 审批意见
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=500,IsNullable = true,ColumnDescription = "审批意见" )]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" ,IsNullable = true,ColumnDescription = "产品" )]
        [Display(Name = "产品")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU码
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU",ColDesc = "SKU码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "SKU码" )]
        [Display(Name = "SKU码")]
        public string SKU 
        { 
            get{return _SKU;}
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 规格
        /// </summary>

        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "规格")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications" ,Length=1000,IsNullable = true,ColumnDescription = "规格" )]
        [Display(Name = "规格")]
        public string Specifications 
        { 
            get{return _Specifications;}
        }

        private string _CNName;
        
        
        /// <summary>
        /// 品名
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName",ColDesc = "品名")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName" ,Length=255,IsNullable = true,ColumnDescription = "品名" )]
        [Display(Name = "品名")]
        public string CNName 
        { 
            get{return _CNName;}
        }

        private string _Model;
        
        
        /// <summary>
        /// 型号
        /// </summary>

        [AdvQueryAttribute(ColName = "Model",ColDesc = "型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model" ,Length=50,IsNullable = true,ColumnDescription = "型号" )]
        [Display(Name = "型号")]
        public string Model 
        { 
            get{return _Model;}
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" ,IsNullable = true,ColumnDescription = "产品类型" )]
        [Display(Name = "产品类型")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}
        }

        private string _property;
        
        
        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        [Display(Name = "属性")]
        public string property 
        { 
            get{return _property;}
        }

        private long? _Location_ID;
        
        
        /// <summary>
        /// 库位
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" ,IsNullable = true,ColumnDescription = "库位" )]
        [Display(Name = "库位")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}
        }

        private int? _Qty;
        
        
        /// <summary>
        /// 借出数量
        /// </summary>

        [AdvQueryAttribute(ColName = "Qty",ColDesc = "借出数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Qty" ,IsNullable = true,ColumnDescription = "借出数量" )]
        [Display(Name = "借出数量")]
        public int? Qty 
        { 
            get{return _Qty;}
        }

        private int? _ReQty;
        
        
        /// <summary>
        /// 归还数量
        /// </summary>

        [AdvQueryAttribute(ColName = "ReQty",ColDesc = "归还数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReQty" ,IsNullable = true,ColumnDescription = "归还数量" )]
        [Display(Name = "归还数量")]
        public int? ReQty 
        { 
            get{return _ReQty;}
        }

        private decimal? _Price;
        
        
        /// <summary>
        /// 售价
        /// </summary>

        [AdvQueryAttribute(ColName = "Price",ColDesc = "售价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Price" ,IsNullable = true,ColumnDescription = "售价" )]
        [Display(Name = "售价")]
        public decimal? Price 
        { 
            get{return _Price;}
        }

        private decimal? _SubtotalPirceAmount;
        
        
        /// <summary>
        /// 金额小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalPirceAmount",ColDesc = "金额小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalPirceAmount" ,IsNullable = true,ColumnDescription = "金额小计" )]
        [Display(Name = "金额小计")]
        public decimal? SubtotalPirceAmount 
        { 
            get{return _SubtotalPirceAmount;}
        }

        private decimal? _Cost;
        
        
        /// <summary>
        /// 成本
        /// </summary>

        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost" ,IsNullable = true,ColumnDescription = "成本" )]
        [Display(Name = "成本")]
        public decimal? Cost 
        { 
            get{return _Cost;}
        }

        private decimal? _SubtotalCostAmount;
        
        
        /// <summary>
        /// 成本小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCostAmount" ,IsNullable = true,ColumnDescription = "成本小计" )]
        [Display(Name = "成本小计")]
        public decimal? SubtotalCostAmount 
        { 
            get{return _SubtotalCostAmount;}
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=500,IsNullable = true,ColumnDescription = "摘要" )]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}
        }







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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(View_ProdBorrowing);
                    
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
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}
