# 🔌 ERP系统集成指南

本文档说明如何将在线帮助系统集成到 RUINOR ERP 系统中。

## 🎯 集成架构

```
用户按F1
    ↓
RUINORERP.UI
    ↓
HelpManager
    ↓
EnhancedCompositeHelpProvider
    ├─ 在线 → WebHelpProvider → 打开网站
    └─ 离线 → LocalCacheHelpProvider → 显示本地缓存
```

## 📋 集成步骤

### 步骤1：部署帮助网站

#### 方式A：IIS部署（推荐）

```bash
# 1. 构建网站
cd RUINORERP.HelpSite
mkdocs build

# 2. 复制到IIS目录
xcopy site\* C:\inetpub\wwwroot\erp-help\ /s /e /y

# 3. 配置IIS
# - 创建网站，指向 C:\inetpub\wwwroot\erp-help\
# - 端口：80 或 8080
# - 访问：http://your-server/erp-help/
```

#### 方式B：内置服务器

```bash
# 在ERP服务器上运行
cd RUINORERP.HelpSite
mkdocs serve --dev-addr=0.0.0.0:8000

# 后台运行（使用nssm或类似工具）
```

#### 方式C：公网托管

使用 Vercel、Netlify 或 GitHub Pages 免费托管

### 步骤2：添加NuGet包

在 `RUINORERP.UI` 项目中添加：

```bash
# WebView2 运行时（如使用WebView2显示）
Install-Package Microsoft.Web.WebView2

# Markdig（Markdown渲染）
Install-Package Markdig
```

### 步骤3：初始化帮助系统

在程序启动时（如 `Main()` 或 `Program.cs`）：

```csharp
using RUINORERP.UI.HelpSystem;

public class Program
{
    [STAThread]
    static void Main()
    {
        // 初始化帮助系统
        HelpLauncher.Initialize(
            baseUrl: "http://your-server/erp-help/",  // 帮助网站URL
            cacheDirectory: @".\HelpContent"            // 本地缓存目录
        );
        
        // 测试连接
        if (HelpLauncher.TestOnlineConnection())
        {
            Console.WriteLine("帮助系统在线模式已就绪");
        }
        else
        {
            Console.WriteLine("帮助系统离线模式（使用本地缓存）");
        }
        
        Application.Run(new MainForm());
        
        // 关闭时清理
        HelpLauncher.Shutdown();
    }
}
```

### 步骤4：配置窗体帮助

以 `UCSaleOrder` 为例：

```csharp
namespace RUINORERP.UI.PSI.SAL
{
    [MenuAttrAssemblyInfo("销售订单", ...)]
    public partial class UCSaleOrder : BaseBillEditGeneric<tb_SaleOrder, tb_SaleOrderDetail>
    {
        public UCSaleOrder()
        {
            InitializeComponent();
            
            if (!this.DesignMode)
            {
                // 设置窗体帮助键（必需）
                FormHelpKey = "UCSaleOrder";
                
                // 启用智能帮助（必需）
                EnableSmartHelp = true;
            }
        }
    }
}
```

### 步骤5：验证集成

1. **启动ERP系统**
2. **打开销售订单窗体**
3. **按F1键**
4. **验证**：
   - 在线时：打开浏览器/WebView2显示帮助网站
   - 离线时：显示本地缓存的Markdown内容

## 🔧 配置选项

### 修改帮助网站URL

```csharp
// 运行时修改
HelpLauncher.SetBaseUrl("http://new-server/help/");

// 或在配置文件中
// App.config:
<appSettings>
    <add key="HelpBaseUrl" value="http://localhost:8000/" />
    <add key="HelpCacheDir" value=".\HelpContent" />
</appSettings>
```

### 强制使用本地帮助

```csharp
// 设置为false强制使用本地帮助（离线模式）
HelpLauncher.SetPreferOnline(false);
```

### 测试连接状态

```csharp
// 检查在线连接
bool isOnline = HelpLauncher.TestOnlineConnection();
MessageBox.Show($"网络状态: {(isOnline ? "在线" : "离线")}");

// 获取详细状态
string status = HelpLauncher.GetStatusInfo();
Console.WriteLine(status);
```

## 📁 新增文件清单

### 1. 帮助网站（MkDocs）

```
RUINORERP.HelpSite/
├── mkdocs.yml                          # MkDocs配置
├── README.md                           # 项目说明
├── deploy.bat                          # 部署脚本
├── start.bat                           # 启动脚本
├── docs/
│   ├── index.md                        # 首页
│   ├── quickstart/
│   │   ├── index.md
│   │   ├── login.md
│   │   ├── interface.md
│   │   ├── basic-operations.md
│   │   └── shortcuts.md
│   ├── modules/
│   │   ├── sales/index.md
│   │   ├── purchase/index.md
│   │   ├── inventory/index.md
│   │   └── finance/index.md
│   ├── forms/
│   │   └── UCSaleOrder.md              # 销售订单帮助
│   └── images/
│       └── README.md                   # 截图指南
```

### 2. UI集成代码（C#）

```
RUINORERP.UI/HelpSystem/
├── Core/
│   ├── WebHelpProvider.cs              # 在线Web帮助（新增）
│   ├── LocalCacheHelpProvider.cs       # 本地缓存帮助（新增）
│   ├── EnhancedCompositeHelpProvider.cs # 组合提供者（新增）
│   └── ...                             # 现有文件
└── HelpLauncher.cs                     # 帮助启动器（新增）
```

## 🧪 测试用例

### 测试1：在线帮助

**前置条件**：网络连接正常，帮助网站可访问

**步骤**：
1. 启动ERP系统
2. 打开销售订单窗体
3. 按F1键

**预期结果**：
- 打开WebView2或系统浏览器
- 显示帮助网站 `http://server/forms/UCSaleOrder/`

### 测试2：离线帮助

**前置条件**：断开网络连接

**步骤**：
1. 启动ERP系统
2. 打开销售订单窗体
3. 按F1键

**预期结果**：
- 显示本地缓存窗口
- 标题显示"RUINOR ERP 帮助（本地缓存）"
- 顶部显示离线提示

### 测试3：字段级帮助

**步骤**：
1. 打开销售订单窗体
2. 点击"客户"下拉框
3. 按F1键

**预期结果**：
- 显示客户字段的详细说明
- URL包含字段标识

## 🐛 故障排除

### 问题1：F1无响应

**原因**：帮助系统未初始化

**解决**：
```csharp
// 在Main()中添加
HelpLauncher.Initialize();
```

### 问题2：无法连接帮助网站

**原因**：URL配置错误或网站未启动

**解决**：
1. 检查 `HelpBaseUrl` 配置
2. 确认帮助网站可访问
3. 检查防火墙设置

### 问题3：本地缓存不显示

**原因**：缓存目录不存在或无权限

**解决**：
```csharp
// 确保目录存在
string cacheDir = @".\HelpContent";
if (!Directory.Exists(cacheDir))
{
    Directory.CreateDirectory(cacheDir);
}
```

### 问题4：WebView2不显示

**原因**：WebView2运行时未安装

**解决**：
1. 安装 WebView2 运行时
2. 或改用系统浏览器：`HelpLauncher.Provider.UseWebView2 = false`

## 📞 技术支持

遇到问题？

1. 查看日志：`HelpLauncher.GetStatusInfo()`
2. 检查网络：`HelpLauncher.TestOnlineConnection()`
3. 联系管理员或查看 README.md

---

**集成完成！** 🎉

用户现在可以按F1键获取帮助了！
