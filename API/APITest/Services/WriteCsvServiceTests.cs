using API.DTOs;
using API.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITest.Services
{
    [TestFixture]
    public class WriteCsvServiceTests
    {
        private WriteCsvService _writeCsvService;

        [SetUp]
        public void Setup()
        {
            _writeCsvService = new WriteCsvService();
        }

        [Test]
        public void WriteOrgsToCsv_Should_Return_Byte_Array_When_Valid_OrgDto_List_Is_Provided()
        {
            var orgDtos = new List<OrganizationInfoDto>
            {
                new OrganizationInfoDto
                {
                    OrgNo = "123",
                    Name = "Test Org",
                    AntallAnsatte = 10,
                    Naeringskode = "1234",
                    Organisasjonsform = "AS",
                    BrregNavn = "Test Company",
                    Status = "Active"
                },
                new OrganizationInfoDto
                {
                    OrgNo = "456",
                    Name = "Another Org",
                    AntallAnsatte = 20,
                    Naeringskode = "5678",
                    Organisasjonsform = "ENK",
                    BrregNavn = "Another Company",
                    Status = "Inactive"
                }
            };

            var result = _writeCsvService.WriteOrgsToCsv(orgDtos);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<byte[]>(result);
        }
        [Test]
        public void WriteOrgsToCsv_Should_Throw_ArgumentNullException_When_OrgDtos_Is_Null()
        {
            List<OrganizationInfoDto> orgDtos = null;

            Assert.Throws<NullReferenceException>(() => _writeCsvService.WriteOrgsToCsv(orgDtos));
        }
    }
}
