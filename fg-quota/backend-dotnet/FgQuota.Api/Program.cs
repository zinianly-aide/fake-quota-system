using FgQuota.Api.Data;
using FgQuota.Api.Exceptions;
using FgQuota.Api.Models;
using FgQuota.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var databaseProvider = builder.Configuration.GetValue<string>("Database:Provider") ?? "H2";
if (databaseProvider.Equals("H2", StringComparison.OrdinalIgnoreCase))
{
    var dbName = builder.Configuration.GetValue<string>("Database:Name") ?? "fg-quota-h2";
    builder.Services.AddDbContext<QuotaDbContext>(options =>
        options.UseInMemoryDatabase(dbName));
}
else
{
    builder.Services.AddDbContext<QuotaDbContext>(options =>
        options.UseInMemoryDatabase("fg-quota"));
}

builder.Services.AddScoped<IQuotaService, QuotaService>();
builder.Services.AddScoped<IRdppService, RdppService>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var firstError = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault() ?? "请求参数错误";

        return new BadRequestObjectResult(new ErrorResponse
        {
            Timestamp = DateTime.Now,
            Status = StatusCodes.Status400BadRequest,
            Error = "参数校验失败",
            Message = firstError,
            Path = context.HttpContext.Request.Path.Value
        });
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(handler =>
{
    handler.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        var status = StatusCodes.Status500InternalServerError;
        var error = "系统异常";
        var message = exception?.Message ?? "未知错误";

        if (exception is ApiException apiException)
        {
            status = apiException.Status;
            error = apiException.Error;
            message = apiException.Message;
        }

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json; charset=utf-8";

        await context.Response.WriteAsJsonAsync(new ErrorResponse
        {
            Timestamp = DateTime.Now,
            Status = status,
            Error = error,
            Message = message,
            Path = context.Request.Path.Value
        });
    });
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuotaDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();

public partial class Program
{
}
