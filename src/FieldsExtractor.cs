using System.Text.Json;

namespace JsonChisel;

public class FieldsExtractor
{
    public static object ExtractFields(string responseBody, string fields)
    {
        return GetJsonElementTree(
            JsonSerializer.Deserialize<JsonElement>(responseBody), 
            Node.Parse(fields.Split(',', StringSplitOptions.RemoveEmptyEntries)));
    }
    static object GetJsonElementTreeItem(JsonElement jsonElement, Node tree)
    {
        if (jsonElement.ValueKind == JsonValueKind.Null || jsonElement.ValueKind == JsonValueKind.Undefined)
            return default;

        jsonElement = jsonElement.TryGetProperty(tree.Name, out JsonElement value) ? value : default;
        
        if (tree.Children.Count == 0)
            return jsonElement;


        if (jsonElement.ValueKind == JsonValueKind.Array)
        {
            return jsonElement.
                EnumerateArray().
                Select(item=> tree.Children.ToDictionary(x => x.Name, y => GetJsonElementTreeItem(item, y)));
        }

       
        return tree.Children.ToDictionary(x => x.Name, y => GetJsonElementTreeItem(jsonElement, y));
    }

    static object GetJsonElementTree(JsonElement jsonElement, Node tree)
    {
        if (jsonElement.ValueKind == JsonValueKind.Null || jsonElement.ValueKind == JsonValueKind.Undefined)
            return default;

        return tree.Children.ToDictionary(x => x.Name, y => GetJsonElementTreeItem(jsonElement, y));
    }
}