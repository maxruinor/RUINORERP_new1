# -*- coding: utf-8 -*-
import os
import re

# 文件路径
file_path = r"e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Helper\系统简介.htm"

print("开始修复系统简介文件编码问题...")

# 检查文件是否存在
if not os.path.exists(file_path):
    print("错误: 文件不存在")
    exit(1)

# 读取文件内容
with open(file_path, "r", encoding="utf-8") as f:
    content = f.read()

print("文件读取成功")
print(f"文件大小: {len(content)} 字符")

# 定义替换规则
replacements = {
    "鍏ㄥ眬鏍峰紡": "全局样式",
    "鏍囬鏍峰紡": "标题样式",
    "娈佃惤鏍峰紡": "段落样式",
    "鍒楄〃鏍峰紡": "列表样式",
    "琛ㄦ牸鏍峰紡": "表格样式",
    "寮鸿皟鏂囨湰": "强调文本",
    "閾炬帴鏍峰紡": "链接样式",
    "鎻愮ず妗嗘牱寮": "提示框样式",
    "瀵艰埅閾炬帴锛": "导航链接：",
    "鐢熶骇绠＄悊": "生产管理",
    "杩涢攢瀛樼鐞": "进销存管理",
    "鍞悗绠＄悊": "售后管理",
    "瀹㈡埛鍏崇郴绠＄悊": "客户关系管理",
    "璐㈠姟绠＄悊": "财务管理",
    "琛屾斂绠＄悊": "行政管理",
    "鎶ヨ〃绠＄悊": "报表管理",
    "鐢靛晢杩愯惀": "电商运营",
    "鍩虹璧勬枡绠＄悊": "基础资料管理",
    "绯荤粺璁剧疆": "系统设置",
    "閫氱敤鍔熻兘": "通用功能",
    "瀹℃牳鍙嶅鏍哥粨妗堜笟鍔℃祦绋": "审核反审核结案业务流程"
}

# 检查是否存在乱码
found_garbled = False
for garbled in replacements.keys():
    if garbled in content:
        print(f"发现乱码: {garbled}")
        found_garbled = True

if not found_garbled:
    print("未发现预期的乱码字符")
    # 检查是否已经修复
    if "全局样式" in content and "标题样式" in content:
        print("文件似乎已经修复过了")
        exit(0)

# 应用替换
fixed_content = content
replacements_made = 0
for garbled, correct in replacements.items():
    if garbled in fixed_content:
        print(f"替换: {garbled} -> {correct}")
        fixed_content = fixed_content.replace(garbled, correct)
        replacements_made += 1

print(f"共进行了 {replacements_made} 次替换")

# 写入修复后的内容
with open(file_path, "w", encoding="utf-8") as f:
    f.write(fixed_content)

print("文件编码修复完成!")