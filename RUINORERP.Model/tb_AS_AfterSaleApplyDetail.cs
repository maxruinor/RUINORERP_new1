
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:34
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
    /// 售后申请单明细
    /// </summary>
    [Serializable()]
    [Description("售后申请单明细")]
    [SugarTable("tb_AS_AfterSaleApplyDetail")]
    public partial class tb_AS_AfterSaleApplyDetail: BaseEntity, ICloneable
    {
        public tb_AS_AfterSaleApplyDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("售后申请单明细tb_AS_AfterSaleApplyDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ASApplyDetailID;
        /// <summary>
        /// 售后产品明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ASApplyDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "售后产品明细" , IsPrimaryKey = true)]
        public long ASApplyDetailID
        { 
            get{return _ASApplyDetailID;}
            set{
            SetProperty(ref _ASApplyDetailID, value);
                base.PrimaryKeyID = _ASApplyDetailID;
            }
        }

        private long _ASApplyID;
        /// <summary>
        /// 售后申请单
        /// </summary>
        [AdvQueryAttribute(ColName = "ASApplyID",ColDesc = "售后申请单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ASApplyID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "售后申请单" )]
        [FKRelationAttribute("tb_AS_AfterSaleApply","ASApplyID")]
        public long ASApplyID
        { 
            get{return _ASApplyID;}
            set{
            SetProperty(ref _ASApplyID, value);
                        }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品" )]
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

        private string _FaultDescription;
        /// <summary>
        /// 问题描述
        /// </summary>
        [AdvQueryAttribute(ColName = "FaultDescription",ColDesc = "问题描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FaultDescription" ,Length=500,IsNullable = true,ColumnDescription = "问题描述" )]
        public string FaultDescription
        { 
            get{return _FaultDescription;}
            set{
            SetProperty(ref _FaultDescription, value);
                        }
        }

        private int _InitialQuantity= ((0));
        /// <summary>
        /// 客户申报数量
        /// </summary>
        [AdvQueryAttribute(ColName = "InitialQuantity",ColDesc = "客户申报数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "InitialQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "客户申报数量" )]
        public int InitialQuantity
        { 
            get{return _InitialQuantity;}
            set{
            SetProperty(ref _InitialQuantity, value);
                        }
        }

        private int _ConfirmedQuantity= ((0));
        /// <summary>
        /// 复核数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ConfirmedQuantity",ColDesc = "复核数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ConfirmedQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "复核数量" )]
        public int ConfirmedQuantity
        { 
            get{return _ConfirmedQuantity;}
            set{
            SetProperty(ref _ConfirmedQuantity, value);
                        }
        }

        private string _CustomerPartNo;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=100,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomerPartNo
        { 
            get{return _CustomerPartNo;}
            set{
            SetProperty(ref _CustomerPartNo, value);
                        }
        }

        private int _DeliveredQty= ((0));
        /// <summary>
        /// 交付数量
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveredQty",ColDesc = "交付数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DeliveredQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "交付数量" )]
        public int DeliveredQty
        { 
            get{return _DeliveredQty;}
            set{
            SetProperty(ref _DeliveredQty, value);
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ASApplyID))]
        public virtual tb_AS_AfterSaleApply tb_as_aftersaleapply { get; set; }

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
            tb_AS_AfterSaleApplyDetail loctype = (tb_AS_AfterSaleApplyDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

