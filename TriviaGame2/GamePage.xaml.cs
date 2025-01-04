namespace TriviaGame2
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            BindingContext = new GameController(new ApiService()); // Set the ViewModel

            // Fetch data when the page is loaded
            ((GameController)BindingContext).GetQuestionFromApi();
        }
    }
}