namespace RUINORERP.PacketSpec.Enums
{
    /// <summary>
    /// 服务器指令枚举
    /// </summary>
    public enum ServerCommand : uint
    {
        /// <summary>
        /// 未知指令
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// 认证相关响应 (0xA)
        /// </summary>
        UserLoginResponse = 0xA,
        LoginReply = 0xAA,  // 登录回复
        DuplicateLoginResponse = 0x20,
        
        /// <summary>
        /// 在线状态管理 (0x1, 0x4)
        /// </summary>
        SendOnlineList = 0x1,
        ForceUserLogout = 0x4,
        
        /// <summary>
        /// 消息推送相关 (0x2, 0x3, 0x5, 0x6, 0x7, 0x8, 0x21)
        /// </summary>
        ForwardPopupMessage = 0x2,
        ForwardMessageResult = 0x3,
        SendNotification = 0x5,
        NotifyApprover = 0x6,
        NotifyCompletion = 0x7,
        NotifyInitiator = 0x8,
        MessageResponse = 0x2A,  // 消息响应
        WorkflowReminderPush = 0x21,
        
        /// <summary>
        /// 缓存数据管理 (0x10, 0x11, 0x14, 0x16, 0x18, 0x22)
        /// </summary>
        SendCacheData = 0x10,
        SendCacheDataList = 0x11,
        ForwardUpdateCache = 0x14,
        SendCacheInfoList = 0x16,
        ForwardDeleteCache = 0x18,
        ForwardUpdateDynamicConfig = 0x22,
        
        /// <summary>
        /// 异常处理 (0x12, 0x13)
        /// </summary>
        ForwardException = 0x12,
        ForwardAssistance = 0x13,
        
        /// <summary>
        /// 系统管理 (0x9, 0x15, 0x23, 0x24, 0x91, 0x94, 0x95, 0x96)
        /// </summary>
        PushVersionUpdate = 0x9,
        ForwardDocumentLock = 0x15,
        CompositeLockProcessing = 0x23,
        UpdateGlobalConfig = 0x24,
        SwitchServer = 0x91,
        Shutdown = 0x94,
        DeleteColumnConfig = 0x95,
        WorkflowDataPush = 0x96,
        
        /// <summary>
        /// 复合指令处理 (0x92, 0x97, 0x98)
        /// </summary>
        CompositeLoginProcessing = 0x92,
        CompositeEntityProcessing = 0x97,
        CompositeMessageProcessing = 0x98,
        
        /// <summary>
        /// 连接管理 (0x93, 0x99)
        /// </summary>
        HeartbeatResponse = 0x93,
        WelcomeMessage = 0x99
    }

    /// <summary>
    /// 服务器主指令枚举（协议头）
    /// </summary>
    public enum ServerMainCommand : byte
    {
        Special3 = 0x3,
        SendCharacterInfo4 = 0x4,
        CharacterPositionDefinition7 = 0x7,
        Settings9 = 0x9,
        Active10 = 0xA,
        Map11 = 0xB,
        HeartbeatResponse13 = 0xD,
        OtherCharacterHeartbeat14 = 0xE,
        Disappear16 = 0x10,
        Display17 = 0x11
    }

    /// <summary>
    /// 主动指令子类型
    /// </summary>
    public enum ActiveSubCommand : ushort
    {
        NPCDialogOption = 0xA0,
        CreateTeam = 0xA1,
        KickPlayerFromTeam = 0xA2,
        PlayerApplyJoinTeam = 0xA3,
        PlayerInviteJoinTeam = 0xA6,
        AgreeJoinTeam1 = 0xA9,
        AgreeJoinTeam2 = 0xAD,
        TeamChat = 0xAE,
        AgreeJoinTeam3 = 0xAF,
        PrepareStall = 0x191,
        ViewStallPosition = 0x1A0
    }

    /// <summary>
    /// 用户相关子指令
    /// </summary>
    public enum UserSubCommand
    {
        SetPhysicalSkillShortcut = 0x1D,
        SetPetName = 0x3E
    }

    /// <summary>
    /// 通用子指令
    /// </summary>
    public enum GeneralSubCommand
    {
        Unknown = 0,
        UserConnectionData = 0x30001,
        SendCharacterInfo = 0x40096
    }
}