using ApiRestRs.Authentication;

var builder = WebApplication.CreateBuilder(args);
var MyCors = "";

// Add services to the container.
builder.Services.AddCors( options =>
{
    options.AddPolicy("MyCors",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                      
                    });
   
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseCors(MyCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
