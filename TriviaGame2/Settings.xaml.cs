using Microsoft.Maui.Controls;

namespace TriviaGame2
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            themePicker.SelectedIndexChanged += ThemePicker_SelectedIndexChanged;
        }

        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected theme
            var selectedTheme = themePicker.SelectedItem.ToString();

            // Apply the selected theme
            if (selectedTheme == "Light")
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("resource://TriviaGame2.Resources.LightTheme.xaml", UriKind.Absolute) });
            }
            else if (selectedTheme == "Dark")
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("resource://TriviaGame2.Resources.DarkTheme.xaml", UriKind.Absolute) });
            }
        }

        // Navigate back to the previous page
        private async void NavigateBackCommand()
        {
            await Navigation.PopAsync();
        }
    }
}
