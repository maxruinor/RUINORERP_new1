
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/25/2025 18:51:45
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using System.Linq;

namespace RUINORERP.Model
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPayment : BaseEntity, ICloneable
    {
        #region 扩展属性

        /// <summary>
        /// 来自核销记录表中的实际核销金额
        /// </summary>
        [SugarColumn(IsIgnore = true, ColumnDescription = "核销金额")]
        public virtual decimal SettledLocalAmount { get; set; }


        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        public virtual tb_SaleOrder tb_saleorder { get; set; }


        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        public virtual tb_PurOrder tb_purorder { get; set; }


        [SugarColumn(IsIgnore = true, ColumnDescription = "来源明细")]
        [Browsable(true)]
        public string SourceItemsDisplayText
        {
            get
            {
                var parts = new List<string>();

                if (this.tb_purorder != null)
                {
                    foreach (var item in tb_purorder.tb_PurOrderDetails)
                    {
                       var part= new List<string>
                           {
                               item.tb_proddetail.DisplayText,
                              "数量:"+ item.Quantity.ToString(),
                              "单价:"+ item.UnitPrice.ToString(),
                              "小计:"+ item.SubtotalAmount.ToString(),
                            }.Where(s => !string.IsNullOrWhiteSpace(s));
                        parts.Add(string.Join(",", part));
                    }
                }


                if (this.tb_saleorder != null)
                {
                    foreach (var item in tb_saleorder.tb_SaleOrderDetails)
                    {
                        var part = new List<string>
                           {
                               item.tb_proddetail.DisplayText,
                               "数量:"+item.Quantity.ToString(),
                               "单价:"+item.TransactionPrice.ToString(),
                               "小计:"+item.SubtotalTransAmount.ToString(),
                            }.Where(s => !string.IsNullOrWhiteSpace(s));
                        parts.Add(string.Join(",", part));
                    }
                }

                return string.Join("-", parts);
            }
        }

        #endregion
    }
}

