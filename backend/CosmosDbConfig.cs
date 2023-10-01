namespace backend
{
    public class CosmosDbConfig
    {
        public string EndpointUrl { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseName { get; set; }
        public string GameContainerName { get; set; }
    }

     public class  OpenAiConfig
    {
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
    }
}
