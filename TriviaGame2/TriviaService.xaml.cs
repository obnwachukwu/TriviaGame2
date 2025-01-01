using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TriviaGame2
{
    public partial class TriviaService
    {
        private const string BaseUrl = "https://opentdb.com/api.php";

        public async Task<List<TriviaQuestion>> GetTriviaQuestionsAsync(TriviaRequest triviaRequest)
        {
            using (var client = new HttpClient())
            {
                // Construct the query parameters
                var url = $"{BaseUrl}?amount={triviaRequest.Amount}" +
                          $"&category={triviaRequest.Category}" +
                          $"&difficulty={triviaRequest.Difficulty}" +
                          $"&type={triviaRequest.Type}" +
                          $"&encode={triviaRequest.Encoding}";

                try
                {
                    // Make the GET request to the API
                    var response = await client.GetStringAsync(url);

                    // Deserialize the response into a list of trivia questions
                    var triviaResponse = JsonConvert.DeserializeObject<TriviaApiResponse>(response);

                    // Return the list of questions
                    return triviaResponse.Results;
                }
                catch (Exception ex)
                {
                    // Handle errors (e.g., no internet, API failure)
                    Console.WriteLine("Error fetching trivia questions: " + ex.Message);
                    return null;
                }
            }
        }
    }

    // Represents a response from the Open Trivia Database API
    public class TriviaApiResponse
    {
        public List<TriviaQuestion> Results { get; set; }
    }

    // Represents a trivia question
    public class TriviaQuestion
    {
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }
    }
}
