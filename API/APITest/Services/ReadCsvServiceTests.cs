using API.Services;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITest.Services
{
    [TestFixture]
    public class ReadCsvServiceTests
    {
        private ReadCsvService _readCsvService;

        [SetUp]
        public void Setup()
        {
            _readCsvService = new ReadCsvService();
        }

        [Test]
        public void ReadOrgsFromCsv_Should_Return_List_Of_OrganizationDto_When_Valid_Csv_File_Is_Provided()
        {
            var csvData = "OrgNo;Name\n123;Test Org\n456;Another Org";
            var csvFile = CreateCsvFile(csvData);

            var result = _readCsvService.ReadOrgsFromCsv(csvFile);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("123", result[0].OrgNo);
            Assert.AreEqual("Test Org", result[0].Name);
            Assert.AreEqual("456", result[1].OrgNo);
            Assert.AreEqual("Another Org", result[1].Name);
        }

        [Test]
        public void ReadOrgsFromCsv_Should_Throw_FormatException_When_Invalid_Csv_File_Is_Provided()
        {
            var csvData = "OrgNo;Name;ExtraColumn\n123;Test Org;Extra Data";
            var csvFile = CreateCsvFile(csvData);

            Assert.Throws<FormatException>(() => _readCsvService.ReadOrgsFromCsv(csvFile));
        }

        [Test]
        public void ReadOrgsInfoFromCsv_Should_Return_List_Of_OrganizationInfoDto_When_Valid_Csv_File_Is_Provided()
        {
            var csvData = "OrgNo;Name;AntallAnsatte;Naeringskode;Organisasjonsform;BrregNavn;Status\n123;Test Org;10;1234;AS;Test Company;Active\n456;Another Org;20;5678;ENK;Another Company;Inactive";
            var csvFile = CreateCsvFile(csvData);

            var result = _readCsvService.ReadOrgsInfoFromCsv(csvFile);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("123", result[0].OrgNo);
            Assert.AreEqual("Test Org", result[0].Name);
            Assert.AreEqual(10, result[0].AntallAnsatte);
            Assert.AreEqual("1234", result[0].Naeringskode);
            Assert.AreEqual("AS", result[0].Organisasjonsform);
            Assert.AreEqual("Test Company", result[0].BrregNavn);
            Assert.AreEqual("Active", result[0].Status);
            Assert.AreEqual("456", result[1].OrgNo);
            Assert.AreEqual("Another Org", result[1].Name);
            Assert.AreEqual(20, result[1].AntallAnsatte);
            Assert.AreEqual("5678", result[1].Naeringskode);
            Assert.AreEqual("ENK", result[1].Organisasjonsform);
            Assert.AreEqual("Another Company", result[1].BrregNavn);
            Assert.AreEqual("Inactive", result[1].Status);
        }
        [Test]
        public void ReadOrgsInfoFromCsv_Should_Throw_FormatException_When_Invalid_Csv_File_Is_Provided()
        {
            var csvData = "OrgNo;Name;ExtraColumn\n123;Test Org;Extra Data";
            var csvFile = CreateCsvFile(csvData);

            Assert.Throws<FormatException>(() => _readCsvService.ReadOrgsInfoFromCsv(csvFile));
        }

        private IFormFile CreateCsvFile(string csvData)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(csvData);
            writer.Flush();
            stream.Position = 0;

            var file = new FormFile(stream, 0, stream.Length, "data", "test.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };

            return file;
        }
    }
}
