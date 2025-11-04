# Bug与建议反馈功能设计文档

## 1. 功能概述

设计一个用户友好的Bug与建议反馈功能，允许客户端用户提交系统Bug报告、功能建议或改进意见，并支持上传截图作为附件，以便开发团队更直观地了解问题或需求。

## 2. 技术架构

### 2.1 前端架构

- **基于现有WinForms UI框架**
- **使用Krypton组件库**提供现代化UI体验
- **复用现有图片上传机制**，利用`FileManagementController`和`MagicPictureBox`控件
- **依赖注入**：通过`Startup.GetFromFac<T>()`获取服务实例

> **注意**：`HttpWebService`已被标记为过时，不再推荐使用，新功能应使用`FileManagementController`和`FileManagementService`来处理文件上传下载操作。

### 2.2 后端架构

- **ORM**：SqlSugar
- **工作流**：WorkflowCore (可选，用于Bug处理流程)
- **缓存**：CacheManager.Core 和 StackExchange.Redis
- **数据库**：MSQL

## 3. 数据模型设计

### 3.1 tb_UserFeedback表

| 字段名 | 数据类型 | 长度 | 约束 | 描述 |
|--------|----------|------|------|------|
| FeedbackID | bigint | - | 主键，自增 | 反馈ID |
| FeedbackType | tinyint | - | 非空 | 反馈类型：1-Bug, 2-功能建议, 3-改进意见 |
| Title | nvarchar | 200 | 非空 | 标题 |
| Description | nvarchar | max | 非空 | 详细描述 |
| UserID | bigint | - | 非空 | 提交用户ID |
| UserName | nvarchar | 50 | 非空 | 提交用户名 |
| ModuleName | nvarchar | 100 | 非空 | 相关模块 |
| Priority | tinyint | - | 非空 | 优先级：1-低, 2-中, 3-高, 4-紧急 |
| Status | tinyint | - | 非空 | 状态：1-待处理, 2-处理中, 3-已解决, 4-已关闭 |
| CreatedDate | datetime | - | 非空 | 创建时间 |
| LastUpdatedDate | datetime | - | 非空 | 最后更新时间 |
| AssignTo | bigint | - | 可空 | 分配给 |
| Resolution | nvarchar | max | 可空 | 解决方案 |
| IsPublic | bit | - | 非空 | 是否公开 |

### 3.2 tb_UserFeedbackAttachment表

| 字段名 | 数据类型 | 长度 | 约束 | 描述 |
|--------|----------|------|------|------|
| AttachmentID | bigint | - | 主键，自增 | 附件ID |
| FeedbackID | bigint | - | 外键 | 反馈ID |
| FileName | nvarchar | 255 | 非空 | 文件名 |
| FilePath | nvarchar | 500 | 非空 | 文件路径 |
| FileSize | bigint | - | 非空 | 文件大小 |
| FileType | nvarchar | 50 | 非空 | 文件类型 |
| UploadDate | datetime | - | 非空 | 上传时间 |

## 4. UI设计

### 4.1 主表单：frmUserFeedback.cs

#### 4.1.1 设计布局

- **标题区域**：Bug与建议反馈表单
- **基本信息区**：
  - 反馈类型（下拉框）：Bug、功能建议、改进意见
  - 标题（文本框）
  - 相关模块（下拉框）：自动获取系统所有模块
  - 优先级（下拉框）：低、中、高、紧急
  - 提交人（标签，只读）：当前登录用户

- **详细描述区**：
  - 多行文本框，支持换行和基本格式化
  - 字数统计

- **图片上传区**：
  - 使用`MagicPictureBox`控件支持多图片上传和预览
  - 添加图片按钮
  - 删除图片按钮
  - 支持拖拽上传和图片预览

- **操作按钮区**：
  - 提交按钮
  - 取消按钮
  - 重置按钮

#### 4.1.2 关键代码片段

