using Microsoft.Extensions.Logging;
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
        public void AutoUdateCRMPlanStatus()
        {

        }
    }
}
