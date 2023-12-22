using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseCors();

app.Use(async (ctx, next) =>
{
ctx.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:5500";
ctx.Response.Headers["Access-Control-Allow-Credentials"] = "true";
ctx.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With";
ctx.Response.Headers["Access-Control-Allow-Methods"]="GET,POST,PUT,PATCH,DELETE,OPTIONS";
    if (HttpMethods.IsOptions(ctx.Request.Method))
    {
        ctx.Response.Headers["Access-Control-Allow-Headers"] = "*";

        await ctx.Response.CompleteAsync();
        return;
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MapControllers();

app.Run();
