namespace TriviaGame2
{
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

        public QuickSettingsPage()
        {
            InitializeComponent();

            CategoryOptions = new List<string>
            {
                "General Knowledge",
                "Sports",
                "Science: Computers",
                "Entertainment: Film",
                "Celebrities",
                "Entertainment: Japanese Anime & Manga",
                "Vehicles"
            };

            DifficultyOptions = new List<string> { "Easy", "Medium", "Hard" };
            TypeOptions = new List<string> { "MCQ", "True or False" };
            NumPlayersOptions = new List<string> { "1", "2", "3", "4" };
            NumQuestionsOptions = new List<string>(Enumerable.Range(1, 50).Select(n => n.ToString()).ToList());

            BindingContext = this;
        }
        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            // Handle settings button logic
            await Navigation.PushAsync(new Settings());
        }
        private void OnCategorySelected(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            SelectedCategory = (string)picker.SelectedItem;
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

        private async void OnStartGameButtonClicked(object sender, EventArgs e)
        {
            var gameController = new GameController(new ApiService());
            // Push the GamePage with the GameController as its BindingContext
            var gamePage = new GamePage
            {
                BindingContext = gameController
            };

            // Navigate to the GamePage
            await Navigation.PushAsync(gamePage);

            // Get the questions from API
            await gameController.GetQuestionFromApi();
        }
    }
}
