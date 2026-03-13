/// <summary>
/// PDF 标签批量生成工具
/// 从PDF提取内容并生成900个标签，保持原始内容，只修改SN编号
/// </summary>

using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System;
using System.IO;
using PdfSharpCore.Pdf.IO;

/// <summary>
/// PDF 标签生成器
/// </summary>
class PdfLabelGenerator
{
    /// <summary>
    /// 标签总数
    /// </summary>
    private const int TotalLabels = 900;

    /// <summary>
    /// 标签间距（毫米）
    /// </summary>
    private const double LabelSpacingMm = 2.0;

    /// <summary>
    /// 生成标签
    /// </summary>
    /// <param name="inputPath">输入文件路径</param>
    /// <param name="outputPath">输出文件路径</param>
    /// <returns>处理是否成功</returns>
    public static bool GenerateLabels(string inputPath, string outputPath)
    {
        PdfDocument? inputDocument = null;
        PdfDocument? outputDocument = null;

        try
        {
            inputDocument = PdfReader.Open(inputPath, PdfDocumentOpenMode.Import);
            outputDocument = new PdfDocument();

            if (inputDocument.PageCount == 0)
            {
                Console.WriteLine($"✗ 输入PDF文件为空");
                return false;
            }

            PdfPage sourcePage = inputDocument.Pages[0];
            double sourceWidth = sourcePage.Width.Point;
            double sourceHeight = sourcePage.Height.Point;

            PdfPage? currentPage = null;
            int labelsOnCurrentPage = 0;
            int currentX = 0;
            int currentY = 0;
            int pageWidth = 0;
            int pageHeight = 0;
            int labelWidth = 0;
            int labelHeight = 0;
            int labelsPerRow = 0;
            int labelsPerColumn = 0;
            int spacingPoints = (int)(LabelSpacingMm * 2.835);

            for (int i = 1; i <= TotalLabels; i++)
            {
                if (currentPage == null || labelsOnCurrentPage == 0)
                {
                    currentPage = outputDocument.AddPage();
                    currentPage.Width = XUnit.FromMillimeter(210);
                    currentPage.Height = XUnit.FromMillimeter(297);
                    pageWidth = (int)currentPage.Width.Point;
                    pageHeight = (int)currentPage.Height.Point;
                    labelWidth = (int)(sourceWidth);
                    labelHeight = (int)(sourceHeight);
                    labelsPerRow = 2;
                    labelsPerColumn = 6;
                    labelsOnCurrentPage = 0;
                    currentX = 0;
                    currentY = 0;
                }

                int xPos = currentX * (labelWidth + spacingPoints) + (int)(pageWidth * 0.02);
                int yPos = currentY * (labelHeight + spacingPoints) + (int)(pageHeight * 0.03);

                DrawLabel(currentPage, sourcePage, xPos, yPos, labelWidth, labelHeight, i);

                labelsOnCurrentPage++;
                currentX++;

                if (currentX >= labelsPerRow)
                {
                    currentX = 0;
                    currentY++;
                }

                if (labelsOnCurrentPage >= labelsPerRow * labelsPerColumn)
                {
                    currentPage = null;
                    labelsOnCurrentPage = 0;
                }
            }

            outputDocument.Save(outputPath);
            Console.WriteLine($"✓ 标签生成成功: {Path.GetFileName(outputPath)} (共{TotalLabels}个标签)");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ 标签生成失败: {Path.GetFileName(inputPath)} - {ex.Message}");
            return false;
        }
        finally
        {
            outputDocument?.Close();
            inputDocument?.Close();
        }
    }

    /// <summary>
    /// 绘制单个标签
    /// </summary>
    /// <param name="targetPage">目标页面</param>
    /// <param name="sourcePage">源页面</param>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <param name="sequenceNumber">序列号</param>
    private static void DrawLabel(PdfPage targetPage, PdfPage sourcePage, int x, int y, int width, int height, int sequenceNumber)
    {
        using (XGraphics graphics = XGraphics.FromPdfPage(targetPage))
        {
            XRect labelRect = new XRect(x, y, width, height);

            XGraphicsState state = graphics.Save();
            try
            {
                graphics.IntersectClip(labelRect);
                
                string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
                PdfDocument tempDoc = new PdfDocument();
                tempDoc.AddPage(sourcePage);
                tempDoc.Save(tempFilePath);
                tempDoc.Close();
                
                XImage sourceImage = XImage.FromFile(tempFilePath);
                graphics.DrawImage(sourceImage, x, y, width, height);
                
                try
                {
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                }
                catch
                {
                }
            }
            finally
            {
                graphics.Restore(state);
            }

            string sequenceText = sequenceNumber.ToString("00000");
            XFont sequenceFont = new XFont("Arial", 14, XFontStyle.Bold);
            XSize sequenceSize = graphics.MeasureString(sequenceText, sequenceFont);
            XPoint sequencePosition = new XPoint(x + width - sequenceSize.Width - 20, y + height - 30);
            graphics.DrawString(sequenceText, sequenceFont, XBrushes.Black, sequencePosition);
        }
    }
}

/// <summary>
/// 主程序
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        string scriptDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(scriptDir, "..", "..", ".."));
        string pdfDir = projectDir;

        string inputFile = Path.Combine(pdfDir, "7142.pdf");
        string outputFile = Path.Combine(pdfDir, "7142_900labels.pdf");

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"✗ 输入文件不存在: {inputFile}");
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("开始生成标签...");
        Console.WriteLine($"输入文件: {inputFile}");
        Console.WriteLine($"输出文件: {outputFile}");
        Console.WriteLine($"标签总数: 900");
        Console.WriteLine($"标签间距: 2mm");
        Console.WriteLine($"保持原始内容，只修改SN编号");
        Console.WriteLine(new string('-', 60));

        bool success = PdfLabelGenerator.GenerateLabels(inputFile, outputFile);

        Console.WriteLine(new string('-', 60));
        if (success)
        {
            Console.WriteLine("标签生成完成！");
        }
        else
        {
            Console.WriteLine("标签生成失败！");
        }

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }
}
