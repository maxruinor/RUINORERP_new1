# Fix HTML encoding issues
# Use only ASCII characters to avoid encoding problems

# Get all HTML files in current directory and subdirectories
$htmlFiles = Get-ChildItem -Path "." -Recurse -Include "*.htm", "*.html"

Write-Host "Processing files for encoding fixes..."

foreach ($file in $htmlFiles) {
    try {
        # Read file content
        $content = Get-Content -Path $file.FullName -Encoding UTF8
        $contentString = $content -join "`r`n"
        
        # Check if file contains garbled text patterns
        if ($contentString -match "瀹㈡埛鍏崇郴|鍩虹璧勬枡|瀹℃牳娴佺▼|鍏ㄥ眬鏍峰紡|鏍囬鏍峰紡|娈佃惤鏍峰紡|鍒楄〃鏍峰紡|琛ㄦ牸鏍峰紡|寮鸿皟鏂囨湰|閾炬帴鏍峰紡|鎻愮ず妗嗘牱寮") {
            Write-Host "Fixing: $($file.Name)"
            
            # Fix common encoding issues
            $fixedContent = $contentString
            $fixedContent = $fixedContent -replace "瀹㈡埛鍏崇郴", "客户关系"
            $fixedContent = $fixedContent -replace "鍩虹璧勬枡", "基础资料"
            $fixedContent = $fixedContent -replace "瀹℃牳娴佺▼", "审核流程"
            $fixedContent = $fixedContent -replace "鍏ㄥ眬鏍峰紡", "全局样式"
            $fixedContent = $fixedContent -replace "鏍囬鏍峰紡", "标题样式"
            $fixedContent = $fixedContent -replace "娈佃惤鏍峰紡", "段落样式"
            $fixedContent = $fixedContent -replace "鍒楄〃鏍峰紡", "列表样式"
            $fixedContent = $fixedContent -replace "琛ㄦ牸鏍峰紡", "表格样式"
            $fixedContent = $fixedContent -replace "寮鸿皟鏂囨湰", "强调文本"
            $fixedContent = $fixedContent -replace "閾炬帴鏍峰紡", "链接样式"
            $fixedContent = $fixedContent -replace "鎻愮ず妗嗘牱寮", "提示框样式"
            
            # Save with UTF-8 encoding with BOM
            $fixedContent | Out-File -FilePath $file.FullName -Encoding UTF8
        }
    } catch {
        Write-Host "Error processing: $($file.Name)"
    }
}

Write-Host "Encoding fixes completed."