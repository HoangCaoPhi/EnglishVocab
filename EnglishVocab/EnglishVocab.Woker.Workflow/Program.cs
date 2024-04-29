using EnglishVocab.Persistence;
using EnglishVocab.Persistence.Contexts;
using EnglishVocab.Woker.Schedule.Consumer;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<ScheduleHostService>();
InjectDI.InjectDbContext(builder.Services, builder.Configuration);


// Masstransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DailyRemindGroupConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "rabbitmq", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("schedule-config", ep =>
        {
            ep.ConfigureConsumer<DailyRemindGroupConsumer>(context);
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
        });
    });
});

// Quartz
builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScheduleContext>();
    db.Database.Migrate();
}

host.Run();
