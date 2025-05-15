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

        var filePath = Path.Combine(ResourcesPath, "en-US.json");

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, """
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
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources here, if any
            }

            // Dispose unmanaged resources here, if any
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
