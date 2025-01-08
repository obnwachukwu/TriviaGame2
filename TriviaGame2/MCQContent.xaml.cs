using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace TriviaGame2;

public partial class MCQContent : ContentView
{
    private List<Result> _questions;
    private int _currentQuestionIndex = 0;
    private int _correctAnswers = 0;
    private int _totalQuestions;

    // These variables are necessary to save the game state
    private string SelectedCategory = "General Knowledge";  // Example
    private string SelectedDifficulty = "Medium";  // Example
    private string SelectedType = "MCQ";  // Example
    private string SelectedNumPlayers = "1";  // Example
    private string SelectedNumQuestions = "10";  // Example

    private const string SavedGameKey = "SavedGame";

    public MCQContent(List<Result> questions)
    {
        InitializeComponent();

        _questions = questions;
        _totalQuestions = questions.Count;
        BindingContext = this;

        DisplayCurrentQuestion();
    }

    public string CurrentQuestion
    {
        get { return WebUtility.HtmlDecode(_questions[_currentQuestionIndex].question); }
    }

    public string OptionA => WebUtility.HtmlDecode(_questions[_currentQuestionIndex].incorrect_answers[0]);
    public string OptionB => WebUtility.HtmlDecode(_questions[_currentQuestionIndex].incorrect_answers[1]);
    public string OptionC => WebUtility.HtmlDecode(_questions[_currentQuestionIndex].incorrect_answers[2]);
    public string OptionD => WebUtility.HtmlDecode(_questions[_currentQuestionIndex].correct_answer);

    private void DisplayCurrentQuestion()
    {
        var options = new List<string>
        {
            OptionA,
            OptionB,
            OptionC,
            OptionD
        }.OrderBy(_ => Guid.NewGuid()).ToList();

        ButtonOptionA.Text = options[0];
        ButtonOptionB.Text = options[1];
        ButtonOptionC.Text = options[2];
        ButtonOptionD.Text = options[3];

        OnPropertyChanged(nameof(CurrentQuestion));
    }

    private void OnOptionAClicked(object sender, EventArgs e) => CheckAnswer(ButtonOptionA.Text);
    private void OnOptionBClicked(object sender, EventArgs e) => CheckAnswer(ButtonOptionB.Text);
    private void OnOptionCClicked(object sender, EventArgs e) => CheckAnswer(ButtonOptionC.Text);
    private void OnOptionDClicked(object sender, EventArgs e) => CheckAnswer(ButtonOptionD.Text);

    private void CheckAnswer(string selectedAnswer)
    {
        var correctAnswer = _questions[_currentQuestionIndex].correct_answer;

        if (selectedAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
        {
            _correctAnswers++;
        }

        _currentQuestionIndex++;

        if (_currentQuestionIndex >= _totalQuestions)
        {
            ShowResults();
        }
        else
        {
            DisplayCurrentQuestion();
        }
    }

    private void ShowResults()
    {
        string resultMessage = $"You got {_correctAnswers} out of {_totalQuestions} correct!";

        var resultLabel = new Label
        {
            Text = resultMessage,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            FontSize = 20,
            Margin = 100
        };

        var backButton = new Button
        {
            Text = "Back to Start",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = 10
        };

        backButton.Clicked += OnBackButtonClicked;

        this.Content = new StackLayout
        {
            Children = { resultLabel, backButton }
        };
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QuickSettingsPage());
    }

    // Save the current game data
    private void SaveGame(int score, int round)
    {
        var gameData = new
        {
            Score = score,
            CurrentRound = round,
            SelectedCategory,
            SelectedDifficulty,
            SelectedType,
            SelectedNumPlayers,
            SelectedNumQuestions
        };

        string gameDataJson = JsonSerializer.Serialize(gameData);
        Preferences.Set(SavedGameKey, gameDataJson);
        Debug.WriteLine("Game saved successfully.");
    }

    private async void OnSaveGameButtonClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new History());
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error navigating to history: {ex.Message}");
            await DisplayAlert("Error", "Failed to navigate to History.", "OK");
        }
        // Save game data with current status
        var gameData = new
        {
            Score = _correctAnswers,  // Replace with your actual score
            CurrentRound = _currentQuestionIndex,  // Replace with your round status
            SelectedCategory,
            SelectedDifficulty,
            SelectedType,
            SelectedNumPlayers,
            SelectedNumQuestions
        };

        // Serialize the game data to JSON
        string gameDataJson = JsonSerializer.Serialize(gameData);

        // Save the serialized game data to Preferences
        Preferences.Set(SavedGameKey, gameDataJson);

        // Optionally, display a message to confirm the game has been saved
        var parentPage = this.Parent as Page; // Get the parent page
        if (parentPage != null)
        {
            await parentPage.DisplayAlert("Game Saved", "Your game has been saved successfully.", "OK");
        }
    }

    private async Task DisplayAlert(string v1, string v2, string v3)
    {
        throw new NotImplementedException();
    }
}
