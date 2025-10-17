@echo off
chcp 65001 >nul
echo 正在生成ERP系统帮助.wcp文件...

REM 检查源文件是否存在
if not exist "ERP系统帮助.wcp.txt" (
    echo 错误: 找不到源文件 ERP系统帮助.wcp.txt
    pause
    exit /b 1
)

REM 复制文本文件为.wcp文件
copy /Y "ERP系统帮助.wcp.txt" "ERP系统帮助.wcp" >nul

if %errorlevel% == 0 (
    echo.
    echo 成功生成 ERP系统帮助.wcp 文件!
    echo 现在您可以使用WinCHM Pro打开并编辑此文件。
) else (
    echo.
    echo 生成文件时出错!
    echo 错误代码: %errorlevel%
)

echo.
pause