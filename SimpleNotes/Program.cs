using SimpleNotes.Configuration;
using SimpleNotes.Database;
using SimpleNotes.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSimpleNotes(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // toDo: добавить авторизацию в сваггере
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSimpleNotesEndpoints();

await DbInitializer.InitializeAsync(app);

app.Run();
