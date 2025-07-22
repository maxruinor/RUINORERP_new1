
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:32
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
    /// 维修工单明细
    /// </summary>
    [Serializable()]
    [Description("维修工单明细")]
    [SugarTable("tb_AS_RepairOrderDetail")]
    public partial class tb_AS_RepairOrderDetail: BaseEntity, ICloneable
    {
        public tb_AS_RepairOrderDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("维修工单明细tb_AS_RepairOrderDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RepairOrderDetailID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RepairOrderDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long RepairOrderDetailID
        { 
            get{return _RepairOrderDetailID;}
            set{
            SetProperty(ref _RepairOrderDetailID, value);
                base.PrimaryKeyID = _RepairOrderDetailID;
            }
        }

        private long? _RepairOrderID;
        /// <summary>
        /// 维修工单
        /// </summary>
        [AdvQueryAttribute(ColName = "RepairOrderID",ColDesc = "维修工单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RepairOrderID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "维修工单" )]
        [FKRelationAttribute("tb_AS_RepairOrder","RepairOrderID")]
        public long? RepairOrderID
        { 
            get{return _RepairOrderID;}
            set{
            SetProperty(ref _RepairOrderID, value);
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

        private string _RepairContent;
        /// <summary>
        /// 维修内容
        /// </summary>
        [AdvQueryAttribute(ColName = "RepairContent",ColDesc = "维修内容")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RepairContent" ,Length=500,IsNullable = true,ColumnDescription = "维修内容" )]
        public string RepairContent
        { 
            get{return _RepairContent;}
            set{
            SetProperty(ref _RepairContent, value);
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
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(RepairOrderID))]
        public virtual tb_AS_RepairOrder tb_as_repairorder { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_AS_RepairOrderDetail loctype = (tb_AS_RepairOrderDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

