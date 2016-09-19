using System;
using System.Configuration;
using System.Dynamic;
using System.Net.Http;
using Simple.Data;
using Xunit;

namespace RunningJournalApi.AcceptanceTests
{
    public class HomeJsonTests
    {
        [Fact]
        [UseDatabase]
        public void GetResponseReturnsCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                Assert.True(response.IsSuccessStatusCode, "actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        [UseDatabase]
        public void GetReturnsJsonContent()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                Assert.Equal(
                    "application/json",
                    response.Content.Headers.ContentType.MediaType);
                var json = response.Content.ReadAsJsonAsync().Result;
                Assert.NotNull(json);
            }
        }

        [Fact]
        [UseDatabase]
        public void PostResponseReturnsCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time = DateTimeOffset.Now,
                    distance = 8500,
                    duration = TimeSpan.FromMinutes(44)
                };

                var response = client.PostAsJsonAsync("", json).Result;

                Assert.True(response.IsSuccessStatusCode, "actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        [UseDatabase]
        public void GetAfterPostResponseReturnsResponseWithPostedEntry()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time = DateTimeOffset.Now,
                    distance = 8100,
                    duration = TimeSpan.FromMinutes(41)
                };
                var expected = json.ToJObject();
                client.PostAsJsonAsync("", json).Wait();

                var response = client.GetAsync("").Result;

                var actual = response.Content.ReadAsJsonAsync().Result;
                Assert.Contains(expected, actual.entries);
            }
        }

        [Theory]
        [UseDatabase]
        [InlineData("foo")]
        [InlineData("baz")]
        [InlineData("bar")]
        public void GetRootReturnsCorrectEntryFromDatabase(string userName)
        {
            dynamic entry = new ExpandoObject();
            entry.time = DateTimeOffset.Now;
            entry.distance = 6000;
            entry.duration = TimeSpan.FromMinutes(31);

            var expected = ((object) entry).ToJObject();

            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);
            var userId = db.User.Insert(UserName: userName).UserId;
            entry.userId = userId;
            db.JournalEntry.Insert(entry);

            using (var client = HttpClientFactory.Create(userName))
            {
                var response = client.GetAsync("").Result;

                var actual = response.Content.ReadAsJsonAsync().Result;
                Assert.Contains(expected, actual.entries);
            }
        }
    }
}
