using System.Diagnostics;
using System.Text.Json;

namespace TriviaGame2;

public partial class History : ContentPage
{
    private const string SavedGameKey = "SavedGame";

    public History()
    {
        InitializeComponent();
        LoadSavedGameDetails();
    }

    private void LoadSavedGameDetails()
    {
        try
        {
            // Check if a saved game exists
            if (Preferences.ContainsKey(SavedGameKey))
            {
                string gameDataJson = Preferences.Get(SavedGameKey, null);

                if (!string.IsNullOrEmpty(gameDataJson))
                {
                    // Deserialize saved game data
                    var gameData = JsonSerializer.Deserialize<dynamic>(gameDataJson);

                    // Display saved game details
                    string details = $"Score: {gameData["Score"]}\n" +
                                     $"Round: {gameData["CurrentRound"]}\n" +
                                     $"Category: {gameData["SelectedCategory"]}\n" +
                                     $"Difficulty: {gameData["SelectedDifficulty"]}\n" +
                                     $"Type: {gameData["SelectedType"]}\n" +
                                     $"Players: {gameData["SelectedNumPlayers"]}\n" +
                                     $"Questions: {gameData["SelectedNumQuestions"]}";

                    SavedGameLabel.Text = details;
                    LoadGameButton.IsVisible = true;
                    ClearGameButton.IsVisible = true;
                }
                else
                {
                    SavedGameLabel.Text = "No saved game found.";
                    LoadGameButton.IsVisible = false;
                    ClearGameButton.IsVisible = false;
                }
            }
            else
            {
                SavedGameLabel.Text = "No saved game found.";
                LoadGameButton.IsVisible = false;
                ClearGameButton.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading saved game details: {ex.Message}");
            SavedGameLabel.Text = "Error loading saved game details.";
        }
    }

    private async void OnLoadSavedGameClicked(object sender, EventArgs e)
    {
        try
        {
            if (Preferences.ContainsKey(SavedGameKey))
            {
                string gameDataJson = Preferences.Get(SavedGameKey, null);
                if (!string.IsNullOrEmpty(gameDataJson))
                {
                    var gameData = JsonSerializer.Deserialize<dynamic>(gameDataJson);

                    // Use loaded game data to resume the game
                    await DisplayAlert("Game Loaded", "Your saved game has been loaded.", "OK");

                    // Example: Pass the data to the game page (you can modify this)
                    var gamePage = new GamePage
                    {
                        BindingContext = new
                        {
                            Score = gameData["Score"],
                            CurrentRound = gameData["CurrentRound"],
                            SelectedCategory = gameData["SelectedCategory"],
                            SelectedDifficulty = gameData["SelectedDifficulty"],
                            SelectedType = gameData["SelectedType"],
                            SelectedNumPlayers = gameData["SelectedNumPlayers"],
                            SelectedNumQuestions = gameData["SelectedNumQuestions"]
                        }
                    };

                    await Navigation.PushAsync(gamePage);
                }
                else
                {
                    await DisplayAlert("Error", "No saved game found.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading saved game: {ex.Message}");
            await DisplayAlert("Error", "Failed to load saved game.", "OK");
        }
    }

    private async void OnClearSavedGameClicked(object sender, EventArgs e)
    {
        try
        {
            // Clear saved game data
            Preferences.Remove(SavedGameKey);

            // Update UI
            SavedGameLabel.Text = "No saved game found.";
            LoadGameButton.IsVisible = false;
            ClearGameButton.IsVisible = false;

            await DisplayAlert("Cleared", "Saved game data has been cleared.", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error clearing saved game: {ex.Message}");
            await DisplayAlert("Error", "Failed to clear saved game.", "OK");
        }
    }

}
