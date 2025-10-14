# 批量添加命名空间引用的PowerShell脚本
# 解决MyCacheManager的CS0103错误

$filesToAddNamespace = @(
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\ASS\UCASAfterSaleDelivery.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\ASS\UCASRepairOrder.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\ChartFramework\DataProviders\Adapters\CRMDataAdapter.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\ChartFramework\DataProviders\Adapters\CustomerDataAdapter.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\ClientSuperSocket\WorkflowService.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\Common\GridViewDisplayNameResolver.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\FM\FMBase\UCFMStatement.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\FM\FMBase\UCPaymentRecord.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\FM\FMBase\UCPreReceivedPayment.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\FM\FMBase\UCPriceAdjustment.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\FM\FMBase\UCReceivablePayable.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\FM\UCExpenseClaim.cs",
    "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\FM\UCPaymentApplication.cs"
)

$namespaceToAdd = "using RUINORERP.Extensions.Middlewares;"

foreach ($file in $filesToAddNamespace) {
    if (Test-Path $file) {
        Write-Host "处理文件: $file"
        
        # 读取文件内容
        $content = Get-Content $file -Raw
        
        # 检查是否已包含该命名空间
        if ($content -match "using RUINORERP.Extensions.Middlewares;") {
            Write-Host "  文件已包含所需命名空间，跳过"
            continue
        }
        
        # 查找namespace关键字
        $namespaceIndex = $content.IndexOf("namespace")
        
        if ($namespaceIndex -gt 0) {
            # 在namespace之前插入新的using语句
            $beforeNamespace = $content.Substring(0, $namespaceIndex)
            $afterNamespace = $content.Substring($namespaceIndex)
            
            # 查找最后一个using语句的位置
            $lastUsingIndex = $beforeNamespace.LastIndexOf("using ")
            if ($lastUsingIndex -ge 0) {
                # 找到最后一个using语句的结束位置
                $lastUsingEndIndex = $beforeNamespace.IndexOf(";", $lastUsingIndex)
                if ($lastUsingEndIndex -ge 0) {
                    # 在最后一个using语句后添加新行和新的using语句
                    $insertPosition = $lastUsingEndIndex + 1
                    $beforeNamespace = $beforeNamespace.Substring(0, $insertPosition) + "`r`n" + $namespaceToAdd + $beforeNamespace.Substring($insertPosition)
                }
            } else {
                # 没有找到using语句，直接在namespace前添加
                $beforeNamespace = $namespaceToAdd + "`r`n`r`n" + $beforeNamespace
            }
            
            # 重新组合内容
            $newContent = $beforeNamespace + $afterNamespace
            
            # 写回文件
            Set-Content $file $newContent -Encoding UTF8
            Write-Host "  已添加命名空间引用"
        } else {
            Write-Host "  未找到namespace关键字，跳过"
        }
    } else {
        Write-Host "文件不存在: $file"
    }
}

Write-Host "处理完成"