using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TriviaGame2
{
    public partial class BeginToPlay
    {
        private readonly TriviaService _triviaService;

        public BeginToPlay(int numPlayers, int difficulty, int numOfQuestions, int questionType)
        {
            _triviaService = new TriviaService();
            InitializeComponent();
        }

        public async Task StartGameAsync()
        {
            var triviaRequest = new TriviaRequest
            {
                Amount = 10, // Number of questions the user wants
                Category = "9", // For example, "9" for General Knowledge
                Difficulty = "easy", // Difficulty level: easy, medium, or hard
                Type = "multiple", // "multiple" for multiple-choice questions
                Encoding = "url3986" // Encoding format (default, url3986, etc.)
            };

            // Fetch trivia questions from the API
            var triviaQuestions = await _triviaService.GetTriviaQuestionsAsync(triviaRequest);

            if (triviaQuestions != null && triviaQuestions.Any())
            {
                // Start the quiz game with the retrieved questions
                foreach (var question in triviaQuestions)
                {
                    // Show the question and options (use your game's UI logic here)
                    Console.WriteLine($"Question: {question.Question}");
                    var options = new List<string>(question.IncorrectAnswers) { question.CorrectAnswer };
                    options = options.OrderBy(o => Guid.NewGuid()).ToList(); // Randomize the options

                    foreach (var option in options)
                    {
                        Console.WriteLine($"- {option}");
                    }

                    // Wait for user input to answer the question
                    Console.WriteLine("Enter your answer: ");
                    var userAnswer = Console.ReadLine();

                    if (userAnswer?.Trim().ToLower() == question.CorrectAnswer.ToLower())
                    {
                        Console.WriteLine("Correct!");
                    }
                    else
                    {
                        Console.WriteLine($"Incorrect! The correct answer was: {question.CorrectAnswer}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No trivia questions could be fetched.");
            }
        }
    }
}
