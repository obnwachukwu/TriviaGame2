namespace TriviaGame2;

public partial class TFContent : ContentView
{
    public TFContent()
    {
        InitializeComponent();

        var gameController = new GameController(new ApiService());
        BindingContext = gameController;

        LoadQuestion(gameController);
    }

    private async void LoadQuestion(GameController gameController)
    {
        await gameController.GetQuestionFromApi();
    }

    private void OnTrueButtonClicked(object sender, EventArgs e)
    {
    
    }

    private void OnFalseButtonClicked(object sender, EventArgs e)
    {
 
    }
}
