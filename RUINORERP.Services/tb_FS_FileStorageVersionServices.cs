
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/28/2025 17:14:17
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
    /// 文件版本表
    /// </summary>
    public partial class tb_FS_FileStorageVersionServices : BaseServices<tb_FS_FileStorageVersion>, Itb_FS_FileStorageVersionServices
    {
        IMapper _mapper;
        public tb_FS_FileStorageVersionServices(IMapper mapper, IBaseRepository<tb_FS_FileStorageVersion> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}