# 修复帮助文件中文乱码问题的 PowerShell 脚本
# 该脚本将修复HTML文件中的中文乱码问题

# 获取当前目录及子目录下所有.htm文件
$htmFiles = Get-ChildItem -Path "." -Recurse -Include "*.htm" -File

Write-Output "找到 $($htmFiles.Count) 个.htm文件"

foreach ($file in $htmFiles) {
    # 读取文件内容（使用UTF8编码）
    $content = Get-Content -Path $file.FullName -Encoding UTF8 -Raw
    
    # 修复CSS注释中的乱码
    $content = $content -replace "/* 鍏ㄥ眬鏍峰紡 */", "/* 全局样式 */"
    $content = $content -replace "/* 鏍囬鏍峰紡 */", "/* 标题样式 */"
    $content = $content -replace "/* 娈佃惤鏍峰紡 */", "/* 段落样式 */"
    $content = $content -replace "/* 鍒楄〃鏍峰紡 */", "/* 列表样式 */"
    $content = $content -replace "/* 琛ㄦ牸鏍峰紡 */", "/* 表格样式 */"
    $content = $content -replace "/* 寮鸿皟鏂囨湰 */", "/* 强调文本 */"
    $content = $content -replace "/* 閾炬帴鏍峰紡 */", "/* 链接样式 */"
    $content = $content -replace "/* 鎻愮ず妗嗘牱寮?*/", "/* 提示框样式 */"
    
    # 修复导航链接中的乱码
    $content = $content -replace "瀵艰埅閾炬帴锛?", "导航链接："
    $content = $content -replace "鐢熶骇绠＄悊", "生产管理"
    $content = $content -replace "杩涢攢瀛樼鐞?", "进销存管理"
    $content = $content -replace "鍞悗绠＄悊", "售后管理"
    $content = $content -replace "瀹㈡埛鍏崇郴绠＄悊", "客户关系管理"
    $content = $content -replace "璐㈠姟绠＄悊", "财务管理"
    $content = $content -replace "琛屾斂绠＄悊", "行政管理"
    $content = $content -replace "鎶ヨ〃绠＄悊", "报表管理"
    $content = $content -replace "鐢靛晢杩愯惀", "电商运营"
    $content = $content -replace "鍩虹璧勬枡绠＄悊", "基础资料管理"
    $content = $content -replace "绯荤粺璁剧疆", "系统设置"
    $content = $content -replace "閫氱敤鍔熻兘", "通用功能"
    $content = $content -replace "瀹℃牳娴佺▼", "审核流程"
    
    # 以UTF-8 with BOM格式重新保存文件
    [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8)
    
    Write-Output "已修复文件乱码: $($file.FullName)"
}

Write-Output "所有文件乱码修复完成！"