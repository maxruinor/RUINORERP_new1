# CHM帮助文件生成配置

## 一、工具准备

### 1.1 安装HTML Help Workshop
1. 下载 Microsoft HTML Help Workshop
2. 安装完成后，在安装目录找到 hhc.exe

### 1.2 安装Pandoc（用于Markdown转HTML）
```powershell
winget install --id JohnMacFarlane.Pandoc -e
```

## 二、项目结构

```
RUINORERP.Help/
├── Project/
│   └── RUINORERP.hhp           # HTML Help Workshop项目文件
├── Source/                     # 帮助源文件（Markdown）
│   ├── modules/
│   ├── forms/
│   ├── controls/
│   └── fields/
├── HTML/                       # 转换后的HTML文件
│   ├── modules/
│   ├── forms/
│   ├── controls/
│   └── fields/
├── Images/                     # 帮助中的图片
├── Scripts/
│   └── md2html.ps1            # Markdown转HTML脚本
└── Output/
    └── RUINORERP.chm          # 生成的CHM文件
```

## 三、CHM项目配置文件 (RUINORERP.hhp)

```ini
[OPTIONS]
Compatibility=1.1 or later
Compiled file=..\Output\RUINORERP.chm
Contents file=RUINORERP.hhc
Default Window=main
Default topic=HTML\modules\index.html
Display compile progress=Yes
Full-text search=Yes
Index file=RUINORERP.hhk
Language=0x804 中文(中国)
Title=RUINORERP 帮助系统

[WINDOWS]
main="RUINORERP 帮助系统","RUINORERP.hhc","RUINORERP.hhk","HTML\modules\index.html",,,,,0x2820,250,0x104E,[10,10,1024,768],,,,,,,0

[FILES]
HTML\modules\Sales\index.html
HTML\modules\Purchase\index.html
HTML\modules\Inventory\index.html
HTML\modules\Finance\index.html
HTML\forms\CustomerEdit.html
HTML\forms\SalesOrderEdit.html
HTML\forms\PurchaseOrderEdit.html
HTML\controls\SalesOrderEdit\CustomerName.html
HTML\controls\SalesOrderEdit\OrderNo.html
HTML\fields\Customer.Name.html
HTML\fields\Customer.Phone.html
HTML\fields\SalesOrder.OrderNo.html
```

## 四、目录配置文件 (RUINORERP.hhc)

```html
<!DOCTYPE HTML PUBLIC "-//IETF//DTD HTML//EN">
<HTML>
<HEAD>
<meta name="GENERATOR" content="Microsoft&reg; HTML Help Workshop 4.1">
<!-- Sitemap 1.0 -->
</HEAD><BODY>
<OBJECT type="text/site properties">
	<param name="Window Styles" value="0x80125">
	<param name="ImageType" value="Folder">
</OBJECT>
<UL>
	<LI> <OBJECT type="text/sitemap">
		<param name="Name" value="RUINORERP 帮助系统">
		<param name="Local" value="HTML\modules\index.html">
		</OBJECT>
	<UL>
		<LI> <OBJECT type="text/sitemap">
			<param name="Name" value="销售管理">
			<param name="Local" value="HTML\modules\Sales\index.html">
			</OBJECT>
		<UL>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="客户管理">
				<param name="Local" value="HTML\forms\CustomerEdit.html">
				</OBJECT>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="销售订单">
				<param name="Local" value="HTML\forms\SalesOrderEdit.html">
				</OBJECT>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="出库管理">
				<param name="Local" value="HTML\forms\SalesOutboundEdit.html">
				</OBJECT>
		</UL>
		<LI> <OBJECT type="text/sitemap">
			<param name="Name" value="采购管理">
			<param name="Local" value="HTML\modules\Purchase\index.html">
			</OBJECT>
		<UL>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="供应商管理">
				<param name="Local" value="HTML\forms\SupplierEdit.html">
				</OBJECT>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="采购订单">
				<param name="Local" value="HTML\forms\PurchaseOrderEdit.html">
				</OBJECT>
		</UL>
		<LI> <OBJECT type="text/sitemap">
			<param name="Name" value="库存管理">
			<param name="Local" value="HTML\modules\Inventory\index.html">
			</OBJECT>
		<UL>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="入库管理">
				<param name="Local" value="HTML\forms\InboundEdit.html">
				</OBJECT>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="盘点管理">
				<param name="Local" value="HTML\forms\InventoryCheckEdit.html">
				</OBJECT>
		</UL>
		<LI> <OBJECT type="text/sitemap">
			<param name="Name" value="财务管理">
			<param name="Local" value="HTML\modules\Finance\index.html">
			</OBJECT>
		<UL>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="收款管理">
				<param name="Local" value="HTML\forms\ReceiptEdit.html">
				</OBJECT>
			<LI> <OBJECT type="text/sitemap">
				<param name="Name" value="付款管理">
				<param name="Local" value="HTML\forms\PaymentEdit.html">
				</OBJECT>
		</UL>
	</UL>
</UL>
</BODY></HTML>
```

