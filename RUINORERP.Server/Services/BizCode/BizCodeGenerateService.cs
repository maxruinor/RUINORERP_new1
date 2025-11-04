using RUINORERP.IServices;
using RUINORERP.Server.BNR;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business;
using System;
using System.Collections.Generic;
using System.Text;
using RUINORERP.Global;
using RUINORERP.Model;
using System.Linq;

namespace RUINORERP.Server.Services.BizCode
{
    /// <summary>
    /// 业务编码服务实现
    /// 包装BNRFactory功能，提供业务编码生成服务
    /// </summary>
    public class BizCodeGenerateService : IBizCodeGenerateService
    {
        private readonly BNRFactory _bnrFactory;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bnrFactory">编号生成工厂实例</param>
        public BizCodeGenerateService(BNRFactory bnrFactory)
        {
            _bnrFactory = bnrFactory;
            // 初始化时注册必要的参数处理器
            Initialize();
        }
        
        /// <summary>
        /// 初始化服务
        /// 注册必要的参数处理器和规则
        /// </summary>
        private void Initialize()
        {
            // 这里可以初始化特定的规则或处理器
            // BNRFactory可能已经在构造时初始化了基本的处理器
        }
        
        
        
        /// <summary>
        /// 生成业务单据编号（支持BizType枚举）
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <returns>生成的单据编号</returns>
        public string GenerateBizBillNo(BizType bizType)
        {
            // 根据业务类型枚举获取对应的规则
            string rule = GetBillNoRule(bizType);
            return _bnrFactory.Create(rule);
        }
        
        /// <summary>
        /// 生成业务单据编号（支持BizType枚举和额外参数）
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="parameter">业务编码参数</param>
        /// <returns>生成的单据编号</returns>
        public string GenerateBizBillNo(BizType bizType, BizCodeParameter parameter = null)
        {
            // 根据业务类型枚举获取对应的规则
            string rule = GetBillNoRule(bizType);
            return _bnrFactory.Create(rule);
        }
        
        /// <summary>
        /// 生成基础信息编号
        /// </summary>
        /// <param name="infoType">信息类型枚举</param>
        /// <returns>生成的信息编号</returns>
        public string GenerateBaseInfoNo(BaseInfoType infoType)
        {
            // 根据信息类型枚举选择不同的规则
            string rule = GetBaseInfoNoRule(infoType);
            return _bnrFactory.Create(rule);
        }
        
        /// <summary>
        /// 生成基础信息编号（带参数）
        /// </summary>
        /// <param name="infoType">信息类型枚举</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>生成的信息编号</returns>
        public string GenerateBaseInfoNo(BaseInfoType infoType, string paraConst)
        {
            // 根据信息类型枚举和常量参数选择规则
            string rule = GetBaseInfoNoRule(infoType, paraConst);
            string result = _bnrFactory.Create(rule);
            
            // 如果有常量参数，将其添加到结果前面
            if (!string.IsNullOrEmpty(paraConst))
            {
                return paraConst + result;
            }
            
            return result;
        }
        
        /// <summary>
        /// 生成基础信息编号（兼容字符串类型参数）
        /// </summary>
        /// <param name="infoTypeStr">信息类型字符串</param>
        /// <returns>生成的信息编号</returns>
        public string GenerateBaseInfoNo(string infoTypeStr)
        {
            // 尝试将字符串转换为枚举
            if (Enum.TryParse<BaseInfoType>(infoTypeStr, true, out var infoType))
            {
                return GenerateBaseInfoNo(infoType);
            }
            // 如果转换失败，使用字符串处理（保留兼容性）
            string rule = GetBaseInfoNoRule(infoTypeStr);
            return _bnrFactory.Create(rule);
        }
        
        /// <summary>
        /// 生成基础信息编号（兼容字符串类型参数，带常量参数）
        /// </summary>
        /// <param name="infoTypeStr">信息类型字符串</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>生成的信息编号</returns>
        public string GenerateBaseInfoNo(string infoTypeStr, string paraConst)
        {
            // 尝试将字符串转换为枚举
            if (Enum.TryParse<BaseInfoType>(infoTypeStr, true, out var infoType))
            {
                return GenerateBaseInfoNo(infoType, paraConst);
            }
            // 如果转换失败，使用字符串处理（保留兼容性）
            string rule = GetBaseInfoNoRule(infoTypeStr, paraConst);
            string result = _bnrFactory.Create(rule);
            
            if (!string.IsNullOrEmpty(paraConst))
            {
                return paraConst + result;
            }
            
            return result;
        }
        
