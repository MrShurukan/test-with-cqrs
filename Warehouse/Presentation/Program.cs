using Presentation;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices.Configure(builder);

var app = builder.Build();

app.Run();