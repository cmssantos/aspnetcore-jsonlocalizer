using System.Globalization;
using Cms.AspNetCore.JsonLocalizer.Abstractions;
using Cms.AspNetCore.JsonLocalizer.Infrastructure;
using Cms.AspNetCore.JsonLocalizer.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cms.AspNetCore.JsonLocalizer.Extensions;

/// <summary>
/// Provides extension methods for registering JSON localization services in the ASP.NET Core dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds JSON localization services to the <see cref="IServiceCollection"/> with the specified configuration options.
    /// </summary>
    /// <param name="services">The service collection to add the localization services to.</param>
    /// <param name="configure">An action to configure <see cref="JsonLocalizationOptions"/> such as resource path and default culture.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddJsonLocalization(this IServiceCollection services,
        Action<JsonLocalizationOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        // Configure localization options
        services.Configure(configure);

        // Register HttpContextAccessor to access HTTP context inside services
        services.AddHttpContextAccessor();

        // Register a singleton JsonLocalizationFileAccessor to manage JSON files and caching
        services.AddSingleton(sp =>
        {
            JsonLocalizationOptions options = sp.GetRequiredService<IOptions<JsonLocalizationOptions>>().Value;
            return new JsonLocalizationFileAccessor(options.ResourcesPath);
        });

        // Register scoped IJsonStringLocalizer that resolves culture from the HTTP request or falls back to default culture
        services.AddScoped<IJsonStringLocalizer>(sp =>
        {
            JsonLocalizationFileAccessor accessor = sp.GetRequiredService<JsonLocalizationFileAccessor>();
            HttpContext httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            JsonLocalizationOptions options = sp.GetRequiredService<IOptions<JsonLocalizationOptions>>().Value;

            // Attempt to get culture from Accept-Language header, fallback to default culture or current culture
            var acceptLanguage = httpContext?.Request.Headers["Accept-Language"].FirstOrDefault();
            var culture = !string.IsNullOrWhiteSpace(acceptLanguage)
                ? acceptLanguage
                : options.DefaultCulture ?? CultureInfo.CurrentCulture.Name;

            return new JsonStringLocalizer(accessor, culture);
        });

        return services;
    }
}
