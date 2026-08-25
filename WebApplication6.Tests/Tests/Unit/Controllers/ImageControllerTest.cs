using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication6.Backend.Controllers;
using WebApplication6.Backend.Models;
using WebApplication6.Backend.Repositories;
using Xunit;

namespace WebApplication6.Tests.Tests.Unit.Controllers;

// TODO get rid of
public class ImageControllerTest
{
    /* 
     * TODO Likely not using this test; likely not using ImageController at any point. Fixed this test anyway though.
     *
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

        var imageList = new List<Image> { image };
        
        repositoryMock.Setup(r => r.GetAllImagesAsync())
            .ReturnsAsync(imageList);
        
        var controller = new ImageController(repositoryMock.Object);
        
        
        // Act
        var images = await controller.GetAllImages();
        
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(images.Result);
        var returnedImages = Assert.IsType<IEnumerable<Image>>(okResult.Value, exactMatch: false);
        var item = Assert.Single(returnedImages);
        Assert.Equal(image, item);
    }
    */
    
}