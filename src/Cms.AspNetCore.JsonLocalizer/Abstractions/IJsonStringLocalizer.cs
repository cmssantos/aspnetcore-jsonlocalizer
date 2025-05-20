namespace Cms.AspNetCore.JsonLocalizer.Abstractions;

/// <summary>
/// Interface for JSON-based string localization services.
/// Provides methods to retrieve localized strings from JSON resources.
/// </summary>
public interface IJsonStringLocalizer
{
    /// <summary>
    /// Gets the localized string for the specified key.
    /// </summary>
    /// <param name="key">The key of the string resource.</param>
    /// <returns>
    /// The localized string. If the key is not found, returns the key itself or a default value
    /// depending on implementation.
    /// </returns>
    string this[string key] { get; }

    /// <summary>
    /// Gets the localized string for the specified key and formats it with the supplied arguments.
    /// </summary>
    /// <param name="key">The key of the string resource.</param>
    /// <param name="arguments">The arguments to format the string with.</param>
    /// <returns>
    /// The formatted localized string. If the key is not found, returns the key itself or a default value
    /// depending on implementation.
    /// </returns>
    /// <remarks>
    /// This method uses composite formatting similar to <see cref="string.Format(string, object[])"/>.
    /// </remarks>
    string this[string key, params object[] arguments] { get; }
}
