
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:06
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
    /// 省份表
    /// </summary>
    [Serializable()]
    [Description("省份表")]
    [SugarTable("tb_Provinces")]
    public partial class tb_Provinces: BaseEntity, ICloneable
    {
        public tb_Provinces()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("省份表tb_Provinces" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ProvinceID;
        /// <summary>
        /// 省
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProvinceID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "省" , IsPrimaryKey = true)]
        public long ProvinceID
        { 
            get{return _ProvinceID;}
            set{
            base.PrimaryKeyID = _ProvinceID;
            SetProperty(ref _ProvinceID, value);
            }
        }

        private string _ProvinceCNName;
        /// <summary>
        /// 省份中文名
        /// </summary>
        [AdvQueryAttribute(ColName = "ProvinceCNName",ColDesc = "省份中文名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProvinceCNName" ,Length=80,IsNullable = true,ColumnDescription = "省份中文名" )]
        public string ProvinceCNName
        { 
            get{return _ProvinceCNName;}
            set{
            SetProperty(ref _ProvinceCNName, value);
            }
        }

        private long? _CountryID;
        /// <summary>
        /// 国家
        /// </summary>
        [AdvQueryAttribute(ColName = "CountryID",ColDesc = "国家")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CountryID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "国家" )]
        public long? CountryID
        { 
            get{return _CountryID;}
            set{
            SetProperty(ref _CountryID, value);
            }
        }

        private string _ProvinceENName;
        /// <summary>
        /// 省份英文名
        /// </summary>
        [AdvQueryAttribute(ColName = "ProvinceENName",ColDesc = "省份英文名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProvinceENName" ,Length=80,IsNullable = true,ColumnDescription = "省份英文名" )]
        public string ProvinceENName
        { 
            get{return _ProvinceENName;}
            set{
            SetProperty(ref _ProvinceENName, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Cities.ProvinceID))]
        public virtual List<tb_Cities> tb_Citieses { get; set; }
        //tb_Cities.ProvinceID)
        //ProvinceID.FK_TB_CITIE_REFERENCE_TB_PROVI)
        //tb_Provinces.ProvinceID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_Customer.ProvinceID))]
        public virtual List<tb_CRM_Customer> tb_CRM_Customers { get; set; }
        //tb_CRM_Customer.ProvinceID)
        //ProvinceID.FK_TB_CRM_C_REFERENCE_TB_PROVI)
        //tb_Provinces.ProvinceID)


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
                    Type type = typeof(tb_Provinces);
                    
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
            tb_Provinces loctype = (tb_Provinces)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

