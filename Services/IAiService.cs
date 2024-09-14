using CodeAIReviewAPI.Models;

namespace CodeAIReviewAPI.Services
{
    public interface IAiService
    {
        Task<AiCodeReviewResult> ReviewCodeChanges(List<FileChange> fileChanges);
    }
}