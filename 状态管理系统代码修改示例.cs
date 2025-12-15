/*
 * 状态管理系统代码修改示例
 * 
 * 此文件包含对UnifiedStateManager和GlobalStateRulesManager的具体修改示例
 * 用于指导开发人员实施状态管理系统的优化
 */

// 文件：RUINORERP.Model\Base\StatusManager\UnifiedStateManager.cs
// 添加以下方法到UnifiedStateManager类

/// <summary>
/// 判断指定实体的指定状态类型是否为终态
/// </summary>
/// <typeparam name="TStatus">状态类型</typeparam>
/// <param name="entity">目标实体</param>
/// <returns>是否为终态</returns>
public bool IsFinalStatus<TStatus>(object entity) where TStatus : struct
{
    if (entity == null)
        throw new ArgumentNullException(nameof(entity));

    try
    {
        // 获取实体当前状态
        TStatus currentStatus = GetBusinessStatus<TStatus>(entity);
        
        // 判断是否为终态
        return GlobalStateRulesManager.Instance.IsFinalStatus(currentStatus);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"判断状态类型 {typeof(TStatus).Name} 是否为终态时发生错误");
        return false;
    }
}

/// <summary>
/// 判断指定实体的数据状态是否为终态
/// </summary>
/// <param name="entity">目标实体</param>
/// <returns>是否为终态</returns>
public bool IsFinalDataStatus(object entity)
{
    if (entity == null)
        throw new ArgumentNullException(nameof(entity));

    try
    {
        // 获取实体当前数据状态
        DataStatus currentStatus = GetDataStatus(entity);
        
        // 判断是否为终态
        return GlobalStateRulesManager.Instance.IsFinalStatus(currentStatus);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "判断数据状态是否为终态时发生错误");
        return false;
    }
}

/// <summary>
/// 判断实体是否可修改
/// </summary>
/// <param name="entity">目标实体</param>
/// <returns>是否可修改</returns>
public bool CanModify(object entity)
{
    if (entity == null)
        throw new ArgumentNullException(nameof(entity));

    try
    {
        // 获取实体当前数据状态
        DataStatus currentStatus = GetDataStatus(entity);
        
        // 判断数据状态是否为终态
        if (IsFinalDataStatus(entity))
            return false;
            
        // 检查提交修改规则模式
        bool isSubmittedStatus = currentStatus == DataStatus.提交 || currentStatus == DataStatus.确认;
        return GlobalStateRulesManager.Instance.AllowModifyAfterSubmit(isSubmittedStatus);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "判断实体是否可修改时发生错误");
        return false;
    }
}

// 文件：RUINORERP.Model\Base\StatusManager\GlobalStateRulesManager.cs
// 添加以下方法到GlobalStateRulesManager类

/// <summary>
/// 判断指定状态是否为终态
/// </summary>
/// <typeparam name="TStatus">状态类型</typeparam>
/// <param name="status">状态值</param>
/// <returns>是否为终态</returns>
public bool IsFinalStatus<TStatus>(TStatus status) where TStatus : struct
{
    var statusType = typeof(TStatus);
    
    // 针对不同状态类型的终态判断
    if (statusType == typeof(DataStatus))
    {
        DataStatus dataStatus = (DataStatus)(object)status;
        return dataStatus == DataStatus.完结 || dataStatus == DataStatus.作废;
    }
    else if (statusType == typeof(ActionStatus))
    {
        ActionStatus actionStatus = (ActionStatus)(object)status;
        return actionStatus == ActionStatus.无操作;
    }
    else if (statusType == typeof(PaymentStatus))
    {
        PaymentStatus paymentStatus = (PaymentStatus)(object)status;
        return paymentStatus == PaymentStatus.已支付;
    }
    else if (statusType == typeof(RefundStatus))
    {
        RefundStatus refundStatus = (RefundStatus)(object)status;
        return refundStatus == RefundStatus.已退款已退货 || refundStatus == RefundStatus.已退款未退货 || refundStatus == RefundStatus.部分退款退货;
    }
    else if (statusType == typeof(PrePaymentStatus))
    {
        PrePaymentStatus prepayStatus = (PrePaymentStatus)(object)status;
        return prepayStatus == PrePaymentStatus.已结案;
    }
    else if (statusType == typeof(ARAPStatus))
    {
        ARAPStatus arapStatus = (ARAPStatus)(object)status;
        return arapStatus == ARAPStatus.全部支付 || arapStatus == ARAPStatus.已冲销;
    }
    else if (statusType == typeof(StatementStatus))
    {
        StatementStatus statementStatus = (StatementStatus)(object)status;
        return statementStatus == StatementStatus.已结清 || statementStatus == StatementStatus.已作废;
    }
    
    // 默认情况下，不是终态
    return false;
}

