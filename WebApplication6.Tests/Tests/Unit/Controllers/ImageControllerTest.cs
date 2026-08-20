// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using WebApplication6.Backend.Controllers;
// using WebApplication6.Backend.Models;
// using WebApplication6.Backend.Repositories;
// using WebApplication6.Backend.Services;
// using Xunit;
// using Xunit.Sdk;
//
// namespace WebApplication6.Tests.Tests.Unit.Controllers;
//
// public class ImageControllerTest
// {
//     [Fact]
//     public async Task GetAllImagesAsync_SingleImageList_ReturnsSameSingleImageInList()
//     {
//         // Arrange
//         var repositoryMock = new Mock<IImageRepository>();
//         var serviceMock = new Mock<IFileHostingService>();
//
//         var image = new Image
//         {
//             Id = 1,
//             FileName = "image1.jpg",
//             ContentType = "image/jpeg",
//             FileSize = 1000,
//             StorageFileName = "image1.jpg",
//             AltText = "alt text",
//             Width = 100,
//             Height = 100
//         };
//         
//         var imageList = new List<Image>
//         {
//             image
//         };
//         repositoryMock.Setup(r => r.GetAllImagesAsync())
//             .ReturnsAsync(imageList);
//         
//         var controller = new ImageController(repositoryMock.Object, serviceMock.Object);
//
//
//
//         // Act
//         var images = await controller.GetAllImages();
//         
//         // Assert
//         repositoryMock.Verify(r => r.GetAllImagesAsync());
//         Assert.Single(images.Value, image);
//     }
//     
//     [Fact]
//     public async Task PostImage_ExceptionInService_ReturnsObjectOfTypeObjectResult()
//     {
//         // Arrange
//         var repositoryMock = new Mock<IImageRepository>();
//         var serviceMock = new Mock<IFileHostingService>();
//
//         var mockFile = new Mock<IFormFile>();
//         // mockFile.Setup(f => f.ContentType).Returns("text/plain");
//         serviceMock.Setup(s => s.HostImageAsync(mockFile.Object)).Throws(new Exception());
//         
//         var controller = new ImageController(repositoryMock.Object, serviceMock.Object);
//
//         
//         // Act
//         var result = await controller.PostImage(mockFile.Object);
//         
//         // Assert
//         Assert.IsType<ObjectResult>(result);
//     }
//     
//     [Fact]
//     public async Task PostImage_ExceptionInRepository_ReturnsObjectOfTypeObjectResult()
//     {
//         // Arrange
//         var repositoryMock = new Mock<IImageRepository>();
//         var serviceMock = new Mock<IFileHostingService>();
//
//         var mockFile = new Mock<IFormFile>();
//         // mockFile.Setup(f => f.ContentType).Returns("text/plain");
//
//         var image = new Image();
//         
//         repositoryMock.Setup(s => s.SaveImageAsync(image)).Throws(new Exception());
//         
//         var controller = new ImageController(repositoryMock.Object, serviceMock.Object);
//
//         
//         // Act
//         var result = await controller.PostImage(mockFile.Object, "alt text");
//         
//         // Assert
//         Assert.IsType<ObjectResult>(result);
//     }
//     
// }