// 新的拖放处理方法
private void NewTxtUpdatedFiles_DragDrop(object sender, DragEventArgs e)
{
    StringBuilder sb = new StringBuilder();
    // 在此处获取文件或者文件夹的路径
    Array array = (Array)e.Data.GetData(DataFormats.FileDrop);

    // 已经存在的文件，用于判断重复
    string[] files = txtPreVerUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    HashSet<string> existingFilesSet = new HashSet<string>(files, StringComparer.OrdinalIgnoreCase);

    // 排序
    Array.Sort(array);
    List<string> addFiles = new List<string>();
    int totalProcessed = 0;
    int successfullyAdded = 0;
    int skippedDuplicates = 0;

    foreach (string path in array)
    {
        totalProcessed++;
        // 这个新拖进来的文件 是全路径，要去前面与来源相同的目录去掉
        if (path.IndexOf(txtCompareSource.Text) == 0)
        {
            string relativePath = path.Substring(txtCompareSource.Text.Length).TrimStart('\');
            
            if (existingFilesSet.Contains(relativePath))
            {
                richTxtLog.AppendText($"{relativePath} 已存在，已跳过。\r\n");
                skippedDuplicates++;
                continue;
            }
            
            if (File.Exists(System.IO.Path.Combine(txtCompareSource.Text, relativePath)))
            {
                addFiles.Add(relativePath);
                existingFilesSet.Add(relativePath);
                successfullyAdded++;
            }
            else if (Directory.Exists(System.IO.Path.Combine(txtCompareSource.Text, relativePath)))
            {
                // 处理目录，递归获取所有文件
                try
                {
                    StringCollection allFilesInDir = GetAllFiles(System.IO.Path.Combine(txtCompareSource.Text, relativePath));
                    foreach (var file in allFilesInDir)
                    {
                        string fileRelativePath = file.Substring(txtCompareSource.Text.Length).TrimStart('\');
                        if (!existingFilesSet.Contains(fileRelativePath))
                        {
                            addFiles.Add(fileRelativePath);
                            existingFilesSet.Add(fileRelativePath);
                            successfullyAdded++;
                        }
                        else
                        {
                            skippedDuplicates++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    richTxtLog.AppendText($"处理目录 {relativePath} 时出错: {ex.Message}\r\n");
                }
            }
            else
            {
                richTxtLog.AppendText($"{relativePath} 不存在，已跳过。\r\n");
            }
        }
        else
        {
            // 直接拖放的文件，不是来自源目录
            string fileName = Path.GetFileName(path);
            richTxtLog.AppendText($"{fileName} 不是来自源目录，已跳过。\r\n");
        }
    }

    // 排序
    addFiles.Sort();
    
    // 构建要添加的文本
    foreach (string path in addFiles)
    {
        sb.Append(path).Append("\r\n");
    }

    // 更新文本框内容
    if (chkAppend.Checked)
    {
        txtPreVerUpdatedFiles.Text += sb.ToString();
    }
    else
    {
        txtPreVerUpdatedFiles.Text = sb.ToString();
    }

    // 显示拖放结果统计
    richTxtLog.AppendText($"拖放完成：共处理 {totalProcessed} 个项目，成功添加 {successfullyAdded} 个，跳过重复 {skippedDuplicates} 个。\r\n");
}