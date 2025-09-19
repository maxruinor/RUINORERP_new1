# RUINORERP 网络服务旧文件清理脚本
# 执行前请确认所有服务都已成功迁移到 RUINORERP.Server 项目

Write-Host "=== RUINORERP 网络服务旧文件清理 ===" -ForegroundColor Green
Write-Host "开始时间: $(Get-Date)"
Write-Host ""

# 需要清理的文件列表
$filesToClean = @(
    # 接口文件 (已迁移到 Server/Network/Interfaces/)
    "e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.PacketSpec\Services\IFileStorageService.cs",
    "e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.PacketSpec\Services\IPacketSpecService.cs",
    "e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.PacketSpec\Services\Core\IUnifiedPacketSpecService.cs"
)

# 备份目录
$backupDir = "e:\CodeRepository\SynologyDrive\RUINORERP\Backup\PacketSpec_Services_Backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"

Write-Host "创建备份目录: $backupDir" -ForegroundColor Yellow
New-Item -ItemType Directory -Path $backupDir -Force | Out-Null

$totalFiles = 0
$cleanedFiles = 0
$skippedFiles = 0

foreach ($file in $filesToClean) {
    $totalFiles++
    
    if (Test-Path $file) {
        $fileName = Split-Path $file -Leaf
        $fileDir = Split-Path $file -Parent
        $relativeDir = $fileDir.Replace("e:\CodeRepository\SynologyDrive\RUINORERP\", "")
        
        # 创建对应的备份目录结构
        $backupSubDir = Join-Path $backupDir $relativeDir
        New-Item -ItemType Directory -Path $backupSubDir -Force | Out-Null
        
        $backupPath = Join-Path $backupSubDir $fileName
        
        Write-Host "处理文件: $file" -ForegroundColor Cyan
        Write-Host "  -> 备份到: $backupPath" -ForegroundColor Gray
        
        try {
            # 备份文件
            Copy-Item $file $backupPath -Force
            
            # 删除原文件
            Remove-Item $file -Force
            
            Write-Host "  ✅ 已清理: $fileName" -ForegroundColor Green
            $cleanedFiles++
            
        } catch {
            Write-Host "  ❌ 清理失败: $($_.Exception.Message)" -ForegroundColor Red
            $skippedFiles++
        }
        
    } else {
        Write-Host "  ⚠️ 文件不存在: $file" -ForegroundColor Yellow
        $skippedFiles++
    }
}

Write-Host ""
Write-Host "=== 清理完成 ===" -ForegroundColor Green
Write-Host "总文件数: $totalFiles"
Write-Host "已清理: $cleanedFiles"
Write-Host "跳过: $skippedFiles"
Write-Host "备份位置: $backupDir"
Write-Host "完成时间: $(Get-Date)"
Write-Host ""

if ($cleanedFiles -gt 0) {
    Write-Host "✅ 清理完成！建议重新编译项目验证依赖关系。" -ForegroundColor Green
} else {
    Write-Host "ℹ️ 没有文件需要清理。" -ForegroundColor Blue
}

Write-Host ""
Write-Host "注意事项：" -ForegroundColor Yellow
Write-Host "1. 清理后请重新编译 RUINORERP.PacketSpec 项目"
Write-Host "2. 检查是否有其他文件引用这些已删除的服务"
Write-Host "3. 运行依赖注入测试验证服务配置"
Write-Host "4. 备份文件可在 $backupDir 中找到"