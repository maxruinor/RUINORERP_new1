using RUINORERP.Global;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        /// <param name="menuId">菜单ID，用于区分不同功能的数据规则</param>
        /// <returns>SQL过滤条件子句</returns>
        string GetUserRowAuthFilterClause(Type entityType, long menuId);

        /// <summary>
        /// 获取用户对特定实体类型的数据权限过滤条件(Expression形式)
        /// 适用于SqlSugar的Lambda表达式查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="menuId">菜单ID</param>
        /// <returns>Lambda表达式</returns>
        Expression<Func<T, bool>> GetUserRowAuthFilterExpression<T>(long menuId) where T : class;

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
        /// 异步保存行级权限配置
        /// </summary>
        /// <param name="config">行级权限配置DTO</param>
        /// <returns>保存是否成功</returns>
        Task<bool> SaveRowAuthConfigAsync(RowAuthConfigDto config);

        /// <summary>
        /// 获取所有权限策略
        /// </summary>
        /// <returns>权限策略列表</returns>
        List<tb_RowAuthPolicy> GetAllPolicies();

        /// <summary>
        /// 根据指定的业务类型获取能参与配置的规则
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>权限策略列表</returns>
        List<tb_RowAuthPolicy> GetAllPolicies(BizType bizType);

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

        /// <summary>
        /// 可用的默认策略（尚未分配给该角色的）
        /// </summary>
        public List<tb_RowAuthPolicy> AvailableDefaultPolicies { get; set; }
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