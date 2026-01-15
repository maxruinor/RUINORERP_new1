# WebView2 集成和 URL 路由完成报告

## 概述

本报告总结了 WebView2 集成和 URL 路由设计的实施情况，这是智能帮助系统中期规划的核心功能。

## 实施内容

### 1. WebView2 帮助面板 (WebView2HelpPanel.cs)

#### 主要功能

- **现代化浏览器控件**: 使用 Microsoft Edge WebView2 控件替代传统的 WebBrowser
- **Markdown 渲染**: 自动识别并渲染 Markdown 格式的帮助内容
- **代码语法高亮**: 支持代码块的语法高亮显示
- **导航功能**: 后退、前进、刷新导航按钮
- **缩放控制**: 支持页面缩放（Ctrl +/-）
- **降级机制**: WebView2 初始化失败时自动降级到传统 WebBrowser

#### 核心特性

1. **智能内容识别**
   - 自动检测 Markdown 语法标记
   - 选择合适的渲染方式

2. **增强的工具栏**
   - 关闭、后退、前进、刷新
   - 缩放控制（+/-）
   - 打印、打开 CHM

3. **JavaScript 注入**
   - 代码语法高亮脚本
   - 链接在新窗口打开
   - 禁用右键菜单

4. **快捷键支持**
   - ESC: 关闭
   - Ctrl+P: 打印
   - F5: 刷新
   - Ctrl+/-: 缩放

### 2. Markdown 渲染器 (MarkdownRenderer.cs)

#### 支持的 Markdown 语法

1. **标题**
   - ATX 风格: # ## ### #### ##### ######

2. **文本格式**
   - 粗体: **text** 或 __text__
   - 斜体: *text* 或 _text_
   - 删除线: ~~text~~
   - 行内代码: `code`

3. **块级元素**
   - 代码块: ```language code ```
   - 引用块: > text
   - 水平线: --- 或 ***

4. **列表**
   - 有序列表: 1. 2. 3.
   - 无序列表: - 或 *

5. **表格**
   - 简单的 Markdown 表格支持

6. **链接和图片**
   - 链接: [text](url)
   - 图片: ![alt](url)

7. **特殊提示框**
   - [NOTE]: 提示信息
   - [TIP]: 技巧提示
   - [WARNING]: 警告信息
   - [INFO]: 信息提示

### 3. URL 路由管理器 (HelpUrlRouter.cs)

#### 核心功能

1. **路由系统**
   - 基于正则表达式的 URL 匹配
   - 可扩展的路由规则注册机制
   - 多种 URL 类型支持

2. **支持的路由模式**

   **本地文件路由**
   - `help://local/form/{formName}` - 窗体帮助
   - `help://local/control/{formName}/{controlName}` - 控件帮助
   - `help://local/field/{entityName}/{fieldName}` - 字段帮助
   - `help://local/module/{moduleName}` - 模块帮助
   - `help://file/{path}` - 直接文件路径

   **远程帮助路由**
   - `help://remote/api/help/{helpKey}` - 远程 API
   - `help://remote/page/{pagePath}` - 远程页面

   **HTTP 远程链接**
   - `https://example.com/help/*` - 自定义帮助服务器
   - `https://*.github.io/ruinorerp-help/*` - GitHub Pages

3. **URL 构建**
   - 根据帮助级别自动构建 URL
   - 支持本地和远程 URL 切换
   - 相对路径和绝对路径支持

4. **配置选项**
   - 本地帮助文件根目录
   - 远程帮助服务器 URL
   - 远程帮助开关

### 4. HelpManager 集成

#### 新增功能

1. **WebView2 支持**
   - 可配置启用/禁用 WebView2
   - 自动降级到传统 WebBrowser

2. **远程帮助支持**
   - 远程帮助开关
   - 远程帮助服务器 URL 配置

3. **URL 帮助显示**
   - `ShowUrlHelpAsync(string url)` - 显示 URL 帮助
   - `BuildHelpUrl(string helpKey, HelpLevel level, bool useRemote)` - 构建帮助 URL

## 使用示例

### 1. 启用 WebView2 和远程帮助

```csharp
// 在应用程序初始化时配置 HelpManager
var helpManager = HelpManager.Instance;

// 启用 WebView2
helpManager.UseWebView2 = true;

// 启用远程帮助
helpManager.EnableRemoteHelp = true;
helpManager.RemoteHelpUrl = "https://help.yourdomain.com/";
```

### 2. 显示 URL 帮助

```csharp
// 显示本地文件帮助
await HelpManager.Instance.ShowUrlHelpAsync("help://local/form/UCSaleOrder");

// 显示远程帮助
await HelpManager.Instance.ShowUrlHelpAsync("help://remote/api/help/UCSaleOrder");

// 显示 HTTP 远程链接
await HelpManager.Instance.ShowUrlHelpAsync("https://help.example.com/sales/order");
```

### 3. 构建帮助 URL

```csharp
// 构建本地帮助 URL
string localUrl = HelpManager.Instance.BuildHelpUrl(
    "UCSaleOrder",
    HelpLevel.Form,
    useRemote: false
);
// 结果: help://local/form/UCSaleOrder

// 构建远程帮助 URL
string remoteUrl = HelpManager.Instance.BuildHelpUrl(
    "UCSaleOrder",
    HelpLevel.Form,
    useRemote: true
);
// 结果: https://help.yourdomain.com/api/help/form/UCSaleOrder
```

### 4. 创建 Markdown 帮助内容

