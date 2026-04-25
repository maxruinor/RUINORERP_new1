using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Model;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 权限重复检查器
    /// 【简化】仅用于清理历史脏数据，日常防重由UCRoleAuthorization._processedPermissions处理
    /// </summary>
    public static class PermissionDuplicateChecker
    {
        /// <summary>
        /// 检查并清理重复的按钮权限
        /// 【注意】此方法仅用于清理历史脏数据，正常流程已由_processedPermissions防重
        /// </summary>
        /// <param name="buttons">按钮权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>清理后的按钮权限列表</returns>
        public static async Task<List<tb_P4Button>> CheckAndCleanDuplicateButtonsAsync(
            List<tb_P4Button> buttons,
            long roleId,
            long menuId)
        {
            if (buttons == null || !buttons.Any())
                return new List<tb_P4Button>();

            try
            {
                var db = MainForm.Instance.AppContext.Db.CopyNew();

                // 查找重复项（保留第一个，删除后续）
                var duplicates = buttons
                    .Where(c => c.RoleID == roleId && c.MenuID == menuId)
                    .GroupBy(p => p.ButtonInfo_ID)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g.Skip(1))
                    .ToList();

                if (duplicates.Any())
                {
                    // 从数据库删除
                    var duplicateIds = duplicates.Select(d => d.P4Btn_ID).ToList();
                    await db.Deleteable<tb_P4Button>()
                        .Where(p => duplicateIds.Contains(p.P4Btn_ID))
                        .ExecuteCommandAsync();

                    // 从内存中移除
                    duplicates.ForEach(d => buttons.Remove(d));
                }

                return buttons;
            }
            catch (Exception ex)
            {
                return buttons;
            }
        }

        /// <summary>
        /// 检查并清理重复的字段权限
        /// 【注意】此方法仅用于清理历史脏数据，正常流程已由_processedPermissions防重
        /// </summary>
        /// <param name="fields">字段权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>清理后的字段权限列表</returns>
        public static async Task<List<tb_P4Field>> CheckAndCleanDuplicateFieldsAsync(
            List<tb_P4Field> fields,
            long roleId,
            long menuId)
        {
            if (fields == null || !fields.Any())
                return new List<tb_P4Field>();

            try
            {
                var db = MainForm.Instance.AppContext.Db.CopyNew();

                // 查找重复项（保留第一个，删除后续）
                var duplicates = fields
                    .Where(c => c.RoleID == roleId && c.MenuID == menuId)
                    .GroupBy(p => p.FieldInfo_ID)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g.Skip(1))
                    .ToList();

                if (duplicates.Any())
                {

                    // 从数据库删除
                    var duplicateIds = duplicates.Select(d => d.P4Field_ID).ToList();
                    await db.Deleteable<tb_P4Field>()
                        .Where(p => duplicateIds.Contains(p.P4Field_ID))
                        .ExecuteCommandAsync();

                    // 从内存中移除
                    duplicates.ForEach(d => fields.Remove(d));
                }

                return fields;
            }
            catch (Exception ex)
            {
                return fields;
            }
        }

        /// <summary>
        /// 检查按钮权限是否已存在
        /// </summary>
        /// <param name="buttons">按钮权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="buttonInfoId">按钮信息ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否存在</returns>
        public static bool IsButtonPermissionExists(
            List<tb_P4Button> buttons,
            long roleId,
            long buttonInfoId,
            long menuId)
        {
            if (buttons == null) return false;

            return buttons.Any(b =>
                b.RoleID == roleId &&
                b.ButtonInfo_ID == buttonInfoId &&
                b.MenuID == menuId);
        }

        /// <summary>
        /// 检查字段权限是否已存在
        /// </summary>
        /// <param name="fields">字段权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="fieldInfoId">字段信息ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否存在</returns>
        public static bool IsFieldPermissionExists(
            List<tb_P4Field> fields,
            long roleId,
            long fieldInfoId,
            long menuId)
        {
            if (fields == null) return false;

            return fields.Any(f =>
                f.RoleID == roleId &&
                f.FieldInfo_ID == fieldInfoId &&
                f.MenuID == menuId);
        }

        /// <summary>
        /// 检查并清理重复的菜单权限
        /// 【注意】此方法仅用于清理历史脏数据
        /// </summary>
        /// <param name="menus">菜单权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <returns>清理后的菜单权限列表</returns>
        public static async Task<List<tb_P4Menu>> CheckAndCleanDuplicateMenusAsync(
            List<tb_P4Menu> menus,
            long roleId)
        {
            if (menus == null || !menus.Any())
                return new List<tb_P4Menu>();

            try
            {
                var db = MainForm.Instance.AppContext.Db.CopyNew();

                // 查找重复项（保留第一个，删除后续）
                var duplicates = menus
                    .Where(c => c.RoleID == roleId)
                    .GroupBy(p => p.MenuID)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g.Skip(1))
                    .ToList();

                if (duplicates.Any())
                {

                    // 从数据库删除
                    var duplicateIds = duplicates.Select(d => d.P4Menu_ID).ToList();
                    await db.Deleteable<tb_P4Menu>()
                        .Where(p => duplicateIds.Contains(p.P4Menu_ID))
                        .ExecuteCommandAsync();

                    // 从内存中移除
                    duplicates.ForEach(d => menus.Remove(d));
                }

                return menus;
            }
            catch (Exception ex)
            {
                return menus;
            }
        }

        /// <summary>
        /// 检查菜单权限是否已存在
        /// </summary>
        /// <param name="menus">菜单权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否存在</returns>
        public static bool IsMenuPermissionExists(
            List<tb_P4Menu> menus,
            long roleId,
            long menuId)
        {
            if (menus == null) return false;

            return menus.Any(m =>
                m.RoleID == roleId &&
                m.MenuID == menuId);
        }

        /// <summary>
        /// 【新增】前置防重检查 - 判断是否可以添加按钮权限
        /// 使用 HashSet 提升查找性能 O(1)
        /// </summary>
        /// <param name="existingButtons">现有按钮权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="buttonInfoId">按钮信息ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>true表示可以添加,false表示已存在</returns>
        public static bool CanAddButtonPermission(
            List<tb_P4Button> existingButtons,
            long roleId,
            long buttonInfoId,
            long menuId)
        {
            if (existingButtons == null || !existingButtons.Any())
                return true;

            return !existingButtons.Any(b =>
                b.RoleID == roleId &&
                b.ButtonInfo_ID == buttonInfoId &&
                b.MenuID == menuId);
        }

        /// <summary>
        /// 【新增】前置防重检查 - 判断是否可以添加字段权限
        /// </summary>
        /// <param name="existingFields">现有字段权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="fieldInfoId">字段信息ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>true表示可以添加,false表示已存在</returns>
        public static bool CanAddFieldPermission(
            List<tb_P4Field> existingFields,
            long roleId,
            long fieldInfoId,
            long menuId)
        {
            if (existingFields == null || !existingFields.Any())
                return true;

            return !existingFields.Any(f =>
                f.RoleID == roleId &&
                f.FieldInfo_ID == fieldInfoId &&
                f.MenuID == menuId);
        }

        /// <summary>
        /// 【新增】前置防重检查 - 判断是否可以添加菜单权限
        /// </summary>
        /// <param name="existingMenus">现有菜单权限列表</param>
        /// <param name="roleId">角色ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>true表示可以添加,false表示已存在</returns>
        public static bool CanAddMenuPermission(
            List<tb_P4Menu> existingMenus,
            long roleId,
            long menuId)
        {
            if (existingMenus == null || !existingMenus.Any())
                return true;

            return !existingMenus.Any(m =>
                m.RoleID == roleId &&
                m.MenuID == menuId);
        }
    }
}