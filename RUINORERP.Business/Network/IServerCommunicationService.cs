using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;

namespace RUINORERP.Business.Network
{
    /// <summary>
    /// 服务器通信服务接口
    /// 提供业务层与服务器通信的能力，用于生成各种业务编号
    /// 抽象了通信细节，使业务层不需要直接依赖UI层的通信组件
    /// </summary>
    public interface IServerCommunicationService
    {
        /// <summary>
        /// 获取当前连接状态
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 生成业务单据编号
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="parameter">业务编码参数</param>
        /// <returns>生成的单据编号</returns>
        string GenerateBizBillNo(BizType bizType, BizCodeParameter parameter = null);

        /// <summary>
        /// 生成基础信息编号
        /// </summary>
        /// <param name="infoType">信息类型</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>生成的信息编号</returns>
        string GenerateBaseInfoNo(string infoType, string paraConst = null);

        /// <summary>
        /// 生成产品编码
        /// </summary>
        /// <param name="categoryId">产品类别ID</param>
        /// <param name="customPrefix">自定义前缀</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <returns>生成的产品编码</returns>
        string GenerateProductNo(long categoryId, string customPrefix = null, bool includeDate = false);

        /// <summary>
        /// 生成产品SKU编码
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="attributes">属性组合信息</param>
        /// <param name="seqLength">序号长度</param>
        /// <returns>生成的产品SKU编码</returns>
        string GenerateProductSKUNo(long productId, string productCode, string attributes = null, int seqLength = 3);

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">补位字符</param>
        /// <returns>生成的条码</returns>
        string GenerateBarCode(string originalCode, char paddingChar = '0');
    }
}