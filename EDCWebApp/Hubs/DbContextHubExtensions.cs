using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EDCWebApp.DAL;
using Microsoft.AspNet.SignalR.Hubs;
using System.Data.Entity;
using System.Threading.Tasks;
using EDCWebApp.Models;

namespace EDCWebApp.Hubs
{
    public static class DbContextHubExtensions
    {
        public static async Task HandleUserConnections(this EDCLoginUserContext db, HubCallerContext context, string userName, bool isTeacher = false)
        {
            if (db != null && context != null)
            {
                if (isTeacher)
                {
                    var teacher = await db.Teachers.Include(p => p.HubConnections)
                        .SingleOrDefaultAsync(p => p.TeacherName == userName);
                    if (teacher != null)
                    {
                        teacher.HubConnections.Add(new Models.EDCHubConnection()
                        {
                            HubConnectionID = context.ConnectionId,
                            Connected = true,
                            LoginDate = DateTime.Now.Date.ToShortDateString(),
                            LoginTime = DateTime.Now.ToString("hh:mm tt")
                        });
                        await db.SaveChangesToDbAsync();
                    }
                }
                else
                {
                    var student = await db.Students.Include(p => p.HubConnections)
                        .SingleOrDefaultAsync(p => p.StudentName == userName);
                    if (student != null)
                    {
                        student.HubConnections.Add(new Models.EDCHubConnection()
                        {
                            HubConnectionID = context.ConnectionId,
                            Connected = true,
                            LoginDate = DateTime.Now.Date.ToShortDateString(),
                            LoginTime = DateTime.Now.ToString("hh:mm tt")
                        });
                        await db.SaveChangesToDbAsync();
                    }
                }
            }
        }

        public static IList<EDCWebApp.Models.EDCHubConnection> GetAllValidConnections(this EDCLoginUserContext db)
        {
            if (db != null)
            {
                IList<EDCHubConnection> connections = new List<EDCHubConnection>();
                var allConnections = db.HubConnections;
                foreach (var c in allConnections)
                {
                    if (c != null && c.Connected)
                    {
                        var date = c.LoginDate + " " + c.LoginTime;
                        var datetime = DateTime.Parse(date);
                        if (datetime != null)
                        {
                            var now = DateTime.Now;
                            if ((now - datetime).TotalMinutes < 100)
                            {
                                connections.Add(c);
                            }
                        }
                    }
                }
                return connections;
            }
            return null;
        }

        public static async Task UpdateUserConnection(this EDCLoginUserContext db, string connectionId, bool connected)
        {
            if (db != null)
            {
                var connection = await db.HubConnections.FindAsync(connectionId);
                if (connection != null)
                {
                    connection.Connected = connected;
                    db.SetEntityModified<EDCHubConnection>(connection);
                    await db.SaveChangesToDbAsync();
                }
            }
        }

        public static IEnumerable<string> GetConnectedStudents(this EDCLoginUserContext db)
        {
            if (db != null)
            {
                var ret = new List<string>();
                var students = db.Students.Include(p => p.HubConnections);
                foreach (var s in students)
                {
                    foreach (var c in s.HubConnections)
                    {
                        if (c.Connected)
                        {
                            var date = c.LoginDate + " " + c.LoginTime;
                            var datetime = DateTime.Parse(date);
                            var now = DateTime.Now;
                            if ((now - datetime).TotalMinutes < 100)
                            {
                                ret.Add(s.StudentName);
                            }
                        }
                    }
                }

                return ret.Distinct();


            }
            return null;
        }

        public static void RemoveAllHubConnections(this EDCLoginUserContext db)
        {
            if (db != null)
            {
                db.HubConnections.RemoveRange(db.HubConnections.Where(p => p.Connected == true || p.Connected == false));
                db.SaveChangesToDb();
            }
        }

        public static string IsTeacherLogged(this EDCLoginUserContext db)
        {
            if (db != null)
            {
                var teacher = db.Teachers.Where(p => p.HubConnections.Any(x => x.Connected == true)).SingleOrDefault();

                if (teacher != null)
                {
                    return teacher.TeacherName;
                }
            }
            return "";
        }

        public static EDCHubConnection GetStudentHubConnection(this EDCLoginUserContext db, string name)
        {
            if (db != null)
            {
                var student = db.Students.Include(p => p.HubConnections)
                    .Where(p => p.StudentName == name).SingleOrDefault();
                if (student != null)
                {
                    var connections = student.HubConnections;
                    foreach (var c in connections)
                    {
                        if (c.Connected)
                        {
                            return c;
                        }
                    }
                }
            }
            return null;
        }
    }
}