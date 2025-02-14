using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class PartsManager
{
    static readonly string BaseAddress = "URL_GOES_HERE";
    static readonly string Url = $"{BaseAddress}/api/";
    private static string authorizationKey;
    private static HttpClient client;

    private static async Task<HttpClient> GetClient()
    {
        if (client != null)
            return client;

        client = new HttpClient();

        if (string.IsNullOrEmpty(authorizationKey))
        {
            authorizationKey = await client.GetStringAsync($"{Url}login");
            authorizationKey = JsonSerializer.Deserialize<string>(authorizationKey);
            client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
        }

        return client;
    }

    public static async Task<List<Part>> GetAll()
    {
        HttpClient httpClient = await GetClient();
        return await httpClient.GetFromJsonAsync<List<Part>>($"{Url}parts");
    }

    public static async Task Add(Part part)
    {
        HttpClient httpClient = await GetClient();
        await httpClient.PostAsJsonAsync($"{Url}parts", part);
    }

    public static async Task Update(Part part)
    {
        HttpClient httpClient = await GetClient();
        await httpClient.PutAsJsonAsync($"{Url}parts/{part.PartID}", part);
    }

    public static async Task Delete(int partId)
    {
        HttpClient httpClient = await GetClient();
        await httpClient.DeleteAsync($"{Url}parts/{partId}");
    }
}
