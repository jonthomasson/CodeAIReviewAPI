namespace CodeAIReviewAPI.Models
{
    public class AiCodeReviewResult
    {
        public string? Standards { get; set; }
        public int? Score { get; set; }
        public List<AiCodeReviewFile> CodeReview { get; set; }
    }

    public class AiCodeReviewFile
    {
        public string? FileName { get; set; }
        public string? Review { get; set; }

    }
}
