using LiteDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StalkerMUD.Server.Data;
using StalkerMUD.Server.Entities;
using StalkerMUD.Server.Hubs;
using StalkerMUD.Server.Models;
using StalkerMUD.Server.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .MinimumLevel.Verbose()
  .WriteTo.Console()
  .CreateLogger();
builder.Services.AddLogging(x =>
{
    x.AddSerilog();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StalkerMUD API info",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IShopsService, ShopsService>();
builder.Services.AddScoped<IFightParamatersCalculator, FightParamatersCalculator>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("JWT")["Audience"],
        ValidIssuer = builder.Configuration.GetSection("JWT")["Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT")["Secret"]))
    };
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<ILiteDatabase>(_ => new LiteDatabase(@"StalkerMUD.db"));
builder.Services.AddScoped(typeof(IRepository<>), typeof(LiteDbRepository<>));
builder.Services.AddSingleton<IRepository<Item>>(_=> new MemoryRepository<Item>(new List<Item>()
{
    new Item()
    {
        Id = 101,
        Name = "Пистолет",
        Type = StalkerMUD.Common.ItemType.Weapon,
        Damage = 4,
        Rounds = 5,
    },
    new Item()
    {
        Id = 102,
        Name = "Дробовик",
        Type = StalkerMUD.Common.ItemType.Weapon,
        Damage = 12,
        Rounds = 2,
    },
    new Item()
    {
        Id = 201,
        Type = StalkerMUD.Common.ItemType.Suit,
        Name = "Куртка",
        Resistance = 2,
    },
    new Item()
    {
        Id = 202,
        Type = StalkerMUD.Common.ItemType.Suit,
        Name = "Защитный комбинезон",
        Health = 100,
        Resistance = 10,
    },
}));
builder.Services.AddSingleton<IRepository<ShopPoint>>(_ => new MemoryRepository<ShopPoint>(new List<ShopPoint>()
{
    new ShopPoint()
    {
        Id = 1,
        ItemId = 101,
        Cost = 100,
    },
    new ShopPoint()
    {
        Id = 2,
        ItemId = 102,
        Cost = 500,
    },
    new ShopPoint()
    {
        Id = 3,
        ItemId = 201,
        Cost = 100,
    },
    new ShopPoint()
    {
        Id = 4,
        ItemId = 202,
        Cost = 500,
    },
}));
builder.Services.AddSingleton<IRepository<Mob>>(_ => new MemoryRepository<Mob>(new List<Mob>()
{
    new Mob()
    {
        Id = 1,
        Name = "Пёс Сутулый",
        Attributes = new Attributes(),
    }
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<FightHub>("/fight");

app.Run();
