using System.Text.Json.Nodes;

namespace Cms.AspNetCore.JsonLocalizer.Infrastructure;

/// <summary>
/// Provides access to localized strings stored in JSON files.
/// </summary>
/// <remarks>
/// This class handles loading, caching, and retrieving localized strings from JSON files
/// organized by domain and culture.
/// </remarks>
public class JsonLocalizationFileAccessor(string basePath)
{
    private readonly string _basePath = basePath;
    private readonly Dictionary<string, Dictionary<string, JsonNode?>> _cache = [];

    /// <summary>
    /// Gets a localized string value from the appropriate JSON resource file.
    /// </summary>
    /// <param name="domain">The domain (resource file name without extension or culture suffix).</param>
    /// <param name="key">The key to look up, which can be a dot-separated path for nested JSON objects.</param>
    /// <param name="culture">The primary culture to search for the value.</param>
    /// <param name="fallbackCulture">Optional fallback culture to use if the value is not found in the primary culture.</param>
    /// <returns>
    /// The localized string if found; otherwise, null.
    /// </returns>
    public string? GetValue(string domain, string key, CultureInfo culture, CultureInfo? fallbackCulture = null)
    {
        foreach (CultureInfo cultureToTry in GetCultureFallbacks(culture, fallbackCulture))
        {
            // Primeiro tenta carregar o arquivo específico do domínio
            if (TryLoadFile(domain, cultureToTry.Name, out JsonNode? root))
            {
                JsonNode? value = Traverse(root, key.Split('.'));
                if (value is not null)
                {
                    return value.ToString();
                }
            }

            // Se a chave não foi encontrada no arquivo específico do domínio,
            // tenta no arquivo apenas com a cultura
            if (TryLoadCultureOnlyFile(cultureToTry.Name, out JsonNode? cultureRoot))
            {
                JsonNode? value = Traverse(cultureRoot, key.Split('.'));
                if (value is not null)
                {
                    return value.ToString();
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Generates a sequence of cultures to try when looking up a localized value.
    /// </summary>
    /// <param name="culture">The primary culture.</param>
    /// <param name="defaultCulture">The fallback culture.</param>
    /// <returns>A sequence of cultures in fallback order.</returns>
    private static IEnumerable<CultureInfo> GetCultureFallbacks(CultureInfo culture, CultureInfo? defaultCulture)
    {
        yield return culture;

        if (!string.IsNullOrEmpty(culture.Parent?.Name) && culture.Parent.Name != culture.Name)
        {
            yield return culture.Parent;
        }

        if (defaultCulture is not null)
        {
            yield return defaultCulture;
        }
    }

    /// <summary>
    /// Attempts to load a JSON file for the specified domain and culture.
    /// </summary>
    /// <param name="domain">The domain (resource file name without extension or culture suffix).</param>
    /// <param name="cultureName">The culture name.</param>
    /// <param name="root">When this method returns, contains the root JSON node if the file was loaded successfully; otherwise, null.</param>
    /// <returns>true if the file was loaded successfully; otherwise, false.</returns>
    private bool TryLoadFile(string domain, string cultureName, out JsonNode? root)
    {
        string cacheKey = $"{domain}:{cultureName}";

        if (_cache.TryGetValue(domain, out Dictionary<string, JsonNode?>? domainCache) &&
            domainCache.TryGetValue(cultureName, out root))
        {
            return root is not null;
        }

        var file = Path.Combine(_basePath, $"{domain}.{cultureName}.json");
        root = null;

        if (File.Exists(file))
        {
            root = JsonNode.Parse(File.ReadAllText(file));
        }

        if (!_cache.TryGetValue(domain, out Dictionary<string, JsonNode?>? value))
        {
            value = [];
            _cache[domain] = value;
        }
        value[cultureName] = root;

        return root is not null;
    }

    /// <summary>
    /// Attempts to load a JSON file for the specified culture only (without domain).
    /// </summary>
    /// <param name="cultureName">The culture name.</param>
    /// <param name="root">When this method returns, contains the root JSON node if the file was loaded successfully; otherwise, null.</param>
    /// <returns>true if the file was loaded successfully; otherwise, false.</returns>
    private bool TryLoadCultureOnlyFile(string cultureName, out JsonNode? root)
    {
        string cacheKey = $"culture-only:{cultureName}";

        if (_cache.TryGetValue(cacheKey, out Dictionary<string, JsonNode?>? cultureCache) &&
            cultureCache.TryGetValue(cultureName, out root))
        {
            return root is not null;
        }

        var file = Path.Combine(_basePath, $"{cultureName}.json");
        root = null;

        if (File.Exists(file))
        {
            root = JsonNode.Parse(File.ReadAllText(file));
        }

        if (!_cache.TryGetValue(cacheKey, out Dictionary<string, JsonNode?>? value))
        {
            value = [];
            _cache[cacheKey] = value;
        }
        value[cultureName] = root;

        return root is not null;
    }

    /// <summary>
    /// Traverses a JSON node hierarchy using the specified path parts.
    /// </summary>
    /// <param name="node">The root JSON node to start traversal from.</param>
    /// <param name="parts">The path parts representing the traversal path.</param>
    /// <returns>The JSON node at the specified path if found; otherwise, null.</returns>
    private static JsonNode? Traverse(JsonNode? node, string[] parts)
    {
        foreach (var part in parts)
        {
            if (node is JsonObject obj && obj.TryGetPropertyValue(part, out JsonNode? next))
            {
                node = next;
            }
            else
            {
                return null;
            }
        }

        return node;
    }
}
