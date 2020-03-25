using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LaboratorySystem
{
    public class RealTimeHub : Hub
    {
        public void BroadCast(string Cl_Name, string Cl_Message)
        {

            Clients.All.NewMessage(Cl_Name, Cl_Message);

        }
    }
}