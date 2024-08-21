using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.WF.WorkFlow.CustomizedWF.Steps
{




    /// <summary>
    /// 指定用户审批StepBody
    /// </summary>
    public class GeneralAuditingStepBody : StepBody
    {
        private const string ActionName = "AuditEvent";
        protected readonly INotificationPublisher _notificationPublisher;
        protected readonly IAbpPersistenceProvider _abpPersistenceProvider;
        protected readonly UserManager _userManager;

        public readonly IRepository<PersistedWorkflowAuditor, Guid> _auditorRepository;

        public GeneralAuditingStepBody(INotificationPublisher notificationPublisher, UserManager userManager, IAbpPersistenceProvider abpPersistenceProvider,
             IRepository<PersistedWorkflowAuditor, Guid> auditorRepository)
        {
            _notificationPublisher = notificationPublisher;
            _abpPersistenceProvider = abpPersistenceProvider;
            _userManager = userManager;
            _auditorRepository = auditorRepository;
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public long UserId { get; set; }

        //public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        //{
        //    ExecutionResult er = new ExecutionResult();
        //    return  await Task.Run(() => Task.FromResult(er));
        //}

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (!context.ExecutionPointer.EventPublished)
            {
                var workflow = _abpPersistenceProvider.GetPersistedWorkflow(context.Workflow.Id.ToGuid()).Result;
                var workflowDefinition = _abpPersistenceProvider.GetPersistedWorkflowDefinition(context.Workflow.WorkflowDefinitionId, context.Workflow.Version).Result;

                var userIdentityName = _userManager.Users.Where(u => u.Id == workflow.CreatorUserId).Select(u => u.FullName).FirstOrDefault();

                //通知审批人
                _notificationPublisher.PublishTaskAsync(new Abp.Notifications.TaskNotificationData($"【{userIdentityName}】提交的{workflowDefinition.Title}需要您审批！"),
                            userIds: new UserIdentifier[] { new UserIdentifier(workflow.TenantId, UserId) },
                             entityIdentifier: new EntityIdentifier(workflow.GetType(), workflow.Id)
       ).Wait();
                //添加审核人记录
                var auditUserInfo = _userManager.GetUserById(UserId);
                _auditorRepository.Insert(new PersistedWorkflowAuditor() { WorkflowId = workflow.Id, ExecutionPointerId = context.ExecutionPointer.Id, Status = Abp.Entitys.CommEnum.EnumAuditStatus.UnAudited, UserId = UserId, TenantId = workflow.TenantId, UserHeadPhoto = auditUserInfo.HeadImage, UserIdentityName = auditUserInfo.FullName });
                DateTime effectiveDate = DateTime.MinValue;
                return ExecutionResult.WaitForEvent(ActionName, Guid.NewGuid().ToString(), effectiveDate);
            }
            var pass = _auditorRepository.GetAll().Any(u => u.ExecutionPointerId == context.ExecutionPointer.Id && u.UserId == UserId && u.Status == Abp.Entitys.CommEnum.EnumAuditStatus.Pass);

            if (!pass)
            {
                context.Workflow.Status = WorkflowStatus.Complete;
                return ExecutionResult.Next();
            }
            return ExecutionResult.Next();
        }
    }


}