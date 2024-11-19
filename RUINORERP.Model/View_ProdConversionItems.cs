
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:21
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
    /// 转换明细统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProdConversionItems")]
    public class View_ProdConversionItems:BaseEntity, ICloneable
    {
        public View_ProdConversionItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_ProdConversionItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _Employee_ID;
        
        
        /// <summary>
        /// 经办人
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "经办人" )]
        [Display(Name = "经办人")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
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

        private string _ConversionNo;
        
        
        /// <summary>
        /// 转换单号
        /// </summary>

        [AdvQueryAttribute(ColName = "ConversionNo",ColDesc = "转换单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ConversionNo" ,Length=50,IsNullable = true,ColumnDescription = "转换单号" )]
        [Display(Name = "转换单号")]
        public string ConversionNo 
        { 
            get{return _ConversionNo;}            set{                SetProperty(ref _ConversionNo, value);                }
        }

        private DateTime? _ConversionDate;
        
        
        /// <summary>
        /// 单据日期
        /// </summary>

        [AdvQueryAttribute(ColName = "ConversionDate",ColDesc = "单据日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ConversionDate" ,IsNullable = true,ColumnDescription = "单据日期" )]
        [Display(Name = "单据日期")]
        public DateTime? ConversionDate 
        { 
            get{return _ConversionDate;}            set{                SetProperty(ref _ConversionDate, value);                }
        }

        private string _Reason;
        
        
        /// <summary>
        /// 转换原因
        /// </summary>

        [AdvQueryAttribute(ColName = "Reason",ColDesc = "转换原因")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Reason" ,Length=300,IsNullable = true,ColumnDescription = "转换原因" )]
        [Display(Name = "转换原因")]
        public string Reason 
        { 
            get{return _Reason;}            set{                SetProperty(ref _Reason, value);                }
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

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}            set{                SetProperty(ref _Created_at, value);                }
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
            get{return _Created_by;}            set{                SetProperty(ref _Created_by, value);                }
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

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 审批状态
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="Byte",  ColumnName = "ApprovalStatus" ,IsNullable = true,ColumnDescription = "审批状态" )]
        [Display(Name = "审批状态")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}            set{                SetProperty(ref _ApprovalStatus, value);                }
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
            get{return _ApprovalResults;}            set{                SetProperty(ref _ApprovalResults, value);                }
        }

        private long? _ProdDetailID_from;
        
        
        /// <summary>
        /// 来源产品
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID_from",ColDesc = "来源产品")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID_from" ,IsNullable = true,ColumnDescription = "来源产品" )]
        [Display(Name = "来源产品")]
        public long? ProdDetailID_from 
        { 
            get{return _ProdDetailID_from;}            set{                SetProperty(ref _ProdDetailID_from, value);                }
        }

        private string _BarCode_from;
        
        
        /// <summary>
        /// 来源条码
        /// </summary>

        [AdvQueryAttribute(ColName = "BarCode_from",ColDesc = "来源条码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BarCode_from" ,Length=255,IsNullable = true,ColumnDescription = "来源条码" )]
        [Display(Name = "来源条码")]
        public string BarCode_from 
        { 
            get{return _BarCode_from;}            set{                SetProperty(ref _BarCode_from, value);                }
        }

        private string _SKU_from;
        
        
        /// <summary>
        /// 来源SKU
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU_from",ColDesc = "来源SKU")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU_from" ,Length=255,IsNullable = true,ColumnDescription = "来源SKU" )]
        [Display(Name = "来源SKU")]
        public string SKU_from 
        { 
            get{return _SKU_from;}            set{                SetProperty(ref _SKU_from, value);                }
        }

        private long? _Type_ID_from;
        
        
        /// <summary>
        /// 来源产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID_from",ColDesc = "来源产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID_from" ,IsNullable = true,ColumnDescription = "来源产品类型" )]
        [Display(Name = "来源产品类型")]
        public long? Type_ID_from 
        { 
            get{return _Type_ID_from;}            set{                SetProperty(ref _Type_ID_from, value);                }
        }

        private string _CNName_from;
        
        
        /// <summary>
        /// 来源品名
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName_from",ColDesc = "来源品名")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName_from" ,Length=255,IsNullable = true,ColumnDescription = "来源品名" )]
        [Display(Name = "来源品名")]
        public string CNName_from 
        { 
            get{return _CNName_from;}            set{                SetProperty(ref _CNName_from, value);                }
        }

        private string _Model_from;
        
        
        /// <summary>
        /// 来源型号
        /// </summary>

        [AdvQueryAttribute(ColName = "Model_from",ColDesc = "来源型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model_from" ,Length=50,IsNullable = true,ColumnDescription = "来源型号" )]
        [Display(Name = "来源型号")]
        public string Model_from 
        { 
            get{return _Model_from;}            set{                SetProperty(ref _Model_from, value);                }
        }

        private string _Specifications_from;
        
        
        /// <summary>
        /// 来源规格
        /// </summary>

        [AdvQueryAttribute(ColName = "Specifications_from",ColDesc = "来源规格")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications_from" ,Length=1000,IsNullable = true,ColumnDescription = "来源规格" )]
        [Display(Name = "来源规格")]
        public string Specifications_from 
        { 
            get{return _Specifications_from;}            set{                SetProperty(ref _Specifications_from, value);                }
        }

        private string _property_from;
        
        
        /// <summary>
        /// 来源属性
        /// </summary>

        [AdvQueryAttribute(ColName = "property_from",ColDesc = "来源属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property_from" ,Length=255,IsNullable = true,ColumnDescription = "来源属性" )]
        [Display(Name = "来源属性")]
        public string property_from 
        { 
            get{return _property_from;}            set{                SetProperty(ref _property_from, value);                }
        }

        private int? _ConversionQty;
        
        
        /// <summary>
        /// 转换数量
        /// </summary>

        [AdvQueryAttribute(ColName = "ConversionQty",ColDesc = "转换数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ConversionQty" ,IsNullable = true,ColumnDescription = "转换数量" )]
        [Display(Name = "转换数量")]
        public int? ConversionQty 
        { 
            get{return _ConversionQty;}            set{                SetProperty(ref _ConversionQty, value);                }
        }

        private long? _ProdDetailID_to;
        
        
        /// <summary>
        /// 目标产品
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID_to",ColDesc = "目标产品")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID_to" ,IsNullable = true,ColumnDescription = "目标产品" )]
        [Display(Name = "目标产品")]
        public long? ProdDetailID_to 
        { 
            get{return _ProdDetailID_to;}            set{                SetProperty(ref _ProdDetailID_to, value);                }
        }

        private string _BarCode_to;
        
        
        /// <summary>
        /// 目标条码
        /// </summary>

        [AdvQueryAttribute(ColName = "BarCode_to",ColDesc = "目标条码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BarCode_to" ,Length=255,IsNullable = true,ColumnDescription = "目标条码" )]
        [Display(Name = "目标条码")]
        public string BarCode_to 
        { 
            get{return _BarCode_to;}            set{                SetProperty(ref _BarCode_to, value);                }
        }

        private string _SKU_to;
        
        
        /// <summary>
        /// 目标SKU
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU_to",ColDesc = "目标SKU")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU_to" ,Length=255,IsNullable = true,ColumnDescription = "目标SKU" )]
        [Display(Name = "目标SKU")]
        public string SKU_to 
        { 
            get{return _SKU_to;}            set{                SetProperty(ref _SKU_to, value);                }
        }

        private long? _Type_ID_to;
        
        
        /// <summary>
        /// 目标产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID_to",ColDesc = "目标产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID_to" ,IsNullable = true,ColumnDescription = "目标产品类型" )]
        [Display(Name = "目标产品类型")]
        public long? Type_ID_to 
        { 
            get{return _Type_ID_to;}            set{                SetProperty(ref _Type_ID_to, value);                }
        }

        private string _CNName_to;
        
        
        /// <summary>
        /// 目标品名
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName_to",ColDesc = "目标品名")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName_to" ,Length=255,IsNullable = true,ColumnDescription = "目标品名" )]
        [Display(Name = "目标品名")]
        public string CNName_to 
        { 
            get{return _CNName_to;}            set{                SetProperty(ref _CNName_to, value);                }
        }

        private string _Model_to;
        
        
        /// <summary>
        /// 目标型号
        /// </summary>

        [AdvQueryAttribute(ColName = "Model_to",ColDesc = "目标型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model_to" ,Length=50,IsNullable = true,ColumnDescription = "目标型号" )]
        [Display(Name = "目标型号")]
        public string Model_to 
        { 
            get{return _Model_to;}            set{                SetProperty(ref _Model_to, value);                }
        }

        private string _Specifications_to;
        
        
        /// <summary>
        /// 目标规格
        /// </summary>

        [AdvQueryAttribute(ColName = "Specifications_to",ColDesc = "目标规格")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications_to" ,Length=1000,IsNullable = true,ColumnDescription = "目标规格" )]
        [Display(Name = "目标规格")]
        public string Specifications_to 
        { 
            get{return _Specifications_to;}            set{                SetProperty(ref _Specifications_to, value);                }
        }

        private string _property_to;
        
        
        /// <summary>
        /// 目标属性
        /// </summary>

        [AdvQueryAttribute(ColName = "property_to",ColDesc = "目标属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property_to" ,Length=255,IsNullable = true,ColumnDescription = "目标属性" )]
        [Display(Name = "目标属性")]
        public string property_to 
        { 
            get{return _property_to;}            set{                SetProperty(ref _property_to, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
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
                    Type type = typeof(View_ProdConversionItems);
                    
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

