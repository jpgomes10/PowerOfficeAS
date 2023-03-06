using API.DTOs;
using API.Models.OrganizationInfos;

namespace API.Services
{
    public interface IOrganizationService
    {
        Task<List<OrganizationInfoDto>> GetOrganizationsFromFile(IFormFile filePath);
        List<OrganizationStatusDto> GetOrganizationsStatusFromFile(IFormFile filePath);
    }
}
