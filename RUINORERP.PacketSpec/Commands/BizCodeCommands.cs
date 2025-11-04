using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 业务编码生成相关命令定义
    /// </summary>
    public static class BizCodeCommands
    {
        #region 业务编码命令 (0x0Fxx)
       

        /// <summary>
        /// 生成业务单据编号命令 - 根据业务类型生成单据编号
        /// </summary>
        public static readonly CommandId GenerateBizBillNo = new CommandId(CommandCategory.BizCode, (byte)(CommandCatalog.BizCode_GenerateBizBillNo & 0xFF));
        
        /// <summary>
        /// 生成基础信息编号命令 - 根据数据表生成基础信息编号
        /// </summary>
        public static readonly CommandId GenerateBaseInfoNo = new CommandId(CommandCategory.BizCode, (byte)(CommandCatalog.BizCode_GenerateBaseInfoNo & 0xFF));

        /// <summary>
        /// 生成产品编码命令 - 生成产品编号
        /// </summary>
        public static readonly CommandId GenerateProductNo = new CommandId(CommandCategory.BizCode, (byte)(CommandCatalog.BizCode_GenerateProductNo & 0xFF));

        /// <summary>
        /// 生成产品SKU编码命令 - 生成产品SKU编号
        /// </summary>
        public static readonly CommandId GenerateProductSKUNo = new CommandId(CommandCategory.BizCode, (byte)(CommandCatalog.BizCode_GenerateProductSKUNo & 0xFF));

        /// <summary>
        /// 生成条码命令 - 根据原始编码生成条形码
        /// </summary>
        public static readonly CommandId GenerateBarCode = new CommandId(CommandCategory.BizCode, (byte)(CommandCatalog.BizCode_GenerateBarCode & 0xFF));
        #endregion
    }
}
