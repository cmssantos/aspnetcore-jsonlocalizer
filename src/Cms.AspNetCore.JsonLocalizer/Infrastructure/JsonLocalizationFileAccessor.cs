using System.Text.Json.Nodes;

namespace Cms.AspNetCore.JsonLocalizer.Infrastructure;

/// <summary>
/// Provides access to JSON localization files and caches their content in memory.
/// </summary>
/// <param name="basePath">The base directory path where localization JSON files are stored.</param>
public class JsonLocalizationFileAccessor(string basePath)
{
    private readonly string _basePath = basePath;
    private readonly Dictionary<string, JsonNode?> _cache = [];

    /// <summary>
    /// Retrieves the localized string value for the specified key and culture.
    /// Supports nested keys separated by dots (e.g., "error.notFound").
    /// </summary>
    /// <param name="key">The localization key, which can represent nested JSON properties separated by dots.</param>
    /// <param name="culture">The culture code (e.g., "en-US", "pt-BR") corresponding to the JSON file name.</param>
    /// <returns>
    /// The localized string value if found; otherwise, <c>null</c>.
    /// </returns>
    public string? GetValue(string key, string culture)
    {
        if (!_cache.TryGetValue(culture, out JsonNode? root))
        {
            var file = Path.Combine(_basePath, $"{culture}.json");
            if (!File.Exists(file))
            {
                return null;
            }

            root = JsonNode.Parse(File.ReadAllText(file));
            _cache[culture] = root;
        }

        return Traverse(root, key.Split('.'))?.ToString();
    }

    /// <summary>
    /// Recursively traverses the JSON node tree based on the provided key parts.
    /// </summary>
    /// <param name="node">The current JSON node to traverse.</param>
    /// <param name="parts">An array of strings representing the nested keys to traverse.</param>
    /// <returns>
    /// The final <see cref="JsonNode"/> corresponding to the key if found; otherwise, <c>null</c>.
    /// </returns>
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