/// <summary>
/// 获取状态类型的描述信息
/// </summary>
/// <param name="statusType">状态类型</param>
/// <returns>状态类型描述</returns>
public string GetStatusTypeDescription(Type statusType)
{
    if (statusType == typeof(DataStatus))
        return "数据状态";
    else if (statusType == typeof(ActionStatus))
        return "操作状态";
    else if (statusType == typeof(PaymentStatus))
        return "付款状态";
    else if (statusType == typeof(RefundStatus))
        return "退款状态";
    else if (statusType == typeof(PrePaymentStatus))
        return "预付款状态";
    else if (statusType == typeof(ARAPStatus))
        return "应收应付状态";
    else if (statusType == typeof(StatementStatus))
        return "对账状态";
    
    return statusType.Name;
}

// 添加以下方法到GlobalStateRulesManager类，并在InitializeBusinessStatusTransitionRules方法中调用

/// <summary>
/// 初始化退款状态转换规则
/// </summary>
private void InitializeRefundStatusTransitionRules()
{
    var statusType = typeof(RefundStatus);
    _stateTransitionRules[statusType] = new Dictionary<object, List<object>>
    {
        [RefundStatus.未退款等待退货] = new List<object> { RefundStatus.未退款已退货, RefundStatus.已退款等待退货, RefundStatus.已退款未退货 },
        [RefundStatus.未退款已退货] = new List<object> { RefundStatus.已退款已退货, RefundStatus.部分退款退货 },
        [RefundStatus.已退款等待退货] = new List<object> { RefundStatus.已退款已退货, RefundStatus.部分退款退货 },
        [RefundStatus.已退款未退货] = new List<object> { RefundStatus.部分退款退货 },
        [RefundStatus.已退款已退货] = new List<object>(),
        [RefundStatus.部分退款退货] = new List<object>()
    };
}

// 修改InitializeBusinessStatusTransitionRules方法，添加对InitializeRefundStatusTransitionRules的调用
private void InitializeBusinessStatusTransitionRules()
{
    InitializePaymentStatusTransitionRules();
    InitializePrePaymentStatusTransitionRules();
    InitializeARAPStatusTransitionRules();
    InitializeStatementStatusTransitionRules();
    InitializeRefundStatusTransitionRules(); // 新增
}

// 文件：RUINORERP.UI\BaseForm\BaseBillEditGeneric.cs
// 修改UpdateSaveButtonState方法

