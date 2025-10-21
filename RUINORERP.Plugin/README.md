# RUINORERP 插件通信通道机制

## 概述

本文档说明了如何在RUINORERP系统中实现插件与主程序之间的数据交互。通过通信通道机制，插件可以与主程序进行双向数据交换，实现更复杂的功能。

## 核心组件

### 1. IPluginCommunicationChannel 接口

定义了插件与主程序之间的通信方法：

- `SendDataToHost`: 发送数据到主程序
- `RequestDataFromHost`: 从主程序请求数据
- `InvokeHostService`: 调用主程序服务

### 2. PluginCommunicationChannel 类

这是[IPluginCommunicationChannel](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Plugin/IPlugin.cs#L67-L87)接口的默认实现，使用事件机制来处理通信。

### 3. 事件参数类

- `PluginDataEventArgs`: 插件发送数据事件参数
- `PluginDataRequestEventArgs`: 插件请求数据事件参数
- `PluginServiceInvokeEventArgs`: 插件调用服务事件参数

## 使用方法

### 在插件中使用通信通道

1. 插件基类已经提供了[CommunicationChannel](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Plugin/PluginBase.cs#L49-L49)属性，可以直接使用
2. 通过[CommunicationChannel](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Plugin/PluginBase.cs#L49-L49)属性调用相应的方法进行数据交互

```csharp
// 发送数据到主程序
CommunicationChannel?.SendDataToHost(this.Name, data);

// 从主程序请求数据
var response = CommunicationChannel?.RequestDataFromHost(this.Name, request);

// 调用主程序服务
var result = CommunicationChannel?.InvokeHostService(this.Name, "ServiceName", parameters);
```

### 在主程序中处理插件通信

1. 创建[PluginCommunicationChannel](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Plugin/PluginCommunicationChannel.cs#L10-L65)实例
2. 订阅相应的事件
3. 在事件处理程序中实现具体的业务逻辑

```csharp
var communicationChannel = new PluginCommunicationChannel();

// 订阅插件发送数据事件
communicationChannel.DataReceivedFromPlugin += (sender, e) => {
    // 处理插件发送的数据
    Console.WriteLine($"收到来自插件 {e.PluginName} 的数据: {e.Data}");
};

// 订阅插件请求数据事件
communicationChannel.DataRequestedFromHost += (sender, e) => {
    // 处理插件的数据请求，设置响应数据
    e.Response = GetDataForPlugin(e.PluginName, e.Request);
};

// 订阅插件服务调用事件
communicationChannel.ServiceInvokedByPlugin += (sender, e) => {
    // 处理插件的服务调用请求
    e.Result = InvokeService(e.PluginName, e.Service, e.Parameters);
};

// 将通信通道设置到插件管理器
pluginManager.SetCommunicationChannel(communicationChannel);
```

## 示例插件

[SampleDataInteractionPlugin.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Plugin/SampleDataInteractionPlugin.cs)文件中提供了一个完整的示例，演示了如何在插件中使用通信通道进行各种数据交互操作。

## 扩展性

通信通道机制设计具有良好的扩展性：

1. 可以通过实现[IPluginCommunicationChannel](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Plugin/IPlugin.cs#L67-L87)接口创建自定义通信通道
2. 可以根据需要添加新的通信方法
3. 事件参数类可以根据具体需求进行扩展

## 注意事项

1. 所有通信方法都应处理异常情况
2. 插件不应假设主程序一定会响应其请求
3. 通信数据建议使用[Dictionary<string, object>](file:///C:/Program%20Files/JetBrains/GoLand%202022.2.2/plugins/go-plugin/lib/sdk/sources/go/bin/src/strings/strings.go#L124-L126)等灵活的数据结构，以适应不同类型的数据
4. 在生产环境中，应考虑添加安全验证机制