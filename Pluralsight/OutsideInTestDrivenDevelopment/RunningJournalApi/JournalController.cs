using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        private static readonly List<JournalEntryModule> entries = new List<JournalEntryModule>();

        public HttpResponseMessage Get()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, new JournalModule { Entries = entries.ToArray() });
        }

        public HttpResponseMessage Post(JournalEntryModule journalEntry)
        {
            entries.Add(journalEntry);
            return this.Request.CreateResponse();
        }
    }
}
