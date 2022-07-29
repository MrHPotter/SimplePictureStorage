var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddPolicy("local",
    policyBuilder =>
    {
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.SetIsOriginAllowed(s => s.Contains("localhost"));
    }));
Directory.CreateDirectory("Storage/Pictures");

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("local");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();