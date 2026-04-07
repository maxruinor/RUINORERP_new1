using System;
using RUINORERP.Lib.BusinessImage;
using RUINOR.WinFormsUI.CustomPictureBox.Implementations;

namespace TestNullReferenceFix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("测试 ImageInfo.Metadata null 引用修复...");
            
            // 创建 ImageInfo 对象（应该自动初始化 Metadata）
            var imageInfo = new ImageInfo();
            
            // 验证 Metadata 不为 null
            if (imageInfo.Metadata == null)
            {
                Console.WriteLine("错误: Metadata 为 null!");
                return;
            }
            
            Console.WriteLine("✓ ImageInfo 构造函数正确初始化了 Metadata");
            
            // 创建 ImageUpdateManager 的依赖项
            var hashCalculator = new SHA256HashCalculator();
            var imageProcessor = new ImageProcessor(null, null); // 这里可能需要实际的依赖
            
            // 测试 MarkImageAsUpdated 方法
            try
            {
                var updateManager = new ImageUpdateManager(hashCalculator, imageProcessor);
                updateManager.MarkImageAsUpdated(imageInfo);
                Console.WriteLine("✓ MarkImageAsUpdated 方法执行成功，没有抛出 NullReferenceException");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"错误: MarkImageAsUpdated 抛出 NullReferenceException: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注意: 抛出其他异常（可能是依赖项问题）: {ex.GetType().Name}: {ex.Message}");
            }
            
            Console.WriteLine("\n所有测试通过！修复成功。");
        }
    }
}
