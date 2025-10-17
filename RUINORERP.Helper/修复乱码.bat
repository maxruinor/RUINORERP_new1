@echo off
chcp 65001 >nul
echo 正在修复帮助文件中文乱码问题...

REM 运行PowerShell脚本修复所有.htm文件的中文乱码
powershell -ExecutionPolicy Bypass -File "%~dp0fix_encoding_issues.ps1"

if %errorlevel% == 0 (
    echo.
    echo 成功修复所有帮助文件的中文乱码问题!
    echo 现在中文应该能正常显示了。
) else (
    echo.
    echo 修复过程中出现错误!
    echo 错误代码: %errorlevel%
)

echo.
pause