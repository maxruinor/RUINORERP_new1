# 阿里巴巴店铺管理插件

## 功能特性

- 集成WebView2浏览器控件，用于操作1688网店后台
- 支持订单数据抓取并通过通信通道发送给主程序
- 支持在页面上填写表单和提交操作
- 作为标准插件集成到现有的RUINORERP插件架构中
- 自动保存账号密码和cookies，支持自动登录

## 主要功能

### 1. 浏览器控制功能
- 后退/前进导航
- URL地址栏输入和导航
- 收藏夹管理
- 页面元素定位和操作

### 2. 数据提取功能
- 订单列表数据提取
- 订单详情数据提取
- 商品信息提取

### 3. 表单操作功能
- 登录表单填写和提交
- 发货表单填写和提交
- 其他业务表单操作

### 4. 账号管理功能
- 自动保存账号密码
- 自动保存和恢复cookies
- 支持自动登录
- 收藏夹管理

## 使用说明

1. 插件启动后会自动导航到1688工作台页面
2. 可以通过地址栏输入其他URL进行导航
3. 使用后退/前进按钮进行页面导航
4. 点击收藏按钮可以管理收藏的网址
5. 点击"登录设置"按钮可以配置账号密码和自动登录选项

## 技术实现

### 核心组件

- **BrowserController**: 负责WebView2浏览器控件的操作
- **DataExtractor**: 负责从1688页面提取订单数据
- **FormOperator**: 负责页面表单操作
- **PluginConfig**: 插件配置模型

### 配置管理

- **plugin_config.json**: 保存账号密码和cookies配置
- **favorites.json**: 保存收藏夹数据

### 自动登录机制

插件支持自动登录功能，通过以下方式实现：

1. 用户在"登录设置"中配置账号密码和启用自动登录选项
2. 插件在启动时自动恢复之前保存的cookies
3. 如果cookies有效，用户无需重新登录即可直接访问1688后台
4. 插件在关闭时自动保存当前的cookies，以便下次自动登录

## 编译和运行

由于项目使用了.NET Framework 4.8和WinForms，建议使用Visual Studio 2019或更高版本进行编译和调试。

### 编译步骤

1. 打开RUINORERP.Plugin.AlibabaStoreManager.sln解决方案文件
2. 恢复NuGet包
3. 编译解决方案
4. 运行项目

### 使用编译脚本

项目包含一个BuildProject.bat脚本，可以尝试使用该脚本来编译项目：

```
BuildProject.bat
```

### 运行要求

- Windows 7 SP1或更高版本
- .NET Framework 4.8
- WebView2运行时

## 项目结构

```
RUINORERP.Plugin.AlibabaStoreManager/
├── Core/                    # 核心功能模块
│   ├── BrowserController.cs # 浏览器控制器
│   ├── DataExtractor.cs     # 数据提取器
│   └── FormOperator.cs      # 表单操作器
├── Models/                  # 数据模型
│   └── PluginConfig.cs      # 插件配置模型
├── Properties/              # 项目属性文件
├── MainForm.cs              # 主界面窗体
├── MainForm.Designer.cs     # 主界面设计器文件
├── FavoritesForm.cs         # 收藏夹窗体
├── FavoritesForm.Designer.cs# 收藏夹设计器文件
├── LoginForm.cs             # 登录设置窗体
├── LoginForm.Designer.cs    # 登录设置设计器文件
├── Program.cs               # 程序入口点
├── ValidationForm.cs        # 技术验证窗体
├── ValidationForm.Designer.cs# 技术验证设计器文件
├── TestProgram.cs           # 测试程序入口点
├── BuildProject.bat         # 编译脚本
├── README.md                # 项目说明文档
└── RUINORERP.Plugin.AlibabaStoreManager.csproj # 项目文件
```

## 注意事项

1. 插件需要与主ERP系统配合使用
2. 需要确保WebView2运行时已安装
3. 插件配置保存在本地文件中
4. 收藏夹数据保存在favorites.json文件中
5. 账号密码和cookies数据保存在plugin_config.json文件中
6. 在生产环境中，建议对保存的密码进行加密处理