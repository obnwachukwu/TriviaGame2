namespace TriviaGame2
{
    public class ApiResponse
    {
        public int ResponseCode { get; set; }
        public List<QuestionResult> Results { get; set; }
    }

    public class QuestionResult
    {
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }
    }
}