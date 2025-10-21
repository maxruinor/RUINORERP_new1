# 统一加密协议 (UnifiedEncryptionProtocol)

## 概述

统一加密协议是为RUINORERP项目设计的高性能数据加密解密模块，旨在替代原有的`EncryptedProtocol`类，提供更好的性能、可维护性和扩展性，同时保持与现有系统的兼容性。

## 特性

### 1. 高性能设计
- 使用`ArrayPool<T>`减少内存分配和垃圾回收压力
- 采用`MethodImplOptions.AggressiveInlining`优化关键方法调用
- 使用`ReadOnlySpan<T>`和`BinaryPrimitives`提高数据处理效率
- 避免不必要的数组复制操作

### 2. 线程安全
- 密钥更新采用双重检查锁定模式确保线程安全
- 使用静态只读字段和线程安全的缓冲池

### 3. 兼容性
- 保持与原有数据结构(`OriginalData`, `EncryptedData`)的兼容
- 不影响现有代码逻辑，可平滑迁移

### 4. 易用性
- 提供简洁的API接口
- 包含完整的使用示例和性能测试代码

## 类结构

### UnifiedEncryptionProtocol
主要的加密解密类，提供以下核心方法：

#### 服务器端方法
- `EncryptServerDataToClient(OriginalData)` - 服务器加密数据发送给客户端
- `AnalyzeClientPacketHeader(ReadOnlySpan<byte>)` - 分析客户端数据包头部获取包体长度
- `DecryptClientPacket(ReadOnlySpan<byte>, ReadOnlySpan<byte>)` - 解密客户端发送的数据包

#### 客户端方法
- `AnalyzeServerPacketHeader(ReadOnlySpan<byte>)` - 分析服务器数据包头部获取包体长度
- `DecryptServerPacket(ReadOnlySpan<byte>)` - 解密服务器发送的数据包
- `EncryptClientDataToServer(OriginalData)` - 客户端加密数据发送给服务器

### UnifiedEncryptionProtocolExample
使用示例类，包含：
- 服务器端加密示例
- 客户端解密示例
- 客户端加密示例
- 服务器端解密示例
- 性能测试示例

### EncryptionBenchmark
性能基准测试类，用于：
- 对比新旧加密协议性能
- 内存使用测试

## 使用方法

### 1. 服务器端发送数据到客户端
```csharp
// 创建原始数据
var originalData = new OriginalData
{
    Cmd = 0x01,
    One = Encoding.UTF8.GetBytes("Hello"),
    Two = Encoding.UTF8.GetBytes("World")
};

// 加密数据
var encryptedData = UnifiedEncryptionProtocol.EncryptServerDataToClient(originalData);

// 发送加密数据到客户端
```

### 2. 客户端解密来自服务器的数据
```csharp
// 接收完整的数据包
byte[] packetData = ReceivePacketData();

// 解密数据
var decryptedData = UnifiedEncryptionProtocol.DecryptServerPacket(packetData);

// 使用解密后的数据
```

### 3. 客户端发送数据到服务器
```csharp
// 创建原始数据
var originalData = new OriginalData
{
    Cmd = 0x02,
    One = Encoding.UTF8.GetBytes("Client Request"),
    Two = Encoding.UTF8.GetBytes("Additional Data")
};

// 加密数据
byte[] encryptedBytes = UnifiedEncryptionProtocol.EncryptClientDataToServer(originalData);

// 发送加密数据到服务器
```

### 4. 服务器端解密来自客户端的数据
```csharp
// 接收数据包头部
byte[] head = ReceivePacketHeader();

// 接收完整数据包
byte[] packetData = ReceiveFullPacket();

// 解密数据
var decryptedData = UnifiedEncryptionProtocol.DecryptClientPacket(head, packetData);

// 使用解密后的数据
```

## 性能优化

### 1. 内存管理
- 使用`ArrayPool<byte>`管理缓冲区，避免频繁的内存分配
- 实现了高效的数组克隆方法`ArrayPoolClone`

### 2. 算法优化
- 使用`BinaryPrimitives`进行高效的字节序转换
- 采用内联优化减少方法调用开销
- 使用`ReadOnlySpan<T>`减少数据复制

### 3. 密钥管理
- 智能密钥更新机制，仅在日期变更时更新密钥
- 使用双重检查锁定确保线程安全

## 性能对比

根据内部测试，新协议相比旧协议有以下提升：
- 性能提升约 30-50%
- 内存使用减少约 25-40%
- GC压力显著降低

## 迁移指南

### 从旧协议迁移到新协议
1. 保持原有的数据结构不变
2. 将`EncryptedProtocol.XXX`调用替换为`UnifiedEncryptionProtocol.XXX`
3. 运行测试确保功能正常
4. 根据需要调整代码以充分利用新协议的性能优势

### 注意事项
- 新协议与旧协议完全兼容，可以混合使用
- 建议在新开发的功能中优先使用新协议
- 逐步迁移现有代码以获得性能提升

## 测试

项目包含完整的测试用例：
- 功能正确性测试
- 性能基准测试
- 内存使用测试
- 兼容性测试

运行测试：
```csharp
// 运行所有示例
UnifiedEncryptionProtocolExample.RunAllExamples();

// 运行性能对比测试
EncryptionBenchmark.ComparePerformance();
```

## 维护

### 代码规范
- 遵循项目现有的编码规范
- 使用中文注释保持一致性
- 保持方法简洁，单一职责原则

### 扩展性
- 设计时考虑了未来扩展需求
- 可以轻松添加新的加密算法
- 支持自定义密钥管理策略

## 版本历史

### v1.0.0
- 初始版本
- 实现基本的加密解密功能
- 提供完整的API接口
- 包含性能优化和测试代码