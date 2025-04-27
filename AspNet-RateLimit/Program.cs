using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRateLimiter(RateLimit =>
{
    /*
     here we are using a fixed window rate limiter
    Fixed is name for the rate limiter
    AddFixedWindowLimiter after limit duration you will start from zero 
     */
    RateLimit.AddFixedWindowLimiter("fixed", options =>
    {
        // ???? ??? ????? ????? ???? 
        options.PermitLimit = 5;
        // ????? ??????? ??? ?? ??? ????
        // ????? ? 5 ????? ?? 10 ?????
        options.Window = TimeSpan.FromSeconds(10);
        // ???? ????? ???? ?? ????? ????????
        // ???? ?? ?? ??? ??? 6 ??? ??????? ??? ?????? ?? ???? ?????.
        options.QueueLimit = 0;
    });
    RateLimit.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthorization();

app.MapControllers();

app.Run();