## 五、索引配置文件 (RUINORERP.hhk)

```html
<!DOCTYPE HTML PUBLIC "-//IETF//DTD HTML//EN">
<HTML>
<HEAD>
<meta name="GENERATOR" content="Microsoft&reg; HTML Help Workshop 4.1">
<!-- Sitemap 1.0 -->
</HEAD><BODY>
<OBJECT type="text/site properties">
	<param name="Window Styles" value="0x80125">
</OBJECT>
<UL>
	<LI> <OBJECT type="text/sitemap">
		<param name="Name" value="客户管理">
		<param name="Name" value="客户">
		<param name="Local" value="HTML\forms\CustomerEdit.html">
		</OBJECT>
	<LI> <OBJECT type="text/sitemap">
		<param name="Name" value="销售订单">
		<param name="Local" value="HTML\forms\SalesOrderEdit.html">
		</OBJECT>
	<LI> <OBJECT type="text/sitemap">
		<param name="Name" value="供应商管理">
		<param name="Local" value="HTML\forms\SupplierEdit.html">
		</OBJECT>
	<LI> <OBJECT type="text/sitemap">
		<param name="Name" value="采购订单">
		<param name="Local" value="HTML\forms\PurchaseOrderEdit.html">
		</OBJECT>
	<LI> <OBJECT type="text/sitemap">
		<param name="Name" value="入库">
		<param name="Local" value="HTML\forms\InboundEdit.html">
		</OBJECT>
	<LI> <OBJECT type="text/sitemap">
		<param name="Name" value="出库">
		<param name="Local" value="HTML\forms\SalesOutboundEdit.html">
		</OBJECT>
	<LI> <OBJECT type="text/sitemap">
		<param name="Name" value="收款">
		<param name="Local" value="HTML\forms\ReceiptEdit.html">
		</OBJECT>
</UL>
</BODY></HTML>
```

## 六、Markdown转HTML脚本 (md2html.ps1)

```powershell
# md2html.ps1 - Markdown转HTML脚本
# 用途：批量将Source目录中的Markdown文件转换为HTML

$SourceDir = ".\Source"
$HtmlDir = ".\HTML"
$Stylesheet = ".\Styles\help.css"

# 创建HTML目录结构
Get-ChildItem -Path $SourceDir -Recurse -Directory | ForEach-Object {
    $relativePath = $_.FullName.Substring($SourceDir.Length + 1)
    $targetPath = Join-Path $HtmlDir $relativePath
    if (-not (Test-Path $targetPath)) {
        New-Item -ItemType Directory -Path $targetPath | Out-Null
    }
}

# 转换所有Markdown文件
Get-ChildItem -Path $SourceDir -Filter "*.md" -Recurse | ForEach-Object {
    $relativePath = $_.FullName.Substring($SourceDir.Length + 1)
    $targetPath = Join-Path $HtmlDir ($relativePath -replace '\.md$', '.html')

    Write-Host "转换: $($_.Name) -> $(Split-Path $targetPath -Leaf)"

    # 使用Pandoc转换
    pandoc $_.FullName `
        -f markdown `
        -t html `
        -o $targetPath `
        --standalone `
        --metadata title="$($_.BaseName)" `
        --css=$Stylesheet `
        --highlight-style=tango `
        --table-of-contents `
        --toc-depth=3
}

Write-Host "`n转换完成！"
```

## 七、CSS样式文件 (Styles/help.css)

```css
/* RUINORERP 帮助系统样式 */

body {
    font-family: "Microsoft YaHei", Arial, sans-serif;
    font-size: 14px;
    line-height: 1.6;
    color: #333;
    background-color: #fff;
    margin: 20px;
    padding: 0;
}

h1, h2, h3, h4, h5, h6 {
    color: #2c3e50;
    font-weight: 600;
    margin-top: 24px;
    margin-bottom: 16px;
}

h1 {
    font-size: 28px;
    border-bottom: 2px solid #3498db;
    padding-bottom: 10px;
}

h2 {
    font-size: 24px;
    border-bottom: 1px solid #bdc3c7;
    padding-bottom: 8px;
}

h3 {
    font-size: 20px;
}

h4 {
    font-size: 18px;
}

p {
    margin: 0 0 16px 0;
}

a {
    color: #3498db;
    text-decoration: none;
}

a:hover {
    text-decoration: underline;
}

code {
    background-color: #f8f9fa;
    padding: 2px 6px;
    border-radius: 3px;
    font-family: Consolas, "Courier New", monospace;
    font-size: 13px;
}

pre {
    background-color: #f8f9fa;
    border: 1px solid #e9ecef;
    border-radius: 4px;
    padding: 12px;
    overflow-x: auto;
}

pre code {
    background-color: transparent;
    padding: 0;
}

table {
    border-collapse: collapse;
    width: 100%;
    margin: 16px 0;
}

th, td {
    border: 1px solid #dee2e6;
    padding: 12px;
    text-align: left;
}

th {
    background-color: #e9ecef;
    font-weight: 600;
}

