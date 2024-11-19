
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:40
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
    /// 退料统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_MaterialReturnItems")]
    public class View_MaterialReturnItems:BaseEntity, ICloneable
    {
        public View_MaterialReturnItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_MaterialReturnItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private string _BillNo;
        
        
        /// <summary>
        /// 退料单号
        /// </summary>

        [AdvQueryAttribute(ColName = "BillNo",ColDesc = "退料单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BillNo" ,Length=50,IsNullable = true,ColumnDescription = "退料单号" )]
        [Display(Name = "退料单号")]
        public string BillNo 
        { 
            get{return _BillNo;}            set{                SetProperty(ref _BillNo, value);                }
        }

        private int? _BillType;
        
        
        /// <summary>
        /// 单据类别
        /// </summary>

        [AdvQueryAttribute(ColName = "BillType",ColDesc = "单据类别")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BillType" ,IsNullable = true,ColumnDescription = "单据类别" )]
        [Display(Name = "单据类别")]
        public int? BillType 
        { 
            get{return _BillType;}            set{                SetProperty(ref _BillType, value);                }
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

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 业务员
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "业务员" )]
        [Display(Name = "业务员")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 生产部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "生产部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "生产部门" )]
        [Display(Name = "生产部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private bool? _Outgoing;
        
        
        /// <summary>
        /// 外发加工
        /// </summary>

        [AdvQueryAttribute(ColName = "Outgoing",ColDesc = "外发加工")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Outgoing" ,IsNullable = true,ColumnDescription = "外发加工" )]
        [Display(Name = "外发加工")]
        public bool? Outgoing 
        { 
            get{return _Outgoing;}            set{                SetProperty(ref _Outgoing, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 退货客户
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "退货客户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "退货客户" )]
        [Display(Name = "退货客户")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }
      
        


        private DateTime? _ReturnDate;
        
        
        /// <summary>
        /// 退货日期
        /// </summary>

        [AdvQueryAttribute(ColName = "ReturnDate",ColDesc = "退货日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ReturnDate" ,IsNullable = true,ColumnDescription = "退货日期" )]
        [Display(Name = "退货日期")]
        public DateTime? ReturnDate 
        { 
            get{return _ReturnDate;}            set{                SetProperty(ref _ReturnDate, value);                }
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

        private string _Notes;
        
        
        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        [Display(Name = "备注")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }

 

        private string _MaterialRequisitionNO;
        
        
        /// <summary>
        /// 领料单号
        /// </summary>

        [AdvQueryAttribute(ColName = "MaterialRequisitionNO",ColDesc = "领料单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MaterialRequisitionNO" ,Length=50,IsNullable = true,ColumnDescription = "领料单号" )]
        [Display(Name = "领料单号")]
        public string MaterialRequisitionNO 
        { 
            get{return _MaterialRequisitionNO;}            set{                SetProperty(ref _MaterialRequisitionNO, value);                }
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
            get{return _isdeleted;}            set{                SetProperty(ref _isdeleted, value);                }
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

        private int? _Quantity;
        
        
        /// <summary>
        /// 退回数量
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "退回数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" ,IsNullable = true,ColumnDescription = "退回数量" )]
        [Display(Name = "退回数量")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
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

        private decimal? _Price;
        
        
        /// <summary>
        /// 单价
        /// </summary>

        [AdvQueryAttribute(ColName = "Price",ColDesc = "单价")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Price" , DecimalDigits = 10,Length=10,IsNullable = true,ColumnDescription = "单价" )]
        [Display(Name = "单价")]
        public decimal? Price 
        { 
            get{return _Price;}            set{                SetProperty(ref _Price, value);                }
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

        private string _ProductNo;
        
        
        /// <summary>
        /// 品号
        /// </summary>

        [AdvQueryAttribute(ColName = "ProductNo",ColDesc = "品号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProductNo" ,Length=40,IsNullable = true,ColumnDescription = "品号" )]
        [Display(Name = "品号")]
        public string ProductNo 
        { 
            get{return _ProductNo;}            set{                SetProperty(ref _ProductNo, value);                }
        }

        private long? _Unit_ID;
        
        
        /// <summary>
        /// 单位
        /// </summary>

        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" ,IsNullable = true,ColumnDescription = "单位" )]
        [Display(Name = "单位")]
        public long? Unit_ID 
        { 
            get{return _Unit_ID;}            set{                SetProperty(ref _Unit_ID, value);                }
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

        private long? _Category_ID;
        
        
        /// <summary>
        /// 类别
        /// </summary>

        [AdvQueryAttribute(ColName = "Category_ID",ColDesc = "类别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Category_ID" ,IsNullable = true,ColumnDescription = "类别" )]
        [Display(Name = "类别")]
        public long? Category_ID 
        { 
            get{return _Category_ID;}            set{                SetProperty(ref _Category_ID, value);                }
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
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
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
                    Type type = typeof(View_MaterialReturnItems);
                    
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

