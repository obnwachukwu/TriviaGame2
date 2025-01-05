
namespace TriviaGame2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Settings());
        }

        private async void OnHistoryButtonClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new History());
        }

        private async void OnChooseNumQuestionsClicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Number of Questions", "Enter the number of questions:");
            if (int.TryParse(result, out int numQuestions) && numQuestions > 0)
            {
                await DisplayAlert("Success", $"You have chosen {numQuestions} questions.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid number of questions.", "OK");
            }
        }

        private async void OnStartGameClicked(object sender, EventArgs e)
        {
            // Navigate to QuickSettingsPage
            await Navigation.PushAsync(new QuickSettingsPage());
        }

    }
}