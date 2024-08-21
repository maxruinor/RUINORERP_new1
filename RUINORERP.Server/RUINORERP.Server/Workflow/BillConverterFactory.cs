using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using RUINORERP.Server.Workflow.WFApproval;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow
{


    /// <summary>
    /// 目前的方法不合理。写死的
    /// 为了 比方传入采购订单类型 及单号就处理对应数据库的事
    /// </summary>
    public class BillConverterFactory
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<BillConverterFactory> _logger;

        public BillConverterFactory(
        ApplicationContext context,
        ILogger<BillConverterFactory> logger)
        {
            _context = context;
            _logger = logger;
        }



        public static void UpdateApprovalData(StepBodyAsync step, ApprovalWFData data)
        {
            switch (data.approvalEntity.bizType)
            {
                case BizType.销售订单:
                    break;
                case BizType.销售出库单:
                    break;
                case BizType.销售退回单:
                    break;
                case BizType.采购订单:
                    break;
                case BizType.采购入库单:
                    break;
                case BizType.返厂入库:
                    break;
                case BizType.返厂出库:
                    break;
                case BizType.售后入库:
                    break;
                case BizType.售后出库:
                    break;
                case BizType.报损单:
                    break;
                case BizType.报溢单:
                    break;
                case BizType.盘点单:
                    break;
                case BizType.制令单:
                    break;
                case BizType.生产领料单:
                    break;
                case BizType.生产退料单:
                    break;
                case BizType.生产补料单:
                    break;
                case BizType.发料计划单:
                    break;
                case BizType.成品缴库:
                    break;
                case BizType.托外加工单:
                    break;
                case BizType.托外领料单:
                    break;
                case BizType.托外退料单:
                    break;
                case BizType.托外补料单:
                    break;
                case BizType.托外加工缴回单:
                    break;
                default:
                    break;
            }

           
        }
    }
}
