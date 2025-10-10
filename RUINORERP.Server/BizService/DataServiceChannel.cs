using Azure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using RUINORERP.Business;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Model.TransModel;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.Workflow.WFReminder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.BizService
{
    /// <summary>
    /// 数据服务的入口
    /// </summary>
    public class DataServiceChannel
    {

        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;
        public static ILogger<DataServiceChannel> _logger { get; set; }
        public DataServiceChannel(ILogger<DataServiceChannel> logger, ApplicationContext _AppContextData, IUnitOfWorkManage unitOfWorkManage)
        {
            _logger = logger;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;

        }

        public async void LoadCRMFollowUpPlansData(ConcurrentDictionary<long, ReminderData> dictionary)
        {
            //只要是未开始 并且结束时间还没有到。就要加载
            var Plans = await _appContext.Db.Queryable<tb_CRM_FollowUpPlans>()
                .Where(c => c.PlanStatus == (int)FollowUpPlanStatus.未开始)
                .Where(c => c.PlanEndDate >= System.DateTime.Now)
                .OrderBy(c => c.PlanStartDate)
                .ToListAsync();

            foreach (var item in Plans)
            {
                ReminderData reminderBizData = new ReminderData();
                reminderBizData.BizPrimaryKey = item.PlanID;
                reminderBizData.BizType = Global.BizType.CRM跟进计划;
                List<long> receiverIDs = new List<long>();
                receiverIDs.Add(item.Employee_ID);
                reminderBizData.ReceiverEmployeeIDs = receiverIDs;
                //reminderBizData.SenderName = item.;
                reminderBizData.RemindSubject = item.PlanSubject;
                reminderBizData.ReminderContent = item.PlanContent;
                reminderBizData.StartTime = item.PlanStartDate;
                reminderBizData.EndTime = item.PlanEndDate;

                //只要是不有结束，未开始的都要加入到等待列表中。每一分种 检测 开始时间前一天开始提醒
                dictionary.AddOrUpdate(reminderBizData.BizPrimaryKey, reminderBizData, (key, value) => value);

            }

        }


        /// <summary>
        /// 如果有超时提醒（即将超时，已经超时），这里可以延后几天来处理。 超时天数 由全局通用配置模块来完成
        /// </summary>
        /// <param name="reminderData"></param>
        public async void ProcessCRMFollowUpPlansData(ReminderData reminderData, MessageStatus messageStatu)
        {
            if (reminderData.BizType == Global.BizType.CRM跟进计划)
            {
                var plan = await _appContext.Db.Queryable<tb_CRM_FollowUpPlans>()
                    .Includes(c => c.tb_CRM_FollowUpRecordses)
                    .Where(c => c.PlanID == reminderData.BizPrimaryKey).SingleAsync();
                //如果不是手动取消的，则系统会自动根据是不是有记录来判断最终计划的不个完成情况
                if (plan.PlanStatus != (int)FollowUpPlanStatus.已取消)
                {
                    if (plan.tb_CRM_FollowUpRecordses.Count > 0)
                    {
                        plan.PlanStatus = (int)FollowUpPlanStatus.已完成;
                    }
                    else
                    {
                        plan.PlanStatus = (int)FollowUpPlanStatus.未执行;
                    }

                    var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_CRM_FollowUpPlans>(plan).UpdateColumns(it => new { it.PlanStatus }).ExecuteCommandAsync();
                }
            }
        }



    }
}
