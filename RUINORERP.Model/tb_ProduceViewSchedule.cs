
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:14
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
    /// 可视化排程
    /// </summary>
    [Serializable()]
    [Description("tb_ProduceViewSchedule")]
    [SugarTable("tb_ProduceViewSchedule")]
    public partial class tb_ProduceViewSchedule: BaseEntity, ICloneable
    {
        public tb_ProduceViewSchedule()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ProduceViewSchedule" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long id
        { 
            get{return _id;}
            set{
            base.PrimaryKeyID = _id;
            SetProperty(ref _id, value);
            }
        }

        private int? _product_id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "product_id",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "product_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public int? product_id
        { 
            get{return _product_id;}
            set{
            SetProperty(ref _product_id, value);
            }
        }

        private int? _quantity;
        /// <summary>
        /// 生产数量
        /// </summary>
        [AdvQueryAttribute(ColName = "quantity",ColDesc = "生产数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "quantity" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "生产数量" )]
        public int? quantity
        { 
            get{return _quantity;}
            set{
            SetProperty(ref _quantity, value);
            }
        }

        private DateTime? _start_date;
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [AdvQueryAttribute(ColName = "start_date",ColDesc = "计划开始日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "start_date" ,IsNullable = true,ColumnDescription = "计划开始日期" )]
        public DateTime? start_date
        { 
            get{return _start_date;}
            set{
            SetProperty(ref _start_date, value);
            }
        }

        private DateTime? _end_date;
        /// <summary>
        /// 计划完成日期
        /// </summary>
        [AdvQueryAttribute(ColName = "end_date",ColDesc = "计划完成日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "end_date" ,IsNullable = true,ColumnDescription = "计划完成日期" )]
        public DateTime? end_date
        { 
            get{return _end_date;}
            set{
            SetProperty(ref _end_date, value);
            }
        }

        #endregion

        #region 扩展属性


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
                    Type type = typeof(tb_ProduceViewSchedule);
                    
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
            tb_ProduceViewSchedule loctype = (tb_ProduceViewSchedule)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

