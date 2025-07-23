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
        public ApplicationContext _appContext { set; get; }
        /// <summary>
        /// 菜单ID，相关信息
        /// </summary>
        private readonly ConcurrentDictionary<long, MenuUseInfo> _menuUsage = new();
        private DateTime _lastSaveTime = DateTime.MinValue;
        private readonly object _saveLock = new();
        private int _totalClicks = 0;
        public MenuTracker(ApplicationContext appContext)
        {
            _appContext = appContext;

        }

        // 加载用户历史数据
        public async void LoadFromDb()
        {
            if (_appContext.CurrentUser_Role_Personalized == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(_appContext.CurrentUser_Role_Personalized.UserFavoriteMenu)) return;

            //如果有变化，则先保存再加载

            string json = JsonConvert.SerializeObject(GetAllMenuUsage());
            if (_appContext.CurrentUser_Role_Personalized.UserFavoriteMenu != json && _menuUsage.Count > 0)
            {
                await SaveToDb();
            }

            try
            {
                var menuInfos = JsonConvert.DeserializeObject<List<MenuUseInfo>>(_appContext.CurrentUser_Role_Personalized.UserFavoriteMenu);
                if (menuInfos != null)
                {
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

            Interlocked.Increment(ref _totalClicks);
            AutoSave();
        }


        // 获取Top10菜单
        public List<long> GetTopMenus()
        {
            return _menuUsage.Values
                .OrderByDescending(x => x.Frequency)
                .ThenByDescending(x => x.LastClickTime)
                .Take(10)
                .Select(x => x.MenuId)
                .ToList();
        }

        // 获取所有菜单使用数据（用于保存）
        private List<MenuUseInfo> GetAllMenuUsage()
        {
            return _menuUsage.Values.ToList();
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

                SaveToDb();
                _lastSaveTime = DateTime.Now;
            }
        }

        // 保存到数据库
        public async Task<bool> SaveToDb()
        {
            if (!_menuUsage.Any()) return false;
            try
            {
                // 更新JSON数据
                _appContext.CurrentUser_Role_Personalized.UserFavoriteMenu = JsonConvert.SerializeObject(GetAllMenuUsage());

                // 更新数据库
                //var rs = await _appContext.Db.Updateable(_appContext.CurrentUser_Role_Personalized)
                //       .UpdateColumns(it => new
                //       {
                //           it.UserFavoriteMenu
                //       })
                //       .Where(it => it.UserPersonalizedID == _appContext.CurrentUser_Role_Personalized.UserPersonalizedID)
                //       .ExecuteCommandAsync();
                var id = _appContext.CurrentUser_Role_Personalized.UserPersonalizedID;
                var rs = await _appContext.Db.Updateable<tb_UserPersonalized>()
               .SetColumns(it => it.UserFavoriteMenu == _appContext.CurrentUser_Role_Personalized.UserFavoriteMenu)
               .Where(it => it.UserPersonalizedID == id)
               .ExecuteCommandAsync();
                return true;
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
                SaveToDb();
            }
        }
    }
}
