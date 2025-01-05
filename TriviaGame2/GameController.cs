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

    public  GameController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task GetQuestionFromApi()
    {
        IsLoading = true;

        try
        {
            var url = "https://opentdb.com/api.php?amount=10&type=boolean";
            Data = await _apiService.GetDataAsync(url);
            if (Data != null)
            {
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
}
