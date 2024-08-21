using AutoMapper;
using RUINORERP.Model;
namespace RUINORERP.Model.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<tb_SupplierDto, tb_Supplier>();
            //CreateMap<tb_Unit, tb_UnitDto>();
            //意思是转换时为空则给默认值?
            CreateMap<tb_Supplier, tb_SupplierDto>().ForMember(destination => destination.Contact, opt => opt.NullSubstitute("缺少值名字")); ;

        }
    }
}
