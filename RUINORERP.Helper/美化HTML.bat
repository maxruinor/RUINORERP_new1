@echo off
chcp 65001 >nul
echo 正在美化帮助文件...

REM 运行PowerShell脚本美化所有.htm文件
powershell -ExecutionPolicy Bypass -File "%~dp0beautify_html.ps1"

if %errorlevel% == 0 (
    echo.
    echo 成功美化所有帮助文件!
    echo 现在HTML文件看起来更专业和美观了。
) else (
    echo.
    echo 美化过程中出现错误!
    echo 错误代码: %errorlevel%
)

echo.
pause