```markdown
# 销售订单

## 概述
销售订单模块用于管理客户订单信息，包括订单创建、修改、审批等功能。

## 主要功能

### 创建订单
1. 点击"新建"按钮
2. 填写客户信息
3. 添加订单明细
4. 保存订单

### 订单状态
- **草稿**: 订单创建后的初始状态
- **已提交**: 订单已提交审批
- **已审批**: 订单已通过审批
- **已拒绝**: 订单审批未通过

## 字段说明

| 字段名 | 说明 | 必填 |
|--------|------|------|
| 订单号 | 系统自动生成的唯一标识 | 是 |
| 客户名称 | 下单客户 | 是 |
| 订单日期 | 订单创建日期 | 是 |
| 总金额 | 订单总金额 | 是 |

## 代码示例

```csharp
// 创建销售订单
var order = new tb_SaleOrder();
order.CustomerID = customerId;
order.OrderDate = DateTime.Now;
order.TotalAmount = totalAmount;

await _saleOrderService.AddAsync(order);
```

> [NOTE] 订单编号由系统自动生成，无需手动输入。

> [WARNING] 删除订单将同时删除所有关联的订单明细。
```

### 5. 自定义路由规则

```csharp
// 获取 URL 路由器
var router = new HelpUrlRouter(localPath, remoteUrl);

// 注册自定义路由
router.RegisterRoute(
    @"^custom://help/(.+)$",
    (url, match) =>
    {
        string helpKey = match.Groups[1].Value;
        // 自定义处理逻辑
        return HelpUrlResolutionResult.Success(
            $"https://custom-help.com/{helpKey}",
            HelpUrlType.RemotePage
        );
    }
);

// 解析 URL
var result = router.ResolveUrl("custom://help/UCSaleOrder");
```

## 文件结构

```
RUINORERP.UI/HelpSystem/
├── Components/
│   ├── WebView2HelpPanel.cs       # WebView2 帮助面板
│   ├── MarkdownRenderer.cs         # Markdown 渲染器
│   ├── HelpPanel.cs                # 传统 WebBrowser 面板（保留）
│   ├── HelpTooltip.cs              # 智能提示
│   ├── DefaultHelpContentGenerator.cs
│   └── FieldNameRecognizer.cs
├── Core/
│   ├── HelpManager.cs              # 帮助管理器（已更新）
│   ├── HelpContext.cs              # 帮助上下文（已更新）
│   ├── HelpUrlRouter.cs            # URL 路由管理器（新增）
│   ├── HelpLevel.cs
│   ├── HelpSearchResult.cs
│   ├── IHelpProvider.cs
│   ├── LocalHelpProvider.cs
│   ├── SmartHelpResolver.cs
│   ├── SmartHelpResolverEnhanced.cs
│   └── HelpContentMonitor.cs
└── Extensions/
    └── HelpExtensions.cs
```

## 技术要点

### 1. WebView2 初始化

```csharp
// 异步初始化 WebView2 环境
private async void InitializeWebView2Async()
{
    var environment = await CoreWebView2Environment.CreateAsync();
    await _webView2.EnsureCoreWebView2Async(environment);
    ConfigureWebView2();
}
```

### 2. JavaScript 注入

```csharp
// 注入代码高亮脚本
string script = @"
    function highlightCode() {
        var codeBlocks = document.querySelectorAll('pre code');
        // 高亮逻辑...
    }
";
await _webView2.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(script);
```

### 3. 路由匹配

```csharp
// 遍历路由规则
foreach (var route in _routes)
{
    var match = Regex.Match(url, route.Key, RegexOptions.IgnoreCase);
    if (match.Success)
    {
        return route.Value(url, match); // 调用处理器
    }
}
```

## 兼容性

### 最低要求

- .NET Framework 4.6.2 或更高
- Microsoft Edge WebView2 Runtime（可选，未安装时自动降级）

### 降级策略

当 WebView2 不可用时：
1. 自动降级到传统 WebBrowser
2. 禁用需要 WebView2 的功能（如远程帮助）
3. 显示友好的提示信息

## 性能优化

1. **异步加载**
   - WebView2 异步初始化
   - 帮助内容异步加载

2. **缓存机制**
   - 帮助内容缓存
   - 路由解析结果缓存

3. **资源释放**
   - 正确释放 WebView2 资源
   - 防止内存泄漏

## 测试建议

### 1. 功能测试

- [ ] WebView2 帮助面板正常显示
- [ ] Markdown 内容正确渲染
- [ ] 代码语法高亮正常工作
- [ ] 导航功能正常（后退/前进/刷新）
- [ ] 缩放功能正常

### 2. 路由测试

- [ ] 本地文件路由解析正确
- [ ] 远程帮助路由解析正确
- [ ] HTTP URL 路由解析正确
- [ ] 自定义路由规则生效

### 3. 降级测试

- [ ] WebView2 未安装时降级到 WebBrowser
- [ ] 降级后基本功能正常

### 4. 兼容性测试

- [ ] 不同 Windows 版本测试
- [ ] 不同 WebView2 版本测试

## 后续规划

### 短期（已完成）

- ✅ WebView2 集成
- ✅ Markdown 渲染
- ✅ 代码语法高亮
- ✅ URL 路由设计

### 中期（待实施）

- 🔄 远程帮助 API 集成
- 🔄 帮助内容在线更新
- 🔄 帮助搜索优化

### 长期（待实施）

- ⏸️ 模块化管理界面
- ⏸️ 用户反馈系统
- ⏸️ 帮助内容版本管理
- ⏸️ 离线模式支持

## 总结

本次实施成功完成了 WebView2 集成和 URL 路由设计，为智能帮助系统提供了：

1. **现代化的帮助显示体验** - WebView2 提供更好的渲染性能和用户体验
2. **灵活的内容格式** - 支持 Markdown、HTML 等多种格式
3. **强大的路由系统** - 支持本地文件和远程帮助
4. **良好的兼容性** - 自动降级机制确保系统稳定

系统架构具有良好的扩展性，为后续功能开发奠定了坚实基础。
