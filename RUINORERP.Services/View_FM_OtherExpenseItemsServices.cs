
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/27/2024 19:36:49
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
    /// 其它费用统计分析
    /// </summary>
    public partial class View_FM_OtherExpenseItemsServices : BaseServices<View_FM_OtherExpenseItems>, IView_FM_OtherExpenseItemsServices
    {
        IMapper _mapper;
        public View_FM_OtherExpenseItemsServices(IMapper mapper, IBaseRepository<View_FM_OtherExpenseItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}