using AutoMapper;
using FamilyHubs.OpenReferral.Function.Models;
using FamilyHubs.SharedKernel.OpenReferral.Entities;
using Attribute = FamilyHubs.SharedKernel.OpenReferral.Entities.Attribute;

namespace FamilyHubs.OpenReferral.Function.Mappers;

public class ServiceDtoToServiceMapper : Profile
{
    public ServiceDtoToServiceMapper()
    {
        CreateMap<OrganizationDto, Organization>();
        CreateMap<OrganizationIdentifierDto, OrganizationIdentifier>();
        CreateMap<ServiceDto, Service>();
        CreateMap<ServiceAreaDto, ServiceArea>();
        CreateMap<ServiceAtLocationDto, ServiceAtLocation>();
        CreateMap<AccessibilityDto, Accessibility>();
        CreateMap<AddressDto, Address>();
        CreateMap<ContactDto, Contact>();
        CreateMap<PhoneDto, Phone>();
        CreateMap<ScheduleDto, Schedule>();
        CreateMap<AttributeDto, Attribute>();
        CreateMap<ProgramDto, SharedKernel.OpenReferral.Entities.Program>();
        CreateMap<RequiredDocumentDto, RequiredDocument>();
        CreateMap<ScheduleDto, Schedule>();
        CreateMap<CostOptionDto, CostOption>();
        CreateMap<FundingDto, Funding>();
        CreateMap<LanguageDto, Language>();
        CreateMap<LocationDto, Location>();
        CreateMap<TaxonomyDto, Taxonomy>();
        CreateMap<TaxonomyTermDto, TaxonomyTerm>();
        CreateMap<MetadataDto, Metadata>();
        
    }
}