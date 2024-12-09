
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:42
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
    /// 联系人表-爱好跟进验证类
    /// </summary>
    /*public partial class tb_CRM_ContactValidator:AbstractValidator<tb_CRM_Contact>*/
    public partial class tb_CRM_ContactValidator : BaseValidatorGeneric<tb_CRM_Contact>
    {
        public tb_CRM_ContactValidator()
        {
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Customer_id).Must(CheckForeignKeyValue).WithMessage("目标客户:下拉选择值不正确。");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.SocialTools).MaximumLength(100).WithMessage("社交工具:不能超过最大长度,100.");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Contact_Name).MaximumLength(25).WithMessage("姓名:不能超过最大长度,25.");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Contact_Email).MaximumLength(50).WithMessage("邮箱:不能超过最大长度,50.");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Contact_Phone).MaximumLength(15).WithMessage("电话:不能超过最大长度,15.");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Position).MaximumLength(25).WithMessage("职位:不能超过最大长度,25.");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Preferences).MaximumLength(100).WithMessage("爱好:不能超过最大长度,100.");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Address).MaximumLength(127).WithMessage("联系地址:不能超过最大长度,127.");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
            RuleFor(tb_CRM_Contact => tb_CRM_Contact.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

            Initialize();
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