ul, ol {
    padding-left: 24px;
}

li {
    margin: 8px 0;
}

blockquote {
    border-left: 4px solid #3498db;
    padding-left: 16px;
    margin: 16px 0;
    color: #6c757d;
}

img {
    max-width: 100%;
    height: auto;
}

/* 目录样式 */
#toc {
    background-color: #f8f9fa;
    border: 1px solid #e9ecef;
    border-radius: 4px;
    padding: 16px;
    margin: 20px 0;
}

#toc ul {
    padding-left: 20px;
}

#toc li {
    margin: 8px 0;
}

#toc a {
    color: #2c3e50;
}

/* 标签样式 */
.tag {
    display: inline-block;
    background-color: #e9ecef;
    color: #495057;
    padding: 2px 8px;
    border-radius: 12px;
    font-size: 12px;
    margin-right: 4px;
}

/* 注意事项样式 */
.note {
    background-color: #fff3cd;
    border-left: 4px solid #ffc107;
    padding: 12px;
    margin: 16px 0;
}

.warning {
    background-color: #f8d7da;
    border-left: 4px solid #dc3545;
    padding: 12px;
    margin: 16px 0;
}

.info {
    background-color: #d1ecf1;
    border-left: 4px solid #17a2b8;
    padding: 12px;
    margin: 16px 0;
}
```

## 八、一键生成CHM脚本 (build-chm.bat)

```batch
@echo off
REM ==========================================
REM RUINORERP CHM 帮助文件一键生成脚本
REM ==========================================

echo.
echo ========================================
echo RUINORERP CHM 帮助文件生成工具
echo ========================================
echo.

REM 检查Pandoc是否安装
where pandoc >nul 2>nul
if %errorlevel% neq 0 (
    echo [错误] 未找到Pandoc，请先安装Pandoc
    pause
    exit /b 1
)

REM 检查HTML Help Workshop是否安装
if not exist "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" (
    echo [错误] 未找到HTML Help Workshop
    pause
    exit /b 1
)

echo [步骤1] 将Markdown转换为HTML...
powershell -ExecutionPolicy Bypass -File Scripts\md2html.ps1
if %errorlevel% neq 0 (
    echo [错误] Markdown转换失败
    pause
    exit /b 1
)

echo.
echo [步骤2] 更新CHM项目文件...
REM 这里可以添加自动更新hhp文件的逻辑

echo.
echo [步骤3] 编译CHM文件...
"C:\Program Files (x86)\HTML Help Workshop\hhc.exe" Project\RUINORERP.hhp
if %errorlevel% neq 0 (
    echo [错误] CHM编译失败
    pause
    exit /b 1
)

echo.
echo ========================================
echo CHM文件生成成功！
echo 输出文件: Output\RUINORERP.chm
echo ========================================
echo.

REM 打开输出目录
explorer Output

pause
```

## 九、在应用程序中调用CHM

```csharp
using System.Diagnostics;

/// <summary>
/// 显示CHM帮助文件
/// </summary>
public static class HelpLauncher
{
    private static readonly string ChmPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Help", "RUINORERP.chm");

    /// <summary>
    /// 显示CHM帮助主页
    /// </summary>
    public static void ShowHelp()
    {
        if (!File.Exists(ChmPath))
        {
            MessageBox.Show("帮助文件不存在！", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = ChmPath,
            UseShellExecute = true
        });
    }

    /// <summary>
    /// 显示CHM帮助的指定页面
    /// </summary>
    /// <param name="topicPath">主题路径，例如：html/forms/CustomerEdit.html</param>
    public static void ShowHelp(string topicPath)
    {
        if (!File.Exists(ChmPath))
        {
            MessageBox.Show("帮助文件不存在！", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var psi = new ProcessStartInfo
        {
            FileName = ChmPath,
            Arguments = $"/html/{topicPath}",
            UseShellExecute = true
        };

        Process.Start(psi);
    }

    /// <summary>
    /// 使用关键字搜索CHM帮助
    /// </summary>
    /// <param name="keyword">搜索关键字</param>
    public static void SearchHelp(string keyword)
    {
        if (!File.Exists(ChmPath))
        {
            MessageBox.Show("帮助文件不存在！", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var psi = new ProcessStartInfo
        {
            FileName = ChmPath,
            Arguments = $"/keyword={keyword}",
            UseShellExecute = true
        };

        Process.Start(psi);
    }
}
```

## 十、自动化集成到构建流程

### 10.1 添加到 .csproj 项目文件

```xml
<Target Name="BuildHelp" BeforeTargets="AfterBuild" Condition="'$(Configuration)'=='Release'">
  <Exec Command="cd Help &amp;&amp; build-chm.bat" />
  <Copy SourceFiles="Help\Output\RUINORERP.chm"
        DestinationFolder="$(OutputPath)" />
</Target>
```

### 10.2 部署说明
编译后的RUINORERP.chm文件需要部署到应用程序的Help目录下：
```
RUINORERP/
├── RUINORERP.exe
└── Help/
    └── RUINORERP.chm
```
