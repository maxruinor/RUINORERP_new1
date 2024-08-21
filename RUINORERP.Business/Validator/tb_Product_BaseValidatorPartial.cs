
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/25/2023 13:09:43
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using FluentValidation;
using RUINORERP.Model;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace RUINORERP.Business
{
    /// <summary>
    /// 货品表验证类
    /// </summary>
    public partial class tb_Prod_BaseValidator : AbstractValidator<tb_Prod_Base>
    {
        tb_Prod_BaseController _controller;



        public tb_Prod_BaseValidator(tb_Prod_BaseController controller)
        {
            _controller = controller;
            RuleFor(tb_Product => tb_Product.Category_ID).NotEmpty().WithMessage("类别不能为空。");
            RuleFor(tb_Product => tb_Product.ProductNo).NotEmpty().WithMessage("品号不能为空。");
            RuleFor(x => x.CNName).NotEmpty().WithMessage("品名必须输入。").Must(UniqueName).WithMessage("这个品名已经存在。");
        }

        private bool UniqueName(string name)
        {
            // tb_ProductController pc = new tb_ProductController();
            bool rs=  _controller.NameIsExists(name);
            return !rs;
            //tb_Product _db = new tb_Product();
            //var category = _db..Where(x => x.Name.ToLower() == name.ToLower()).SingleOrDefault();
            //if (category == null) return true;
            // return false;
        }
        public void InitHelpInfo()
        {

        }
    }




}