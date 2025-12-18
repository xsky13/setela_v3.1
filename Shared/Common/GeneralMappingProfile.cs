using AutoMapper;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Common.DTO;

namespace SetelaServerV3._1.Shared.Common
{
    public class GeneralMappingProfile : Profile
    {
        public GeneralMappingProfile()
        {
            CreateMap<SysUser, UserSimpleDTO>();
            CreateMap<Resource, CourseResourceDTO>();
        }
    }
}
