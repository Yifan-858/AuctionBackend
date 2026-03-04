using AuctionBackend.Core.Interfaces;
using AuctionBackend.Core.Services;
using AuctionBackend.Data;
using AuctionBackend.Data.Interfaces;
using AuctionBackend.Data.Profiles;
using AuctionBackend.Data.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var jwtConfig = builder.Configuration.GetSection("Jwt");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddCors();

builder.Services.AddAutoMapper(cfg => { }, typeof(UserProfile).Assembly);

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(opt => {
       opt.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = jwtConfig["Issuer"],
           ValidAudience = jwtConfig["Audience"],
           IssuerSigningKey =
      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
       };
   });

builder.Services.AddScoped<IAuctionRepo, AuctionRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IBidRepo, BidRepo>();
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IBidService, BidService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

var origins = builder.Configuration.GetSection("Origins").Get<string[]>();

app.UseRouting();

app.UseCors(options =>
    options.WithOrigins(origins)
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(endpoint =>
{
endpoint.SwaggerEndpoint("/swagger/v1/swagger.json", "My API dok");
});

app.UseEndpoints(endpoints => {  endpoints.MapControllers(); });

app.Run();
