using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;

namespace TriviaGame2
{
    public partial class BeginToPlay : ContentPage
    {
        private const string ApiUrl = "https://opentdb.com/api.php";  // URL of the trivia API

        public BeginToPlay(int numOfPlayers, int difficulty, int numOfQuestions, int type)
        {
            InitializeComponent();

            // Call the method to fetch questions
            FetchQuestions(numOfPlayers, difficulty, numOfQuestions, type);
        }

        // Fetch questions from the API
        private async Task FetchQuestions(int numOfPlayers, int difficulty, int numOfQuestions, int type)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Construct the API request URL based on the parameters
                    string apiUrlWithParams = $"{ApiUrl}?amount={numOfQuestions}&difficulty={difficulty}&type={type}";

                    // Send the GET request to the trivia API
                    HttpResponseMessage response = await client.GetAsync(apiUrlWithParams);
                    response.EnsureSuccessStatusCode();

                    // Read the response content
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response to extract the questions
                    JObject jsonData = JObject.Parse(jsonResponse);

                    // Example: Loop through the questions and print them (or use in your game logic)
                    foreach (var question in jsonData["results"])
                    {
                        string questionText = question["question"].ToString();
                        string correctAnswer = question["correct_answer"].ToString();
                        Console.WriteLine($"Question: {questionText}");
                        Console.WriteLine($"Answer: {correctAnswer}");

                        // You can process the questions here as needed for your game
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching trivia questions: {ex.Message}");
            }
        }
    }
}
