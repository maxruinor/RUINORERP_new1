
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:03
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
    /// 商品信息汇总
    /// </summary>
    [Serializable()]
    [Description("商品信息汇总")]
    [SugarTable("tb_ProdInfoSummary")]
    public partial class tb_ProdInfoSummary: BaseEntity, ICloneable
    {
        public tb_ProdInfoSummary()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("商品信息汇总tb_ProdInfoSummary" + "外键ID与对应主主键名称不一致。请修改数据库");
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

        private decimal? _平均价格;
        /// <summary>
        /// 平均价格
        /// </summary>
        [AdvQueryAttribute(ColName = "平均价格",ColDesc = "平均价格")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "平均价格" , DecimalDigits = 2,IsNullable = true,ColumnDescription = "平均价格" )]
        public decimal? 平均价格
        { 
            get{return _平均价格;}
            set{
            SetProperty(ref _平均价格, value);
            }
        }

        private int? _总销售量;
        /// <summary>
        /// 总销售量
        /// </summary>
        [AdvQueryAttribute(ColName = "总销售量",ColDesc = "总销售量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "总销售量" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "总销售量" )]
        public int? 总销售量
        { 
            get{return _总销售量;}
            set{
            SetProperty(ref _总销售量, value);
            }
        }

        private int? _库存总量;
        /// <summary>
        /// 库存总量
        /// </summary>
        [AdvQueryAttribute(ColName = "库存总量",ColDesc = "库存总量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "库存总量" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "库存总量" )]
        public int? 库存总量
        { 
            get{return _库存总量;}
            set{
            SetProperty(ref _库存总量, value);
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
                    Type type = typeof(tb_ProdInfoSummary);
                    
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
            tb_ProdInfoSummary loctype = (tb_ProdInfoSummary)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

