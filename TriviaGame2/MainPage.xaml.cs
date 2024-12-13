namespace TriviaGame2
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        // Event handler for when the Start Game button is clicked
        private void OnStartGameClicked(object sender, EventArgs e)
        {
            string selectedDifficulty = difficulty.SelectedItem?.ToString();
            string selectedQuestionType = questionType.SelectedItem?.ToString();
            string selectedPlayers = numPlayers.SelectedItem?.ToString();

            // Example of handling the selected values
            if (string.IsNullOrEmpty(selectedDifficulty) || string.IsNullOrEmpty(selectedQuestionType) || string.IsNullOrEmpty(selectedPlayers))
            {
                // Handle the case where a selection is missing (e.g., show an alert)
                DisplayAlert("Error", "Please select all options", "OK");
            }
            else
            {
                // Proceed with starting the game based on selected options
                // Example: Navigate to another page or set game parameters
                DisplayAlert("Game Starting",
                    $"Difficulty: {selectedDifficulty}\nQuestion Type: {selectedQuestionType}\nPlayers: {selectedPlayers}",
                    "OK");
            }
        }

        private async void OnChooseNumQuestionsClicked(object sender, EventArgs e)
        {
            // Display an Entry prompt to the user asking how many questions they want
            var result = await DisplayPromptAsync("Number of Questions", "Enter the number of questions you want:",
                initialValue: "10",
                maxLength: 2,
                keyboard: Keyboard.Numeric);

            // Parse the result to an integer, ensure it's valid
            if (int.TryParse(result, out int number) && number > 0)
            {
                int numQuestions = number; // Update the number of questions
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid number of questions.", "OK");
            }
        }
    }

}
