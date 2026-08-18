using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WebApplication6.Backend.Controllers;
using Xunit;

namespace WebApplication6.Tests.Tests.Integration.Controllers;

[TestSubject(typeof(ImageController))]
public class ImageControllerTest(TestDatabaseFixture dbFixture) : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture { get; } = dbFixture;

    [Fact]
    public async Task GetAllImages_TwoImages_ReturnsExpectedValue()
    {
        await using var context = Fixture.CreateContext();
        var controller = new ImageController(context);


        var imageEnumerable = (await controller.GetAllImages()).Value;

        var images = imageEnumerable.ToList();
        Assert.Equal(2, images.Count);
        Assert.Equal(1, images[0].Id);
        Assert.Equal(3, images[1].Id);
    }
    
    [Fact]
    public async Task GetImageById_ImageIdIs1_ReturnsExpectedValue()
    {
        await using var context = Fixture.CreateContext();
        var controller = new ImageController(context);


        var image = (await controller.GetImageById(1)).Value;
        
        // FileName = "image1", ContentType = "image/png",
        // FileSize = 617238, StorageFileName = "a7344381-fe36-4139-be80-5c61cb7fe8af.png",
        // AltText = "alt text", Height = 771, Width = 646
        Assert.Equal("image1", image.FileName);
        Assert.Equal("image/png", image.ContentType);
        Assert.Equal(617238, image.FileSize);
        Assert.Equal("a7344381-fe36-4139-be80-5c61cb7fe8af.png", image.StorageFileName);
        Assert.Equal("alt text", image.AltText);
        Assert.Equal(771, image.Height);
        Assert.Equal(646, image.Width);
    }
    
}