
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
            get{return _BOM_ID;}            set{                SetProperty(ref _BOM_ID, value);                }
        }

        private string _BOM_No;
        
        
        /// <summary>
        /// 配方编号
        /// </summary>
        [SugarColumn(ColumnName = "BOM_No",Length=50,IsNullable = true,ColumnDescription = "配方编号")]
        [Display(Name = "配方编号")]
        public string BOM_No 
        { 
            get{return _BOM_No;}            set{                SetProperty(ref _BOM_No, value);                }
        }

        private string _BOM_Name;
        
        
        /// <summary>
        /// 配方名称
        /// </summary>
        [SugarColumn(ColumnName = "BOM_Name",Length=100,IsNullable = true,ColumnDescription = "配方名称")]
        [Display(Name = "配方名称")]
        public string BOM_Name 
        { 
            get{return _BOM_Name;}            set{                SetProperty(ref _BOM_Name, value);                }
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU
        /// </summary>
        [SugarColumn(ColumnName = "SKU",Length=80,IsNullable = true,ColumnDescription = "SKU")]
        [Display(Name = "SKU")]
        public string SKU 
        { 
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>
        [SugarColumn(ColumnName = "Type_ID",IsNullable = true,ColumnDescription = "产品类型")]
        [Display(Name = "产品类型")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
        }

        private string _CNName;
        
        
        /// <summary>
        /// 品名
        /// </summary>
        [SugarColumn(ColumnName = "CNName",Length=255,IsNullable = true,ColumnDescription = "品名")]
        [Display(Name = "品名")]
        public string CNName 
        { 
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 母件
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "母件")]
        [Display(Name = "母件")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 制造部门
        /// </summary>
        [SugarColumn(ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "制造部门")]
        [Display(Name = "制造部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _Doc_ID;
        
        
        /// <summary>
        /// 工艺文件
        /// </summary>
        [SugarColumn(ColumnName = "Doc_ID",IsNullable = true,ColumnDescription = "工艺文件")]
        [Display(Name = "工艺文件")]
        public long? Doc_ID 
        { 
            get{return _Doc_ID;}            set{                SetProperty(ref _Doc_ID, value);                }
        }

        private long? _BOM_S_VERID;
        
        
        /// <summary>
        /// 版本号
        /// </summary>
        [SugarColumn(ColumnName = "BOM_S_VERID",IsNullable = true,ColumnDescription = "版本号")]
        [Display(Name = "版本号")]
        public long? BOM_S_VERID 
        { 
            get{return _BOM_S_VERID;}            set{                SetProperty(ref _BOM_S_VERID, value);                }
        }

        private DateTime? _Effective_at;
        
        
        /// <summary>
        /// 生效时间
        /// </summary>
        [SugarColumn(ColumnName = "Effective_at",IsNullable = true,ColumnDescription = "生效时间")]
        [Display(Name = "生效时间")]
        public DateTime? Effective_at 
        { 
            get{return _Effective_at;}            set{                SetProperty(ref _Effective_at, value);                }
        }

        private bool? _is_enabled;
        
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "is_enabled",IsNullable = true,ColumnDescription = "是否启用")]
        [Display(Name = "是否启用")]
        public bool? is_enabled 
        { 
            get{return _is_enabled;}            set{                SetProperty(ref _is_enabled, value);                }
        }

        private bool? _is_available;
        
        
        /// <summary>
        /// 是否可用
        /// </summary>
        [SugarColumn(ColumnName = "is_available",IsNullable = true,ColumnDescription = "是否可用")]
        [Display(Name = "是否可用")]
        public bool? is_available 
        { 
            get{return _is_available;}            set{                SetProperty(ref _is_available, value);                }
        }

        private decimal? _ManufacturingCost;
        
        
        /// <summary>
        /// 自产制造成本
        /// </summary>
        [SugarColumn(ColumnName = "ManufacturingCost",IsNullable = true,ColumnDescription = "自产制造成本")]
        [Display(Name = "自产制造成本")]
        public decimal? ManufacturingCost 
        { 
            get{return _ManufacturingCost;}            set{                SetProperty(ref _ManufacturingCost, value);                }
        }

        private decimal? _OutManuCost;
        
        
        /// <summary>
        /// 外发加工费用
        /// </summary>
        [SugarColumn(ColumnName = "OutManuCost",IsNullable = true,ColumnDescription = "外发加工费用")]
        [Display(Name = "外发加工费用")]
        public decimal? OutManuCost 
        { 
            get{return _OutManuCost;}            set{                SetProperty(ref _OutManuCost, value);                }
        }

        private decimal? _TotalMaterialCost;
        
        
        /// <summary>
        /// 物料成本
        /// </summary>
        [SugarColumn(ColumnName = "TotalMaterialCost",IsNullable = true,ColumnDescription = "物料成本")]
        [Display(Name = "物料成本")]
        public decimal? TotalMaterialCost 
        { 
            get{return _TotalMaterialCost;}            set{                SetProperty(ref _TotalMaterialCost, value);                }
        }

        private decimal? _TotalMaterialQty;
        
        
        /// <summary>
        /// 用料总量
        /// </summary>
        [SugarColumn(ColumnName = "TotalMaterialQty",Length=15,IsNullable = true,ColumnDescription = "用料总量")]
        [Display(Name = "用料总量")]
        public decimal? TotalMaterialQty 
        { 
            get{return _TotalMaterialQty;}            set{                SetProperty(ref _TotalMaterialQty, value);                }
        }

        private decimal? _OutputQty;
        
        
        /// <summary>
        /// 产出量
        /// </summary>
        [SugarColumn(ColumnName = "OutputQty",Length=15,IsNullable = true,ColumnDescription = "产出量")]
        [Display(Name = "产出量")]
        public decimal? OutputQty 
        { 
            get{return _OutputQty;}            set{                SetProperty(ref _OutputQty, value);                }
        }

        private decimal? _PeopleQty;
        
        
        /// <summary>
        /// 人数
        /// </summary>
        [SugarColumn(ColumnName = "PeopleQty",Length=15,IsNullable = true,ColumnDescription = "人数")]
        [Display(Name = "人数")]
        public decimal? PeopleQty 
        { 
            get{return _PeopleQty;}            set{                SetProperty(ref _PeopleQty, value);                }
        }

        private decimal? _WorkingHour;
        
        
        /// <summary>
        /// 工时
        /// </summary>
        [SugarColumn(ColumnName = "WorkingHour",Length=15,IsNullable = true,ColumnDescription = "工时")]
        [Display(Name = "工时")]
        public decimal? WorkingHour 
        { 
            get{return _WorkingHour;}            set{                SetProperty(ref _WorkingHour, value);                }
        }

        private decimal? _MachineHour;
        
        
        /// <summary>
        /// 机时
        /// </summary>
        [SugarColumn(ColumnName = "MachineHour",Length=15,IsNullable = true,ColumnDescription = "机时")]
        [Display(Name = "机时")]
        public decimal? MachineHour 
        { 
            get{return _MachineHour;}            set{                SetProperty(ref _MachineHour, value);                }
        }

        private DateTime? _ExpirationDate;
        
        
        /// <summary>
        /// 截止日期
        /// </summary>
        [SugarColumn(ColumnName = "ExpirationDate",IsNullable = true,ColumnDescription = "截止日期")]
        [Display(Name = "截止日期")]
        public DateTime? ExpirationDate 
        { 
            get{return _ExpirationDate;}            set{                SetProperty(ref _ExpirationDate, value);                }
        }

        private decimal? _DailyQty;
        
        
        /// <summary>
        /// 日产量
        /// </summary>
        [SugarColumn(ColumnName = "DailyQty",IsNullable = true,ColumnDescription = "日产量")]
        [Display(Name = "日产量")]
        public decimal? DailyQty 
        { 
            get{return _DailyQty;}            set{                SetProperty(ref _DailyQty, value);                }
        }

        private decimal? _SelfProductionAllCosts;
        
        
        /// <summary>
        /// 自产总成本
        /// </summary>
        [SugarColumn(ColumnName = "SelfProductionAllCosts",IsNullable = true,ColumnDescription = "自产总成本")]
        [Display(Name = "自产总成本")]
        public decimal? SelfProductionAllCosts 
        { 
            get{return _SelfProductionAllCosts;}            set{                SetProperty(ref _SelfProductionAllCosts, value);                }
        }

        private decimal? _OutProductionAllCosts;
        
        
        /// <summary>
        /// 外发总成本
        /// </summary>
        [SugarColumn(ColumnName = "OutProductionAllCosts",IsNullable = true,ColumnDescription = "外发总成本")]
        [Display(Name = "外发总成本")]
        public decimal? OutProductionAllCosts 
        { 
            get{return _OutProductionAllCosts;}            set{                SetProperty(ref _OutProductionAllCosts, value);                }
        }

        private byte[] _BOM_Iimage;
        
        
        /// <summary>
        /// BOM图片
        /// </summary>
        [SugarColumn(ColumnName = "BOM_Iimage",IsNullable = true,ColumnDescription = "BOM图片")]
        [Display(Name = "BOM图片")]
        public byte[] BOM_Iimage 
        { 
            get{return _BOM_Iimage;}            set{                SetProperty(ref _BOM_Iimage, value);                }
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注说明
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=500,IsNullable = true,ColumnDescription = "备注说明")]
        [Display(Name = "备注说明")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间")]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}            set{                SetProperty(ref _Created_at, value);                }
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人")]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return _Created_by;}            set{                SetProperty(ref _Created_by, value);                }
        }

        private DateTime? _Modified_at;
        
        
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间")]
        [Display(Name = "修改时间")]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}            set{                SetProperty(ref _Modified_at, value);                }
        }

        private long? _Modified_by;
        
        
        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人")]
        [Display(Name = "修改人")]
        public long? Modified_by 
        { 
            get{return _Modified_by;}            set{                SetProperty(ref _Modified_by, value);                }
        }

        private bool? _isdeleted;
        
        
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [SugarColumn(ColumnName = "isdeleted",IsNullable = true,ColumnDescription = "逻辑删除")]
        [Display(Name = "逻辑删除")]
        public bool? isdeleted 
        { 
            get{return _isdeleted;}            set{                SetProperty(ref _isdeleted, value);                }
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 数据状态
        /// </summary>
        [SugarColumn(ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "数据状态")]
        [Display(Name = "数据状态")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}            set{                SetProperty(ref _DataStatus, value);                }
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 审批意见
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalOpinions",Length=500,IsNullable = true,ColumnDescription = "审批意见")]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}            set{                SetProperty(ref _ApprovalOpinions, value);                }
        }

        private long? _Approver_by;
        
        
        /// <summary>
        /// 审批人
        /// </summary>
        [SugarColumn(ColumnName = "Approver_by",IsNullable = true,ColumnDescription = "审批人")]
        [Display(Name = "审批人")]
        public long? Approver_by 
        { 
            get{return _Approver_by;}            set{                SetProperty(ref _Approver_by, value);                }
        }

        private DateTime? _Approver_at;
        
        
        /// <summary>
        /// 审批时间
        /// </summary>
        [SugarColumn(ColumnName = "Approver_at",IsNullable = true,ColumnDescription = "审批时间")]
        [Display(Name = "审批时间")]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}            set{                SetProperty(ref _Approver_at, value);                }
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 审批状态
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "审批状态")]
        [Display(Name = "审批状态")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}            set{                SetProperty(ref _ApprovalStatus, value);                }
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 审批结果
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "审批结果")]
        [Display(Name = "审批结果")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}            set{                SetProperty(ref _ApprovalResults, value);                }
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