```csharp
// 反馈类型枚举
public enum FeedbackType
{
    Bug = 1,
    FeatureRequest = 2,
    Improvement = 3
}

// 优先级枚举
public enum FeedbackPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Urgent = 4
}

// 状态枚举
public enum FeedbackStatus
{
    Pending = 1,
    Processing = 2,
    Resolved = 3,
    Closed = 4
}

// 初始化MagicPictureBox
private void InitializeMagicPictureBox()
{
    // 启用多图片支持
    magicPictureBoxFeedback.MultiImageSupport = true;
    // 显示图片信息
    magicPictureBoxFeedback.ShowImageInfo = true;
    // 设置图片路径（用于加载已保存的图片）
    magicPictureBoxFeedback.ImagePaths = string.Empty;
}

// 上传图片按钮事件
private void btnAddImage_Click(object sender, EventArgs e)
{
    try
    {
        // 直接调用MagicPictureBox内置的图片选择功能
        magicPictureBoxFeedback.SelectAndLoadImages();
        MainForm.Instance.PrintInfoLog("已添加图片到预览");
    }
    catch (Exception ex)
    {
        MainForm.Instance.uclog.AddLog($"添加图片失败: {ex.Message}", Global.UILogType.错误);
    }
}

// 提交反馈按钮事件
private async void btnSubmit_Click(object sender, EventArgs e)
{
    // 表单验证
    if (!ValidateForm())
    {
        return;
    }
    
    // 创建反馈实体
    tb_UserFeedback feedback = new tb_UserFeedback
    {
        FeedbackType = (int)cmbFeedbackType.SelectedValue,
        Title = txtTitle.Text.Trim(),
        Description = txtDescription.Text.Trim(),
        UserID = MainForm.Instance.AppContext.CurrentUserID,
        UserName = MainForm.Instance.AppContext.CurrentUserName,
        ModuleName = cmbModule.Text.Trim(),
        Priority = (int)cmbPriority.SelectedValue,
        Status = (int)FeedbackStatus.Pending,
        CreatedDate = DateTime.Now,
        LastUpdatedDate = DateTime.Now,
        IsPublic = chkIsPublic.Checked
    };
    
    try
    {
        // 显示等待提示
        MainForm.Instance.ShowWaiting("正在提交反馈...");
        
        // 调用服务保存反馈
        var result = await _feedbackService.SaveFeedbackAsync(feedback);
        
        if (result.Succeeded)
        {
            // 上传附件
            await UploadImageAttachments(result.Data.FeedbackID);
            
            MessageBox.Show("反馈提交成功，感谢您的宝贵意见！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            MessageBox.Show($"提交失败: {result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    catch (Exception ex)
    {
        MainForm.Instance.uclog.AddLog($"提交反馈异常: {ex.Message}", Global.UILogType.错误);
        MessageBox.Show("提交过程中发生异常，请稍后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
        MainForm.Instance.HideWaiting();
    }
}

// 上传图片附件
private async Task UploadImageAttachments(long feedbackId)
{
    try
    {
        // 获取文件管理控制器
        var fileController = Startup.GetFromFac<FileManagementController>();
        
        // 检查是否有图片需要上传
        if (magicPictureBoxFeedback.Image == null && !magicPictureBoxFeedback.HasImages)
        {
            MainForm.Instance.PrintInfoLog("没有需要上传的图片");
            return;
        }
        
        // 获取所有需要上传的图片数据和信息
        var imageBytesWithInfoList = magicPictureBoxFeedback.GetAllImageBytesWithInfo();
        
        if (imageBytesWithInfoList == null || imageBytesWithInfoList.Count == 0)
        {
            MainForm.Instance.PrintInfoLog("没有需要上传的图片数据");
            return;
        }
        
        int successCount = 0;
        
        // 遍历上传所有图片
        foreach (var imageDataWithInfo in imageBytesWithInfoList)
        {
            byte[] imageData = imageDataWithInfo.Item1;
            ImageInfo imageInfo = imageDataWithInfo.Item2;
            
            if (imageData == null || imageData.Length == 0)
            {
                MainForm.Instance.uclog.AddLog($"跳过空图片数据: {imageInfo.OriginalFileName}", Global.UILogType.警告);
                continue;
            }
            
            // 检查文件大小（10MB限制）
            if (imageData.Length > 10 * 1024 * 1024)
            {
                MainForm.Instance.uclog.AddLog($"图片 {imageInfo.OriginalFileName} 超过大小限制(10MB)", Global.UILogType.错误);
                continue;
            }
            
            long? existingFileId = null;
            string updateReason = "图片更新";
            
            // 如果图片信息包含文件ID且图片已更新
            if (imageInfo.FileId > 0 && imageInfo.IsUpdated)
            {
                existingFileId = imageInfo.FileId;
            }
            
            // 创建反馈实体用于上传
            tb_UserFeedback feedback = new tb_UserFeedback { FeedbackID = feedbackId };
            
            // 上传图片
            var response = await fileController.UploadImageAsync(
                feedback,
                imageInfo.OriginalFileName,
                imageData,
                "BugFeedback",
                existingFileId,
                updateReason,
                false);
            
            if (response.IsSuccess)
            {
                successCount++;
                MainForm.Instance.PrintInfoLog($"图片上传成功：{imageInfo.OriginalFileName}");
                
                // 记录附件信息
                tb_UserFeedbackAttachment attachment = new tb_UserFeedbackAttachment
                {
                    FeedbackID = feedbackId,
                    FileName = imageInfo.OriginalFileName,
                    FilePath = response.FileStorageInfos[0]?.StoragePath ?? string.Empty,
                    FileSize = imageData.Length,
                    FileType = Path.GetExtension(imageInfo.OriginalFileName).ToLower(),
                    UploadDate = DateTime.Now
                };
                
                await _feedbackService.SaveAttachmentAsync(attachment);
                
                // 上传成功后，将图片标记为未更新
                if (imageInfo.IsUpdated)
                {
                    imageInfo.IsUpdated = false;
                }
            }
            else
            {
                MainForm.Instance.uclog.AddLog($"图片上传失败：{imageInfo.OriginalFileName}，原因：{response.Message}", Global.UILogType.错误);
            }
        }
        
        if (successCount > 0)
        {
            MainForm.Instance.PrintInfoLog($"成功上传 {successCount} 张图片");
        }
    }
    catch (Exception ex)
    {
        MainForm.Instance.uclog.AddLog($"上传图片附件异常: {ex.Message}", Global.UILogType.错误);
    }
}
```

