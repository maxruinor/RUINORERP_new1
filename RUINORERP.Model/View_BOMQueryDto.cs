﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/10/2024 20:24:12
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
    /// View_BOM
    /// </summary>
    [Serializable()]
    [SugarTable("View_BOM")]
    public class View_BOMQueryDto:BaseEntity, ICloneable
    {
        public View_BOMQueryDto()
        {

        }

    
        private long? _BOM_ID;
        
        
        /// <summary>
        /// 标准配方
        /// </summary>
        [SugarColumn(ColumnName = "BOM_ID",IsNullable = true,ColumnDescription = "标准配方")]
        [Display(Name = "标准配方")]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}
        }

        private string _BOM_No;
        
        
        /// <summary>
        /// 配方编号
        /// </summary>
        [SugarColumn(ColumnName = "BOM_No",Length=50,IsNullable = true,ColumnDescription = "配方编号")]
        [Display(Name = "配方编号")]
        public string BOM_No 
        { 
            get{return _BOM_No;}
        }

        private string _BOM_Name;
        
        
        /// <summary>
        /// 配方名称
        /// </summary>
        [SugarColumn(ColumnName = "BOM_Name",Length=100,IsNullable = true,ColumnDescription = "配方名称")]
        [Display(Name = "配方名称")]
        public string BOM_Name 
        { 
            get{return _BOM_Name;}
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU
        /// </summary>
        [SugarColumn(ColumnName = "SKU",Length=80,IsNullable = true,ColumnDescription = "SKU")]
        [Display(Name = "SKU")]
        public string SKU 
        { 
            get{return _SKU;}
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>
        [SugarColumn(ColumnName = "Type_ID",IsNullable = true,ColumnDescription = "产品类型")]
        [Display(Name = "产品类型")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}
        }

        private string _CNName;
        
        
        /// <summary>
        /// 品名
        /// </summary>
        [SugarColumn(ColumnName = "CNName",Length=255,IsNullable = true,ColumnDescription = "品名")]
        [Display(Name = "品名")]
        public string CNName 
        { 
            get{return _CNName;}
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 母件
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "母件")]
        [Display(Name = "母件")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 制造部门
        /// </summary>
        [SugarColumn(ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "制造部门")]
        [Display(Name = "制造部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
        }

        private long? _Doc_ID;
        
        
        /// <summary>
        /// 工艺文件
        /// </summary>
        [SugarColumn(ColumnName = "Doc_ID",IsNullable = true,ColumnDescription = "工艺文件")]
        [Display(Name = "工艺文件")]
        public long? Doc_ID 
        { 
            get{return _Doc_ID;}
        }

        private long? _BOM_S_VERID;
        
        
        /// <summary>
        /// 版本号
        /// </summary>
        [SugarColumn(ColumnName = "BOM_S_VERID",IsNullable = true,ColumnDescription = "版本号")]
        [Display(Name = "版本号")]
        public long? BOM_S_VERID 
        { 
            get{return _BOM_S_VERID;}
        }

        private DateTime? _Effective_at;
        
        
        /// <summary>
        /// 生效时间
        /// </summary>
        [SugarColumn(ColumnName = "Effective_at",IsNullable = true,ColumnDescription = "生效时间")]
        [Display(Name = "生效时间")]
        public DateTime? Effective_at 
        { 
            get{return _Effective_at;}
        }

        private bool? _is_enabled;
        
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "is_enabled",IsNullable = true,ColumnDescription = "是否启用")]
        [Display(Name = "是否启用")]
        public bool? is_enabled 
        { 
            get{return _is_enabled;}
        }

        private bool? _is_available;
        
        
        /// <summary>
        /// 是否可用
        /// </summary>
        [SugarColumn(ColumnName = "is_available",IsNullable = true,ColumnDescription = "是否可用")]
        [Display(Name = "是否可用")]
        public bool? is_available 
        { 
            get{return _is_available;}
        }

        private decimal? _ManufacturingCost;
        
        
        /// <summary>
        /// 自产制造成本
        /// </summary>
        [SugarColumn(ColumnName = "ManufacturingCost",IsNullable = true,ColumnDescription = "自产制造成本")]
        [Display(Name = "自产制造成本")]
        public decimal? ManufacturingCost 
        { 
            get{return _ManufacturingCost;}
        }

        private decimal? _OutManuCost;
        
        
        /// <summary>
        /// 外发加工费用
        /// </summary>
        [SugarColumn(ColumnName = "OutManuCost",IsNullable = true,ColumnDescription = "外发加工费用")]
        [Display(Name = "外发加工费用")]
        public decimal? OutManuCost 
        { 
            get{return _OutManuCost;}
        }

        private decimal? _TotalMaterialCost;
        
        
        /// <summary>
        /// 物料成本
        /// </summary>
        [SugarColumn(ColumnName = "TotalMaterialCost",IsNullable = true,ColumnDescription = "物料成本")]
        [Display(Name = "物料成本")]
        public decimal? TotalMaterialCost 
        { 
            get{return _TotalMaterialCost;}
        }

        private decimal? _TotalMaterialQty;
        
        
        /// <summary>
        /// 用料总量
        /// </summary>
        [SugarColumn(ColumnName = "TotalMaterialQty",Length=15,IsNullable = true,ColumnDescription = "用料总量")]
        [Display(Name = "用料总量")]
        public decimal? TotalMaterialQty 
        { 
            get{return _TotalMaterialQty;}
        }

        private decimal? _OutputQty;
        
        
        /// <summary>
        /// 产出量
        /// </summary>
        [SugarColumn(ColumnName = "OutputQty",Length=15,IsNullable = true,ColumnDescription = "产出量")]
        [Display(Name = "产出量")]
        public decimal? OutputQty 
        { 
            get{return _OutputQty;}
        }

        private decimal? _PeopleQty;
        
        
        /// <summary>
        /// 人数
        /// </summary>
        [SugarColumn(ColumnName = "PeopleQty",Length=15,IsNullable = true,ColumnDescription = "人数")]
        [Display(Name = "人数")]
        public decimal? PeopleQty 
        { 
            get{return _PeopleQty;}
        }

        private decimal? _WorkingHour;
        
        
        /// <summary>
        /// 工时
        /// </summary>
        [SugarColumn(ColumnName = "WorkingHour",Length=15,IsNullable = true,ColumnDescription = "工时")]
        [Display(Name = "工时")]
        public decimal? WorkingHour 
        { 
            get{return _WorkingHour;}
        }

        private decimal? _MachineHour;
        
        
        /// <summary>
        /// 机时
        /// </summary>
        [SugarColumn(ColumnName = "MachineHour",Length=15,IsNullable = true,ColumnDescription = "机时")]
        [Display(Name = "机时")]
        public decimal? MachineHour 
        { 
            get{return _MachineHour;}
        }

        private DateTime? _ExpirationDate;
        
        
        /// <summary>
        /// 截止日期
        /// </summary>
        [SugarColumn(ColumnName = "ExpirationDate",IsNullable = true,ColumnDescription = "截止日期")]
        [Display(Name = "截止日期")]
        public DateTime? ExpirationDate 
        { 
            get{return _ExpirationDate;}
        }

        private decimal? _DailyQty;
        
        
        /// <summary>
        /// 日产量
        /// </summary>
        [SugarColumn(ColumnName = "DailyQty",IsNullable = true,ColumnDescription = "日产量")]
        [Display(Name = "日产量")]
        public decimal? DailyQty 
        { 
            get{return _DailyQty;}
        }

        private decimal? _SelfProductionAllCosts;
        
        
        /// <summary>
        /// 自产总成本
        /// </summary>
        [SugarColumn(ColumnName = "SelfProductionAllCosts",IsNullable = true,ColumnDescription = "自产总成本")]
        [Display(Name = "自产总成本")]
        public decimal? SelfProductionAllCosts 
        { 
            get{return _SelfProductionAllCosts;}
        }

        private decimal? _OutProductionAllCosts;
        
        
        /// <summary>
        /// 外发总成本
        /// </summary>
        [SugarColumn(ColumnName = "OutProductionAllCosts",IsNullable = true,ColumnDescription = "外发总成本")]
        [Display(Name = "外发总成本")]
        public decimal? OutProductionAllCosts 
        { 
            get{return _OutProductionAllCosts;}
        }

        private byte[] _BOM_Iimage;
        
        
        /// <summary>
        /// BOM图片
        /// </summary>
        [SugarColumn(ColumnName = "BOM_Iimage",IsNullable = true,ColumnDescription = "BOM图片")]
        [Display(Name = "BOM图片")]
        public byte[] BOM_Iimage 
        { 
            get{return _BOM_Iimage;}
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注说明
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=500,IsNullable = true,ColumnDescription = "备注说明")]
        [Display(Name = "备注说明")]
        public string Notes 
        { 
            get{return _Notes;}
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间")]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人")]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return _Created_by;}
        }

        private DateTime? _Modified_at;
        
        
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间")]
        [Display(Name = "修改时间")]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
        }

        private long? _Modified_by;
        
        
        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人")]
        [Display(Name = "修改人")]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
        }

        private bool? _isdeleted;
        
        
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [SugarColumn(ColumnName = "isdeleted",IsNullable = true,ColumnDescription = "逻辑删除")]
        [Display(Name = "逻辑删除")]
        public bool? isdeleted 
        { 
            get{return _isdeleted;}
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 数据状态
        /// </summary>
        [SugarColumn(ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "数据状态")]
        [Display(Name = "数据状态")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 审批意见
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalOpinions",Length=500,IsNullable = true,ColumnDescription = "审批意见")]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
        }

        private long? _Approver_by;
        
        
        /// <summary>
        /// 审批人
        /// </summary>
        [SugarColumn(ColumnName = "Approver_by",IsNullable = true,ColumnDescription = "审批人")]
        [Display(Name = "审批人")]
        public long? Approver_by 
        { 
            get{return _Approver_by;}
        }

        private DateTime? _Approver_at;
        
        
        /// <summary>
        /// 审批时间
        /// </summary>
        [SugarColumn(ColumnName = "Approver_at",IsNullable = true,ColumnDescription = "审批时间")]
        [Display(Name = "审批时间")]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 审批状态
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "审批状态")]
        [Display(Name = "审批状态")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 审批结果
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "审批结果")]
        [Display(Name = "审批结果")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}
