
using System.Diagnostics;
using TriviaGame2;

public class GameController : BindableObject
{
    private QuestionsResponse _data;
    private bool _isLoading;
    private ContentView _contentView;

    public List<Player> Players { get; private set; } = new List<Player>();
    private int _currentPlayerIndex = 0;
    private int _currentRound = 0;
    private int _totalRounds;

    // Getter and Setters

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

    // Constructors
    public GameController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public GameController(int numPlayers, int numQuestions)
    {
        _totalRounds = numQuestions;
        InitializePlayers(numPlayers);
    }


    private void InitializePlayers(int numPlayers)
    {
        for (int i = 0; i < numPlayers; i++)
        {
            Players.Add(new Player { Name = $"Player {i + 1}" });
        }
    }

    public Player GetCurrentPlayer()
    {
        return Players[_currentPlayerIndex];
    }

    public void NextTurn()
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % Players.Count;

        if (_currentPlayerIndex == 0)
        {
            _currentRound++;
        }
    }

    public bool IsGameOver()
    {
        return _currentRound >= _totalRounds;
    }



    // API Calls
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
        int categoryId;
        int numQuestions = 0;

        IsLoading = true;

        if (SelectedNumQuestions != "")
        {
             numQuestions = int.Parse(SelectedNumQuestions);
        }

        if (selectedCategory != "")
        {
            categoryId = int.Parse(selectedCategory);
        }


        if (SelectedType == "MCQ")
        {
            selectedType = "multiple";
        } else
        {
            selectedType = "boolean";
        }

        if (string.IsNullOrEmpty(SelectedNumQuestions))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Enter Amount of Questions", "OK");
            await Application.Current.MainPage.Navigation.PushAsync(new QuickSettingsPage());
        }
        try
        {
            var url="";

            if (token == null)
            {
                url = "https://opentdb.com/api.php?amount=" + SelectedNumQuestions
                + "&type=" + selectedType
                + "&difficulty=" + SelectedDifficulty.ToLower()
                + "&category=" + selectedCategory;
            }
            else
            {
                Debug.WriteLine("There was a token");
                url = "https://opentdb.com/api.php?amount=" + SelectedNumQuestions
                +"&type=" + selectedType
                +"&difficulty=" +  SelectedDifficulty.ToLower()
                +"&category=" + selectedCategory
                +"&token=" + token;
                Debug.WriteLine(url);
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
            Debug.WriteLine($"{ex}");
            
        } finally { IsLoading = false; }
    }

    public async Task LoadQuestionsForPlayer(Player player)
    {
        string selectedCategory = player.SelectedCategory;
        string selectedType = player.SelectedType;
        string selectedDifficulty = player.SelectedDifficulty;

        var url = $"https://opentdb.com/api.php?amount=1&type={selectedType}&difficulty={selectedDifficulty.ToLower()}&category={selectedCategory}";

        // Fetch the questions from the API
        var questionsResponse = await _apiService.GetQuestionAsync(url);

        // Assign the results to the player's Questions property
        if (questionsResponse != null && questionsResponse.results != null)
        {
            player.Questions = questionsResponse.results; 
        }
        else
        {
            player.Questions = new List<Result>(); // Fallback in case of no results
        }
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
