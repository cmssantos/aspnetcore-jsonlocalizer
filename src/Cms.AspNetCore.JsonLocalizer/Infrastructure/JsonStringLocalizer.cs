using System.Globalization;
using Cms.AspNetCore.JsonLocalizer.Abstractions;

namespace Cms.AspNetCore.JsonLocalizer.Infrastructure;

/// <summary>
/// Default implementation of <see cref="IJsonStringLocalizer"/> that retrieves
/// localized strings from JSON resources using a specified culture.
/// </summary>
/// <param name="accessor">The accessor responsible for loading and reading JSON localization files.</param>
/// <param name="culture">The culture used to resolve the localized strings.</param>
public class JsonStringLocalizer(JsonLocalizationFileAccessor accessor, string culture) : IJsonStringLocalizer
{
    private readonly string _culture = culture;
    private readonly JsonLocalizationFileAccessor _accessor = accessor;

    /// <inheritdoc />
    public string this[string key] => Format(key, []);

    /// <inheritdoc />
    public string this[string key, params object[] arguments] => Format(key, arguments);

    private string Format(string key, object[] args)
    {
        var template = _accessor.GetValue(key, _culture);
        return string.Format(CultureInfo.CurrentCulture, template ?? key, args);
    }
}
