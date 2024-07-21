
using MassTransit;
using SearchService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Make conncetion to RabbitMQ and configure endpoints
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
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
