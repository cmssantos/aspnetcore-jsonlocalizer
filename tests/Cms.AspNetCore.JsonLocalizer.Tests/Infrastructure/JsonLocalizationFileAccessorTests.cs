using Cms.AspNetCore.JsonLocalizer.Infrastructure;
using Cms.AspNetCore.JsonLocalizer.Tests.Fixtures;
using System.Globalization;

namespace Cms.AspNetCore.JsonLocalizer.Tests.Infrastructure;

[Collection("Localization collection")]
public class JsonLocalizationFileAccessorTests(LocalizationTestFixture fixture)
{
    private readonly JsonLocalizationFileAccessor _accessor = new(fixture.ResourcesPath);

    [Fact]
    public void GetValue_ReturnsValue_ForSimpleKey()
    {
        var value = _accessor.GetValue("common", "welcome", new CultureInfo("en-US"));
        Assert.Equal("Welcome, {0}!", value);
    }

    [Fact]
    public void GetValue_ReturnsValue_ForNestedKey()
    {
        var value = _accessor.GetValue("common", "error.notFound", new CultureInfo("en-US"));
        Assert.Equal("Resource not found", value);
    }

    [Fact]
    public void GetValue_ReturnsValue_ForDeepNestedKey()
    {
        var value = _accessor.GetValue("common", "error.validation.required", new CultureInfo("en-US"));
        Assert.Equal("Required field: {0}", value);
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenFileDoesNotExist()
    {
        var value = _accessor.GetValue("common", "any.key", new CultureInfo("xx-XX"));
        Assert.Null(value);
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenKeyDoesNotExist()
    {
        var value = _accessor.GetValue("common", "error.nonexistent", new CultureInfo("en-US"));
        Assert.Null(value);
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenPathContinuesAfterLeafValue()
    {
        // "welcome" é uma string, então tentar acessar "welcome.anything" deve falhar
        var value = _accessor.GetValue("common", "welcome.anything", new CultureInfo("en-US"));

        Assert.Null(value);
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenPropertyMissingWithinJsonObject()
    {
        var value = _accessor.GetValue("common", "level1.missing", new CultureInfo("en-US"));
        Assert.Null(value);
    }
}
