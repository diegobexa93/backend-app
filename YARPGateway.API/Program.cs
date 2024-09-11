using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using NetDevPack.Security.JwtExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add YARP services
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

bool.TryParse(builder.Configuration["Logging:Trace:Enable"], out bool enableTrace);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.SaveToken = true; // keep the public key at Cache for 10 min.
    x.IncludeErrorDetails = true; // <- great for debugging
    x.BackchannelHttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; } // Ignore certificate validation for development
    };
    x.SetJwksOptions(new JwkOptions("https://host.docker.internal:8081/jwks",
                                    issuer: "https://host.docker.internal:8081",
                                    audience: "NetDevPack.Security.Jwt.AspNet"));
});



builder.Services.AddHttpClient();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define the security scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

if (enableTrace)
    app.UseMiddleware<RequestLoggingMiddleware>();

// Use YARP
app.MapGet("/", () => "YARP API Gateway is running...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins"); // Use the CORS policy


app.UseAuthentication();
app.UseAuthorization();
//app.UseHttpsRedirection();

app.MapReverseProxy();

app.MapControllers();

app.Run();