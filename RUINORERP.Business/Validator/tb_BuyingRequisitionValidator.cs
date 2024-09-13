
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:38
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 请购单，可能来自销售订单,也可以来自其它日常需求也可能来自生产需求也可以直接录数据，是一个纯业务性的数据表
    /// </summary>
    /*public partial class tb_BuyingRequisitionValidator:AbstractValidator<tb_BuyingRequisition>*/
    public partial class tb_BuyingRequisitionValidator : BaseValidatorGeneric<tb_BuyingRequisition>
    {
        public tb_BuyingRequisitionValidator()
        {
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.PuRequisitionNo).MaximumLength(50).WithMessage("请购单号:不能超过最大长度,50.");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.PuRequisitionNo).NotEmpty().WithMessage("请购单号:不能为空。");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.Employee_ID).Must(CheckForeignKeyValue).WithMessage("申请人:下拉选择值不正确。");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.Location_ID).Must(CheckForeignKeyValueCanNull).WithMessage("库位:下拉选择值不正确。");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("使用部门:下拉选择值不正确。");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.RefBillNO).MaximumLength(25).WithMessage("引用单据编号:不能超过最大长度,25.");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.RefBizType).NotEmpty().When(x => x.RefBizType.HasValue);
            //***** 
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.TotalQty).NotNull().WithMessage("总数量:不能为空。");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
            //***** 
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
            //***** 
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
            RuleFor(tb_BuyingRequisition => tb_BuyingRequisition.CloseCaseOpinions).MaximumLength(100).WithMessage("结案情况:不能超过最大长度,100.");

            //long?
            //PuRequisition_ID
            //tb_BuyingRequisitionDetail
            //RuleFor(x => x.tb_BuyingRequisitionDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
            //视图不需要验证，目前认为无编辑新增操作
            //RuleFor(c => c.tb_BuyingRequisitionDetails).NotNull();
            //RuleForEach(x => x.tb_BuyingRequisitionDetails).NotNull();
            //RuleFor(x => x.tb_BuyingRequisitionDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");

            Initialize();
        }




        private bool DetailedRecordsNotEmpty(List<tb_BuyingRequisitionDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }






        private bool CheckForeignKeyValue(long ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID == 0 || ForeignKeyID == -1)
            {
                return false;
            }
            return rs;
        }

        private bool CheckForeignKeyValueCanNull(long? ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID.HasValue)
            {
                if (ForeignKeyID == 0 || ForeignKeyID == -1)
                {
                    return false;
                }
            }
            return rs;

        }
    }

}

