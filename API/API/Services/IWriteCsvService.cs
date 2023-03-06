using API.DTOs;

namespace API.Services
{
    public interface IWriteCsvService
    {
        byte[] WriteOrgsToCsv(List<OrganizationInfoDto> orgDtos);
    }
}
