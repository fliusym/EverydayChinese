using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace EDCWebApp.Hubs
{
    
    public class LearnRequestHub : Hub
    {
        [Authorize(Roles="Teacher")]
        public void UpdatePosition(BlackBoard board)
        {
            board.LastUpdateBy = Context.ConnectionId;
            Clients.AllExcept(board.LastUpdateBy).draw(board);
        }

        public override async System.Threading.Tasks.Task OnConnected()
        {
            var name = Context.User.Identity.Name;
            var isTeacher = Context.User.IsInRole("Teacher");
            using (var db = new EDCWebApp.DAL.EDCLoginUserContext())
            {
                await db.HandleUserConnections(Context, name, isTeacher);
                var connections = db.GetAllValidConnections();
                if (isTeacher)
                {
                    foreach (var c in connections)
                    {
                        Clients.Client(c.HubConnectionID).userConnected(new HubUser
                        {
                            Name = name,
                            IsTeacher = true
                        });
                    }
                }
                else
                {
                    foreach (var c in connections)
                    {
                        Clients.Client(c.HubConnectionID).userConnected(new HubUser
                        {
                            Name = name,
                            IsTeacher = false
                        });
                    }
                }
            }
            await base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        } 
    }
}