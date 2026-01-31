"""
PDF 标签纸尺寸批量修改工具
将亚马逊外箱标签从 100mm×150mm 裁剪为 100mm×100mm
"""

import os
from PyPDF2 import PdfReader, PdfWriter
from PyPDF2 import Transformation
from PyPDF2.generic import RectangleObject


def resize_pdf_page(page, new_height_mm):
    """
    调整 PDF 页面尺寸

    Args:
        page: PDF 页面对象
        new_height_mm: 新的高度（毫米）

    Returns:
        调整后的页面对象
    """
    # 获取当前页面尺寸
    current_rect = page.mediabox
    current_width = float(current_rect.width)
    current_height = float(current_rect.height)

    # 将毫米转换为点（PDF点：1mm = 2.834645669 点）
    mm_to_point = 2.834645669
    new_height_point = new_height_mm * mm_to_point

    # 裁剪页面：保留顶部内容，裁剪底部
    # 假设内容从顶部开始，我们需要保留 new_height_point 的高度
    new_top = current_rect.top
    new_bottom = current_rect.top - new_height_point

    # 设置新的媒体框（裁剪）
    page.mediabox = RectangleObject([
        current_rect.left,
        new_bottom,
        current_rect.right,
        new_top
    ])

    # 同时设置裁剪框和出血框
    page.cropbox = page.mediabox
    page.bleedbox = page.mediabox

    return page


def process_pdf_file(input_path, output_path, new_height_mm=100):
    """
    处理单个 PDF 文件

    Args:
        input_path: 输入 PDF 文件路径
        output_path: 输出 PDF 文件路径
        new_height_mm: 新的高度（毫米），默认 100mm

    Returns:
        bool: 处理是否成功
    """
    try:
        # 读取原始 PDF
        reader = PdfReader(input_path)
        writer = PdfWriter()

        # 处理每一页
        for page in reader.pages:
            resized_page = resize_pdf_page(page, new_height_mm)
            writer.add_page(resized_page)

        # 保存新的 PDF
        with open(output_path, 'wb') as f:
            writer.write(f)

        print(f"✓ 处理成功: {os.path.basename(input_path)}")
        return True

    except Exception as e:
        print(f"✗ 处理失败: {os.path.basename(input_path)} - {str(e)}")
        return False


def batch_process_pdf_files(root_dir, new_height_mm=100, suffix="_100x100"):
    """
    批量处理目录下的所有 PDF 文件

    Args:
        root_dir: 根目录
        new_height_mm: 新的高度（毫米），默认 100mm
        suffix: 输出文件后缀
    """
    # 统计信息
    total_files = 0
    success_files = 0
    failed_files = 0

    print(f"开始处理目录: {root_dir}")
    print(f"目标尺寸: 100mm × {new_height_mm}mm")
    print("-" * 60)

    # 遍历所有子目录
    for root, dirs, files in os.walk(root_dir):
        for file in files:
            if file.lower().endswith('.pdf'):
                input_path = os.path.join(root, file)
                filename, ext = os.path.splitext(file)
                output_filename = f"{filename}{suffix}{ext}"
                output_path = os.path.join(root, output_filename)

                total_files += 1
                if process_pdf_file(input_path, output_path, new_height_mm):
                    success_files += 1
                else:
                    failed_files += 1

    print("-" * 60)
    print(f"处理完成！总计: {total_files} 个文件")
    print(f"成功: {success_files} 个")
    print(f"失败: {failed_files} 个")


if __name__ == "__main__":
    # 设置工作目录
    script_dir = os.path.dirname(os.path.abspath(__file__))
    pdf_dir = os.path.join(script_dir, "外箱标签")

    # 批量处理
    batch_process_pdf_files(pdf_dir, new_height_mm=100, suffix="_100x100")
