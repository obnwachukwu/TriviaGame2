
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

    private Dictionary<int, string> CategoryMapping { get; set; }

    public QuickSettingsPage()
    {
        InitializeComponent();
        _ = InitializeAsync();

    }


    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Settings());
    }

    private void OnDifficultySelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedDifficulty = (string)picker.SelectedItem;
    }

    private void OnTypeSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedType = (string)picker.SelectedItem;
    }

    private void OnNumPlayersSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedNumPlayers = (string)picker.SelectedItem;
    }

    private void OnNumQuestionsSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedNumQuestions = (string)picker.SelectedItem;
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


        BindingContext = this;
    }

    private void OnCategorySelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategoryName = (string)picker.SelectedItem;

        if (CategoryMapping.FirstOrDefault(c => c.Value == selectedCategoryName).Key is int categoryId)
        {
            SelectedCategoryId = categoryId;
        }
    }

    private async void OnStartGameButtonClicked(object sender, EventArgs e)
    {
        var gameController = new GameController(new ApiService());
        var gamePage = new GamePage
        {
            BindingContext = gameController
        };

        // Navigate to the GamePage
        await Navigation.PushAsync(gamePage);

        await gameController.GetQuestionFromApi(
            SelectedCategoryId.ToString(),
            SelectedDifficulty,
            SelectedType,
            SelectedNumPlayers,
            SelectedNumQuestions
        );
    }
}
