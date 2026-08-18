using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace WebApplication6.Tests;

public class TestWebHostFixture
{
    public TestWebHostFixture()
    {
    }

    public IWebHostEnvironment CreateHost()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Environment.ContentRootPath = "TestImages";
        return builder.Environment;
    }
}