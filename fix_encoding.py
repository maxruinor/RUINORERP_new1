import os

# 文件路径
file_path = r'e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Helper\系统简介.htm'
output_path = r'e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Helper\系统简介_fixed.htm'

print(f"正在处理文件: {file_path}")
print(f"文件是否存在: {os.path.exists(file_path)}")

# 尝试不同的编码读取文件
encodings_to_try = ['utf-8', 'gbk', 'gb2312', 'latin1']

for encoding in encodings_to_try:
    try:
        print(f"尝试使用编码 {encoding} 读取文件...")
        with open(file_path, 'r', encoding=encoding) as f:
            content = f.read()
        print(f"使用编码 {encoding} 成功读取文件")
        
        # 尝试检测乱码字符
        # 如果内容中包含乱码特征（如：锟斤拷，锘跨，雃等）
        if '锘' in content or '锟' in content or '雃' in content:
            print("检测到可能的乱码字符")
        else:
            print("未检测到明显的乱码字符")
        
        # 以UTF-8编码写入新文件
        with open(output_path, 'w', encoding='utf-8') as out_f:
            out_f.write(content)
        
        print(f'文件已成功转换并保存到: {output_path}')
        break
    except UnicodeDecodeError as e:
        print(f"使用编码 {encoding} 读取文件失败: {e}")
        continue
    except Exception as e:
        print(f"使用编码 {encoding} 处理文件时出现错误: {e}")
        import traceback
        traceback.print_exc()
        continue
else:
    print("所有编码尝试都失败了")