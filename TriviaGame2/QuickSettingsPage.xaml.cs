using System.Diagnostics;

namespace TriviaGame2;

public partial class QuickSettingsPage : ContentPage
{
    public List<string> CategoryOptions { get; set; }
    public List<string> DifficultyOptions { get; set; }
    public List<string> TypeOptions { get; set; }
    public List<string> NumPlayersOptions { get; set; }
    public List<string> NumQuestionsOptions { get; set; }

    public string SelectedCategory { get; set; }
    public string SelectedDifficulty { get; set; }
    public string SelectedType { get; set; }
    public string SelectedNumPlayers { get; set; }
    public string SelectedNumQuestions { get; set; }
    public int SelectedCategoryId { get; private set; }

    public int ReceivedSessionToken { get; set; }

    private Dictionary<int, string> CategoryMapping { get; set; }

    public QuickSettingsPage()
    {
        InitializeComponent();
        _ = InitializeAsync();


    }

    private async Task InitializeAsync()
    {
        var gameController = new GameController(new ApiService());
        var categories = await gameController.GetCategoryAsync();
        CategoryOptions = categories.Values.ToList();
        CategoryMapping = categories;

        DifficultyOptions = new List<string> { "Easy", "Medium", "Hard" };
        TypeOptions = new List<string> { "MCQ", "True or False" };
        NumPlayersOptions = new List<string> { "1", "2", "3", "4" };
        NumQuestionsOptions = Enumerable.Range(1, 50).Select(n => n.ToString()).ToList();

        // Load persistent data
        SelectedCategory = Preferences.Get("SelectedCategory", string.Empty);
        SelectedDifficulty = Preferences.Get("SelectedDifficulty", "medium");
        SelectedType = Preferences.Get("SelectedType", "multiple");
        SelectedNumPlayers = Preferences.Get("SelectedNumPlayers", "1");
        SelectedNumQuestions = Preferences.Get("SelectedNumQuestions", "10");
        SelectedCategoryId = Preferences.Get("SelectedCategoryId", 0);

    
            category.SelectedItem = SelectedCategory; // Matches XAML name

            difficulty.SelectedItem = SelectedDifficulty; // Matches XAML name

            type.SelectedItem = SelectedType; // Matches XAML name

            players.SelectedItem = SelectedNumPlayers; // Matches XAML name

       
            questions.SelectedItem = SelectedNumQuestions; // Matches XAML name

        BindingContext = this;
    }

    private void OnCategorySelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategoryName = (string)picker.SelectedItem;

        if (CategoryMapping.FirstOrDefault(c => c.Value == selectedCategoryName).Key is int categoryId)
        {
            Debug.WriteLine("here");
            SelectedCategoryId = categoryId;
            SelectedCategory = selectedCategoryName;
            Preferences.Set("SelectedCategory", SelectedCategory);
            Preferences.Set("SelectedCategoryId", SelectedCategoryId);
        }
    }

    private void OnDifficultySelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedDifficulty = (string)picker.SelectedItem;
        Preferences.Set("SelectedDifficulty", SelectedDifficulty);
    }

    private void OnTypeSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedType = (string)picker.SelectedItem;
        Preferences.Set("SelectedType", SelectedType);
    }

    private void OnNumPlayersSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedNumPlayers = (string)picker.SelectedItem;
        Preferences.Set("SelectedNumPlayers", SelectedNumPlayers);
    }

    private void OnNumQuestionsSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedNumQuestions = (string)picker.SelectedItem;
        Preferences.Set("SelectedNumQuestions", SelectedNumQuestions);
    }

    private async void OnStartGameButtonClicked(object sender, EventArgs e)
    {
        var gameController = new GameController(new ApiService());
        var gamePage = new GamePage
        {
            BindingContext = gameController
        };

        string token = await gameController.GetSessionTokenAsync();

        // Set defaults for selected variables if not chosen
        string categoryId = SelectedCategoryId != 0 ? SelectedCategoryId.ToString() : "";
        string difficulty = SelectedDifficulty ?? ""; 
        string type = SelectedType ?? ""; 
        string numPlayers = SelectedNumPlayers ?? ""; 
        string numQuestions = SelectedNumQuestions ?? "5"; 

        await Navigation.PushAsync(gamePage);

        await gameController.GetQuestionFromApi(
            categoryId,
            difficulty,
            type,
            numPlayers,
            numQuestions,
            token
        );
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Settings());
    }

}
