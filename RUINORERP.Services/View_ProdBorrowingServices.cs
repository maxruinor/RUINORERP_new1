
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/27/2024 19:26:27
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
    /// 借出单统计
    /// </summary>
    public partial class View_ProdBorrowingServices : BaseServices<View_ProdBorrowing>, IView_ProdBorrowingServices
    {
        IMapper _mapper;
        public View_ProdBorrowingServices(IMapper mapper, IBaseRepository<View_ProdBorrowing> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}