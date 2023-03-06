using API.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(le => le.Level == Serilog.Events.LogEventLevel.Error)
        .WriteTo.File(Path.Combine(builder.Configuration.GetSection("Logging:File").Value, 
        DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ")+  ".txt"), outputTemplate: "{Message}"))
    .CreateLogger();

builder.Logging.AddSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IBrregClient, BrregClient>();
builder.Services.AddTransient<IOrganizationService, OrganizationService>();
builder.Services.AddTransient<IReadCsvService, ReadCsvService>();
builder.Services.AddTransient<IWriteCsvService, WriteCsvService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000", "https://powerofficeasui.azurewebsites.net");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CORSPolicy");

app.UseAuthorization();

app.MapControllers();



app.Run();
