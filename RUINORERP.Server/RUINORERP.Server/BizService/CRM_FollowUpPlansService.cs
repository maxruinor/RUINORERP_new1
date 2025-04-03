using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.ServerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RUINORERP.Server.BizService
{
    public class CRM_FollowUpPlansService
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;
        public static ILogger<CRM_FollowUpPlansService> _logger { get; set; }
        public CRM_FollowUpPlansService(ILogger<CRM_FollowUpPlansService> logger, ApplicationContext _AppContextData, IUnitOfWorkManage unitOfWorkManage)
        {
            _logger = logger;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;
        }
        public async Task<int> AutoUdateCRMPlanStatus()
        {
            //
            List<tb_CRM_FollowUpPlans> followUpPlans = await _unitOfWorkManage.GetDbClient().Queryable<tb_CRM_FollowUpPlans>()
                .Includes(c => c.tb_CRM_FollowUpRecordses)
                .Where(c => c.PlanEndDate < System.DateTime.Today && (c.PlanStatus != (int)FollowUpPlanStatus.已完成 || c.PlanStatus == (int)FollowUpPlanStatus.未执行))
                .ToListAsync();

            // 假设配置的延期天数存储在 DelayDays 变量中
            int DelayDays = 3;
            List<tb_CRM_FollowUpPlans> needUpdateList = new List<tb_CRM_FollowUpPlans>();

            for (int i = 0; i < followUpPlans.Count; i++)
            {
                //if (followUpPlans[i].PlanStartDate >= System.DateTime.Today)
                //{
                //    followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.进行中; 
                //    needUpdateList.Add(followUpPlans[i]);
                //}
                if (followUpPlans[i].tb_CRM_FollowUpRecordses.Count > 0)
                {
                    //如果当前计划有跟进记录了，并且结束时间超过了当前时间。将这个计划设置为已经完成
                    if (followUpPlans[i].PlanEndDate > System.DateTime.Today)
                    {
                        followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.已完成;
                        needUpdateList.Add(followUpPlans[i]);
                    }
                    
                }
                else
                {
                    if (followUpPlans[i].PlanEndDate < System.DateTime.Today)
                    {
                        TimeSpan timeSinceEnd = System.DateTime.Today - followUpPlans[i].PlanEndDate;
                        if (timeSinceEnd.TotalDays <= DelayDays)
                        {
                            followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.延期中;
                            needUpdateList.Add(followUpPlans[i]);
                        }
                        else
                        {
                            followUpPlans[i].PlanStatus = (int)FollowUpPlanStatus.未执行;
                            needUpdateList.Add(followUpPlans[i]);
                        }

                        //发送消息给执行人。
                        //您要有执行计划已经超时


                    }
                }


            }
            int updateCount = await _unitOfWorkManage.GetDbClient().Updateable<tb_CRM_FollowUpPlans>(needUpdateList).UpdateColumns(it => new { it.PlanStatus }).ExecuteCommandAsync();
            return updateCount;
        }
    }
}
