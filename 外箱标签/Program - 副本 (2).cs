/// <summary>
/// PDF 标签批量复制工具
/// 将PDF标签每张复制10份，保持原始尺寸不变
/// </summary>

using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System;
using System.IO;
using PdfSharpCore.Pdf.IO;

/// <summary>
/// PDF 标签复制器
/// </summary>
class PdfLabelResizer
{

    /// <summary>
    /// 复制份数（每张标签复制10份）
    /// </summary>
    private const int CopyCount = 10;

    /// <summary>
    /// 处理单个 PDF 文件
    /// </summary>
    /// <param name="inputPath">输入文件路径</param>
    /// <param name="outputPath">输出文件路径</param>
    /// <returns>处理是否成功</returns>
    public static bool ProcessPdfFile(string inputPath, string outputPath)
    {
        try
        {
            // 用Import模式打开文档，直接复制页面（不调整尺寸）
            var importDocument = PdfReader.Open(inputPath, PdfDocumentOpenMode.Import);

            int originalPageCount = importDocument.PageCount;

            // 创建最终的输出文档
            var outputDocument = new PdfDocument();

            int totalPagesGenerated = 0;

            // 将原始页面复制10份到输出文档
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
    /// <param name="suffix">输出文件后缀</param>
    public static void BatchProcessPdfFiles(string rootDir, string suffix = "_10copies")
    {
        int totalFiles = 0;
        int successFiles = 0;
        int failedFiles = 0;

        Console.WriteLine($"开始处理目录: {rootDir}");
        Console.WriteLine($"复制份数: 每张标签{CopyCount}份");
        Console.WriteLine($"保持原始尺寸不变");
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
            if (ProcessPdfFile(file, outputPath))
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
        PdfLabelResizer.BatchProcessPdfFiles(pdfDir, suffix: "_10copies");

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }
}