        /// <summary>
        /// 生成产品编码
        /// </summary>
        /// <param name="productCategory">产品类别</param>
        /// <returns>生成的产品编码</returns>
        public string GenerateProductNo(string productCategory = null)
        {
            // 使用与原始BizCodeGenerationHelper一致的规则
            string rule = "{S:P}{Hex:yyMM}{DB:{S:ProductNo}/000}";
            return _bnrFactory.Create(rule);
        }
        
        /// <summary>
        /// 生成产品SKU编码
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="attributes">属性组合</param>
        /// <returns>生成的SKU编码</returns>
        public string GenerateProductSKUNo(string productId = null, string attributes = null)
        {
            // 使用与原始BizCodeGenerationHelper一致的规则
            string rule = "{S:SK}{Hex:yyMM}{DB:SKU_No/0000}";
            return _bnrFactory.Create(rule);
        }
        
        /// <summary>
        /// 根据规则生成编号
        /// </summary>
        /// <param name="rule">编码规则</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>生成的编号</returns>
        public string GenerateByRule(string rule, Dictionary<string, object> parameters = null)
        {
            // 如果有额外参数，可以在这里处理
            // 目前BNRFactory可能不直接支持参数传递，后续可以扩展
            return _bnrFactory.Create(rule);
        }
        
        /// <summary>
        /// 获取单据编号规则（基于字符串类型）
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <returns>编码规则</returns>
        private string GetBillNoRule(string billType)
        {
            // 根据单据类型返回对应的规则
            switch (billType.ToUpper())
            {
                case "SO": // 销售订单
                    return "{{S:SO}}{{D:yyMMdd}}{{DB:{S:销售订单}{D:yyMM}/000}}".ToUpper();
                case "PO": // 采购订单
                    return "{{S:PO}}{{Hex:yyMMdd}}{{DB:{S:采购订单}{D:yyMM}/000}}".ToUpper();
                case "IN": // 入库单
                    return "{{S:IN}}{{D:yyMMdd}}{{DB:{S:其他入库单}{D:yyMM}/000}}".ToUpper();
                case "OUT": // 出库单
                    return "{{S:OUT}}{{D:yyMMdd}}{{DB:{S:其他出库单}{D:yyMM}/000}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{S:{billType}}}{{D:yyMMdd}}{{DB:{billType}/000}}".ToUpper();
            }
        }
        
