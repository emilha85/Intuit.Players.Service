using Intuit.Players.Service.DIExtensions;
using Intuit.Players.Service.StartupTasks;

namespace Intuit.Players.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddBus();
            builder.Services.AddCache();
            builder.Services.AddComponents();
            builder.Services.AddConfiguration(builder.Configuration);
            builder.Services.AddDal();
            builder.Services.AddStartupTasks();

            builder.Services.AddHostedService<StartupWorker>();

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

        }
    }
}
