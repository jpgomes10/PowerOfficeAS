using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IWriteCsvService _writeCsvService;
        public OrganizationController(IOrganizationService organizationInfoService, IWriteCsvService writeCsvService)
        {
            _organizationService = organizationInfoService;
            _writeCsvService = writeCsvService;
        }

        /// <summary>
        /// This method creates a new organization object by reading data from a CSV file that is uploaded by the user.
        /// </summary>
        /// <param name="file">Specifies the CSV file containing organization data to be uploaded.</param>
        /// <returns>
        ///     Returns an HTTP response indicating success or failure of the operation and a CSV file containing the created organizations.
        ///     Success: new generated File
        ///     Failure: BadRequest
        /// </returns>
        [HttpPost(Name = "organizations")]
        public async Task<IActionResult> PostOrganizationsAsync(IFormFile file)
        {
            try
            {
                var organizations = await _organizationService.GetOrganizationsFromFile(file);
                var fileBytes = _writeCsvService.WriteOrgsToCsv(organizations);
                var memoryStream = new MemoryStream(fileBytes);
                return File(memoryStream, "text/csv", "output.csv");
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
             
        }

        /// <summary>
        ///     This method retrieves the status of all organizations specified in a CSV file that is uploaded by the user.
        /// </summary>
        /// <param name="file">Specifies the CSV file containing the list of organizations to retrieve status for.</param>
        /// <returns>
        ///     Returns an HTTP response indicating success or failure of the operation and the status of the requested organizations.
        ///     Success: List of OrganizationStatusDto
        ///     Failure: BadRequest
        /// </returns>
        [HttpPost("stats")]
        public IActionResult PostOrganizationStats(IFormFile file)
        {
            try
            {
                var result = _organizationService.GetOrganizationsStatusFromFile(file);
                return Ok(result);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
