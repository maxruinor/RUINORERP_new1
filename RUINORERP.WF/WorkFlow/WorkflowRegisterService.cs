using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Services.DefinitionStorage;

 
namespace RUINORERP.WF.WorkFlow
{


    /// <summary>
    /// 工作流注册BY josn ,要用单例模式注册，用一个列表保存注册过的流程。保证不会重复注册
    /// </summary>
    public class WorkflowRegisterService
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public IUnitOfWorkManage _unitOfWorkManage;
        public ILogger<BaseController> _logger;
        public ApplicationContext _appContext;
     

 


        public WorkflowRegisterService(ILogger<BaseController> logger,
          
            IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null)
        {
            _logger = logger;
           
            _unitOfWorkManage = unitOfWorkManage;
            _appContext = appContext;
        }


        public void RegisterWorkflowDefinition(string json, string workflowid = null)
        {
            if (workflowid == null)
            {
                return;
            }
            // 检查是否已经注册过相同的工作流定义
            if (!IsWorkflowDefinitionRegistered(json, workflowid))
            {
                _appContext.definitionLoader.LoadDefinition(json, Deserializers.Json);
                _appContext.RegistedWorkflowList.TryAdd(workflowid, json);
            }
        }


        private bool IsWorkflowDefinitionRegistered(string json, string workflowid = null)
        {
        
            // 实现检查逻辑，确保不会重复注册相同的工作流定义
            // 例如，可以比较工作流定义的哈希值或唯一标识符
            if (_appContext.RegistedWorkflowList.ContainsKey(workflowid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
