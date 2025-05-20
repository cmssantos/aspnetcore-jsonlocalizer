using Cms.AspNetCore.JsonLocalizer.Abstractions;
using Cms.AspNetCore.JsonLocalizer.Extensions;
using Cms.AspNetCore.JsonLocalizer.Infrastructure;
using Cms.AspNetCore.JsonLocalizer.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cms.AspNetCore.JsonLocalizer.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddJsonLocalization_RegistersDependenciesCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        var context = new DefaultHttpContext();
        context.Request.Headers["Accept-Language"] = "en-US";

        services.AddSingleton<IHttpContextAccessor>(_ => new HttpContextAccessor { HttpContext = context });

        services.AddJsonLocalization(options =>
        {
            options.ResourcesPath = "TestResources";
            options.DefaultCulture = "pt-BR";
        });

        ServiceProvider provider = services.BuildServiceProvider();

        // Act - Use GetJsonLocalizer extension method instead
        IJsonStringLocalizer localizer = provider.GetJsonLocalizer("common");

        // Assert
        Assert.NotNull(localizer);
        Assert.IsType<JsonStringLocalizer>(localizer);
    }

    [Fact]
    public void AddJsonLocalization_AcceptsNullSetup()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act - This should not throw an exception
        services.AddJsonLocalization(null);

        // Assert
        ServiceProvider provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetRequiredService<JsonLocalizationFileAccessor>());
        Assert.NotNull(provider.GetRequiredService<JsonLocalizationOptions>());
    }
}
