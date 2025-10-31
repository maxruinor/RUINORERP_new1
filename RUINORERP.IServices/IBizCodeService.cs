using System;
using System.Collections.Generic;
using System.Text;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;

namespace RUINORERP.IServices
{
    /// <summary>
    /// 业务编码服务接口
    /// 提供业务单据编号、基础信息编号等编码生成功能
    /// </summary>
    public interface IBizCodeService
    {


        /// <summary>
        /// 生成业务单据编号（支持BizType枚举）
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <returns>生成的单据编号</returns>
        string GenerateBizBillNo(BizType bizType);

        /// <summary>
        /// 生成业务单据编号（支持BizType枚举和额外参数）
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="parameter">业务编码参数</param>
        /// <returns>生成的单据编号</returns>
        string GenerateBizBillNo(BizType bizType, BizCodeParameter parameter = null);

        /// <summary>
        /// 生成基础信息编号
        /// </summary>
        /// <param name="infoType">信息类型</param>
        /// <returns>生成的信息编号</returns>
        string GenerateBaseInfoNo(string infoType);

        /// <summary>
        /// 生成基础信息编号（带参数）
        /// </summary>
        /// <param name="infoType">信息类型</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>生成的信息编号</returns>
        string GenerateBaseInfoNo(string infoType, string paraConst);

        /// <summary>
        /// 生成产品编码
        /// </summary>
        /// <param name="productCategory">产品类别</param>
        /// <returns>生成的产品编码</returns>
        string GenerateProductNo(string productCategory = null);

        /// <summary>
        /// 生成产品SKU编码
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="attributes">属性组合</param>
        /// <returns>生成的SKU编码</returns>
        string GenerateProductSKUNo(string productId = null, string attributes = null);

        /// <summary>
        /// 根据规则生成编号
        /// </summary>
        /// <param name="rule">编码规则</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>生成的编号</returns>
        string GenerateByRule(string rule, Dictionary<string, object> parameters = null);
        
        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="code">原始编码</param>
        /// <param name="bwcode">条码补位码</param>
        /// <returns>生成的条码</returns>
        string GenerateBarCode(string code, char bwcode = '0');
    }
}