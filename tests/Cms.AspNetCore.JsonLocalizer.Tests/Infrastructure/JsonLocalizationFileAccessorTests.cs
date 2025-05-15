using Cms.AspNetCore.JsonLocalizer.Infrastructure;
using Cms.AspNetCore.JsonLocalizer.Tests.Fixtures;

namespace Cms.AspNetCore.JsonLocalizer.Tests.Infrastructure;

[Collection("Localization collection")]
public class JsonLocalizationFileAccessorTests(LocalizationTestFixture fixture)
{
    private readonly JsonLocalizationFileAccessor _accessor = new(fixture.ResourcesPath);

    [Fact]
    public void GetValue_ReturnsValue_ForSimpleKey()
    {
        var value = _accessor.GetValue("welcome", "en-US");
        Assert.Equal("Welcome, {0}!", value);
    }

    [Fact]
    public void GetValue_ReturnsValue_ForNestedKey()
    {
        var value = _accessor.GetValue("error.notFound", "en-US");
        Assert.Equal("Resource not found", value);
    }

    [Fact]
    public void GetValue_ReturnsValue_ForDeepNestedKey()
    {
        var value = _accessor.GetValue("error.validation.required", "en-US");
        Assert.Equal("Required field: {0}", value);
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenFileDoesNotExist()
    {
        var value = _accessor.GetValue("any.key", "xx-XX");
        Assert.Null(value);
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenKeyDoesNotExist()
    {
        var value = _accessor.GetValue("error.nonexistent", "en-US");
        Assert.Null(value);
    }
}
