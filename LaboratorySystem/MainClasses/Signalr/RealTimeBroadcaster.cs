using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public class RealTimeBroadcaster
    {
        public static void BroadCast(string name, string message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();
            hubContext.Clients.All.NewMessage(name, message);
        }
    }
}