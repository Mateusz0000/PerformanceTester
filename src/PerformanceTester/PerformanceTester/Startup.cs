using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PerformanceTester.MetricClasses;
using Prometheus;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using PerformanceTester.Data;
using Microsoft.EntityFrameworkCore;
using PerformanceTester.Configuration;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace PerformanceTester
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
            services.AddResponseCompression(opt =>
            {
                opt.EnableForHttps = true;
                opt.Providers.Add<BrotliCompressionProvider>();
            });


            services.Configure<AppSettings>(Configuration);

            var appSettings = Configuration.Get<AppSettings>();

            services.AddSingleton<MetricReporter>();
            services.AddSingleton<IRandom, DefaultRandom>();
            services.AddEntityFrameworkSqlServer();
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(appSettings.ConnectionString));
            services.AddScoped<EfDb>();

            //var settings = new TextEncoderSettings(UnicodeRanges.BasicLatin, UnicodeRanges.Katakana, UnicodeRanges.Hiragana);
            //settings.AllowCharacter('\u2014');  // allow EM DASH through
            //services.AddWebEncoders((options) =>
            //{
            //    options.TextEncoderSettings = settings;
            //});

            services.AddMvc();
            services.AddControllers().AddNewtonsoftJson();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "PerformanceTester API",
                    Description = "Aplikcaja powsta³a na potrzeby badañ do pracy magisterskiej.",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();

            //Ustawienie wersji TLS na 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PerformanceTester API v1");
                //c.RoutePrefix = string.Empty;
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            var counter = Metrics.CreateCounter("performance_tester_api", "Counts requests to the PerformanceTester API endpoints", new CounterConfiguration
            {
                LabelNames = new[] { "method", "endpoint" },
            });
            app.Use((context, next) =>
                {
                    counter.WithLabels(context.Request.Method, context.Request.Path).Inc();

                    return next();
                });

            app.UseMetricServer();
            app.UseHttpMetrics();
            app.UseMiddleware<ResponseMetricMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
