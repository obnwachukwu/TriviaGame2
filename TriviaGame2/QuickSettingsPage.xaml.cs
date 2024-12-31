namespace TriviaGame2;

public partial class QuickSettingsPage : ContentPage

{
    int numOfQuestions;
    public QuickSettingsPage()
    {
        InitializeComponent();
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        // Handle settings button logic
    }

    private async void OnChooseNumQuestionsClicked(object sender, EventArgs e)
    {
        string numQuestion = await DisplayPromptAsync("Enter number of Questions", "Enter number:");
        int.TryParse(numQuestion, out int numQuestions);
        numOfQuestions = numQuestions;
    }

    private async void OnStartGameButtonClicked(object sender, EventArgs e)
    {
        // Example logic for starting the game
        await DisplayAlert("Info", "Game is starting!", "OK");
        // Navigate to another page if needed
        await Navigation.PushAsync(new BeginToPlay(numPlayers.SelectedIndex, difficulty.SelectedIndex, numOfQuestions, questionType.SelectedIndex));
    }
}
