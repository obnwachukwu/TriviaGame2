namespace TriviaGame2;

public partial class TFContent : ContentView
{
    private string questionText;

    public TFContent(string questionText)
    {
        InitializeComponent();

        this.QuestionText = questionText;
        BindingContext = this; 
    }

    public string QuestionText
    {
        get { return questionText; }
        set
        {
            questionText = value;
            OnPropertyChanged(nameof(QuestionText)); 
        }
    }

    private void OnTrueButtonClicked(object sender, EventArgs e)
    {
    
    }

    private void OnFalseButtonClicked(object sender, EventArgs e)
    {
 
    }
}
