using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/foods", async (HttpClient httpClient) =>
{
	var response = await httpClient.GetAsync($"https://random-mat.azurewebsites.net/foods");
	if (!response.IsSuccessStatusCode)
		Console.WriteLine(response);

	var data = await response.Content.ReadAsStringAsync();
    var json = JsonConvert.DeserializeObject<Foods[]>(data);

	return Results.Ok(json);
});

app.MapGet("/foodsbyid/{id}", async (HttpClient httpClient, int id) =>
{
    var response = await httpClient.GetAsync($"https://random-mat.azurewebsites.net/foodsbyid/{id}");
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine("There was an error");
        return Results.NotFound();
    }
	Console.WriteLine(response.StatusCode);

	var data = await response.Content.ReadAsStringAsync();
	if (data.Equals("Ingen mat med det ID:t"))
	{
		return Results.Ok("Ingen mat med det ID:t");
	}
	var json = JsonConvert.DeserializeObject<Foods>(data);

	return Results.Ok(json);
});

app.MapPost("/newEntry", async (HttpClient httpClient) =>
{
    var response = await httpClient.GetAsync($"https://random-mat.azurewebsites.net/newentry");
    if (!response.IsSuccessStatusCode)
        Console.WriteLine(response);
	var data = await response.Content.ReadAsStringAsync();
	var json = JsonConvert.DeserializeObject<Foods>(data);

	return Results.Ok(json);
});

app.MapDelete("/delete/{id}", async (HttpClient httpClient, int id) =>
{
    var response = await httpClient.GetAsync($"https://random-mat.azurewebsites.net/delete/{id}");
    if (!response.IsSuccessStatusCode)
        Console.WriteLine("There was an error");
	var data = await response.Content.ReadAsStringAsync();
	if (data.Equals("Ingen mat med det ID:t"))
	{
		return Results.Ok("Ingen mat med det ID:t");
	}
	var json = JsonConvert.DeserializeObject<Foods[]>(data);

	return Results.Ok(json);
});

app.Run();



public class Foods
{

	public int id { get; set; }

	public string name { get; set; }

	public string drink { get; set; }

	public string type { get; set; }



}