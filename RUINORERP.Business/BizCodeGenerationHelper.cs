﻿using BNR;
using RUINORERP.Common.CustomAttribute;
using RUINORERP.Extensions.Redis;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public class AutoComplete
    {
        public AutoComplete(SearchType searcherType)
        {
            SearcherType = searcherType;
        }
        public SearchType SearcherType { get; private set; }

    }

    // 日期格式枚举
    public enum DateFormat
    {
        [Description("年月日yyyyMMdd")]
        YearMonthDay,  // yyyyMMdd
        [Description("年月yyyyMM")]
        YearMonth,     // yyyyMM
        [Description("年周yyyyWW")]
        YearWeek       // yyyyWW
    }

    //0=不重置 1=按日 2=按月 3=按年
    public enum ResetMode
    {
        [Description("不重置")]
        None = 0,
        [Description("按日")]
        Day = 1,
        [Description("按月")]
        Month = 2,
        [Description("按年")]
        Year = 3
    }



    /// <summary>
    /// 业务编号生成助手 如果生成的序列号会重复，可能单机上不会重复。多台同时操作。
    /// 可能会重复员。则可以通知服务器上生成。socket传送
    /// </summary>

    public class BizCodeGenerator
    {



        private string redisServerIP = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string RedisServerIP { get => redisServerIP; set => redisServerIP = value; }
        public BizCodeGenerator()
        {
            //RedisHelper.ConnectionString = "192.168.0.254:6379";
            RedisHelper.DbNum = 1;//使用第一个数据库
            //RedisSequenceParameter 对应 redis  类中有特性标识
            BNRFactory.Default.Register("redis", new RedisSequenceParameter(RedisHelper.Db));
        }

        private static BizCodeGenerator m_instance = null;
        public static BizCodeGenerator Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Initialize();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }



        public static void Initialize()
        {
            m_instance = new BizCodeGenerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BIT"></param>
        /// <returns></returns>
        public string GetBaseInfoNo(BaseInfoType BIT)
        {
            //string rule = "{S:OD}{CN:广州}{D:yyyyMM}{N:{S:ORDER}{CN:广州}{D:yyyyMM}/00000000}{N:{S:ORDER_SUB}{CN:广州}{D:yyyyMM}/00000000}";
            string rule = "{S:OD}{CN:广州}{D:yyyyMM}{N:{S:ORDER}{CN:广州}{D:yyyyMM}/00000000}{N:{S:ORDER_SUB}{CN:广州}{D:yyyyMM}/00000000}";

            switch (BIT)
            {
                case BaseInfoType.ModuleDefinition:
                    rule = "{S:MD}{redis:{S:ModuleDefinition}/000}";
                    break;
                case BaseInfoType.ProductNo:
                    //rule = "{S:P}{D:yyMM}{redis:P/000}";//bst001
                    rule = "{S:P}{Hex:yyMM}{redis:{S:ProductNo}/000}";

                    break;
                case BaseInfoType.Employee:
                    rule = "{S:EMP}{redis:{S:Employee}/000}";
                    break;

                case BaseInfoType.Department:
                    rule = "{S:D}{redis:{S:Department}/000}";
                    break;
                case BaseInfoType.Storehouse:
                    rule = "{S:ST}{redis:{S:Storehouse}/000}";
                    break;
                case BaseInfoType.Supplier:
                    rule = "{S:SU}{redis:{S:Supplier}/000}";
                    break;
                case BaseInfoType.Customer:
                    rule = "{S:CU}{redis:{S:Customer}/000}";
                    break;
                case BaseInfoType.CVOther:
                    rule = "{S:CV}{redis:{S:CVOther}/000}";
                    break;
                case BaseInfoType.StoreCode:
                    rule = "{S:SHOP}{redis:{S:StoreCode}/000}";
                    break;

                case BaseInfoType.Location:

                    //生成的编码是L开头，在缓存数据库是LOC标识
                    rule = "{S:L}{redis:{S:LOC}/000}";
                    break;
                case BaseInfoType.SKU_No:
                    rule = "{S:SK}{Hex:yyMM}{redis:SKU_No/0000}";
                    break;
                case BaseInfoType.ProCategories:
                    rule = "{S:C}{redis:{S:ProCategories}/000}";
                    break;
                case BaseInfoType.BusinessPartner:
                    rule = "{S:BP}{redis:{S:BusinessPartner}/0000}";
                    break;
                //case BaseInfoType.ShortCode:
                //    rule = "{S:SC}{Hex:yyMM}{redis:C/000}";
                case BaseInfoType.ShortCode:
                    rule = "{S:SC}{redis:{S:ShortCode}/000}";
                    break;
                case BaseInfoType.ProjectGroupCode:
                    rule = "{S:PG}{redis:{S:ProjectGroupCode}/000}";
                    break;
                default:
                    break;
            }
            //rule + "{D:yy}{N:{D:yyyMMdd}/0000}{S:RJ}{N:ORDER/00000}";



            string BaseInfoNo = BNRFactory.Default.Create(rule);
            return BaseInfoNo;
        }

        /// <summary>
        /// 产生基本信息编号
        /// </summary>
        /// <param name="ParaConst">常量固定参数</param>
        /// <param name="BIT">基本信息类型关联性</param>
        /// <returns></returns>
        public string GetBaseInfoNo(BaseInfoType BIT, string ParaConst)
        {
            string rule = "{S:OD}{CN:广州}{D:yyyyMM}{N:{S:ORDER}{CN:广州}{D:yyyyMM}/00000000}{N:{S:ORDER_SUB}{CN:广州}{D:yyyyMM}/00000000}";
            switch (BIT)
            {
                case BaseInfoType.BusinessPartner:
                    break;
                case BaseInfoType.ProductNo:
                    break;
                case BaseInfoType.FMSubject:
                    rule = "{redis:BST/000}";//bst001
                    break;
                case BaseInfoType.CRM_RegionCode:
                    rule = "{redis:CRC/00}";//bst001
                    break;
                case BaseInfoType.ShortCode:
                    rule = "{redis:{S:" + ParaConst + "}/000}";//            "{redis:" + ParaConst + "/000}";
                    break;
                case BaseInfoType.ModuleDefinition:
                    break;
                case BaseInfoType.ProCategories:
                    break;
                case BaseInfoType.Employee:
                    break;
                case BaseInfoType.Department:
                    break;
                case BaseInfoType.Storehouse:
                    break;
                case BaseInfoType.Supplier:
                    break;
                case BaseInfoType.Customer:
                    break;
                case BaseInfoType.Location:
                    break;
                case BaseInfoType.SKU_No:
                    break;
                case BaseInfoType.StoreCode:
                    break;
                default:
                    break;
            }
            string BaseInfoNo = BNRFactory.Default.Create(rule);
            return ParaConst + BaseInfoNo;
        }


        /// <summary>
        /// 前两码是单据识别码，第三码为年，如此类往下推，後三码为当天的流号
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public string GetBizBillNo(BizType bt, BizCodeParameter bizCodePara = null)
        {
            string rule = "{S:NO}{D:yy}{redis:{S:ORDER}{D:dd}/0000}{redis:{S:ORDER_SUB}{CN:广州}{D:yyyyMM}/00000000}";
            string BizCode = string.Empty;
            switch (bt)
            {
                case BizType.销售订单:
                    rule = "{S:SO}{D:yyMMdd}{redis:{S:销售订单}{D:yyMM}/000}";
                    break;
                case BizType.销售出库单:
                    rule = "{S:SOD}{Hex:yyMM}{redis:{S:销售出库单}{D:yyMM}/0000}";
                    break;
                case BizType.销售退回单:
                    rule = "{S:SODR}{D:yyMMdd}{redis:{S:销售退回单}{D:yyMM}/000}";
                    break;
                case BizType.采购订单:
                    rule = "{S:PO}{Hex:yyMMdd}{redis:{S:采购订单}{D:yyMM}/000}";
                    break;
                case BizType.采购入库单:
                    rule = "{S:PIR}{D:yyMMdd}{redis:{S:采购入库单}{D:yyMM}/000}";
                    break;
                case BizType.采购退货单:
                    rule = "{S:PIRR}{D:yyMMdd}{redis:{S:采购退货单}{D:yyMM}/000}";
                    break;
                case BizType.采购退货入库:
                    rule = "{S:PIRRE}{D:yyMMdd}{redis:{S:采购退货入库}{D:yyMM}/000}";
                    break;
                case BizType.其他入库单:
                    rule = "{S:OIR}{D:yyMMdd}{redis:{S:其他入库单}{D:yyMM}/000}";
                    break;
                case BizType.其他出库单:
                    rule = "{S:OQD}{D:yyMMdd}{redis:{S:其他出库单}{D:yyMM}/000}";
                    break;

                case BizType.盘点单:
                    rule = "{S:CS}{D:yyMMdd}{redis:{S:盘点单}{D:yyMM}/000}";
                    break;
                case BizType.BOM物料清单://标准配方  BS， 订单配方 BO
                    rule = "{S:BS}{D:yyMMdd}{redis:{S:BOM物料清单}{D:yyMM}/000}";
                    break;
                case BizType.其他费用支出:
                    rule = "{S:EXP}{D:yyMMdd}{redis:{S:其他费用支出}{D:yyMM}/000}";
                    break;
                case BizType.其他费用收入:
                    rule = "{S:INC}{D:yyMMdd}{redis:{S:其他费用收入}{D:yyMM}/000}";
                    break;
                case BizType.费用报销单:
                    rule = "{S:EC}{D:yyMMdd}{redis:{S:费用报销单}{D:yyMM}/000}";
                    break;
                case BizType.生产计划单:
                    rule = "{S:PP}{D:yyMMdd}{redis:{S:生产计划单}{D:yyMM}/00}";
                    break;
                case BizType.需求分析:
                    rule = "{S:PD}{D:yyMMdd}{redis:{S:生产需求分析}{D:yyMM}/00}";
                    break;
                case BizType.制令单:
                    rule = "{S:MO}{D:yyMMdd}{redis:{S:制令单}{D:yyMM}/00}";
                    break;
                case BizType.请购单:
                    rule = "{S:RP}{D:yyMMdd}{redis:{S:请购单}{D:yyMM}/000}";
                    break;
                case BizType.生产领料单://Production Requisition Document
                    rule = "{S:PRD}{D:yyMMdd}{redis:{S:生产领料单}{D:yyMM}/000}";
                    break;
                case BizType.生产退料单://Production Requisition Document
                    rule = "{S:PRR}{D:yyMMdd}{redis:{S:生产退料单}{D:yyMM}/000}";
                    break;
                case BizType.缴库单://Production Requisition Document
                    rule = "{S:PR}{D:yyMMdd}{redis:{S:缴库单}{D:yyMM}/000}";
                    break;
                case BizType.产品分割单://Production Requisition Document
                    rule = "{S:PS}{D:yyMMdd}{redis:{S:产品分割单}{D:yyMM}/00}";
                    break;
                case BizType.产品组合单://Production Requisition Document
                    rule = "{S:PM}{D:yyMMdd}{redis:{S:产品组合单}{D:yyMM}/00}";
                    break;
                case BizType.借出单://Production Requisition Document
                    rule = "{S:JC}{D:yyMMdd}{redis:{S:借出单}{D:yyMM}/000}";
                    break;
                case BizType.归还单://Production Requisition Document
                    rule = "{S:GH}{D:yyMMdd}{redis:{S:归还单}{D:yyMM}/000}";
                    break;
                case BizType.产品转换单://Production Requisition Document
                    rule = "{S:ZH}{D:yyMMdd}{redis:{S:产品转换单}{D:yyMM}/000}";
                    break;
                case BizType.调拨单://Production Requisition Document
                    rule = "{S:DB}{D:yyMMdd}{redis:{S:调拨单}{D:yyMM}/000}";
                    break;
                case BizType.返工退库单:
                    rule = "{S:RW}{D:yyMMdd}{redis:{S:返工退库单}{D:yyMM}/00}";
                    break;
                case BizType.返工入库单:
                    rule = "{S:RE}{D:yyMMdd}{redis:{S:返工入库单}{D:yyMM}/00}";
                    break;
                case BizType.付款申请单:
                    rule = "{S:PA}{D:yyMMdd}{redis:{S:付款申请单}{D:yyMM}/00}";
                    break;
                case BizType.销售合同:
                    rule = "{S:SC-}{D:yyMMdd}{redis:{S:销售合同}{D:yyMM}/00}";
                    break;
                case BizType.预付款单:
                    rule = "{S:YF}{D:yyMMdd}{redis:{S:预付款单}{D:yyMM}/000}";
                    break;
                case BizType.预收款单:
                    rule = "{S:YS}{D:yyMMdd}{redis:{S:预收款单}{D:yyMM}/000}";
                    break;
                case BizType.付款单:
                    rule = "{S:FK}{D:yyMMdd}{redis:{S:付款单}{D:yyMM}/000}";
                    break;
                case BizType.收款单:
                    rule = "{S:SK}{D:yyMMdd}{redis:{S:收款单}{D:yyMM}/000}";
                    break;
                case BizType.应付款单:
                    rule = "{S:AP}{D:yyMMdd}{redis:{S:应付单}{D:yyMM}/000}";
                    break;
                case BizType.应收款单:
                    rule = "{S:AR}{D:yyMMdd}{redis:{S:应收单}{D:yyMM}/000}";
                    break;
                case BizType.收款核销:
                    rule = "{S:SKHX}{D:yyMMdd}{redis:{S:收款核销}{D:yyMM}/000}";
                    break;
                case BizType.付款核销:
                    rule = "{S:FKHX}{D:yyMMdd}{redis:{S:付款核销}{D:yyMM}/000}";
                    break;
                case BizType.销售价格调整单:
                    rule = "{S:SPA}{D:yyMMdd}{redis:{S:销售价格调整单}{D:yyMM}/000}";
                    break;
                case BizType.采购价格调整单:
                    rule = "{S:PPA}{D:yyMMdd}{redis:{S:采购价格调整单}{D:yyMM}/000}";
                    break;
                case BizType.售后申请单:
                    rule = "{S:ASAP}{D:yyMMdd}{redis:{S:售后申请单}{D:yyMM}/000}";
                    break;
                case BizType.售后交付单:
                    rule = "{S:ASAPD}{D:yyMMdd}{redis:{S:售后交付单}{D:yyMM}/000}";
                    break;
                case BizType.维修工单:
                    rule = "{S:ASRO}{D:yyMMdd}{redis:{S:维修工单}{D:yyMM}/000}";
                    break;
                case BizType.维修入库单:
                    rule = "{S:ASRIS}{D:yyMMdd}{redis:{S:维修入库单}{D:yyMM}/000}";
                    break;
                case BizType.维修领料单:
                    rule = "{S:ASRMR}{D:yyMMdd}{redis:{S:维修领料单}{D:yyMM}/000}";
                    break;
                case BizType.报废单:
                    rule = "{S:ASSD}{D:yyMMdd}{redis:{S:报废单}{D:yyMM}/000}";
                    break;
                case BizType.售后借出单:
                    rule = "{S:ASBR}{D:yyMMdd}{redis:{S:售后借出单}{D:yyMM}/000}";
                    break;
                case BizType.售后归还单:
                    rule = "{S:ASRE}{D:yyMMdd}{redis:{S:售后归还单}{D:yyMM}/000}";
                    break;
                default:
                    break;
            }
            //序号时，如果开多个端。是否会生成重复呢？
            //如果重复 则可以放到服务器用socket传输。
            //  BizCode = BNRFactory.Default.Create(rule);
            // DefaultRedis.Instance.Host.AddWriteHost("localhost");
            // var number =  BNRFactory.Default.Create("[CN:广州][redis:[D:yyyy]/000000]");
            BizCode = BNRFactory.Default.Create(rule);

            return BizCode;
        }

        /*
        #region 优化用到的方法


        // 优化后的单号生成方法
        public string GetBizBillNo(BizType bt, string companyCode = "CN")
        {
            string prefix = GetBizPrefix(bt);
            string datePart = GetDatePart(bt);
            string sequence = GetSequence(bt, datePart);
            string checkDigit = GetCheckDigit(prefix + datePart + sequence);

            return $"{prefix}{datePart}{sequence}{checkDigit}";
        }

        // 获取业务前缀
        private string GetBizPrefix(BizType bt)
        {
            switch (bt)
            {
                case BizType.销售订单: return "SO";
                case BizType.销售出库单: return "STO";
                case BizType.采购订单: return "PO";
                case BizType.采购入库单: return "PIO";
                case BizType.制令单: return "MO";
                case BizType.BOM物料清单: return "BOM";
                case BizType.生产领料单: return "MPR";
                case BizType.应收款单: return "AR";
                case BizType.应付款单: return "AP";
                case BizType.付款单: return "PYM";
                case BizType.收款单: return "RCP";
                default: return "SYS";
            }
        }

        // 获取日期部分
        private string GetDatePart(BizType bt)
        {
            // 关键业务使用年月日格式，次要业务使用年月格式
            if (bt == BizType.销售订单 || bt == BizType.采购订单 || bt == BizType.制令单)
                return DateTime.Now.ToString("yyyyMMdd");
            else
                return DateTime.Now.ToString("yyyyMM");
        }

        // 获取流水号（确保分布式环境唯一）
        private string GetSequence(BizType bt, string datePart)
        {
            string key = $"SEQ:{bt}:{datePart}";
            // 使用Redis原子操作生成流水号
            long seq = redisDb.Increment(key);

            // 根据业务类型设置流水号长度
            int length = bt == BizType.BOM物料清单 ? 4 : 5;
            return seq.ToString().PadLeft(length, '0');
        }

        // 生成校验码（增强数据完整性）
        private string GetCheckDigit(string code)
        {
            int sum = 0;
            for (int i = 0; i < code.Length; i++)
            {
                if (char.IsLetter(code[i]))
                    sum += (code[i] - 'A' + 10) * (i % 2 == 0 ? 3 : 1);
                else
                    sum += int.Parse(code[i].ToString()) * (i % 2 == 0 ? 3 : 1);
            }
            int checkDigit = (10 - sum % 10) % 10;
            return checkDigit.ToString();
        }

        // 优化后的基本信息编号生成
        public string GetBaseInfoNo(BaseInfoType bit)
        {
            string prefix = GetBasePrefix(bit);
            string sequence = redisDb.Increment($"BASE:{bit}").ToString().PadLeft(5, '0');
            return $"{prefix}{sequence}";
        }

        private string GetBasePrefix(BaseInfoType bit)
        {
            switch (bit)
            {
                case BaseInfoType.ProductNo: return "PRD";
                case BaseInfoType.Employee: return "EMP";
                case BaseInfoType.Department: return "DEPT";
                case BaseInfoType.Storehouse: return "WH";
                case BaseInfoType.Supplier: return "SUPL";
                case BaseInfoType.Customer: return "CUST";
                default: return "SYS";
            }
        }

        // 分布式环境下的单号生成优化
        public string GetDistributedBizNo(BizType bt)
        {
            // 生成包含服务器标识的唯一键
            string serverId = Environment.MachineName;
            string key = $"BIZ:{bt}:{DateTime.Now:yyyyMMdd}:{serverId}";

            // 使用Redis的Lua脚本确保原子性
            string script = @"
        local key = KEYS[1]
        local val = redis.call('get', key)
        if val then
            return val
        else
            local seq = redis.call('incr', key)
            redis.call('expire', key, 86400) -- 24小时过期
            return seq
        end
    ";

            long seq = redisDb.ScriptEvaluate(script, new RedisKey[] { key });
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string serverCode = serverId.Length > 2 ? serverId.Substring(0, 2).ToUpper() : serverId.ToUpper();

            return $"BT{serverCode}{datePart}{seq.ToString().PadLeft(6, '0')}";
        }


        #endregion
        */
        public string GetProdNo(tb_ProdCategories pc)
        {
            string rule = "{S:OD}{CN:广州}{D:yyyyMM}{redis:{S:ORDER}{CN:广州}{D:yyyyMM}/00000000}{N:{S:ORDER_SUB}{CN:广州}{D:yyyyMM}/00000000}";
            string BizCode = BNRFactory.Default.Create(rule);

            return BizCode;
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="code">原始编码</param>
        /// <param name="bwcode">条码补位码</param>
        /// <returns></returns>
        public string GetBarCode(string code, char bwcode)
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

    /// <summary>
    /// 合表时根据这个参考生成对应的业务编号
    /// 如：预收付款表。
    /// 是预收 还是预付通过这个参数来定
    /// </summary>
    public class BizCodeParameter
    {
        //财务用
        public ReceivePaymentType PaymentType = ReceivePaymentType.收款;

    }

}
