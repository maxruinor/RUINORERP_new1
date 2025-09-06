
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/06/2025 15:41:56
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
    /// 菜单程序集信息表
    /// </summary>
    public partial class tb_MenuInfoServices : BaseServices<tb_MenuInfo>, Itb_MenuInfoServices
    {
        IMapper _mapper;
        public tb_MenuInfoServices(IMapper mapper, IBaseRepository<tb_MenuInfo> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}