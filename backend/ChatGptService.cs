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
           // Console.WriteLine(gameDataString);
           // string preprompt = """{"gameid":"550e8400-e29b-41d4-a716-446655440010","id":"550e8400-e29b-41d4-a716-446655440010","Players":[{"PlayerId":"110e8400-e29b-41d4-a716-446655440001","Name":"John","ChipCount":5000.0},{"PlayerId":"120e8400-e29b-41d4-a716-446655440002","Name":"Jane","ChipCount":5000.0},{"PlayerId":"130e8400-e29b-41d4-a716-446655440003","Name":"Robert","ChipCount":5000.0},{"PlayerId":"140e8400-e29b-41d4-a716-446655440004","Name":"Emily","ChipCount":5000.0},{"PlayerId":"150e8400-e29b-41d4-a716-446655440005","Name":"Michael","ChipCount":5000.0},{"PlayerId":"160e8400-e29b-41d4-a716-446655440006","Name":"Sara","ChipCount":5000.0}],"Table":{"CommunityCards":[{"Rank":"3","Suit":"Hearts"},{"Rank":"5","Suit":"Hearts"},{"Rank":"2","Suit":"Spades"}]},"PotAmount":100.0,"HoleCards":[{"Rank":"Ace","Suit":"Hearts"},{"Rank":"10","Suit":"Clubs"}],"Status":"ongoing"}""";
           // preprompt = preprompt + "\n\nUser Question: what is the probability of me getting the pot odds if I bet 100  and what is the probability of me hitting a flush draw at showdown" +"\n\nAnswer : The pot odd is 50% as 100 divided by 100+100  and as two card left to be drawan and 10 hearts which can come up hence 40% chances of hitting flush draw";
            // Create the prompt
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
