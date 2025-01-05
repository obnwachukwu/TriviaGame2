using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Text.Json;
using TriviaGame2;

public class GameController : BindableObject
{
    private QuestionsResponse _data;
    private bool _isLoading;
    private ContentView _contentView;

    public QuestionsResponse Data
    {
        get => _data;
        set
        {
            _data = value;
            OnPropertyChanged();
        }
    }


    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public ContentView ContentView
    {
        get => _contentView;
        set
        {
            _contentView = value;
            OnPropertyChanged();
        }
    }

    private readonly ApiService _apiService;

    public  GameController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task GetQuestionFromApi(
                string selectedCategory,
                string SelectedDifficulty,
                string SelectedType,
                string SelectedNumPlayers,
                string SelectedNumQuestions,
                string token 
        )
    {
        var selectedType = "";

        IsLoading = true;

        int numQuestions = int.Parse(SelectedNumQuestions);

        int categoryId = int.Parse(selectedCategory);

        if (SelectedType == "MCQ")
        {
            selectedType = "multiple";
        } else
        {
            selectedType = "boolean";
        }

        try
        {
            var url="";

            if (token == null)
            {
                 url = $"https://opentdb.com/api.php?amount={SelectedNumQuestions}&type={selectedType}&difficulty={SelectedDifficulty.ToLower()}&category={selectedCategory}";
            }
            else
            {
                Debug.WriteLine("There was a token");
                 url = $"https://opentdb.com/api.php?amount={SelectedNumQuestions}&type={selectedType}&difficulty={SelectedDifficulty.ToLower()}&category={selectedCategory}&token={token}";
            }
            
            Data = await _apiService.GetQuestionAsync(url);
            if (Data.results[0].type == "boolean")
            {

                Debug.WriteLine($"Response Code: {Data.ResponseCode}");
                Debug.WriteLine($"First question: {Data.results[0].question}");
               
                ContentView = new TFContent(Data.results);

            }
            else {
                ContentView = new MCQContent(Data.results);
            }
        }
        catch (Exception ex) {
            // Handle case when no data is found
            Debug.WriteLine("No True/false question found!");
            
        } finally { IsLoading = false; }
    }

    public async Task<Dictionary<int, string>> GetCategoryAsync()
    {
        IsLoading = true;

        try
        {
            var url = "https://opentdb.com/api_category.php";

            // Fetch the data using the ApiService
            var jsonResponse = await _apiService.GetCategoryAsync(url);

            // Convert the list of categories to a dictionary
            if (jsonResponse?.Trivia_Categories != null)
            {
                return jsonResponse.Trivia_Categories.ToDictionary(cat => cat.Id, cat => cat.Name);
            }

            // Return empty dictionary if no categories found
            return new Dictionary<int, string>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching categories: {ex.Message}");
            return new Dictionary<int, string>();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task <String> GetSessionTokenAsync()
    {
        IsLoading = true;

        try
        {
            var url = "https://opentdb.com/api_token.php?command=request";

            // Fetch the data using the ApiService
            var jsonResponse = await _apiService.GetSessionTokenAsync(url);
            Debug.WriteLine($"This is the token: {jsonResponse.Token}");

      
            return jsonResponse.Token;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching categories: {ex.Message}");
            return ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

}
