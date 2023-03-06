using API.DTOs;
using API.Utils;

namespace API.Services
{
    /// <summary>
    /// Writes a list of OrganizationInfoDto objects to a CSV file.
    /// </summary>
    /// <param name="orgDtos">The list of OrganizationInfoDto objects to write to the CSV file.</param>
    /// <returns>
    /// A byte array representing the contents of the CSV file.
    /// If the CSV file cannot be written, returns null.
    /// </returns>
    public class WriteCsvService: IWriteCsvService
    {
        private readonly string _directoryName = "Output";
        private readonly string _fileExtension = "csv";
        public byte[] WriteOrgsToCsv(List<OrganizationInfoDto> orgDtos)
        {
            string outputFilePath = DirectoryUtil.CreateOutputFileWithTimestamp(_directoryName, _fileExtension);
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine("OrgNo;Name;AntallAnsatte;Naeringskode;Organisasjonsform;BrregNavn");

                foreach (OrganizationInfoDto orgDto in orgDtos)
                {
                    writer.WriteLine($"{orgDto.OrgNo};{orgDto.Name};{orgDto.AntallAnsatte};{orgDto.Naeringskode};{orgDto.Organisasjonsform};{orgDto.BrregNavn}");
                }
            }
            return File.ReadAllBytes(outputFilePath);
        }
    }
}
