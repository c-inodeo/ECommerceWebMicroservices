using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok("Ocelot Gateway is healthy"));
await app.UseOcelot();


app.Run();

