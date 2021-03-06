﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Xunit;

namespace RunningJournalApi.UnitTests
{
    public class SimpleWebTokenUserNameProjectionTests
    {
        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        [InlineData("baz")]
        public void GetUserNameFromProperSimpleWebTokenReturnsCorrectResult(
            string expected)
        {
            // Fixture setup
            var sut = new SimpleWebTokenUserNameProjection();

            var request = new HttpRequestMessage();
            request.Headers.Authorization = 
                new AuthenticationHeaderValue(
                    "Bearer",
                    new SimpleWebToken(new Claim("userName", expected)).ToString());
            // Exercise system
            var actual = sut.GetUserName(request);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void GetUserNameFromNullRequestThrows()
        {
            var sut = new SimpleWebTokenUserNameProjection();
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetUserName(null));
        }

        [Fact]
        public void GetUserNameFromRquestWithoutAuthorizationHeaderReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SimpleWebTokenUserNameProjection();

            var request = new HttpRequestMessage();
            Assert.Null(request.Headers.Authorization);
            // Exercise system
            var actual = sut.GetUserName(request);
            // Verify outcome
            Assert.Null(actual);
            // Teardown
        }

        [Theory]
        [InlineData("Invalid")]
        [InlineData("Not-Bearer")]
        [InlineData("Bear")]
        [InlineData("Bearer-it-is-not")]
        public void GetUserNameFromRequestWithIncorrectAuthorizationSchemeReturnsCorrectResult(
            string invalidSchema)
        {
            // Fixture setup
            var sut = new SimpleWebTokenUserNameProjection();

            var request = new HttpRequestMessage();
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    invalidSchema,
                    new SimpleWebToken(new Claim("userName", "dummy")).ToString());
            // Exercise system
            var actual = sut.GetUserName(request);
            // Verify outcome
            Assert.Null(actual);
            // Teardown
        }

        [Fact]
        public void GetUserNameFromInvalidSimpleWebTokenReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SimpleWebTokenUserNameProjection();

            var request = new HttpRequestMessage();
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    "invalid token value");
            // Exercise system
            var actual = sut.GetUserName(request);
            // Verify outcome
            Assert.Null(actual);
            // Teardown
        }

        [Fact]
        public void GetUserNameFromSimpleWebTokenWithNoUserNameClaimReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SimpleWebTokenUserNameProjection();

            var request = new HttpRequestMessage();
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    new SimpleWebToken(new Claim("someClaim", "dummy")).ToString());
            // Exercise system
            var actual = sut.GetUserName(request);
            // Verify outcome
            Assert.Null(actual);
            // Teardown
        }
    }
}
