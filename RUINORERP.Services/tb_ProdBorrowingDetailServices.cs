﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/07/2025 13:17:57
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
    /// 借出单明细
    /// </summary>
    public partial class tb_ProdBorrowingDetailServices : BaseServices<tb_ProdBorrowingDetail>, Itb_ProdBorrowingDetailServices
    {
        IMapper _mapper;
        public tb_ProdBorrowingDetailServices(IMapper mapper, IBaseRepository<tb_ProdBorrowingDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}