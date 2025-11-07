
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/07/2025 10:19:28
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容
    /// </summary>
    [Serializable()]
    [Description("货物类型  成品  半成品  包装材料 下脚料这种内容")]
    [SugarTable("tb_ProductType")]
    public partial class tb_ProductType: BaseEntity, ICloneable
    {
        public tb_ProductType()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("货物类型  成品  半成品  包装材料 下脚料这种内容tb_ProductType" + "外键ID与对应主主键名称不一致。请修改数据库");
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

        private bool _ForSale;
        /// <summary>
        /// 待销类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ForSale",ColDesc = "待销类型")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ForSale" ,IsNullable = false,ColumnDescription = "待销类型" )]
        public bool ForSale
        { 
            get{return _ForSale;}
            set{
            SetProperty(ref _ForSale, value);
                        }
        }

        #endregion

        #region 扩展属性


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.Type_ID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.Type_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdConversionDetail.Type_ID_from))]
        public virtual List<tb_ProdConversionDetail> tb_ProdConversionDetails { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdConversionDetail.Type_ID_to))]
        public virtual List<tb_ProdConversionDetail> tb_ProdConversionDetailsByTypeIdTo { get; set; }


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ProductType loctype = (tb_ProductType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

