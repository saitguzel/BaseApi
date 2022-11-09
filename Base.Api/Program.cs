using Base.Service.Helpers;
using Base.Service.Interfaces;
using Base.Service.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

DatabaseHelper.ConnectionString = builder.Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Base.Api", Version = "v1" });
});

builder.Services.AddScoped<IDBService, DBService>();

builder.Services.AddMemoryCache();
builder.Services.AddDataProtection();

builder.Services.AddCors(c =>
{
    c.AddPolicy("CorsPolicy", options => options
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});


builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    //options.Providers.Add<CustomCompressionProvider>();
    options.MimeTypes =
    ResponseCompressionDefaults.MimeTypes.Concat(
        new[] {
            // Default
            "text/plain",
            "text/css",
            "application/javascript",
            "text/html",
            "application/xml",
            "text/xml",
            "application/json",
            "text/json",
            // Custom
            "image/svg+xml"
           });
});

builder.WebHost.ConfigureKestrel(a =>
{
    a.Limits.MaxRequestBodySize = long.MaxValue;
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Base.Api v1"));
}

app.UseRouting();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
