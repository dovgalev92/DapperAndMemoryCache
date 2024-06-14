using DapperMemoryCache.DbContext;
using DapperMemoryCache.CastomServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddReposService();
//builder.Services.AddMemoryCache(options =>
//{
//    options.SizeLimit = 100;// ������ ����
//    options.CompactionPercentage = 0.1;//������� ��� �� 10% ����� �� ��������� ��� ��������
//    options.TrackStatistics = true; // ����� ���������� ����
//});
builder.Services.AddOutputCache();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseOutputCache();


app.Run();
