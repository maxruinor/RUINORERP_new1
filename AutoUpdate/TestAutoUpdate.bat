@echo off
echo ========================================
echo RUINORERP 自动更新程序测试脚本
echo ========================================
echo.

echo 1. 清理旧日志文件...
if exist AutoUpdateLog.txt del AutoUpdateLog.txt
if exist UpdaterData (
    echo 清理旧的更新数据...
    rmdir /s /q UpdaterData
)

echo.
echo 2. 检查关键文件...
if exist AutoUpdate.exe (
    echo [✓] AutoUpdate.exe 存在
) else (
    echo [✗] AutoUpdate.exe 不存在
    goto :error
)

if exist AutoUpdaterList.xml (
    echo [✓] AutoUpdaterList.xml 存在
) else (
    echo [✗] AutoUpdaterList.xml 不存在
    goto :error
)

echo.
echo 3. 启动自动更新程序进行测试...
echo 注意事项：
echo - 请观察文件是否正确下载到 UpdaterData 目录
echo - 检查日志文件 AutoUpdateLog.txt 的输出
echo - 验证文件是否正确复制到应用程序目录
echo.

pause
AutoUpdate.exe

goto :end

:error
echo.
echo [错误] 测试失败，请检查必要文件是否完整
pause
exit /b 1

:end
echo.
echo 测试完成
pause