/// <summary>
/// 更新保存按钮状态
/// </summary>
protected void UpdateSaveButtonState()
{
    try
    {
        // 获取当前实体
        var entity = this.Entity;
        if (entity == null)
        {
            toolStripButtonSave.Enabled = false;
            return;
        }
        
        // 实体变更检查
        bool hasChanged = entity.HasChanged;
        if (!hasChanged)
        {
            toolStripButtonSave.Enabled = false;
            return;
        }
        
        // 使用统一状态管理器判断是否可修改
        var stateManager = entity.StateManager;
        bool canModify = stateManager.CanModify(entity);
        
        // 锁定状态检查
        bool isLocked = false;
        var lockInfoProperty = entity.GetType().GetProperty("LockInfo", BindingFlags.Instance | BindingFlags.Public);
        if (lockInfoProperty != null)
        {
            var lockInfo = lockInfoProperty.GetValue(entity);
            isLocked = lockInfo != null && !string.IsNullOrEmpty(lockInfo.ToString());
        }
        
        // 综合判断保存按钮状态
        toolStripButtonSave.Enabled = canModify && !isLocked;
    }
    catch (Exception ex)
    {
        // 异常处理
        toolStripButtonSave.Enabled = false;
        LogHelper.WriteLog(ex.Message);
    }
}

// 添加以下辅助方法到BaseBillEditGeneric类

/// <summary>
/// 判断当前实体的指定业务状态类型是否为终态
/// </summary>
/// <typeparam name="TStatus">业务状态类型</typeparam>
/// <returns>是否为终态</returns>
protected bool IsBusinessStatusFinal<TStatus>() where TStatus : struct
{
    if (Entity == null)
        return false;
    
    return Entity.StateManager.IsFinalStatus<TStatus>(Entity);
}

// 文件：RUINORERP.Business\StatusManagerService\FMPaymentStatusHelper.cs
// 修改现有方法，委托给统一状态管理系统

/// <summary>
/// 判断实体的付款状态是否为终态
/// </summary>
/// <param name="entity">实体</param>
/// <returns>是否为终态</returns>
public static bool IsFinalStatus(object entity)
{
    if (entity == null)
        return false;
    
    // 委托给统一状态管理器
    return entity.StateManager.IsFinalStatus<PaymentStatus>(entity);
}

/// <summary>
/// 判断实体是否可修改
/// </summary>
/// <param name="entity">实体</param>
/// <returns>是否可修改</returns>
public static bool CanModify(object entity)
{
    if (entity == null)
        return false;
    
    // 委托给统一状态管理器
    return entity.StateManager.CanModify(entity);
}

// 文件：RUINORERP.Business\StatusManagerService\RefundStatusHelper.cs
// 修改现有方法，委托给统一状态管理系统

/// <summary>
/// 判断退款状态是否为终态
/// </summary>
/// <param name="status">退款状态</param>
/// <returns>是否为终态</returns>
public static bool IsFinalStatus(RefundStatus status)
{
    // 委托给全局规则管理器
    return GlobalStateRulesManager.Instance.IsFinalStatus(status);
}

/// <summary>
/// 验证状态转换是否有效
/// </summary>
/// <param name="fromStatus">源状态</param>
/// <param name="toStatus">目标状态</param>
/// <returns>转换结果</returns>
public static StateTransitionResult TryValidateTransition(RefundStatus fromStatus, RefundStatus toStatus)
{
    // 委托给全局规则管理器
    bool isValid = GlobalStateRulesManager.Instance.IsValidTransition(fromStatus, toStatus);
    return new StateTransitionResult(isValid, isValid ? string.Empty : $"无法从 {fromStatus} 转换到 {toStatus}");
}

// 文件：RUINORERP.Business\StatusManagerService\StatusManagerService.cs
// 修改现有方法，委托给统一状态管理系统

/// <summary>
/// 判断实体是否可编辑
/// </summary>
/// <param name="entity">实体</param>
/// <returns>是否可编辑</returns>
public bool IsEditable(object entity)
{
    if (entity == null)
        return false;
    
    // 委托给统一状态管理器
    return entity.StateManager.CanModify(entity);
}

/// <summary>
/// 判断数据状态是否为终态
/// </summary>
/// <param name="status">数据状态</param>
/// <returns>是否为终态</returns>
public bool IsFinalStatus(DataStatus status)
{
    // 委托给全局规则管理器
    return GlobalStateRulesManager.Instance.IsFinalStatus(status);
}
