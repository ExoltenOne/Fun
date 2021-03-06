﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using Moq;
using Xunit;

namespace RunningJournalApi.UnitTests
{
    public class JournalControllerTests
    {
        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        [InlineData("baz")]
        public void GetReturnsCorrectResult(string userName)
        {
            // Fixture setup
            var projectionStub = new Mock<IUserNameProjection>();
            var queryStub = new Mock<IJournalEntriesQuery>();
            var cmdDummy = new Mock<IAddJournalEntryCommand>();
            var sut = new JournalController(
                projectionStub.Object,
                queryStub.Object,
                cmdDummy.Object)
            {
                Request = new HttpRequestMessage()
            };
            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());

            projectionStub.Setup(p => p.GetUserName(sut.Request)).Returns(userName);

            var expected = new[]
                 {
                     new JournalEntryModel
                     {
                         Time = new DateTime(2012, 12, 10),
                         Distance = 5780,
                         Duration = TimeSpan.FromMinutes(33)
                     },
                     new JournalEntryModel
                     {
                         Time = new DateTime(2012, 12, 5),
                         Distance = 5760,
                         Duration = TimeSpan.FromMinutes(31)
                     },
                     new JournalEntryModel
                     {
                         Time = new DateTime(2012, 12, 3),
                         Distance = 5780,
                         Duration = TimeSpan.FromMinutes(31)
                     }
                 };
            queryStub.Setup(q => q.GetJournalEntries(userName)).Returns(expected);
            // Exercise system
            var response = sut.Get();
            var actual = response.Content.ReadAsAsync<JournalModel>().Result;
            // Verify outcome
            Assert.True(expected.SequenceEqual(actual.Entries));
            // Teardown
        }

        [Fact]
        public void GetWithoutUserNameReturnsCorrectResponse()
        {
            // Fixture setup
            var projectionStub = new Mock<IUserNameProjection>();
            var queryDummy = new Mock<IJournalEntriesQuery>();
            var cmdDummy = new Mock<IAddJournalEntryCommand>();
            var sut = new JournalController(
                projectionStub.Object,
                queryDummy.Object,
                cmdDummy.Object)
            {
                Request = new HttpRequestMessage()
            };
            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());

            projectionStub
                .Setup(p => p.GetUserName(It.IsAny<HttpRequestMessage>()))
                .Returns((string)null);
            // Exercise system
            var response = sut.Get();
            // Verify outcome
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            // Teardown
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        [InlineData("baz")]
        public void PostInsertsEntry(string userName)
        {
            // Fixture setup
            var projectionStub = new Mock<IUserNameProjection>();
            var queryDummy = new Mock<IJournalEntriesQuery>();
            var cmdMock = new Mock<IAddJournalEntryCommand>();
            var sut = new JournalController(
                projectionStub.Object,
                queryDummy.Object,
                cmdMock.Object)
            {
                Request = new HttpRequestMessage()
            };
            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());

            projectionStub.Setup(p => p.GetUserName(sut.Request)).Returns(userName);
            // Exercise system
            var entry = new JournalEntryModel();
            sut.Post(entry);
            // Verify outcome
            cmdMock.Verify(c => c.AddJournalEntry(entry, userName));
            // Teardown
        }

        [Fact]
        public void PostWithoutUserNameReturnsCorrectResponse()
        {
            // Fixture setup
            var projectionStub = new Mock<IUserNameProjection>();
            var queryDummy = new Mock<IJournalEntriesQuery>();
            var cmdMock = new Mock<IAddJournalEntryCommand>();
            var sut = new JournalController(
                projectionStub.Object,
                queryDummy.Object,
                cmdMock.Object)
            {
                Request = new HttpRequestMessage()
            };
            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());

            projectionStub
                .Setup(p => p.GetUserName(It.IsAny<HttpRequestMessage>()))
                .Returns((string)null);
            // Exercise system
            var dummyEntry = new JournalEntryModel();
            var response = sut.Post(dummyEntry);
            // Verify outcome
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            cmdMock.Verify(
                c => c.AddJournalEntry(It.IsAny<JournalEntryModel>(), It.IsAny<string>()),
                Times.Never());
            // Teardown
        }
    }
}
