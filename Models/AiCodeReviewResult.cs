using System.Text.Json.Serialization;

namespace CodeAIReviewAPI.Models
{
    public class AiCodeReviewResult
    {
        [JsonPropertyName("standards")]
        public string? Standards { get; set; }
        [JsonPropertyName("score")]
        public int? Score { get; set; }
        [JsonPropertyName("codeReview")]
        public AiCodeReviewFile[]? CodeReview { get; set; }
    }

    public class AiCodeReviewFile
    {
        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }
        [JsonPropertyName("review")]
        public string? Review { get; set; }

    }
}
