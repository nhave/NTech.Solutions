using NTech.Solutions.Api.Data;

namespace NTech.Solutions.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddRepositories();
            builder.Services.AddServices();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    //options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.SwaggerEndpoint("/openapi/v1.json", "v1");
                    options.RoutePrefix = "swagger";

                    //options.OAuthAppName("NTech.Solutions.Api");

                    options.AddSwaggerBootstrap()
                        .AddExperimentalFeatures();
                });
            }

            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
