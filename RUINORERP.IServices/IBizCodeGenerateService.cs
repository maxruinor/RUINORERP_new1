using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.ProductAttribute;
using RUINORERP.PacketSpec.Models.BizCodeGenerate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.IServices
{
    /// <summary>
    /// 业务编码服务接口
    /// 提供业务单据编号、基础信息编号等编码生成功能
    /// </summary>
    public interface IBizCodeGenerateService
    {
        /// <summary>
        /// 生成业务单据编号（支持BizType枚举）
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的单据编号</returns>
        Task<string> GenerateBizBillNoAsync(BizType bizType, CancellationToken ct = default);


        /// <summary>
        /// 生成基础信息编号
        /// </summary>
        /// <param name="infoType">信息类型枚举</param>
        /// <param name="paraConst">常量参数</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的信息编号</returns>
        Task<string> GenerateBaseInfoNoAsync(BaseInfoType infoType, string paraConst = null, CancellationToken ct = default);

        /// <summary>
        /// 生成产品相关编码
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="attributes">属性组合信息</param>
        /// <param name="seqLength">序号长度</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品相关编码</returns>
        Task<string> GenerateProductRelatedCodeAsync(BaseInfoType baseInfoType, tb_Prod prod,
            string PrefixParaConst = null, int seqLength = 3, bool includeDate = false, CancellationToken ct = default);


        Task<string> GenerateProductSKUCodeAsync(BaseInfoType baseInfoType, tb_Prod prod,
        tb_ProdDetail prodDetail,
        int seqLength = 3, bool includeDate = false, CancellationToken ct = default);

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">补位字符</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的条码</returns>
        Task<string> GenerateBarCodeAsync(string originalCode, char paddingChar = '0', CancellationToken ct = default);

        /// <summary>
        /// 获取所有规则配置
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>规则配置列表</returns>
        Task<List<tb_sys_BillNoRule>> GetAllRuleConfigsAsync(CancellationToken ct = default);

        /// <summary>
        /// 保存规则配置
        /// </summary>
        /// <param name="config">规则配置</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>任务</returns>
        Task SaveRuleConfigAsync(tb_sys_BillNoRule config, CancellationToken ct = default);

        /// <summary>
        /// 删除规则配置
        /// </summary>
        /// <param name="id">规则配置ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>任务</returns>
        Task DeleteRuleConfigAsync(long id, CancellationToken ct = default);
    }
}