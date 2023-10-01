using backend;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

public class CosmosDbService
{
    private Container _container;

    public CosmosDbService(
        IOptions<CosmosDbConfig> cosmosDbConfig)
    {
        var client = new CosmosClient(cosmosDbConfig.Value.EndpointUrl, cosmosDbConfig.Value.PrimaryKey);
        var database = client.GetDatabase(cosmosDbConfig.Value.DatabaseName);
        _container = database.GetContainer(cosmosDbConfig.Value.GameContainerName);
    }

    public async Task AddItemAsync(backend.Game game)
    {
        await _container.CreateItemAsync(game, new PartitionKey(game.gameid.ToString()));
    }

    public async Task<backend.Game> GetItemAsync(Guid id)
    {
        try
        {
            ItemResponse<backend.Game> response = await _container.ReadItemAsync<backend.Game>(id.ToString(), new PartitionKey(id.ToString()));
            return response.Resource;   
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }
}
