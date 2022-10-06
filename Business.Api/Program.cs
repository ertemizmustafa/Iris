using Iris;
using Iris.Localization;
using Iris.Localization.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddMeLocalization();


AssemblyHelper.SetOptions(x => { x.DomainNames = new string[] { "Business", "Iris" }; });
AssemblyHelper.LoadAssemblies();
AssemblyHelper.SearchinDomain();

builder.Services.AddIrisLocalization(x => { x.ResourcesPath = "Resources"; x.ConnectionString = "lelele"; });

//builder.Services.AddCustomLocalization(x => { x.UseDatabase = true; x.UseJsonFile = true; x.ResourcesPath = "Resources"; });



//builder.Services.AddJsonLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

var supportedCultures = new[] { "tr-TR", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCustomLocalization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
