namespace TriviaGame2;

public partial class MCQContent : ContentView
{
    private List<Result> _questions;
    private int _currentQuestionIndex = 0;
    private int _correctAnswers = 0;
    private int _totalQuestions;

    public MCQContent(List<Result> questions)
    {
        InitializeComponent();

        _questions = questions;
        _totalQuestions = questions.Count;
        BindingContext = this;

        DisplayCurrentQuestion();
    }

    public string CurrentQuestion
    {
        get { return _questions[_currentQuestionIndex].question; }
    }

    public string OptionA => _questions[_currentQuestionIndex].incorrect_answers[0];
    public string OptionB => _questions[_currentQuestionIndex].incorrect_answers[1];
    public string OptionC => _questions[_currentQuestionIndex].incorrect_answers[2];
    public string OptionD => _questions[_currentQuestionIndex].correct_answer;

    private void DisplayCurrentQuestion()
    {
        // Shuffle the options to randomize their order
        var options = new List<string>
        {
            OptionA,
            OptionB,
            OptionC,
            OptionD
        }.OrderBy(_ => Guid.NewGuid()).ToList();

        ButtonOptionA.Text = options[0];
        ButtonOptionB.Text = options[1];
        ButtonOptionC.Text = options[2];
        ButtonOptionD.Text = options[3];

        OnPropertyChanged(nameof(CurrentQuestion));
    }

    private void OnOptionAClicked(object sender, EventArgs e) => CheckAnswer(ButtonOptionA.Text);
    private void OnOptionBClicked(object sender, EventArgs e) => CheckAnswer(ButtonOptionB.Text);
    private void OnOptionCClicked(object sender, EventArgs e) => CheckAnswer(ButtonOptionC.Text);
    private void OnOptionDClicked(object sender, EventArgs e) => CheckAnswer(ButtonOptionD.Text);

    private void CheckAnswer(string selectedAnswer)
    {
        var correctAnswer = _questions[_currentQuestionIndex].correct_answer;

        if (selectedAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
        {
            _correctAnswers++;
        }

        _currentQuestionIndex++;

        if (_currentQuestionIndex >= _totalQuestions)
        {
            ShowResults();
        }
        else
        {
            DisplayCurrentQuestion();
        }
    }

    private void ShowResults()
    {
        string resultMessage = $"You got {_correctAnswers} out of {_totalQuestions} correct!";

        var resultLabel = new Label
        {
            Text = resultMessage,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            FontSize = 20,
            Margin = 100
        };

        var backButton = new Button
        {
            Text = "Back to Start",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = 10
        };

        backButton.Clicked += OnBackButtonClicked;

        this.Content = new StackLayout
        {
            Children = { resultLabel, backButton }
        };
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QuickSettingsPage());
    }
}
