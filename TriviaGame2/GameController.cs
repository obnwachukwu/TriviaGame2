using TriviaGame2;

public class GameController : BindableObject
{
    private ApiResponse _data;
    private string _questionText;
    private string _questionType;
    private bool _isLoading;
    private ContentView _contentView;

    public ApiResponse Data
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

    public GameController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task GetQuestionFromApi()
    {
        IsLoading = true;
        var url = "https://opentdb.com/api.php?amount=1&category=21&difficulty=easy&type=boolean";
        Data = await _apiService.GetDataAsync(url);

        if (Data != null && Data.Results.Count > 0)
        {
            // Set the QuestionText and QuestionType from the response
            QuestionText = Data.Results[0].Question;
            QuestionType = Data.Results[0].Type;

            // Dynamically set the ContentView based on question type
            if (QuestionType == "boolean")
            {
                ContentView = new TFContent(); // True/False content view
            }
            else
            {
                ContentView = new MCQContent(); // Multiple-choice content view
            }
        }
        else
        {
            // Handle case when no data is found
            QuestionText = "No question found!";
            ContentView = new TFContent(); // Default content view
        }

        IsLoading = false;
    }
}
