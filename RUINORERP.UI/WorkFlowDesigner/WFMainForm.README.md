# 工作流设计器入口说明

## 概述

WFMainForm.cs 是工作流设计器的主入口文件，基于Netron图形控件构建。该窗体提供了完整的图形化流程设计环境，支持创建、编辑、保存和执行各种业务流程。

## 功能特性

### 1. 图形化设计环境
- 拖拽式节点添加
- 连接线绘制
- 节点属性配置
- 流程图布局调整

### 2. 节点类型支持
- 开始节点
- 结束节点
- 审批节点
- 提交节点
- 通知节点
- 条件判断节点
- 会签节点（新增）
- 或签节点（新增）

### 3. 流程管理
- 流程保存与加载
- 流程导出为XML/JSON格式
- 流程版本管理

### 4. 与WorkflowCore集成
- 流程定义转换
- 流程执行测试
- 流程监控

## 使用说明

### 启动设计器
运行应用程序后，WFMainForm将作为主界面自动加载，显示工作流设计环境。

### 创建新流程
1. 点击"文件"菜单中的"新建"选项
2. 从左侧"流程设计元素库"中拖拽节点到画布
3. 连接节点形成流程图
4. 双击节点配置属性
5. 保存流程设计

### 配置会签/或签节点
1. 从工具箱拖拽会签或或签节点到画布
2. 双击节点打开属性面板
3. 点击"审批人员"属性，打开审批人员编辑器
4. 添加审批人员并保存

### 导出流程
1. 点击"文件"菜单中的"导出"选项
2. 选择导出格式（XML或JSON）
3. 保存文件

## 技术架构

### 核心组件
- **Netron GraphLib**: 图形化设计核心库
- **Mediator**: 中间协调器，管理各组件间通信
- **WorkflowDefinitionConverter**: 流程定义转换器
- **WorkflowDefinitionService**: 流程定义持久化服务

### 扩展节点
- **CountersignNode**: 会签节点
- **OrSignNode**: 或签节点

### 数据模型
- **WorkflowDefinition**: 流程定义
- **WorkflowStep**: 流程步骤
- **WorkflowConnection**: 步骤连接
- **CountersignStep**: 会签步骤
- **OrSignStep**: 或签步骤

## 开发说明

### 代码结构
- WFMainForm.cs: 主窗体逻辑
- WFMainForm.Designer.cs: 界面设计器代码
- Mediator.cs: 中间协调器
- Nodes/: 节点类定义
- Entities/: 数据模型
- Service/: 服务类
- UI/: 用户界面组件

### 扩展开发
1. 添加新节点类型：继承BaseNode类
2. 添加新服务：在Service目录下创建新类
3. 添加UI组件：在UI目录下创建新控件

## 注意事项

1. 确保所有节点类正确注册到Netron控件中
2. 流程定义的持久化通过WorkflowDefinitionService处理
3. 与WorkflowCore的集成通过WorkflowDefinitionConverter实现
4. 所有扩展属性需要在节点的AddProperties方法中注册

## 最新更新

### 会签/或签功能已完成
- 会签节点和或签节点已正确集成到设计器中
- 节点属性面板支持审批人员配置
- 流程定义支持导出为WorkflowCore兼容的JSON/XML格式
- 数据库持久化功能已实现
- 与WorkflowCore的集成已完成