        /// <summary>
        /// 获取单据编号规则（基于BizType枚举）
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <returns>编码规则</returns>
        private string GetBillNoRule(BizType bizType)
        {
            // 根据业务类型枚举返回对应的规则，与原始BizCodeGenerationHelper保持一致
            // 移除整体ToUpper()调用，为需要大写的参数添加:upper后缀，保持日期格式的正确大小写
            switch (bizType)
            {
                case BizType.销售订单:
                    return "{{S:SO:upper}}{{D:yyMMdd}}{{DB:{S:销售订单}{D:yyMM}/000}}";
                case BizType.销售出库单:
                    return "{{S:SOD:upper}}{{Hex:yyMM}}{{DB:{S:销售出库单}{D:yyMM}/0000}}";
                case BizType.销售退回单:
                    return "{{S:SODR:upper}}{{D:yyMMdd}}{{DB:{S:销售退回单}{D:yyMM}/000}}";
                case BizType.采购订单:
                    return "{{S:PO:upper}}{{Hex:yyMMdd}}{{DB:{S:采购订单}{D:yyMM}/000}}";
                case BizType.采购入库单:
                    return "{{S:PIR:upper}}{{D:yyMMdd}}{{DB:{S:采购入库单}{D:yyMM}/000}}";
                case BizType.采购退货单:
                    return "{{S:PIRR:upper}}{{D:yyMMdd}}{{DB:{S:采购退货单}{D:yyMM}/000}}";
                case BizType.其他入库单:
                    return "{{S:OIR:upper}}{{D:yyMMdd}}{{DB:{S:其他入库单}{D:yyMM}/000}}";
                case BizType.其他出库单:
                    return "{{S:OQD:upper}}{{D:yyMMdd}}{{DB:{S:其他出库单}{D:yyMM}/000}}";
                case BizType.盘点单:
                    return "{{S:CS:upper}}{{D:yyMMdd}}{{DB:{S:盘点单}{D:yyMM}/000}}";
                case BizType.BOM物料清单:
                    return "{{S:BS:upper}}{{D:yyMMdd}}{{DB:{S:BOM物料清单}{D:yyMM}/000}}";
                case BizType.其他费用支出:
                    return "{{S:EXP:upper}}{{D:yyMMdd}}{{DB:{S:其他费用支出}{D:yyMM}/000}}";
                case BizType.其他费用收入:
                    return "{{S:INC:upper}}{{D:yyMMdd}}{{DB:{S:其他费用收入}{D:yyMM}/000}}";
                case BizType.费用报销单:
                    return "{{S:EC:upper}}{{D:yyMMdd}}{{DB:{S:费用报销单}{D:yyMM}/000}}";
                case BizType.生产计划单:
                    return "{{S:PP:upper}}{{D:yyMMdd}}{{DB:{S:生产计划单}{D:yyMM}/00}}";
                case BizType.需求分析:
                    return "{{S:PD:upper}}{{D:yyMMdd}}{{DB:{S:生产需求分析}{D:yyMM}/00}}";
                case BizType.制令单:
                    return "{{S:MO:upper}}{{D:yyMMdd}}{{DB:{S:制令单}{D:yyMM}/00}}";
                case BizType.请购单:
                    return "{{S:RP:upper}}{{D:yyMMdd}}{{DB:{S:请购单}{D:yyMM}/000}}";
                case BizType.生产领料单:
                    return "{{S:PRD:upper}}{{D:yyMMdd}}{{DB:{S:生产领料单}{D:yyMM}/000}}";
                case BizType.生产退料单:
                    return "{{S:PRR:upper}}{{D:yyMMdd}}{{DB:{S:生产退料单}{D:yyMM}/000}}";
                case BizType.缴库单:
                    return "{{S:PR:upper}}{{D:yyMMdd}}{{DB:{S:缴库单}{D:yyMM}/000}}";
                case BizType.产品分割单:
                    return "{{S:PS:upper}}{{D:yyMMdd}}{{DB:{S:产品分割单}{D:yyMM}/00}}";
                case BizType.产品组合单:
                    return "{{S:PM:upper}}{{D:yyMMdd}}{{DB:{S:产品组合单}{D:yyMM}/00}}";
                case BizType.借出单:
                    return "{{S:JC:upper}}{{D:yyMMdd}}{{DB:{S:借出单}{D:yyMM}/000}}";
                case BizType.归还单:
                    return "{{S:GH:upper}}{{D:yyMMdd}}{{DB:{S:归还单}{D:yyMM}/000}}";
                case BizType.产品转换单:
                    return "{{S:ZH:upper}}{{D:yyMMdd}}{{DB:{S:产品转换单}{D:yyMM}/000}}";
                case BizType.调拨单:
                    return "{{S:DB:upper}}{{D:yyMMdd}}{{DB:{S:调拨单}{D:yyMM}/000}}";
                case BizType.返工退库单:
                    return "{{S:RW:upper}}{{D:yyMMdd}}{{DB:{S:返工退库单}{D:yyMM}/00}}";
                case BizType.返工入库单:
                    return "{{S:RE:upper}}{{D:yyMMdd}}{{DB:{S:返工入库单}{D:yyMM}/00}}";
                case BizType.付款申请单:
                    return "{{S:PA:upper}}{{D:yyMMdd}}{{DB:{S:付款申请单}{D:yyMM}/00}}";
                case BizType.销售合同:
                    return "{{S:SC-:upper}}{{D:yyMMdd}}{{DB:{S:销售合同}{D:yyMM}/00}}";
                case BizType.预付款单:
                    return "{{S:YF:upper}}{{D:yyMMdd}}{{DB:{S:预付款单}{D:yyMM}/000}}";
                case BizType.预收款单:
                    return "{{S:YS:upper}}{{D:yyMMdd}}{{DB:{S:预收款单}{D:yyMM}/000}}";
                case BizType.付款单:
                    return "{{S:FK:upper}}{{D:yyMMdd}}{{DB:{S:付款单}{D:yyMM}/000}}";
                case BizType.收款单:
                    return "{{S:SK:upper}}{{D:yyMMdd}}{{DB:{S:收款单}{D:yyMM}/000}}";
                case BizType.应付款单:
                    return "{{S:AP:upper}}{{D:yyMMdd}}{{DB:{S:应付款单}{D:yyMM}/000}}";
                case BizType.应收款单:
                    return "{{S:AR:upper}}{{D:yyMMdd}}{{DB:{S:应收款单}{D:yyMM}/000}}";
                case BizType.对账单:
                    return "{{S:PS:upper}}{{D:yyMMdd}}{{DB:{S:对账单}{D:yyMM}/000}}";
                case BizType.损失确认单:
                    return "{{S:LO:upper}}{{D:yyMMdd}}{{DB:{S:损失确认单}{D:yyMM}/00}}";
                case BizType.溢余确认单:
                    return "{{S:OY:upper}}{{D:yyMMdd}}{{DB:{S:溢余确认单}{D:yyMM}/00}}";
                case BizType.付款核销:
                    return "{{S:FKHX:upper}}{{D:yyMMdd}}{{DB:{S:付款核销}{D:yyMM}/0000}}";
                case BizType.收款核销:
                    return "{{S:SKHX:upper}}{{D:yyMMdd}}{{DB:{S:收款核销}{D:yyMM}/0000}}";
                case BizType.销售价格调整单:
                    return "{{S:SPA:upper}}{{D:yyMMdd}}{{DB:{S:销售价格调整单}{D:yyMM}/000}}";
                case BizType.采购价格调整单:
                    return "{{S:PPA:upper}}{{D:yyMMdd}}{{DB:{S:采购价格调整单}{D:yyMM}/000}}";
                case BizType.采购退货入库:
                    return "{{S:PIRRE:upper}}{{D:yyMMdd}}{{DB:{S:采购退货入库}{D:yyMM}/000}}";
                case BizType.售后申请单:
                    return "{{S:ASAP:upper}}{{D:yyMMdd}}{{DB:{S:售后申请单}{D:yyMM}/000}}";
                case BizType.售后交付单:
                    return "{{S:ASAPD:upper}}{{D:yyMMdd}}{{DB:{S:售后交付单}{D:yyMM}/000}}";
                case BizType.维修工单:
                    return "{{S:ASRO:upper}}{{D:yyMMdd}}{{DB:{S:维修工单}{D:yyMM}/000}}";
                case BizType.维修入库单:
                    return "{{S:ASRIS:upper}}{{D:yyMMdd}}{{DB:{S:维修入库单}{D:yyMM}/000}}";
                case BizType.维修领料单:
                    return "{{S:ASRMR:upper}}{{D:yyMMdd}}{{DB:{S:维修领料单}{D:yyMM}/000}}";
                case BizType.报废单:
                    return "{{S:ASSD:upper}}{{D:yyMMdd}}{{DB:{S:报废单}{D:yyMM}/000}}";
                case BizType.售后借出单:
                    return "{{S:ASBR:upper}}{{D:yyMMdd}}{{DB:{S:售后借出单}{D:yyMM}/000}}";
                case BizType.售后归还单:
                    return "{{S:ASRE:upper}}{{D:yyMMdd}}{{DB:{S:售后归还单}{D:yyMM}/000}}";
                default:
                    // 默认规则 - 为前缀添加大写转换
                    return $"{{S:{bizType}:upper}}{{D:yyMMdd}}{{DB:{bizType}{{D:yyMM}}/000}}";
            }
        }
        
