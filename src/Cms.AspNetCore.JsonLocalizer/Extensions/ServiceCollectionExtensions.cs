using Cms.AspNetCore.JsonLocalizer.Abstractions;
using Cms.AspNetCore.JsonLocalizer.Infrastructure;
using Cms.AspNetCore.JsonLocalizer.Options;

namespace Cms.AspNetCore.JsonLocalizer.Extensions;

/// <summary>
/// Extension methods for setting up JSON-based localization services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds JSON-based localization services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="setup">An optional action to configure the <see cref="JsonLocalizationOptions"/>.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    /// <remarks>
    /// This method registers the required services for JSON localization:
    /// <list type="bullet">
    /// <item><description>JsonLocalizationOptions as a singleton</description></item>
    /// <item><description>JsonLocalizationFileAccessor as a singleton</description></item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddJsonLocalization(
        this IServiceCollection services,
        Action<JsonLocalizationOptions>? setup = null)
    {
        var options = new JsonLocalizationOptions();
        setup?.Invoke(options);

        services.AddSingleton(options);
        services.AddSingleton(new JsonLocalizationFileAccessor(options.ResourcesPath));

        return services;
    }

    /// <summary>
    /// Creates an <see cref="IJsonStringLocalizer"/> instance for the specified domain and culture.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve services from.</param>
    /// <param name="domain">The domain (resource file name without extension or culture suffix).</param>
    /// <param name="culture">The culture to use for localization. If null, the current culture is used.</param>
    /// <returns>An <see cref="IJsonStringLocalizer"/> instance for the specified domain and culture.</returns>
    /// <remarks>
    /// This method creates a new instance of <see cref="JsonStringLocalizer"/> each time it's called.
    /// It's not registered in the service collection as a singleton or scoped service.
    /// </remarks>
    public static IJsonStringLocalizer GetJsonLocalizer(
        this IServiceProvider provider,
        string domain,
        CultureInfo? culture = null)
    {
        JsonLocalizationFileAccessor accessor = provider.GetRequiredService<JsonLocalizationFileAccessor>();
        JsonLocalizationOptions options = provider.GetRequiredService<JsonLocalizationOptions>();

        return new JsonStringLocalizer(accessor, domain, culture ?? CultureInfo.CurrentCulture, options);
    }
}
