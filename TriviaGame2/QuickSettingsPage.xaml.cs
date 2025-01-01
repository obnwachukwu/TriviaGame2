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
        // Prompt the user to enter a number of questions
        string numQuestion = await DisplayPromptAsync("Enter number of Questions: (5, 10, 15, or 20)", "Enter number:");

        // Try to parse the input to an integer
        int.TryParse(numQuestion, out int numQuestions);

        // Validate the input
        if (numQuestions == 5 || numQuestions == 10 || numQuestions == 15 || numQuestions == 20)
        {
            // Valid input, store the number of questions
            numOfQuestions = numQuestions;
        }
        else
        {
            // Invalid input, show a warning message
            await DisplayAlert("Invalid Input", "Please enter a valid number: 5, 10, 15, or 20.", "OK");
        }
    }

    private async void OnStartGameButtonClicked(object sender, EventArgs e)
    {
        // Example logic for starting the game
        await DisplayAlert("Info", "Game is starting!", "OK");
        // Navigate to another page if needed
        await Navigation.PushAsync(new BeginToPlay(numPlayers.SelectedIndex, difficulty.SelectedIndex, numOfQuestions, questionType.SelectedIndex));
    }
}