        /// <summary>
        /// 获取基础信息编号规则（字符串版本，仅用于兼容性）
        /// </summary>
        /// <param name="infoType">信息类型字符串</param>
        /// <returns>编码规则</returns>
        private string GetBaseInfoNoRule(string infoType)
        {
            // 根据信息类型返回对应的规则
            switch (infoType.ToUpper())
            {
                case "EMPLOYEE": // 员工编号
                    return "{{S:EMP}}{{DB:{S:Employee}/000}}".ToUpper();
                case "SUPPLIER": // 供应商编号
                    return "{{S:SU}}{{DB:{S:Supplier}/000}}".ToUpper();
                case "CUSTOMER": // 客户编号
                    return "{{S:CU}}{{DB:{S:Customer}/000}}".ToUpper();
                case "WAREHOUSE": // 仓库编号
                    return "{{S:ST}}{{DB:{S:Storehouse}/000}}".ToUpper();
                case "PRODUCTNO": // 产品编号
                    return "{{S:P}}{{Hex:yyMM}}{{DB:{S:ProductNo}/000}}".ToUpper();
                case "LOCATION": // 库位编号
                    return "{{S:L}}{{DB:{S:LOC}/000}}".ToUpper();
                case "SKU_NO": // SKU编号
                    return "{{S:SK}}{{Hex:yyMM}}{{DB:SKU_No/0000}}".ToUpper();
                case "MODULEDEFINITION": // 模块定义
                    return "{{S:MD}}{{DB:{S:ModuleDefinition}/000}}".ToUpper();
                case "DEPARTMENT": // 部门编号
                    return "{{S:D}}{{DB:{S:Department}/000}}".ToUpper();
                case "CVOTHER": // CVOther编号
                    return "{{S:CV}}{{DB:{S:CVOther}/000}}".ToUpper();
                case "STORECODE": // 门店编号
                    return "{{S:SHOP}}{{DB:{S:StoreCode}/000}}".ToUpper();
                case "PRODCATEGORIES": // 产品分类编号
                    return "{{S:C}}{{DB:{S:ProCategories}/000}}".ToUpper();
                case "BUSINESSPARTNER": // 业务伙伴编号
                    return "{{S:BP}}{{DB:{S:BusinessPartner}/0000}}".ToUpper();
                case "SHORTCODE": // 简码
                    return "{{S:SC}}{{DB:{S:ShortCode}/000}}".ToUpper();
                case "PROJECTGROUPCODE": // 项目组编号
                    return "{{S:PG}}{{DB:{S:ProjectGroupCode}/000}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{S:{infoType}}}{{DB:{infoType}/000}}".ToUpper();
            }
        }
        
