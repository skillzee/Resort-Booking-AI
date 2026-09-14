using OnlineResortBooking.Application.Interfaces;
using OnlineResortBooking.Infrastructure.Repositories;
using OnlineResortBooking.Infrastructure.Inventory;
using OnlineResortBooking.Infrastructure.Pricing;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddControllers();


builder.Services.AddSingleton<IResortRepository, InMemoryResortRepository>();
builder.Services.AddSingleton<IInventoryService, InMemoryInventoryService>();
builder.Services.AddSingleton<IPricingService, PricingService>();




var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

app.MapControllers();


app.Run();


