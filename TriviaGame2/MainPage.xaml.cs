using System;
using Microsoft.Maui.Controls;

namespace TriviaGame2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Event handler for the Settings button
        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the settings page or show a settings dialog
            await DisplayAlert("Settings", "Settings functionality not implemented yet.", "OK");
        }

        // Event handler for choosing the number of questions
        private async void OnChooseNumQuestionsClicked(object sender, EventArgs e)
        {
            // Here you can implement logic to choose the number of questions
            // For example, you could show a dialog to select the number of questions
            string result = await DisplayPromptAsync("Number of Questions", "Enter the number of questions:");
            if (int.TryParse(result, out int numQuestions) && numQuestions > 0)
            {
                // Store the number of questions or use it to start the game
                await DisplayAlert("Success", $"You have chosen {numQuestions} questions.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid number of questions.", "OK");
            }
        }

        private async void OnChooseNumQuestion(object sender, EventArgs e)
        {

        }

        private async void OnStartGameClicked(object sender, EventArgs e)
        {
            // Navigate to QuickSettingsPage
            await Navigation.PushAsync(new QuickSettingsPage());
        }

    }
}