using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using mvc_examen_aws.Models;
using mvc_examen_aws.Repositories;
using mvc_examen_aws.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

try
{
    var secretsClient = new AmazonSecretsManagerClient(RegionEndpoint.USEast2);
    var response = await secretsClient.GetSecretValueAsync(new GetSecretValueRequest
    {
        SecretId = "prod/conexion-mysql"
    });
    var secret = JsonSerializer.Deserialize<Dictionary<string, string>>(response.SecretString)!;
    var connectionString = $"server={secret["host"]};port={secret["port"]};database=examen_zapatillas;uid={secret["username"]};pwd={secret["password"]}";
    builder.Services.AddDbContext<ZapatillaContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}
catch
{
    var connectionString = builder.Configuration.GetConnectionString("MySql");
    builder.Services.AddDbContext<ZapatillaContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

builder.Services.AddScoped<ZapatillaRepository>();
builder.Services.AddScoped<ZapatillaService>();

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Profile = null;
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Zapatilla}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
