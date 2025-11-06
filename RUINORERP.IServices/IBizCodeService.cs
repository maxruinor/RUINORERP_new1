using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.IServices
{
    /// <summary>
    /// 业务编码服务接口
    /// 提供业务单据编号、基础信息编号等编码生成功能
    /// </summary>
    public interface IBizCodeService
    {
        /// <summary>
        /// 生成业务单据编号
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的业务单据编号</returns>
        Task<string> GenerateBizBillNoAsync(BizType bizType, CancellationToken ct = default);

        /// <summary>
        /// 生成基础信息编号（枚举版本）
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的基础信息编号</returns>
        Task<string> GenerateBaseInfoNoAsync(BaseInfoType baseInfoType, string paraConst = null, CancellationToken ct = default);

        /// <summary>
        /// 生成基础信息编号（字符串版本）
        /// </summary>
        /// <param name="baseInfoType">基础信息类型</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的基础信息编号</returns>
        Task<string> GenerateBaseInfoNoAsync(string baseInfoType, string paraConst = null, CancellationToken ct = default);

        /// <summary>
        /// 生成产品编码
        /// </summary>
        /// <param name="categoryId">产品类别ID</param>
        /// <param name="customPrefix">自定义前缀（可选）</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品编码</returns>
        Task<string> GenerateProductNoAsync(long categoryId, string customPrefix = null, bool includeDate = false, CancellationToken ct = default);

        /// <summary>
        /// 生成产品SKU编码
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="attributes">属性组合信息</param>
        /// <param name="seqLength">序号长度</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品SKU编码</returns>
        Task<string> GenerateProductSKUNoAsync(long productId, string productCode, string attributes = null, int seqLength = 3, CancellationToken ct = default);

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">补位字符</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的条码</returns>
        Task<string> GenerateBarCodeAsync(string originalCode, char paddingChar = '0', CancellationToken ct = default);
    }
}