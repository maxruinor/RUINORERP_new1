// **********************************************
// 文件名: PermissionDiffComparer.cs
// 创建时间: 2026-03-27
// 作者: RUINORERP
// 功能描述: 权限差异对比器 - 对比两个角色的权限差异
// **********************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Common.CustomAttribute;
using static RUINORERP.UI.SysConfig.RolePermissionCacheService;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 权限差异对比器 - 对比两个角色的权限差异

    /// </summary>
    [NoWantIOC]
    public class PermissionDiffComparer
    {
        private readonly RolePermissionCacheService _cacheService;

        public PermissionDiffComparer()
        {
            _cacheService = RolePermissionCacheService.Instance;
        }

        /// <summary>
        /// 对比两个角色的权限差异
        /// </summary>
        public async Task<PermissionDiffResult> CompareRolesAsync(long sourceRoleId, long targetRoleId)
        {
            var sourcePermissions = await _cacheService.GetRolePermissionAsync(sourceRoleId);
            var targetPermissions = await _cacheService.GetRolePermissionAsync(targetRoleId);

            var result = new PermissionDiffResult
            {
                SourceRoleId = sourceRoleId,
                TargetRoleId = targetRoleId,
                ComparisonTime = DateTime.Now,
                MenuDiffs = CompareMenus(sourcePermissions, targetPermissions),
                ButtonDiffs = CompareButtons(sourcePermissions, targetPermissions),
                FieldDiffs = CompareFields(sourcePermissions, targetPermissions),
                RowAuthDiffs = CompareRowAuths(sourcePermissions, targetPermissions)
            };

            // 计算统计信息
            CalculateStatistics(result);

            return result;
        }

        /// <summary>
        /// 对比当前权限与数据库原始权限
        /// </summary>
        public async Task<PermissionDiffResult> CompareWithOriginalAsync(long roleId)
        {
            var currentPermissions = await _cacheService.GetRolePermissionAsync(roleId);
            var originalPermissions = await _cacheService.GetRolePermissionAsync(roleId, forceRefresh: true);

            var result = new PermissionDiffResult
            {
                SourceRoleId = roleId,
                TargetRoleId = roleId,
                ComparisonTime = DateTime.Now,
                IsCurrentVsOriginal = true,
                MenuDiffs = CompareMenus(originalPermissions, currentPermissions),
                ButtonDiffs = CompareButtons(originalPermissions, currentPermissions),
                FieldDiffs = CompareFields(originalPermissions, currentPermissions),
                RowAuthDiffs = CompareRowAuths(originalPermissions, currentPermissions)
            };

            CalculateStatistics(result);

            return result;
        }

        /// <summary>
        /// 批量对比多个角色与基准角色的差异
        /// </summary>
        public async Task<List<PermissionDiffResult>> CompareBatchAsync(long baseRoleId, List<long> compareRoleIds)
        {
            var results = new List<PermissionDiffResult>();

            foreach (var roleId in compareRoleIds)
            {
                if (roleId != baseRoleId)
                {
                    var result = await CompareRolesAsync(baseRoleId, roleId);
                    results.Add(result);
                }
            }

            return results;
        }

        /// <summary>
        /// 对比菜单权限
        /// </summary>
        private List<MenuPermissionDiff> CompareMenus(RolePermissionCacheItem source, RolePermissionCacheItem target)
        {
            var diffs = new List<MenuPermissionDiff>();

            // 获取所有菜单ID
            var allMenuIds = source.MenuPermissions.Select(m => m.MenuId)
                .Union(target.MenuPermissions.Select(m => m.MenuId))
                .Distinct()
                .ToList();

            foreach (var menuId in allMenuIds)
            {
                var sourceMenu = source.MenuPermissions.FirstOrDefault(m => m.MenuId == menuId);
                var targetMenu = target.MenuPermissions.FirstOrDefault(m => m.MenuId == menuId);

                if (sourceMenu == null && targetMenu != null)
                {
                    // 目标有，源没有 - 新增
                    diffs.Add(new MenuPermissionDiff
                    {
                        MenuId = menuId,
                        MenuName = targetMenu.MenuName,
                        DiffType = DiffType.Added,
                        SourceVisible = false,
                        TargetVisible = targetMenu.IsVisible
                    });
                }
                else if (sourceMenu != null && targetMenu == null)
                {
                    // 源有，目标没有 - 删除
                    diffs.Add(new MenuPermissionDiff
                    {
                        MenuId = menuId,
                        MenuName = sourceMenu.MenuName,
                        DiffType = DiffType.Removed,
                        SourceVisible = sourceMenu.IsVisible,
                        TargetVisible = false
                    });
                }
                else if (sourceMenu != null && targetMenu != null)
                {
                    // 都有 - 检查是否有变更
                    if (sourceMenu.IsVisible != targetMenu.IsVisible)
                    {
                        diffs.Add(new MenuPermissionDiff
                        {
                            MenuId = menuId,
                            MenuName = sourceMenu.MenuName,
                            DiffType = DiffType.Modified,
                            SourceVisible = sourceMenu.IsVisible,
                            TargetVisible = targetMenu.IsVisible
                        });
                    }
                }
            }

            return diffs;
        }

        /// <summary>
        /// 对比按钮权限
        /// </summary>
        private List<ButtonPermissionDiff> CompareButtons(RolePermissionCacheItem source, RolePermissionCacheItem target)
        {
            var diffs = new List<ButtonPermissionDiff>();

            var allButtonIds = source.ButtonPermissions.Select(b => b.ButtonInfoId)
                .Union(target.ButtonPermissions.Select(b => b.ButtonInfoId))
                .Distinct()
                .ToList();

            foreach (var buttonId in allButtonIds)
            {
                var sourceBtn = source.ButtonPermissions.FirstOrDefault(b => b.ButtonInfoId == buttonId);
                var targetBtn = target.ButtonPermissions.FirstOrDefault(b => b.ButtonInfoId == buttonId);

                if (sourceBtn == null && targetBtn != null)
                {
                    diffs.Add(new ButtonPermissionDiff
                    {
                        ButtonId = buttonId,
                        ButtonName = targetBtn.ButtonName,
                        MenuId = targetBtn.MenuId,
                        DiffType = DiffType.Added,
                        SourceVisible = false,
                        TargetVisible = targetBtn.IsVisible
                    });
                }
                else if (sourceBtn != null && targetBtn == null)
                {
                    diffs.Add(new ButtonPermissionDiff
                    {
                        ButtonId = buttonId,
                        ButtonName = sourceBtn.ButtonName,
                        MenuId = sourceBtn.MenuId,
                        DiffType = DiffType.Removed,
                        SourceVisible = sourceBtn.IsVisible,
                        TargetVisible = false
                    });
                }
                else if (sourceBtn != null && targetBtn != null)
                {
                    if (sourceBtn.IsVisible != targetBtn.IsVisible)
                    {
                        diffs.Add(new ButtonPermissionDiff
                        {
                            ButtonId = buttonId,
                            ButtonName = sourceBtn.ButtonName,
                            MenuId = sourceBtn.MenuId,
                            DiffType = DiffType.Modified,
                            SourceVisible = sourceBtn.IsVisible,
                            TargetVisible = targetBtn.IsVisible
                        });
                    }
                }
            }

            return diffs;
        }

        /// <summary>
        /// 对比字段权限
        /// </summary>
        private List<FieldPermissionDiff> CompareFields(RolePermissionCacheItem source, RolePermissionCacheItem target)
        {
            var diffs = new List<FieldPermissionDiff>();

            var allFieldIds = source.FieldPermissions.Select(f => f.FieldInfoId)
                .Union(target.FieldPermissions.Select(f => f.FieldInfoId))
                .Distinct()
                .ToList();

            foreach (var fieldId in allFieldIds)
            {
                var sourceField = source.FieldPermissions.FirstOrDefault(f => f.FieldInfoId == fieldId);
                var targetField = target.FieldPermissions.FirstOrDefault(f => f.FieldInfoId == fieldId);

                if (sourceField == null && targetField != null)
                {
                    diffs.Add(new FieldPermissionDiff
                    {
                        FieldId = fieldId,
                        FieldName = targetField.FieldName,
                        MenuId = targetField.MenuId,
                        DiffType = DiffType.Added,
                        SourceVisible = false,
                        TargetVisible = targetField.IsVisible
                    });
                }
                else if (sourceField != null && targetField == null)
                {
                    diffs.Add(new FieldPermissionDiff
                    {
                        FieldId = fieldId,
                        FieldName = sourceField.FieldName,
                        MenuId = sourceField.MenuId,
                        DiffType = DiffType.Removed,
                        SourceVisible = sourceField.IsVisible,
                        TargetVisible = false
                    });
                }
                else if (sourceField != null && targetField != null)
                {
                    if (sourceField.IsVisible != targetField.IsVisible)
                    {
                        diffs.Add(new FieldPermissionDiff
                        {
                            FieldId = fieldId,
                            FieldName = sourceField.FieldName,
                            MenuId = sourceField.MenuId,
                            DiffType = DiffType.Modified,
                            SourceVisible = sourceField.IsVisible,
                            TargetVisible = targetField.IsVisible
                        });
                    }
                }
            }

            return diffs;
        }

        /// <summary>
        /// 对比行权限
        /// </summary>
        private List<RowAuthDiff> CompareRowAuths(RolePermissionCacheItem source, RolePermissionCacheItem target)
        {
            var diffs = new List<RowAuthDiff>();

            var allRowAuthIds = source.RowAuthPermissions.Select(r => r.PolicyId)
                .Union(target.RowAuthPermissions.Select(r => r.PolicyId))
                .Distinct()
                .ToList();

            foreach (var rowAuthId in allRowAuthIds)
            {
                var sourceRow = source.RowAuthPermissions.FirstOrDefault(r => r.PolicyId == rowAuthId);
                var targetRow = target.RowAuthPermissions.FirstOrDefault(r => r.PolicyId == rowAuthId);

                if (sourceRow == null && targetRow != null)
                {
                    diffs.Add(new RowAuthDiff
                    {
                        RowAuthId = rowAuthId,
                        BizType = targetRow.BizType,
                        BizName = targetRow.BizName,
                        DiffType = DiffType.Added,
                        SourceCondition = null,
                        TargetCondition = targetRow.Condition
                    });
                }
                else if (sourceRow != null && targetRow == null)
                {
                    diffs.Add(new RowAuthDiff
                    {
                        RowAuthId = rowAuthId,
                        BizType = sourceRow.BizType,
                        BizName = sourceRow.BizName,
                        DiffType = DiffType.Removed,
                        SourceCondition = sourceRow.Condition,
                        TargetCondition = null
                    });
                }
                else if (sourceRow != null && targetRow != null)
                {
                    if (sourceRow.Condition != targetRow.Condition)
                    {
                        diffs.Add(new RowAuthDiff
                        {
                            RowAuthId = rowAuthId,
                            BizType = sourceRow.BizType,
                            BizName = sourceRow.BizName,
                            DiffType = DiffType.Modified,
                            SourceCondition = sourceRow.Condition,
                            TargetCondition = targetRow.Condition
                        });
                    }
                }
            }

            return diffs;
        }

        /// <summary>
        /// 计算统计信息
        /// </summary>
        private void CalculateStatistics(PermissionDiffResult result)
        {
            result.TotalDifferences = result.MenuDiffs.Count + result.ButtonDiffs.Count +
                                     result.FieldDiffs.Count + result.RowAuthDiffs.Count;

            result.AddedCount = result.MenuDiffs.Count(d => d.DiffType == DiffType.Added) +
                               result.ButtonDiffs.Count(d => d.DiffType == DiffType.Added) +
                               result.FieldDiffs.Count(d => d.DiffType == DiffType.Added) +
                               result.RowAuthDiffs.Count(d => d.DiffType == DiffType.Added);

            result.RemovedCount = result.MenuDiffs.Count(d => d.DiffType == DiffType.Removed) +
                                 result.ButtonDiffs.Count(d => d.DiffType == DiffType.Removed) +
                                 result.FieldDiffs.Count(d => d.DiffType == DiffType.Removed) +
                                 result.RowAuthDiffs.Count(d => d.DiffType == DiffType.Removed);

            result.ModifiedCount = result.MenuDiffs.Count(d => d.DiffType == DiffType.Modified) +
                                  result.ButtonDiffs.Count(d => d.DiffType == DiffType.Modified) +
                                  result.FieldDiffs.Count(d => d.DiffType == DiffType.Modified) +
                                  result.RowAuthDiffs.Count(d => d.DiffType == DiffType.Modified);
        }

        /// <summary>
        /// 生成差异报告
        /// </summary>
        public string GenerateDiffReport(PermissionDiffResult result)
        {
            var report = $"权限差异报告 ({result.ComparisonTime:yyyy-MM-dd HH:mm:ss})\n";
            report += new string('=', 60) + "\n\n";

            if (result.IsCurrentVsOriginal)
            {
                report += $"对比对象: 角色 [{result.SourceRoleId}] 的当前权限 vs 原始权限\n";
            }
            else
            {
                report += $"对比对象: 角色 [{result.SourceRoleId}] vs 角色 [{result.TargetRoleId}]\n";
            }

            report += $"总差异数: {result.TotalDifferences}\n";
            report += $"  - 新增: {result.AddedCount}\n";
            report += $"  - 删除: {result.RemovedCount}\n";
            report += $"  - 修改: {result.ModifiedCount}\n\n";

            // 菜单差异
            if (result.MenuDiffs.Count > 0)
            {
                report += "【菜单权限差异】\n";
                report += new string('-', 40) + "\n";
                foreach (var diff in result.MenuDiffs)
                {
                    report += $"  {GetDiffIcon(diff.DiffType)} [{diff.MenuId}] {diff.MenuName}\n";
                    report += $"      源: {(diff.SourceVisible ? "可见" : "隐藏")} -> 目标: {(diff.TargetVisible ? "可见" : "隐藏")}\n";
                }
                report += "\n";
            }

            // 按钮差异
            if (result.ButtonDiffs.Count > 0)
            {
                report += "【按钮权限差异】\n";
                report += new string('-', 40) + "\n";
                foreach (var diff in result.ButtonDiffs)
                {
                    report += $"  {GetDiffIcon(diff.DiffType)} [{diff.ButtonId}] {diff.ButtonName}\n";
                    report += $"      源: {(diff.SourceVisible ? "可见" : "隐藏")} -> 目标: {(diff.TargetVisible ? "可见" : "隐藏")}\n";
                }
                report += "\n";
            }

            // 字段差异
            if (result.FieldDiffs.Count > 0)
            {
                report += "【字段权限差异】\n";
                report += new string('-', 40) + "\n";
                foreach (var diff in result.FieldDiffs)
                {
                    report += $"  {GetDiffIcon(diff.DiffType)} [{diff.FieldId}] {diff.FieldName}\n";
                    report += $"      源: {(diff.SourceVisible ? "可见" : "隐藏")} -> 目标: {(diff.TargetVisible ? "可见" : "隐藏")}\n";
                }
                report += "\n";
            }

            // 行权限差异
            if (result.RowAuthDiffs.Count > 0)
            {
                report += "【行权限差异】\n";
                report += new string('-', 40) + "\n";
                foreach (var diff in result.RowAuthDiffs)
                {
                    report += $"  {GetDiffIcon(diff.DiffType)} [{diff.RowAuthId}] {diff.BizName}\n";
                    if (diff.DiffType != DiffType.Added)
                        report += $"      源条件: {diff.SourceCondition ?? "无"}\n";
                    if (diff.DiffType != DiffType.Removed)
                        report += $"      目标条件: {diff.TargetCondition ?? "无"}\n";
                }
                report += "\n";
            }

            return report;
        }

        /// <summary>
        /// 获取差异图标
        /// </summary>
        private string GetDiffIcon(DiffType diffType)
        {
            switch (diffType)
            {
                case DiffType.Added:
                    return "[+]";
                case DiffType.Removed:
                    return "[-]";
                case DiffType.Modified:
                    return "[~]";
                default:
                    return "[?]";
            }
        }
    }

    /// <summary>
    /// 权限差异结果
    /// </summary>
    public class PermissionDiffResult
    {
        /// <summary>
        /// 源角色ID
        /// </summary>
        public long SourceRoleId { get; set; }

        /// <summary>
        /// 目标角色ID
        /// </summary>
        public long TargetRoleId { get; set; }

        /// <summary>
        /// 对比时间
        /// </summary>
        public DateTime ComparisonTime { get; set; }

        /// <summary>
        /// 是否为当前权限与原始权限对比
        /// </summary>
        public bool IsCurrentVsOriginal { get; set; }

        /// <summary>
        /// 菜单权限差异
        /// </summary>
        public List<MenuPermissionDiff> MenuDiffs { get; set; }

        /// <summary>
        /// 按钮权限差异
        /// </summary>
        public List<ButtonPermissionDiff> ButtonDiffs { get; set; }

        /// <summary>
        /// 字段权限差异
        /// </summary>
        public List<FieldPermissionDiff> FieldDiffs { get; set; }

        /// <summary>
        /// 行权限差异
        /// </summary>
        public List<RowAuthDiff> RowAuthDiffs { get; set; }

        /// <summary>
        /// 总差异数
        /// </summary>
        public int TotalDifferences { get; set; }

        /// <summary>
        /// 新增数量
        /// </summary>
        public int AddedCount { get; set; }

        /// <summary>
        /// 删除数量
        /// </summary>
        public int RemovedCount { get; set; }

        /// <summary>
        /// 修改数量
        /// </summary>
        public int ModifiedCount { get; set; }
    }

    /// <summary>
    /// 差异类型
    /// </summary>
    public enum DiffType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Added,

        /// <summary>
        /// 删除
        /// </summary>
        Removed,

        /// <summary>
        /// 修改
        /// </summary>
        Modified
    }

    /// <summary>
    /// 菜单权限差异
    /// </summary>
    public class MenuPermissionDiff
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public DiffType DiffType { get; set; }
        public bool SourceVisible { get; set; }
        public bool TargetVisible { get; set; }
    }

    /// <summary>
    /// 按钮权限差异
    /// </summary>
    public class ButtonPermissionDiff
    {
        public long ButtonId { get; set; }
        public string ButtonName { get; set; }
        public long MenuId { get; set; }
        public DiffType DiffType { get; set; }
        public bool SourceVisible { get; set; }
        public bool TargetVisible { get; set; }
    }

    /// <summary>
    /// 字段权限差异
    /// </summary>
    public class FieldPermissionDiff
    {
        public long FieldId { get; set; }
        public string FieldName { get; set; }
        public long MenuId { get; set; }
        public DiffType DiffType { get; set; }
        public bool SourceVisible { get; set; }
        public bool TargetVisible { get; set; }
    }

    /// <summary>
    /// 行权限差异
    /// </summary>
    public class RowAuthDiff
    {
        public long RowAuthId { get; set; }
        public string BizType { get; set; }
        public string BizName { get; set; }
        public DiffType DiffType { get; set; }
        public string SourceCondition { get; set; }
        public string TargetCondition { get; set; }
    }
}
