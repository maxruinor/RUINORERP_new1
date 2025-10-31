using RUINORERP.IServices;
using RUINORERP.Server.BNR;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business;
using System;
using System.Collections.Generic;
using System.Text;
using RUINORERP.Global;
using RUINORERP.Model;

namespace RUINORERP.Server.Services.BizCode
{
    /// <summary>
    /// 业务编码服务实现
    /// 包装BNRFactory功能，提供业务编码生成服务
    /// </summary>
    public class BizCodeService : IBizCodeService
    {
        private readonly BNRFactory _bnrFactory;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bnrFactory">编号生成工厂实例</param>
        public BizCodeService(BNRFactory bnrFactory)
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
        /// <param name="infoType">信息类型</param>
        /// <returns>生成的信息编号</returns>
        public string GenerateBaseInfoNo(string infoType)
        {
            // 根据信息类型选择不同的规则
            string rule = GetBaseInfoNoRule(infoType);
            return _bnrFactory.Create(rule);
        }
        
        /// <summary>
        /// 生成基础信息编号（带参数）
        /// </summary>
        /// <param name="infoType">信息类型</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>生成的信息编号</returns>
        public string GenerateBaseInfoNo(string infoType, string paraConst)
        {
            // 根据信息类型和常量参数选择规则
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
        /// 生成产品编码
        /// </summary>
        /// <param name="productCategory">产品类别</param>
        /// <returns>生成的产品编码</returns>
        public string GenerateProductNo(string productCategory = null)
        {
            // 使用与原始BizCodeGenerationHelper一致的规则
            string rule = "{S:P}{Hex:yyMM}{redis:{S:ProductNo}/000}";
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
            string rule = "{S:SK}{Hex:yyMM}{redis:SKU_No/0000}";
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
                    return "{{S:SO}}{{D:yyMMdd}}{{redis:{S:销售订单}{D:yyMM}/000}}".ToUpper();
                case "PO": // 采购订单
                    return "{{S:PO}}{{Hex:yyMMdd}}{{redis:{S:采购订单}{D:yyMM}/000}}".ToUpper();
                case "IN": // 入库单
                    return "{{S:IN}}{{D:yyMMdd}}{{redis:{S:其他入库单}{D:yyMM}/000}}".ToUpper();
                case "OUT": // 出库单
                    return "{{S:OUT}}{{D:yyMMdd}}{{redis:{S:其他出库单}{D:yyMM}/000}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{S:{billType}}}{{D:yyMMdd}}{{redis:{billType}/000}}".ToUpper();
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
            switch (bizType)
            {
                case BizType.销售订单:
                    return "{{S:SO}}{{D:yyMMdd}}{{redis:{S:销售订单}{D:yyMM}/000}}".ToUpper();
                case BizType.销售出库单:
                    return "{{S:SOD}}{{Hex:yyMM}}{{redis:{S:销售出库单}{D:yyMM}/0000}}".ToUpper();
                case BizType.销售退回单:
                    return "{{S:SODR}}{{D:yyMMdd}}{{redis:{S:销售退回单}{D:yyMM}/000}}".ToUpper();
                case BizType.采购订单:
                    return "{{S:PO}}{{Hex:yyMMdd}}{{redis:{S:采购订单}{D:yyMM}/000}}".ToUpper();
                case BizType.采购入库单:
                    return "{{S:PIR}}{{D:yyMMdd}}{{redis:{S:采购入库单}{D:yyMM}/000}}".ToUpper();
                case BizType.采购退货单:
                    return "{{S:PIRR}}{{D:yyMMdd}}{{redis:{S:采购退货单}{D:yyMM}/000}}".ToUpper();
                case BizType.其他入库单:
                    return "{{S:OIR}}{{D:yyMMdd}}{{redis:{S:其他入库单}{D:yyMM}/000}}".ToUpper();
                case BizType.其他出库单:
                    return "{{S:OQD}}{{D:yyMMdd}}{{redis:{S:其他出库单}{D:yyMM}/000}}".ToUpper();
                case BizType.盘点单:
                    return "{{S:CS}}{{D:yyMMdd}}{{redis:{S:盘点单}{D:yyMM}/000}}".ToUpper();
                case BizType.BOM物料清单:
                    return "{{S:BS}}{{D:yyMMdd}}{{redis:{S:BOM物料清单}{D:yyMM}/000}}".ToUpper();
                case BizType.其他费用支出:
                    return "{{S:EXP}}{{D:yyMMdd}}{{redis:{S:其他费用支出}{D:yyMM}/000}}".ToUpper();
                case BizType.其他费用收入:
                    return "{{S:INC}}{{D:yyMMdd}}{{redis:{S:其他费用收入}{D:yyMM}/000}}".ToUpper();
                case BizType.费用报销单:
                    return "{{S:EC}}{{D:yyMMdd}}{{redis:{S:费用报销单}{D:yyMM}/000}}".ToUpper();
                case BizType.生产计划单:
                    return "{{S:PP}}{{D:yyMMdd}}{{redis:{S:生产计划单}{D:yyMM}/00}}".ToUpper();
                case BizType.需求分析:
                    return "{{S:PD}}{{D:yyMMdd}}{{redis:{S:生产需求分析}{D:yyMM}/00}}".ToUpper();
                case BizType.制令单:
                    return "{{S:MO}}{{D:yyMMdd}}{{redis:{S:制令单}{D:yyMM}/00}}".ToUpper();
                case BizType.请购单:
                    return "{{S:RP}}{{D:yyMMdd}}{{redis:{S:请购单}{D:yyMM}/000}}".ToUpper();
                case BizType.生产领料单:
                    return "{{S:PRD}}{{D:yyMMdd}}{{redis:{S:生产领料单}{D:yyMM}/000}}".ToUpper();
                case BizType.生产退料单:
                    return "{{S:PRR}}{{D:yyMMdd}}{{redis:{S:生产退料单}{D:yyMM}/000}}".ToUpper();
                case BizType.缴库单:
                    return "{{S:PR}}{{D:yyMMdd}}{{redis:{S:缴库单}{D:yyMM}/000}}".ToUpper();
                case BizType.产品分割单:
                    return "{{S:PS}}{{D:yyMMdd}}{{redis:{S:产品分割单}{D:yyMM}/00}}".ToUpper();
                case BizType.产品组合单:
                    return "{{S:PM}}{{D:yyMMdd}}{{redis:{S:产品组合单}{D:yyMM}/00}}".ToUpper();
                case BizType.借出单:
                    return "{{S:JC}}{{D:yyMMdd}}{{redis:{S:借出单}{D:yyMM}/000}}".ToUpper();
                case BizType.归还单:
                    return "{{S:GH}}{{D:yyMMdd}}{{redis:{S:归还单}{D:yyMM}/000}}".ToUpper();
                case BizType.产品转换单:
                    return "{{S:ZH}}{{D:yyMMdd}}{{redis:{S:产品转换单}{D:yyMM}/000}}".ToUpper();
                case BizType.调拨单:
                    return "{{S:DB}}{{D:yyMMdd}}{{redis:{S:调拨单}{D:yyMM}/000}}".ToUpper();
                case BizType.返工退库单:
                    return "{{S:RW}}{{D:yyMMdd}}{{redis:{S:返工退库单}{D:yyMM}/00}}".ToUpper();
                case BizType.返工入库单:
                    return "{{S:RE}}{{D:yyMMdd}}{{redis:{S:返工入库单}{D:yyMM}/00}}".ToUpper();
                case BizType.付款申请单:
                    return "{{S:PA}}{{D:yyMMdd}}{{redis:{S:付款申请单}{D:yyMM}/00}}".ToUpper();
                case BizType.销售合同:
                    return "{{S:SC-}}{{D:yyMMdd}}{{redis:{S:销售合同}{D:yyMM}/00}}".ToUpper();
                case BizType.预付款单:
                    return "{{S:YF}}{{D:yyMMdd}}{{redis:{S:预付款单}{D:yyMM}/000}}".ToUpper();
                case BizType.预收款单:
                    return "{{S:YS}}{{D:yyMMdd}}{{redis:{S:预收款单}{D:yyMM}/000}}".ToUpper();
                case BizType.付款单:
                    return "{{S:FK}}{{D:yyMMdd}}{{redis:{S:付款单}{D:yyMM}/000}}".ToUpper();
                case BizType.收款单:
                    return "{{S:SK}}{{D:yyMMdd}}{{redis:{S:收款单}{D:yyMM}/000}}".ToUpper();
                case BizType.应付款单:
                    return "{{S:AP}}{{D:yyMMdd}}{{redis:{S:应付款单}{D:yyMM}/000}}".ToUpper();
                case BizType.应收款单:
                    return "{{S:AR}}{{D:yyMMdd}}{{redis:{S:应收款单}{D:yyMM}/000}}".ToUpper();
                case BizType.对账单:
                    return "{{S:PS}}{{D:yyMMdd}}{{redis:{S:对账单}{D:yyMM}/000}}".ToUpper();
                case BizType.损失确认单:
                    return "{{S:LO}}{{D:yyMMdd}}{{redis:{S:损失确认单}{D:yyMM}/00}}".ToUpper();
                case BizType.溢余确认单:
                    return "{{S:OY}}{{D:yyMMdd}}{{redis:{S:溢余确认单}{D:yyMM}/00}}".ToUpper();
                case BizType.付款核销:
                    return "{{S:FKHX}}{{D:yyMMdd}}{{redis:{S:付款核销}{D:yyMM}/0000}}".ToUpper();
                case BizType.收款核销:
                    return "{{S:SKHX}}{{D:yyMMdd}}{{redis:{S:收款核销}{D:yyMM}/0000}}".ToUpper();
                case BizType.销售价格调整单:
                    return "{{S:SPA}}{{D:yyMMdd}}{{redis:{S:销售价格调整单}{D:yyMM}/000}}".ToUpper();
                case BizType.采购价格调整单:
                    return "{{S:PPA}}{{D:yyMMdd}}{{redis:{S:采购价格调整单}{D:yyMM}/000}}".ToUpper();
                case BizType.采购退货入库:
                    return "{{S:PIRRE}}{{D:yyMMdd}}{{redis:{S:采购退货入库}{D:yyMM}/000}}".ToUpper();
                case BizType.售后申请单:
                    return "{{S:ASAP}}{{D:yyMMdd}}{{redis:{S:售后申请单}{D:yyMM}/000}}".ToUpper();
                case BizType.售后交付单:
                    return "{{S:ASAPD}}{{D:yyMMdd}}{{redis:{S:售后交付单}{D:yyMM}/000}}".ToUpper();
                case BizType.维修工单:
                    return "{{S:ASRO}}{{D:yyMMdd}}{{redis:{S:维修工单}{D:yyMM}/000}}".ToUpper();
                case BizType.维修入库单:
                    return "{{S:ASRIS}}{{D:yyMMdd}}{{redis:{S:维修入库单}{D:yyMM}/000}}".ToUpper();
                case BizType.维修领料单:
                    return "{{S:ASRMR}}{{D:yyMMdd}}{{redis:{S:维修领料单}{D:yyMM}/000}}".ToUpper();
                case BizType.报废单:
                    return "{{S:ASSD}}{{D:yyMMdd}}{{redis:{S:报废单}{D:yyMM}/000}}".ToUpper();
                case BizType.售后借出单:
                    return "{{S:ASBR}}{{D:yyMMdd}}{{redis:{S:售后借出单}{D:yyMM}/000}}".ToUpper();
                case BizType.售后归还单:
                    return "{{S:ASRE}}{{D:yyMMdd}}{{redis:{S:售后归还单}{D:yyMM}/000}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{S:{bizType}}}{{D:yyMMdd}}{{redis:{bizType}{{D:yyMM}}/000}}".ToUpper();
            }
        }
        
        /// <summary>
        /// 获取基础信息编号规则
        /// </summary>
        /// <param name="infoType">信息类型</param>
        /// <returns>编码规则</returns>
        private string GetBaseInfoNoRule(string infoType)
        {
            // 根据信息类型返回对应的规则
            switch (infoType.ToUpper())
            {
                case "EMPLOYEE": // 员工编号
                    return "{{S:EMP}}{{redis:{S:Employee}/000}}".ToUpper();
                case "SUPPLIER": // 供应商编号
                    return "{{S:SU}}{{redis:{S:Supplier}/000}}".ToUpper();
                case "CUSTOMER": // 客户编号
                    return "{{S:CU}}{{redis:{S:Customer}/000}}".ToUpper();
                case "WAREHOUSE": // 仓库编号
                    return "{{S:ST}}{{redis:{S:Storehouse}/000}}".ToUpper();
                case "PRODUCTNO": // 产品编号
                    return "{{S:P}}{{Hex:yyMM}}{{redis:{S:ProductNo}/000}}".ToUpper();
                case "LOCATION": // 库位编号
                    return "{{S:L}}{{redis:{S:LOC}/000}}".ToUpper();
                case "SKU_NO": // SKU编号
                    return "{{S:SK}}{{Hex:yyMM}}{{redis:SKU_No/0000}}".ToUpper();
                case "MODULEDEFINITION": // 模块定义
                    return "{{S:MD}}{{redis:{S:ModuleDefinition}/000}}".ToUpper();
                case "DEPARTMENT": // 部门编号
                    return "{{S:D}}{{redis:{S:Department}/000}}".ToUpper();
                case "CVOTHER": // CVOther编号
                    return "{{S:CV}}{{redis:{S:CVOther}/000}}".ToUpper();
                case "STORECODE": // 门店编号
                    return "{{S:SHOP}}{{redis:{S:StoreCode}/000}}".ToUpper();
                case "PRODCATEGORIES": // 产品分类编号
                    return "{{S:C}}{{redis:{S:ProCategories}/000}}".ToUpper();
                case "BUSINESSPARTNER": // 业务伙伴编号
                    return "{{S:BP}}{{redis:{S:BusinessPartner}/0000}}".ToUpper();
                case "SHORTCODE": // 简码
                    return "{{S:SC}}{{redis:{S:ShortCode}/000}}".ToUpper();
                case "PROJECTGROUPCODE": // 项目组编号
                    return "{{S:PG}}{{redis:{S:ProjectGroupCode}/000}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{S:{infoType}}}{{redis:{infoType}/000}}".ToUpper();
            }
        }
        
        /// <summary>
        /// 获取基础信息编号规则（带参数）
        /// </summary>
        /// <param name="infoType">信息类型</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>编码规则</returns>
        private string GetBaseInfoNoRule(string infoType, string paraConst)
        {
            // 根据信息类型和常量参数返回对应的规则
            switch (infoType.ToUpper())
            {
                case "SHORTCODE": // 简码
                    return $"{{redis:S:{paraConst}/000}}".ToUpper();
                case "FMSUBJECT": // 会计科目
                    return "{{redis:BST/000}}".ToUpper();
                case "CRM_REGIONCODE": // 地区编码
                    return "{{redis:CRC/00}}".ToUpper();
                default:
                    // 默认规则
                    return $"{{redis:{paraConst}/000}}".ToUpper();
            }
        }
        
        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="code">原始编码</param>
        /// <param name="bwcode">条码补位码</param>
        /// <returns>生成的条码</returns>
        public string GenerateBarCode(string code, char bwcode = '0')
        {   
            //条码校验
            string ENA_13str = "131313131313";
            //定义输出条码
            string barcode = "";
            //临时生成条码
            string tmpbarcode = code;
            //判断条码长度不足12位用补位码补足
            if (tmpbarcode.Length < 12)
            {   
                tmpbarcode = tmpbarcode.PadLeft(12, bwcode);
            }
            //计算校验位
            string checkstr = "";
            int sum = 0, j = 0;
            for (int i = 0; i < ENA_13str.Length; i++)
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
    }
}