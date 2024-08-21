﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.WF.WFApproval.Steps
{
    /// <summary>
    /// InitialStep 初始化,通知审批人有新的请求
    /// </summary>
    public class InitialStep : StepBodyAsync
    {
       // private readonly IApplicationDbContext _context;
       // private readonly IMailService _mailService;
       // private readonly ILogger<InitialStep> _logger;
        public string WorkId { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string DocumentName { get; set; }
        public int DocumentId { get; set; }

        //public InitialStep(
        //    IApplicationDbContext context,
        //    IMailService mailService,
        //    ILogger<InitialStep> logger)
        //{
        //    _context = context;
        //    _mailService = mailService;
        //    _logger = logger;
        //}
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            Body = $"Please,Approval this document:{DocumentName},workflowid:{WorkId},URL:http://....";
            Subject = $"a new document approval request";
            await Task.Delay(1);
            //var request = new MailRequest();
            //request.To = To;
            //request.Subject = Subject;
            //request.Body = Body;
            //await _mailService.SendAsync(request);
            Console.WriteLine($"Send Mail to {To},{Body}");
            //_logger.LogInformation($"Send Mail to {To},{Body}");
           // var approval = _context.ApprovalDatas.FirstOrDefault(x => x.WorkflowId == WorkId);
           // if (approval != null)
           // {
           //     approval.Status = "Pending";
           //     approval.RequestDateTime = DateTime.Now;
           //     await _context.SaveChangesAsync(default);
          //  }
            return ExecutionResult.Next();
        }
    }
}
