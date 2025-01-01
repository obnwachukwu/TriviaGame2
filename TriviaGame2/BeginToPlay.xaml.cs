using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace TriviaGame2
{
    public partial class BeginToPlay : ContentPage
    {
        private readonly TriviaService _triviaService;
        private List<TriviaQuestion> _triviaQuestions;
        private int _currentQuestionIndex;

        public BeginToPlay(int numPlayers, int difficulty, int numOfQuestions, int questionType)
        {
            _triviaService = new TriviaService();
            InitializeComponent();
            _currentQuestionIndex = 0;

            // Prepare the trivia request
            var triviaRequest = new TriviaRequest
            {
                Amount = numOfQuestions, // User's choice
                Category = "9", // General Knowledge category (you can update this)
                Difficulty = "easy", // Difficulty level: easy, medium, hard
                Type = "multiple", // For multiple choice questions
                Encoding = "url3986" // URL-encoded answers
            };

            // Fetch trivia questions
            FetchQuestions(triviaRequest);
        }

        // Fetch trivia questions and display the first question
        private async void FetchQuestions(TriviaRequest triviaRequest)
        {
            _triviaQuestions = await _triviaService.GetTriviaQuestionsAsync(triviaRequest);

            if (_triviaQuestions.Any())
            {
                // Show the first question
                DisplayCurrentQuestion();
            }
            else
            {
                // Handle the case where no questions were fetched
                questionLabel.Text = "No trivia questions found.";
            }
        }

        // Display the current question and its options
        private void DisplayCurrentQuestion()
        {
            var currentQuestion = _triviaQuestions[_currentQuestionIndex];

            // Set the question text
            questionLabel.Text = currentQuestion.Question;

            // Clear the existing options
            optionsStackLayout.Children.Clear();

            // Shuffle the answer options (correct answer + incorrect ones)
            var options = new List<string>(currentQuestion.IncorrectAnswers) { currentQuestion.CorrectAnswer };
            options = options.OrderBy(o => Guid.NewGuid()).ToList(); // Shuffle the answers

            // Display the options as buttons
            foreach (var option in options)
            {
                var optionButton = new Button
                {
                    Text = option,
                    HorizontalOptions = LayoutOptions.Center
                };
                optionButton.Clicked += OnOptionButtonClicked;
                optionsStackLayout.Children.Add(optionButton);
            }

            // Show the Next button
            nextButton.IsVisible = false; // Hide next button initially
        }

        // Handle the option button click
        private void OnOptionButtonClicked(object sender, EventArgs e)
        {
            var selectedButton = (Button)sender;
            var selectedAnswer = selectedButton.Text;

            // Check if the answer is correct
            var correctAnswer = _triviaQuestions[_currentQuestionIndex].CorrectAnswer;

            if (selectedAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
            {
                DisplayAlert("Correct!", "You chose the correct answer!", "OK");
            }
            else
            {
                DisplayAlert("Incorrect", $"The correct answer was: {correctAnswer}", "OK");
            }

            // Show the Next button after answering
            nextButton.IsVisible = true;
        }

        // Handle Next button click to go to the next question
        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            _currentQuestionIndex++;

            // Check if there are more questions
            if (_currentQuestionIndex < _triviaQuestions.Count)
            {
                // Display the next question
                DisplayCurrentQuestion();
            }
            else
            {
                // If there are no more questions, display a message
                DisplayAlert("Quiz Finished", "You have completed the quiz!", "OK");
                // Optionally, you could navigate back or reset the game
            }
        }
    }
}