## 5. 服务层设计

### 5.1 IUserFeedbackService接口

```csharp
public interface IUserFeedbackService
{
    /// <summary>
    /// 保存反馈信息
    /// </summary>
    /// <param name="feedback">反馈实体</param>
    /// <returns>操作结果</returns>
    Task<ResponseResult<tb_UserFeedback>> SaveFeedbackAsync(tb_UserFeedback feedback);
    
    /// <summary>
    /// 保存附件信息
    /// </summary>
    /// <param name="attachment">附件实体</param>
    /// <returns>操作结果</returns>
    Task<ResponseResult<tb_UserFeedbackAttachment>> SaveAttachmentAsync(tb_UserFeedbackAttachment attachment);
    
    /// <summary>
    /// 获取用户反馈列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页大小</param>
    /// <returns>反馈列表</returns>
    Task<ResponseResult<PagedResult<tb_UserFeedback>>> GetUserFeedbacksAsync(long userId, int pageIndex, int pageSize);
    
    /// <summary>
    /// 获取反馈详情
    /// </summary>
    /// <param name="feedbackId">反馈ID</param>
    /// <returns>反馈详情</returns>
    Task<ResponseResult<tb_UserFeedback>> GetFeedbackDetailAsync(long feedbackId);
}
```

### 5.2 UserFeedbackService实现类

```csharp
public class UserFeedbackService : IUserFeedbackService
{
    private readonly ISqlSugarClient _db;
    private readonly ILogger _logger;
    
    public UserFeedbackService(ISqlSugarClient db, ILogger logger)
    {
        _db = db;
        _logger = logger;
    }
    
    public async Task<ResponseResult<tb_UserFeedback>> SaveFeedbackAsync(tb_UserFeedback feedback)
    {
        try
        {
            long id = await _db.Insertable(feedback).ExecuteReturnBigIdentityAsync();
            feedback.FeedbackID = id;
            return ResponseFactory.CreateSuccessResponse(feedback);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "保存用户反馈失败");
            return ResponseFactory.CreateSpecificErrorResponse<tb_UserFeedback>("保存反馈信息失败，请稍后重试");
        }
    }
    
    public async Task<ResponseResult<tb_UserFeedbackAttachment>> SaveAttachmentAsync(tb_UserFeedbackAttachment attachment)
    {
        try
        {
            long id = await _db.Insertable(attachment).ExecuteReturnBigIdentityAsync();
            attachment.AttachmentID = id;
            return ResponseFactory.CreateSuccessResponse(attachment);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "保存附件信息失败");
            return ResponseFactory.CreateSpecificErrorResponse<tb_UserFeedbackAttachment>("保存附件信息失败");
        }
    }
    
    // 其他方法实现...
}
```

## 6. 数据访问层设计

### 6.1 UserFeedbackRepository

```csharp
public class UserFeedbackRepository : IUserFeedbackRepository
{
    private readonly ISqlSugarClient _db;
    
    public UserFeedbackRepository(ISqlSugarClient db)
    {
        _db = db;
    }
    
    // 数据访问方法...
}
```

## 7. 服务端API设计

### 7.1 UserFeedbackController

```csharp
public class UserFeedbackController : BaseApiController
{
    private readonly IUserFeedbackService _feedbackService;
    
    public UserFeedbackController(IUserFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }
    
    [HttpPost("/api/feedback/save")]
    public async Task<IActionResult> SaveFeedback([FromBody] tb_UserFeedback feedback)
    {
        var result = await _feedbackService.SaveFeedbackAsync(feedback);
        return Ok(result);
    }
    
    [HttpPost("/api/feedback/attachment/save")]
    public async Task<IActionResult> SaveAttachment([FromBody] tb_UserFeedbackAttachment attachment)
    {
        var result = await _feedbackService.SaveAttachmentAsync(attachment);
        return Ok(result);
    }
    
    // 其他API方法...
}
```

