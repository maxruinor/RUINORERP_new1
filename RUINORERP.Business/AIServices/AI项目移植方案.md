# AI项目移植方案

## 1. 方案概述

### 1.1 背景
- 原 `RUINORERP.AI` 项目仅包含基础的Ollama服务调用
- 需要支持多种LLM提供商（本地Ollama、外部API）
- 需要复用现有的配置体系，避免重复实现配置管理

### 1.2 目标
- 将AI服务从独立项目迁移到 `RUINORERP.Business` 项目
- 复用现有的 `SystemGlobalConfig` 配置体系
- 提供统一的LLM服务接口，支持多种实现
- 为后续数据导入智能映射功能提供基础

## 2. 架构设计

### 2.1 整体架构
```
┌─────────────────────────────────────────────────────────────┐
│                    配置体系（已存在）                          │
│  SystemGlobalConfig ──► 包含AI配置项                          │
│       ▲                                                     │
│       │ 通过依赖注入/服务定位                                  │
├───────┼─────────────────────────────────────────────────────┤
│       │         RUINORERP.Business                           │
│       │    ┌─────────────────────────┐                       │
│       └────┤    AI服务实现            │                       │
│            │  - OllamaService        │                       │
│            │  - OpenAIService        │                       │
│            │  - LLMServiceFactory    │                       │
│            └─────────────────────────┘                       │
│                      ▲                                      │
│            ┌────────┴────────┐                              │
│            │   ILLMService    │ ◄── 接口定义                 │
│            │    接口          │                              │
│            └─────────────────┘                              │
└─────────────────────────────────────────────────────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        ▼                     ▼                     ▼
   ┌─────────┐          ┌─────────┐           ┌─────────┐
   │   UI    │◄────────►│ Server  │◄─────────►│  LLM    │
   │ (调用API)│   网络    │(业务路由)│   HTTP    │ (本地/云) │
   └─────────┘          └─────────┘           └─────────┘
```

### 2.2 目录结构
```
RUINORERP.Business/
├── AIServices/
│   ├── ILLMService.cs              # LLM服务接口
│   ├── OllamaService.cs            # Ollama实现
│   ├── OpenAIService.cs            # 外部API实现（预留）
│   ├── LLMServiceFactory.cs        # 服务工厂
│   ├── Models/                     # AI相关模型
│   │   ├── ChatModels.cs           # 聊天请求/响应模型
│   │   └── GenerateModels.cs       # 生成请求/响应模型
│   └── DataImport/                 # 数据导入AI服务
│       ├── IIntelligentMappingService.cs
│       └── ColumnMappingService.cs
└── AI项目移植方案.md
```

## 3. 配置集成

### 3.1 扩展现有配置
在 `SystemGlobalConfig` 中已存在基础AI配置：
- `OllamaApiAddress`: Ollama API地址
- `OllamaDefaultModel`: 默认模型名称

### 3.2 配置读取方式
```csharp
// 通过配置服务获取（复用现有配置体系）
public class OllamaService : ILLMService
{
    private readonly SystemGlobalConfig _config;
    
    public OllamaService()
    {
        _config = ConfigManager.GetConfig<SystemGlobalConfig>();
    }
}
```

## 4. 服务接口设计

### 4.1 ILLMService 接口
- `GetModelsAsync()`: 获取可用模型列表
- `ChatAsync()`: 发送聊天请求
- `GenerateAsync()`: 生成文本响应

### 4.2 工厂模式
- `LLMServiceFactory.Create()`: 根据配置创建对应的服务实例
- 支持Ollama、OpenAI等多种实现

## 5. 迁移步骤

1. **创建目录结构**: 在Business项目创建AIServices目录
2. **创建接口**: 定义ILLMService接口
3. **迁移服务**: 将OllamaService迁移到Business项目，优化配置读取
4. **创建工厂**: 实现LLMServiceFactory
5. **更新引用**: Server项目引用Business项目的AI服务
6. **删除原项目**: 删除RUINORERP.AI项目

## 6. 后续扩展

### 6.1 数据导入功能
- 基于AI服务实现智能列映射
- 支持Excel数据导入时的字段自动匹配

### 6.2 其他AI功能
- 智能数据分析
- 自然语言查询
- 智能提醒优化
