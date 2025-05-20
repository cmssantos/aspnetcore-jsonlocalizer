namespace Cms.AspNetCore.JsonLocalizer.Options;

/// <summary>
/// Configuration options for JSON-based localization services.
/// </summary>
public class JsonLocalizationOptions
{
    /// <summary>
    /// Gets or sets the relative path to the directory containing localization JSON files.
    /// </summary>
    /// <remarks>
    /// The path is relative to the application's content root path.
    /// </remarks>
    /// <value>
    /// The default value is "Resources".
    /// </value>
    public string ResourcesPath { get; set; } = "Resources";

    /// <summary>
    /// Gets or sets the default culture to use when a specific culture is not available.
    /// </summary>
    /// <remarks>
    /// This culture will be used as a fallback when the requested culture doesn't have
    /// a corresponding resource file or when a specific string key is not found in the
    /// current culture's resource file.
    /// </remarks>
    /// <value>
    /// The default value is "en-US". Can be null if no default culture should be used.
    /// </value>
    public string? DefaultCulture { get; set; } = "en-US";
}
