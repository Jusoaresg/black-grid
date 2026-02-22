using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlackGrid.Data;

public static class JsonOptions
{
	public static JsonSerializerOptions Options = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true,
		Converters = {
				new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
				new JsonStringEnumConverter()
			}
	};
}
