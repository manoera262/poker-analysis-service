using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Azure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Azure.AI.OpenAI;
using Newtonsoft.Json;

namespace backend
{
    public class ChatGptService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _openAiEndpoint;
        private readonly string _apiKey;

        public ChatGptService(IOptions<OpenAiConfig> config, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _openAiEndpoint = config.Value.Endpoint;
            _apiKey = config.Value.ApiKey;
        }

        public async Task<string> AskQuestionAsync(backend.Game gameData, string userQuery)
        {
           
            OpenAIClient client = new OpenAIClient(
                new Uri(_openAiEndpoint),
                new AzureKeyCredential(_apiKey));


            
            string gameDataString = JsonConvert.SerializeObject(gameData,Formatting.Indented);
         
            string prompt = "Game Data: " + gameDataString + "\n\nUser Question: " + userQuery + "\n\nAnswer";

            // Ensure the prompt does not exceed token limits (this is a simple length check, but consider token count for accuracy)
            if (prompt.Length > 2100)  // this is just an example threshold, adjust as needed
            {
                return "Error: Game data is too long to process.";
            }

            Response<Completions> completionsResponse = await client.GetCompletionsAsync(
                deploymentOrModelName: "gpt35model",
                new CompletionsOptions()
                {
                    Prompts = {prompt},
                    Temperature = (float)0,
                    MaxTokens = 100,
                    NucleusSamplingFactor = (float)1,
                    FrequencyPenalty = (float)0,
                    PresencePenalty = (float)0,
                });

            Completions completions = completionsResponse.Value;

            string result = completions.Choices.First().Text;
            if (result.Contains("User Question:"))
            {
                result = result.Split("User Question:")[0].Trim();
            }
            return result;
        }

    }
}
