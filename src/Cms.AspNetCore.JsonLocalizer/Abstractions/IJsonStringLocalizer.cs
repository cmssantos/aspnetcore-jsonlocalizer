namespace Cms.AspNetCore.JsonLocalizer.Abstractions;

/// <summary>
/// Defines a contract for retrieving localized strings from JSON files.
/// Supports simple and nested keys with or without format arguments.
/// </summary>
public interface IJsonStringLocalizer
{
    /// <summary>
    /// Gets the localized string associated with the specified key.
    /// </summary>
    /// <param name="key">
    /// The key of the localized string.
    /// Supports nested keys using dot notation, e.g., "errors.notFound.user".
    /// </param>
    /// <returns>
    /// The localized string if found; otherwise, the key itself.
    /// </returns>
    string this[string key] { get; }

    /// <summary>
    /// Gets the localized string associated with the specified key,
    /// formatted using the provided arguments.
    /// </summary>
    /// <param name="key">
    /// The key of the localized string.
    /// Supports nested keys using dot notation, e.g., "validation.required".
    /// </param>
    /// <param name="arguments">
    /// Optional arguments to format the localized string using <c>string.Format</c>.
    /// </param>
    /// <returns>
    /// The formatted localized string if found; otherwise, the key with arguments.
    /// </returns>
    string this[string key, params object[] arguments] { get; }
}