## 8. 流程设计

### 8.1 提交反馈流程

1. 用户打开反馈表单
2. 选择反馈类型、填写标题、描述等信息
3. 可选：上传相关图片
4. 点击提交按钮
5. 系统验证表单数据
6. 调用服务保存反馈信息
7. 上传图片附件（如有）
8. 显示提交结果提示

### 8.2 图片上传流程

1. 用户选择本地图片文件（通过MagicPictureBox控件）
2. 系统读取图片文件内容到内存
3. MagicPictureBox生成图片预览并存储图片信息
4. 表单提交后，获取所有待上传的图片数据
5. 调用`FileManagementController.UploadImageAsync()`上传图片到服务器
6. 保存附件信息到数据库

> **注意**：不再使用过时的`HttpWebService`进行文件上传，改为使用更现代和稳定的`FileManagementController`机制。

## 9. 安全考虑

1. **文件类型验证**：仅允许上传指定类型的图片文件（jpg, jpeg, png, gif, bmp）
2. **文件大小限制**：单张图片不超过5MB，总附件不超过20MB
3. **SQL注入防护**：使用参数化查询
4. **用户权限**：验证用户身份后才能提交反馈
5. **敏感信息过滤**：在保存反馈内容前过滤可能的敏感信息

## 10. 性能优化

1. **异步处理**：所有网络操作和耗时操作使用异步方法
2. **图片压缩**：对上传的图片进行适当压缩，减少存储空间和传输时间
3. **分页查询**：查询反馈列表时使用分页，避免返回大量数据
4. **缓存机制**：缓存常用模块列表等数据

## 11. 部署与集成

1. **数据库脚本**：提供创建表和索引的SQL脚本
2. **依赖注入配置**：在Startup中注册服务
3. **UI集成**：在系统菜单中添加"反馈与建议"入口
4. **权限设置**：为所有用户授予提交反馈的权限

## 12. 测试计划

1. **功能测试**：验证基本提交、图片上传、验证等功能
2. **性能测试**：测试大量图片上传的性能
3. **兼容性测试**：在不同环境下测试功能
4. **安全测试**：验证防注入、文件类型验证等安全措施

## 13. 维护与支持

1. **日志记录**：记录所有反馈提交和处理日志
2. **错误处理**：完善的错误处理和友好的错误提示
3. **监控机制**：监控反馈提交的成功率和响应时间

## 14. 附录

### 14.1 相关数据表SQL脚本

```sql
-- 创建反馈表
CREATE TABLE tb_UserFeedback (
    FeedbackID BIGINT IDENTITY(1,1) PRIMARY KEY,
    FeedbackType TINYINT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    UserID BIGINT NOT NULL,
    UserName NVARCHAR(50) NOT NULL,
    ModuleName NVARCHAR(100) NOT NULL,
    Priority TINYINT NOT NULL,
    Status TINYINT NOT NULL,
    CreatedDate DATETIME NOT NULL,
    LastUpdatedDate DATETIME NOT NULL,
    AssignTo BIGINT NULL,
    Resolution NVARCHAR(MAX) NULL,
    IsPublic BIT NOT NULL
);

-- 创建附件表
CREATE TABLE tb_UserFeedbackAttachment (
    AttachmentID BIGINT IDENTITY(1,1) PRIMARY KEY,
    FeedbackID BIGINT NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileSize BIGINT NOT NULL,
    FileType NVARCHAR(50) NOT NULL,
    UploadDate DATETIME NOT NULL,
    CONSTRAINT FK_UserFeedbackAttachment_UserFeedback FOREIGN KEY (FeedbackID) REFERENCES tb_UserFeedback(FeedbackID)
);

-- 创建索引
CREATE INDEX IX_UserFeedback_UserID ON tb_UserFeedback(UserID);
CREATE INDEX IX_UserFeedback_Status ON tb_UserFeedback(Status);
CREATE INDEX IX_UserFeedbackAttachment_FeedbackID ON tb_UserFeedbackAttachment(FeedbackID);
```

### 14.2 依赖项清单

- WinForms
- Krypton Toolkit
- SqlSugar
- CacheManager.Core
- StackExchange.Redis
- WorkflowCore (可选)
- RUINOR.WinFormsUI.CustomPictureBox (MagicPictureBox组件)

> **重要提示**：`HttpWebService`已被标记为过时，不再推荐使用。新功能开发应使用`FileManagementController`和`MagicPictureBox`控件来处理文件上传下载操作，这是当前系统中更推荐的图片处理方式。