using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using InfoProtection.Controllers;
using InfoProtection.Models;
using InfoProtection.Servises;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace InfoProtection.Tests
{
    public class MyEncryptionsControllerTests
    {
        private readonly HttpClient _client;
        private Mock<PdfSignatureService> _pdfServiceMock;
        private readonly WebApplicationFactory<Program> _factory;

        public MyEncryptionsControllerTests()
        {
            // Настраиваем тестовое окружение
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Настраиваем тестовую базу данных
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseNpgsql("Host=localhost;Port=5432;Database=InfProtec;Username=postgres;Password=vladimir"));

                        // Добавляем Mock PdfSignatureService
                        _pdfServiceMock = new Mock<PdfSignatureService>();
                        services.AddScoped(_ => _pdfServiceMock.Object);
                    });
                });

            _client = factory.CreateClient();
        }

        [Fact]
        public void DownloadPdf_ValidId_ReturnsPdfFile()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<EncryptedMessage>>();
            var testData = new List<EncryptedMessage>
    {
        new EncryptedMessage
        {
            Id = 1,
            UserId = 3,
            Algorithm = "AES",
            OriginalText = "Hello",
            EncryptedText = "Encrypted",
            EncryptionDate = DateTime.Now
        }
    }.AsQueryable();

            mockDbSet.As<IQueryable<EncryptedMessage>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<EncryptedMessage>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<EncryptedMessage>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<EncryptedMessage>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.EncryptedMessages).Returns(mockDbSet.Object);

            var controller = new EncryptionController(mockContext.Object);

            // Act
            var result = controller.DownloadPdf(1) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("application/pdf", result.ContentType);
            Assert.Equal("EncryptedInfo.pdf", result.FileDownloadName);
        }



        [Fact]
        public async Task DownloadPdf_InvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/Myencryptions/DownloadPdf/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorMessage = await response.Content.ReadAsStringAsync();
            Assert.Equal("Шифр не найден или у вас нет доступа.", errorMessage);
        }
    }
}
