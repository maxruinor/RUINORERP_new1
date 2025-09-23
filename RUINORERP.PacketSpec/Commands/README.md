# Commands 目录说明

## 目录结构

```
Commands/
├── System/                  # 系统级（心跳、握手、协议协商…）
│   ├── HeartbeatCommand.cs
│   └── ProtocolNegotiateCommand.cs
├── Authentication/          # 认证相关
│   ├── LoginCommand.cs
│   ├── LogoutCommand.cs
│   └── RefreshTokenCommand.cs
├── Cache/                   # 缓存同步
│   ├── CacheSyncCommand.cs
│   └── CacheInvalidateCommand.cs
├── FileTransfer/            # 文件上传下载
│   ├── FileUploadCommand.cs
│   ├── FileDownloadCommand.cs
│   └── FileDeleteCommand.cs
├── Message/                 # 消息/通知
│   ├── SendPopupMessageCommand.cs
│   └── BroadcastMessageCommand.cs
├── Lock/                    # 单据锁
│   ├── LockApplyCommand.cs
│   └── LockReleaseCommand.cs
└── Workflow/                # 工作流
    ├── WorkflowApproveCommand.cs
    └── WorkflowRejectCommand.cs
```

## 设计原则

### ✅ 应该放在 Commands 目录的内容

1. **命令契约** - 只包含类声明、属性（DTO）
2. **命令标识符** - CommandId 定义
3. **基础校验** - Validate() 方法（非业务逻辑）
4. **零业务逻辑** - 不包含任何业务处理代码
5. **零数据库操作** - 不直接访问数据库
6. **零外部调用** - 不调用外部服务

### ❌ 不应该放在 Commands 目录的内容

| 内容 | 应该放在 |
|------|----------|
| 业务校验（"用户名是否存在"） | 服务端 Handler |
| 弹窗/跳转/保存本地缓存 | 客户端 Handler |
| SQL、Redis、文件读写 | 各自项目内部 |
| 加密/压缩具体实现 | 公共 Security/ 只给接口/工具，不调业务 |

## 命名规范

1. **文件命名** - 使用 PascalCase，以 Command 结尾（如 LoginCommand.cs）
2. **类命名** - 与文件名一致
3. **命令ID** - 集中定义在各业务领域命令定义文件中
4. **目录命名** - 使用业务领域名称（如 Authentication、Cache 等）

## 一句话总结

**PacketSpec/Commands 是"通信字典"，只收"词汇"，不收"句子"；业务"造句"请回各自项目里的 Handlers 去完成。**