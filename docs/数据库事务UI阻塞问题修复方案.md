# 数据库事务中UI阻塞问题修复方案

## 问题描述

在数据库事务执行过程中，如果发生唯一键约束冲突等数据库错误，系统会在AOP层的 `OnError` 事件中弹出 `MessageBox.Show()`，导致：

1. **事务锁死**：用户不点击确定按钮，事务永远不会提交或回滚
2. **表级锁定**：持有数据库锁，影响其他用户操作
3. **用户体验差**：在事务执行中途弹出模态对话框

## 根本原因

```csharp
// ❌ 错误做法：在AOP OnError回调中弹出MessageBox
db.Aop.OnError = (e) => {
    if (RemindEvent != null) {
        RemindEvent(e); // 这里会调用 SqlsugarSetup_RemindEvent
    }
};

private bool SqlsugarSetup_RemindEvent(SqlSugarException ex) {
    // ⚠️ 致命问题：在事务回调中弹出模态对话框
    MessageBox.Show(message, "唯一性错误", ...); 
    return true; // 吞掉异常，事务无法正确回滚
}
```

## 修复方案

### 核心原则

**事务边界内禁止任何UI交互！**

- ✅ AOP层：只负责记录错误信息，不显示UI
- ✅ 业务层：捕获异常，获取友好错误信息
- ✅ UI层：在事务外显示友好提示

### 修复步骤

#### 1. 修改 AOP 事件处理（已完成✅）

文件：`RUINORERP.UI/MainForm.cs`

```csharp
/// <summary>
/// SQL唯一键约束异常处理
/// 注意：此方法在AOP错误回调中被调用，不能弹出MessageBox，否则会阻塞事务导致锁表
/// 关键点：此方法必须返回 false，让异常继续传播到业务层
/// </summary>
private bool SqlsugarSetup_RemindEvent(SqlSugarException ex)
{
    // 处理唯一键约束异常，将错误信息存储到静态属性
    HandleUniqueConstraintException(ex);
    
    // ⚠️ 关键：始终返回 false，让异常继续传播
    // 这样事务才能正确回滚，不会锁表
    return false;
}

/// <summary>
/// 存储最近一次唯一键约束错误信息，供业务层获取并显示
/// </summary>
public static string LastUniqueConstraintError { get; private set; }

/// <summary>
/// 处理唯一键约束异常
/// 注意：此方法在AOP错误回调中被调用，不能弹出MessageBox
/// 只负责解析错误信息并存储，由业务层决定如何展示
/// </summary>
private bool HandleUniqueConstraintException(SqlSugarException ex)
{
    string errorMsg = ex.Message;

    // 判断是否为唯一约束冲突（中文/英文消息兼容）
    if (errorMsg.Contains("UNIQUE KEY") || errorMsg.Contains("unique key") || errorMsg.Contains("重复键"))
    {
        try
        {
            string value = ExtractDuplicateValue(errorMsg);
            
            // 提取表名并获取中文描述
            string tableName = ExtractTableName(errorMsg);
            string tableDescription = GetTableDescription(tableName);
            
            // 构建友好的错误信息
            string message = string.IsNullOrEmpty(tableDescription)
                ? $"【{value}】已存在，操作失败，请检查后重试！"
                : $"{tableDescription}中【{value}】已存在，操作失败，请检查后重试！";

            // 存储错误信息，供业务层获取
            LastUniqueConstraintError = message;
            
            // 记录日志
            System.Diagnostics.Debug.WriteLine($"[唯一键约束] {message}");
            
            return true;
        }
        catch (Exception parseEx)
        {
            LastUniqueConstraintError = $"数据唯一性验证失败：{parseEx.Message}";
            System.Diagnostics.Debug.WriteLine($"[唯一键约束解析失败] {parseEx.Message}");
            return true;
        }
    }
    
    return false;
}
```

#### 2. 创建异常处理辅助类（新增✅）

文件：`RUINORERP.UI/Common/DatabaseExceptionHandler.cs`

