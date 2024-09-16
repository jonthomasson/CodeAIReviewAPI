using Azure;
using Azure.AI.OpenAI;
using CodeAIReviewAPI.Models;
using OpenAI.Chat;
using System.Text.Json;

namespace CodeAIReviewAPI.Services
{
    public class AiService : IAiService
    {
        private readonly IConfiguration _configuration;
        public AiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<AiCodeReviewResult> ReviewCodeChanges(List<FileChange> fileChanges)
        {
            // Get environment variables for OpenAI
            string endpoint = _configuration["AzureAI:Endpoint"];
            string key = _configuration["AzureAI:Key"];

            // Create the OpenAI client
            AzureKeyCredential credential = new AzureKeyCredential(key);
            AzureOpenAIClient azureClient = new(new Uri(endpoint), credential);
            ChatClient chatClient = azureClient.GetChatClient("gpt4");

            ChatCompletion completion = chatClient.CompleteChat(
                [
                    // System messages represent instructions or other guidance about how the assistant should behave
                    new SystemChatMessage("You're an AI code reviewer. You get pull request file changes and you reply with helpful code suggestions if appropriate. If the code is C#, you should use common C# code conventions as explained here: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions. If the code is Angular, use the Angular style guide. If a different framework, try to determine the framework and use the appropriate style guide and standards. You will receive the file changes in a JSON packet like this [{\"fileName\": \"GitTestMerge/Services/Service1.cs\", \"patch\": \"code updates and deletions\"}]. After processing the data, return the results as a JSON packet like this {\"standards\": \"comma separated list with one or more standards applied to the code (example Angular Style Guide, C# Style Guide etc).\", \"score\": \"score is a number from 1 to 10. 1 would be poor quality code, 10 would be exceptional code quality.\", \"codeReview\":[{\"fileName\": \"fileName\", \"review\": \"string will contain the code suggestions, if any. Can be left empty if there are no suggestions.\"}]}"),
                    new UserChatMessage(JsonSerializer.Serialize(fileChanges)),
                ],
                 new ChatCompletionOptions()
                 {
                     Temperature = (float)0.7,
                     MaxTokens = 4096,
                     FrequencyPenalty = (float)0,
                     PresencePenalty = (float)0,
                 }
                );

            var response = completion.Content[0].Text;
            // Trim from the first '{' to the last '}' to ensure valid JSON format
            int startIndex = response.IndexOf('{');
            int endIndex = response.LastIndexOf('}');
            if (startIndex != -1 && endIndex != -1)
            {
                response = response.Substring(startIndex, endIndex - startIndex + 1);
            }
            if (response == null)
            {
                throw new Exception("Error processessing response");
            }

            return JsonSerializer.Deserialize<AiCodeReviewResult>(response);
        }
    }
}
