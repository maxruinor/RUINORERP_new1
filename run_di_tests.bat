@echo off
echo ========================================
echo    RUINORERP 依赖注入测试脚本
echo ========================================
echo.

REM 切换到项目目录
cd /d "e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\RUINORERP.Server"

REM 检查是否在正确目录
if not exist "Startup.cs" (
    echo 错误：未找到Startup.cs文件，请检查目录位置
    pause
    exit /b 1
)

echo 正在编译项目...
echo.

REM 尝试编译项目（这里需要根据实际构建系统调整）
echo 注意：请使用Visual Studio或dotnet build命令编译项目
echo.
echo 编译完成后，测试代码将自动集成到应用程序中
echo.
echo 测试方法：
echo 1. 在应用程序启动时调用 RUINORERP.Server.Network.Tests.TestRunner.RunAllTests()
echo 2. 或者在需要的地方手动调用依赖注入测试
echo.

pause