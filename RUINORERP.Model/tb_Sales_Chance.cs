
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:33
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
    /// 销售机会
    /// </summary>
    [Serializable()]
    [Description("tb_Sales_Chance")]
    [SugarTable("tb_Sales_Chance")]
    public partial class tb_Sales_Chance: BaseEntity, ICloneable
    {
        public tb_Sales_Chance()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Sales_Chance" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _opportunity_id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "opportunity_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long opportunity_id
        { 
            get{return _opportunity_id;}
            set{
            base.PrimaryKeyID = _opportunity_id;
            SetProperty(ref _opportunity_id, value);
            }
        }

        private long? _Customer_id;
        /// <summary>
        /// 意向客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "意向客户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Customer_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "意向客户" )]
        [FKRelationAttribute("tb_Customer","Customer_id")]
        public long? Customer_id
        { 
            get{return _Customer_id;}
            set{
            SetProperty(ref _Customer_id, value);
            }
        }

        private string _opportunity_name;
        /// <summary>
        /// 机会名称
        /// </summary>
        [AdvQueryAttribute(ColName = "opportunity_name",ColDesc = "机会名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "opportunity_name" ,Length=50,IsNullable = true,ColumnDescription = "机会名称" )]
        public string opportunity_name
        { 
            get{return _opportunity_name;}
            set{
            SetProperty(ref _opportunity_name, value);
            }
        }

        private string _opportunity_amount;
        /// <summary>
        /// 机会金额
        /// </summary>
        [AdvQueryAttribute(ColName = "opportunity_amount",ColDesc = "机会金额")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "opportunity_amount" ,Length=100,IsNullable = true,ColumnDescription = "机会金额" )]
        public string opportunity_amount
        { 
            get{return _opportunity_amount;}
            set{
            SetProperty(ref _opportunity_amount, value);
            }
        }

        private string _opportunity_stage;
        /// <summary>
        /// 机会阶段
        /// </summary>
        [AdvQueryAttribute(ColName = "opportunity_stage",ColDesc = "机会阶段")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "opportunity_stage" ,Length=200,IsNullable = true,ColumnDescription = "机会阶段" )]
        public string opportunity_stage
        { 
            get{return _opportunity_stage;}
            set{
            SetProperty(ref _opportunity_stage, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Customer_id))]
        public virtual tb_Customer tb_customer { get; set; }



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
                    Type type = typeof(tb_Sales_Chance);
                    
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
            tb_Sales_Chance loctype = (tb_Sales_Chance)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

