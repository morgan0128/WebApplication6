using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using WebApplication6.Backend.Controllers;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using Xunit;
using ImageController = WebApplication6.Backend.Controllers.ImageController;

namespace WebApplication6.Tests.Tests.Unit.Controllers;

public class ImageControllerTest
{
    [Fact]
    public async Task GetAllImagesAsync_SingleImageList_ReturnsSameSingleImageInList()
    {
        // Arrange
        var repositoryMock = new Mock<IImageRepository>();

        var image = new Image
        {
            Id = 1,
            FileName = "image1.jpg",
            ContentType = "image/jpeg",
            FileSize = 1000,
            StorageFileName = "image1.jpg",
            AltText = "alt text",
            Width = 100,
            Height = 100
        };
        
        var imageList = new List<Image>
        {
            image
        };
        repositoryMock.Setup(r => r.GetAllImagesAsync())
            .ReturnsAsync(imageList);
        
        var controller = new ImageController(repositoryMock.Object);



        // Act
        var images = await controller.GetAllImages();
        
        // Assert
        repositoryMock.Verify(r => r.GetAllImagesAsync());
        Assert.Single(images.Value, image);
    }
    
}