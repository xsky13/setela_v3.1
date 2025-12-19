using AutoMapper;
using SetelaServerV3._1.Application.Features.ModuleFeature.DTO;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature
{
    public class ModuleMappingProfile : Profile
    {
        public ModuleMappingProfile()
        {
            CreateMap<Module, ModuleDTO>();
        }
    }
}
