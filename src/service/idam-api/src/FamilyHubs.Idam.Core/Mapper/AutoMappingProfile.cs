using AutoMapper;
using FamilyHubs.Idam.Core.Models;
using FamilyHubs.Idam.Data.Entities;

namespace FamilyHubs.Idam.Core.Mapper
{
    public class AutoMappingProfile : Profile
    {

        public AutoMappingProfile()
        {
            CreateMap<UserSession, UserSessionDto>();
        }
    }
}
