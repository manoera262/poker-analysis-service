using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokerGameController : ControllerBase
    {
        private readonly ILogger<PokerGameController> _logger;
        private readonly CosmosDbService _cosmosDbService;

        public PokerGameController(ILogger<PokerGameController> logger, CosmosDbService cosmosDbService)
        {
            _logger = logger;
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("{id}")]
        public async Task<Game> Get(Guid id)
        {
            return await _cosmosDbService.GetItemAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Game game)
        {
           
                await _cosmosDbService.AddItemAsync(game);
                return Ok();         
        }

    }
}
