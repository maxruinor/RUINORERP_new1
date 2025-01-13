
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:10
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
    /// 组合单明细
    /// </summary>
    [Serializable()]
    [Description("tb_ProdMergeDetail")]
    [SugarTable("tb_ProdMergeDetail")]
    public partial class tb_ProdMergeDetail: BaseEntity, ICloneable
    {
        public tb_ProdMergeDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ProdMergeDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _MergeSub_ID;
        /// <summary>
        /// 明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MergeSub_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "明细" , IsPrimaryKey = true)]
        public long MergeSub_ID
        { 
            get{return _MergeSub_ID;}
            set{
            base.PrimaryKeyID = _MergeSub_ID;
            SetProperty(ref _MergeSub_ID, value);
            }
        }

        private long _MergeID;
        /// <summary>
        /// 组合单
        /// </summary>
        [AdvQueryAttribute(ColName = "MergeID",ColDesc = "组合单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MergeID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "组合单" )]
        [FKRelationAttribute("tb_ProdMerge","MergeID")]
        public long MergeID
        { 
            get{return _MergeID;}
            set{
            SetProperty(ref _MergeID, value);
            }
        }

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 子件
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "子件")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "子件" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
            }
        }

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
            }
        }

        private int _Qty= ((0));
        /// <summary>
        /// 子件数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "子件数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Qty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "子件数量" )]
        public int Qty
        { 
            get{return _Qty;}
            set{
            SetProperty(ref _Qty, value);
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
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(MergeID))]
        public virtual tb_ProdMerge tb_prodmerge { get; set; }



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
                    Type type = typeof(tb_ProdMergeDetail);
                    
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
            tb_ProdMergeDetail loctype = (tb_ProdMergeDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

