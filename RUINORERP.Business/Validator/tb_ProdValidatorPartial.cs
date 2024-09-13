
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
    /// 产品表验证类
    /// </summary>
    public partial class tb_ProdValidator : BaseValidatorGeneric<tb_Prod>
    {
        tb_ProdController<tb_Prod> _controller;



        public tb_ProdValidator(tb_ProdController<tb_Prod> controller)
        {
            _controller = controller;
            RuleFor(tb_Product => tb_Product.Category_ID).NotEmpty().WithMessage("类别不能为空。");
            RuleFor(tb_Product => tb_Product.ProductNo).NotEmpty().WithMessage("品号不能为空。").Must(UniqueProductNo).WithMessage("这个品号已经存在。"); ;
            RuleFor(x => x.CNName).NotEmpty().WithMessage("品名必须输入。").Must(UniqueName).WithMessage("这个品名已经存在。");
            RuleFor(tb_Prod => tb_Prod.PropertyType).NotNull().WithMessage("产品类型:不能为空。");
            RuleFor(x => x.PropertyType).NotEmpty().WithMessage("属性类型必须输入。");
            RuleFor(tb_Prod => tb_Prod.Unit_ID).Must(CheckForeignKeyValue).WithMessage("单位:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.Type_ID).Must(CheckForeignKeyValue).WithMessage("产品类型:下拉选择值不正确。");
            RuleFor(x => x.Location_ID).NotNull().WithMessage("默认仓库不能为空。");
            RuleFor(tb_Prod => tb_Prod.Location_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认仓库:下拉选择值不正确。");
        }

        private bool UniqueName(string name)
        {
            // tb_ProductController pc = new tb_ProductController();
            bool rs = _controller.NameIsExists(name);
            return !rs;
            //tb_Product _db = new tb_Product();
            //var category = _db..Where(x => x.Name.ToLower() == name.ToLower()).SingleOrDefault();
            //if (category == null) return true;
            // return false;
        }
        private bool UniqueProductNo(string ProductNo)
        {
            bool rs = _controller.ProductNoIsExists(ProductNo);
            return !rs;
        }

        public void InitHelpInfo()
        {

        }
    }




}