using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserQueryController : ControllerBase
    {
        private readonly CosmosDbService _cosmosDbService;
        private readonly ChatGptService _chatGptService;

        public UserQueryController(CosmosDbService cosmosDbService, ChatGptService chatGptService)
        {
            _cosmosDbService = cosmosDbService;
            _chatGptService = chatGptService;
        }

        [HttpPost]
        public string GetUserResponse([FromBody] UserQueryInput input)
        {
            var gameData =  _cosmosDbService.GetItemAsync(input.GameId).Result;
            if (gameData == null)
                return "Game data not found.";

            var response =  _chatGptService.AskQuestionAsync(gameData, input.Query).Result;

            return response.ToString();
        }
    }

    public class UserQueryInput
    {
        public Guid GameId { get; set; }
        public string Query { get; set; }
    }
}
