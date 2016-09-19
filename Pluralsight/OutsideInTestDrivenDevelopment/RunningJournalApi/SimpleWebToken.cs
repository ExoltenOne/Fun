using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Simple.Data;

namespace RunningJournalApi
{
    public class SimpleWebToken : IEnumerable<Claim>
    {
        private readonly IEnumerable<Claim> claims;

        public SimpleWebToken(params Claim[] claims)
        {
            this.claims = claims;
        }

        public override string ToString()
        {
            return this.claims.Select(c => c.Type + "=" + c.Value)
                .DefaultIfEmpty(string.Empty)
                .Aggregate((x, y) => x + "&" + y);
        }

        public IEnumerator<Claim> GetEnumerator()
        {
            return this.claims.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public static bool TryParse(string tokenString, out SimpleWebToken token)
        {
            token = null;
            if (tokenString == string.Empty)
            {
                token = new SimpleWebToken();
                return true;
            }

            if (tokenString == null)
            {
                return false;
            }

            var claimPairs = tokenString.Split('&');
            if (!claimPairs.All(x => x.Contains("=")))
            {
                return false;
            }

            var claims = claimPairs
                .Select(s => s.Split('='))
                .Select(a => new Claim(a[0], a[1]));

            token = new SimpleWebToken(claims.ToArray());
            return true;
        }
    }
}