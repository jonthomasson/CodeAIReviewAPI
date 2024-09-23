using Azure;
using Azure.AI.OpenAI;
using CodeAIReviewAPI.Models;
using CodeAIReviewAPI.Resources;
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
            string system_prompt = AIPrompts.code_review_system_prompt;

            // Create the OpenAI client
            AzureKeyCredential credential = new AzureKeyCredential(key);
            AzureOpenAIClient azureClient = new(new Uri(endpoint), credential);
            ChatClient chatClient = azureClient.GetChatClient("gpt4");

            ChatCompletion completion = chatClient.CompleteChat(
                [
                    // System messages represent instructions or other guidance about how the assistant should behave
                    new SystemChatMessage(system_prompt),
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
