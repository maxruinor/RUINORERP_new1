
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/27/2024 15:11:27
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
    /// 
    /// </summary>
    [Serializable()]
    [SugarTable("Proc_InventoryTracking_Horizontal")]
    public class Proc_InventoryTracking_Horizontal:BaseEntity, ICloneable
    {
        public Proc_InventoryTracking_Horizontal()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("Proc_InventoryTracking_Horizontal" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long _库位;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "库位",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "库位" , DecimalDigits = 255,Length=8,IsNullable = false,ColumnDescription = "库位")]
        [Display(Name = "")]
        public long 库位 
        { 
            get{return _库位;}
            set{
                SetProperty(ref _库位, value);
                }
        }

        private long _ProdDetailID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 255,Length=8,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{
                SetProperty(ref _ProdDetailID, value);
                }
        }

        private int? _期初;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "期初",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "期初" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "期初")]
        [Display(Name = "")]
        public int? 期初 
        { 
            get{return _期初;}
            set{
                SetProperty(ref _期初, value);
                }
        }

        private int? _采购入库;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "采购入库",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "采购入库" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "采购入库")]
        [Display(Name = "")]
        public int? 采购入库 
        { 
            get{return _采购入库;}
            set{
                SetProperty(ref _采购入库, value);
                }
        }

        private int? _采购退回;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "采购退回",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "采购退回" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "采购退回")]
        [Display(Name = "")]
        public int? 采购退回 
        { 
            get{return _采购退回;}
            set{
                SetProperty(ref _采购退回, value);
                }
        }

        private int? _销售出库;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "销售出库",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "销售出库" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "销售出库")]
        [Display(Name = "")]
        public int? 销售出库 
        { 
            get{return _销售出库;}
            set{
                SetProperty(ref _销售出库, value);
                }
        }

        private int? _销售退回;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "销售退回",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "销售退回" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "销售退回")]
        [Display(Name = "")]
        public int? 销售退回 
        { 
            get{return _销售退回;}
            set{
                SetProperty(ref _销售退回, value);
                }
        }

        private int? _其他出库;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "其他出库",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "其他出库" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "其他出库")]
        [Display(Name = "")]
        public int? 其他出库 
        { 
            get{return _其他出库;}
            set{
                SetProperty(ref _其他出库, value);
                }
        }

        private int? _其他入库;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "其他入库",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "其他入库" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "其他入库")]
        [Display(Name = "")]
        public int? 其他入库 
        { 
            get{return _其他入库;}
            set{
                SetProperty(ref _其他入库, value);
                }
        }

        private int? _缴库;


        /// <summary>
        /// 缴库
        /// </summary>

        [AdvQueryAttribute(ColName = "缴库", ColDesc = "缴库")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "缴库", DecimalDigits = 255, Length = 4, IsNullable = true, ColumnDescription = "缴库")]
        [Display(Name = "")]
        public int? 缴库
        {
            get { return _缴库; }
            set
            {
                SetProperty(ref _缴库, value);
            }
        }


        private int? _库存盘点;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "库存盘点",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "库存盘点" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "库存盘点")]
        [Display(Name = "")]
        public int? 库存盘点 
        { 
            get{return _库存盘点;}
            set{
                SetProperty(ref _库存盘点, value);
                }
        }

        private int? _期末库存;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "期末库存",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "期末库存" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "期末库存")]
        [Display(Name = "")]
        public int? 期末库存 
        { 
            get{return _期末库存;}
            set{
                SetProperty(ref _期末库存, value);
                }
        }







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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(Proc_InventoryTracking_Horizontal);
                    
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
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

