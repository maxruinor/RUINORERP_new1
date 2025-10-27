# MagicPictureBox 控件使用说明文档

## 1. 控件概述

`MagicPictureBox` 是一个增强版的 PictureBox 控件，专为企业级 ERP 系统设计，提供了丰富的图片处理功能，包括多图片支持、图片上传优化（只上传变更图片）、哈希值计算、图片裁剪、缩放等高级功能。

## 2. 主要功能特性

- **多图片支持**：支持在单个控件中显示和管理多张图片
- **图片变更跟踪**：自动跟踪图片的变更状态，只上传修改过的图片
- **安全的图片唯一性验证**：使用 SHA256 算法计算图片哈希值
- **丰富的图片操作**：支持裁剪、缩放、旋转、拖拽等操作
- **图片信息管理**：完整的图片元数据管理，包括文件名、大小、哈希值等
- **自动生成文件名**：基于业务信息生成唯一的文件名

## 3. 核心属性

### 3.1 基础属性

- **MultiImageSupport**：是否启用多图片支持
- **ImagePaths**：图片路径字符串，多图片模式下使用分号分隔多个路径

### 3.2 图片信息相关属性

- **ImageInfo**：图片信息类，包含以下关键属性：
  - **FileName**：图片文件名
  - **FileSize**：图片文件大小（字节）
  - **HashValue**：图片内容的 SHA256 哈希值
  - **IsUpdated**：标记图片是否已更新
  - **Metadata**：用于存储额外信息，如更新标记

## 4. 主要方法

### 4.1 图片加载方法

```csharp
// 从字节数组加载多张图片
public void LoadImagesFromBytes(List<byte[]> imageBytesList, List<string> fileNamesList = null, bool isFromServer = false)

// 从文件存储信息加载图片
public void LoadImagesFromFileStorageInfos(List<FileStorageInfo> fileStorageInfos)

// 从文件存储信息加载单张图片
public void LoadImageFromFileStorageInfo(FileStorageInfo fileStorageInfo)
```

### 4.2 图片上传优化方法

```csharp
// 获取已更新图片的字节数据和信息
public List<Tuple<byte[], ImageInfo>> GetUpdatedImageBytesWithInfo(ImageFormat format = null)

// 获取需要更新的图片
public List<Tuple<byte[], ImageInfo>> GetImagesNeedingUpdate()

// 标记图片为已更新
public void MarkImageAsUpdated(int index)

// 检查图片是否需要更新
public bool IsImageNeedingUpdate(int index)

// 重置图片更新状态
public void ResetImageUpdateStatus(int index)

// 重置所有图片的更新状态
public void ResetAllImageUpdateStatuses()
```

### 4.3 图片处理方法

```csharp
// 添加图片
public void AddImage(string imagePath)

// 删除当前图片
public void DeleteCurrentImage()

// 清空图片
public void ClearImage()

// 旋转图片
public void RotateImage(float angle)

// 开始裁剪操作
public void StartCrop()

// 保存裁剪后的图片
public void SaveCrop()
```

### 4.4 哈希计算方法

```csharp
// 计算图片内容的哈希值
public string CalculateImageHash(byte[] imageBytes)

// 比较两张图片是否相同（基于哈希值）
public bool AreImagesEqual(byte[] imageBytes1, byte[] imageBytes2)
```

## 5. 辅助类

### 5.1 BusinessImageManager

负责处理业务单据和图片之间的关联关系，提供以下主要功能：

- 生成唯一标识符和文件名
- 创建业务图片信息
- 检测重复图片
- 处理业务单据更新时的图片变更

```csharp
// 处理业务单据更新时的图片变更
public ImageChangeResult ProcessBusinessImageChanges(int businessType, long businessId, List<ImageInfo> oldImages, List<Image> newImages)
```

### 5.2 ImageUpdateManager

负责跟踪图片的更新状态和检测变更：

