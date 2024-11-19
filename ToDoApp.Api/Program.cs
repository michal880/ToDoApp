using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ToDoApp;
using ToDoApp.Application;
using ToDoApp.Application.AddTask;
using ToDoApp.Domain.Repositories;
using ToDoApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();
    config.UsingRabbitMq((context, rabbitConfig) =>
    {
        RabbitMqSettings settings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
        
        rabbitConfig.Host(settings.Host, h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });
        rabbitConfig.ConfigureEndpoints(context);
    });
    
    //config.AddConsumer<TaskUpdatedEventConsumer>();
    config.AddConsumer<UpdateTaskStatusMessageConsumer>();
});
builder.Services.AddTransient<IServiceBus, MassTransitServiceBus>();

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddTaskCommandHandler).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();