```csharp
using System;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 数据库异常处理助手
    /// 用于在事务外捕获并显示友好的错误提示
    /// </summary>
    public static class DatabaseExceptionHandler
    {
        /// <summary>
        /// 检查并显示唯一键约束错误
        /// 应在 catch 块中调用，用于显示友好的错误提示
        /// </summary>
        /// <param name="ex">捕获的异常</param>
        /// <returns>是否已处理（显示了友好提示）</returns>
        public static bool TryShowUniqueConstraintError(Exception ex)
        {
            // 检查是否有存储的唯一键约束错误信息
            if (!string.IsNullOrEmpty(MainForm.LastUniqueConstraintError))
            {
                // 显示友好提示
                MessageBox.Show(
                    MainForm.LastUniqueConstraintError,
                    "数据验证错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                
                // 清除已显示的错误信息，避免重复显示
                MainForm.LastUniqueConstraintError = null;
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 安全的执行业务操作并处理异常
        /// 自动处理唯一键约束等常见数据库错误
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="successMessage">成功时的提示消息（可选）</param>
        /// <param name="showSuccessToast">是否显示成功提示</param>
        public static void ExecuteWithDbErrorHandling(
            Action action, 
            string successMessage = null,
            bool showSuccessToast = false)
        {
            try
            {
                action();
                
                // 操作成功
                if (showSuccessToast && !string.IsNullOrEmpty(successMessage))
                {
                    MessageBox.Show(
                        successMessage,
                        "操作成功",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                // 优先尝试显示唯一键约束错误
                if (TryShowUniqueConstraintError(ex))
                {
                    return; // 已显示友好提示，直接返回
                }
                
                // 其他异常：记录日志并显示通用错误
                System.Diagnostics.Debug.WriteLine($"[数据库操作异常] {ex.Message}\n{ex.StackTrace}");
                
                MessageBox.Show(
                    $"操作失败：{ex.Message}",
                    "系统错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        
        /// <summary>
        /// 异步版本的异常处理
        /// </summary>
        public static async Task ExecuteWithDbErrorHandlingAsync(
            Func<Task> action, 
            string successMessage = null,
            bool showSuccessToast = false)
        {
            try
            {
                await action();
                
                if (showSuccessToast && !string.IsNullOrEmpty(successMessage))
                {
                    MessageBox.Show(
                        successMessage,
                        "操作成功",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                if (TryShowUniqueConstraintError(ex))
                {
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine($"[数据库操作异常] {ex.Message}\n{ex.StackTrace}");
                
                MessageBox.Show(
                    $"操作失败：{ex.Message}",
                    "系统错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
```

#### 3. 修改销售订单保存逻辑（示例✅）

文件：`RUINORERP.UI/PSI/SAL/UCSaleOrder.cs`

```csharp
protected async override Task<bool> Save(bool NeedValidated)
{
    // ... 前面的验证逻辑保持不变 ...
    
    try
    {
        ReturnMainSubResults<tb_SaleOrder> SaveResult = new ReturnMainSubResults<tb_SaleOrder>();
        if (NeedValidated)
        {
            // 1. 先保存业务数据，获取主键后再同步图片（确保图片与业务正确关联）
            SaveResult = await base.Save(EditEntity);

            if (!SaveResult.Succeeded)
            {
                // ✅ 关键修改：检查是否有唯一键约束错误
                if (DatabaseExceptionHandler.TryShowUniqueConstraintError(null))
                {
                    MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    return false;
                }
                
                MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                return false;
            }

            // 2. 再处理主表图片同步（此时实体已有主键）
            if (magicPictureBox订金付款凭证 != null)
            {
                var updated = magicPictureBox订金付款凭证.GetImageInfosNeedingUpdate();
                var deleted = magicPictureBox订金付款凭证.GetDeletedImages();

                if (updated.Count > 0 || deleted.Count > 0)
                {
                    MainForm.Instance.PrintInfoLog("正在同步订单凭证图片...");
                    bool imgSuccess = await UploadUpdatedImagesAsync(EditEntity, updated, deleted, c => c.VoucherImage);
                    if (!imgSuccess)
                    {
                        MainForm.Instance.uclog.AddLog("凭证图片同步失败，但单据已保存成功。", Global.UILogType.警告);
                    }
                    else
                    {
                        magicPictureBox订金付款凭证.ClearDeletedImagesList();
                        magicPictureBox订金付款凭证.ResetImageChangeStatus();
                        MainForm.Instance.PrintInfoLog("凭证图片同步完成。");
                    }
                }
            }

            MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SOrderNo}。");
        }
        return SaveResult.Succeeded;
    }
    catch (Exception ex)
    {
        // ✅ 关键修改：在事务外捕获异常并显示友好提示
        if (DatabaseExceptionHandler.TryShowUniqueConstraintError(ex))
        {
            return false;
        }
        
        // 其他异常处理
        MainForm.Instance.uclog.AddLog($"保存异常：{ex.Message}", UILogType.错误);
        logger.LogError(ex, "保存销售订单异常");
        return false;
    }
}
```

#### 4. 基类统一异常处理（推荐✅）

文件：`RUINORERP.UI/BaseForm/BaseBillEditGeneric.cs`

在 `Save(T entity)` 方法中添加统一的异常处理：

