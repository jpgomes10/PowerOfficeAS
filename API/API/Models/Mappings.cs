using API.DTOs;
using API.Models.OrganizationInfos;
using API.Models.OrganizationInfos.ValueObjects;
using AutoMapper;

namespace API.Models
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<OrganizationDto, OrganizationInfo>();
            CreateMap<OrganizationInfo, OrganizationInfoDto>()
                .ForMember(dest => dest.Naeringskode, opt => opt.MapFrom(src => src.Naeringskode1.Kode))
                .ForMember(dest => dest.Organisasjonsform, opt => opt.MapFrom(src => src.Organisasjonsform.Kode))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));         
        }
        private static StatusEnum StatusEnumHelper(string status)
        {
            Enum.TryParse(status, out StatusEnum myStatus);
            return myStatus;
        }
    }
}
