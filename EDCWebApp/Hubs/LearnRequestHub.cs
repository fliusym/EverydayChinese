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

        public void TeacherStopped()
        {
            Clients.AllExcept(Context.ConnectionId).teacherStoppedExplicitly();
            using (var db = new EDCWebApp.DAL.EDCLoginUserContext())
            {
                db.RemoveAllHubConnections();
            }
        }

        public IEnumerable<string> GetConnectedStudents()
        {
            using (var db = new EDCWebApp.DAL.EDCLoginUserContext())
            {
                return db.GetConnectedStudents();
            }
        }

        public string IsTeacherLoggedIn()
        {
            using (var db = new EDCWebApp.DAL.EDCLoginUserContext())
            {
                return db.IsTeacherLogged();
            }
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
        
        public override async System.Threading.Tasks.Task OnReconnected()
        {
            try
            {
                using (var db = new EDCWebApp.DAL.EDCLoginUserContext())
                {
                    await db.UpdateUserConnection(Context.ConnectionId, true);
                }
                using (var db = new EDCWebApp.DAL.EDCLoginUserContext())
                {
                    var connections = db.GetAllValidConnections();
                    var isTeacher = Context.User.IsInRole("Teacher");
                    if (isTeacher)
                    {
                        foreach (var c in connections)
                        {
                            Clients.Client(c.HubConnectionID).userReconnected(new HubUser
                            {
                                Name = Context.User.Identity.Name,
                                IsTeacher = true
                            });
                        }
                    }
                    else
                    {
                        foreach (var c in connections)
                        {
                            Clients.Client(c.HubConnectionID).userReconnected(new HubUser
                            {
                                Name = Context.User.Identity.Name,
                                IsTeacher = false
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            await base.OnReconnected();
        }

        public override async System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            try
            {
                using (var db = new EDCWebApp.DAL.EDCLoginUserContext())
                {
                    await db.UpdateUserConnection(Context.ConnectionId, false);
                }
                //send out the disconnect
                using (var db = new EDCWebApp.DAL.EDCLoginUserContext())
                {
                    var isTeacher = Context.User.IsInRole("Teacher");
                    var connections = db.GetAllValidConnections();
                    if (isTeacher)
                    {
                        foreach (var c in connections)
                        {
                            Clients.Client(c.HubConnectionID).userDisconnected(new HubUser
                            {
                                Name = Context.User.Identity.Name,
                                IsTeacher = true
                            },stopCalled);
                        }
                        //if (stopCalled)
                        //{
                        //    db.RemoveAllHubConnections();
                        //}
                    }
                    else
                    {
                        foreach (var c in connections)
                        {
                            Clients.Client(c.HubConnectionID).userDisconnected(new HubUser
                            {
                                Name = Context.User.Identity.Name,
                                IsTeacher = false
                            },stopCalled);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
                throw;
            }
            await base.OnDisconnected(stopCalled);
        } 
    }
}