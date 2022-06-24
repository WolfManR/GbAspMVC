using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;

var host = Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder =>
{
    builder.ConfigureServices(services => services.AddSingleton<EntryGenerator>());

    builder.Configure(app =>
    {
        app.UseRouting();
        app.UseEndpoints(e =>
        {
            var entryGenerator = e.ServiceProvider.GetRequiredService<EntryGenerator>();

            e.MapGet("/entries/{count:int}", async context =>
            {
                var input = context.Request.RouteValues["count"]?.ToString();
                if (input is null || !int.TryParse(input, out var count))
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                await context.Response.WriteAsJsonAsync(entryGenerator.Generate(count));
            });
        });
    });
});

await host.Build().RunAsync();

internal record Entry(int Id, string Word, int Value);

internal class EntryGenerator
{
    private static readonly Random Random = new Random();
    private static int _lastIndex;
    private static int GetIndex => _lastIndex++;
    private static readonly string[] Words = new[] { "Product", "Automation", "Semaphore", "Sky", "Weather", "Program" };

    public IEnumerable<Entry> Generate(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new Entry(GetIndex, Words[Random.Next(Words.Length)], Random.Next()); 
        }
    }
}