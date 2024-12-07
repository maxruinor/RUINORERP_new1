
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/05/2024 23:44:20
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
    /// UI表格设置
    /// </summary>
    public partial class tb_UIGridSettingServices : BaseServices<tb_UIGridSetting>, Itb_UIGridSettingServices
    {
        IMapper _mapper;
        public tb_UIGridSettingServices(IMapper mapper, IBaseRepository<tb_UIGridSetting> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}