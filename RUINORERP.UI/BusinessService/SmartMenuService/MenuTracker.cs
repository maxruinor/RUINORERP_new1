using LiveChartsCore.Geo;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using RUINORERP.Common.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.UI.UControls;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.BusinessService.SmartMenuService
{

    [NoWantIOC]
    public class MenuTracker
    {
        public ApplicationContext _appContext { get; set; }

        /// <summary>
        /// 菜单ID，相关信息
        /// </summary>
        private readonly ConcurrentDictionary<long, MenuUseInfo> _menuUsage = new();
        private DateTime _lastSaveTime = DateTime.MinValue;
        private readonly object _saveLock = new();

        // 用于比较的原始数据快照
        private Dictionary<long, int> _originalSnapshot = new();

        public MenuTracker(ApplicationContext appContext)
        {
            _appContext = appContext;
            LoadFromDb();
        }

        // 加载用户历史数据
        public void LoadFromDb()
        {
            if (_appContext.CurrentUser_Role_Personalized == null ||
                string.IsNullOrEmpty(_appContext.CurrentUser_Role_Personalized.UserFavoriteMenu))
            {
                return;
            }

            try
            {
                var menuInfos = JsonConvert.DeserializeObject<List<MenuUseInfo>>(
                    _appContext.CurrentUser_Role_Personalized.UserFavoriteMenu);

                if (menuInfos != null)
                {
                    // 创建原始快照（只保存MenuId和Frequency）
                    _originalSnapshot = menuInfos.ToDictionary(
                        item => item.MenuId,
                        item => item.Frequency);

                    // 加载到内存
                    foreach (var item in menuInfos)
                    {
                        _menuUsage[item.MenuId] = item;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError($"加载用户菜单偏好数据时出错: {ex.Message}");
            }
        }

        // 记录菜单使用
        public void RecordMenuUsage(long menuId)
        {
            _menuUsage.AddOrUpdate(menuId,
                id => new MenuUseInfo
                {
                    MenuId = id,
                    Frequency = 1,
                    LastClickTime = DateTime.Now
                },
                (id, existing) =>
                {
                    existing.Frequency++;
                    existing.LastClickTime = DateTime.Now;
                    return existing;
                });

            AutoSave();
        }

        // 获取Top20常用菜单
        public List<long> GetTopMenus()
        {
            return _menuUsage.Values
                .OrderByDescending(x => x.Frequency)
                .ThenByDescending(x => x.LastClickTime)
                .Take(20)
                .Select(x => x.MenuId)
                .ToList();
        }

        // 获取所有菜单使用数据（用于保存）
        private List<MenuUseInfo> GetAllMenuUsage()
        {
            return _menuUsage.Values.ToList();
        }

        // 检查数据是否有变化（忽略LastClickTime）
        private bool HasDataChanged()
        {
            // 检查数量变化
            if (_menuUsage.Count != _originalSnapshot.Count)
                return true;

            // 创建当前快照（只包含MenuId和Frequency）
            var currentSnapshot = _menuUsage.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.Frequency);

            // 比较每个菜单项的变化
            foreach (var kv in currentSnapshot)
            {
                if (!_originalSnapshot.TryGetValue(kv.Key, out int originalFrequency) ||
                    originalFrequency != kv.Value)
                {
                    return true;
                }
            }

            return false;
        }

        // 自动保存（5分钟检查）
        public void AutoSave()
        {
            if ((DateTime.Now - _lastSaveTime).TotalMinutes < 5)
                return;

            lock (_saveLock)
            {
                if ((DateTime.Now - _lastSaveTime).TotalMinutes < 5)
                    return;

                // 只有数据变化时才保存
                if (HasDataChanged())
                {
                    SaveToDb();
                    _lastSaveTime = DateTime.Now;
                }
            }
        }

        // 保存到数据库
        public async Task<bool> SaveToDb()
        {
            if (!_menuUsage.Any())
                return false;

            try
            {
                // 更新JSON数据
                var jsonData = JsonConvert.SerializeObject(GetAllMenuUsage());
                _appContext.CurrentUser_Role_Personalized.UserFavoriteMenu = jsonData;

                // 更新数据库
                long id = _appContext.CurrentUser_Role_Personalized.UserPersonalizedID;
                int rs = await _appContext.Db.Updateable<tb_UserPersonalized>()
                   .SetColumns(it => it.UserFavoriteMenu == jsonData)
                   .Where(it => it.UserPersonalizedID == id)
                   .ExecuteCommandAsync();

                if (rs > 0)
                {
                    // 更新成功后，更新原始快照
                    _originalSnapshot = _menuUsage.ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Frequency);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError($"保存用户菜单偏好时出错: {ex.Message}");
            }
            return false;
        }

        // 显式保存（用于程序退出时）
        public void ForceSave()
        {
            lock (_saveLock)
            {
                // 只有数据变化时才保存
                if (HasDataChanged())
                {
                    SaveToDb();
                }
            }
        }
    }

}
