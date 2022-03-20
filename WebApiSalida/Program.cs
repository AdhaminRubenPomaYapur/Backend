using Newtonsoft.Json;
using WebApiSalida;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("/get", async () =>
{
    string message = await Cola.Receive();

    var result = JsonConvert.DeserializeObject<Response<List<Auto>>>(message);
    return Results.Ok(result);
});

app.MapGet("/post", async () =>
{
    string message = await Cola.Receive();

    var result = JsonConvert.DeserializeObject<Response<Auto>>(message);
    return Results.Ok(result);
});
app.MapGet("/put", async () =>
{
    string message = await Cola.Receive();

    var result = JsonConvert.DeserializeObject<Response<Auto>>(message);
    return Results.Ok(result);
});
app.MapGet("/delete", async () =>
{
    string message = await Cola.Receive();

    var result = JsonConvert.DeserializeObject<Response<string>>(message);
    return Results.Ok(result);
});



app.Run();
public class Auto
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("modelo")]
    public string Modelo { get; set; }
    [JsonProperty("color")]
    public string Color { get; set; }
}
//Entendible :)
public class Response<T>
{
    [JsonProperty("data")]
    public T Data { get; set; } //Lista return
    [JsonProperty("success")]
    public bool Success { get; set; }
    [JsonProperty("method")]
    public string Method { get; set; }
}