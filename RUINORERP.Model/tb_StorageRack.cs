
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:19
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
    /// 货架信息表
    /// </summary>
    [Serializable()]
    [Description("货架信息表")]
    [SugarTable("tb_StorageRack")]
    public partial class tb_StorageRack: BaseEntity, ICloneable
    {
        public tb_StorageRack()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("货架信息表tb_StorageRack" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Rack_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Rack_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Rack_ID
        { 
            get{return _Rack_ID;}
            set{
            SetProperty(ref _Rack_ID, value);
                base.PrimaryKeyID = _Rack_ID;
            }
        }

        private long? _Location_ID;
        /// <summary>
        /// 所属仓库
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "所属仓库")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "所属仓库" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long? Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
                        }
        }

        private string _RackNO;
        /// <summary>
        /// 货架编号
        /// </summary>
        [AdvQueryAttribute(ColName = "RackNO",ColDesc = "货架编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RackNO" ,Length=50,IsNullable = false,ColumnDescription = "货架编号" )]
        public string RackNO
        { 
            get{return _RackNO;}
            set{
            SetProperty(ref _RackNO, value);
                        }
        }

        private string _RackName;
        /// <summary>
        /// 货架名称
        /// </summary>
        [AdvQueryAttribute(ColName = "RackName",ColDesc = "货架名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RackName" ,Length=50,IsNullable = false,ColumnDescription = "货架名称" )]
        public string RackName
        { 
            get{return _RackName;}
            set{
            SetProperty(ref _RackName, value);
                        }
        }

        private string _RackLocation;
        /// <summary>
        /// 货架位置
        /// </summary>
        [AdvQueryAttribute(ColName = "RackLocation",ColDesc = "货架位置")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RackLocation" ,Length=100,IsNullable = true,ColumnDescription = "货架位置" )]
        public string RackLocation
        { 
            get{return _RackLocation;}
            set{
            SetProperty(ref _RackLocation, value);
                        }
        }

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Desc" ,Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc
        { 
            get{return _Desc;}
            set{
            SetProperty(ref _Desc, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockOutDetail.Rack_ID))]
        public virtual List<tb_StockOutDetail> tb_StockOutDetails { get; set; }
        //tb_StockOutDetail.Rack_ID)
        //Rack_ID.FK_TB_STOCKOUTDETAIL_REF_TB_STORARACK)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutDetail.Rack_ID))]
        public virtual List<tb_SaleOutDetail> tb_SaleOutDetails { get; set; }
        //tb_SaleOutDetail.Rack_ID)
        //Rack_ID.FK_TB_SO_RE_STORAGERACK)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInvDetail.Rack_ID))]
        public virtual List<tb_FinishedGoodsInvDetail> tb_FinishedGoodsInvDetails { get; set; }
        //tb_FinishedGoodsInvDetail.Rack_ID)
        //Rack_ID.FK_FINISHEDGOODSDETAIL_REF_STORARACK)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.Rack_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }
        //tb_Prod.Rack_ID)
        //Rack_ID.FK_TB_PRODBASE_REF_TB_STORARACK)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_AS_RepairInStockDetail.Rack_ID))]
        public virtual List<tb_AS_RepairInStockDetail> tb_AS_RepairInStockDetails { get; set; }
        //tb_AS_RepairInStockDetail.Rack_ID)
        //Rack_ID.FK_AS_RepairInStockDetail_REF_STORAGERACK)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutReDetail.Rack_ID))]
        public virtual List<tb_SaleOutReDetail> tb_SaleOutReDetails { get; set; }
        //tb_SaleOutReDetail.Rack_ID)
        //Rack_ID.FK_SOREDETAIL_RE_STORARACK)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkEntryDetail.Rack_ID))]
        public virtual List<tb_MRP_ReworkEntryDetail> tb_MRP_ReworkEntryDetails { get; set; }
        //tb_MRP_ReworkEntryDetail.Rack_ID)
        //Rack_ID.FK_MRP_ReworkEntrydetail_REF_Rack)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Inventory.Rack_ID))]
        public virtual List<tb_Inventory> tb_Inventories { get; set; }
        //tb_Inventory.Rack_ID)
        //Rack_ID.FK_TB_INVEN_REF_TB_STORARACK)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryDetail.Rack_ID))]
        public virtual List<tb_PurEntryDetail> tb_PurEntryDetails { get; set; }
        //tb_PurEntryDetail.Rack_ID)
        //Rack_ID.FK_TB_PURENTRYDETAIL_REFE_TB_STORA)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockInDetail.Rack_ID))]
        public virtual List<tb_StockInDetail> tb_StockInDetails { get; set; }
        //tb_StockInDetail.Rack_ID)
        //Rack_ID.FK_TB_STOCKINDE_REF_TB_STORA)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StocktakeDetail.Rack_ID))]
        public virtual List<tb_StocktakeDetail> tb_StocktakeDetails { get; set; }
        //tb_StocktakeDetail.Rack_ID)
        //Rack_ID.FK_TB_STOCKSTAKEDETAIL_REF_TB_STORA)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryReDetail.Rack_ID))]
        public virtual List<tb_PurEntryReDetail> tb_PurEntryReDetails { get; set; }
        //tb_PurEntryReDetail.Rack_ID)
        //Rack_ID.FK_TB_PURENTRYREDETAIL_REF_TB_STORA)
        //tb_StorageRack.Rack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurReturnEntryDetail.Rack_ID))]
        public virtual List<tb_PurReturnEntryDetail> tb_PurReturnEntryDetails { get; set; }
        //tb_PurReturnEntryDetail.Rack_ID)
        //Rack_ID.FK_PURRETURNENTRYDETAIL_REF_STORAGERACK)
        //tb_StorageRack.Rack_ID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_StorageRack loctype = (tb_StorageRack)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

