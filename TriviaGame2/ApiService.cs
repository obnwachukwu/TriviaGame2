using Newtonsoft.Json;
using TriviaGame2;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse> GetDataAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(data);
        }
        catch (Exception ex)
        {
            // Handling errors
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
}
