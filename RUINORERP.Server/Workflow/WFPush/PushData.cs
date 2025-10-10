using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace RUINORERP.Server.Workflow.WFPush
{
    public class PushData
    {
        public string InputData { get; set; }
    }
   
    ///// <summary>
    ///// 有两种传递方法：使用泛型，从运行工作流时就要传入；使用 object 简单类型，由单独的步骤产生并且传递给下一个节点。所以这里有两个接口？
    ///// </summary>
    ///// <typeparam name="TData"></typeparam>
    //public interface IPushWorkflow<PushData>
    //     where PushData : new()
    //{
    //    /// <summary>
    //    /// 此工作流的唯一标识符
    //    /// </summary>
    //    string Id { get; }

    //    /// <summary>
    //    /// 此工作流的版本
    //    /// </summary>
    //    int Version { get; }
 

    //    /// <summary>
    //    /// 在此方法内构建工作流
    //    /// </summary>
    //    /// <param name="builder"></param>
    //    void Build(IWorkflowBuilder<PushData> builder);
    //}

    //public interface IPushWorkflow : IWorkflow<PushData>
    //{
    //}
}