```csharp
// 标记图片为已更新
public void MarkImageAsUpdated(ImageInfo imageInfo)

// 检查图片是否需要更新
public bool IsImageNeedingUpdate(ImageInfo imageInfo)

// 重置图片更新状态
public void ResetImageUpdateStatus(ImageInfo imageInfo)

// 检查图片内容是否发生变化
public bool HasImageContentChanged(ImageInfo originalInfo, byte[] newImageBytes)
```

## 6. 使用示例

### 6.1 基本使用

```csharp
// 创建并配置 MagicPictureBox
var magicPictureBox = new MagicPictureBox
{
    MultiImageSupport = true,
    SizeMode = PictureBoxSizeMode.Zoom,
    Dock = DockStyle.Fill
};

// 添加到窗体
this.Controls.Add(magicPictureBox);

// 加载图片
magicPictureBox.LoadImagesFromBytes(imageBytesList, fileNamesList, isFromServer: true);
```

### 6.2 只上传变更图片的实现

```csharp
// 保存业务单据时，只上传变更的图片
private async Task SaveBusinessWithImagesAsync()
{
    try
    {
        // 获取需要更新的图片
        var updatedImages = magicPictureBox.GetUpdatedImageBytesWithInfo();
        
        if (updatedImages.Count > 0)
        {
            // 上传变更的图片
            await UploadImagesAsync(updatedImages);
            
            // 上传成功后重置更新状态
            magicPictureBox.ResetAllImageUpdateStatuses();
        }
        
        // 保存业务数据
        await SaveBusinessDataAsync();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

### 6.3 使用业务图片管理器处理图片变更

```csharp
// 创建业务图片管理器
var hashCalculator = new SHA256HashCalculator();
var imageProcessor = new ImageProcessor();
var updateManager = new ImageUpdateManager(hashCalculator, imageProcessor);
var businessImageManager = new BusinessImageManager(hashCalculator, imageProcessor, updateManager);

// 处理图片变更
var changeResult = businessImageManager.ProcessBusinessImageChanges(
    businessType: 1, // 业务类型
    businessId: businessId,
    oldImages: existingImageInfos,
    newImages: newImages);

// 根据变更结果进行相应操作
if (changeResult.HasChanges)
{
    // 处理新增图片
    foreach (var (image, info) in changeResult.ImagesToAdd)
    {
        await UploadImageAsync(image, info);
    }
    
    // 处理更新图片
    foreach (var (image, info) in changeResult.ImagesToUpdate)
    {
        await UpdateImageAsync(image, info);
    }
    
    // 处理删除图片
    foreach (var info in changeResult.ImagesToDelete)
    {
        await DeleteImageAsync(info.FileId);
    }
}
```

## 7. 最佳实践

1. **启用多图片支持**：对于需要管理多张图片的场景，启用 MultiImageSupport 属性
2. **正确设置 isFromServer 参数**：从服务器加载图片时，将 isFromServer 设置为 true，这样 IsUpdated 会被正确初始化为 false
3. **使用哈希值验证**：利用 CalculateImageHash 方法计算的哈希值来验证图片内容是否相同
4. **保存后重置更新状态**：图片成功上传后，调用 ResetAllImageUpdateStatuses 重置更新状态
5. **使用业务图片管理器**：对于复杂的业务场景，使用 BusinessImageManager 来处理图片与业务单据的关系

## 8. 注意事项

1. **内存管理**：处理大量图片时，注意及时释放不再使用的图片资源
2. **异常处理**：图片处理操作可能抛出异常，确保添加适当的异常处理
3. **图片格式**：默认使用 JPEG 格式，可根据需要指定其他格式
4. **性能考虑**：对于大图，考虑在上传前进行压缩处理
5. **哈希计算**：SHA256 哈希计算会消耗一定资源，对于特别大的图片需注意性能影响

## 9. 版本历史

- **1.0.0**：初始版本，实现基本功能
- **1.1.0**：新增只上传变更图片功能，优化图片处理性能
- **1.2.0**：添加业务图片管理器，增强业务单据与图片的关联管理

## 10. 支持与维护

如有任何问题或需要进一步的功能支持，请联系开发团队。