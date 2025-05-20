using Cms.AspNetCore.JsonLocalizer.Infrastructure;
using Cms.AspNetCore.JsonLocalizer.Tests.Fixtures;

namespace Cms.AspNetCore.JsonLocalizer.Tests.Infrastructure;

[Collection("Localization collection")]
public class JsonStringLocalizerTests(LocalizationTestFixture fixture)
{
    private readonly JsonLocalizationFileAccessor _accessor = new(fixture.ResourcesPath);

    [Fact]
    public void Indexer_ReturnsFormattedString_WhenArgumentsProvided()
    {
        var localizer = new JsonStringLocalizer(_accessor, "en-US");
        var result = localizer["welcome", "John"];

        Assert.Equal("Welcome, John!", result);
    }

    [Fact]
    public void Indexer_ReturnsKey_WhenTemplateNotFound()
    {
        var localizer = new JsonStringLocalizer(_accessor, "en-US");
        var result = localizer["nonexistent.key"];

        Assert.Equal("nonexistent.key", result);
    }

    [Fact]
    public void Indexer_ReturnsFormattedKey_WhenTemplateNotFoundWithArgs()
    {
        var localizer = new JsonStringLocalizer(_accessor, "en-US");
        var result = localizer["nonexistent.key", 123];

        Assert.Equal("nonexistent.key", result);
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenPropertyMissingWithinJsonObject()
    {
        // level1 existe, mas dentro dele "missing" não existe.
        var value = _accessor.GetValue("level1.missing", "en-US");
        Assert.Null(value);
    }
}
