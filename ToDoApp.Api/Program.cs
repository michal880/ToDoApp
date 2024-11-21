using Microsoft.EntityFrameworkCore;
using ToDoApp;
using ToDoApp.Application;
using ToDoApp.Application.AddTask;
using ToDoApp.Consumers.TaskStatusUpdatedEvent;
using ToDoApp.Consumers.UpdateTaskStatusMessage;
using ToDoApp.Domain.Repositories;
using ToDoApp.Infrastructure;
using ToDoApp.Infrastructure.Persistence;
using ToDoApp.Infrastructure.ServiceBus;
using ToDoApp.ServiceBus;
using TaskUpdatedEvent = ToDoApp.Consumers.TaskStatusUpdatedEvent.TaskUpdatedEvent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));


builder.Services.AddTransient<RabbitMqPublisher>();

builder.Services.AddTransient<IConsumer<UpdateTaskStatusMessage>, UpdateTaskStatusMessageConsumer>();
builder.Services.AddTransient<IConsumer<TaskUpdatedEvent>, TaskStatusUpdatedEventConsumer>();
builder.Services.AddHostedService<RabbitMqListenerService>();
builder.Services.AddTransient<IServiceBus, ServiceBus>();

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