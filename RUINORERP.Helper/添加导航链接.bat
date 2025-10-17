@echo off
chcp 65001 >nul
echo 正在为帮助文件添加导航链接...

REM 运行PowerShell脚本为所有.htm文件添加导航链接
powershell -ExecutionPolicy Bypass -File "%~dp0add_navigation.ps1"

if %errorlevel% == 0 (
    echo.
    echo 成功为所有帮助文件添加导航链接!
    echo 现在用户可以在帮助文件间自由切换了。
) else (
    echo.
    echo 添加导航链接过程中出现错误!
    echo 错误代码: %errorlevel%
)

echo.
pause