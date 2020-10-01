﻿using Mews.Fiscalization.Germany.Model;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Mews.Fiscalization.Germany.Tests
{
    [TestFixture]
    public class TssTests
    {
        private static readonly Guid TssId = new Guid("INSERT_TSS_ID");
        private static readonly ApiKey ApiKey = new ApiKey("INSERT_API_KEY");
        private static readonly ApiSecret ApiSecret = new ApiSecret("INSERT_API_SECRET");

        [Test]
        public async Task CreateTssSucceeds()
        {
            var client = GetClient();
            var accessToken = (await client.GetAccessTokenAsync()).SuccessResult;
            var createdTss = await client.CreateTssAsync(accessToken, TssState.Initialized, description: "Creating a test TSS.");

            AssertTss(createdTss.IsSuccess, createdTss.SuccessResult.Id);
        }

        [Test]
        public async Task GetTssSucceeds()
        {
            var client = GetClient();
            var accessToken = (await client.GetAccessTokenAsync()).SuccessResult;
            var tss = await client.GetTssAsync(accessToken, TssId);

            AssertTss(tss.IsSuccess, tss.SuccessResult.Id);
        }

        private FiskalyClient GetClient()
        {
            return new FiskalyClient(ApiKey, ApiSecret);
        }

        private void AssertTss(bool isSuccess, object value)
        {
            Assert.IsTrue(isSuccess);
            Assert.IsNotNull(value);
        }
    }
}