using apiRedis.Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddHttpClient();
var redisConnectionString = builder.Configuration.GetValue<string>("RedisSettings:ConnectString");
builder.Services.AddStackExchangeRedisCache(o =>
{
    o.InstanceName = "cep";
    o.Configuration = redisConnectionString;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ApiCep",
        Version = "v1",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ranyere",
            Email = "ranyereramiresdev@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/ranyere-ramires-076aa1336/")
        }
    });

    var xmlfile = "ApiRedis.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Configure the HTTP request pipeline.

app.Run();
