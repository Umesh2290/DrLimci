using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using LaboratorySystem;

[assembly: OwinStartup(typeof(LaboratorySystem.Startup))]

namespace LaboratorySystem
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }

    }
}
