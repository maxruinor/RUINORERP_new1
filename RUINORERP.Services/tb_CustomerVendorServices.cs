
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:57
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
    /// 客户厂商表 开票资料这种与财务有关另外开表
    /// </summary>
    public partial class tb_CustomerVendorServices : BaseServices<tb_CustomerVendor>, Itb_CustomerVendorServices
    {
        IMapper _mapper;
        public tb_CustomerVendorServices(IMapper mapper, IBaseRepository<tb_CustomerVendor> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}