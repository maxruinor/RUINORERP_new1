
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
using RUINORERP.Model.Base;

namespace RUINORERP.Model
{
    /// <summary>
    /// 借出单统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProdBorrowing")]
    public partial class View_ProdBorrowing: BaseViewEntity
    {
        public View_ProdBorrowing()
        {
            
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
            get{return _BorrowID;}            set{                SetProperty(ref _BorrowID, value);                }
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
            get{return _BorrowNo;}            set{                SetProperty(ref _BorrowNo, value);                }
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
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
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
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
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
            get{return _Out_date;}            set{                SetProperty(ref _Out_date, value);                }
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
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
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
            get{return _DueDate;}            set{                SetProperty(ref _DueDate, value);                }
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
            get{return _Reason;}            set{                SetProperty(ref _Reason, value);                }
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
            get{return _DataStatus;}            set{                SetProperty(ref _DataStatus, value);                }
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
            get{return _CloseCaseOpinions;}            set{                SetProperty(ref _CloseCaseOpinions, value);                }
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
            get{return _ApprovalOpinions;}            set{                SetProperty(ref _ApprovalOpinions, value);                }
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
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
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
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
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
            get{return _Specifications;}            set{                SetProperty(ref _Specifications, value);                }
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
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
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
            get{return _Model;}            set{                SetProperty(ref _Model, value);                }
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" ,IsNullable = true,ColumnDescription = "产品类型" )]
        [Display(Name = "产品类型")]
        [FKRelationAttribute("tb_ProductType","Type_ID")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
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
            get{return _property;}            set{                SetProperty(ref _property, value);                }
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
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
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
            get{return _Qty;}            set{                SetProperty(ref _Qty, value);                }
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
            get{return _ReQty;}            set{                SetProperty(ref _ReQty, value);                }
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
            get{return _Price;}            set{                SetProperty(ref _Price, value);                }
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
            get{return _SubtotalPirceAmount;}            set{                SetProperty(ref _SubtotalPirceAmount, value);                }
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
            get{return _Cost;}            set{                SetProperty(ref _Cost, value);                }
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
            get{return _SubtotalCostAmount;}            set{                SetProperty(ref _SubtotalCostAmount, value);                }
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
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }







//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

