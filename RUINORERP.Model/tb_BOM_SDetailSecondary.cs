
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:52
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
    /// 标准物料表次级产出明细
    /// </summary>
    [Serializable()]
    [Description("标准物料表次级产出明细")]
    [SugarTable("tb_BOM_SDetailSecondary")]
    public partial class tb_BOM_SDetailSecondary: BaseEntity, ICloneable
    {
        public tb_BOM_SDetailSecondary()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("标准物料表次级产出明细tb_BOM_SDetailSecondary" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SecID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SecID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long SecID
        { 
            get{return _SecID;}
            set{
            SetProperty(ref _SecID, value);
                base.PrimaryKeyID = _SecID;
            }
        }

        private long? _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "产品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long? ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private long? _BOM_ID;
        /// <summary>
        /// BOM
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "BOM")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "BOM" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long? BOM_ID
        { 
            get{return _BOM_ID;}
            set{
            SetProperty(ref _BOM_ID, value);
                        }
        }

        private decimal _Qty= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Qty" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "数量" )]
        public decimal Qty
        { 
            get{return _Qty;}
            set{
            SetProperty(ref _Qty, value);
                        }
        }

        private decimal _Scale= ((0));
        /// <summary>
        /// 比例
        /// </summary>
        [AdvQueryAttribute(ColName = "Scale",ColDesc = "比例")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Scale" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "比例" )]
        public decimal Scale
        { 
            get{return _Scale;}
            set{
            SetProperty(ref _Scale, value);
                        }
        }

        private string _property;
        /// <summary>
        /// 副产品属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "副产品属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "副产品属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
                        }
        }

        private decimal _UnitCost= ((0));
        /// <summary>
        /// 单位成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "单位成本")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位成本" )]
        public decimal UnitCost
        { 
            get{return _UnitCost;}
            set{
            SetProperty(ref _UnitCost, value);
                        }
        }

        private decimal _SubtotalCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCost",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCost
        { 
            get{return _SubtotalCost;}
            set{
            SetProperty(ref _SubtotalCost, value);
                        }
        }

        private string _Remarks;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Remarks",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Remarks" ,Length=200,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Remarks
        { 
            get{return _Remarks;}
            set{
            SetProperty(ref _Remarks, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}







        public override object Clone()
        {
            tb_BOM_SDetailSecondary loctype = (tb_BOM_SDetailSecondary)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

