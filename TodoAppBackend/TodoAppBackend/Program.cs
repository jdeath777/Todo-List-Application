using TodoAppBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddSingleton<ITodoService, TodoService>();

// ----------- CORS -----------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()       // allow all origins
              .AllowAnyHeader()       // allow headers like Content-Type
              .AllowAnyMethod();      // allow GET, POST, DELETE, etc.
    });
});

var app = builder.Build();

// ----------- Middleware -----------

app.UseHttpsRedirection();
app.UseCors(); 

app.UseAuthorization();
app.MapControllers();

app.Run();