        /// <summary>
        /// 获取基础信息编号规则（带参数，字符串版本，仅用于兼容性）
        /// </summary>
        /// <param name="infoType">信息类型字符串</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>编码规则</returns>
        private string GetBaseInfoNoRule(string infoType, string paraConst)
        {
            // 根据信息类型和常量参数返回对应的规则
            switch (infoType.ToUpper())
            {
                case "SHORTCODE": // 简码
                    return $"{{DB:S:{paraConst}/000}}".ToUpper();
                case "FMSUBJECT": // 会计科目
                    return "{{DB:BST/000}}".ToUpper();
                case "CRM_REGIONCODE": // 地区编码
                    return "{{DB:CRC/00}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{DB:{paraConst}/000}}".ToUpper();
            }
        }
        
        /// <summary>
        /// 获取基础信息编号规则（枚举版本，主要使用版本）
        /// </summary>
        /// <param name="infoType">信息类型枚举</param>
        /// <returns>编码规则</returns>
        private string GetBaseInfoNoRule(BaseInfoType infoType)
        {
            // 根据信息类型枚举返回对应的规则
            switch (infoType)
            {
                case BaseInfoType.Employee: // 员工编号
                    return "{{S:EMP}}{{DB:{S:Employee}/000}}".ToUpper();
                case BaseInfoType.Supplier: // 供应商编号
                    return "{{S:SU}}{{DB:{S:Supplier}/000}}".ToUpper();
                case BaseInfoType.Customer: // 客户编号
                    return "{{S:CU}}{{DB:{S:Customer}/000}}".ToUpper();
                case BaseInfoType.Storehouse: // 仓库编号
                    return "{{S:ST}}{{DB:{S:Storehouse}/000}}".ToUpper();
                case BaseInfoType.ProductNo: // 产品编号
                    return "{{S:P}}{{Hex:yyMM}}{{DB:{S:ProductNo}/000}}".ToUpper();
                case BaseInfoType.Location: // 库位编号
                    return "{{S:L}}{{DB:{S:LOC}/000}}".ToUpper();
                case BaseInfoType.SKU_No: // SKU编号
                    return "{{S:SK}}{{Hex:yyMM}}{{DB:SKU_No/0000}}".ToUpper();
                case BaseInfoType.ModuleDefinition: // 模块定义
                    return "{{S:MD}}{{DB:{S:ModuleDefinition}/000}}".ToUpper();
                case BaseInfoType.Department: // 部门编号
                    return "{{S:D}}{{DB:{S:Department}/000}}".ToUpper();
                case BaseInfoType.CVOther: // CVOther编号
                    return "{{S:CV}}{{DB:{S:CVOther}/000}}".ToUpper();
                case BaseInfoType.StoreCode: // 门店编号
                    return "{{S:SHOP}}{{DB:{S:StoreCode}/000}}".ToUpper();
                case BaseInfoType.ProCategories: // 产品分类编号
                    return "{{S:C}}{{DB:{S:ProCategories}/000}}".ToUpper();
                case BaseInfoType.BusinessPartner: // 业务伙伴编号
                    return "{{S:BP}}{{DB:{S:BusinessPartner}/0000}}".ToUpper();
                case BaseInfoType.ShortCode: // 简码
                    return "{{S:SC}}{{DB:{S:ShortCode}/000}}".ToUpper();
                case BaseInfoType.ProjectGroupCode: // 项目组编号
                    return "{{S:PG}}{{DB:{S:ProjectGroupCode}/000}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{S:{infoType}}}{{DB:{infoType}/000}}".ToUpper();
            }
        }
        
