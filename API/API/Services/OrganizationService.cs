using API.DTOs;
using API.Models.OrganizationInfos.ValueObjects;
using AutoMapper;

namespace API.Services
{
    public class OrganizationService: IOrganizationService
    {
        private readonly IBrregClient _brregClient;
        private readonly IMapper _mapper;
        private readonly IReadCsvService _readCsvService;


        public OrganizationService(IBrregClient brregClient, IMapper mapper, IReadCsvService readCsvService)
        {
            _brregClient = brregClient;
            _mapper = mapper;
            _readCsvService = readCsvService;
        }

        /// <summary>
        /// This method retrieves a list of OrganizationInfoDto objects from a CSV file uploaded by the user, and maps the data to the corresponding
        /// organizations obtained from the BrregClient. If the organization exists in the BrregClient, its properties are mapped to the OrganizationInfoDto
        /// and the resulting DTO is added to a list which is returned to the caller.
        /// </summary>
        /// <param name="filePath">The CSV file containing the organization data.</param>
        /// <returns>
        ///     Success: Returns a list of OrganizationInfoDto objects mapped from the CSV data and corresponding organizations from the BrregClient.
        ///     Failure: Throws an exception if the CSV file could not be read, or if there was an error retrieving an organization from the BrregClient.
        /// </returns>
        public async Task<List<OrganizationInfoDto>> GetOrganizationsFromFile(IFormFile filePath)
        {
            var organizations = new List<OrganizationInfoDto>(); 

            var records = _readCsvService.ReadOrgsFromCsv(filePath);
            foreach(var record in records)
            {
                var result = await _brregClient.GetOrganization(record.OrgNo);
                if(result != null)
                {
                    _mapper.Map(record, result);
                    organizations.Add(_mapper.Map<OrganizationInfoDto>(result));
                }
            }

            return organizations;
        }

        /// <summary>
        /// This method retrieves a list of OrganizationStatusDto objects from a CSV file uploaded by the user/could be automatic from the frontend, 
        /// and groups the organizations by their Organisasjonsform property. It returns a list of DTOs with the number of organizations in each group, 
        /// as well as the percentage that group represents in relation to the total number of organizations in the CSV file.
        /// </summary>
        /// <param name="filePath">The CSV file containing the organization data.</param>
        /// <returns>
        ///     Success: Returns a list of OrganizationStatusDto objects with the count and percentage of organizations in each group.
        ///     Failure: Throws an exception if the CSV file could not be read.
        /// </returns>
        public List<OrganizationStatusDto> GetOrganizationsStatusFromFile(IFormFile filePath)
        {
            var records = _readCsvService.ReadOrgsInfoFromCsv(filePath);
            var groups = records.GroupBy(GetOrganizationGroup)
                                .ToDictionary(g => g.Key, g => g.Count());

            var dtos = new List<OrganizationStatusDto>();

            foreach (var group in groups)
            {
                var dto = new OrganizationStatusDto
                {
                    Organisasjonsform = group.Key,
                    Count = group.Value
                };

                dtos.Add(dto);
            }

            int totalCount = dtos.Sum(dto => dto.Count);

            foreach (var dto in dtos)
            {
                dto.Percentage = (int)Math.Round((double)dto.Count / totalCount * 100);
            }

            return dtos;
        }

        /// <summary>
        /// This method returns the group to which an organization belongs based on its Organisasjonsform property and AntallAnsatte property. 
        /// </summary>
        /// <param name="org">The organization for which to determine the group.</param>
        /// <returns>The group to which the organization belongs.</returns>
        private static string GetOrganizationGroup(OrganizationInfoDto org)
        {
            if (org.Organisasjonsform == OrganisasjonsformEnum.ENK.ToString())
            {
                return OrganisasjonsformEnum.ENK.ToString();
            }
            else if (org.Organisasjonsform == OrganisasjonsformEnum.AS.ToString())
            {
                if (org.AntallAnsatte <= 4)
                {
                    return "AS_0_4";
                }
                else if (org.AntallAnsatte <= 10)
                {
                    return "AS_5_10";
                }
                else
                {
                    return "AS_GT_10";
                }
            }
            else
            {
                return OrganisasjonsformEnum.ANDRE.ToString();
            }
        }

    }
}
