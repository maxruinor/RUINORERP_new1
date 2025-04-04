﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/21/2025 14:35:41
// **************************************
using AutoMapper;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using System.Threading.Tasks;
using System;
﻿using SqlSugar;
using System.Collections.Generic;


namespace RUINORERP.Services
{
    /// <summary>
    /// 产品类别表 与行业相关的产品分类
    /// </summary>
    public partial class tb_ProdCategoriesServices : BaseServices<tb_ProdCategories>, Itb_ProdCategoriesServices
    {
        IMapper _mapper;
        public tb_ProdCategoriesServices(IMapper mapper, IBaseRepository<tb_ProdCategories> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}