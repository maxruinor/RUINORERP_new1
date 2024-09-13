
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:34
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
    /// 往来单位类型,如级别，电商，大客户，亚马逊等
    /// </summary>
    [Serializable()]
    [Description("tb_CustomerVendorType")]
    [SugarTable("tb_CustomerVendorType")]
    public partial class tb_CustomerVendorType: BaseEntity, ICloneable
    {
        public tb_CustomerVendorType()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_CustomerVendorType" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Type_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Type_ID
        { 
            get{return _Type_ID;}
            set{
            base.PrimaryKeyID = _Type_ID;
            SetProperty(ref _Type_ID, value);
            }
        }

        private string _TypeName;
        /// <summary>
        /// 类型等级名称
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeName",ColDesc = "类型等级名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TypeName" ,Length=50,IsNullable = true,ColumnDescription = "类型等级名称" )]
        public string TypeName
        { 
            get{return _TypeName;}
            set{
            SetProperty(ref _TypeName, value);
            }
        }

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Desc" ,Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc
        { 
            get{return _Desc;}
            set{
            SetProperty(ref _Desc, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CustomerVendor.Type_ID))]
        public virtual List<tb_CustomerVendor> tb_CustomerVendors { get; set; }
        //tb_CustomerVendor.Type_ID)
        //Type_ID.FK_TB_CUSTOM_1_TB_CUSTOM)
        //tb_CustomerVendorType.Type_ID)


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
                    Type type = typeof(tb_CustomerVendorType);
                    
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
            tb_CustomerVendorType loctype = (tb_CustomerVendorType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

