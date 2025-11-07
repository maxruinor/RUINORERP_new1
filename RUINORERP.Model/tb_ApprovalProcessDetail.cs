
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:36
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
    /// 审核流程明细表
    /// </summary>
    [Serializable()]
    [Description("审核流程明细表")]
    [SugarTable("tb_ApprovalProcessDetail")]
    public partial class tb_ApprovalProcessDetail: BaseEntity, ICloneable
    {
        public tb_ApprovalProcessDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("审核流程明细表tb_ApprovalProcessDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long? _ApprovalID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ApprovalID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Approval","ApprovalID")]
        public long? ApprovalID
        { 
            get{return _ApprovalID;}
            set{
            SetProperty(ref _ApprovalID, value);
                        }
        }

        private long _ApprovalCID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ApprovalCID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ApprovalCID
        { 
            get{return _ApprovalCID;}
            set{
            SetProperty(ref _ApprovalCID, value);
                base.PrimaryKeyID = _ApprovalCID;
            }
        }

        private int? _ApprovalResults;
        /// <summary>
        /// 审核结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审核结果")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ApprovalResults" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审核结果" )]
        public int? ApprovalResults
        { 
            get{return _ApprovalResults;}
            set{
            SetProperty(ref _ApprovalResults, value);
                        }
        }

        private int? _ApprovalOrder;
        /// <summary>
        /// 审核顺序
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOrder",ColDesc = "审核顺序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ApprovalOrder" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审核顺序" )]
        public int? ApprovalOrder
        { 
            get{return _ApprovalOrder;}
            set{
            SetProperty(ref _ApprovalOrder, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ApprovalID))]
        public virtual tb_Approval tb_approval { get; set; }



        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ApprovalProcessDetail loctype = (tb_ApprovalProcessDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

