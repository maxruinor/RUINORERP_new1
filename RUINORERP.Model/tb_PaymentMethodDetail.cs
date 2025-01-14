
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:00
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
    /// 交易方式设定，后面扩展有关账期 账龄分析的字段,暂时保存一个主子关系方便后面扩展
    /// </summary>
    [Serializable()]
    [Description("交易方式设定，后面扩展有关账期 账龄分析的字段,暂时保存一个主子关系方便后面扩展")]
    [SugarTable("tb_PaymentMethodDetail")]
    public partial class tb_PaymentMethodDetail: BaseEntity, ICloneable
    {
        public tb_PaymentMethodDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("交易方式设定，后面扩展有关账期 账龄分析的字段,暂时保存一个主子关系方便后面扩展tb_PaymentMethodDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Paytype_DetailID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_DetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Paytype_DetailID
        { 
            get{return _Paytype_DetailID;}
            set{
            base.PrimaryKeyID = _Paytype_DetailID;
            SetProperty(ref _Paytype_DetailID, value);
            }
        }

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Desc" ,Length=50,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc
        { 
            get{return _Desc;}
            set{
            SetProperty(ref _Desc, value);
            }
        }

        private bool? _Cash;
        /// <summary>
        /// 现金
        /// </summary>
        [AdvQueryAttribute(ColName = "Cash",ColDesc = "现金")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Cash" ,IsNullable = true,ColumnDescription = "现金" )]
        public bool? Cash
        { 
            get{return _Cash;}
            set{
            SetProperty(ref _Cash, value);
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
                    Type type = typeof(tb_PaymentMethodDetail);
                    
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
            tb_PaymentMethodDetail loctype = (tb_PaymentMethodDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

