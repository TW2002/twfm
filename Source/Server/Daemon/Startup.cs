using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;\
using FirstMate;
using Microsoft.AspNetCore.Builder;

namespace Daemon
{
    public class Startup
    {
        string localdb = "";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FirstMateData>(options => options.UseSqlServer(localdb));
        }

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //}
    }
}