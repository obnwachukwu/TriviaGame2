
using System.Net;

namespace TriviaGame2;

public partial class TFContent : ContentView
{
    private List<Result> _questions;
    private int _currentQuestionIndex = 0;
    private int _correctAnswers = 0;
    private int _totalQuestions;

    public TFContent(List<Result> questions)
    {
        InitializeComponent();

        _questions = questions;
        _totalQuestions = questions.Count;
        BindingContext = this;

        DisplayCurrentQuestion();
    }

    public string CurrentQuestion
    {
        get { return WebUtility.HtmlDecode(_questions[_currentQuestionIndex].question); }
    }

    private void DisplayCurrentQuestion()
    {
        OnPropertyChanged(nameof(CurrentQuestion));
    }

    private void OnTrueButtonClicked(object sender, EventArgs e)
    {
        CheckAnswer("True");
    }

    private void OnFalseButtonClicked(object sender, EventArgs e)
    {
        CheckAnswer("False");
    }

    private void CheckAnswer(string userAnswer)
    {
        var correctAnswer = _questions[_currentQuestionIndex].correct_answer;

        if (userAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
        {
            _correctAnswers++;
        }

        // Move to the next question or show results if last question
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

        this.Content = resultLabel;

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
