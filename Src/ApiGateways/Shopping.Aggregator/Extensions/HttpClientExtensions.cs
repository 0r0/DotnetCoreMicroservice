using System.Text.Json;

namespace Shopping.Aggregator.Extensions;

public static class HttpClientExtensions
{
    public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"some thing went  wrong  calling the api happen{response.ReasonPhrase}");
        // return await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(dataAsString,new JsonSerializerOptions(){ PropertyNameCaseInsensitive = true});
    }
}