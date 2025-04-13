using System;
using System.ComponentModel;

namespace RUINORERP.UI.WorkFlowDesigner
{
    public enum ApprovalMode
    {
        Single,     // ��������
        All,        // ��ǩ����������ͬ�⣩
        Any         // ��ǩ������һ��ͬ�⼴�ɣ�
    }
    public enum ConditionType
    {
        [Description("��Ԫ����")]
        BinaryDecision,

        [Description("��·��֧")]
        MultipleBranches,

        [Description("��Χ���")]
        RangeCheck,

        [Description("��Ա�ʸ���")]
        MembershipCheck,

        [Description("״̬���")]
        StatusCheck,

        [Description("ʱ������")]
        TimeCondition,

        [Description("���ݴ�����")]
        DataExistence,

        [Description("���ȼ��ж�")]
        PriorityJudgment,

        [Description("˳������")]
        SequenceDependency,

        [Description("�쳣����")]
        ExceptionHandling,

        [Description("���д������")]
        ParallelProcessingDecision,

        [Description("ѭ������")]
        LoopControl,

        [Description("��Դ������")]
        ResourceAvailability,

        [Description("�Ϲ��Լ��")]
        ComplianceCheck,

        [Description("�Զ����߼�")]
        CustomLogic
    }
    public enum WFStepType
    {
        �ύ,
        ���,
        �᰸,
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
