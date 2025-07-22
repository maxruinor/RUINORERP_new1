
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:25
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
    /// 售后交付明细
    /// </summary>
    [Serializable()]
    [Description("售后交付明细")]
    [SugarTable("tb_AS_AfterSaleDeliveryDetail")]
    public partial class tb_AS_AfterSaleDeliveryDetail: BaseEntity, ICloneable
    {
        public tb_AS_AfterSaleDeliveryDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("售后交付明细tb_AS_AfterSaleDeliveryDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ASDeliveryDetailID;
        /// <summary>
        /// 交付明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ASDeliveryDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "交付明细" , IsPrimaryKey = true)]
        public long ASDeliveryDetailID
        { 
            get{return _ASDeliveryDetailID;}
            set{
            SetProperty(ref _ASDeliveryDetailID, value);
                base.PrimaryKeyID = _ASDeliveryDetailID;
            }
        }

        private long? _ASDeliveryID;
        /// <summary>
        /// 售后交付单
        /// </summary>
        [AdvQueryAttribute(ColName = "ASDeliveryID",ColDesc = "售后交付单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ASDeliveryID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "售后交付单" )]
        [FKRelationAttribute("tb_AS_AfterSaleDelivery","ASDeliveryID")]
        public long? ASDeliveryID
        { 
            get{return _ASDeliveryID;}
            set{
            SetProperty(ref _ASDeliveryID, value);
                        }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
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

        private int _Quantity= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数量" )]
        public int Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
                        }
        }

        private string _SaleFlagCode;
        /// <summary>
        /// 标识代码
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleFlagCode",ColDesc = "标识代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SaleFlagCode" ,Length=200,IsNullable = true,ColumnDescription = "标识代码" )]
        public string SaleFlagCode
        { 
            get{return _SaleFlagCode;}
            set{
            SetProperty(ref _SaleFlagCode, value);
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

        private string _CustomerPartNo;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomerPartNo
        { 
            get{return _CustomerPartNo;}
            set{
            SetProperty(ref _CustomerPartNo, value);
                        }
        }

        private long? _ASApplyDetailID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ASApplyDetailID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ASApplyDetailID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? ASApplyDetailID
        { 
            get{return _ASApplyDetailID;}
            set{
            SetProperty(ref _ASApplyDetailID, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ASDeliveryID))]
        public virtual tb_AS_AfterSaleDelivery tb_as_aftersaledelivery { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

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
            tb_AS_AfterSaleDeliveryDetail loctype = (tb_AS_AfterSaleDeliveryDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

