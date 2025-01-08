using System.Diagnostics;
using System.Text.Json;

namespace TriviaGame2;

public partial class QuickSettingsPage : ContentPage
{
    private const string SavedGameKey = "SavedGame";

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
        IsBusy = true;

        try
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

            // Update UI elements
            category.SelectedItem = SelectedCategory;
            difficulty.SelectedItem = SelectedDifficulty;
            type.SelectedItem = SelectedType;
            players.SelectedItem = SelectedNumPlayers;

            if (!string.IsNullOrEmpty(SelectedNumQuestions))
                questions.SelectedItem = SelectedNumQuestions;

            // Debug: Log the loaded preferences
            Debug.WriteLine($"Loaded Preferences:");
            Debug.WriteLine($"Category: {SelectedCategory}");
            Debug.WriteLine($"Difficulty: {SelectedDifficulty}");
            Debug.WriteLine($"Type: {SelectedType}");
            Debug.WriteLine($"Players: {SelectedNumPlayers}");
            Debug.WriteLine($"Questions: {SelectedNumQuestions}");
            Debug.WriteLine($"Category ID: {SelectedCategoryId}");

            BindingContext = this;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing settings: {ex.Message}");
            await DisplayAlert("Error", "Failed to load settings. Please try again.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnCategorySelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategoryName = (string)picker.SelectedItem;

        if (CategoryMapping.FirstOrDefault(c => c.Value == selectedCategoryName).Key is int categoryId)
        {
            SelectedCategoryId = categoryId;
            SelectedCategory = selectedCategoryName;
            Preferences.Set("SelectedCategory", SelectedCategory);
            Preferences.Set("SelectedCategoryId", SelectedCategoryId);

            // Debug: Log the updated preference
            Debug.WriteLine($"Updated Category: {SelectedCategory} (ID: {SelectedCategoryId})");
        }
    }

    private void OnDifficultySelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedDifficulty = (string)picker.SelectedItem;
        Preferences.Set("SelectedDifficulty", SelectedDifficulty);

        // Debug: Log the updated preference
        Debug.WriteLine($"Updated Difficulty: {SelectedDifficulty}");
    }

    private void OnTypeSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedType = (string)picker.SelectedItem;
        Preferences.Set("SelectedType", SelectedType);

        // Debug: Log the updated preference
        Debug.WriteLine($"Updated Type: {SelectedType}");
    }

    private void OnNumPlayersSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedNumPlayers = (string)picker.SelectedItem;
        Preferences.Set("SelectedNumPlayers", SelectedNumPlayers);

        // Debug: Log the updated preference
        Debug.WriteLine($"Updated Number of Players: {SelectedNumPlayers}");
    }

    private void OnNumQuestionsSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedNumQuestions = (string)picker.SelectedItem;
        Preferences.Set("SelectedNumQuestions", SelectedNumQuestions);

        // Debug: Log the updated preference
        Debug.WriteLine($"Updated Number of Questions: {SelectedNumQuestions}");
    }

    private async void OnStartGameButtonClicked(object sender, EventArgs e)
    {
        var gameController = new GameController(new ApiService());
        var gamePage = new GamePage
        {
            BindingContext = gameController
        };

        try
        {
            string token = await gameController.GetSessionTokenAsync();

            // Game State
            string categoryId = SelectedCategoryId != 0 ? SelectedCategoryId.ToString() : "";
            string difficulty = SelectedDifficulty ?? "";
            string type = SelectedType ?? "";
            string numPlayers = SelectedNumPlayers ?? "";
            string numQuestions = SelectedNumQuestions ?? "5";

            // Save Game Progress (e.g., current score, question number, etc.)
            Preferences.Set("GameProgress", "InProgress");
            Preferences.Set("CurrentQuestion", 1);  // For example, start from the first question
            Preferences.Set("CurrentScore", 0);     // Set initial score
            Preferences.Set("NumQuestions", numQuestions);

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
        catch (Exception ex)
        {
            Debug.WriteLine($"Error starting the game: {ex.Message}");
            await DisplayAlert("Error", "Failed to start the game. Please check your settings.", "OK");
        }
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Settings());
    }

    private void OnResetButtonClicked(object sender, EventArgs e)
    {
        // Reset specific settings only
        SelectedCategory = string.Empty;
        SelectedDifficulty = "medium";
        SelectedType = "multiple";
        SelectedNumPlayers = "1";
        SelectedNumQuestions = "10";
        SelectedCategoryId = 0;

        // Remove only specific preferences, not all
        Preferences.Remove("SelectedCategory");
        Preferences.Remove("SelectedCategoryId");
        Preferences.Remove("SelectedDifficulty");
        Preferences.Remove("SelectedType");
        Preferences.Remove("SelectedNumPlayers");
        Preferences.Remove("SelectedNumQuestions");

        // Reset the UI elements
        category.SelectedItem = SelectedCategory;
        difficulty.SelectedItem = SelectedDifficulty;
        type.SelectedItem = SelectedType;
        players.SelectedItem = SelectedNumPlayers;
        questions.SelectedItem = SelectedNumQuestions;

        Debug.WriteLine("Settings reset to default values.");
    }
}
