using SimpleNotes.Configuration;
using SimpleNotes.Database;
using SimpleNotes.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSimpleNotes(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseSimpleNotesEndpoints();

await DbInitializer.InitializeAsync(app);

app.Run();