```csharp
protected async Task<ReturnMainSubResults<T>> Save(T entity)
{
    // ... 前面的代码保持不变 ...
    
    try
    {
        ReturnMainSubResults<T> rmr = new ReturnMainSubResults<T>();
        BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        rmr = await ctr.BaseSaveOrUpdateWithChild<T>(entity);
        
        if (rmr.Succeeded)
        {
            // ... 成功处理逻辑 ...
        }
        else
        {
            // ✅ 关键修改：检查是否有唯一键约束错误
            if (!string.IsNullOrEmpty(MainForm.LastUniqueConstraintError))
            {
                MainForm.Instance.uclog.AddLog(
                    MainForm.LastUniqueConstraintError, 
                    UILogType.错误
                );
                // 清除已处理的错误
                MainForm.LastUniqueConstraintError = null;
            }
            else
            {
                MainForm.Instance.uclog.AddLog(
                    "保存失败，请重试;或联系管理员。" + rmr.ErrorMsg, 
                    UILogType.错误
                );
            }
        }

        return rmr;
    }
    catch (Exception ex)
    {
        // ✅ 关键修改：在事务外捕获异常并显示友好提示
        if (DatabaseExceptionHandler.TryShowUniqueConstraintError(ex))
        {
            return new ReturnMainSubResults<T>()
            {
                Succeeded = false,
                ErrorMsg = MainForm.LastUniqueConstraintError ?? ex.Message
            };
        }
        
        // 重新抛出其他异常
        throw;
    }
}
```

## 全局排查清单

需要检查以下位置是否存在类似问题：

### 1. AOP 事件处理器
- ✅ `SqlsugarSetup.RemindEvent` - 已修复
- ⚠️ 检查是否有其他 AOP 事件中有 UI 操作

### 2. 事务中的 UI 操作
搜索以下模式：
```csharp
_unitOfWorkManage.BeginTran();
// ... 
MessageBox.Show(...); // ❌ 禁止
// ...
_unitOfWorkManage.CommitTran();
```

### 3. Controller 层的异常处理
检查所有 `BaseSaveOrUpdateWithChild` 的重写方法，确保：
- ✅ 不在事务中弹出对话框
- ✅ 异常时正确回滚事务
- ✅ 错误信息能传递到 UI 层

## 测试验证

### 测试场景 1：唯一键约束冲突
1. 创建销售订单，输入已存在的平台单号
2. 点击保存
3. **预期结果**：
   - ✅ 立即显示友好提示："销售订单中【XXX】已存在，操作失败，请检查后重试！"
   - ✅ 事务正确回滚
   - ✅ 不锁表，其他用户可以正常操作

### 测试场景 2：并发保存
1. 两个用户同时编辑同一张单据
2. 用户A先保存成功
3. 用户B后保存
4. **预期结果**：
   - ✅ 用户B收到明确的冲突提示
   - ✅ 事务立即回滚
   - ✅ 不阻塞其他操作

### 测试场景 3：网络超时
1. 模拟网络延迟
2. 执行保存操作
3. **预期结果**：
   - ✅ 超时后显示通用错误提示
   - ✅ 事务正确回滚
   - ✅ 不锁表

## 注意事项

1. **严禁在事务中弹出模态对话框**
   - MessageBox.Show()
   - KryptonMessageBox.Show()
   - Form.ShowDialog()
   - 任何需要用户交互的UI操作

2. **异常处理原则**
   - AOP层：只记录，不显示
   - Service/Controller层：捕获并转换错误信息
   - UI层：统一显示友好提示

3. **事务管理**
   - 使用 `try-catch-finally` 确保事务回滚
   - 避免在事务中执行耗时操作
   - 尽量缩短事务持续时间

4. **日志记录**
   - 所有异常都要记录日志
   - 包含足够的上下文信息
   - 便于问题追踪和调试

## 相关文件清单

### 已修改文件
1. `RUINORERP.UI/MainForm.cs` - AOP事件处理
2. `RUINORERP.UI/Common/DatabaseExceptionHandler.cs` - 新建辅助类

### 需要检查的文件
1. `RUINORERP.UI/BaseForm/BaseBillEditGeneric.cs` - 基类Save方法
2. `RUINORERP.UI/PSI/SAL/UCSaleOrder.cs` - 销售订单保存
3. `RUINORERP.Business/*Controller*.cs` - 所有Controller的BaseSaveOrUpdateWithChild实现

### 相关配置文件
1. `RUINORERP.Extensions/ServiceExtensions/SqlsugarSetup.cs` - SqlSugar配置

## 后续优化建议

1. **统一错误码体系**
   - 定义标准化的错误码
   - 便于前端国际化
   - 支持更精细的错误处理

2. **异步异常处理**
   - 所有数据库操作改为异步
   - 避免UI线程阻塞
   - 提升用户体验

3. **乐观锁机制**
   - 添加版本号字段
   - 检测并发修改
   - 提供更友好的并发冲突提示

4. **监控告警**
   - 记录唯一键冲突频率
   - 分析热点数据
   - 优化业务逻辑

## 总结

本次修复的核心思想是：**职责分离 + 事务纯净**。

- **AOP层**：只做错误检测和记录
- **业务层**：负责错误转换和传递
- **UI层**：负责友好提示展示

通过这种分层处理，确保了：
1. ✅ 事务不会被UI操作阻塞
2. ✅ 数据库锁能及时释放
3. ✅ 用户能获得清晰的错误提示
4. ✅ 系统稳定性和可用性得到保障
