using BaseShare.Common.Interface.Communication;
using Communication.API.Endpoints;
using Communication.API.Helper;
using Microsoft.Extensions.Options;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiUrlSettings>(builder.Configuration.GetSection("ApiSettings"));

// Register the Refit client for the User API
builder.Services.AddRefitClient<IUserLogAPI>()
    .ConfigureHttpClient((serviceProvider, c) =>
    {
        var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiUrlSettings>>().Value;
        c.BaseAddress = new Uri(apiSettings.LogApiBaseUrl);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapUserLogEndpoints();

app.Run();

