namespace TriviaGame2
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            BindingContext = new GameController(new ApiService());
        }
    }
}