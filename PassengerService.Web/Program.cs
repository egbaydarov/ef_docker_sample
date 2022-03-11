using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PassengerService.Web.DbContext;
using PassengerService.Web.Services;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
builder.WebHost.ConfigureKestrel(opts =>
{
    opts.ListenAnyIP(1448, o => o.Protocols = HttpProtocols.Http2);
    opts.ListenAnyIP(1488);
});

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcHttpApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Passengers API",
            Version = "v1",
            Description = "API for Public Transport Aggregator"
        });
    }
);
builder.Services.AddCors(o => o.AddPolicy("AllowAll", b =>
        b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
));
var connectionString = ConnectionService.GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString));
var app = builder.Build();

app.MapGet("/hello", () => "HELLO");
app.UseRouting();

app.UseCors("AllowAll");
            
app.UseSwagger();
app.UseSwaggerUI(endpoints =>
{
    endpoints.SwaggerEndpoint("/swagger/v1/swagger.json", "PassengerServiceAPI");
});

app.MapControllers();
app.MapGrpcService<GreeterService>();

app.Run();