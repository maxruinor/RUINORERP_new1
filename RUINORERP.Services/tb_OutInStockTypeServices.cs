
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2024 14:01:32
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
    /// 出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？
    /// </summary>
    public partial class tb_OutInStockTypeServices : BaseServices<tb_OutInStockType>, Itb_OutInStockTypeServices
    {
        IMapper _mapper;
        public tb_OutInStockTypeServices(IMapper mapper, IBaseRepository<tb_OutInStockType> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}