var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
