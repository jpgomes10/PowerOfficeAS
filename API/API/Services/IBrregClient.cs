using API.Models.OrganizationInfos;

namespace API.Services
{
    public interface IBrregClient
    {
        Task<OrganizationInfo> GetOrganization(string orgNo);
    }
}
