using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Model;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.CustomAttribute;
using RUINORERP.Repository.UnitOfWorks;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 批量权限操作器
    /// 提供批量授权、撤销、复制等操作功能
    /// 【优化】支持高效的批量权限操作
    /// </summary>
    [NoWantIOC]
    public class BatchPermissionOperator
    {
        #region 单例模式

        private static readonly Lazy<BatchPermissionOperator> _instance =
            new Lazy<BatchPermissionOperator>(() => new BatchPermissionOperator());

        public static BatchPermissionOperator Instance => _instance.Value;

        /// <summary>
        /// 公共构造函数（用于 Autofac 兼容）
        /// 注意：请使用 Instance 属性获取单例实例
        /// </summary>
        public BatchPermissionOperator()
        {
        }

        #endregion

        #region 批量菜单权限操作

        /// <summary>
        /// 批量授权菜单
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="menuIds">菜单ID列表</param>
        /// <returns>操作结果</returns>
        public async Task<BatchOperationResult> BatchGrantMenusAsync(long roleId, List<long> menuIds)
        {
            if (menuIds == null || !menuIds.Any())
                return new BatchOperationResult { Success = true, Message = "没有需要授权的菜单" };

            try
            {
                var cacheService = RolePermissionCacheService.Instance;
                var cacheItem = await cacheService.GetRolePermissionAsync(roleId);

                if (cacheItem == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取角色权限数据" };

                int successCount = 0;
                int skipCount = 0;

                foreach (var menuId in menuIds)
                {
                    var existing = cacheItem.MenuPermissions.FirstOrDefault(m => m.MenuId == menuId);
                    if (existing != null)
                    {
                        if (!existing.IsVisible)
                        {
                            existing.IsVisible = true;
                            existing.Status = RolePermissionCacheService.PermissionStatus.Modified;
                            successCount++;
                        }
                        else
                        {
                            skipCount++;
                        }
                    }
                    else
                    {
                        cacheItem.MenuPermissions.Add(new RolePermissionCacheService.MenuPermissionCache
                        {
                            MenuId = menuId,
                            IsVisible = true,
                            Status = RolePermissionCacheService.PermissionStatus.Added
                        });
                        successCount++;
                    }
                }

                cacheItem.IsDirty = true;

                MainForm.Instance.logger.LogInformation($"批量授权菜单完成：角色[{roleId}]，成功{successCount}个，跳过{skipCount}个");

                return new BatchOperationResult
                {
                    Success = true,
                    Message = $"成功授权{successCount}个菜单，跳过{skipCount}个",
                    SuccessCount = successCount,
                    SkipCount = skipCount
                };
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"批量授权菜单失败：角色[{roleId}]");
                return new BatchOperationResult { Success = false, Message = $"操作失败：{ex.Message}" };
            }
        }

        /// <summary>
        /// 批量撤销菜单权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="menuIds">菜单ID列表</param>
        /// <returns>操作结果</returns>
        public async Task<BatchOperationResult> BatchRevokeMenusAsync(long roleId, List<long> menuIds)
        {
            if (menuIds == null || !menuIds.Any())
                return new BatchOperationResult { Success = true, Message = "没有需要撤销的菜单" };

            try
            {
                var cacheService = RolePermissionCacheService.Instance;
                var cacheItem = await cacheService.GetRolePermissionAsync(roleId);

                if (cacheItem == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取角色权限数据" };

                int successCount = 0;
                int skipCount = 0;

                foreach (var menuId in menuIds)
                {
                    var existing = cacheItem.MenuPermissions.FirstOrDefault(m => m.MenuId == menuId);
                    if (existing != null && existing.IsVisible)
                    {
                        existing.IsVisible = false;
                        existing.Status = RolePermissionCacheService.PermissionStatus.Modified;
                        successCount++;
                    }
                    else
                    {
                        skipCount++;
                    }
                }

                cacheItem.IsDirty = true;

                MainForm.Instance.logger?.LogInformation(
                    $"批量撤销菜单权限完成：角色[{roleId}]，成功{successCount}个，跳过{skipCount}个");

                return new BatchOperationResult
                {
                    Success = true,
                    Message = $"成功撤销{successCount}个菜单权限，跳过{skipCount}个",
                    SuccessCount = successCount,
                    SkipCount = skipCount
                };
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"批量撤销菜单权限失败：角色[{roleId}]");
                return new BatchOperationResult { Success = false, Message = $"操作失败：{ex.Message}" };
            }
        }

        #endregion

        #region 批量按钮权限操作

        /// <summary>
        /// 批量授权按钮
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="buttonInfoIds">按钮信息ID列表</param>
        /// <param name="menuId">所属菜单ID</param>
        /// <returns>操作结果</returns>
        public async Task<BatchOperationResult> BatchGrantButtonsAsync(long roleId, List<long> buttonInfoIds, long menuId)
        {
            if (buttonInfoIds == null || !buttonInfoIds.Any())
                return new BatchOperationResult { Success = true, Message = "没有需要授权的按钮" };

            try
            {
                var cacheService = RolePermissionCacheService.Instance;
                var cacheItem = await cacheService.GetRolePermissionAsync(roleId);

                if (cacheItem == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取角色权限数据" };

                int successCount = 0;
                int skipCount = 0;

                foreach (var buttonInfoId in buttonInfoIds)
                {
                    var existing = cacheItem.ButtonPermissions
                        .FirstOrDefault(b => b.ButtonInfoId == buttonInfoId && b.MenuId == menuId);

                    if (existing != null)
                    {
                        if (!existing.IsVisible)
                        {
                            existing.IsVisible = true;
                            existing.Status = RolePermissionCacheService.PermissionStatus.Modified;
                            successCount++;
                        }
                        else
                        {
                            skipCount++;
                        }
                    }
                    else
                    {
                        cacheItem.ButtonPermissions.Add(new RolePermissionCacheService.ButtonPermissionCache
                        {
                            ButtonInfoId = buttonInfoId,
                            MenuId = menuId,
                            IsVisible = true,
                            IsEnabled = true,
                            Status = RolePermissionCacheService.PermissionStatus.Added
                        });
                        successCount++;
                    }
                }

                cacheItem.IsDirty = true;

                MainForm.Instance.logger?.LogInformation(
                    $"批量授权按钮完成：角色[{roleId}]，菜单[{menuId}]，成功{successCount}个，跳过{skipCount}个");

                return new BatchOperationResult
                {
                    Success = true,
                    Message = $"成功授权{successCount}个按钮，跳过{skipCount}个",
                    SuccessCount = successCount,
                    SkipCount = skipCount
                };
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"批量授权按钮失败：角色[{roleId}]，菜单[{menuId}]");
                return new BatchOperationResult { Success = false, Message = $"操作失败：{ex.Message}" };
            }
        }

        /// <summary>
        /// 批量撤销按钮权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="buttonInfoIds">按钮信息ID列表</param>
        /// <param name="menuId">所属菜单ID</param>
        /// <returns>操作结果</returns>
        public async Task<BatchOperationResult> BatchRevokeButtonsAsync(long roleId, List<long> buttonInfoIds, long menuId)
        {
            if (buttonInfoIds == null || !buttonInfoIds.Any())
                return new BatchOperationResult { Success = true, Message = "没有需要撤销的按钮" };

            try
            {
                var cacheService = RolePermissionCacheService.Instance;
                var cacheItem = await cacheService.GetRolePermissionAsync(roleId);

                if (cacheItem == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取角色权限数据" };

                int successCount = 0;
                int skipCount = 0;

                foreach (var buttonInfoId in buttonInfoIds)
                {
                    var existing = cacheItem.ButtonPermissions
                        .FirstOrDefault(b => b.ButtonInfoId == buttonInfoId && b.MenuId == menuId);

                    if (existing != null && existing.IsVisible)
                    {
                        existing.IsVisible = false;
                        existing.Status = RolePermissionCacheService.PermissionStatus.Modified;
                        successCount++;
                    }
                    else
                    {
                        skipCount++;
                    }
                }

                cacheItem.IsDirty = true;

                MainForm.Instance.logger?.LogInformation(
                    $"批量撤销按钮权限完成：角色[{roleId}]，菜单[{menuId}]，成功{successCount}个，跳过{skipCount}个");

                return new BatchOperationResult
                {
                    Success = true,
                    Message = $"成功撤销{successCount}个按钮权限，跳过{skipCount}个",
                    SuccessCount = successCount,
                    SkipCount = skipCount
                };
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"批量撤销按钮权限失败：角色[{roleId}]，菜单[{menuId}]");
                return new BatchOperationResult { Success = false, Message = $"操作失败：{ex.Message}" };
            }
        }

        #endregion

        #region 批量字段权限操作

        /// <summary>
        /// 批量授权字段
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="fieldInfoIds">字段信息ID列表</param>
        /// <param name="menuId">所属菜单ID</param>
        /// <returns>操作结果</returns>
        public async Task<BatchOperationResult> BatchGrantFieldsAsync(long roleId, List<long> fieldInfoIds, long menuId)
        {
            if (fieldInfoIds == null || !fieldInfoIds.Any())
                return new BatchOperationResult { Success = true, Message = "没有需要授权的字段" };

            try
            {
                var cacheService = RolePermissionCacheService.Instance;
                var cacheItem = await cacheService.GetRolePermissionAsync(roleId);

                if (cacheItem == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取角色权限数据" };

                int successCount = 0;
                int skipCount = 0;

                foreach (var fieldInfoId in fieldInfoIds)
                {
                    var existing = cacheItem.FieldPermissions
                        .FirstOrDefault(f => f.FieldInfoId == fieldInfoId && f.MenuId == menuId);

                    if (existing != null)
                    {
                        if (!existing.IsVisible)
                        {
                            existing.IsVisible = true;
                            existing.Status = RolePermissionCacheService.PermissionStatus.Modified;
                            successCount++;
                        }
                        else
                        {
                            skipCount++;
                        }
                    }
                    else
                    {
                        cacheItem.FieldPermissions.Add(new RolePermissionCacheService.FieldPermissionCache
                        {
                            FieldInfoId = fieldInfoId,
                            MenuId = menuId,
                            IsVisible = true,
                            Status = RolePermissionCacheService.PermissionStatus.Added
                        });
                        successCount++;
                    }
                }

                cacheItem.IsDirty = true;

                MainForm.Instance.logger?.LogInformation(
                    $"批量授权字段完成：角色[{roleId}]，菜单[{menuId}]，成功{successCount}个，跳过{skipCount}个");

                return new BatchOperationResult
                {
                    Success = true,
                    Message = $"成功授权{successCount}个字段，跳过{skipCount}个",
                    SuccessCount = successCount,
                    SkipCount = skipCount
                };
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"批量授权字段失败：角色[{roleId}]，菜单[{menuId}]");
                return new BatchOperationResult { Success = false, Message = $"操作失败：{ex.Message}" };
            }
        }

        /// <summary>
        /// 批量撤销字段权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="fieldInfoIds">字段信息ID列表</param>
        /// <param name="menuId">所属菜单ID</param>
        /// <returns>操作结果</returns>
        public async Task<BatchOperationResult> BatchRevokeFieldsAsync(long roleId, List<long> fieldInfoIds, long menuId)
        {
            if (fieldInfoIds == null || !fieldInfoIds.Any())
                return new BatchOperationResult { Success = true, Message = "没有需要撤销的字段" };

            try
            {
                var cacheService = RolePermissionCacheService.Instance;
                var cacheItem = await cacheService.GetRolePermissionAsync(roleId);

                if (cacheItem == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取角色权限数据" };

                int successCount = 0;
                int skipCount = 0;

                foreach (var fieldInfoId in fieldInfoIds)
                {
                    var existing = cacheItem.FieldPermissions
                        .FirstOrDefault(f => f.FieldInfoId == fieldInfoId && f.MenuId == menuId);

                    if (existing != null && existing.IsVisible)
                    {
                        existing.IsVisible = false;
                        existing.Status = RolePermissionCacheService.PermissionStatus.Modified;
                        successCount++;
                    }
                    else
                    {
                        skipCount++;
                    }
                }

                cacheItem.IsDirty = true;

                MainForm.Instance.logger?.LogInformation(
                    $"批量撤销字段权限完成：角色[{roleId}]，菜单[{menuId}]，成功{successCount}个，跳过{skipCount}个");

                return new BatchOperationResult
                {
                    Success = true,
                    Message = $"成功撤销{successCount}个字段权限，跳过{skipCount}个",
                    SuccessCount = successCount,
                    SkipCount = skipCount
                };
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"批量撤销字段权限失败：角色[{roleId}]，菜单[{menuId}]");
                return new BatchOperationResult { Success = false, Message = $"操作失败：{ex.Message}" };
            }
        }

        #endregion

        #region 复制角色权限

        /// <summary>
        /// 复制角色权限
        /// 【优化】支持将一个角色的权限复制到另一个角色
        /// </summary>
        /// <param name="sourceRoleId">源角色ID</param>
        /// <param name="targetRoleId">目标角色ID</param>
        /// <param name="copyMode">复制模式（覆盖/合并）</param>
        /// <returns>操作结果</returns>
        public async Task<BatchOperationResult> CopyRolePermissionsAsync(
            long sourceRoleId,
            long targetRoleId,
            CopyMode copyMode = CopyMode.Overwrite)
        {
            if (sourceRoleId == targetRoleId)
                return new BatchOperationResult { Success = false, Message = "源角色和目标角色不能相同" };

            try
            {
                var cacheService = RolePermissionCacheService.Instance;

                // 获取源角色权限
                var sourceCache = await cacheService.GetRolePermissionAsync(sourceRoleId);
                if (sourceCache == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取源角色权限数据" };

                // 获取目标角色权限
                var targetCache = await cacheService.GetRolePermissionAsync(targetRoleId, true);
                if (targetCache == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取目标角色权限数据" };

                int menuCount = 0;
                int buttonCount = 0;
                int fieldCount = 0;

                // 根据复制模式处理
                if (copyMode == CopyMode.Overwrite)
                {
                    // 覆盖模式：清除目标角色现有权限
                    ClearTargetPermissions(targetCache);
                }

                // 复制菜单权限
                foreach (var sourceMenu in sourceCache.MenuPermissions.Where(m => m.IsVisible))
                {
                    var targetMenu = targetCache.MenuPermissions.FirstOrDefault(m => m.MenuId == sourceMenu.MenuId);
                    if (targetMenu != null)
                    {
                        if (!targetMenu.IsVisible)
                        {
                            targetMenu.IsVisible = true;
                            targetMenu.Status = RolePermissionCacheService.PermissionStatus.Modified;
                            menuCount++;
                        }
                    }
                    else
                    {
                        targetCache.MenuPermissions.Add(new RolePermissionCacheService.MenuPermissionCache
                        {
                            MenuId = sourceMenu.MenuId,
                            IsVisible = true,
                            Status = RolePermissionCacheService.PermissionStatus.Added
                        });
                        menuCount++;
                    }
                }

                // 复制按钮权限
                foreach (var sourceButton in sourceCache.ButtonPermissions.Where(b => b.IsVisible))
                {
                    var targetButton = targetCache.ButtonPermissions
                        .FirstOrDefault(b => b.ButtonInfoId == sourceButton.ButtonInfoId && b.MenuId == sourceButton.MenuId);

                    if (targetButton != null)
                    {
                        if (!targetButton.IsVisible)
                        {
                            targetButton.IsVisible = true;
                            targetButton.IsEnabled = sourceButton.IsEnabled;
                            targetButton.Status = RolePermissionCacheService.PermissionStatus.Modified;
                            buttonCount++;
                        }
                    }
                    else
                    {
                        targetCache.ButtonPermissions.Add(new RolePermissionCacheService.ButtonPermissionCache
                        {
                            ButtonInfoId = sourceButton.ButtonInfoId,
                            MenuId = sourceButton.MenuId,
                            IsVisible = true,
                            IsEnabled = sourceButton.IsEnabled,
                            ButtonName = sourceButton.ButtonName,
                            Status = RolePermissionCacheService.PermissionStatus.Added
                        });
                        buttonCount++;
                    }
                }

                // 复制字段权限
                foreach (var sourceField in sourceCache.FieldPermissions.Where(f => f.IsVisible))
                {
                    var targetField = targetCache.FieldPermissions
                        .FirstOrDefault(f => f.FieldInfoId == sourceField.FieldInfoId && f.MenuId == sourceField.MenuId);

                    if (targetField != null)
                    {
                        if (!targetField.IsVisible)
                        {
                            targetField.IsVisible = true;
                            targetField.Status = RolePermissionCacheService.PermissionStatus.Modified;
                            fieldCount++;
                        }
                    }
                    else
                    {
                        targetCache.FieldPermissions.Add(new RolePermissionCacheService.FieldPermissionCache
                        {
                            FieldInfoId = sourceField.FieldInfoId,
                            MenuId = sourceField.MenuId,
                            IsVisible = true,
                            IsChild = sourceField.IsChild,
                            FieldName = sourceField.FieldName,
                            Status = RolePermissionCacheService.PermissionStatus.Added
                        });
                        fieldCount++;
                    }
                }

                targetCache.IsDirty = true;

                MainForm.Instance.logger?.LogInformation(
                    $"复制角色权限完成：从角色[{sourceRoleId}]到角色[{targetRoleId}]，" +
                    $"菜单{menuCount}个，按钮{buttonCount}个，字段{fieldCount}个");

                return new BatchOperationResult
                {
                    Success = true,
                    Message = $"成功复制权限：菜单{menuCount}个，按钮{buttonCount}个，字段{fieldCount}个",
                    SuccessCount = menuCount + buttonCount + fieldCount
                };
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"复制角色权限失败：从角色[{sourceRoleId}]到角色[{targetRoleId}]");
                return new BatchOperationResult { Success = false, Message = $"操作失败：{ex.Message}" };
            }
        }

        /// <summary>
        /// 清除目标角色权限
        /// </summary>
        private void ClearTargetPermissions(RolePermissionCacheService.RolePermissionCacheItem targetCache)
        {
            foreach (var menu in targetCache.MenuPermissions)
            {
                if (menu.IsVisible)
                {
                    menu.IsVisible = false;
                    menu.Status = RolePermissionCacheService.PermissionStatus.Modified;
                }
            }

            foreach (var button in targetCache.ButtonPermissions)
            {
                if (button.IsVisible)
                {
                    button.IsVisible = false;
                    button.Status = RolePermissionCacheService.PermissionStatus.Modified;
                }
            }

            foreach (var field in targetCache.FieldPermissions)
            {
                if (field.IsVisible)
                {
                    field.IsVisible = false;
                    field.Status = RolePermissionCacheService.PermissionStatus.Modified;
                }
            }
        }

        #endregion

        #region 批量保存到数据库

        /// <summary>
        /// 批量保存权限变更到数据库
        /// 【P0修复】使用项目标准事务体系，确保数据一致性
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>操作结果</returns>
        public async Task<BatchOperationResult> BatchSaveToDatabaseAsync(long roleId)
        {
            IUnitOfWorkManage uow = null;
            
            try
            {
                var cacheService = RolePermissionCacheService.Instance;
                var cacheItem = await cacheService.GetRolePermissionAsync(roleId);

                if (cacheItem == null)
                    return new BatchOperationResult { Success = false, Message = "无法获取角色权限数据" };

                if (!cacheItem.IsDirty)
                    return new BatchOperationResult { Success = true, Message = "没有需要保存的变更" };

                // 【P0修复】使用项目标准事务管理体系
                uow = Startup.GetFromFac<IUnitOfWorkManage>();
                
                // 开始事务（使用配置默认超时30秒）
                await uow.BeginTranAsync(timeoutSeconds: PermissionConfig.BatchOperationTimeoutSeconds);
                
                try
                {
                    // 【优化】直接使用UnitOfWork中的db，保证在同一事务上下文中
                    var db = uow.GetDbClient();
                    
                    int menuCount = 0;
                    int buttonCount = 0;
                    int fieldCount = 0;

                    MainForm.Instance.logger?.LogInformation($"开始保存角色[{roleId}]权限变更，事务已开启");

                    // 保存菜单权限变更
                    var menuChanges = cacheItem.MenuPermissions
                        .Where(m => m.Status != RolePermissionCacheService.PermissionStatus.Unchanged)
                        .ToList();

                    foreach (var menu in menuChanges)
                    {
                        await SaveMenuPermissionChangeAsync((ISqlSugarClient)db, roleId, menu);
                        menu.Status = RolePermissionCacheService.PermissionStatus.Unchanged;
                        menuCount++;
                    }

                    // 保存按钮权限变更
                    var buttonChanges = cacheItem.ButtonPermissions
                        .Where(b => b.Status != RolePermissionCacheService.PermissionStatus.Unchanged)
                        .ToList();

                    foreach (var button in buttonChanges)
                    {
                        await SaveButtonPermissionChangeAsync((ISqlSugarClient)db, roleId, button);
                        button.Status = RolePermissionCacheService.PermissionStatus.Unchanged;
                        buttonCount++;
                    }

                    // 保存字段权限变更
                    var fieldChanges = cacheItem.FieldPermissions
                        .Where(f => f.Status != RolePermissionCacheService.PermissionStatus.Unchanged)
                        .ToList();

                    foreach (var field in fieldChanges)
                    {
                        await SaveFieldPermissionChangeAsync((ISqlSugarClient)db, roleId, field);
                        field.Status = RolePermissionCacheService.PermissionStatus.Unchanged;
                        fieldCount++;
                    }

                    // 更新缓存状态
                    cacheItem.IsDirty = false;
                    cacheItem.LastSyncTime = DateTime.Now;
                    cacheItem.Version++;

                    // 【P0修复】提交事务
                    await uow.CommitTranAsync();

                    MainForm.Instance.logger?.LogInformation(
                        $"批量保存权限变更成功：角色[{roleId}]，菜单{menuCount}个，按钮{buttonCount}个，字段{fieldCount}个，事务已提交");

                    return new BatchOperationResult
                    {
                        Success = true,
                        Message = $"成功保存{menuCount + buttonCount + fieldCount}条权限变更",
                        SuccessCount = menuCount + buttonCount + fieldCount
                    };
                }
                catch (Exception ex)
                {
                    // 【P0修复】异常时回滚事务
                    if (uow.TranCount > 0)
                    {
                        await uow.RollbackTranAsync();
                    }
                    MainForm.Instance.logger?.LogError(ex, $"批量保存权限变更失败，事务已回滚：角色[{roleId}]");
                    return new BatchOperationResult { Success = false, Message = $"保存失败：{ex.Message}" };
                }
            }
            catch (Exception ex)
            {
                // 确保异常时回滚
                if (uow != null && uow.TranCount > 0)
                {
                    try
                    {
                        await uow.RollbackTranAsync();
                    }
                    catch (Exception rollbackEx)
                    {
                        MainForm.Instance.logger?.LogError(rollbackEx, $"回滚事务时发生异常：角色[{roleId}]");
                    }
                }
                
                MainForm.Instance.logger?.LogError(ex, $"批量保存权限变更异常：角色[{roleId}]");
                return new BatchOperationResult { Success = false, Message = $"保存异常：{ex.Message}" };
            }
            finally
            {
                // 释放UnitOfWork资源
                if (uow != null)
                {
                    try
                    {
                        await uow.DisposeAsync();
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger?.LogWarning(ex, "释放UnitOfWork时发生异常");
                    }
                }
            }
        }

        /// <summary>
        /// 保存菜单权限变更到数据库
        /// </summary>
        private async Task SaveMenuPermissionChangeAsync(ISqlSugarClient db, long roleId, RolePermissionCacheService.MenuPermissionCache menu)
        {
            if (menu.Status == RolePermissionCacheService.PermissionStatus.Added)
            {
                // 新增
                var entity = new tb_P4Menu
                {
                    MenuID = menu.MenuId,
                    RoleID = roleId,
                    IsVisble = menu.IsVisible,
                    Created_at = DateTime.Now,
                    Created_by = MainForm.Instance.AppContext.CurUserInfo?.EmployeeId
                };
                menu.P4MenuId = await db.Insertable(entity).ExecuteReturnIdentityAsync();
            }
            else if (menu.Status == RolePermissionCacheService.PermissionStatus.Modified && menu.P4MenuId.HasValue)
            {
                // 修改
                await db.Updateable<tb_P4Menu>()
                    .SetColumns(m => new tb_P4Menu
                    {
                        IsVisble = menu.IsVisible,
                        Modified_at = DateTime.Now,
                        Modified_by = MainForm.Instance.AppContext.CurUserInfo.EmployeeId
                    })
                    .Where(m => m.P4Menu_ID == menu.P4MenuId.Value)
                    .ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 保存按钮权限变更到数据库
        /// </summary>
        private async Task SaveButtonPermissionChangeAsync(ISqlSugarClient db, long roleId, RolePermissionCacheService.ButtonPermissionCache button)
        {
            if (button.Status == RolePermissionCacheService.PermissionStatus.Added)
            {
                // 新增
                var entity = new tb_P4Button
                {
                    ButtonInfo_ID = button.ButtonInfoId,
                    MenuID = button.MenuId,
                    RoleID = roleId,
                    IsVisble = button.IsVisible,
                    IsEnabled = button.IsEnabled,
                    Created_at = DateTime.Now,
                    Created_by = MainForm.Instance.AppContext.CurUserInfo?.EmployeeId
                };
                button.P4BtnId = await db.Insertable(entity).ExecuteReturnIdentityAsync();
            }
            else if (button.Status == RolePermissionCacheService.PermissionStatus.Modified && button.P4BtnId.HasValue)
            {
                // 修改
                await db.Updateable<tb_P4Button>()
                    .SetColumns(b => new tb_P4Button
                    {
                        IsVisble = button.IsVisible,
                        IsEnabled = button.IsEnabled,
                        Modified_at = DateTime.Now,
                        Modified_by = MainForm.Instance.AppContext.CurUserInfo.EmployeeId
                    })
                    .Where(b => b.P4Btn_ID == button.P4BtnId.Value)
                    .ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 保存字段权限变更到数据库
        /// </summary>
        private async Task SaveFieldPermissionChangeAsync(ISqlSugarClient db, long roleId, RolePermissionCacheService.FieldPermissionCache field)
        {
            if (field.Status == RolePermissionCacheService.PermissionStatus.Added)
            {
                // 新增
                var entity = new tb_P4Field
                {
                    FieldInfo_ID = field.FieldInfoId,
                    MenuID = field.MenuId,
                    RoleID = roleId,
                    IsVisble = field.IsVisible,
                    IsChild = field.IsChild,
                    Created_at = DateTime.Now,
                    Created_by = MainForm.Instance.AppContext.CurUserInfo.EmployeeId
                };
                field.P4FieldId = await db.Insertable(entity).ExecuteReturnIdentityAsync();
            }
            else if (field.Status == RolePermissionCacheService.PermissionStatus.Modified && field.P4FieldId.HasValue)
            {
                // 修改
                await db.Updateable<tb_P4Field>()
                    .SetColumns(f => new tb_P4Field
                    {
                        IsVisble = field.IsVisible,
                        Modified_at = DateTime.Now,
                        Modified_by = MainForm.Instance.AppContext.CurUserInfo.EmployeeId
                    })
                    .Where(f => f.P4Field_ID == field.P4FieldId.Value)
                    .ExecuteCommandAsync();
            }
        }

        #endregion
    }

    /// <summary>
    /// 批量操作结果
    /// </summary>
    public class BatchOperationResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 操作消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 跳过数量
        /// </summary>
        public int SkipCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailCount { get; set; }
    }

    /// <summary>
    /// 复制模式
    /// </summary>
    public enum CopyMode
    {
        /// <summary>
        /// 覆盖模式：清除目标角色现有权限，复制源角色权限
        /// </summary>
        Overwrite,

        /// <summary>
        /// 合并模式：保留目标角色现有权限，添加源角色的新权限
        /// </summary>
        Merge
    }
}