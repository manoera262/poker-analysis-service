var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.Configure<backend.CosmosDbConfig>(builder.Configuration.GetSection("CosmosDb"));
builder.Services.Configure<backend.OpenAiConfig>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddSingleton<backend.ChatGptService>();
builder.Services.AddSingleton<CosmosDbService>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000") // Default port for React
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS middleware
app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
