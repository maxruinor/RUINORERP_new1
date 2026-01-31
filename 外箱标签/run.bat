@echo off
chcp 65001 >nul
echo ========================================
echo PDF 标签尺寸批量修改工具
echo ========================================
echo.

echo 正在检查 .NET SDK...
dotnet --version
if %errorlevel% neq 0 (
    echo.
    echo 错误：未检测到 .NET SDK！
    echo 请先安装 .NET 6.0 SDK：https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo.
echo 正在恢复依赖包...
dotnet restore
if %errorlevel% neq 0 (
    echo.
    echo 依赖包恢复失败！
    pause
    exit /b 1
)

echo.
echo 正在编译项目...
dotnet build -c Release
if %errorlevel% neq 0 (
    echo.
    echo 编译失败！
    pause
    exit /b 1
)

echo.
echo ========================================
echo 开始处理 PDF 文件...
echo ========================================
echo.

dotnet run -c Release

echo.
echo ========================================
echo 处理完成！
echo ========================================
pause
