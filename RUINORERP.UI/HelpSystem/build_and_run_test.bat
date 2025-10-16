@echo off
echo 正在编译帮助系统测试程序...
echo.

REM 检查是否安装了.NET SDK
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo 未找到.NET SDK，请先安装.NET SDK
    pause
    exit /b
)

REM 编译项目
dotnet build HelpSystemTest.csproj -c Release
if %errorlevel% neq 0 (
    echo 编译失败
    pause
    exit /b
)

echo.
echo 编译成功！
echo.

REM 运行测试程序
echo 正在启动帮助系统测试程序...
echo.
dotnet run --project HelpSystemTest.csproj

pause