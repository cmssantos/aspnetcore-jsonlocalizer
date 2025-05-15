namespace Cms.AspNetCore.JsonLocalizer.Options;

/// <summary>
/// Represents configuration options for the JSON localization system.
/// </summary>
public class JsonLocalizationOptions
{
    /// <summary>
    /// Gets or sets the relative path to the directory containing localization JSON files.
    /// Default is <c>"Resources"</c>.
    /// </summary>
    public string ResourcesPath { get; set; } = "Resources";

    /// <summary>
    /// Gets or sets the default culture used for localization fallback when no culture is specified.
    /// Default is <c>"en-US"</c>.
    /// </summary>
    public string? DefaultCulture { get; set; } = "en-US";
}
