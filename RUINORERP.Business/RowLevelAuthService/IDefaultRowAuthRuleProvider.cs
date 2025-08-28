using RUINORERP.Global;
using RUINORERP.Model;
using System.Collections.Generic;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 默认行级权限规则提供者接口
    /// 提供默认规则选项和基于默认规则创建权限策略的功能
    /// </summary>
    public interface IDefaultRowAuthRuleProvider
    {
        /// <summary>
        /// 获取指定业务类型的所有可用的默认规则选项
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>默认规则选项列表</returns>
        List<DefaultRuleOption> GetDefaultRuleOptions(BizType bizType);

        /// <summary>
        /// 根据默认规则选项生成完整的行级权限策略对象
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <param name="option">选择的默认规则选项</param>
        /// <param name="roleId">要应用的角色ID</param>
        /// <returns>可用于保存的RowAuthPolicy对象</returns>
        tb_RowAuthPolicy CreatePolicyFromDefaultOption(BizType bizType, DefaultRuleOption option, long roleId);
    }

    /// <summary>
    /// 默认规则选项类
    /// 用于表示系统预定义的行级权限规则选项
    /// </summary>
    public class DefaultRuleOption
    {
        /// <summary>
        /// 规则选项的唯一标识
        /// </summary>
        public long Key { get; set; }

        /// <summary>
        /// 规则选项的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 规则选项的描述
        /// </summary>
        public string Description { get; set; }
    }
}
