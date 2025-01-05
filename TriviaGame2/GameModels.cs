using Newtonsoft.Json;

public class QuestionsResponse
{
    //[JsonProperty("response_code")]
    //public int ResponseCode { get; set; }

    //[JsonProperty("results")]
    //public List<Result> Results { get; set; }
    //[JsonProperty("userId")]
    //public int userId { get; set; }

    //[JsonProperty("id")]
    // public string id { get; set; }

    //[JsonProperty("title")]
    // public string title { get; set; }

    // [JsonProperty("completed")]
    //public string completed { get; set; }

    [JsonProperty("response_code")]
    public int ResponseCode { get; set; }

    [JsonProperty("results")]
    public List<Result> results { get; set; }
    public object trivia_categories { get; internal set; }
}

public class Result
{
    [JsonProperty("category")]
    public string category { get; set; }

    [JsonProperty("type")]
    public string type { get; set; }

    [JsonProperty("difficulty")]
    public string difficulty { get; set; }

    [JsonProperty("question")]
    public string question { get; set; }

    [JsonProperty("correct_answer")]
    public string correct_answer { get; set; }

    [JsonProperty("incorrect_answers")]
    public List<string> incorrect_answers { get; set; }
}

public class TriviaCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CategoryResponse
{
    public List<TriviaCategory> Trivia_Categories { get; set; }
}

public class TokenResponse
{
    [JsonProperty("response_code")]
    public int ResponseCode { get; set; }

    [JsonProperty("response_message")]
    public string ResponseMessage { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }
}

public class Player
{
    public string Name { get; set; }
    public int Score { get; set; } = 0;
    public string SelectedCategory { get; set; }
    public string SelectedDifficulty { get; set; }
    public string SelectedType { get; set; }
    public List<Result> Questions { get; set; } = new List<Result>();
}