﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/10/2024 20:24:11
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
    public class View_BOM:BaseEntity, ICloneable
    {
        public View_BOM()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_BOM" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _BOM_ID;
        
        
        /// <summary>
        /// 标准配方
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "标准配方")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" ,IsNullable = true,ColumnDescription = "标准配方" )]
        [Display(Name = "标准配方")]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}
        }

        private string _BOM_No;
        
        
        /// <summary>
        /// 配方编号
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_No",ColDesc = "配方编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BOM_No" ,Length=50,IsNullable = true,ColumnDescription = "配方编号" )]
        [Display(Name = "配方编号")]
        public string BOM_No 
        { 
            get{return _BOM_No;}
        }

        private string _BOM_Name;
        
        
        /// <summary>
        /// 配方名称
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_Name",ColDesc = "配方名称")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BOM_Name" ,Length=100,IsNullable = true,ColumnDescription = "配方名称" )]
        [Display(Name = "配方名称")]
        public string BOM_Name 
        { 
            get{return _BOM_Name;}
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU",ColDesc = "母件SKU")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "母件SKU" )]
        [Display(Name = "SKU")]
        public string SKU 
        { 
            get{return _SKU;}
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

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 母件
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "母件")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" ,IsNullable = true,ColumnDescription = "母件" )]
        [Display(Name = "母件")]
        [FKRelationAttribute("tb_ProdDetail", "ProdDetailID")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 制造部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "制造部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "制造部门" )]
        [Display(Name = "制造部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
        }

        private long? _Doc_ID;
        
        
        /// <summary>
        /// 工艺文件
        /// </summary>

        [AdvQueryAttribute(ColName = "Doc_ID",ColDesc = "工艺文件")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Doc_ID" ,IsNullable = true,ColumnDescription = "工艺文件" )]
        [Display(Name = "工艺文件")]
        public long? Doc_ID 
        { 
            get{return _Doc_ID;}
        }

        private long? _BOM_S_VERID;
        
        
        /// <summary>
        /// 版本号
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_S_VERID",ColDesc = "版本号")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_S_VERID" ,IsNullable = true,ColumnDescription = "版本号" )]
        [Display(Name = "版本号")]
        public long? BOM_S_VERID 
        { 
            get{return _BOM_S_VERID;}
        }

        private DateTime? _Effective_at;
        
        
        /// <summary>
        /// 生效时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Effective_at",ColDesc = "生效时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Effective_at" ,IsNullable = true,ColumnDescription = "生效时间" )]
        [Display(Name = "生效时间")]
        public DateTime? Effective_at 
        { 
            get{return _Effective_at;}
        }

        private bool? _is_enabled;
        
        
        /// <summary>
        /// 是否启用
        /// </summary>

        [AdvQueryAttribute(ColName = "is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_enabled" ,IsNullable = true,ColumnDescription = "是否启用" )]
        [Display(Name = "是否启用")]
        public bool? is_enabled 
        { 
            get{return _is_enabled;}
        }

        private bool? _is_available;
        
        
        /// <summary>
        /// 是否可用
        /// </summary>

        [AdvQueryAttribute(ColName = "is_available",ColDesc = "是否可用")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_available" ,IsNullable = true,ColumnDescription = "是否可用" )]
        [Display(Name = "是否可用")]
        public bool? is_available 
        { 
            get{return _is_available;}
        }

        private decimal? _ManufacturingCost;
        
        
        /// <summary>
        /// 自产制造成本
        /// </summary>

        [AdvQueryAttribute(ColName = "ManufacturingCost",ColDesc = "自产制造成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ManufacturingCost" ,IsNullable = true,ColumnDescription = "自产制造成本" )]
        [Display(Name = "自产制造成本")]
        public decimal? ManufacturingCost 
        { 
            get{return _ManufacturingCost;}
        }

        private decimal? _OutManuCost;
        
        
        /// <summary>
        /// 外发加工费用
        /// </summary>

        [AdvQueryAttribute(ColName = "OutManuCost",ColDesc = "外发加工费用")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OutManuCost" ,IsNullable = true,ColumnDescription = "外发加工费用" )]
        [Display(Name = "外发加工费用")]
        public decimal? OutManuCost 
        { 
            get{return _OutManuCost;}
        }

        private decimal? _TotalMaterialCost;
        
        
        /// <summary>
        /// 物料成本
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalMaterialCost",ColDesc = "物料成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalMaterialCost" ,IsNullable = true,ColumnDescription = "物料成本" )]
        [Display(Name = "物料成本")]
        public decimal? TotalMaterialCost 
        { 
            get{return _TotalMaterialCost;}
        }

        private decimal? _TotalMaterialQty;
        
        
        /// <summary>
        /// 用料总量
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalMaterialQty",ColDesc = "用料总量")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TotalMaterialQty" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "用料总量" )]
        [Display(Name = "用料总量")]
        public decimal? TotalMaterialQty 
        { 
            get{return _TotalMaterialQty;}
        }

        private decimal? _OutputQty;
        
        
        /// <summary>
        /// 产出量
        /// </summary>

        [AdvQueryAttribute(ColName = "OutputQty",ColDesc = "产出量")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "OutputQty" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "产出量" )]
        [Display(Name = "产出量")]
        public decimal? OutputQty 
        { 
            get{return _OutputQty;}
        }

        private decimal? _PeopleQty;
        
        
        /// <summary>
        /// 人数
        /// </summary>

        [AdvQueryAttribute(ColName = "PeopleQty",ColDesc = "人数")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "PeopleQty" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "人数" )]
        [Display(Name = "人数")]
        public decimal? PeopleQty 
        { 
            get{return _PeopleQty;}
        }

        private decimal? _WorkingHour;
        
        
        /// <summary>
        /// 工时
        /// </summary>

        [AdvQueryAttribute(ColName = "WorkingHour",ColDesc = "工时")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "WorkingHour" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "工时" )]
        [Display(Name = "工时")]
        public decimal? WorkingHour 
        { 
            get{return _WorkingHour;}
        }

        private decimal? _MachineHour;
        
        
        /// <summary>
        /// 机时
        /// </summary>

        [AdvQueryAttribute(ColName = "MachineHour",ColDesc = "机时")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "MachineHour" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "机时" )]
        [Display(Name = "机时")]
        public decimal? MachineHour 
        { 
            get{return _MachineHour;}
        }

        private DateTime? _ExpirationDate;
        
        
        /// <summary>
        /// 截止日期
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpirationDate",ColDesc = "截止日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ExpirationDate" ,IsNullable = true,ColumnDescription = "截止日期" )]
        [Display(Name = "截止日期")]
        public DateTime? ExpirationDate 
        { 
            get{return _ExpirationDate;}
        }

        private decimal? _DailyQty;
        
        
        /// <summary>
        /// 日产量
        /// </summary>

        [AdvQueryAttribute(ColName = "DailyQty",ColDesc = "日产量")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "DailyQty" ,IsNullable = true,ColumnDescription = "日产量" )]
        [Display(Name = "日产量")]
        public decimal? DailyQty 
        { 
            get{return _DailyQty;}
        }

        private decimal? _SelfProductionAllCosts;
        
        
        /// <summary>
        /// 自产总成本
        /// </summary>

        [AdvQueryAttribute(ColName = "SelfProductionAllCosts",ColDesc = "自产总成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SelfProductionAllCosts" ,IsNullable = true,ColumnDescription = "自产总成本" )]
        [Display(Name = "自产总成本")]
        public decimal? SelfProductionAllCosts 
        { 
            get{return _SelfProductionAllCosts;}
        }

        private decimal? _OutProductionAllCosts;
        
        
        /// <summary>
        /// 外发总成本
        /// </summary>

        [AdvQueryAttribute(ColName = "OutProductionAllCosts",ColDesc = "外发总成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OutProductionAllCosts" ,IsNullable = true,ColumnDescription = "外发总成本" )]
        [Display(Name = "外发总成本")]
        public decimal? OutProductionAllCosts 
        { 
            get{return _OutProductionAllCosts;}
        }

        private byte[] _BOM_Iimage;
        
        
        /// <summary>
        /// BOM图片
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_Iimage",ColDesc = "BOM图片")]
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "BOM_Iimage" ,IsNullable = true,ColumnDescription = "BOM图片" )]
        [Display(Name = "BOM图片")]
        public byte[] BOM_Iimage 
        { 
            get{return _BOM_Iimage;}
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注说明
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=500,IsNullable = true,ColumnDescription = "备注说明" )]
        [Display(Name = "备注说明")]
        public string Notes 
        { 
            get{return _Notes;}
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 创建人
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" ,IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return _Created_by;}
        }

        private DateTime? _Modified_at;
        
        
        /// <summary>
        /// 修改时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        [Display(Name = "修改时间")]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
        }

        private long? _Modified_by;
        
        
        /// <summary>
        /// 修改人
        /// </summary>

        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" ,IsNullable = true,ColumnDescription = "修改人" )]
        [Display(Name = "修改人")]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
        }

        private bool? _isdeleted;
        
        
        /// <summary>
        /// 逻辑删除
        /// </summary>

        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = true,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        [Display(Name = "逻辑删除")]
        public bool? isdeleted 
        { 
            get{return _isdeleted;}
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

        private long? _Approver_by;
        
        
        /// <summary>
        /// 审批人
        /// </summary>

        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Approver_by" ,IsNullable = true,ColumnDescription = "审批人" )]
        [Display(Name = "审批人")]
        public long? Approver_by 
        { 
            get{return _Approver_by;}
        }

        private DateTime? _Approver_at;
        
        
        /// <summary>
        /// 审批时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Approver_at" ,IsNullable = true,ColumnDescription = "审批时间" )]
        [Display(Name = "审批时间")]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 审批状态
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="Byte",  ColumnName = "ApprovalStatus" ,IsNullable = true,ColumnDescription = "审批状态" )]
        [Display(Name = "审批状态")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 审批结果
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ApprovalResults" ,IsNullable = true,ColumnDescription = "审批结果" )]
        [Display(Name = "审批结果")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}
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
                    Type type = typeof(View_BOM);
                    
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
