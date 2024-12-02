using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using InfoProtection.Controllers;
using InfoProtection.Models;
using InfoProtection.Servises;
using InfoProtection.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace InfoProtection.Tests.Tests
{
    public class MyEncryptionsControllerTests
    {
        [Fact]
        public void DownloadPdf_ValidId_ReturnsPdfFile()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContext();

            context.Users.Add(new User
            {
                Id = 1,
                Username = "testuser",
                PasswordHash = "hash",
                Salt = "salt",
                Role = "Admin",
                Email = "test@example.com"
            });


            context.EncryptedMessages.Add(new EncryptedMessage
            {
                Id = 1,
                UserId = 1,
                Algorithm = "AES",
                OriginalText = "Original Text",
                EncryptedText = "Encrypted Text",
                EncryptionDate = DateTime.Now,
            });
            context.SaveChanges();

            var controller = new EncryptionController(context);

            // Act
            var result = controller.DownloadPdf(1);

            // Assert
            Assert.IsType<FileContentResult>(result);
            var fileResult = result as FileContentResult;
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal("EncryptedInfo.pdf", fileResult.FileDownloadName);
        }


        //[Fact]
        //public void DownloadPdf_InvalidId_ReturnsNotFound()
        //{
        //    var context = TestDbContextFactory.CreateInMemoryDbContext();

        //    context.Users.Add(new User
        //    {
        //        Id = 1,
        //        Username = "testuser",
        //        PasswordHash = "hash",
        //        Salt = "salt",
        //        Role = "Admin",
        //        Email = "test@example.com"
        //    });


        //    context.EncryptedMessages.Add(new EncryptedMessage
        //    {
        //        Id = 1,
        //        UserId = 1,
        //        Algorithm = "AES",
        //        OriginalText = "Original Text",
        //        EncryptedText = "Encrypted Text",
        //        EncryptionDate = DateTime.Now,
        //    });
        //    context.SaveChanges();

        //    var controller = new EncryptionController(context);

        //    // Act
        //    var result = controller.DownloadPdf(999);

        //    // Assert
        //    Assert.IsType<NotFoundObjectResult>(result);
        //}
    }
}
