@echo off
echo 正在编译帮助文件...
echo.

REM 检查HTML Help Workshop是否已安装
if exist "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" (
    echo 找到HTML Help Workshop
    "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" help.hhp
) else (
    echo 未找到HTML Help Workshop
    echo 请安装HTML Help Workshop或使用WinCHM Pro编译项目
    echo.
    echo 使用WinCHM Pro:
    echo 1. 打开ERP系统帮助.wcp项目文件
    echo 2. 点击"编译"按钮
    echo 3. 生成的CHM文件将保存在当前目录
)

echo.
echo 编译完成
pause