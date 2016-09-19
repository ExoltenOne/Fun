using System.Collections.Generic;
using System.Security.Claims;

namespace RunningJournalApi.UnitTests
{
    public class ClaimComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {
            return object.Equals(x.Type, y.Type) && object.Equals(x.Value, y.Value);
        }

        public int GetHashCode(Claim obj)
        {
            return 0;
        }
    }
}