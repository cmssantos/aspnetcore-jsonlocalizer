using System.Text;

namespace Cms.AspNetCore.JsonLocalizer.Tests.Fixtures;

public class LocalizationTestFixture : IDisposable
{
    public string ResourcesPath { get; }
    private bool _disposed;

    public LocalizationTestFixture()
    {
        ResourcesPath = Path.Combine(AppContext.BaseDirectory, "TestResources");

        Directory.CreateDirectory(ResourcesPath);

        // Arquivo com formato {domain}.{cultureName}.json
        var domainFilePath = Path.Combine(ResourcesPath, "common.en-US.json");
        if (!File.Exists(domainFilePath))
        {
            File.WriteAllText(domainFilePath, """
            {
              "error": {
                "notFound": "Resource not found",
                "validation": {
                  "required": "Required field: {0}"
                }
              },
              "welcome": "Welcome, {0}!"
            }
            """, Encoding.UTF8);
        }

        // Arquivo com formato {cultureName}.json (sem domínio)
        var cultureFilePath = Path.Combine(ResourcesPath, "fr-FR.json");
        if (!File.Exists(cultureFilePath))
        {
            File.WriteAllText(cultureFilePath, """
            {
              "greeting": "Bonjour, {0}!",
              "farewell": "Au revoir!",
              "error": {
                "generic": "Une erreur s'est produite"
              }
            }
            """, Encoding.UTF8);
        }

        // Arquivo que demonstra a precedência do domínio
        var domainOverrideFilePath = Path.Combine(ResourcesPath, "common.fr-FR.json");
        if (!File.Exists(domainOverrideFilePath))
        {
            File.WriteAllText(domainOverrideFilePath, """
            {
              "greeting": "Salut, {0}!",
              "welcome": "Bienvenue, {0}!"
            }
            """, Encoding.UTF8);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Limpar arquivos temporários se necessário
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    ~LocalizationTestFixture()
    {
        Dispose(disposing: false);
    }
}
