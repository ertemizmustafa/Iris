using Iris;
using Iris.Localization;
using Iris.Localization.AspNetCore;

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


builder.Services.AddCustomLocalization(x => { x.UseDatabase = true; x.UseJsonFile = true; x.ResourcesPath = "Resources"; });



//builder.Services.AddJsonLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomLocalization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
