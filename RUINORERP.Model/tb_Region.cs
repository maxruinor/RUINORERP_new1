
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:47
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 地区表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Region")]
    public partial class tb_Region : BaseEntity, ICloneable
    {
        public tb_Region()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                //throw new Exception("tb_Region" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Region_ID;
        /// <summary>
        /// 地区
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Region_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "地区", IsPrimaryKey = true)]
        public long Region_ID
        {
            get { return _Region_ID; }
            set
            {
                base.PrimaryKeyID = _Region_ID;
                SetProperty(ref _Region_ID, value);
            }
        }

        private string _Region_Name;
        /// <summary>
        /// 地区名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Region_Name", ColDesc = "地区名称")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Region_Name", Length = 50, IsNullable = true, ColumnDescription = "地区名称")]
        public string Region_Name
        {
            get { return _Region_Name; }
            set
            {
                SetProperty(ref _Region_Name, value);
            }
        }

        private string _Region_code;
        /// <summary>
        /// 地区代码
        /// </summary>
        [AdvQueryAttribute(ColName = "Region_code", ColDesc = "地区代码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Region_code", Length = 20, IsNullable = true, ColumnDescription = "地区代码")]
        public string Region_code
        {
            get { return _Region_code; }
            set
            {
                SetProperty(ref _Region_code, value);
            }
        }

        private long? _Parent_region_id;
        /// <summary>
        ///  父地区
        /// </summary>
        [AdvQueryAttribute(ColName = "Parent_region_id", ColDesc = " 父地区")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Parent_region_id", DecimalDigits = 0, IsNullable = true, ColumnDescription = " 父地区")]
        [FKRelationAttribute("tb_Region", "Parent_region_id")]
        public long? Parent_region_id
        {
            get { return _Parent_region_id; }
            set
            {
                SetProperty(ref _Parent_region_id, value);
            }
        }

        private long? _Customer_id;
        /// <summary>
        /// 意向客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id", ColDesc = "意向客户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Customer_id", DecimalDigits = 0, IsNullable = true, ColumnDescription = "意向客户")]
        [FKRelationAttribute("tb_Customer", "Customer_id")]
        public long? Customer_id
        {
            get { return _Customer_id; }
            set
            {
                SetProperty(ref _Customer_id, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Parent_region_id))]
        public virtual tb_Region tb_region { get; set; }
        //public virtual tb_Region tb_Parent_region_id { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Customer_id))]
        public virtual tb_Customer tb_customer { get; set; }
        //public virtual tb_Customer tb_Customer_id { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Region.Parent_region_id))]
        public virtual List<tb_Region> tb_Regions { get; set; }
        //tb_Region.Region_ID)
        //Region_ID.FK_REGIO_REF_REGIO)
        //tb_Region.Parent_region_id)


        #endregion




        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
            if ("Region_ID" != "Parent_region_id")
            {
                rs = false;
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
                    Type type = typeof(tb_Region);

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
            tb_Region loctype = (tb_Region)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

