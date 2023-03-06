using API.DTOs;

namespace API.Services
{
    public class ReadCsvService : IReadCsvService
    {

        /// <summary>
        /// Reads organizations from a CSV file and maps them to OrganizationDto objects.
        /// The CSV file must have headers "OrgNo" and "Name" in that order, with one row per organization.
        /// </summary>
        /// <param name="file">The CSV file to read from.</param>
        /// <returns>
        /// A list of OrganizationDto objects representing the organizations in the CSV file.
        /// If the CSV file is not in the correct format, a FormatException is thrown.
        /// </returns>
        public List<OrganizationDto> ReadOrgsFromCsv(IFormFile file)
        {
            List<OrganizationDto> orgDtos = new List<OrganizationDto>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                // Read headers from the CSV file
                string headerLine = reader.ReadLine();
                string[] headers = headerLine.Split(';');
                if (headers.Length != 2 || headers[0] != "OrgNo" || headers[1] != "Name")
                {
                    throw new FormatException("Invalid CSV file format");
                }

                // Loop through each line (starting from the second line) and create DTOs
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(';');
                    if (parts.Length != 2)
                    {
                        // Skip lines that don't have exactly two parts
                        continue;
                    }

                    // Map the parts to the DTO
                    OrganizationDto orgDto = new OrganizationDto
                    {
                        OrgNo = parts[0],
                        Name = parts[1]
                    };
                    orgDtos.Add(orgDto);
                }
            }

            return orgDtos;
        }

        /// <summary>
        /// Reads organization information from a CSV file and maps them to OrganizationInfoDto objects.
        /// The CSV file must have headers "OrgNo", "Name", "AntallAnsatte", "Naeringskode", "Organisasjonsform", "BrregNavn", and "Status" in that order, with one row per organization.
        /// </summary>
        /// <param name="csvFile">The CSV file to read from.</param>
        /// <returns>
        /// A list of OrganizationInfoDto objects representing the organizations in the CSV file.
        /// If the CSV file is not in the correct format, a FormatException is thrown.
        /// </returns>
        public List<OrganizationInfoDto> ReadOrgsInfoFromCsv(IFormFile csvFile)
        {
            List<OrganizationInfoDto> orgDtos = new List<OrganizationInfoDto>();

            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            {
                // Read the first line to check for the headers
                string headerLine = reader.ReadLine();
                string[] headers = headerLine.Split(';');
                if (headers.Length != 6 || headers[0] != "OrgNo" || headers[1] != "Name" || headers[2] != "AntallAnsatte"
                    || headers[3] != "Naeringskode" || headers[4] != "Organisasjonsform" || headers[5] != "BrregNavn")
                {
                    throw new FormatException("Invalid CSV file format");
                }

                // Loop through each line and create DTOs
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(';');
                    if (parts.Length != 6)
                    {
                        // Skip lines that don't have exactly seven parts
                        continue;
                    }

                    // Map the parts to the DTO
                    OrganizationInfoDto orgDto = new OrganizationInfoDto
                    {
                        OrgNo = parts[0],
                        Name = parts[1],
                        AntallAnsatte = Int32.Parse(parts[2]),
                        Naeringskode = parts[3],
                        Organisasjonsform = parts[4],
                        BrregNavn = parts[5]
                    };
                    orgDtos.Add(orgDto);
                }
            }

            return orgDtos;
        }

    }
}
