using Cms.AspNetCore.JsonLocalizer.Abstractions;
using Cms.AspNetCore.JsonLocalizer.Options;

namespace Cms.AspNetCore.JsonLocalizer.Infrastructure;

/// <summary>
/// Implementation of <see cref="IJsonStringLocalizer"/> that retrieves localized strings from JSON files.
/// </summary>
/// <remarks>
/// This class handles the retrieval and formatting of localized strings based on the specified culture
/// and domain (resource file). It supports fallback to a default culture when necessary.
/// </remarks>
public class JsonStringLocalizer(
    JsonLocalizationFileAccessor accessor,
    string domain,
    CultureInfo culture,
    JsonLocalizationOptions options) : IJsonStringLocalizer
{
    private readonly JsonLocalizationFileAccessor _accessor = accessor;
    private readonly string _domain = domain;
    private readonly CultureInfo _culture = culture;
    private readonly CultureInfo? _fallback = !string.IsNullOrWhiteSpace(options.DefaultCulture)
        ? new CultureInfo(options.DefaultCulture)
        : null;

    /// <inheritdoc/>
    public string this[string key] => Format(key, []);

    /// <inheritdoc/>
    public string this[string key, params object[] arguments] => Format(key, arguments);

    /// <summary>
    /// Formats the localized string with the provided arguments.
    /// </summary>
    /// <param name="key">The key of the string resource.</param>
    /// <param name="args">The arguments to format the string with.</param>
    /// <returns>
    /// The formatted localized string. If the key is not found, returns the key itself.
    /// </returns>
    /// <remarks>
    /// This method first attempts to retrieve the localized string using the current culture.
    /// If not found and a fallback culture is defined, it will try to retrieve the string using the fallback culture.
    /// The string is then formatted using the current culture's formatting rules.
    /// </remarks>
    private string Format(string key, object[] args)
    {
        var template = _accessor.GetValue(_domain, key, _culture, _fallback);
        return string.Format(CultureInfo.CurrentCulture, template ?? key, args);
    }
}
