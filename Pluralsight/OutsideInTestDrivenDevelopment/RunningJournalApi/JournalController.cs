using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Http;
using Simple.Data;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        private readonly dynamic db;

        public JournalController()
        {
            this.db = CreateDb();
        }

        private dynamic CreateDb()
        {
            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            return Database.OpenConnection(connStr);
        }

        public HttpResponseMessage Get()
        {
            SimpleWebToken swt;
            SimpleWebToken.TryParse(this.Request.Headers.Authorization.Parameter, out swt);
            var userName = swt.Single(c => c.Type == "userName").Value;

            var entries = db.JournalEntry
                .FindAll(db.JournalEntry.User.UserName == userName)
                .ToArray<JournalEntryModel>();
            return this.Request.CreateResponse(HttpStatusCode.OK, new JournalModule { Entries = entries });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            SimpleWebToken swt;
            SimpleWebToken.TryParse(this.Request.Headers.Authorization.Parameter, out swt);
            var userName = swt.Single(c => c.Type == "userName").Value;

            var userId = db.User.Insert(UserName: userName).UserId;
            this.db.JournalEntry.Insert(
                UserId: userId,
                Time: journalEntry.Time,
                Distance: journalEntry.Distance,
                Duration: journalEntry.Duration);

            return this.Request.CreateResponse();
        }
    }
}
