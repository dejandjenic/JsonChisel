using JsonChisel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.UseJsonChisel();

app.UseHttpsRedirection();


app.MapGet("/user", () =>
    new User()
    {
        Email = "john.doe@example.com",
        Id = 1,
        Name = "John Doe",
        Address = new Address()
        {
            City = "Anytown",
            Street = "123 Main St"
        }
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();


public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}