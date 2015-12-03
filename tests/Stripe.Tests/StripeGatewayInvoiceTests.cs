// Copyright (c) Service Stack LLC. All Rights Reserved.
// License: https://raw.github.com/ServiceStack/ServiceStack/master/license.txt


using NUnit.Framework;
using ServiceStack.Stripe;
using ServiceStack.Text;

namespace Stripe.Tests
{
    [TestFixture]
    public class StripeGatewayInvoiceTests : TestsBase
    {
        [Test]
        public void Can_Create_and_Pay_CustomerInvoice()
        {
            var customer = CreateCustomer();
            var plan = GetOrCreatePlan();

            var subscription = gateway.Post(new SubscribeStripeCustomer
            {
                CustomerId = customer.Id,
                Plan = plan.Id,
                Quantity = 1,
            });

            subscription = gateway.Post(new SubscribeStripeCustomer
            {
                CustomerId = customer.Id,
                Plan = plan.Id,
                Quantity = 2,
            });

            subscription.PrintDump();

            var stripeInvoice = gateway.Post(new CreateStripeInvoice
            {
                Customer = customer.Id
            });

            stripeInvoice.PrintDump();

            Assert.That(stripeInvoice.Id, Is.Not.Null);
            Assert.That(stripeInvoice.Customer, Is.EqualTo(customer.Id));

            var paidInvoice = gateway.Post(new PayStripeInvoice
            {
                Id = stripeInvoice.Id
            });

            Assert.That(paidInvoice.Id, Is.EqualTo(stripeInvoice.Id));
            Assert.That(paidInvoice.Customer, Is.EqualTo(customer.Id));
            Assert.That(paidInvoice.Paid, Is.True);
        }

        [Test]
        public void Can_Get_All_Invoices()
        {
            var invoices = gateway.Get(new GetStripeInvoices { Count = 20 });

            invoices.PrintDump();
        }

        [Test]
        public void Can_Get_Upcoming_Invoice()
        {
            var customer = CreateCustomer();
            var plan = GetOrCreatePlan();

            var subscription = gateway.Post(new SubscribeStripeCustomer
            {
                CustomerId = customer.Id,
                Plan = plan.Id,
                Quantity = 1,
            });

            var upcomingInvoice = gateway.Get(new GetUpcomingStripeInvoice
            {
                Customer = customer.Id,
            });

            upcomingInvoice.PrintDump();

            Assert.That(upcomingInvoice.Closed, Is.False);
            Assert.That(upcomingInvoice.Paid, Is.False);
            Assert.That(upcomingInvoice.Customer, Is.EqualTo(customer.Id));
            Assert.That(upcomingInvoice.Lines.TotalCount, Is.GreaterThanOrEqualTo(1));
        }
    }
}