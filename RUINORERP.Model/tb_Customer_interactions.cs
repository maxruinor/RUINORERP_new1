
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/13/2023 17:33:43
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
    /// 客户交互表，CRM系统中使用      
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Customer_interactions")]
    public partial class tb_Customer_interactions: BaseEntity, ICloneable
    {
        public tb_Customer_interactions()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Customer_interactions" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _interaction_id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "interaction_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long interaction_id
        { 
            get{return _interaction_id;}
            set{
            base.PrimaryKeyID = _interaction_id;
            SetProperty(ref _interaction_id, value);
            }
        }

        private long? _Customer_id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Customer_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Customer","Customer_id")]
        public long? Customer_id
        { 
            get{return _Customer_id;}
            set{
            SetProperty(ref _Customer_id, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private string _interaction_date;
        /// <summary>
        /// 交互日期
        /// </summary>
        [AdvQueryAttribute(ColName = "interaction_date",ColDesc = "交互日期")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "interaction_date" ,Length=50,IsNullable = true,ColumnDescription = "交互日期" )]
        public string interaction_date
        { 
            get{return _interaction_date;}
            set{
            SetProperty(ref _interaction_date, value);
            }
        }

        private string _interaction_type;
        /// <summary>
        /// 交互类型
        /// </summary>
        [AdvQueryAttribute(ColName = "interaction_type",ColDesc = "交互类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "interaction_type" ,Length=100,IsNullable = true,ColumnDescription = "交互类型" )]
        public string interaction_type
        { 
            get{return _interaction_type;}
            set{
            SetProperty(ref _interaction_type, value);
            }
        }

        private string _interaction_detail;
        /// <summary>
        /// 交互详情
        /// </summary>
        [AdvQueryAttribute(ColName = "interaction_detail",ColDesc = "交互详情")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "interaction_detail" ,Length=2147483647,IsNullable = true,ColumnDescription = "交互详情" )]
        public string interaction_detail
        { 
            get{return _interaction_detail;}
            set{
            SetProperty(ref _interaction_detail, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_Employee { get; set; }
        //public virtual tb_Employee tb_Employee_ID { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Customer_id))]
        public virtual tb_Customer tb_Customer { get; set; }
        //public virtual tb_Customer tb_Customer_id { get; set; }



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
                    Type type = typeof(tb_Customer_interactions);
                    
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
            tb_Customer_interactions loctype = (tb_Customer_interactions)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

