import os
import glob

# 定义模块分类规则
modules = {
    "生产管理": ["制造规划", "MRP基本资料", "制程生产", "生产品控", "BOM分析报表", "成本管理", "生产分析", "生产管理"],
    "进销存管理": ["采购管理", "销售管理", "库存管理", "借出归还", "盘点管理", "调拨管理", "产品分割与组合", "其他出入库管理", "进销存分析", "进销存管理"],
    "售后管理": ["售后流程", "维修中心", "资产处置", "提前交付"],
    "客户关系": ["市场营销", "客户管理", "跟进管理", "报价管理", "合同管理", "回款管理", "开票管理", "商机总览", "绩效分析", "客户关系"],
    "财务管理": ["基础设置", "收款管理", "付款管理", "对账管理", "费用管理", "费用报销", "固定资产", "发票管理", "账务处理", "财务管理"],
    "行政管理": ["业务流管理", "资料管理", "基础资料", "人事管理", "行政管理", "行政基础资料", "行政资料"],
    "报表管理": ["报表管理", "生产分析", "进销存分析", "综合分析", "报表设置", "报表基础资料", "销售分析报表", "库存分析报表"],
    "电商运营": ["电商运营", "蓄水管理"],
    "基础资料": ["产品资料", "仓库资料", "供销资料", "行政资料", "财务资料", "包装资料", "基础资料", "货品资料"],
    "系统设置": ["系统参数", "权限管理", "个性化设置", "流程设计", "系统工具", "动态参数", "系统设置"],
    "通用功能": ["通用功能", "打印功能", "数据查询", "查询", "高级查询", "提醒工具"]
}

# 获取所有HTML文件
html_files = glob.glob("*.htm*")
controls_files = glob.glob("controls/*.html")
forms_files = glob.glob("forms/*.html")

print("=== ERP帮助系统文件分析报告 ===\n")

print(f"总文件数: {len(html_files) + len(controls_files) + len(forms_files)}")
print(f"  主目录文件: {len(html_files)}")
print(f"  控件帮助文件: {len(controls_files)}")
print(f"  窗体帮助文件: {len(forms_files)}\n")

# 分析各模块文件
print("=== 按模块分类 ===")
module_files = {}
for module, keywords in modules.items():
    module_files[module] = []
    for file in html_files:
        # 移除文件扩展名
        filename = os.path.splitext(file)[0]
        # 检查文件名是否包含模块关键词
        for keyword in keywords:
            if keyword in filename:
                module_files[module].append(file)
                break

# 输出各模块文件
for module, files in module_files.items():
    if files:
        print(f"\n{module}模块 ({len(files)}个文件):")
        for file in files:
            print(f"  - {file}")

# 查找未分类的文件
classified_files = set()
for files in module_files.values():
    classified_files.update(files)

unclassified_files = set(html_files) - classified_files
if unclassified_files:
    print(f"\n未分类文件 ({len(unclassified_files)}个文件):")
    for file in unclassified_files:
        print(f"  - {file}")

# 检查重复文件
print(f"\n=== 重复文件检查 ===")
all_files = html_files + controls_files + forms_files
file_names = [os.path.basename(f) for f in all_files]
duplicates = set([name for name in file_names if file_names.count(name) > 1])

if duplicates:
    print(f"发现 {len(duplicates)} 个重复文件名:")
    for name in duplicates:
        print(f"  - {name}")
        # 找出具体重复的文件路径
        for f in all_files:
            if os.path.basename(f) == name:
                print(f"    {f}")
else:
    print("未发现重复文件名")

print(f"\n=== 控件帮助文件 ({len(controls_files)}个文件) ===")
for file in controls_files:
    print(f"  - {file}")

print(f"\n=== 窗体帮助文件 ({len(forms_files)}个文件) ===")
for file in forms_files:
    print(f"  - {file}")