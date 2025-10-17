@echo off
chcp 65001 >nul
echo 正在编译RUINOR ERP帮助文件...
echo.

REM 检查HTML Help Workshop是否安装
if not exist "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" (
    echo 错误: 未找到HTML Help Workshop
    echo 请安装HTML Help Workshop后再编译帮助文件
    pause
    exit /b 1
)

REM 编译CHM文件
"C:\Program Files (x86)\HTML Help Workshop\hhc.exe" help.hhp

if %errorlevel% == 0 (
    echo.
    echo 帮助文件编译成功!
    echo 输出文件: help.chm
) else (
    echo.
    echo 帮助文件编译失败!
    echo 错误代码: %errorlevel%
)

echo.
pause