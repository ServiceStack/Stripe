
using System;
using System.Collections.Generic;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using ServiceStack.Text;

namespace Stripe.Tests
{
    [TestFixture]
    public class StripeGatewayAccountTests : TestsBase
    {
        [Test]
        public void Can_Create_Account()
        {
            var response = gateway.Post(new CreateStripeAccount
            {
                Country = "AU",
                Email = "test@email.com",
                Managed = true,
            });

            response.PrintDump();
        }
    }
}