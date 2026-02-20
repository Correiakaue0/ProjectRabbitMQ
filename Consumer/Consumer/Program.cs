using Consumer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Producer.Arguments.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<RabbitMqService>();

var app = builder.Build();

var service = app.Services.GetRequiredService<RabbitMqService>();

await service.ReadMessages();