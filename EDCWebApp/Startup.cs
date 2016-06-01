using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(EDCWebApp.Startup))]

namespace EDCWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            //for the hubs need user authentication
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}
