namespace TriviaGame2;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
        timerSlider.ValueChanged += OnTimerSliderValueChanged;
    }

    private void OnTimerSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        timerLabel.Text = $"Timer: {Math.Round(e.NewValue)} seconds";
    }

    private async void OnClearHistoryClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm", "Are you sure you want to clear your history?", "Yes", "No");
        if (confirm)
        {
            // Add logic to clear history here
            await DisplayAlert("History Cleared", "Your game history has been cleared.", "OK");
        }
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        string selectedTheme = themePicker.SelectedItem?.ToString();

        if (selectedTheme == "Light")
        {
            Application.Current.UserAppTheme = AppTheme.Light;
        }
        else if (selectedTheme == "Dark")
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
        }

        // No need to navigate; the UI will update automatically
    }


    private void OnBackButton(object sender, EventArgs e)
    {
        // Handle navigation logic
        if (Navigation.NavigationStack.Count > 1)
        {
            Navigation.PopAsync();
        }
        else
        {
            DisplayAlert("Notice", "No previous page available.", "OK");
        }
    }
}
