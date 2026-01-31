/// <summary>
/// PDF 标签纸尺寸批量修改工具
/// 将亚马逊外箱标签从 100mm×150mm 裁剪为 100mm×100mm，并每张复制3份
/// </summary>

using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System;
using System.IO;
using PdfSharpCore.Pdf.IO;

/// <summary>
/// PDF 标签调整器
/// </summary>
class PdfLabelResizer
{
    /// <summary>
    /// 毫米转换为点（PDF点：1mm = 2.834645669 点）
    /// </summary>
    private const double MmToPoint = 2.834645669;

    /// <summary>
    /// 复制份数（每张标签复制3份）
    /// </summary>
    private const int CopyCount = 3;

    /// <summary>
    /// 处理单个 PDF 文件
    /// </summary>
    /// <param name="inputPath">输入文件路径</param>
    /// <param name="outputPath">输出文件路径</param>
    /// <param name="newHeightMm">新高度（毫米）</param>
    /// <returns>处理是否成功</returns>
    public static bool ProcessPdfFile(string inputPath, string outputPath, double newHeightMm = 100)
    {
        try
        {
            // 第一步：用Modify模式打开文档，调整页面尺寸为100x100mm
            var modifyDocument = PdfReader.Open(inputPath, PdfDocumentOpenMode.Modify);

            // 计算新的高度（点）
            double newHeightPoint = newHeightMm * MmToPoint;

            int originalPageCount = modifyDocument.PageCount;

            // 调整所有页面的尺寸为100x100mm
            for (int i = 0; i < originalPageCount; i++)
            {
                PdfPage page = modifyDocument.Pages[i];

                // 获取当前页面尺寸
                double currentWidth = page.Width;
                double currentHeight = page.Height;

                // 直接修改页面尺寸为100x100mm
                page.Height = newHeightPoint;

                // 修改 MediaBox（媒体框）- 保留顶部内容，去掉底部
                var mediaBoxRect = new XRect(
                    0,                                  // 左边界
                    currentHeight - newHeightPoint,     // 下边界（从底部向上newHeightPoint）
                    currentWidth,                        // 宽度
                    newHeightPoint                      // 新高度
                );

                // 创建 PdfRectangle 从 XRect
                var mediaBox = new PdfRectangle(mediaBoxRect);

                // 设置 MediaBox
                page.MediaBox = mediaBox;

                // 设置 CropBox（裁剪框）与 MediaBox 相同
                page.CropBox = mediaBox;

                // 设置 TrimBox（修剪框）与 MediaBox 相同
                page.TrimBox = mediaBox;
            }

            // 保存临时调整后的文档到内存流
            int totalPagesGenerated = 0;
            using (var tempStream = new MemoryStream())
            {
                modifyDocument.Save(tempStream);
                modifyDocument.Close();

                // 第二步：用Import模式打开调整后的文档，复制页面
                tempStream.Position = 0;
                var importDocument = PdfReader.Open(tempStream, PdfDocumentOpenMode.Import);

                // 创建最终的输出文档
                var outputDocument = new PdfDocument();

                // 将调整后的页面复制3份到输出文档
                for (int i = 0; i < originalPageCount; i++)
                {
                    PdfPage sourcePage = importDocument.Pages[i];

                    for (int copyIndex = 0; copyIndex < CopyCount; copyIndex++)
                    {
                        // 使用AddPage添加页面到新文档
                        PdfPage newPage = outputDocument.AddPage(sourcePage);
                        totalPagesGenerated++;
                    }
                }

                // 保存新的 PDF
                outputDocument.Save(outputPath);
                outputDocument.Close();
                importDocument.Close();
            }

            Console.WriteLine($"✓ 处理成功: {Path.GetFileName(inputPath)} (原{originalPageCount}页 → {totalPagesGenerated}页)");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ 处理失败: {Path.GetFileName(inputPath)} - {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 批量处理目录下的所有 PDF 文件
    /// </summary>
    /// <param name="rootDir">根目录</param>
    /// <param name="newHeightMm">新高度（毫米）</param>
    /// <param name="suffix">输出文件后缀</param>
    public static void BatchProcessPdfFiles(string rootDir, double newHeightMm = 100, string suffix = "_100x100_3copies")
    {
        int totalFiles = 0;
        int successFiles = 0;
        int failedFiles = 0;

        Console.WriteLine($"开始处理目录: {rootDir}");
        Console.WriteLine($"目标尺寸: 100mm × {newHeightMm}mm");
        Console.WriteLine($"复制份数: 每张标签{CopyCount}份");
        Console.WriteLine(new string('-', 60));

        // 遍历所有子目录
        foreach (string file in Directory.GetFiles(rootDir, "*.pdf", SearchOption.AllDirectories))
        {
            // 跳过已处理过的文件
            if (file.Contains(suffix))
            {
                continue;
            }

            string directory = Path.GetDirectoryName(file)!;
            string filename = Path.GetFileNameWithoutExtension(file);
            string extension = Path.GetExtension(file);
            string outputPath = Path.Combine(directory, $"{filename}{suffix}{extension}");

            totalFiles++;
            if (ProcessPdfFile(file, outputPath, newHeightMm))
            {
                successFiles++;
            }
            else
            {
                failedFiles++;
            }
        }

        Console.WriteLine(new string('-', 60));
        Console.WriteLine($"处理完成！总计: {totalFiles} 个文件");
        Console.WriteLine($"成功: {successFiles} 个");
        Console.WriteLine($"失败: {failedFiles} 个");
    }
}

/// <summary>
/// 主程序
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        // 设置工作目录
        string scriptDir = AppDomain.CurrentDomain.BaseDirectory;
        string pdfDir = Path.Combine(scriptDir, "外箱标签");

        // 如果找不到目录，使用脚本所在目录
        if (!Directory.Exists(pdfDir))
        {
            pdfDir = Path.GetDirectoryName(scriptDir)!;
        }

        // 批量处理
        PdfLabelResizer.BatchProcessPdfFiles(pdfDir, newHeightMm: 100, suffix: "_100x100_3copies");

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }
}
