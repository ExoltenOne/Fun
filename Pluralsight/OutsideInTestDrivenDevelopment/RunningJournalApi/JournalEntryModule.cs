using System;
using System.Security.Principal;

namespace RunningJournalApi
{
    public class JournalEntryModule
    {
        public DateTimeOffset Time { get; set; }

        public int Distance { get; set; }

        public TimeSpan Duration { get; set; }
    }
}