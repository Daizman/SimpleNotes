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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseNoteEndpoints();

await DbInitializer.InitializeAsync(app);

app.Run();
