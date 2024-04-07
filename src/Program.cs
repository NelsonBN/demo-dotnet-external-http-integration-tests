using Demo.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .ConfigureOptions<JsonPlaceholderOptions.Setup>()
    .AddHttpClient<JsonPlaceholderService>((sp, client) =>
    {
        var options = sp.GetRequiredService<IOptions<JsonPlaceholderOptions>>().Value;
        client.BaseAddress = new(options.Url);
    });

var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI();

app.MapProductsEndpoints();

app.Run();


public partial class Program;
