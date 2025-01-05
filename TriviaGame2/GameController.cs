using System.Diagnostics;
using System.Text.Json;
using TriviaGame2;

public class GameController : BindableObject
{
    private QuestionsResponse _data;
    private string _questionText;
    private string _questionType;
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

    public string QuestionText
    {
        get => _questionText;
        set
        {
            _questionText = value;
            OnPropertyChanged();
        }
    }

    public string QuestionType
    {
        get => _questionType;
        set
        {
            _questionType = value;
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
                string SelectedNumQuestions
        )
    {
        var selectedType = "";

        IsLoading = true;

        // Ensure SelectedNumQuestions is parsed as an integer
        int numQuestions = int.Parse(SelectedNumQuestions);

        // Ensure selectedCategory is a valid category ID, typically an integer
        int categoryId = int.Parse(selectedCategory); // Assuming selectedCategory is an ID (string -> int)

        if (SelectedType == "MCQ")
        {
            selectedType = "multiple";
        } else
        {
            selectedType = "boolean";
        }

        try
        {
            var url = $"https://opentdb.com/api.php?amount={SelectedNumQuestions}&type={selectedType}&difficulty={SelectedDifficulty.ToLower()}&category={selectedCategory}";
            Data = await _apiService.GetQuestionAsync(url);
            if (Data != null)
            {

                Debug.WriteLine($"Generated URL: {Data}");
                Debug.WriteLine($"First question: {Data.results[0].question}");
                _questionText = Data.results[0].question;

                ContentView = new TFContent(_questionText);

            }
        }
        catch (Exception ex) {
            // Handle case when no data is found
            QuestionText = "No True/false question found!";
            ContentView = new MCQContent(); // Default content view
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
            Console.WriteLine($"Error fetching categories: {ex.Message}");
            return new Dictionary<int, string>();
        }
        finally
        {
            IsLoading = false;
        }
    }

}
