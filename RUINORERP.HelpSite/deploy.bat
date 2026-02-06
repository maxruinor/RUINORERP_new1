@echo off
REM ==========================================
REM RUINOR ERP 帮助网站部署脚本
REM ==========================================

echo.
echo ========================================
echo RUINOR ERP 帮助网站部署工具
echo ========================================
echo.

REM 检查Python和pip
python --version >nul 2>nul
if %errorlevel% neq 0 (
    echo [错误] 未找到Python，请先安装Python 3.8+
    echo 下载地址：https://www.python.org/downloads/
    pause
    exit /b 1
)

echo [步骤1/5] 检查MkDocs...

pip show mkdocs >nul 2>nul
if %errorlevel% neq 0 (
    echo [信息] 安装MkDocs...
    pip install mkdocs
)

pip show mkdocs-material >nul 2>nul
if %errorlevel% neq 0 (
    echo [信息] 安装Material主题...
    pip install mkdocs-material
)

echo [步骤2/5] 安装依赖...
pip install mkdocs-minify-plugin
pip install pymdown-extensions

echo.
echo [步骤3/5] 构建网站...
cd /d "%~dp0"
mkdocs build

if %errorlevel% neq 0 (
    echo [错误] 网站构建失败
    pause
    exit /b 1
)

echo.
echo [步骤4/5] 复制到输出目录...
if exist "..\RUINORERP.UI\Help\site" (
    rmdir /s /q "..\RUINORERP.UI\Help\site"
)
xcopy /s /e /i /y "site" "..\RUINORERP.UI\Help\site\" >nul

echo.
echo ========================================
echo 部署完成！
echo ========================================
echo.
echo 网站文件位置：
echo   开发环境：%CD%\site\
echo   生产环境：..\RUINORERP.UI\Help\site\
echo.
echo 启动本地预览：
echo   mkdocs serve
echo.
echo 部署到IIS：
echo   将 site 目录复制到IIS网站目录
echo ========================================
echo.

choice /c YN /m "是否立即启动本地预览"
if %errorlevel% equ 1 (
    start mkdocs serve
)

pause
