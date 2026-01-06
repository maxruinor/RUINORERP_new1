
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:00
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
    /// 出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？
    /// </summary>
    [Serializable()]
    [Description("出入库类型")]
    [SugarTable("tb_OutInStockType")]
    public partial class tb_OutInStockType: BaseEntity, ICloneable
    {
        public tb_OutInStockType()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？tb_OutInStockType" + "外键ID与对应主主键名称不一致。请修改数据库");
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
            SetProperty(ref _Type_ID, value);
                base.PrimaryKeyID = _Type_ID;
            }
        }

        private string _TypeName;
        /// <summary>
        /// 类型名称
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeName",ColDesc = "类型名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TypeName" ,Length=50,IsNullable = false,ColumnDescription = "类型名称" )]
        public string TypeName
        { 
            get{return _TypeName;}
            set{
            SetProperty(ref _TypeName, value);
                        }
        }

        private string _TypeDesc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeDesc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TypeDesc" ,Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string TypeDesc
        { 
            get{return _TypeDesc;}
            set{
            SetProperty(ref _TypeDesc, value);
                        }
        }

        private bool _OutIn= false;
        /// <summary>
        /// 出入类型
        /// </summary>
        [AdvQueryAttribute(ColName = "OutIn",ColDesc = "出入类型")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "OutIn" ,IsNullable = false,ColumnDescription = "出入类型" )]
        public bool OutIn
        { 
            get{return _OutIn;}
            set{
            SetProperty(ref _OutIn, value);
                        }
        }

        private bool _Is_enabled= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = false,ColumnDescription = "是否可用" )]
        public bool Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockOut.Type_ID))]
        public virtual List<tb_StockOut> tb_StockOuts { get; set; }
        //tb_StockOut.Type_ID)
        //Type_ID.FK_TB_STOCKOUT_REF_TB_OUTIN_TYPE)
        //tb_OutInStockType.Type_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockIn.Type_ID))]
        public virtual List<tb_StockIn> tb_StockIns { get; set; }
        //tb_StockIn.Type_ID)
        //Type_ID.FK_STOCKIN1_REF_OUTINSTOCKType)
        //tb_OutInStockType.Type_ID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_OutInStockType loctype = (tb_OutInStockType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

