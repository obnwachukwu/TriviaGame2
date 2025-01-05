using Newtonsoft.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<QuestionsResponse> GetQuestionAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(url);

            return  JsonConvert.DeserializeObject<QuestionsResponse>(response);
        }
        catch (Exception ex)
        {
            // Handling errors
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<CategoryResponse> GetCategoryAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(url);

            return JsonConvert.DeserializeObject<CategoryResponse>(response);
        }
        catch (Exception ex)
        {
            // Handling errors
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
}
