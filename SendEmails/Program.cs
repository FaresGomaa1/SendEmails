using Microsoft.Extensions.Configuration;
using SendEmails.Service;
using SendEmails.Settings;

namespace SendEmails
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adding IConfiguration
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<MailSettings>(
                configuration.GetSection("MailSettings")
            );
            builder.Services.AddTransient<IMailingService, MailingService>();
            
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("Allownce", crosPolicy =>
                {
                    crosPolicy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("Allownce");

            app.MapControllers();

            app.Run();
        }
    }
}