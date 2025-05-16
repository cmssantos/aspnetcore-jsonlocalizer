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

        IServiceCollection safeServices = services;

        // Configure localization options
        safeServices.Configure(configure);

        // Register HttpContextAccessor to access HTTP context inside services
        safeServices.AddHttpContextAccessor();

        // Register a singleton JsonLocalizationFileAccessor to manage JSON files and caching
        safeServices.AddSingleton(sp =>
        {
            JsonLocalizationOptions options = sp.GetRequiredService<IOptions<JsonLocalizationOptions>>().Value
                ?? throw new InvalidOperationException("JsonLocalizationOptions is not configured.");

            return new JsonLocalizationFileAccessor(options.ResourcesPath);
        });

        // Register scoped IJsonStringLocalizer that resolves culture from the HTTP request or falls back to default culture
        safeServices.AddScoped<IJsonStringLocalizer>(sp =>
        {
            var accessor = sp.GetRequiredService<JsonLocalizationFileAccessor>();
            var httpContext = sp.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
            var options = sp.GetRequiredService<IOptions<JsonLocalizationOptions>>()?.Value;

            if (options == null)
                throw new InvalidOperationException("JsonLocalizationOptions is not configured.");

            var acceptLanguage = httpContext?.Request?.Headers["Accept-Language"].FirstOrDefault();
            var culture = !string.IsNullOrWhiteSpace(acceptLanguage)
                ? acceptLanguage
                : options.DefaultCulture ?? CultureInfo.CurrentCulture.Name;

            return new JsonStringLocalizer(accessor, culture);
        });

        return safeServices;
    }
}
