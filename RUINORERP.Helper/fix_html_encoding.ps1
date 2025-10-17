# Fix HTML encoding issues
Get-ChildItem -Path "." -Recurse -Include "*.htm","*.html" | ForEach-Object {
    Write-Host "Processing: $($_.Name)"
    $content = Get-Content $_.FullName -Encoding UTF8
    $hasIssues = $false
    
    # Check for garbled text patterns
    if ($content -match "瀹㈡埛鍏崇郴|鍩虹璧勬枡|瀹℃牳娴佺▼") {
        $hasIssues = $true
    }
    
    if ($hasIssues) {
        # Fix common encoding issues
        $content = $content -replace "瀹㈡埛鍏崇郴", "客户关系"
        $content = $content -replace "鍩虹璧勬枡", "基础资料"
        $content = $content -replace "瀹℃牳娴佺▼", "审核流程"
        $content = $content -replace "瀵艰埅閾炬帴锛?", "导航链接："
        $content = $content -replace "鐢熶骇绠＄悊", "生产管理"
        $content = $content -replace "杩涢攢瀛樼鐞?", "进销存管理"
        $content = $content -replace "鍞悗绠＄悊", "售后管理"
        $content = $content -replace "璐㈠姟绠＄悊", "财务管理"
        $content = $content -replace "琛屾斂绠＄悊", "行政管理"
        $content = $content -replace "鎶ヨ〃绠＄悊", "报表管理"
        $content = $content -replace "鐢靛晢杩愯惀", "电商运营"
        $content = $content -replace "绯荤粺璁剧疆", "系统设置"
        $content = $content -replace "閫氱敤鍔熻兘", "通用功能"
        $content = $content -replace "瀹℃牳鍙嶅鏍哥粨妗堜笟鍔℃祦绋?", "审核反审核结案业务流程"
        
        # Save with UTF8 encoding with BOM
        $content | Out-File -FilePath $_.FullName -Encoding UTF8
        Write-Host "  Fixed encoding issues in $($_.Name)"
    }
}

Write-Host "Completed processing all HTML files."