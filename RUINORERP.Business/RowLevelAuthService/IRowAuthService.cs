using RUINORERP.Global;
using RUINORERP.Model;
using System.Collections.Generic;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 行级权限服务接口
    /// 提供行级数据权限控制的核心功能
    /// </summary>
    public interface IRowAuthService
    {
        /// <summary>
        /// 获取用户对特定实体类型的数据权限过滤条件
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>SQL过滤条件子句</returns>
        string GetUserRowAuthFilterClause(System.Type entityType);

        /// <summary>
        /// 获取指定角色和业务类型的行级权限配置
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="bizType">业务类型</param>
        /// <returns>行级权限配置DTO</returns>
        RowAuthConfigDto GetRowAuthConfig(long roleId, BizType bizType);

        /// <summary>
        /// 保存行级权限配置
        /// </summary>
        /// <param name="config">行级权限配置DTO</param>
        /// <returns>保存是否成功</returns>
        bool SaveRowAuthConfig(RowAuthConfigDto config);

        /// <summary>
        /// 获取所有权限策略
        /// </summary>
        /// <returns>权限策略列表</returns>
        List<tb_RowAuthPolicy> GetAllPolicies();

        /// <summary>
        /// 为规则分配角色
        /// </summary>
        /// <param name="policyId">策略ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns>分配是否成功</returns>
        bool AssignPolicyToRole(long policyId, long roleId);

        /// <summary>
        /// 从角色移除规则
        /// </summary>
        /// <param name="policyId">策略ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns>移除是否成功</returns>
        bool RemovePolicyFromRole(long policyId, long roleId);
    }

    /// <summary>
    /// 行级权限配置DTO
    /// 用于传输行级权限配置信息
    /// </summary>
    public class RowAuthConfigDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BizType BizType { get; set; }

        /// <summary>
        /// 可用的默认规则选项
        /// </summary>
        public List<DefaultRuleOption> AvailableDefaultOptions { get; set; }

        public List<tb_RowAuthPolicy> AvailableDefaultPolicies { get; set; } // 新增
        /// <summary>
        /// 已分配的权限策略列表
        /// </summary>
        public List<tb_RowAuthPolicy> AssignedPolicies { get; set; }

        /// <summary>
        /// 新创建的自定义权限策略
        /// </summary>
        public tb_RowAuthPolicy NewCustomPolicy { get; set; }
    }
}