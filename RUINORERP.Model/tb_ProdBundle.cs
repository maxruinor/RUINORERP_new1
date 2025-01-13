
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:06
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
    /// 产品套装表
    /// </summary>
    [Serializable()]
    [Description("tb_ProdBundle")]
    [SugarTable("tb_ProdBundle")]
    public partial class tb_ProdBundle: BaseEntity, ICloneable
    {
        public tb_ProdBundle()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ProdBundle" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BundleID;
        /// <summary>
        /// 套装组合
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BundleID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "套装组合" , IsPrimaryKey = true)]
        public long BundleID
        { 
            get{return _BundleID;}
            set{
            base.PrimaryKeyID = _BundleID;
            SetProperty(ref _BundleID, value);
            }
        }

        private string _BundleName;
        /// <summary>
        /// 套装名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BundleName",ColDesc = "套装名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BundleName" ,Length=255,IsNullable = false,ColumnDescription = "套装名称" )]
        public string BundleName
        { 
            get{return _BundleName;}
            set{
            SetProperty(ref _BundleName, value);
            }
        }

        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=255,IsNullable = true,ColumnDescription = "描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
            }
        }

        private long _Unit_ID;
        /// <summary>
        /// 套装单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "套装单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "套装单位" )]
        [FKRelationAttribute("tb_Unit","Unit_ID")]
        public long Unit_ID
        { 
            get{return _Unit_ID;}
            set{
            SetProperty(ref _Unit_ID, value);
            }
        }

        private string _ImagesPath;
        /// <summary>
        /// 产品图片
        /// </summary>
        [AdvQueryAttribute(ColName = "ImagesPath",ColDesc = "产品图片")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ImagesPath" ,Length=2000,IsNullable = true,ColumnDescription = "产品图片" )]
        public string ImagesPath
        { 
            get{return _ImagesPath;}
            set{
            SetProperty(ref _ImagesPath, value);
            }
        }

        private byte[] _BundleImage;
        /// <summary>
        /// 产品图片
        /// </summary>
        [AdvQueryAttribute(ColName = "BundleImage",ColDesc = "产品图片")] 
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "BundleImage" ,Length=2147483647,IsNullable = true,ColumnDescription = "产品图片" )]
        public byte[] BundleImage
        { 
            get{return _BundleImage;}
            set{
            SetProperty(ref _BundleImage, value);
            }
        }

        private decimal? _Weight;
        /// <summary>
        /// 重量（千克）
        /// </summary>
        [AdvQueryAttribute(ColName = "Weight",ColDesc = "重量（千克）")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Weight" , DecimalDigits = 3,IsNullable = true,ColumnDescription = "重量（千克）" )]
        public decimal? Weight
        { 
            get{return _Weight;}
            set{
            SetProperty(ref _Weight, value);
            }
        }

        private decimal? _Market_Price;
        /// <summary>
        /// 市场零售价
        /// </summary>
        [AdvQueryAttribute(ColName = "Market_Price",ColDesc = "市场零售价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Market_Price" , DecimalDigits = 6,IsNullable = true,ColumnDescription = "市场零售价" )]
        public decimal? Market_Price
        { 
            get{return _Market_Price;}
            set{
            SetProperty(ref _Market_Price, value);
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

        private bool _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
            }
        }

        private bool _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_available" ,IsNullable = false,ColumnDescription = "是否可用" )]
        public bool Is_available
        { 
            get{return _Is_available;}
            set{
            SetProperty(ref _Is_available, value);
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

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=500,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions
        { 
            get{return _ApprovalOpinions;}
            set{
            SetProperty(ref _ApprovalOpinions, value);
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

        private int? _ApprovalStatus= ((0));
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Unit_ID))]
        public virtual tb_Unit tb_unit { get; set; }


        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Packing.BundleID))]
        public virtual List<tb_Packing> tb_Packings { get; set; }
        //tb_Packing.BundleID)
        //BundleID.FK_PACKI_REF_TB_PRODBundle)
        //tb_ProdBundle.BundleID)

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdBundleDetail.BundleID))]
        public virtual List<tb_ProdBundleDetail> tb_ProdBundleDetails { get; set; }
        //tb_ProdBundleDetail.BundleID)
        //BundleID.FK_PRODBUNDLEDETAIL_REF_PRODBUNDLE)
        //tb_ProdBundle.BundleID)


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
                    Type type = typeof(tb_ProdBundle);
                    
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
            tb_ProdBundle loctype = (tb_ProdBundle)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