        /// <summary>
        /// 获取基础信息编号规则（带参数，枚举版本）
        /// </summary>
        /// <param name="infoType">信息类型枚举</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>编码规则</returns>
        private string GetBaseInfoNoRule(BaseInfoType infoType, string paraConst)
        {
            // 根据信息类型枚举和常量参数返回对应的规则
            switch (infoType)
            {
                case BaseInfoType.ShortCode: // 简码
                    return $"{{DB:S:{paraConst}/000}}".ToUpper();
                case BaseInfoType.FMSubject: // 会计科目
                    return "{{DB:BST/000}}".ToUpper();
                case BaseInfoType.CRM_RegionCode: // 地区编码
                    return "{{DB:CRC/00}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{DB:{paraConst}/000}}".ToUpper();
            }
        }
        
        /// <summary>
        /// 生成唯一的条码
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">条码补位字符</param>
        /// <returns>生成的唯一的13位ENA条码</returns>
        public string GenerateBarCode(string originalCode, char paddingChar = '0')
        {   
            if (string.IsNullOrEmpty(originalCode))
            {   
                throw new ArgumentNullException(nameof(originalCode), "原始编码不能为空");
            }
            
            // 为了确保唯一性，我们生成一个基于原始编码但添加了时间戳和随机数的组合编码
            string uniqueBaseCode = GenerateUniqueBaseCode(originalCode);
            
            //条码校验
            string ENA_13str = "131313131313";
            //定义输出条码
            string barcode = "";
            //临时生成条码
            string tmpbarcode = uniqueBaseCode;
            
            //判断条码长度不足12位用补位码补足
            if (tmpbarcode.Length < 12)
            {   
                tmpbarcode = tmpbarcode.PadLeft(12, paddingChar);
            }
            // 如果超过12位，只取后12位
            else if (tmpbarcode.Length > 12)
            {   
                tmpbarcode = tmpbarcode.Substring(tmpbarcode.Length - 12);
            }
            
            //计算校验位
            string checkstr = "";
            int sum = 0, j = 0;
            for (int i = 0; i < 12; i++)
            {   
                sum = sum + int.Parse(tmpbarcode[i].ToString())
                      * int.Parse(ENA_13str[i].ToString());
            }
            
            //取余数，如果余数大于0则校验位为10-J，否则为0
            j = sum % 10;
            if (j > 0) checkstr = (10 - j).ToString();
            else checkstr = "0";

            //获取最后条码
            barcode = tmpbarcode + checkstr;

            return barcode;
        }
        
        /// <summary>
        /// 生成唯一的基础编码
        /// 结合原始编码、时间戳和随机数，确保生成的编码具有高度唯一性
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <returns>唯一的基础编码</returns>
        private string GenerateUniqueBaseCode(string originalCode)
        {   
            // 使用当前时间戳（精确到毫秒）和一个随机数来增强唯一性
            string timestamp = DateTime.Now.ToString("HHmmssfff");
            string random = new Random().Next(100, 999).ToString();
            
            // 将原始编码、时间戳和随机数组合起来
            // 为了避免过长，只取原始编码的前几位（如果原始编码很长）
            string shortOriginalCode = originalCode.Length > 5 ? originalCode.Substring(0, 5) : originalCode;
            
            // 移除可能的非数字字符，只保留数字
            string numericCode = new string(shortOriginalCode.Where(char.IsDigit).ToArray());
            
            // 如果原始编码中没有数字，则使用ASCII码值
            if (string.IsNullOrEmpty(numericCode))
            {   
                numericCode = string.Join("", shortOriginalCode.Take(5).Select(c => ((int)c % 10).ToString()));
            }
            
            // 组合最终的唯一编码
            string uniqueCode = numericCode + timestamp + random;
            
            // 确保编码不会太长（最多20位）
            if (uniqueCode.Length > 20)
            {   
                uniqueCode = uniqueCode.Substring(uniqueCode.Length - 20);
            }
            
            return uniqueCode;
        }
    }
}