//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
//using FirstMate;

namespace Daemon
{
    public class Startup
    {
        string localdb = "";

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<FirstMateData>(options => options.UseSqlServer(localdb));
        }

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //}
    }
}