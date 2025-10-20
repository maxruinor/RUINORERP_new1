# Simple script to fix system introduction file encoding
# Using ASCII characters only

Write-Host "Starting to fix system introduction file..."

# Define file path
$file = "系统简介.htm"

# Read file content
$content = Get-Content -Path $file -Encoding UTF8
$contentString = $content -join "`r`n"

Write-Host "File content read successfully"

# Fix common encoding issues
$fixedContent = $contentString
$fixedContent = $fixedContent -replace "鍏ㄥ眬鏍峰紡", "全局样式"
$fixedContent = $fixedContent -replace "鏍囬鏍峰紡", "标题样式"
$fixedContent = $fixedContent -replace "娈佃惤鏍峰紡", "段落样式"
$fixedContent = $fixedContent -replace "鍒楄〃鏍峰紡", "列表样式"
$fixedContent = $fixedContent -replace "琛ㄦ牸鏍峰紡", "表格样式"
$fixedContent = $fixedContent -replace "寮鸿皟鏂囨湰", "强调文本"
$fixedContent = $fixedContent -replace "閾炬帴鏍峰紡", "链接样式"
$fixedContent = $fixedContent -replace "鎻愮ず妗嗘牱寮", "提示框样式"
$fixedContent = $fixedContent -replace "瀵艰埅閾炬帴锛", "导航链接："
$fixedContent = $fixedContent -replace "鐢熶骇绠＄悊", "生产管理"
$fixedContent = $fixedContent -replace "杩涢攢瀛樼鐞", "进销存管理"
$fixedContent = $fixedContent -replace "鍞悗绠＄悊", "售后管理"
$fixedContent = $fixedContent -replace "瀹㈡埛鍏崇郴绠＄悊", "客户关系管理"
$fixedContent = $fixedContent -replace "璐㈠姟绠＄悊", "财务管理"
$fixedContent = $fixedContent -replace "琛屾斂绠＄悊", "行政管理"
$fixedContent = $fixedContent -replace "鎶ヨ〃绠＄悊", "报表管理"
$fixedContent = $fixedContent -replace "鐢靛晢杩愯惀", "电商运营"
$fixedContent = $fixedContent -replace "鍩虹璧勬枡绠＄悊", "基础资料管理"
$fixedContent = $fixedContent -replace "绯荤粺璁剧疆", "系统设置"
$fixedContent = $fixedContent -replace "閫氱敤鍔熻兘", "通用功能"
$fixedContent = $fixedContent -replace "瀹℃牳鍙嶅鏍哥粨妗堜笟鍔℃祦绋", "审核反审核结案业务流程"

# Save with UTF-8 encoding
$fixedContent | Out-File -FilePath $file -Encoding UTF8

Write-Host "File encoding fixed successfully!"