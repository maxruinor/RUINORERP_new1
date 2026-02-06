@echo off
REM ==========================================
REM RUINOR ERP 帮助网站快速启动脚本
REM ==========================================

echo.
echo ========================================
echo 启动 RUINOR ERP 帮助网站
echo ========================================
echo.

cd /d "%~dp0"

REM 检查MkDocs
pip show mkdocs >nul 2>nul
if %errorlevel% neq 0 (
    echo [信息] 首次运行，正在安装MkDocs...
    pip install mkdocs mkdocs-material mkdocs-minify-plugin pymdown-extensions
)

echo 启动网站服务...
echo 访问地址：http://127.0.0.1:8000
echo.
echo 按 Ctrl+C 停止服务
echo.

mkdocs serve --dev-addr=0.0.0.0:8000
