using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace App.Common.Utility.Extensions
{
    public static class JsonExtension
    {
        public static Dictionary<string, JsonElement> Deserialize(this string json)
        {
            IEnumerable<(string Path, JsonProperty P)> GetLeaves(string path, JsonProperty p)
                => p.Value.ValueKind != JsonValueKind.Object
                    ? new[] { (Path: path == null ? p.Name : path + "." + p.Name, p) }
                    : p.Value.EnumerateObject().SelectMany(child => GetLeaves(path == null ? p.Name : path + "." + p.Name, child));

            using (JsonDocument document = JsonDocument.Parse(json)) // Optional JsonDocumentOptions options
                return document.RootElement.EnumerateObject()
                    .SelectMany(p => GetLeaves(null, p))
                    .ToDictionary(k => k.Path, v => v.P.Value.Clone()); //Clone so that we can use the values outside of using
        }

        public static string Serialize(this object obj, JsonTypeInfo typeInfo) => JsonSerializer.Serialize(obj, typeInfo);

        public static string Serialize<T>(this T obj, JsonTypeInfo typeInfo) => JsonSerializer.Serialize(obj, typeInfo);
    }
}
