using Newtonsoft.Json;

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
            var response = await _httpClient.GetStringAsync(url);

            return  JsonConvert.DeserializeObject<ApiResponse>(response);
        }
        catch (Exception ex)
        {
            // Handling errors
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
}
