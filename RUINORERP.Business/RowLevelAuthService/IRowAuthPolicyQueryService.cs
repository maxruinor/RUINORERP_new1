// ********************************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：系统自动生成
// 时间：2025-01-09
// 描述：行级权限策略查询服务接口，支持根据用户和角色获取对应的权限策略
// ********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Model;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 行级权限策略查询服务接口
    /// 支持根据用户和角色获取对应的权限策略
    /// </summary>
    public interface IRowAuthPolicyQueryService
    {
        /// <summary>
        /// 根据用户ID获取该用户的所有行级权限策略
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="menuId">菜单ID（可选，用于限定特定菜单的权限）</param>
        /// <returns>行级权限策略列表</returns>
        Task<List<tb_RowAuthPolicy>> GetPoliciesByUserAsync(long userId, long? menuId = null);

        /// <summary>
        /// 根据角色ID列表获取所有对应的行级权限策略
        /// </summary>
        /// <param name="roleIds">角色ID列表</param>
        /// <param name="menuId">菜单ID（可选，用于限定特定菜单的权限）</param>
        /// <returns>行级权限策略列表</returns>
        Task<List<tb_RowAuthPolicy>> GetPoliciesByRolesAsync(List<long> roleIds, long? menuId = null);

        /// <summary>
        /// 根据用户ID和该用户的角色ID列表获取所有行级权限策略（用户策略和角色策略的并集）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleIds">角色ID列表</param>
        /// <param name="menuId">菜单ID（可选，用于限定特定菜单的权限）</param>
        /// <returns>行级权限策略列表（去重）</returns>
        Task<List<tb_RowAuthPolicy>> GetPoliciesByUserAndRolesAsync(long userId, List<long> roleIds, long? menuId = null);

        /// <summary>
        /// 根据实体类型获取所有启用的行级权限策略
        /// </summary>
        /// <param name="entityType">实体类型全限定名</param>
        /// <returns>行级权限策略列表</returns>
        Task<List<tb_RowAuthPolicy>> GetPoliciesByEntityTypeAsync(string entityType);

        /// <summary>
        /// 检查指定菜单是否配置了行级权限策略
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否有配置策略</returns>
        Task<bool> HasPolicyForMenuAsync(long menuId);

        /// <summary>
        /// 清除策略缓存（在策略变更时调用）
        /// </summary>
        void ClearPolicyCache();
    }
}
