
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/07/2025 12:00:55
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
    /// 产品主次及属性关系表
    /// </summary>
    [Serializable()]
    [Description("产品主次及属性关系表")]
    [SugarTable("tb_Prod_Attr_Relation")]
    public partial class tb_Prod_Attr_Relation: BaseEntity, ICloneable
    {
        public tb_Prod_Attr_Relation()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("产品主次及属性关系表tb_Prod_Attr_Relation" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RAR_ID;
        /// <summary>
        /// 属性关系
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RAR_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "属性关系" , IsPrimaryKey = true)]
        public long RAR_ID
        { 
            get{return _RAR_ID;}
            set{
            SetProperty(ref _RAR_ID, value);
                base.PrimaryKeyID = _RAR_ID;
            }
        }

        private long? _PropertyValueID;
        /// <summary>
        /// 属性值
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyValueID",ColDesc = "属性值")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PropertyValueID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "属性值" )]
        [FKRelationAttribute("tb_ProdPropertyValue","PropertyValueID")]
        public long? PropertyValueID
        { 
            get{return _PropertyValueID;}
            set{
            SetProperty(ref _PropertyValueID, value);
                        }
        }

        private long? _Property_ID;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "Property_ID",ColDesc = "属性")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Property_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "属性" )]
        [FKRelationAttribute("tb_ProdProperty","Property_ID")]
        public long? Property_ID
        { 
            get{return _Property_ID;}
            set{
            SetProperty(ref _Property_ID, value);
                        }
        }

        private long? _ProdDetailID;
        /// <summary>
        /// 货品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "货品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long? ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private long? _ProdBaseID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdBaseID",ColDesc = "货品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdBaseID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "货品" )]
        [FKRelationAttribute("tb_Prod","ProdBaseID")]
        public long? ProdBaseID
        { 
            get{return _ProdBaseID;}
            set{
            SetProperty(ref _ProdBaseID, value);
                        }
        }

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(ProdBaseID))]
        public virtual tb_Prod tb_prod { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(PropertyValueID))]
        public virtual tb_ProdPropertyValue tb_prodpropertyvalue { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(Property_ID))]
        public virtual tb_ProdProperty tb_prodproperty { get; set; }



        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_Prod_Attr_Relation loctype = (tb_Prod_Attr_Relation)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

