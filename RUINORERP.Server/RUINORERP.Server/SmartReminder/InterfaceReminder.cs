﻿using RUINORERP.Global.EnumExt;
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

    public interface IReminderStrategy
    {
        Task<bool> CheckAsync(IReminderRule rule, IReminderContext context);
        int Priority { get; }
        bool CanHandle(ReminderBizType reminderType);
    }
    public interface ISmartReminderMonitor
    {
        bool IsRunning { get; }
        bool PerformQuickCheck();
        Task CheckRemindersAsync();
        void StartMonitoring(TimeSpan interval);
  
        void AddStrategy(IReminderStrategy strategy);
        Task<List<IReminderRule>> GetActiveRulesAsync();
    }
    // 4. 通知服务实现
    public interface INotificationService
    {
        Task SendNotificationAsync(IReminderRule rule, string message, object contextData);
    }

    public interface IHealthCheck
    {

    }



}
