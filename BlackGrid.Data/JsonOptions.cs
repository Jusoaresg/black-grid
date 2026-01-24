using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlackGrid.Data;

public static class JsonOptions
{
	public static JsonSerializerOptions GetJsonOptions()
	{
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		options.Converters.Add(new JsonStringEnumConverter());

		return options;
	}
}
