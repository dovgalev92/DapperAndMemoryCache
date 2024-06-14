using DapperMemoryCache.DbContext;
using DapperMemoryCache.CastomServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddReposService();
//builder.Services.AddMemoryCache(options =>
//{
//    options.SizeLimit = 100;// размер кэша
//    options.CompactionPercentage = 0.1;//сжимаем кэш на 10% когда он достигнет мах значения
//    options.TrackStatistics = true; // вести статистику кэша
//});
builder.Services.AddOutputCache();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseOutputCache();


app.Run();
