using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Server.SmartReminder.InvReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    public interface IRuleEditorService
    {
        Task<IReminderRule> SaveRuleAsync(tb_ReminderRule model);
        Task ValidateRuleSyntaxAsync(string condition, string type);
    }
    public interface IAlertStrategy
    {
        Task CheckAsync(tb_ReminderRule policy, tb_Inventory stock);
        int Priority { get; }
    }
    public interface IInventoryMonitor
    {
        bool IsRunning { get; }
        bool PerformQuickCheck();
        void StartMonitoring(TimeSpan interval);
        Task CheckInventoryAsync();
        void AddStrategy(IAlertStrategy strategy);
    }
    // 4. 通知服务实现
    public interface INotificationService
    {
        public Task SendAlertAsync(tb_ReminderRule policy, string message);
    }

    public interface IHealthCheck
    {

    }

}
