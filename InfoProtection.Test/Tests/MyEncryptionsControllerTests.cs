using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InfoProtection.Models;
using InfoProtection.Servises;
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
                            options.UseNpgsql("TestDatabase"));

                        // Добавляем Mock PdfSignatureService
                        _pdfServiceMock = new Mock<PdfSignatureService>();
                        services.AddScoped(_ => _pdfServiceMock.Object);
                    });
                });

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task DownloadPdf_ValidId_ReturnsPdfFile()
        {
            // Arrange: Создаём тестовые данные
            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.EncryptedMessages.Add(new EncryptedMessage
                {
                    Id = 1,
                    UserId = "test-user-id",
                    Algorithm = "RSA",
                    OriginalText = "Test text",
                    EncryptedText = "Encrypted text",
                    EncryptionDate = DateTime.UtcNow
                });
                context.SaveChanges();
            }

            // Настраиваем Mock для возвращения данных
            _pdfServiceMock.Setup(s => s.CreateAndSignPdf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(new byte[] { 0x25, 0x50, 0x44, 0x46 }); // Заголовок PDF

            // Act
            var response = await _client.GetAsync("/Myencryptions/DownloadPdf/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/pdf", response.Content.Headers.ContentType.ToString());

            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0, "PDF файл пуст.");
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
