
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:13
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
    /// 产品转换单明细
    /// </summary>
    [Serializable()]
    [Description("产品转换单明细")]
    [SugarTable("tb_ProdConversionDetail")]
    public partial class tb_ProdConversionDetail: BaseEntity, ICloneable
    {
        public tb_ProdConversionDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("产品转换单明细tb_ProdConversionDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ConversionID;
        /// <summary>
        /// 组合单
        /// </summary>
        [AdvQueryAttribute(ColName = "ConversionID",ColDesc = "组合单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ConversionID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "组合单" )]
        [FKRelationAttribute("tb_ProdConversion","ConversionID")]
        public long ConversionID
        { 
            get{return _ConversionID;}
            set{
            SetProperty(ref _ConversionID, value);
                        }
        }

        private long _ConversionSub_ID;
        /// <summary>
        /// 明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ConversionSub_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "明细" , IsPrimaryKey = true)]
        public long ConversionSub_ID
        { 
            get{return _ConversionSub_ID;}
            set{
            SetProperty(ref _ConversionSub_ID, value);
                base.PrimaryKeyID = _ConversionSub_ID;
            }
        }

        private long _ProdDetailID_from;
        /// <summary>
        /// 来源产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID_from",ColDesc = "来源产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID_from" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "来源产品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID_from")]
        public long ProdDetailID_from
        { 
            get{return _ProdDetailID_from;}
            set{
            SetProperty(ref _ProdDetailID_from, value);
                        }
        }

        private string _BarCode_from;
        /// <summary>
        /// 来源条码
        /// </summary>
        [AdvQueryAttribute(ColName = "BarCode_from",ColDesc = "来源条码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BarCode_from" ,Length=255,IsNullable = true,ColumnDescription = "来源条码" )]
        public string BarCode_from
        { 
            get{return _BarCode_from;}
            set{
            SetProperty(ref _BarCode_from, value);
                        }
        }

        private string _SKU_from;
        /// <summary>
        /// 来源SKU
        /// </summary>
        [AdvQueryAttribute(ColName = "SKU_from",ColDesc = "来源SKU")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU_from" ,Length=255,IsNullable = true,ColumnDescription = "来源SKU" )]
        public string SKU_from
        { 
            get{return _SKU_from;}
            set{
            SetProperty(ref _SKU_from, value);
                        }
        }

        private long _Type_ID_from;
        /// <summary>
        /// 来源产品类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Type_ID_from",ColDesc = "来源产品类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID_from" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "来源产品类型" )]
        [FKRelationAttribute("tb_ProductType","Type_ID_from")]
        public long Type_ID_from
        { 
            get{return _Type_ID_from;}
            set{
            SetProperty(ref _Type_ID_from, value);
                        }
        }

        private string _CNName_from;
        /// <summary>
        /// 来源品名
        /// </summary>
        [AdvQueryAttribute(ColName = "CNName_from",ColDesc = "来源品名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName_from" ,Length=255,IsNullable = false,ColumnDescription = "来源品名" )]
        public string CNName_from
        { 
            get{return _CNName_from;}
            set{
            SetProperty(ref _CNName_from, value);
                        }
        }

        private string _Model_from;
        /// <summary>
        /// 来源型号
        /// </summary>
        [AdvQueryAttribute(ColName = "Model_from",ColDesc = "来源型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model_from" ,Length=50,IsNullable = true,ColumnDescription = "来源型号" )]
        public string Model_from
        { 
            get{return _Model_from;}
            set{
            SetProperty(ref _Model_from, value);
                        }
        }

        private string _Specifications_from;
        /// <summary>
        /// 来源规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specifications_from",ColDesc = "来源规格")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications_from" ,Length=1000,IsNullable = true,ColumnDescription = "来源规格" )]
        public string Specifications_from
        { 
            get{return _Specifications_from;}
            set{
            SetProperty(ref _Specifications_from, value);
                        }
        }

        private string _property_from;
        /// <summary>
        /// 来源属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property_from",ColDesc = "来源属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property_from" ,Length=255,IsNullable = true,ColumnDescription = "来源属性" )]
        public string property_from
        { 
            get{return _property_from;}
            set{
            SetProperty(ref _property_from, value);
                        }
        }

        private int _ConversionQty= ((0));
        /// <summary>
        /// 转换数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ConversionQty",ColDesc = "转换数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ConversionQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "转换数量" )]
        public int ConversionQty
        { 
            get{return _ConversionQty;}
            set{
            SetProperty(ref _ConversionQty, value);
                        }
        }

        private long _ProdDetailID_to;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID_to",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID_to" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID_to")]
        public long ProdDetailID_to
        { 
            get{return _ProdDetailID_to;}
            set{
            SetProperty(ref _ProdDetailID_to, value);
                        }
        }

        private string _BarCode_to;
        /// <summary>
        /// 目标条码
        /// </summary>
        [AdvQueryAttribute(ColName = "BarCode_to",ColDesc = "目标条码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BarCode_to" ,Length=255,IsNullable = true,ColumnDescription = "目标条码" )]
        public string BarCode_to
        { 
            get{return _BarCode_to;}
            set{
            SetProperty(ref _BarCode_to, value);
                        }
        }

        private string _SKU_to;
        /// <summary>
        /// 目标SKU
        /// </summary>
        [AdvQueryAttribute(ColName = "SKU_to",ColDesc = "目标SKU")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU_to" ,Length=255,IsNullable = true,ColumnDescription = "目标SKU" )]
        public string SKU_to
        { 
            get{return _SKU_to;}
            set{
            SetProperty(ref _SKU_to, value);
                        }
        }

        private long _Type_ID_to;
        /// <summary>
        /// 目标产品类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Type_ID_to",ColDesc = "目标产品类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID_to" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "目标产品类型" )]
        [FKRelationAttribute("tb_ProductType","Type_ID_to")]
        public long Type_ID_to
        { 
            get{return _Type_ID_to;}
            set{
            SetProperty(ref _Type_ID_to, value);
                        }
        }

        private string _CNName_to;
        /// <summary>
        /// 目标品名
        /// </summary>
        [AdvQueryAttribute(ColName = "CNName_to",ColDesc = "目标品名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName_to" ,Length=255,IsNullable = false,ColumnDescription = "目标品名" )]
        public string CNName_to
        { 
            get{return _CNName_to;}
            set{
            SetProperty(ref _CNName_to, value);
                        }
        }

        private string _Model_to;
        /// <summary>
        /// 目标型号
        /// </summary>
        [AdvQueryAttribute(ColName = "Model_to",ColDesc = "目标型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model_to" ,Length=50,IsNullable = true,ColumnDescription = "目标型号" )]
        public string Model_to
        { 
            get{return _Model_to;}
            set{
            SetProperty(ref _Model_to, value);
                        }
        }

        private string _Specifications_to;
        /// <summary>
        /// 目标规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specifications_to",ColDesc = "目标规格")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications_to" ,Length=1000,IsNullable = true,ColumnDescription = "目标规格" )]
        public string Specifications_to
        { 
            get{return _Specifications_to;}
            set{
            SetProperty(ref _Specifications_to, value);
                        }
        }

        private string _property_to;
        /// <summary>
        /// 目标属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property_to",ColDesc = "目标属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property_to" ,Length=255,IsNullable = true,ColumnDescription = "目标属性" )]
        public string property_to
        { 
            get{return _property_to;}
            set{
            SetProperty(ref _property_to, value);
                        }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID_from))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ConversionID))]
        public virtual tb_ProdConversion tb_prodconversion { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID_to))]
        public virtual tb_ProdDetail tb_proddetail_to { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Type_ID_from))]
        public virtual tb_ProductType tb_producttype_from { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Type_ID_to))]
        public virtual tb_ProductType tb_producttype_to { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("ProdDetailID"!="ProdDetailID_from")
        {
        // rs=false;
        }
         if("ProdDetailID"!="ProdDetailID_to")
        {
        // rs=false;
        }
         if("Type_ID"!="Type_ID_from")
        {
        // rs=false;
        }
         if("Type_ID"!="Type_ID_to")
        {
        // rs=false;
        }
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
                    Type type = typeof(tb_ProdConversionDetail);
                    
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
            tb_ProdConversionDetail loctype = (tb_ProdConversionDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

