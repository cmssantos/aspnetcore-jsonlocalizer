using Cms.AspNetCore.JsonLocalizer.Infrastructure;
using Cms.AspNetCore.JsonLocalizer.Tests.Fixtures;
using System.Globalization;

namespace Cms.AspNetCore.JsonLocalizer.Tests.Infrastructure;

[Collection("Localization collection")]
public class JsonLocalizationCultureFileTests(LocalizationTestFixture fixture)
{
    private readonly JsonLocalizationFileAccessor _accessor = new(fixture.ResourcesPath);

    [Fact]
    public void GetValue_ReturnsValue_FromCultureOnlyFile()
    {
        // Act - Usando um domínio que não existe como arquivo específico
        var value = _accessor.GetValue("nonexistent-domain", "greeting", new CultureInfo("fr-FR"));

        // Assert - Deve encontrar no arquivo fr-FR.json
        Assert.Equal("Bonjour, {0}!", value);
    }

    [Fact]
    public void GetValue_ReturnsValue_FromCultureOnlyFile_WithNestedKey()
    {
        // Act
        var value = _accessor.GetValue("nonexistent-domain", "error.generic", new CultureInfo("fr-FR"));

        // Assert
        Assert.Equal("Une erreur s'est produite", value);
    }

    [Fact]
    public void GetValue_PrefersSpecificDomainFile_OverCultureOnlyFile()
    {
        // Act - Usando o domínio "common" que existe tanto como arquivo específico quanto no arquivo de cultura
        var value = _accessor.GetValue("common", "greeting", new CultureInfo("fr-FR"));

        // Assert - Deve usar o valor do arquivo common.fr-FR.json, não do fr-FR.json
        Assert.Equal("Salut, {0}!", value);
    }

    [Fact]
    public void GetValue_UsesCultureOnlyFile_WhenKeyNotFoundInDomainFile()
    {
        // Act - "farewell" existe apenas no arquivo fr-FR.json, não no common.fr-FR.json
        var value = _accessor.GetValue("common", "farewell", new CultureInfo("fr-FR"));

        // Assert - Deve encontrar no arquivo fr-FR.json como fallback
        Assert.Equal("Au revoir!", value);
    }
}
