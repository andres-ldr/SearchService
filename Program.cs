
using MassTransit;
using SearchService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add AutoMapper to the project
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Make conncetion to RabbitMQ and configure endpoints
builder.Services.AddMassTransit(x =>
{
    // Add consumers from the namespace containing AuctionCreatedConsumer
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    // Set the endpoint name formatter to kebab case
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ReceiveEndpoint("search-auction-created", e =>
        {
            // Configure the retry policy for the consumer
            e.UseMessageRetry(r => r.Interval(5, 5));
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

try
{
    await DBInitializer.InitDB(app);
}
catch (Exception e)
{

    Console.WriteLine(e);
}


app.Run();
