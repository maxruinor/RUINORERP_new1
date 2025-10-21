@echo off
echo 正在编译阿里巴巴店铺管理插件...
echo.

REM 检查是否存在MSBuild
where msbuild >nul 2>&1
if %errorlevel% neq 0 (
    echo 错误: 未找到MSBuild。请确保已安装Visual Studio或.NET SDK。
    echo.
    pause
    exit /b 1
)

REM 编译项目
msbuild RUINORERP.Plugin.AlibabaStoreManager.csproj /p:Configuration=Debug /p:Platform="Any CPU"

if %errorlevel% equ 0 (
    echo.
    echo 编译成功完成!
    echo.
) else (
    echo.
    echo 编译失败，请检查错误信息。
    echo.
)

pause