using API.DTOs;

namespace API.Services
{
    public interface IReadCsvService
    {
        List<OrganizationDto> ReadOrgsFromCsv(IFormFile csvFilePath);
        List<OrganizationInfoDto> ReadOrgsInfoFromCsv(IFormFile csvFilePath);
    }
}
