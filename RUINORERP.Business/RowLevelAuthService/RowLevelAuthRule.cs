namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 行级权限规则类型枚举
    /// 用于表示系统支持的所有默认行级权限规则
    /// </summary>
    public enum RowLevelAuthRule
    {
        /// <summary>
        /// 仅客户数据
        /// 适用于：销售订单、销售出库单、销售退回单等销售相关单据
        /// </summary>
        OnlyCustomer = 1,

        /// <summary>
        /// 仅供应商数据
        /// 适用于：采购订单、采购入库单、采购退货单等采购相关单据
        /// </summary>
        OnlySupplier = 2,

        /// <summary>
        /// 其他出入库单全部数据
        /// 适用于：其他入库单、其他出库单等特殊出入库单据
        /// </summary>
        AllDataForOtherInOut = 3,

        /// <summary>
        /// 仅收款数据
        /// 适用于：应收款单、收款单等收款相关单据
        /// </summary>
        OnlyReceivable = 4,

        /// <summary>
        /// 仅付款数据
        /// 适用于：应付款单、付款单等付款相关单据
        /// </summary>
        OnlyPayable = 5,

        /// <summary>
        /// 全部数据
        /// 适用于：所有业务类型，拥有最高权限
        /// </summary>
        AllData = 6
    }
}