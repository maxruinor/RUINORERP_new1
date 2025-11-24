using System;
using System.ComponentModel;

namespace RUINORERP.UI.WorkFlowDesigner
{
    public enum ApprovalMode
    {
        Single,     // 单人审批
        All,        // 会签（需所有人同意）
        Any         // 或签（任意一人同意即可）
    }
    public enum ConditionType
    {
        [Description("二元决策")]
        BinaryDecision,

        [Description("多路分支")]
        MultipleBranches,

        [Description("范围检查")]
        RangeCheck,

        [Description("成员资格检查")]
        MembershipCheck,

        [Description("状态检查")]
        StatusCheck,

        [Description("时间条件")]
        TimeCondition,

        [Description("数据存在性")]
        DataExistence,

        [Description("优先级判断")]
        PriorityJudgment,

        [Description("顺序依赖")]
        SequenceDependency,

        [Description("异常处理")]
        ExceptionHandling,

        [Description("并行处理决策")]
        ParallelProcessingDecision,

        [Description("循环控制")]
        LoopControl,

        [Description("资源可用性")]
        ResourceAvailability,

        [Description("合规性检查")]
        ComplianceCheck,

        [Description("自定义逻辑")]
        CustomLogic
    }
    public enum WFStepType
    {
        提交,
        审核,
        结案,
    }


    public enum WFNodeType
    {   
        Start,
        End,
        Step,
        Connector,
        Shape,
    }

    public enum Samples
    {
        GraphLibInterfaces,
        GraphLibClasses,
        Background,
        RandomTree,
        NoNewShapes,
        TreeAsCode,
        NoLinking,
        Controls,
        Layout,
        Layering,
        ZOrder,
        FancyConnections,
        NoNewConnections,
        ShapeEvents,
        Snap,
        ItemCannotMove,
        ClassInheritance
    }
}
