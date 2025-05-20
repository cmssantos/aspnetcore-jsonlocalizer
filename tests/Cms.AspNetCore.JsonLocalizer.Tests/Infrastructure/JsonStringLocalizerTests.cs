using Cms.AspNetCore.JsonLocalizer.Infrastructure;
using Cms.AspNetCore.JsonLocalizer.Tests.Fixtures;

namespace Cms.AspNetCore.JsonLocalizer.Tests.Infrastructure;

// Garante que os testes utilizem o fixture compartilhado de localização
[Collection("Localization collection")]
public class JsonStringLocalizerTests(LocalizationTestFixture fixture)
{
    private readonly JsonLocalizationFileAccessor _accessor = new(fixture.ResourcesPath);

    [Fact]
    public void Indexer_DeveRetornarStringFormatada_QuandoArgumentosForemFornecidos()
    {
        // Arrange
        var localizer = new JsonStringLocalizer(_accessor, "en-US");

        // Act
        var result = localizer["welcome", "John"];

        // Assert
        Assert.Equal("Welcome, John!", result);
    }

    [Fact]
    public void Indexer_DeveRetornarKey_QuandoTemplateNaoForEncontrado()
    {
        // Arrange
        var localizer = new JsonStringLocalizer(_accessor, "en-US");

        // Act
        var result = localizer["nonexistent.key"];

        // Assert
        Assert.Equal("nonexistent.key", result);
    }

    [Fact]
    public void Indexer_DeveRetornarKey_QuandoTemplateNaoForEncontradoComArgumentos()
    {
        // Arrange
        var localizer = new JsonStringLocalizer(_accessor, "en-US");

        // Act
        var result = localizer["nonexistent.key", 123];

        // Assert
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
